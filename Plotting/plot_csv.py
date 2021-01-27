import sys
import numpy as np
from matplotlib import pyplot as plt
import csv

class Snaps():
	def __init__(self, e, px, py, pz, rx, ry, rz, rw):
		self.elapsed = float(e)
		self.posX    = float(px)
		self.posY    = float(py)
		self.posZ    = float(pz)
		self.rotX    = float(rx)
		self.rotY    = float(ry)
		self.rotZ    = float(rz)
		self.rotW    = float(rw)  

def main(argv):
		if len(argv) != 2:
			print("usage: python plot_csv.py [filename]")
			return
		timelapse = []	
		with open(argv[1], "r") as data:
			csv_reader = csv.reader(data, delimiter = ",")
			line_count = 0
			for row in csv_reader:
				if line_count != 0:
					print(str(row))
					timelapse.append(Snaps(row[0], 
						                   row[1], 
						                   row[2],
						                   row[3],
						                   row[4],
						                   row[5],
						                   row[6],
						                   row[7]))
				line_count += 1
		xcoords, zcoords = [], []
		for item in timelapse:
			xcoords.append(item.posX)
			zcoords.append(item.posZ)
		plt.title("Path Trajectory")
		plt.xlabel("x coordinate")
		plt.ylabel("z coordinate")
		plt.scatter(xcoords, zcoords)
		plt.show()	
if __name__ == "__main__":
	main(sys.argv)