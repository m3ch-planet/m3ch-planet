using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { HEALTH, ENERGY, DAMAGE, SHIELD, TELEPORT };
    PowerUpType type;
    TurnController TC;
    UIController UI;

    private void Start()
    {
        TC = GameObject.FindWithTag("TurnController").GetComponent<TurnController>();
        UI = GameObject.FindWithTag("GameController").GetComponent<UIController>();
    }


    public string GetPowerUpType()
    {
        switch (type)
        {
            case PowerUpType.ENERGY:
                return "PowerupEnergy";
            case PowerUpType.HEALTH:
                return "PowerupHealth";
            case PowerUpType.DAMAGE:
                return "PowerupDamage";
            case PowerUpType.SHIELD:
                return "PowerupShield";
            case PowerUpType.TELEPORT:
                return "PowerupTeleport";
            default:
                return "";
        }
    }

    public PowerUpType GetPowerUpTypeEnum()
    {
        return type;
    }

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
                    TC.GetCurrentPlayer().CmdAddToInventory(this);
                    break;
            }

            Destroy(gameObject);
        }
    }

}
