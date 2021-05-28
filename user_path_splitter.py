import pandas 
import json
import sys

def main():
    if len(sys.argv) == 3:
        sys.argv[2] = int(sys.argv[2])
        read_user_paths(sys.argv[1], sys.argv[2])
    elif len(sys.argv) > 3 or len(sys.argv) < 2:
        print("usage: python user_path_splitter.py file_path [last_x_timesteps]")
        return
    else:
        read_user_paths(sys.argv[1])

def read_user_paths(filepath, last_x=0):
    user_paths = {'user_path_1':[], 'user_path_2':[]}
    cells = ['user_path_1', 'user_path_2']
    path_data = pandas.read_excel(filepath)
    for cell in cells:
        for paths in path_data[cell]:
            if isinstance(paths, str):
                if paths[-3:] != '}]}':
                    while paths[-1] != '}':
                        paths = paths[:-1]
                    paths = paths + ']}'
                the_path = json.loads(paths)['Items']
                the_path = the_path[-last_x:]
                userDict = {'T':[], 'G':[], 'X':[], 'Y':[], 'R':[]}
                for timesteps in the_path:
                    userDict['T'].append(timesteps['T'])
                    userDict['G'].append(timesteps['G'])
                    userDict['X'].append(timesteps['X'])
                    userDict['Y'].append(timesteps['Y'])    
                    userDict['R'].append(timesteps['R'])
                    user_paths[cell].append(userDict)
    return user_paths

if __name__ == '__main__':  
    main()          