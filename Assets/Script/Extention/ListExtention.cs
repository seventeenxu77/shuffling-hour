using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtention
{
    public static T draw<T>(this List<T>target)
    {
        if(target.Count==0)return default;
        int r=Random.Range(0,target.Count);
        T b=target[r];
        target.Remove(b);
        return b;
    }
}
