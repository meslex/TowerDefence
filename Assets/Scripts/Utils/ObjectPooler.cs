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
    public Stack<GameObject> pooledObjects = new Stack<GameObject>();

    public ObjectPoolItem() { }

    public ObjectPoolItem(GameObject objectToPool, int amountToPool, bool shouldExpand = true)
    {
        this.objectToPool = objectToPool;
        this.amountToPool = amountToPool;
        this.shouldExpand = shouldExpand;
    }
}

/// <summary>
/// Uses simple list inside
/// </summary>
public class ObjectPooler : Singlenton<ObjectPooler>, IObjectPooler
{

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
                item.pooledObjects.Push(obj);
            }
        }
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
                itemsToPool[c].pooledObjects.Push(objectToPool);
                
            }

        }
    }

    public GameObject GetPooledObject(GameObject objectToPool)
    {
        GameObject result;

        for (int c = 0; c < itemsToPool.Count; ++c)
        {
            if (itemsToPool[c].objectToPool.tag == objectToPool.tag)
            {
                if (itemsToPool[c].pooledObjects.Count > 0)
                {
                    result = itemsToPool[c].pooledObjects.Pop();

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

        itemsToPool.Add(new ObjectPoolItem(objectToPool, 1));
        result = (GameObject)Instantiate(itemsToPool[itemsToPool.Count - 1].objectToPool);
        result.transform.SetParent(transform, false);
        result.SetActive(false);
        return result;
    }


}
