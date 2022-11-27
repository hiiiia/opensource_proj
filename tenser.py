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

file_path = "./data/D08-1-033.json"

with open(file_path, 'r', encoding='UTF-8') as file:
    data = json.load(file)
    exercies_type = data['type_info']['exercise']
    exercies_data_v1= data['frames'][0]
    print(exercies_type)
    for exercies_type in data['frames']:
        for exercies_data_view,data in exercies_type.items():
            for x,point_data in data['pts'].items():
                print(f'{x}',end=' : ')
                for t,y in point_data.items():
                    print(f'{t} {y}',end=' ')
                print("")

            print(data['active'])

