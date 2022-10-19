using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamerOwer : MonoBehaviour
{
    private static GamerOwer instance;
    public static GamerOwer Instance { get {
            if (instance == null)
            {
                var gameObject = new GameObject("GameOwer");
                instance = gameObject.AddComponent<GamerOwer>();
            }
            return instance;
        } }

    void Awake()
    {
        if (instance == null)
            instance = this;
        GameObject.DontDestroyOnLoad(this);
    }
}
