using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionExtension
{
    public static T RandomElement<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
