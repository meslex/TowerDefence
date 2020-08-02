using UnityEngine;

interface IObjectPooler
{
    GameObject GetPooledObject(GameObject objectToPool);
    //GameObject GetPooledObject(string tag);
    void ReturnObjectToPool(GameObject objectToPool);
}
