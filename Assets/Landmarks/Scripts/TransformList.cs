﻿/*
    Copyright (C) 2010  Jason Laczko

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class TransformList : ExperimentTask {

	public GameObject parentObject;
	public int current = 0;

	public static List<GameObject> objects;
	public EndListMode EndListBehavior; 
	public bool shuffle;
	public GameObject order;

	public override void startTask () {
		//ViewObject.startObjects.current = 0;
		//current = 0;
		GameObject[] objs;

		objs = new GameObject[parentObject.transform.childCount];
		Array.Sort(objs);
		int i = 0;
		foreach (Transform child in parentObject.transform) {
			objs[i] = child.gameObject;
			i++;
		}



		if (order ) {
			// Deal with specific ordering
			TransformOrder ordered = order.GetComponent("TransformOrder") as TransformOrder;

			if (ordered) {
				Debug.Log("ordered");
				Debug.Log(ordered.order.Count);

				if (ordered.order.Count > 0) {
					objs = ordered.order.ToArray();
				}
			}
		}

		if ( shuffle ) {
			Experiment.Shuffle(objs);			

		}

		TASK_START();



		foreach (GameObject obj in objs) {	             
			objects.Add(obj);
			log.log("TASK_ADD	" + name  + "\t" + this.GetType().Name + "\t" + obj.name  + "\t" + "null",1 );
		}

	}	

	public override void TASK_ADD(GameObject go, string txt) {
		objects.Add(go);
	}

	public override void TASK_START()
	{
		base.startTask();		
		if (!manager) Start();

		objects = new List<GameObject>();
	}

	public override bool updateTask () {
		return true;
	}
	public override void endTask() {
		//current = 0;
		TASK_END();
	}

	public override void TASK_END() {
		base.endTask();
	}

	public GameObject currentObject() {
		if (current >= objects.Count) {
			return null;
			current = 0;
		} else {
			return objects[current];
		}
	}

	public  void incrementCurrent() {
		current++;
		if (current >= objects.Count && EndListBehavior == EndListMode.Loop) {
			current = 0;
		}
	}
}
