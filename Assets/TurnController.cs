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
    ARDebugger d;

    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        d = GC.gameObject.GetComponent<ARDebugger>();
        currentPlayer = -1;
        TurnStartTime = -1f;
    }

    public void InitPlayers(LinkedList<PlayerController> PlayersList)
    {
        d.LogPersist("TC initing players");
        Players = new PlayerController[PlayersList.Count];
        LinkedListNode<PlayerController> cur = PlayersList.First;
        for(int i = 0; i < PlayersList.Count; i++)
        {
            Players[i] = cur.Value;
            cur = cur.Next;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Players != null && Players.Length > 0)
        {
            d.Log("TC Players length " + Players.Length);
        }
        else
        {
            d.Log("TC has no Players");
        }
    }

    public PlayerController[] GetPlayers()
    {
        return Players;
    }
}
