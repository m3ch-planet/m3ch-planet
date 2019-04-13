using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

//TODO change date time from system to unity engine
public class TurnController : NetworkBehaviour
{
    //Turn Variables
    [SyncVar]
    int currentPlayer;
    const int TURN_TIME_LIMIT = 30;
    [SyncVar]
    int TimeLeftInTurn;
    int TurnStartTime; //time when the current turn started
    
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
        TurnStartTime = -1;
        UI.SetTurnPanel(false);
    }

    public void InitPlayers(LinkedList<PlayerController> PlayersList)
    {
        //Called when Game Starts
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
    void LateUpdate()
    {
        if(Players != null && Players.Length > 0)
        {
            //Only update Time Left In Turn if you are the server
            if (isServer)
            {
                TimeLeftInTurn = TURN_TIME_LIMIT - (TurnStartTime - CurrentTime());
            }
            //If there is no current player, then init current player
            //If there is no time left in turn, then end the turn
            if ((currentPlayer == -1 && isServer) || TimeLeftInTurn < 0)
            {
                EndTurn();
            }
            else
            {
                UI.SetTurnTimeText(TimeLeftInTurn.ToString() + " seconds left");
                SpinPlanetToPlayer();
            }
        }
    }

    void SpinPlanetToPlayer()
    {
        GameObject planet = AssetManager.Instance.Get("Planet");
        Quaternion original = planet.transform.rotation;
        Vector3 n = Players[currentPlayer].transform.position - planet.transform.position;
        Quaternion target = Quaternion.FromToRotation(n, Vector3.up)*original;
        planet.transform.rotation = Quaternion.Slerp(original,target,Time.deltaTime*2);
    }

    //returns time in terms of seconds
    private int CurrentTime()
    {
        return -(int)(System.DateTime.UtcNow.Ticks / 10000000);
    }


    public PlayerController[] GetPlayers()
    {
        return Players;
    }

    //Player's Local Device tells the Local Player Controller
    //To send a command to the server
    public void EndTurn()
    {
        int NewPlayer = currentPlayer;
        if (NewPlayer == -1) NewPlayer = 0;
        else NewPlayer = (NewPlayer + 1) % Players.Length;
        GC.GetLocalPlayer().CmdEndTurn(NewPlayer, CurrentTime());
    }

    public void DoEndTurn(int curPlayer, int TimeStartTurn)
    {
        //End Last Player's Turn TODO
        
        //Start Next Player's Turn
        currentPlayer = curPlayer;
        TurnStartTime = TimeStartTurn;
        PlayerController currentPlayerController = Players[currentPlayer];

        if (currentPlayerController == GC.GetLocalPlayer())
        {
            UI.SetTurnText("Your Turn");
            UI.SetPlayerTurnPanel(true);
        }
        else
        {
            UI.SetTurnText(currentPlayerController.GetPlayerName() + "'s Turn");
            UI.SetPlayerTurnPanel(false);
        }
    }

    public void Walk()
    {
        //TODO
        print("Walking");
    }

    public void Attack()
    {
        //TODO
        print("Attacking");
    }

   
}
