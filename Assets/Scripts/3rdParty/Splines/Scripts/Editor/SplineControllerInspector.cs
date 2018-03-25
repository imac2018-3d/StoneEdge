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
/// SplineController Inspector for Defective Spline
/// </summary>

[CustomEditor(typeof(SplineController))]
public class SplineControllerInspector : Editor {
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
	}
}
