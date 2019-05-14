using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
	TurnController TC;
	GameController GC;
    UIController UI;

	const float PERCENT_ARROW_HEAD = 0.1f; // Percent of the arrow that makes up the tip
	const float ARROW_HEAD_WIDTH = 0.8f; // Default width of the arrow
	const float ARROW_SHAFT_WIDTH = 0.4f; // Default width of the arrow head
	const float MAX_LEN = 3.0f; // Maximum magnitude of the arrow

    bool AttackDown;
    Vector3 PrevWandPosition;
    Vector3 direction;
    Vector3 ArrowHead;
    Vector3 ArrowTail;
    LineRenderer arrow;
    PlayerController LocalPlayer;

    GameObject selectedItem;

    void Start()
	{
		GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        UI = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();

        TC = null;
		arrow = GetComponent<LineRenderer>();
        if (arrow == null)
        {
            print("Arrow is null");
        }
        else
        {
            print("Arrow not null!!______");
        }
            
        AttackDown = false;
        PrevWandPosition = transform.position;
        direction = ArrowHead = ArrowTail = Vector3.zero;
        arrow.enabled = false;
    }

	public void OnAttackDown()
	{
        AttackDown = true;
        PrevWandPosition = transform.position;
        arrow.enabled = true;
    }

	public void OnAttackUp()
	{
        AttackDown = false;
        arrow.enabled = false;
        if(TC == null)
            TC = GameObject.FindGameObjectWithTag("TurnController").GetComponent<TurnController>();
        if(TC != null)
        {
            //TC.Attack(Vector3.up*4f);
            TC.Attack(direction * 3); // Shoot grenade in the direction of the arrow
        }
            
	}

	void Update()
	{
        if (AttackDown && GC.GetGameHappening())
        {
            //get direction and magnitude of attack
            direction = PrevWandPosition - transform.position;
            //TODO implement max magnitude
            //draw arrow
            arrow.enabled = true;
            LocalPlayer = GC.GetLocalPlayer();
            ArrowTail = LocalPlayer.transform.position + LocalPlayer.transform.up*0.3f;
            ArrowHead = ArrowTail + direction;
            arrow.SetPositions(new Vector3[] { ArrowTail, ArrowHead });
            arrow.startWidth = 0.05f;
            arrow.endWidth = 0.05f;
        }
        else
        {
            arrow.enabled = false;
        }
	}

    public bool GetAttackButtonDown()
    {
        return AttackDown;
    }

    public Vector3 GetArrowHead()
    {
        return ArrowHead;
    }

    public Vector3 GetArrowTail()
    {
        return ArrowTail;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InventoryItem"))
        {
            UI.EnableUseItemBtn(true);
            selectedItem = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InventoryItem"))
        {
            UI.EnableUseItemBtn(false);
            selectedItem = null;
        }
    }

    public void ConsumeItem()
    {
        if (selectedItem != null)
        {
            Debug.Log(selectedItem.GetComponent<PowerUp>().GetPowerUpType());
            switch (selectedItem.GetComponent<PowerUp>().GetPowerUpType()) {
                case "PowerupShield":
                    GC.GetLocalPlayer().CmdSetShield(true);
                    break;
                case "PowerupDamage":
                    GC.GetLocalPlayer().CmdSetDoubleDmg(true);
                    break;
                case "PowerupTeleport":
                    break;
            }

            UI.EnableUseItemBtn(false);
            Destroy(selectedItem);
            selectedItem = null;
        }
    }
    
    public void SetArrow(bool active)
    {
        arrow.enabled = active;
    }
}
