using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject WaitRoomPanel;
    public GameObject TurnPanel;
    public GameObject PlayerTurnPanel;
    public TextMeshProUGUI TurnText;
    public TextMeshProUGUI TurnTimeText;

    private void Start()
    {
        WaitRoomPanel.SetActive(true);
        TurnPanel.SetActive(false);
        PlayerTurnPanel.SetActive(false);
    }

    public void SetWaitRoomPanel(bool active)
    {
        WaitRoomPanel.SetActive(active);
    }

    public void SetTurnPanel(bool active)
    {
        TurnPanel.SetActive(active);
    }

    public void SetTurnText(string s)
    {
        TurnText.text = s;
    }

    public void SetTurnTimeText(string s)
    {
        TurnTimeText.text = s;
    }

    public void SetPlayerTurnPanel(bool active)
    {
        PlayerTurnPanel.SetActive(active);
    }
}
