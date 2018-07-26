using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CollectionExtension
{
    public static T RandomElement<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }
}
