using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public GameObject PlanetPrefab;
    public GameObject GroundImageTarget;

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
