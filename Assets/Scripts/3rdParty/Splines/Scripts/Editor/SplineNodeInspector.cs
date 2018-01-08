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
/// SplineNode Inspector for Defective Spline
/// </summary>

[CustomEditor(typeof(SplineNode)), CanEditMultipleObjects]
public class SplineNodeInspector : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();														//Expose all public members
		SplineNode node = target as SplineNode;                   //Cast the target (from Unity) as a SplineNode
		if (node != null)
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Add Prev"))
			{                     //Create a new node before this one
				Selection.activeObject = node.AddPrev();

			}
			if (GUILayout.Button("Add Next"))
			{
				Selection.activeObject = node.AddNext();
			}
			GUILayout.EndHorizontal();
		}
	}
}