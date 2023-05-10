using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Projectile : MonoBehaviour
{
    public float speed = 10;
    Rigidbody _RB;
    private float fuseTime;
    private bool launchPressed;
    public bool isDud;
    public bool inAir;
    public bool slowMoOn;
    public bool hasHit;
    private bool thrusterSpent;
    public AudioSource fuseBurningSound;
    public RagdollActivator _ragdollActivator;
    public ParticleSystem exhaust;
    public ParticleSystem smoke;
    public AudioSource rocketSwish;
    public Transform target;
    public GameObject fuse;
    private Renderer fuseBurn;
    public float explosionStrength = 10f;
    public float upwardsThrust = 6f;
    public float rocketDrag = 0.1f;
    private Vector3 forceDirection;
    private GameObject rocket;
    private bool hasExploded;
    public GameObject explosionPrefab;


    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
        fuseBurn = fuse.GetComponent<Renderer>();
        fuseTime = fuseBurn.material.GetFloat("_FuseTime");
        fuseBurningSound = fuse.GetComponent<AudioSource>();
        rocket = transform.GetChild(0).gameObject;
        hasHit = false;
    }

    void FixedUpdate()
    {
        if (launchPressed)
        {

            fuseTime -= Time.deltaTime;
            fuseBurn.material.SetFloat("_FuseTime", fuseTime);
            
            if (fuseTime <= 0 && !hasHit)
            {
                inAir = true;
                if(!isDud)
                {
                    _RB.velocity = new Vector3(0, upwardsThrust, -speed);
                }
                _RB.useGravity = true;
                exhaust.Play();
                smoke.Play();
                rocketSwish.Play();

                launchPressed = false;
                fuseBurningSound.Stop();
            }
        }

        if(inAir)
        {
            if(isDud)
            {
                StartCoroutine("dudForce");
            }
            var rocketRotation = Quaternion.LookRotation(_RB.velocity, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rocketRotation, 5f * Time.fixedDeltaTime);
            if(_RB.velocity.y <= 0)
            {
                _RB.drag = 0f;
            } else
            {
                _RB.drag = rocketDrag;
            }
        }
        else
        {
            StopCoroutine("dudForce");
        }

        if(hasHit)
        {
            exhaust.Stop();
            smoke.Stop();
            rocketSwish.Stop();

            if (isDud && !hasExploded)
            {
                StartCoroutine("dudExplosion");
                hasExploded = true;
            } else if (!hasExploded)
            {
                if(_ragdollActivator != null)
                {
                    _ragdollActivator.isHit = true;
                    _ragdollActivator.forceDirection = this.transform.position;
                    _ragdollActivator.forceStrenght = explosionStrength;
                }
                Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
                inAir = false;
                Destroy(gameObject);
                hasExploded = true;
            }
        }
    }

    public void fireRocket()
    {
        if(!launchPressed) 
        {
            launchPressed = true;
        }

        fuseBurningSound.Play();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        if(collision.collider.tag == "Target")
        {
            hasHit = true;
            _ragdollActivator = collision.gameObject.GetComponent<RagdollActivator>();
        } else
        {
            hasHit = true;
            _ragdollActivator = null;
        }
    }

    IEnumerator dudExplosion()
    {
        inAir = false;
        yield return new WaitForSeconds(2f);
        if (_ragdollActivator != null)
        {
            _ragdollActivator.isHit = true;
            _ragdollActivator.forceDirection = this.transform.position;
            _ragdollActivator.forceStrenght = explosionStrength;
        }
        Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator dudForce() //Special two stage force-based rocket, much slower moving, feels heavier, enable with isDud bool.
    {
        if(!thrusterSpent)
        {
            forceDirection = new Vector3(0, upwardsThrust * 2, 0);
            _RB.AddForce(forceDirection * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
        yield return new WaitForSeconds(2f);
        thrusterSpent = true;
        forceDirection = new Vector3(0, upwardsThrust, -speed);
        _RB.AddForce(forceDirection * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
}
