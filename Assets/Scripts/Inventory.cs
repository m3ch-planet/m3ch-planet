using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    GameController GC;
    PlayerController LocalPlayer;
    Vector3 GroundPos;
    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        GroundPos = AssetManager.Instance.Get("GroundImageTarget").transform.position;
        LocalPlayer = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(LocalPlayer == null && GC.GetGameHappening())
        {
            LocalPlayer = GC.GetLocalPlayer();
        }
        else if(LocalPlayer != null)
        {
            Vector3 displacement = Camera.main.transform.position - GroundPos;
            displacement = new Vector3(displacement.x, 0, displacement.z).normalized;
            transform.position = GroundPos + displacement * 2.5f;
            transform.LookAt(AssetManager.Instance.Get("GroundImageTarget").transform);
        }
    }
}
