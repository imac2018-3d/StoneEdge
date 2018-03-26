using UnityEngine;
using UnityEditor;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * Copyright Defective Studios 2011
 */
///<author>Matt Schoen</author>
///<date>5/21/2011</date>
///<email>schoen@defectivestudios.com</email>
/// <summary>
/// Spline Inspector for Defective Spline
/// </summary>

[CustomEditor(typeof(Spline))]
public class SplineInspector : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		Spline spline = (Spline)target;
		if(spline) {
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();
			if (spline.begin) {
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Set Speed")) {
					SplineNode begin = spline.begin;
					do {
						begin.speed = spline.setSpeed;
						begin = begin.next;
					} while(begin && begin != spline.begin);
				}
				if(GUILayout.Button("Toggle nodes")) {
					SplineNode begin = spline.begin;
					spline.nodesOn = !spline.nodesOn;
					do {
						begin.GetComponent<Renderer>().enabled = spline.nodesOn;
						begin = begin.next;
					} while(begin && begin != spline.begin);
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}
	}
}
