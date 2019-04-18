using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuUIHandler : MonoBehaviour
{
    public GameObject StartMenuPanel;
    public GameObject MyProfilePanel;
    public GameObject PlayerNameInputPlaceHolder;
    // Start is called before the first frame update
    void Start()
    {
        StartMenu();
        if (PlayerPrefs.GetString("PlayerName") != "")
            PlayerNameInputPlaceHolder.GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom()
    {
        //TODO handle actual creation of a room? or naw?
        SceneManager.LoadScene("Waitroom", LoadSceneMode.Single);
    }

    public void JoinRoom()
    {
        //TODO
        Debug.Log("Handling join room");
    }

    public void MyProfile()
    {
        StartMenuPanel.SetActive(false);
        MyProfilePanel.SetActive(true);
    }

    public void StartMenu()
    {
        StartMenuPanel.SetActive(true);
        MyProfilePanel.SetActive(false);
        Debug.Log(PlayerPrefs.GetString("PlayerName"));
    }

    public void ChangeName(string NewName)
    {
        PlayerPrefs.SetString("PlayerName", NewName);
        PlayerPrefs.Save();
        PlayerNameInputPlaceHolder.GetComponent<Text>().text = NewName;
    }
}
