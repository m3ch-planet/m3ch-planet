using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    GameController GC;

    int maxHealth = 100;
    int maxEnergy = 100;

    [SyncVar]
    int currentHealth;
    [SyncVar]
    int currentEnergy;
    private void Awake()
    {
        currentHealth = maxHealth;
        currentEnergy = 0;
    }

    private void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void TakeDamage(int _amt)
    {
        currentHealth -= _amt;
    }

    
}
