using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
	TurnController TC;
	GameController GC;

	LineRenderer arrow;
	Vector3 direction;

	const float PERCENT_ARROW_HEAD = 0.1f; // Percent of the arrow that makes up the tip
	const float ARROW_HEAD_WIDTH = 0.8f; // Default width of the arrow
	const float ARROW_SHAFT_WIDTH = 0.4f; // Default width of the arrow head
	const float MAX_LEN = 3.0f; // Maximum magnitude of the arrow

	void Start()
	{
		GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		arrow = GetComponent<LineRenderer>();
	}

	public void OnAttackDown()
	{
		arrow.enabled = true;
	}

	public void OnAttackUp()
	{
		arrow.enabled = false;
		TC.Attack(direction); // Shoot grenade in the direction of the arrow
	}

	void Update()
	{
		if (!GC.GetGameHappening())
			arrow.enabled = false;

		else if (arrow.enabled)
		{
			// Obtain reference to TurnController (must be done late when TC is active)
			if (!TC)
				TC = GameObject.Find("TurnController").GetComponent<TurnController>();

			// Arrow drawn between player model and tracked wand
			else
			{
				Vector3 arrowTarget = TC.GetCurrentPlayer().gameObject.transform.position + new Vector3(0, .1f, 0);
				Vector3 arrowOrigin = transform.position;

                // Enforce maximum magnitude of the arrow
                Vector3 toTarget = arrowTarget - arrowOrigin;
				if (toTarget.magnitude > MAX_LEN) {
					float u = toTarget.magnitude - MAX_LEN;
					arrowOrigin += (u * toTarget.normalized);
                    toTarget = arrowTarget - arrowOrigin;
                }

                // Scale between 20% and 100% of arrow shaft width to add "stretching" effect
                float scale = ((Mathf.Pow(toTarget.magnitude, 4) - 0f) / (Mathf.Pow(MAX_LEN, 4) - 0f)) * (1f - .0f) + .0f;
                Debug.Log("Mag " + toTarget.magnitude);
                Debug.Log("Scale " + scale);


                // Set line renderer interpolated widths
                arrow.widthCurve = new AnimationCurve(
						new Keyframe(0, ARROW_SHAFT_WIDTH * scale),
						new Keyframe(0.999f - PERCENT_ARROW_HEAD, ARROW_SHAFT_WIDTH * scale),
						new Keyframe(1 - PERCENT_ARROW_HEAD, ARROW_HEAD_WIDTH * scale),
						new Keyframe(1, 0f)
				);

				arrow.SetPositions(new Vector3[] {
						arrowOrigin,
						Vector3.Lerp(arrowOrigin, arrowTarget, 0.999f - PERCENT_ARROW_HEAD),
						Vector3.Lerp(arrowOrigin, arrowTarget, 1 - PERCENT_ARROW_HEAD),
						arrowTarget
				});

				direction = arrowTarget - arrowOrigin;
			}
		}
	}
}
