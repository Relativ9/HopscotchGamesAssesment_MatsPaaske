using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PrefabSpawner : MonoBehaviour
{

    public GameObject[] rocketSpots;
    public Button LaunchYellow;
    public Button newRocket;
    public GameObject yellowRocketPrefab;
    public GameObject yellowInstance;
    public GameObject blueRocketPrefab;
    public GameObject redRocketPrefab;
    // Start is called before the first frame update
    void Start()
    {
        newRocket.onClick.AddListener(respawnRocketOne);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void respawnRocketOne()
    {
        yellowInstance = Instantiate(yellowRocketPrefab, rocketSpots[0].transform.position, yellowRocketPrefab.transform.rotation);
        //LaunchYellow.onClick.AddListener(linkButtonToPrefabYellow);
    }

    public void linkButtonToPrefabYellow()
    {
        yellowInstance.GetComponent<Projectile>().fireRocket();
    }
}