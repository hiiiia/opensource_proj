#coding:utf-8
import sys,os
sys.path.append('../')
import tensorflow as tf
import numpy as np
import argparse
import h5py
from network_mobilenet import MobilenetNetwork
from keras.preprocessing.image import load_img, img_to_array
from PIL import Image
from keras.applications.mobilenet import DepthwiseConv2D
from keras import backend as K
from keras.models import Model
from keras.layers import Input, Conv2D, MaxPooling2D, concatenate, BatchNormalization, Activation
from keras.regularizers import l2

config = tf.ConfigProto()

parser = argparse.ArgumentParser(description='Tensorflow Openpose Inference')
# parser.add_argument('--imgpath', type=str, default='./images/person1.jpg')
parser.add_argument('--input-width', type=int, default=368)
parser.add_argument('--input-height', type=int, default=368)
args = parser.parse_args()

input_node = tf.placeholder(tf.float32, shape=(1, args.input_height, args.input_width, 3), name='image')

global_layers = []

def get_variables(model_path, height , width):
    input_node = tf.placeholder(tf.float32, shape=(1, height, width, 3), name='image')

    net = MobilenetNetwork({'image': input_node}, trainable=False, conv_width=0.75, conv_width2=0.50)
    saver = tf.train.Saver(max_to_keep=100)
    config = tf.ConfigProto(allow_soft_placement=True, log_device_placement=False)
    with tf.Session(config=config) as sess:

        saver.restore(sess, model_path)
        variables = tf.global_variables()
        variables = [(v.name, v.eval(session=sess).copy(order='C')) for v in variables]
    return variables

# Load Trained Weights
tf_model_path = './models/model-388003' # includes model-388003.index, model-388003.meta, model-388003.data-00000-of-00001
# tf_model_path = './models/model_final-365221' # includes model-388003.index, model-388003.meta, model-388003.data-00000-of-00001
variables = get_variables(tf_model_path, args.input_height, args.input_width)

def getTupleLayer(prefix,name):

    if name == "Conv2d_0":
        conv2d = (name,  prefix + "/" + name + "/weights:0")
        waits = []
        waits.append(prefix + "/" + name + "/BatchNorm/beta:0")
        waits.append(prefix + "/" + name + "/BatchNorm/moving_mean:0")
        waits.append(prefix + "/" + name + "/BatchNorm/moving_variance:0")
        wait = (name + "_bn", waits)
        layers = [conv2d,wait]
    else:
        sepConv2d = (name + "_depthwise",  prefix + "/" + name + "_depthwise/depthwise_weights:0")
        conv2d = (name + "_pointwise", prefix + "/" + name + "_pointwise/weights:0")

        waits = []
        waits.append(prefix + "/" + name + "_pointwise/BatchNorm/beta:0")
        waits.append(prefix + "/" + name + "_pointwise/BatchNorm/moving_mean:0")
        waits.append(prefix + "/" + name + "_pointwise/BatchNorm/moving_variance:0")
        wait = (name + "_bn" , waits)
        
        layers = [sepConv2d,conv2d,wait]
    return layers

def setLayer(model,layers):
    global variables
    vnames = [name for name, v in variables]

    for ln in layers:
        layer = model.get_layer(name=ln[0])
        layer_weights = layer.get_weights()
        print("ln: ", ln[1])
        wn = []
        if isinstance(ln[1],list):
            # batch_norm
            # gamma
            wn.append(layer_weights[0])

            # 1.beta, 2.moving_mean, 3.variance
            for i, name in enumerate(ln[1]):
                ix = vnames.index(name)
                v = variables[ix][1]
                wn.append(v)
        else:
            ix = vnames.index(ln[1])
            v = variables[ix][1]
            wn.append(v)

            if len(layer_weights) > 1:
                for n in range(1,len(layer_weights)):
                    # pointwise
                    wn.append(layer_weights[n])

        layer.set_weights(wn)

    return model

def separable_conv(x, c_o, kernel,stride, name, relu=True):
    global global_layers
    global_layers.append(name)

    x = DepthwiseConv2D(kernel
        , strides=stride
        , padding='same'
        , use_bias=False
        , depthwise_regularizer=l2(0.00004)
        , name=name+'_depthwise'
    )(x)

    x = Conv2D(c_o,(1,1)
        ,strides=1
        ,use_bias=False
        ,padding='same'
        ,kernel_regularizer=l2(0.004)
        ,name=name+"_pointwise"
    )(x)

    x = BatchNormalization(scale=True, name=name+'_bn')(x,training=False)
    if relu:
        x = Activation('relu', name=name+'_relu')(x)
    
    return x

def get_model(sess, height, width):

    init = tf.global_variables_initializer()
    sess.run(init)

    net = MobilenetNetwork({'image': input_node}
    , trainable=False, conv_width=0.75, conv_width2=0.50)

    K.set_session(sess)
    conv_width=0.75
    conv_width2=0.50
    min_depth = 8

    depth = lambda d: max(int(d * conv_width), min_depth)
    depth2 = lambda d: max(int(d * conv_width2), min_depth)

    image = Input(shape=(width, height, 3),name="image")

    x = Conv2D(depth(32),(3,3)
            , strides=2
            , use_bias=False
            , name="Conv2d_0"
            , trainable = False
            , padding='same'
            , kernel_regularizer=l2(0.04)
            )(image)
    
    x = BatchNormalization(scale=True, name='Conv2d_0_bn')(x,training=False)
    x = Activation('relu', name='Conv2d_0_relu')(x)

    x = separable_conv(x,depth(64),(3,3),1,name='Conv2d_1')
    x = separable_conv(x,depth(128),(3,3),2,name='Conv2d_2')
    o3 = separable_conv(x,depth(128),(3,3),1,name='Conv2d_3')
    x = separable_conv(o3,depth(256),(3,3),2,name='Conv2d_4')
    x = separable_conv(x,depth(256),(3,3),1,name='Conv2d_5')
    x = separable_conv(x,depth(512),(3,3),1,name='Conv2d_6')
    o7 = separable_conv(x,depth(512),(3,3),1,name='Conv2d_7')
    x = separable_conv(o7,depth(512),(3,3),1,name='Conv2d_8')
    x = separable_conv(x,depth(512),(3,3),1,name='Conv2d_9')
    x = separable_conv(x,depth(512),(3,3),1,name='Conv2d_10')
    o11 = separable_conv(x,depth(512),(3,3),1,name='Conv2d_11')
    
    o3_pool = MaxPooling2D((2, 2),(2, 2),padding='same')(o3)
    feat_concat = concatenate([o3_pool,o7,o11], axis=3)

    prefix = 'MConv_Stage1'

    r1 = separable_conv(feat_concat,depth2(128),(3,3),1,name=prefix + '_L1_1')
    r1 = separable_conv(r1,depth2(128),(3,3),1,name=prefix + '_L1_2')
    r1 = separable_conv(r1,depth2(128),(3,3),1,name=prefix + '_L1_3')
    r1 = separable_conv(r1,depth2(512),(1,1),1,name=prefix + '_L1_4')
    r1 = separable_conv(r1,38,(1,1),1,relu=False,name=prefix + '_L1_5')

    # concat = Input(shape=(46, 46, 864))
    r2 = separable_conv(feat_concat,depth2(128),(3,3),1,name=prefix + '_L2_1')
    r2 = separable_conv(r2,depth2(128),(3,3),1,name=prefix + '_L2_2')
    r2 = separable_conv(r2,depth2(128),(3,3),1,name=prefix + '_L2_3')
    r2 = separable_conv(r2,depth2(512),(1,1),1,name=prefix + '_L2_4')
    r2 = separable_conv(r2,19,(1,1),1,relu=False,name=prefix + '_L2_5')
    
    for stage_id in range(5):
        prefix = 'MConv_Stage%d' % (stage_id + 2)
        cc = concatenate([r1,r2,feat_concat], axis=3)

        r1 = separable_conv(cc,depth2(128),(3,3),1,name=prefix + '_L1_1')
        r1 = separable_conv(r1,depth2(128),(3,3),1,name=prefix + '_L1_2')
        r1 = separable_conv(r1,depth2(128),(3,3),1,name=prefix + '_L1_3')
        r1 = separable_conv(r1,depth2(128),(1,1),1,name=prefix + '_L1_4')
        r1 = separable_conv(r1,38,(1,1),1,relu=False,name=prefix + '_L1_5')
        
        r2 = separable_conv(cc,depth2(128),(3,3),1,name=prefix + '_L2_1')
        r2 = separable_conv(r2,depth2(128),(3,3),1,name=prefix + '_L2_2')
        r2 = separable_conv(r2,depth2(128),(3,3),1,name=prefix + '_L2_3')
        r2 = separable_conv(r2,depth2(128),(1,1),1,name=prefix + '_L2_4')
        r2 = separable_conv(r2,19,(1,1),1,relu=False,name=prefix + '_L2_5')

    out = concatenate([r2, r1],axis=3)
    print(out)
    
    model = Model(image, out)

    layers = getTupleLayer("MobilenetV1","Conv2d_0")
    model = setLayer(model,layers)

    for (i, layer) in enumerate(global_layers):
        # idx = i + 2
        n = layer.split("_")
        n.pop()

        prefix = ""
        if n[0] == "Conv2d":
            prefix = "MobilenetV1"
        if n[0] == "MConv":
            prefix = "Openpose"

        if prefix != "":

            layers = getTupleLayer(prefix,layer)
            model = setLayer(model,layers)
    
    if not os.path.exists("output"):
        os.mkdir("output")
    model.save('output/predict.hd5')

    # plot_model(model, to_file='model_shape.png', show_shapes=True)

    # img = load_img(args.imgpath, target_size=(args.input_width, args.input_height))
    # img = np.expand_dims(img, axis=0)
    # print(img.shape)
    # prediction = model.predict(img)
    # prediction = prediction[0]
    # print("#output")
    # print(prediction.shape)
    # print(prediction[0:1, 0:1, :])
    # print(np.mean(prediction))

    # np.save('output/prediction.npy', prediction, allow_pickle=False)

    return model

def run():
    with tf.Session(config=config) as sess:
        net = get_model(sess, args.input_height, args.input_width)

if __name__ == "__main__":
    run()
