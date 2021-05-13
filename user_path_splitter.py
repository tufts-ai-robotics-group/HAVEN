import pandas 
import json
import sys

def main():
    if len(sys.argv) != 2:
        print("usage: python user_path_splitter.py [file path]")
        return
    read_user_paths(sys.argv[1])

def read_user_paths(filepath):
    user_paths_1 = []
    user_paths_2 = []
    iterations = 0
    path_data = pandas.read_excel(filepath)
    cell_names = ['user_path_1', 'user_path_2']
    for cell in cell_names:
        for paths in path_data[cell]:
            the_path = json.loads(paths)['Items']
            userDict = {'T':[], 'G':[], 'X':[], 'Y':[], 'R':[]}
            for timesteps in the_path:
                userDict['T'].append(timesteps['T'])
                userDict['G'].append(timesteps['G'])
                userDict['X'].append(timesteps['X'])
                userDict['Y'].append(timesteps['Y'])
                userDict['R'].append(timesteps['R'])
            user_paths_1.append(userDict)
            userDict = {'T':[], 'G':[], 'X':[], 'Y':[], 'R':[]}
    return user_paths_1, user_paths_2

if __name__ == '__main__':  
    main()      