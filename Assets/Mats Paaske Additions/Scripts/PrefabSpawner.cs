using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PrefabSpawner : MonoBehaviour
{
    //Script to manage all the prefab spawning (and the buttons that trigger it)
    [Header("Unity-Chan")]
    [SerializeField] private Button newTargets;
    [SerializeField] private GameObject[] targetSpots;
    private bool targetsRespawned;
    public bool canRespawn1;
    public bool canRespawn2;
    public bool canRespawn3;
    private RagdollActivator ragdollActivator;

    [Header("Rocket Stuff")]
    [SerializeField] private Button LaunchYellow;
    [SerializeField] private Button LaunchBlue;
    [SerializeField] private Button LaunchRed;
    [SerializeField] private Button newYellow;
    [SerializeField] private Button newBlue;
    [SerializeField] private Button newRed;
    [SerializeField] private GameObject yellowRocketPrefab;
    private GameObject yellowInstance;
    [SerializeField] private GameObject blueRocketPrefab;
    private GameObject blueInstance;
    [SerializeField] private GameObject redRocketPrefab;
    private GameObject redInstance;
    private bool respawnedYellow;
    private bool respawnedBlue;
    private bool respawnedRed;
    [SerializeField] private GameObject[] rocketSpots;
    public TMP_InputField thrustInputYellow;
    public TMP_InputField thrustInputBlue;
    public TMP_InputField thrustInputRed;

    public void Start()
    {
        respawnTargets();
        respawnRocketYellow();
        respawnRocketBlue();
        respawnRocketRed();
    }
    public void Update()
    {
        if (yellowInstance != null && yellowInstance.GetComponent<Projectile>().hasHit)
        {
            canRespawn1 = true;
        }

        if (blueInstance != null && blueInstance.GetComponent<Projectile>().hasHit)
        {
            canRespawn2 = true;
        }

        if (redInstance != null && redInstance.GetComponent<Projectile>().hasHit)
        {
            canRespawn3 = true;
        }

        if (canRespawn1 && canRespawn2 && canRespawn3)
        {
            targetsRespawned = false;
            newTargets.interactable = true;
        }

        if (yellowInstance != null) //lets the player set the upwards thrust of the rockets
        {
            yellowInstance.GetComponent<Projectile>().upwardsThrust = float.Parse(thrustInputYellow.text);
        }

        if (blueInstance != null) //lets the player set the upwards thrust of the rockets
        {
            blueInstance.GetComponent<Projectile>().upwardsThrust = float.Parse(thrustInputBlue.text);
        }

        if (redInstance != null) //lets the player set the upwards thrust of the rockets
        {
            redInstance.GetComponent<Projectile>().upwardsThrust = float.Parse(thrustInputRed.text) * 50;
        }
    }
    public void respawnTargets() //Checks if all the targets (the Unity Chan object) have been hit by a rocket or not, if they have they can be respawned 
    {
        if (targetsRespawned) return;

        GameObject target1 = TargetPool.instance.GetPooledObject();
        if (target1 != null)
        {
            target1.transform.position = targetSpots[0].transform.position;
            target1.SetActive(true);
            canRespawn1 = false;
        }

        GameObject target2 = TargetPool.instance.GetPooledObject();
        if (target2 != null)
        {
            target2.transform.position = targetSpots[1].transform.position;
            target2.SetActive(true);
            canRespawn2 = false;
        }

        GameObject target3 = TargetPool.instance.GetPooledObject();
        if (target3 != null)
        {
            target3.transform.position = targetSpots[2].transform.position;
            target3.SetActive(true);
            canRespawn3 = false;
        }
        targetsRespawned = true;
        newTargets.interactable = false;
    }

    public void respawnRocketYellow() //instantiates the yellow rocket and adds a listener to the launch button for it
    {
        if(!respawnedYellow)
        {

            yellowInstance = Instantiate(yellowRocketPrefab, rocketSpots[0].transform.position, yellowRocketPrefab.transform.rotation);
            LaunchYellow.onClick.AddListener(linkButtonToPrefabYellow);
            LaunchYellow.interactable = true; //after the prefab is instantiated it can now be launched
            newYellow.interactable = false; //can't spawn a new one before this one is used up
            respawnedYellow = true;
        }
    }

    public void respawnRocketBlue() //same as above but for blue
    {
        if(!respawnedBlue)
        {
            blueInstance = Instantiate(blueRocketPrefab, rocketSpots[1].transform.position, blueRocketPrefab.transform.rotation);
            LaunchBlue.onClick.AddListener(linkButtonToPrefabBlue);
            LaunchBlue.interactable = true;
            newBlue.interactable = false;
            respawnedBlue = true;
        }
    }

    public void respawnRocketRed()  //same as above but for red
    {
        if(!respawnedRed)
        {
            redInstance = Instantiate(redRocketPrefab, rocketSpots[2].transform.position, redRocketPrefab.transform.rotation);
            LaunchRed.onClick.AddListener(linkButtonToPrefabRed);
            LaunchRed.interactable = true;
            newRed.interactable = false;
            respawnedRed = true;
        }
    }

    public void linkButtonToPrefabYellow() //Method to fire the yellow rocket (or start the fuse technically) in the prefab spawner class so it can be easily assigned to when the prefab is instantiated
    {
        yellowInstance.GetComponent<Projectile>().fireRocket();
        LaunchYellow.interactable = false; //grays out the launch button after use
        newYellow.interactable = true; //unlocks the respawn button to instantiate new rocket
        respawnedYellow = false;
    }

    public void linkButtonToPrefabBlue() //exact same as above but for the blue rocket
    {
        blueInstance.GetComponent<Projectile>().fireRocket();
        LaunchBlue.interactable = false;
        newBlue.interactable = true;
        respawnedBlue = false;
    }

    public void linkButtonToPrefabRed() //exact same as above but for the red rocket
    {
        redInstance.GetComponent<Projectile>().fireRocket();
        LaunchRed.interactable = false;
        newRed.interactable = true;
        respawnedRed = false;
    }
}
