﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineCreator : MonoBehaviour {

	public float ColliderRadius = 0.7f;
	public int gap = 0;

	void Awake()
	{
		UpdateSpline();
	}

	// Use this for initialization
	public void UpdateSpline () {
		RemoveSpline();
		GameObject splineSystem = new GameObject();
		GameObject splineObject = new GameObject();
		splineSystem.transform.SetParent(transform, true);
		splineSystem.name = "Spline System";
		splineObject.transform.SetParent(splineSystem.transform, false);
		splineObject.name = "Spline";
		Spline currentSpline = splineObject.AddComponent<Spline>();
		currentSpline.colliderRadius = ColliderRadius;

		Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		int i = 0;
		SplineNode currentNode = null;

		while (i < vertices.Length - 4)
		{
			if (currentSpline.Length == 0)
			{
				GameObject nodeObject = new GameObject();
				nodeObject.transform.SetParent(splineObject.transform, false);
				if (currentNode == null)
				{
					currentNode = nodeObject.AddComponent<SplineNode>();
					currentSpline.AddVert(currentNode);
				}
				else
				{
					SplineNode tmp = nodeObject.AddComponent<SplineNode>();
					tmp.transform.position = currentNode.transform.position;
					tmp.up = currentNode.up;
					currentSpline.AddVert(tmp);
					currentNode = tmp.AddNext().GetComponent<SplineNode>();
				}
			}
			else
			{
				currentNode = currentNode.AddNext().GetComponent<SplineNode>();
			}
			currentNode.up = normals[i];
			Vector3 v = new Vector3();
			for(int j = 0; j < 4; ++j)
			{
					v += vertices[i];
					++i;
			}
			v.x *= 0.25f; v.y *= 0.25f; v.z *= 0.25f;
			v.Scale(transform.localScale);
			currentNode.transform.position = v;
			i += gap*4;

			if (currentSpline.Length >= Spline.MAX_SPLINE_LENGTH)
			{
				splineObject = new GameObject();
				splineObject.transform.SetParent(splineSystem.transform, false);
				splineObject.name = "Spline";
				Spline tmp = splineObject.AddComponent<Spline>();
				currentSpline.next = tmp;
				tmp.previous = currentSpline;
				currentSpline = tmp;
				currentSpline.colliderRadius = ColliderRadius;
			}
		}
		splineSystem.transform.rotation = transform.rotation;
		splineSystem.transform.position = transform.position;
		splineSystem.transform.position.Scale(transform.localScale);
		foreach (Spline s in GetComponentsInChildren<Spline>())
		{
			for (i = 0; i < s.length; ++i)
			{
				s[i].updateForward();
			}
		}
	}

	public void RemoveSpline()
	{
		Spline s = GetComponentInChildren<Spline>();
		if (s != null)
		{
			DestroyImmediate(s.transform.parent.gameObject);
		}
	}
}
