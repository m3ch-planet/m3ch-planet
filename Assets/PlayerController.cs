using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    //Warfare variables
    int maxHealth = 100;
    float maxEnergy = 100f;
    [SyncVar]
    int currentHealth;
    [SyncVar]
    float currentEnergy;

    //Networking variables
    public bool _isLocalPlayer;
    [SyncVar]
    private bool _ready; //whether the Game server can start the game
    private bool prevReady;

    //PlayerVariables
    [SyncVar]
    string PlayerName;
    GameObject PlayerMesh;

    //Other Game variables
    GameController GC;
    AssetManager AM;
    TurnController TC;

    //Debug Variables
    ARDebugger d;

    //PlayerUI
    public GameObject PlayerNameText;
    GameObject PlayerUICanvas;
    public GameObject HealthBar;
    public GameObject EnergyBar;
    Slider HealthBarSlider;
    Slider EnergyBarSlider;
    public TextMeshProUGUI ReadyText;
    
    private void Awake()
    {
        currentHealth = maxHealth;
        currentEnergy = 0;
        //Init Player UI variables
        HealthBarSlider = HealthBar.GetComponent<Slider>();
        EnergyBarSlider = EnergyBar.GetComponent<Slider>();
    }

    private void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        d = GC.GetComponent<ARDebugger>();
        AM = GC.GetComponent<AssetManager>();
        TC = GC.GetComponent<TurnController>();
        prevReady = false;
        PlayerUICanvas = PlayerNameText.transform.parent.gameObject;
        PlayerUICanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        PlayerMesh = transform.GetChild(0).gameObject;
    }

    public void InitPlayerName(string name)
    {
        if (name == "") PlayerName = "Bob";
        else PlayerName = name;
        PlayerNameText.GetComponent<TextMeshProUGUI>().text = PlayerName;
    }

    public void TakeDamage(int _amt)
    {
        currentHealth -= _amt;
    }

    public void Update()
    {
        if (GC.GetGameHappening())
        {

        }
        else
        {
            CheckReady();
            PlayerNameText.GetComponent<TextMeshProUGUI>().text = PlayerName;
        }

        PlayerUICanvas.transform.LookAt(Camera.main.transform);
        HealthBarSlider.value = Mathf.Lerp(HealthBarSlider.value, (float)currentHealth / 100, Time.deltaTime*6);
        EnergyBarSlider.value = Mathf.Lerp(EnergyBarSlider.value, currentEnergy / 100, Time.deltaTime*6); 
    }

    [Command]
    //Player on Server is set to ready
    //_ready is SyncVar so syncs to all clients
    public void CmdSetReady(bool ready)
    {
        _ready = ready;
        if (GC.AreAllPlayersReady())
            CmdStartGame();
    }

    [Command]
    public void CmdDecreaseEnergy()
    {
        if(currentEnergy > 0)
        {
            currentEnergy -= Time.deltaTime * 20;
        }
        else
        {
            TC.EndTurn();
        }
    }

    public bool GetReady()
    {
        return _ready;
    }

    void CheckReady()
    {
        if (_ready != prevReady)
        {
            //User just updated ready
            ReadyText.text = _ready ? "Ready" : "Not Ready";
            ReadyText.color = _ready ? Color.green : Color.red;
            prevReady = _ready;
        }
    }

    public void SetReadyText(bool active)
    {
        ReadyText.gameObject.SetActive(active);
    }

    public string GetPlayerName()
    {
        return PlayerName;
    }

    #region Game Initializers
    //Tells server to start game
    [Command]
    private void CmdStartGame()
    {
        GameObject Planet = Instantiate(AM.Get("Planet"));
        NetworkServer.Spawn(Planet);
        Planet.gameObject.name = "Planet " + Planet.GetComponent<NetworkIdentity>().netId.ToString();
        RpcStartGame(Planet);
    }

    //Tells every client to start game
    [ClientRpc]
    private void RpcStartGame(GameObject Planet)
    {
        Planet.transform.parent = AssetManager.Instance.Get("GroundImageTarget").transform;
        AssetManager.Instance.PlanetPrefab = Planet;
        GC.StartGame(Planet);
    }
    #endregion

    #region TurnController Helpers
    [Command]
    public void CmdEndTurn(int currrentPlayer,int TimeStartTurn)
    {
        currentEnergy = 0;
        TC.GetPlayers()[currrentPlayer].currentEnergy = maxEnergy;
        RpcEndTurn(currrentPlayer, TimeStartTurn);
    }

    [ClientRpc]
    public void RpcEndTurn(int currrentPlayer, int TimeStartTurn)
    {
        TC.DoEndTurn(currrentPlayer, TimeStartTurn);
    }
    #endregion
}
