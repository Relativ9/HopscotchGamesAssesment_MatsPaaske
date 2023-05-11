using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPool : MonoBehaviour
{

    //Just a super simple object pooler since the project doesn't really need it but figured I should show it off anyway
    public static ExplosionPool instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 2;

    [SerializeField] private GameObject explosionPrefab;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.SetActive(false);
            pooledObjects.Add(explosion);
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
