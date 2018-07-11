using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPoolDictionary = System.Collections.Generic.Dictionary<ObjectPoolDefinition, System.Collections.Generic.List<UnityEngine.GameObject>>;

[System.Serializable]
public class ObjectPoolDefinition
{
    public bool IsExpandable;
    public GameObject poolObject;
    public int poolSize;
}

// Pool of objects organized by key -> value pair of Object definition -> Instanced Object List.
public class ObjectPool : MonoBehaviour {
    public static ObjectPool Instance;
    public ObjectPoolDictionary pooledObjects;
    public List<ObjectPoolDefinition> itemsToPool;

    // Use this for initialization
    void Start ()
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
            }
        }
	}
	
    public GameObject GetPooledObject (string tag)
    {
        ObjectPoolDefinition poolDefinition = null;
        for (int i=0; i < itemsToPool.Count; ++i)
        {
            if (itemsToPool[i].poolObject.tag == tag)
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
                    if (!objects[i].activeInHierarchy)
                    {
                        objects[i].SetActive(true);
                        return objects[i];
                    }
                }

                if (poolDefinition.IsExpandable)
                {
                    GameObject obj = Instantiate(poolDefinition.poolObject) as GameObject;
                    obj.SetActive(true);
                    pooledObjects[poolDefinition].Add(obj);
                    return obj;
                }
            }
        }

        Debug.Log("Requested a pooled object [" + tag + "] but could not retrieve it.");
        return null;
    }

    public GameObject GetPooledObject (string tag, Vector3 pos)
    {
        GameObject go = null;
        if (go = GetPooledObject(tag))
        {
            go.transform.SetPositionAndRotation(pos, Quaternion.identity);
        }

        return go;
    }

    // When active is false the object can be retrieved by another function.  It's a sort of pointless function but we can use it to mark and search the code for pooled items specifically instead of objects that are deactivating normally.
    public void ReturnPooledObject ( GameObject go)
    {
        go.SetActive(false);
    }
}
