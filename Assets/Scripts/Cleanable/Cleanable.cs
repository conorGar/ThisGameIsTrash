using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CLEANABLE_TYPE
{
    NONE,
    // World One
    W1_CENTER_BUSHES,
    W1_TRAIL_BUSHES,

    // World Two
    W2_CENTER_BEACH_BUSHES,
    W2_INSIDE_A_WHALE_BUSHES,

    // More Worlds...


    // Size
    SIZE
}

public class Cleanable : MonoBehaviour
{
    public Color gizmoColor = Color.green;
    public virtual CLEANABLE_TYPE CleanableType()
    {
        return CLEANABLE_TYPE.NONE;
    }
}
