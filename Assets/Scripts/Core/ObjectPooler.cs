using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public GameObject PooledObject;

    public int PooledAmount = 0;

    List<GameObject> PooledObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < PooledAmount; ++i) {
            GameObject newPooledObject = Instantiate(PooledObject);
            newPooledObject.SetActive(false);
            PooledObjects.Add(newPooledObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPooledObject() {
        for(int i = 0; i < PooledObjects.Count; ++i) {
            if(!PooledObjects[i].gameObject.activeInHierarchy) {
                return PooledObjects[i];
            }
        }
        return null;
    }
}
