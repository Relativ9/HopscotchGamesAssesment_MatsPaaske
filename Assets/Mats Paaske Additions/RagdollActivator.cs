using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityChan;

public class RagdollActivator : MonoBehaviour
{
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

    void RagdollComponents()
    {
        rigCols = charRig.GetComponentsInChildren<Collider>();
        rigRbs = charRig.GetComponentsInChildren<Rigidbody>();
    }

    void RagdollOn()
    {
        rigRbs[0].AddExplosionForce(forceStrenght, forceDirection, 4f, 0.6f);
        charAnim.enabled = false;
        randomWind.enabled = false;
        foreach (Collider col in rigCols)
        {
            col.enabled = true;
        }

        foreach (Rigidbody rbs in rigRbs)
        {
            rbs.isKinematic = false;
            rbs.interpolation = RigidbodyInterpolation.Interpolate;
            rbs.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
        charCollider.enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
    }

    void RagdollOff()
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
