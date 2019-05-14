using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
	TurnController TC;
	GameController GC;

	const float PERCENT_ARROW_HEAD = 0.1f; // Percent of the arrow that makes up the tip
	const float ARROW_HEAD_WIDTH = 0.8f; // Default width of the arrow
	const float ARROW_SHAFT_WIDTH = 0.4f; // Default width of the arrow head
	const float MAX_LEN = 3.0f; // Maximum magnitude of the arrow

    bool AttackDown;
    Vector3 PrevWandPosition;
    Vector3 direction;
    LineRenderer arrow;
    PlayerController LocalPlayer;
    void Start()
	{
		GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
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
        direction = Vector3.zero;
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
            TC.Attack(direction*3); // Shoot grenade in the direction of the arrow
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
            Vector3 ArrowTail = LocalPlayer.transform.position + LocalPlayer.transform.up*0.3f;
            Vector3 ArrowHead = ArrowTail + direction;
            arrow.SetPositions(new Vector3[] { ArrowTail, ArrowHead });
            arrow.startWidth = 0.05f;
            arrow.endWidth = 0.05f;
        }
	}

    public bool GetAttackButtonDown()
    {
        return AttackDown;
    }
}
