using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ALIGNED_AXIS
{
    X_AXIS,
    Y_AXIS
}

// Component that can be queried by other components to find out if this gameObject is axis aligned with another gameObject based on some threshold.
public class AlignedWithObjectOnAxis : MonoBehaviour {
    public GameObject alignedObject;
    public ALIGNED_AXIS alignedAxis;
    public float threshold;

    public bool IsAligned()
    {
        switch (alignedAxis) {
            case ALIGNED_AXIS.X_AXIS:
                if (Mathf.Abs(gameObject.transform.position.x - alignedObject.transform.position.x) < threshold)
                    return true;
                break;
            case ALIGNED_AXIS.Y_AXIS:
                if (Mathf.Abs(gameObject.transform.position.y - alignedObject.transform.position.y) < threshold)
                    return true;
                break;
        }

        return false;
    }

    // Returns the positional difference on the align axis.
    public float GetAlignDiff()
    {
        switch (alignedAxis) {
            case ALIGNED_AXIS.X_AXIS:
                return gameObject.transform.position.x - alignedObject.transform.position.x;
            case ALIGNED_AXIS.Y_AXIS:
                return gameObject.transform.position.y - alignedObject.transform.position.y;
            default:
                return 0f;
        }
    }
}
