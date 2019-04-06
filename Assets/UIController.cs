using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject WaitRoomPanel;

    public void SetWaitRoomPanel(bool active)
    {
        WaitRoomPanel.SetActive(active);
    }
}
