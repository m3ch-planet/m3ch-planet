using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    GameController GC;
    PlayerController LocalPlayer;
    Vector3 GroundPos;
    AssetManager AM;
    bool hasItem1 = false;
    bool hasItem2 = false;

    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        GroundPos = AssetManager.Instance.Get("GroundImageTarget").transform.position;
        LocalPlayer = null;
        AM = GameObject.FindGameObjectWithTag("GameController").GetComponent<AssetManager>();
    }

    void AddToInventory(GameObject prefab, GameObject parent)
    {
        GameObject obj = Instantiate(prefab, parent.transform);
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.tag = "InventoryItem";
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

            List <PowerUp> inventory = LocalPlayer.GetInventory();
            if (!hasItem1 && inventory.Count > 0)
            {
                PowerUp item1 = inventory[0];
                GameObject prefab = AM.Get(item1.GetPowerUpType());
                GameObject parent = GameObject.Find("Item1");
                AddToInventory(prefab, parent);
                hasItem1 = true;
            }
            if (!hasItem2 && inventory.Count > 1)
            {
                PowerUp item2 = inventory[1];
                GameObject prefab = AM.Get(item2.GetPowerUpType());
                GameObject parent = GameObject.Find("Item2");
                AddToInventory(prefab, parent);
                hasItem2 = true;
            }
        }
    }
}
