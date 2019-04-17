using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    //Design of this class follows Singleton Design pattern
    public GameObject PlanetPrefab;
    public GameObject GroundImageTarget;

    public static AssetManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject Get(string name)
    {
        switch (name)
        {
             case "Planet":
                return PlanetPrefab;
            case "GroundImageTarget":
                return GroundImageTarget;
            default:
                return null;
        }
    }
}
