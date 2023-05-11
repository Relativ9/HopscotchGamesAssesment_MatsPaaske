using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPool : MonoBehaviour
{

    //Just a super simple object pooler since the project doesn't really need it but figured I should show it off anyway
    public static TargetPool instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 9;

    [SerializeField] private GameObject targetprefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject target = Instantiate(targetprefab);
            target.SetActive(false);
            pooledObjects.Add(target);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
