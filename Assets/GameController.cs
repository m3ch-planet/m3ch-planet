using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    const string PLAYER_ID_PREFIX = "Player ";
    static Dictionary<string, PlayerController> Players = new Dictionary<string, PlayerController>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static PlayerController GetPlayer(string _ID)
    {
        return Players[_ID];
    }

    public static void RegisterPlayer(string _ID, PlayerController _player)
    {
        string _playerId = PLAYER_ID_PREFIX + _ID;
        Players.Add(_playerId, _player);
        _player.transform.name = _playerId;
    }

    public static void UnRegisterPlayer(string _ID)
    {
        Players.Remove(_ID);
    }
}
