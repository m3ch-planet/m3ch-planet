using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    //Turn Variables
    int currentPlayer;
    const float TURN_TIME_LIMIT = 30;
    float TurnStartTime; //time when the current turn started
    
    //Player Variables
    PlayerController[] Players;

    //Other Game Variables
    GameController GC;

    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        currentPlayer = -1;
        TurnStartTime = -1f;
    }

    public void InitPlayers(LinkedList<PlayerController> PlayersList)
    {
        Players = new PlayerController[PlayersList.Count];
        LinkedListNode<PlayerController> cur = PlayersList.First;
        for(int i = 0; i < PlayersList.Count; i++)
        {
            Players[i] = cur.Value;
            cur = cur.Next;
            print("Got " + Players[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerController[] GetPlayers()
    {
        return Players;
    }
}
