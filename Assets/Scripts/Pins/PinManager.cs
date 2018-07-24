using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinManager : MonoBehaviour {
    public PinConfig pinConfig;
    public Dictionary<PIN, PinDefinition> pinLookUp = new Dictionary<PIN, PinDefinition>();

	// Use this for initialization
	void Start () {

        // populate the pin look up table.
        for (int i = 0; i < pinConfig.pinList.Count; ++i)
        {
            Debug.Log(pinConfig.pinList[i].displayName);
            pinLookUp[pinConfig.pinList[i].Type] = pinConfig.pinList[i];
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
