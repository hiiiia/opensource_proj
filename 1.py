# from tensorflow.python.framework import convert_to_constants

# @tf.function(input_signature=[tf.TensorSpec(shape=[22], dtype=tf.float32)])

# def to_save(x):
#     return model(x)
# f = to_save.get_concrete_function()

# constantGraph = convert_to_constants.convert_variables_to_constants_v2(f)
# tf.io.write_graph(constantGraph.graph.as_graph_def(), <output_dir>, <output_file>)


import tensorflow as tf
import cv2
import numpy as np
import socket
import time

new_model = tf.keras.models.load_model('./model.h5',compile=False)

capture = cv2.VideoCapture(0)
capture.set(cv2.CAP_PROP_FRAME_WIDTH, 480)
capture.set(cv2.CAP_PROP_FRAME_HEIGHT, 680)
host, port = "127.0.0.1", 25001
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((host, port))


while cv2.waitKey(33) < 0:
    ret, frame_1 = capture.read()

    frame = cv2.resize(frame_1, dsize=(224, 224), interpolation=cv2.INTER_AREA)
    frame = np.array(frame)
    frame = frame.reshape(-1,150528) / 255
    frame = frame.astype('float32')
    a = str(np.argmax(new_model.predict(frame),axis=1))
    sock.sendall(a.encode("UTF-8"))
    #cv2.putText(frame_1, a ,(50,50),cv2.FONT_HERSHEY_SIMPLEX,1,color=(0,255,0))
    #cv2.imshow("VideoFrame", frame_1)
capture.release()
cv2.destroyAllWindows()





