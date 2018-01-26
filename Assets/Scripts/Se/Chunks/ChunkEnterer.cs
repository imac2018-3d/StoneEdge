/// <summary>
/// A component for the camera and the hero.
/// They trigger loading of chunks when entering appropriate colliders.
/// </summary>

using UnityEngine;

namespace Se {
    [RequireComponent(typeof(Collider))]
    public class ChunkEnterer : MonoBehaviour {}
}
