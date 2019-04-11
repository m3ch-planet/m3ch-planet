using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject WaitRoomPanel;
    public GameObject TurnPanel;
    public TextMeshProUGUI TurnText;
    public TextMeshProUGUI TurnTimeText;

    public void SetWaitRoomPanel(bool active)
    {
        WaitRoomPanel.SetActive(active);
    }

    public void SetTurnPanel(bool active)
    {
        TurnPanel.SetActive(active);
        print("Turn Panel " + TurnPanel.activeSelf);
    }

    public void SetTurnText(string s)
    {
        TurnText.text = s;
    }

    public void SetTurnTimeText(string s)
    {
        TurnTimeText.text = s;
    }
}
