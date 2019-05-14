using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    //Design of this class follows Singleton Design pattern
    public GameObject PlanetPrefab;
    public GameObject GroundImageTarget;
    public GameObject Grenade;
    public GameObject PowerupHealth;
    public GameObject PowerupEnergy;
    public GameObject PowerupDamage;
    public GameObject PowerupShield;
    public GameObject PowerupTeleport;


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
            case "Grenade":
                return Grenade;
            case "PowerupHealth":
                return PowerupHealth;
            case "PowerupEnergy":
                return PowerupEnergy;
            case "PowerupDamage":
                return PowerupDamage;
            case "PowerupShield":
                return PowerupShield;
            case "PowerupTeleport":
                return PowerupTeleport;
            default:
                return null;
        }
    }
}
