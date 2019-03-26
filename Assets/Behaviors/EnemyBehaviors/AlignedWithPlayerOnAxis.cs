using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Same as alignedwithObjectOnAxis except it defaults in the player as the alignedObject.
public class AlignedWithPlayerOnAxis : AlignedWithObjectOnAxis {
	// Use this for initialization
	void Start () {
        alignedObject = PlayerManager.Instance.player;
	}
}
