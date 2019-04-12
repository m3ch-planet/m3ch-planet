using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//TODO convert into Network behaviour and
//Sync CurrentPlayer and TurnStartTime 
// After changing UI for Network HUD
public class TurnController : NetworkBehaviour
{
    //Turn Variables
    [SyncVar]
    int currentPlayer;
    const float TURN_TIME_LIMIT = 31; //shows up as 30
    [SyncVar]
    int TimeLeftInTurn;
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
            //Only update Time Left In Turn if you are the server
            if(isServer) TimeLeftInTurn = (int)(TURN_TIME_LIMIT - (Time.time - TurnStartTime));
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
        else
        {
            d.Log("TC has no Players");
        }
    }

    void SpinPlanetToPlayer()
    {
        GameObject planet = AssetManager.Instance.Get("Planet");
        Quaternion original = planet.transform.rotation;
        Vector3 n = GC.GetLocalPlayer().transform.position - planet.transform.position;
        print(n.ToString());
        print(Vector3.up.ToString());
        Quaternion target = original*Quaternion.FromToRotation(n, Vector3.up); //TODO get the rotation such that player is on top
        planet.transform.rotation = Quaternion.Slerp(original,target,Time.deltaTime);
    }


    public PlayerController[] GetPlayers()
    {
        return Players;
    }

    public void EndTurn()
    {
        GC.GetLocalPlayer().CmdEndTurn();
    }

    public void DoEndTurn()
    {
        if (currentPlayer == -1) currentPlayer = 0;
        else currentPlayer = (currentPlayer + 1) % Players.Length;
        PlayerController currentPlayerController = Players[currentPlayer];
        TurnStartTime = Time.time;
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
