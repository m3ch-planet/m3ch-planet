using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { HEALTH, ENERGY };
    PowerUpType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPowerUpType(PowerUpType type)
    {
        this.type = type;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            // TODO: Based on tag of this powerup, we execute different effects 
            //    on the player
            switch (type)
            {
                case PowerUpType.ENERGY:
                    player.CmdAddEnergy(30);
                    break;
                case PowerUpType.HEALTH:
                    player.CmdAddHealth(30);
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
    }

}
