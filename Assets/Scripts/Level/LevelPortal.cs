using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Takes the player up and down sorting layers
// Lower <---> Upper

public enum LEVEL_PORTAL_DIRECTION {
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class LevelPortal : MonoBehaviour {
    public LEVEL_PORTAL_DIRECTION upperLevelDirection;
    private Collider2D portal;
    private int lowerLayer;
    private int upperLayer;
    private int layerOffset;
    private int lowerSortingLayerID;
    private int upperSortingLayerID;

    // Use this for initialization
    void Awake () {
        lowerLayer = LayerMask.NameToLayer("tiles");
        upperLayer = LayerMask.NameToLayer("UpperTiles");
        layerOffset = upperLayer - lowerLayer;
        lowerSortingLayerID = SortingLayer.NameToID("Layer01");
        upperSortingLayerID = SortingLayer.NameToID("UpperLayer01");
        portal = GetComponent<Collider2D>();
	}

    // Leaving the portal will resolve which level Jim is on.
    private void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("HIT");
        bool isUpper = false;

        if (collider.gameObject.tag == "Player") {
            switch (upperLevelDirection) {
                case LEVEL_PORTAL_DIRECTION.UP:
                    isUpper = collider.bounds.center.y > portal.bounds.center.y;
                    break;
                case LEVEL_PORTAL_DIRECTION.DOWN:
                    isUpper = collider.bounds.center.y < portal.bounds.center.y;
                    break;
                case LEVEL_PORTAL_DIRECTION.LEFT:
                    isUpper = collider.bounds.center.x < portal.bounds.center.x;
                    break;
                case LEVEL_PORTAL_DIRECTION.RIGHT:
                    isUpper = collider.bounds.center.x > portal.bounds.center.x;
                    break;
            }

            var renderer = collider.gameObject.GetComponent<Renderer>();

            if (renderer != null) {
                // Moving to the upper level.  Modulo to add or remove the layering offsets without needing to know which layer they were on.
                if (isUpper) {
                    renderer.sortingLayerID = upperSortingLayerID;

                    var result2 = collider.gameObject.layer % layerOffset;
                    collider.gameObject.layer = result2 + layerOffset;
                } else {
                    renderer.sortingLayerID = lowerSortingLayerID;

                    var result2 = collider.gameObject.layer % layerOffset;
                    collider.gameObject.layer = result2;
                }
            }
        }
    }
}
