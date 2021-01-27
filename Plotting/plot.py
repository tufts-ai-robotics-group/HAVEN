import numpy as np
from matplotlib import pyplot as plt
import json

def main(argv):
	if len(argv) != 2:
		print("usage: python plot.py [filename]")
		return
	with open(argv[1], "r") as data:
		items = json.loads(data.read())["Items"]
		xcoords, zcoords = [], []
		for item in items:
			xcoords.append(item["currPos"]["x"])
			zcoords.append(item["currPos"]["z"])
		plt.title("Path Trajectory")
		plt.xlabel("x coordinate")
		plt.ylabel("z coordinate")
		plt.scatter(xcoords, zcoords)
		plt.show()	

if __name__ == "__main__":
	main(sys.argv) 