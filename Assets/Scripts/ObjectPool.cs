using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPoolDictionary = System.Collections.Generic.Dictionary<ObjectPoolDefinition, System.Collections.Generic.List<UnityEngine.GameObject>>;

[System.Serializable]
public class ObjectPoolDefinition
{
    public bool IsExpandable;
    public GameObject poolObject;
    public GameObject parentObject;
    public int poolSize;

    public ObjectPoolDefinition()
    {
        IsExpandable = true;
        poolObject = null;
        parentObject = null;
        poolSize = 0;
    }
}

// Pool of objects organized by key -> value pair of Object definition -> Instanced Object List.
public class ObjectPool : MonoBehaviour {
    public static ObjectPool Instance;
    public ObjectPoolDictionary pooledObjects;
    public List<ObjectPoolDefinition> itemsToPool;

    // Use this for initialization
    void Awake ()
    {
        Instance = this;
        pooledObjects = new ObjectPoolDictionary();

        for (int i=0; i < itemsToPool.Count; ++i)
        {
            var poolDefinition = itemsToPool[i];
            pooledObjects[poolDefinition] = new List<GameObject>();

            for (int j=0; j < poolDefinition.poolSize; ++j)
            {
                var obj = Instantiate(poolDefinition.poolObject) as GameObject;
                obj.SetActive(false);
                pooledObjects[poolDefinition].Add(obj);

                if (poolDefinition.parentObject != null) {
                    obj.transform.parent = poolDefinition.parentObject.transform;
                }
            }
        }
	}

    public GameObject GetPooledObject(string tag, Vector3 pos = new Vector3(), bool setActive = true)
    {
        GameObject obj = GetInternalPooledObject(tag);
        if (obj != null) {
            obj.transform.SetPositionAndRotation(pos, Quaternion.identity);
            obj.SetActive(setActive);
        }

        return obj;
    }

    private GameObject GetInternalPooledObject(string tag)
    {
        ObjectPoolDefinition poolDefinition = null;
        for (int i = 0; i < itemsToPool.Count; ++i) {
            if (itemsToPool[i].poolObject.tag == tag) {
                poolDefinition = itemsToPool[i];
                break;
            }
        }

        if (poolDefinition != null) {
            var objects = pooledObjects[poolDefinition];
            if (objects != null) {
                for (int i = 0; i < objects.Count; ++i) {
                    if (!objects[i].activeInHierarchy) {
                        return objects[i];
                    }
                }

                if (poolDefinition.IsExpandable) {
                    GameObject obj = Instantiate(poolDefinition.poolObject) as GameObject;
                    pooledObjects[poolDefinition].Add(obj);

                    if (poolDefinition.parentObject != null) {
                        obj.transform.parent = poolDefinition.parentObject.transform;
                    }

                    return obj;
                }
            }
        }

        Debug.LogError("Requested a pooled object [" + tag + "] but could not retrieve it.");
        return null;
    }

    public void ClearPooledObjects (GameObject go)
    {
        ObjectPoolDefinition poolDefinition = null;
        for (int i = 0; i < itemsToPool.Count; ++i)
        {
            if (itemsToPool[i].poolObject.tag == go.tag)
            {
                poolDefinition = itemsToPool[i];
                break;
            }
        }

        if (poolDefinition != null)
        {
            var objects = pooledObjects[poolDefinition];
            if (objects != null)
            {
                for (int i = 0; i < objects.Count; ++i)
                {
                    if (objects[i].activeInHierarchy)
                    {
                        objects[i].SetActive(false);
                    }
                }
            }
        }
    }

    // When active is false the object can be retrieved by another function.  It's a sort of pointless function but we can use it to mark and search the code for pooled items specifically instead of objects that are deactivating normally.
    public void ReturnPooledObject ( GameObject go)
    {
        if (go != null)
            go.SetActive(false);
    }
}
