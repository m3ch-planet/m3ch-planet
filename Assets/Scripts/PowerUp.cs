using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Upon collision
    //  1. Cause effect on player
    //  2. Destroy itself
    //  3. Notify PowerUpsController that this has been destroyed
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            // TODO: Based on tag of this powerup, we execute different effects 
            //    on the player
            player.CmdChangeEnergy(30);
            Destroy(gameObject);
        }
    }
}
