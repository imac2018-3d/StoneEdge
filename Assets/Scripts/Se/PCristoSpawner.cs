using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PCristoSpawner : MonoBehaviour {

    /// <summary>
    /// Target toward projectiles are thrown.
    /// </summary>
    [Tooltip("Target toward projectiles are thrown.")]
    public Transform Target;

    /// <summary>
    /// Object of projectiles thrown.
    /// </summary>
    [Tooltip("Projectile thrown")]
    public Pcristo ProjectilePrefab;

    /// <summary>
    /// These two values are used to make the parabola trajectory of thrown bullets. There are default values that works, changing them can lead to miss the target.
    /// </summary>
    [Tooltip("Two values to make the parabola hit the Target. 7.4 and 0.68 works, but you can test and found other values for other types of parabolas.")]
    public float UpForceMultiplier = 7.4f, FrontForceMultiplier = 0.68f;
    //public const float UpForceMultiplier = 7.4f;         // These are "magic" values found by testing and adjusting...
    //public const float FrontForceMultiplier = 0.68f;    // They make the parabola of projectiles to reach the target exactly

    /// <summary>
    /// Position to spawn projectiles. If null, projectiles spawn at spawner location.
    /// </summary>
    [Tooltip("[Optional] Position where spawn projectiles instead of Spawner's position.")]
    public Transform SpawnPosition;

    /// <summary>
    /// Parent object where all projectiles will be instancied. If undefined, projectiles will be instancied at Root.
    /// </summary>
    [Tooltip("[Optional] Parent Gameobject where projectiles will be instancied.")]
    public Transform ProjectilePoolTransform;

    void Start () {
        // Ça c'est pour du test
        //InvokeRepeating("Fire", 0f, 1f);
    }

    /// <summary>
    /// Trigger the fire : instanciate a Projectile to be thrown at Target.
    /// If there is no Target defined, throws a InvalidOperationException
    /// </summary>
    void Fire () {
        Assert.IsNotNull(Target, "PCristoSpawner must have Target defined to Fire.");
        Assert.IsNotNull(ProjectilePrefab, "PCristoSpawner must have ProjectilePrefab defined to Fire.");

        Transform effectiveSpawnTransform;
        if (SpawnPosition == null) {
            effectiveSpawnTransform = transform;
        } else {
            effectiveSpawnTransform = SpawnPosition;
        }

        var bullet = Instantiate(
            ProjectilePrefab,
            effectiveSpawnTransform.position,
            effectiveSpawnTransform.rotation,
            ProjectilePoolTransform
        );

        // bullet HAS surely a RigidBody
        var bulletRbdy = bullet.GetComponent<Rigidbody>();
        bulletRbdy.velocity = (Target.position - effectiveSpawnTransform.position) * FrontForceMultiplier * bulletRbdy.mass + transform.up * UpForceMultiplier;
    }
}
