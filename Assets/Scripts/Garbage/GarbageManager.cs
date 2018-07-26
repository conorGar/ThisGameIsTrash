using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageManager : MonoBehaviour {
    public static GarbageManager Instance;
    public List<GarbageSpawner> garbageSpawners = new List<GarbageSpawner>();
    public GameObject trashCollectedDisplay;

    void Awake()
    {
        Instance = this;

        // Find all the spawners in the scene.
        // TODO: Done once at the start of the scene, but make sure this isn't too slow.  It's probably just more convienent and less error prone than maintaining a list in the editor manually.
        GarbageSpawner[] garbageSpawnersArr = FindObjectsOfType<GarbageSpawner>();
        for (int i = 0; i < garbageSpawnersArr.Length; ++i)
        {
            garbageSpawners.Add(garbageSpawnersArr[i]);
        }
    }

    void Start () {

	}
	
	void Update () {
		
	}

    public void PopulateGarbage(GARBAGETYPE type, int garbageCount)
    {
        // Cap garbageCount at garbageSpawners.Count so we don't run out of spawners and crash.
        garbageCount = Mathf.Min(garbageCount, garbageSpawners.Count);

        // Shuffle the garbage spawners for some randomness in where the trash spawns;
        var shuffledGarbageSpawners = garbageSpawners;
        shuffledGarbageSpawners.Shuffle();

        GameObject go = null;
        Ev_GenericGarbage evGarbage = null;

        switch (type)
        {
            case GARBAGETYPE.STANDARD:
                StandardGarbage garbage = new StandardGarbage();
                for (int i = 0; i < garbageCount; ++i)
                {
                    // Generate a random type of garbage.
                    garbage.type = StandardGarbage.GarbageByIndex(Random.Range(0, StandardGarbage.Count()));

                    go = ObjectPool.Instance.GetPooledObject("generic_garbage", shuffledGarbageSpawners[i].transform.position);
                    //go.GetComponent<Ev_GenericGarbage>().trashCollectedDisplay = trashCollectedDisplay;
                    evGarbage = go.GetComponent<Ev_GenericGarbage>();
                    evGarbage.garbage = garbage;
					evGarbage.trashCollectedDisplay = trashCollectedDisplay;
                    evGarbage.SetSprite(garbage.GarbageSprite());
                    go.SetActive(true);
                }
                break;
            default:
                break;
        }
    }
}
