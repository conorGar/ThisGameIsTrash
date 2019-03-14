using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Little helper behavior to make Ex's slime spawning random but a little more controlled.
public class SlimeSpot : MonoBehaviour {
    public SlimeStateController slime;

    // Monitor the slime and if it's become unactive, remove reference to it.
    private void Update()
    {
        if (slime != null && !slime.gameObject.activeInHierarchy)
            slime = null;
    }
}
