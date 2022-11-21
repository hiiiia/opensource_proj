# coding:utf-8
import matplotlib as mpl
mpl.use('Agg')
import matplotlib.pyplot as plt
from keras import backend as K

from PIL import Image
import numpy as np
import sys
import os
import cv2
sys.path.append('../')

from common import estimate_pose, CocoPairsRender, read_imgfile, CocoColors, draw_humans
from pose_dataset import CocoPoseLMDB
from keras.models import Model
from keras.models import load_model


test_img_path = "../images/pose.jpg"
input_height = 368
input_width = 368

im = read_imgfile(test_img_path, 368, 368)
s = im.shape
_im = im.reshape(1, s[0], s[1], s[2])

# if os.path.exists("output/predict.hd5"):
#     from keras.applications.mobilenet import DepthwiseConv2D
#     from keras.utils.generic_utils import CustomObjectScope
#     with CustomObjectScope({'DepthwiseConv2D': DepthwiseConv2D}):
#         net = load_model('output/predict.hd5')
# else:
import tensorflow as tf
from tensorToKeras import get_model
config = tf.ConfigProto()
with tf.Session(config=config) as sess:
    net = get_model(sess, input_height, input_width)
    out = net.predict(_im)


heatMat = out[:, :, :, :19]
pafMat = out[:, :, :, 19:]

heatMat, pafMat = heatMat[0], pafMat[0]

#---------------
# Draw Image
#---------------

humans = estimate_pose(heatMat, pafMat)

# im = im[:, :, ::-1]
process_img = CocoPoseLMDB.display_image(im, heatMat, pafMat, as_numpy=True)

# display
image = cv2.imread(test_img_path)
image_h, image_w = image.shape[:2]
image = draw_humans(image, humans)

scale = 480.0 / image_h
newh, neww = 480, int(scale * image_w + 0.5)

image = cv2.resize(image, (neww, newh), interpolation=cv2.INTER_AREA)


convas = np.zeros([480, 640 + neww, 3], dtype=np.uint8)
convas[:, :640] = process_img
convas[:, 640:] = image

pilImg = Image.fromarray(np.uint8(convas))
pilImg.save("result.png")

