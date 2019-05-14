using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Planet : MonoBehaviour
{
    //Game Variables
    GameController GC;
    TurnController TC;
    List<Rigidbody> RB;
    ARDebugger d;
    AssetManager AM;
    int NUMBER_OF_POWERUPS = 20;
    public int seed = 10;


    //Player Book Keeping
    bool init;

    // Start is called before the first frame update
    void Start()
    {
        init = false;
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        TC = GameObject.FindGameObjectWithTag("TurnController").GetComponent<TurnController>();
        d = GC.gameObject.GetComponent<ARDebugger>();
        RB = new List<Rigidbody>();
        AM = AssetManager.Instance;
    }

    public void Init(int InitSeed)
    {
        PlayerController[] PCs = TC.GetPlayers();
        for (int i = 0; i < PCs.Length; i++)
        {
            RB.Add(PCs[i].GetComponent<Rigidbody>());
            Rigidbody rb = RB[RB.Count-1];
            rb.useGravity = false;
            PCs[i].SetReadyText(false);
        }
        Random.seed = InitSeed;
        foreach (PlayerController Player in PCs)
        {
            Player.transform.parent = transform;
            //Randomly place the player somewhere
            Player.transform.position = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f));
            //Place the player on the surface of the planet
            Vector3 n = Player.transform.position - transform.position;
            n = n.normalized * (transform.localScale.x + 0.8f);
            Player.transform.position = transform.position + n;
            ClampPlayerUpright(Player);
        }
        InitPowerUps();
        init = true;
    }

    public void InitPowerUps()
    {
        print("Spawning powerups...");

        // For loop of Instantiate and spawning
        for (int i = 0; i < NUMBER_OF_POWERUPS; i++)
        {
            Vector3 randomPos = (Random.onUnitSphere * 1.5f) + transform.position;

            float random = Random.Range(0f, 1f);

            GameObject prefab = null;
            global::PowerUp.PowerUpType type;

            if (random <= .3f)
            {
                prefab = AM.Get("PowerupHealth");
                type = global::PowerUp.PowerUpType.HEALTH;
            }
            else if (random <= .6f)
            {
                prefab = AM.Get("PowerupEnergy");
                type = global::PowerUp.PowerUpType.ENERGY;

            }
            else if (random <= .75f)
            {
                prefab = AM.Get("PowerupDamage");
                type = global::PowerUp.PowerUpType.DAMAGE;

            }
            else if (random <= .9f)
            {
                prefab = AM.Get("PowerupShield");
                type = global::PowerUp.PowerUpType.SHIELD;

            }
            else
            {
                prefab = AM.Get("PowerupTeleport");
                type = global::PowerUp.PowerUpType.TELEPORT;

            }

            GameObject PowerUp = Instantiate(prefab, randomPos, Quaternion.identity);
            PowerUp.GetComponent<PowerUp>().SetPowerUpType(type);
            NetworkServer.Spawn(PowerUp);

            // TODO: Randomly assign power up category by setting the appropriate tag
            // First do wayfinding before this

            // Set parent of powerups to planet
            PowerUp.transform.parent = transform;
            GetComponent<Planet>().ClampPowerupUpright(PowerUp);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!init && TC.GetPlayers().Length > 0)
        {
            Debug.Log("Seed is " + seed);
            Init(seed);
        }
        //TODO handle when a player disconnects or leaves the room
        if (RB != null && RB.Count > 0)
        {
            foreach (Rigidbody rb in RB)
            {
                //print("gravitying " + rb.gameObject.name);
                Vector3 force = transform.position - rb.transform.position;
                force = force.normalized * 3f;
                rb.AddForce(force);
            }
        }
    }

    public void ClampPlayerUpright(PlayerController p)
    {
        Vector3 n = (p.transform.position - transform.position).normalized;
        //Align Player's rotation to planet's normal
        RaycastHit hit;
        if (Physics.Raycast(
            p.transform.position, //Shoot a ray from player's position
            -n,  //in the direction from the player to the planet
            out hit, //store the ray hit in hit
            Mathf.Infinity, //no limit to distance
            1 << 9) //only detect objects in layer 9 (Planet)
            )
        {
            p.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }

        //p.transform.rotation = Quaternion.LookRotation()
    }

    public void ClampPowerupUpright(GameObject p)
    {
        Vector3 n = (p.transform.position - transform.position).normalized;
        //Align Player's rotation to planet's normal
        //RaycastHit hit;
        //if (Physics.Raycast(
        //    p.transform.position, //Shoot a ray from player's position
        //    -n,  //in the direction from the player to the planet
        //    out hit, //store the ray hit in hit
        //    Mathf.Infinity, //no limit to distance
        //    1 << 9) //only detect objects in layer 9 (Planet)
        //    )
        //{
        p.transform.rotation = Quaternion.FromToRotation(Vector3.up, n);
        //}

        //p.transform.rotation = Quaternion.LookRotation()
    }

    public void AddRigidbody(Rigidbody rb)
    {
        RB.Add(rb);
    }

    public void RemoveRigidbody(Rigidbody rb)
    {
        RB.Remove(rb);
    }
}
