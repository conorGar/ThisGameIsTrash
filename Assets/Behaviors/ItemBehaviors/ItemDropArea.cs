using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Special definable area where special items are allowed to drop.
// Useful for making sure items don't end up in places the player can't pick them up.
[RequireComponent(typeof(BoxCollider2D))]
public class ItemDropArea : MonoBehaviour {
    BoxCollider2D boundary;

    void Awake()
    {
        boundary = GetComponent<BoxCollider2D>();
    }

    // clamps the drop position to something within or on the boundary lines.
    public Vector3 GetDropPosition(Vector3 estimatePositon)
    {
        return new Vector3(Mathf.Clamp(estimatePositon.x, boundary.bounds.min.x, boundary.bounds.max.x),
                           Mathf.Clamp(estimatePositon.y, boundary.bounds.min.y, boundary.bounds.max.y),
                           estimatePositon.z);
    }
}
