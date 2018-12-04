using UnityEngine;
using System.Collections;

public class WanderZone : MonoBehaviour
{
	public BoxCollider2D zone;
	// Use this for initialization


	public Rect GetWanderBounds(){
		Rect rect = new Rect();
		rect.xMin = zone.bounds.min.x;
        rect.xMax = zone.bounds.max.x;
        rect.yMin = zone.bounds.min.y;
        rect.yMax = zone.bounds.max.y;

        return rect;
	}
}

