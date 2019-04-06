using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    

    //Warfare variables
    int maxHealth = 100;
    int maxEnergy = 100;
    [SyncVar]
    int currentHealth;
    [SyncVar]
    int currentEnergy;

    //Networking variables
    public bool _isLocalPlayer;
    [SyncVar]
    private bool _ready; //whether the Game server can start the game
    private bool prevReady;

    //Other Game variables
    GameController GC;

    //Debug Variables
    Renderer[] Renderers;
    ARDebugger d;
    
    private void Awake()
    {
        currentHealth = maxHealth;
        currentEnergy = 0;
        Renderers = gameObject.GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        d = GC.GetComponent<ARDebugger>();
        prevReady = false;
    }

    public void TakeDamage(int _amt)
    {
        currentHealth -= _amt;
    }

    public void Update()
    {
        CheckReady();
    }

    public void SetReady(bool ready)
    {
        _ready = ready;
    }

    public bool GetReady()
    {
        return _ready;
    }

    void CheckReady()
    {
        d.Log(gameObject.name + " is " + _ready + " and " + prevReady);
        if (_ready != prevReady)
        {
            //User just updated ready
            foreach (Renderer r in Renderers)
            {
                r.material.color = _ready ? Color.red : Color.white;
            }
            prevReady = _ready;
        }
    }
}
