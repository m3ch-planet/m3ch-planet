using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerController))]
public class PlayerNetworkSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            for(int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
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
}
