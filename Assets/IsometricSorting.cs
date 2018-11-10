using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricSorting : MonoBehaviour {
    [Serializable]
    public struct SortBuddy
    {
        public Renderer renderer;
        public int offset;
    }

	public bool movingObject;
    public List<SortBuddy> sortBuddies;

	Renderer myRenderer;
    BoxCollider2D boxCollider2D;
    const int SOMELARGEOFFSET = 10000; // To keep everything positive (so less than -1 when the inverse multiplication happens)


	// Use this for initialization
	void Start () {
        myRenderer = gameObject.GetComponent<Renderer>();

        if (myRenderer == null) {
            Debug.Log("Missing Renderer! " + this.GetInstanceID());
        }
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();

        if (!movingObject){
            UpdateSortingOrder();
		}
	}

	// Update is called once per frame
	void Update () {
		if(movingObject){
            UpdateSortingOrder();
		}
	}

    void UpdateSortingOrder()
    {
        int sortingOrder;

        // Get the sorting order from the Y-value of the best component.
        // Try to use the box collider since it's more accurate than the sprite size, typically.
        if (boxCollider2D != null) {
            sortingOrder = GetSortOrder(boxCollider2D);
        } else {
            sortingOrder = GetSortOrder(myRenderer);
        }

        // apply the sorting order.
        myRenderer.sortingOrder = sortingOrder;

        // sort any other gameObjects this one is in charge of sorting.
        for (int i = 0; i < sortBuddies.Count; i++) {
            sortBuddies[i].renderer.sortingOrder = sortingOrder + sortBuddies[i].offset;
        }
    }

    // * Find the actual y position at the "feet" of the gameObject by subtracting 1/2 the size of the sprite.
    // * Multiply the y by 100 to get a more accurate percentage (2 decimal places).
    // * Round everything to an int.
    // * Add some large offset so the SortingOrder is always positive before it's multiplied.
    // * Multiply by -1 so it's projected away from the camera.
    int GetSortOrder(Renderer renderer)
    {
        return (Mathf.RoundToInt((renderer.bounds.min.y) * 100f) + SOMELARGEOFFSET) * -1;
    }

    int GetSortOrder(BoxCollider2D collider2D)
    {
        return (Mathf.RoundToInt(collider2D.bounds.min.y * 100f) + SOMELARGEOFFSET) * -1;
    }
}
