using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityChan;

public class RagdollActivator : MonoBehaviour
{

    //This script activates the ragdoll of the unity-chan prefab once it's hit by a rocket
    [SerializeField] private GameObject charRig;

    public bool isHit;
    public Collider charCollider;
    public Animator charAnim;
    public RandomWind randomWind;

    public Collider[] rigCols;
    public Rigidbody[] rigRbs;

    public Vector3 forceDirection;
    public float forceStrenght;


    void Start()
    {
        charAnim = GetComponent<Animator>();
        charCollider = GetComponent<Collider>();
        randomWind = GetComponent<RandomWind>();

        RagdollComponents();
        RagdollOff();
    }

    void Update()
    {
        if (isHit)
        {
            RagdollOn();
        }
        else
        {
            RagdollOff();
        }
    }

    void RagdollComponents()   //collects all the ragdolls and colliders the bones so they don't need manual assingment
    {
        rigCols = charRig.GetComponentsInChildren<Collider>();
        rigRbs = charRig.GetComponentsInChildren<Rigidbody>();
    }

    void RagdollOn()
    {
        rigRbs[0].AddExplosionForce(forceStrenght, forceDirection, 4f, 0.6f);
        charAnim.enabled = false;
        randomWind.enabled = false;
        foreach (Collider col in rigCols) //cycles through each collider in the bones and turns on when ragdoll should activate
        {
            col.enabled = true;
        }

        foreach (Rigidbody rbs in rigRbs) //same as above but with rigidbodies, also sets the interpelation and collision detection to better track the movement at high speeds
        {
            rbs.isKinematic = false;
            rbs.interpolation = RigidbodyInterpolation.Interpolate;
            rbs.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        charCollider.enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
    }

    void RagdollOff() //Same as the ragdoll on method but in reverse
    {
        foreach (Collider col in rigCols)
        {
            col.enabled = false;
        }

        foreach (Rigidbody rbs in rigRbs)
        {
            rbs.isKinematic = true;
        }
        randomWind.enabled = true;
        charCollider.enabled = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
        charAnim.enabled = true;
    }
}
