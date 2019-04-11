using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO convert into Network behaviour and
//Sync CurrentPlayer and TurnStartTime 
// After changing UI for Network HUD
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
    UIController UI;

    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        d = GC.gameObject.GetComponent<ARDebugger>();
        UI = GC.gameObject.GetComponent<UIController>();
        currentPlayer = -1;
        TurnStartTime = -1f;
        UI.SetTurnPanel(false);
    }

    public void InitPlayers(LinkedList<PlayerController> PlayersList)
    {
        //Called when Game Starts
        d.LogPersist("TC initing players");
        Players = new PlayerController[PlayersList.Count];
        LinkedListNode<PlayerController> cur = PlayersList.First;
        for(int i = 0; i < PlayersList.Count; i++)
        {
            Players[i] = cur.Value;
            cur = cur.Next;
        }
        UI.SetTurnPanel(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Players != null && Players.Length > 0)
        {
            float TimeLeftInTurn = TURN_TIME_LIMIT - (Time.time - TurnStartTime);
            //If there is no current player, then init current player
            //If there is no time left in turn, then end the turn
            if(currentPlayer == -1 || TimeLeftInTurn < 0) EndTurn();
            else
            {
                UI.SetTurnTimeText(TimeLeftInTurn.ToString());
            }
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

    void EndTurn()
    {
        if (currentPlayer == -1) currentPlayer = 0;
        else currentPlayer = (currentPlayer + 1) % Players.Length;
        PlayerController currentPlayerController = Players[currentPlayer];
        TurnStartTime = Time.time;
        if (currentPlayerController == GC.GetLocalPlayer())
        {
            UI.SetTurnText("Your Turn");
        }
        else
        {
            UI.SetTurnText(currentPlayerController.GetPlayerName() + "'s Turn");
        }
        
    }
}
