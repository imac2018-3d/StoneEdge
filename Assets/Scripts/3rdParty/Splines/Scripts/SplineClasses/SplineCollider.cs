using UnityEngine;
using System.Collections;

/*
 * http://www.opensource.org/licenses/lgpl-2.1.php
 * Copyright Defective Studios 2011
 */
///<author>Matt Schoen</author>
///<date>5/21/2011</date>
///<email>schoen@defectivestudios.com</email>
/// <summary>
/// Spline Collider Drawer for Defective Spline
/// </summary>

public class SplineCollider : MonoBehaviour {
	public SplineNode node;

	private void OnTriggerEnter(Collider other)
	{
		SplineController character = other.gameObject.GetComponent<SplineController>();
		if (character != null)
		{
			if (character.CurrentSpline == null)
			{
				Spline nextSpline = transform.parent.GetComponent<Spline>();
				SplineNode nextNode = node;
				Vector3 target = (other.transform.position + transform.position) * 0.5f;
				Vector3 position;
				if (character.FindSplineVertex(nextSpline, other.transform.position, transform.position,
																		out nextNode, out position))
				{
					character.Land(position, nextSpline, nextNode);
				}
				else
				{
					Debug.LogWarning("Error in findNextSpline - Couldn't Land");
				}
			}
		}
	}
}
