using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pcristo : MonoBehaviour {

    /// <summary>
    /// Time before projectiles will be destroyed.
    /// </summary>
    [Tooltip("Time before projectile will be destroyed.")]
    public float TimeToLive = 4.0f;

    // Use this for initialization
    void Start () {
        Invoke("Explode", TimeToLive);
	}

    void Explode()
    {
        // TODO Implémenter les explosions
        Destroy(this.gameObject);
    }
}
