using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    Rigidbody _RB;
    public float explosionStrength = 10f;
    public float upwardsThrust = 6f;
    public float rocketDrag = 0.1f;
    public float speed = 10;
    private Vector3 forceDirection;

    private float fuseTime;
    private bool launchPressed;
    public bool isDud;
    private bool inAir;
    public bool hasHit;
    private bool thrusterSpent;
    private bool hasExploded;
    
    private AudioSource fuseBurningSound;
    private RagdollActivator _ragdollActivator;
    [SerializeField] private ParticleSystem exhaust;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private AudioSource rocketSwish;
    [SerializeField] private GameObject fuse;
    private Renderer fuseBurn;
    [SerializeField] private GameObject explosionPrefab;
    private CameraShake camShake;

    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
        camShake = FindObjectOfType<CameraShake>();
        fuseBurn = fuse.GetComponent<Renderer>();
        fuseTime = fuseBurn.material.GetFloat("_FuseTime");
        fuseBurningSound = fuse.GetComponent<AudioSource>();
        hasHit = false;
    }

    void FixedUpdate()
    {
        if (launchPressed)
        {

            fuseTime -= Time.deltaTime;
            fuseBurn.material.SetFloat("_FuseTime", fuseTime); //sets the _FuseTime float in my shader to match the launch countdown so the fuse burn effect visually ligns up
            
            if (fuseTime <= 0 && !hasHit) //Once the fuse hits zero the rocket is launched
            {
                inAir = true;
                if(!isDud)
                {
                    _RB.velocity = new Vector3(0, upwardsThrust, -speed); //Rocket instantly takes off, upwardsThrust is there to compensate for gravity and curved paths
                    camShake.shakeIntensity = 0.0025f;
                } else
                {
                    camShake.shakeIntensity = 0.01f;
                }
                camShake.isShaking = true;
                _RB.useGravity = true;
                exhaust.Play();
                smoke.Play();
                rocketSwish.Play();
                fuseBurningSound.Stop();
                launchPressed = false; //makes sure you launch the next rocket
            }
        }

        if(inAir)
        {
            if(isDud) //triggers the dud version of the rockets flight path
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

        if(hasHit) //stops the rocket engine once its hit a collider
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
                StartCoroutine("normalExplosion");
                hasExploded = true;
            }
        }

        if (!inAir && hasExploded)
        {
            camShake.isShaking = false;
        }
    }

    public void fireRocket() //Is activated from the corresponding UI button making the setting the fuse on fire starting the countdown for launch
    {
        if(!launchPressed) 
        {
            launchPressed = true;
        }

        fuseBurningSound.Play();
    }

    public void OnCollisionEnter(Collision collision) //References the ragdoll activation script on whatever object is it (if it has one), sets the hasHit bool to true so explosions can trigger
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

    IEnumerator dudExplosion() //The rockets that are duds collide with an object and then fall ot the ground, only to explose two seconds later, the pooled explosion object is set to inactive to be reused.
    {
        inAir = false;
        yield return new WaitForSeconds(2f);
        if (_ragdollActivator != null)
        {
            _ragdollActivator.isHit = true;
            _ragdollActivator.forceDirection = this.transform.position;
            _ragdollActivator.forceStrenght = explosionStrength;
        }
        GameObject explosion = ExplosionPool.instance.GetPooledObject();
        if(explosion != null)
        {
            explosion.transform.position = this.transform.position;
            explosion.SetActive(true);
        }
        this.gameObject.GetComponentInChildren<Renderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        explosion.SetActive(false);
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

    IEnumerator normalExplosion() //triggers the explosion for the normal rockets (that aren't duds) and waits a second before setting the pooled explosion object to inactive so it can be reused
    {
        if (_ragdollActivator != null)
        {
            _ragdollActivator.isHit = true;
            _ragdollActivator.forceDirection = this.transform.position;
            _ragdollActivator.forceStrenght = explosionStrength;
        }
        GameObject explosion = ExplosionPool.instance.GetPooledObject();
        if (explosion != null)
        {
            explosion.transform.position = this.transform.position;
            explosion.SetActive(true);
        }
        inAir = false;
        this.gameObject.GetComponentInChildren<Renderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        explosion.SetActive(false);
        Destroy(gameObject);
    }
}
