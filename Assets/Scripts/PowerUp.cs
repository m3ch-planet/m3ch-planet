using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { HEALTH, ENERGY, DAMAGE, SHIELD, TELEPORT };
    PowerUpType type;

    public void SetPowerUpType(PowerUpType type)
    {
        this.type = type;
    }

    // Upon collision
    //  1. Cause effect on player
    //  2. Destroy itself
    //  3. Notify PowerUpsController that this has been destroyed
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            switch (type)
            {
                case PowerUpType.ENERGY:
                    player.CmdChangeEnergy(30);
                    break;
                case PowerUpType.HEALTH:
                    player.CmdChangeHealth(30);
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
    }
}
