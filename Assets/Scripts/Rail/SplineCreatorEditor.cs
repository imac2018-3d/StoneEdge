using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplineCreator))]
public class SplineCreatorInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();                           //Expose all public members
		SplineCreator creator = (SplineCreator)target;                   //Cast the target (from Unity) as a SplineNode
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Update"))
		{
			creator.UpdateSpline();
		}
		if (GUILayout.Button("Remove"))
		{
			creator.RemoveSpline();
		}
		GUILayout.EndHorizontal();
	}
}