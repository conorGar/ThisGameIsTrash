using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
    public static WorldManager Instance;
    public GlobalVariableManager.WORLDS worldNumber = GlobalVariableManager.WORLDS.NONE;
    public int amountTrashHere = 0;

	void Awake () {
        Instance = this;
	}

    private void Start()
    {
        // TODO: Base this on bag type whether they are going for standard, compost, recyclable, etc.
        GarbageManager.Instance.PopulateGarbage(GARBAGETYPE.STANDARD, amountTrashHere);
    }

    void Update () {
		
	}
}
