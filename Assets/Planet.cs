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

    //Player Book Keeping
    bool init;

    // Start is called before the first frame update
    void Start()
    {
        init = false;
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        d = GC.gameObject.GetComponent<ARDebugger>();
        TC = GC.gameObject.GetComponent<TurnController>();
        RB = new List<Rigidbody>();
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
        init = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!init && TC.GetPlayers().Length > 0)
        {
            Init(5);
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

    public void AddRigidbody(Rigidbody rb)
    {
        RB.Add(rb);
    }

    public void RemoveRigidbody(Rigidbody rb)
    {
        RB.Remove(rb);
    }
}
