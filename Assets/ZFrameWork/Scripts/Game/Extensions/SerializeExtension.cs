using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SerializeExtension
{
    public static string ToJson(this object data,bool prettyPrint = true) { 
        return JsonUtility.ToJson(data, prettyPrint);
    }
    public static T FromJson<T>(this string json) {
        return JsonUtility.FromJson<T>(json);
    }
    public static void FromJsonOverwrite<T>(this string json, T objectToOverwrite)
    {
        JsonUtility.FromJsonOverwrite(json, objectToOverwrite);
    }
}
