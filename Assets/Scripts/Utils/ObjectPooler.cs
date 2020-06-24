using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand = true;

    [HideInInspector]
    public List<GameObject> pooledObjects = new List<GameObject>();
}

public class ObjectPooler : Singlenton<ObjectPooler>
{
    //public List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;

    private void Start()
    {
        //Instantiate pool objects and add them to pool
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.transform.SetParent(transform, false);
                obj.SetActive(false);
                item.pooledObjects.Add(obj);
            }
        }
    }

    /// <summary>
    /// Returns Pooled Object based on gameObject
    /// </summary>
    /// <param name="objectToPool"></param>
    /// <returns></returns>
    public GameObject GetPooledObject(GameObject objectToPool)
    {
        GameObject result;

        for (int c = 0; c < itemsToPool.Count; ++c)
        {
            if (itemsToPool[c].objectToPool.tag == objectToPool.tag)
            {
                if(itemsToPool[c].pooledObjects.Count > 0)
                {
                    result = itemsToPool[c].pooledObjects[0];

                    itemsToPool[c].pooledObjects.RemoveAt(0);
                    return result;
                }

                if (itemsToPool[c].shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(objectToPool);
                    obj.transform.SetParent(transform, false);
                    obj.SetActive(false);
                    return obj;

                }
            }

        }
        return null;
    }


    /// <summary>
    /// Returns Pooled Object based on its tag
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public GameObject GetPooledObject(string tag)
    {
        GameObject result;

        for (int c = 0; c < itemsToPool.Count; ++c)
        {
            if (itemsToPool[c].objectToPool.tag == tag)
            {
                if (itemsToPool[c].pooledObjects.Count > 0)
                {
                    result = itemsToPool[c].pooledObjects[0];

                    itemsToPool[c].pooledObjects.RemoveAt(0);
                    return result;
                }

                if (itemsToPool[c].shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(itemsToPool[c].objectToPool);
                    obj.transform.SetParent(transform, false);
                    obj.SetActive(false);
                    return obj;

                }
            }

        }
        return null;
    }

    /// <summary>
    /// Returns Object to pool, so it can be pooled again
    /// </summary>
    /// <param name="objectToPool"></param>
    public void ReturnObjectToPool(GameObject objectToPool)
    {
        for (int c = 0; c < itemsToPool.Count; ++c)
        {
            if (itemsToPool[c].objectToPool.tag == objectToPool.tag)
            {
                itemsToPool[c].pooledObjects.Add(objectToPool);
                
            }

        }
    }
}
