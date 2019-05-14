using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUpsController : NetworkBehaviour
{
    public int NUMBER_OF_POWERUPS;

    AssetManager AM;
    GameController GC;
    TurnController TC;

    private void Start()
    {
        GC = GetComponent<GameController>();
        AM = GetComponent<AssetManager>();
        TC = GetComponent<TurnController>();
    }

    [Command]
    public void CmdInitPowerUps(GameObject Planet)
    {
        print("Spawning powerups...");
        // For loop of Instantiate and spawning
        for (int i = 0; i < NUMBER_OF_POWERUPS; i++)
        {
            Vector3 randomPos = (Random.onUnitSphere * 1.5f) + Planet.transform.position;
            GameObject PowerUp = Instantiate(AM.Get("PowerUp"), randomPos, Quaternion.identity);
            NetworkServer.Spawn(PowerUp);
            PowerUp.gameObject.name = "PowerUp " + PowerUp.GetComponent<NetworkIdentity>().netId.ToString();

            // TODO: Randomly assign power up category by setting the appropriate tag
            // First do wayfinding before this

            // Set parent of powerups to planet
            PowerUp.transform.parent = Planet.transform;
            Planet.GetComponent<Planet>().ClampPowerupUpright(PowerUp);
        }
    }
}
