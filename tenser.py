# import json
# from glob import glob

# exercise_dict = {}
# annot_path_list = glob('./*.json')
# exercise_idx = 0
# for annot_path in annot_path_list:
#     with open(annot_path,'rt', encoding='UTF8') as f:
#         data = json.load(f)['type_info']
#     seq_idx = annot_path.split('/')[-1].split('-')[2][:-5]

#     if data['exercise'] not in exercise_dict:
#         exercise_dict[data['exercise']] = {'exercise_idx': exercise_idx, 'seq_idx': [], 'attr_name': []}
#         exercise_idx += 1
#     if seq_idx not in exercise_dict[data['exercise']]['seq_idx']:
#         exercise_dict[data['exercise']]['seq_idx'].append(seq_idx)

#     for condition in data['conditions']:
#         attr_name = condition['condition']
#         if attr_name not in exercise_dict[data['exercise']]['attr_name']:
#             exercise_dict[data['exercise']]['attr_name'].append(attr_name)

# with open('exercise_dict.json', 'w') as f:
#     json.dump(exercise_dict, f)


# import json
# import numpy as np
# import tensorflow as tf
#
# def get_bbox(str):
#
#     obj = json.loads(str.decode('utf-8'))
#     bbox = obj['bounding_box']
#     return np.array([bbox['x'], bbox['y'], bbox['height'], bbox['width']], dtype='f')
#
# def get_multiple_bboxes(str):
#     return [[get_bbox(x) for x in str]]
#
# raw = tf.placeholder(tf.string, [None])
# [parsed] = tf.py_func(get_multiple_bboxes, [raw], [tf.float32])

import json
import pandas as pd
import os

global body_cord

file_path = "./data"
file_list = os.listdir(file_path)
file_list = [file_json for file_json in file_list if file_json.endswith(".json")]
#print(file_list)
percent = len(file_list)
percent_num=0
for file_json in file_list:
    with open(file_path+'/'+file_json, 'r', encoding='UTF-8') as file:
        data = json.load(file)
        exercies_type = data['type_info']['exercise']
        #print(exercies_type)

        file = "./" + exercies_type + ".csv"
        if not os.path.exists(file):
            body_cord = pd.DataFrame(columns=[

                'Nose_x','Nose_y',\
                'Left Eye_x', 'Left Eye_y',\
                'Right Eye_x','Right Eye_y',\
                'Left Ear_x','Left Ear_y',\
                'Right Ear_x','Right Ear_y',\
                'Left Shoulder_x','Left Shoulder_y',\
                'Right Shoulder_x','Right Shoulder_y',\
                'Left Elbow_x','Left Elbow_y',\
                'Right Elbow_x','Right Elbow_y',\
                'Left Wrist_x','Left Wrist_y',\
                'Right Wrist_x','Right Wrist_y',\
                'Left Hip_x','Left Hip_y',\
                'Right Hip_x','Right Hip_y',\
                'Left Knee_x','Left Knee_y',\
                'Right Knee_x','Right Knee_y',\
                'Left Ankle_x','Left Ankle_y',\
                'Right Ankle_x','Right Ankle_y',\
                'Neck_x','Neck_y',\
                'Left Palm_x','Left Palm_y',\
                'Right Palm_x','Right Palm_y',\
                'Back_x','Back_y',\
                'Waist_x','Waist_y',\
                'Left Foot_x','Left Foot_y',\
                'Right Foot_x','Right Foot_y',\
                'active'], dtype=float)
        else:
            body_cord = pd.read_csv(file, sep=",")

        for exercies_type in data['frames']:
            for exercies_data_view,data in exercies_type.items():
                tmp_list = []

                for x,point_data in data['pts'].items():
                    #print(f'{x}',end=' : ')
                    for t,y in point_data.items():
                        #print(f'{t} {y}',end=' ')
                        tmp_list.append(y)
                    #print("")


                #print(data['active'])
                tmp_list.append(data['active'])
                tmp_df = pd.Series(tmp_list,index=body_cord.columns)
                #body_cord = pd.concat([body_cord,tmp_df],ignore_index=True)
                body_cord = body_cord.append(pd.Series(tmp_list,index=body_cord.columns), ignore_index=True)
        body_cord.to_csv(file,index=False)
    percent_num+=1
    print(f'{percent_num/percent*100}% Done')