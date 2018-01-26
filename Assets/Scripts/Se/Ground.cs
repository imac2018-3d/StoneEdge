using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Se {
	public enum GroundType {
		Rock, Grass, Sand, Snow, Glass,
	}
	[RequireComponent(typeof(Collider))]
    public class Ground : MonoBehaviour {
		public GroundType Type;
    }
}
