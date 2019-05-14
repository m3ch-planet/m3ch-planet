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
    bool Walking;
    public bool Shooting;

    //Player Network Variables
    Vector3 TargetLocalPos;
    Quaternion TargetLocalRot;
    string TargetPlayerName;
    bool HasEnergy;

    //Player Variables
    PlayerController[] Players;
    

    //Other Game Variables
    GameController GC;
    AssetManager AM;
    ARDebugger d;
    UIController UI;

    //Attacking variables
    ArrowController WandHead;

    public int NUMBER_OF_POWERUPS;

    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        d = GC.gameObject.GetComponent<ARDebugger>();
        UI = GC.gameObject.GetComponent<UIController>();
        AM = AssetManager.Instance;
        currentPlayer = -1;
        TurnStartTime = -1;
        HasEnergy = true;
        Shooting = false;
        WandHead = GameObject.FindGameObjectWithTag("WandHead").GetComponent<ArrowController>();
    }

    public void InitPlayers(LinkedList<PlayerController> PlayersList)
    {
        //Called when Game Starts
        Players = new PlayerController[PlayersList.Count];
        LinkedListNode<PlayerController> cur = PlayersList.First;
        for (int i = 0; i < PlayersList.Count; i++)
        {
            Players[i] = cur.Value;
            cur = cur.Next;
        }
        UI.SetTurnPanel(true);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Make sure that Turn Controller is Init
        if (Players != null && Players.Length > 0)
        {
            //Only update Time Left In Turn if you are the server
            if (isServer)
            {
                TimeLeftInTurn = TURN_TIME_LIMIT - (TurnStartTime - CurrentTime());
            }
            //If there is no current player, then init current player
            //If there is no time left in turn, then end the turn
            if ((currentPlayer == -1) ||
                TimeLeftInTurn < 0 ||
                Players[currentPlayer].GetEnergy() < 0)
            {
                if (HasEnergy)
                {
                    EndTurn();
                }
            }
            else
            {
                if (Shooting)
                {
                    //Player just threw grenade
                    UI.SetTurnTime(false);
                    UI.SetPlayerTurnPanel(false);
                }
                else
                {
                    //Player has now thrown grenade this turn (walking, standing).
                    UI.SetTurnTimeText(TimeLeftInTurn.ToString() + " seconds left");
                }
                HandleSyncTransforms();
            }
        }
    }


    #region Networking & Transformations
    void HandleSyncTransforms()
    {
        bool Attacking = WandHead.GetAttackButtonDown();
        if (Players[currentPlayer]._isLocalPlayer)
        {
            if (Walking)
            {
                Players[currentPlayer].GetComponent<Rigidbody>().angularDrag = 10f;
                HandleWalk();
            }
            else
            {
                Players[currentPlayer].GetComponent<Rigidbody>().angularDrag = 2;
            }
            //Camera up
            if (Attacking)
                AlignPlayerWithArrow(); 
            else
                AlignPlayerWithCamera();
            //if is local player then send position to other clients via server
            Players[currentPlayer].CmdSendPlayerTransform(
                Players[currentPlayer].transform.localPosition,
                Players[currentPlayer].transform.localRotation,
                Players[currentPlayer].GetPlayerName()
                );
        }
        else
        {
            SyncCurrentPlayerTransform();
        }
        if (Attacking && !Shooting)
        {
            //render everything
            Camera.main.cullingMask = ~(1 << 10);            
            //follow current player, but spin player towards cam
            SpinPlanet("To Cam");
        }
        else if (!Attacking && !Shooting)
        {
            //don't render player ui
            Camera.main.cullingMask = ~(0);
            //follow current player, but spin player to top
            SpinPlanet("To up");
        }
        else if(!Attacking && Shooting)
        {
            //render everything
            Camera.main.cullingMask = ~(0);
            //follow grenade
            SpinPlanet("To Grenade");
        }
    }

    void HandleWalk()
    {
        Players[currentPlayer].transform.position = Players[currentPlayer].transform.position + GetCurrentPlayerForward() * Time.deltaTime;
        Players[currentPlayer].CmdDecreaseEnergy();
        //Straighten the player
        AssetManager.Instance.Get("Planet").GetComponent<Planet>().ClampPlayerUpright(Players[currentPlayer]);
    }

    public void UpdatePlayerNetworkTransforms(Vector3 localPos, Quaternion localRot, string PlayerName)
    {
        TargetLocalPos = localPos;
        TargetLocalRot = localRot;
        TargetPlayerName = PlayerName;
    }

    public void SyncCurrentPlayerTransform()
    {
        if (!Players[currentPlayer]._isLocalPlayer &&
            TargetLocalPos != null && TargetLocalRot != null && TargetPlayerName != null &&
            Players[currentPlayer].GetPlayerName() == TargetPlayerName)
        {
            if (Vector3.Distance(Players[currentPlayer].transform.localPosition, TargetLocalPos) > 0.01) 
            {
                Walking = true;
                Players[currentPlayer].transform.localPosition =
                    Vector3.Lerp(
                        Players[currentPlayer].transform.localPosition,
                        TargetLocalPos,
                        Time.deltaTime * 15
                        );
            }
            else
            {
                Walking = false;
                Players[currentPlayer].transform.localPosition = TargetLocalPos;
            }
            Players[currentPlayer].transform.localRotation = Quaternion.Slerp(
                Players[currentPlayer].transform.localRotation,
                TargetLocalRot,
                Time.deltaTime * 10
                );
        }
    }

    void SpinPlanet(string param)
    {
        GameObject planet = AssetManager.Instance.Get("Planet");
        Quaternion original = planet.transform.rotation;
        Vector3 n = Players[currentPlayer].transform.position - planet.transform.position;
        Quaternion target = Quaternion.identity;
        if (param == "To up")
        {
            target = Quaternion.FromToRotation(n, Vector3.up) * original;
            planet.transform.rotation = Quaternion.Slerp(original, target, Time.deltaTime * 2);
        }
        else if(param == "To Cam")
        {
            Vector3 PlanetToCamera = (Camera.main.transform.position - planet.transform.position).normalized;
            target = Quaternion.FromToRotation(n, PlanetToCamera) * original;
            planet.transform.rotation = Quaternion.Slerp(original, target, Time.deltaTime * 2);
        }
        else if (param == "To Grenade")
        {
            GameObject Grenade = GameObject.FindGameObjectWithTag("Grenade");
            Vector3 PlanetToGrenade = (Grenade.transform.position - planet.transform.position).normalized;
            target = Quaternion.FromToRotation(n, PlanetToGrenade) * original;
            planet.transform.rotation = target;
        }        
    }
    #endregion

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
        HasEnergy = false;
        Walking = false;
        //End Last Player's Turn
        if (currentPlayer == -1)
        {
            GC.GetLocalPlayer().CmdEndTurn(NewPlayer, CurrentTime());
        }
        else
        {
            Players[currentPlayer].CmdEndTurn(NewPlayer, CurrentTime());
        }
    }

    public void DoEndTurn(int curPlayer, int TimeStartTurn)
    {
        //Start Next Player's Turn
        currentPlayer = curPlayer;
        TurnStartTime = TimeStartTurn;
        HasEnergy = true;
        Walking = false;
        Shooting = false;
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
        UI.SetTurnTime(true);
    }


    void AlignPlayerWithCamera()
    {
        Players[currentPlayer].transform.rotation = Quaternion.LookRotation(GetCurrentPlayerForward(), GetCurrentPlayerUp());
    }

    void AlignPlayerWithArrow()
    {
        if(WandHead.GetArrowHead() != WandHead.GetArrowTail())
        {
            Vector3 PlayerToCamera = Camera.main.transform.position - Players[currentPlayer].transform.position;
            Vector3 left = Vector3.Cross(PlayerToCamera.normalized, (WandHead.GetArrowHead() - WandHead.GetArrowTail()).normalized);
            Vector3 forward = Vector3.Cross(left, PlayerToCamera.normalized);
            Players[currentPlayer].transform.LookAt(Players[currentPlayer].transform.position + forward, PlayerToCamera);
            AssetManager.Instance.Get("Planet").GetComponent<Planet>().ClampPlayerUpright(Players[currentPlayer]);
        }
    }

    Vector3 GetCurrentPlayerForward()
    {
        Vector3 right = Vector3.Cross(
            Players[currentPlayer].transform.position - Camera.main.transform.position,
            GetCurrentPlayerUp());
        right.Normalize();
        Vector3 forward = Vector3.Cross(GetCurrentPlayerUp(), right);
        return forward.normalized;
    }

    Vector3 GetCurrentPlayerUp()
    {
        Vector3 up = Players[currentPlayer].transform.position - AssetManager.Instance.Get("Planet").transform.position;
        return up.normalized;
    }

    public void SetWalking(bool WalkButtonHoldDown)
    {
        Walking = WalkButtonHoldDown;
    }

    public bool GetWalking()
    {
        return Walking;
    }

    public void Attack(Vector3 direction)
    {
        Players[currentPlayer].CmdShoot(direction);
    }

    public PlayerController GetCurrentPlayer()
    {
        if (currentPlayer > -1 && currentPlayer < Players.Length)
            return Players[currentPlayer];
        else
            return null;
    }

    [Command]
    public void CmdInitPowerUps(GameObject Planet)
    {
        print("Spawning powerups...");
        // For loop of Instantiate and spawning
        for (int i = 0; i < NUMBER_OF_POWERUPS; i++)
        {


            Vector3 randomPos = (UnityEngine.Random.onUnitSphere * 1.5f) + Planet.transform.position;


            float random = UnityEngine.Random.Range(0f, 1f);

            GameObject prefab = null;
            global::PowerUp.PowerUpType type;

            if (random <= .3f)
            {
                prefab = AM.Get("PowerupHealth");
                type = global::PowerUp.PowerUpType.HEALTH;
            }
            else if (random <= .6f)
            {
                prefab = AM.Get("PowerupEnergy");
                type = global::PowerUp.PowerUpType.ENERGY;

            }
            else if (random <= .75f)
            {
                prefab = AM.Get("PowerupDamage");
                type = global::PowerUp.PowerUpType.DAMAGE;

            }
            else if (random <= .9f)
            {
                prefab = AM.Get("PowerupShield");
                type = global::PowerUp.PowerUpType.SHIELD;

            }
            else
            {
                prefab = AM.Get("PowerupTeleport");
                type = global::PowerUp.PowerUpType.TELEPORT;

            }

            GameObject PowerUp = Instantiate(prefab, randomPos, Quaternion.identity);
            PowerUp.GetComponent<PowerUp>().SetPowerUpType(type);
            NetworkServer.Spawn(PowerUp);
            //PowerUp.gameObject.name = "PowerUp " + PowerUp.GetComponent<NetworkIdentity>().netId.ToString();

            // TODO: Randomly assign power up category by setting the appropriate tag
            // First do wayfinding before this

            // Set parent of powerups to planet
            PowerUp.transform.parent = Planet.transform;
            Planet.GetComponent<Planet>().ClampPowerupUpright(PowerUp);
        }
    }
}
