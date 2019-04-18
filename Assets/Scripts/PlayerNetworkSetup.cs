using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerController))]
public class PlayerNetworkSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    GameController GC;
    ARDebugger d;

    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        d = GC.GetComponent<ARDebugger>();

        if (!isLocalPlayer)
        {
            for(int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
            GetComponent<PlayerController>()._isLocalPlayer = false;
            d.LogPersist("Player " + 
                GetComponent<NetworkIdentity>().netId.ToString() +
                " " +  
                GetComponent<PlayerController>().GetPlayerName() + " joined");
        }
        else
        {
            string _playerID = GetComponent<NetworkIdentity>().netId.ToString();
            GetComponent<PlayerController>()._isLocalPlayer = true;
            CmdInitPlayerName(PlayerPrefs.GetString("PlayerName"), _playerID);
            d.LogPersist("Player " +
                _playerID +
                " " +
                GetComponent<PlayerController>().GetPlayerName() +
                " created");
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        PlayerController _player = GetComponent<PlayerController>();
        string _ID = GetComponent<NetworkIdentity>().netId.ToString();
        GameController.RegisterPlayer(_ID, _player);
    }

    private void OnDisable()
    {
        GameController.UnRegisterPlayer(transform.name);
    }

    [Command]
    private void CmdInitPlayerName(string name,string _PlayerID)
    {
        GC.InitPlayerName(name, _PlayerID);
    }
}
