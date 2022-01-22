using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/InputConfig")]
public class InputConfig : ScriptableObject
{
	public string horizontalName;
	public string verticalName;
	public string jumpName;
	public string attackName;
	public string chargeName;
	public string dashName;

	public bool jumpButtonDown { get; private set; }
	public bool attackButtonDown { get; private set; }
	public bool chargeButtonDown { get; private set; }
	public bool dashButtonDown { get; private set; }
	public bool jumpButton => Input.GetButton(jumpName);
	public bool chargeButton => Input.GetButton(chargeName);
	public Vector2 move => new Vector2(Input.GetAxisRaw(horizontalName),Input.GetAxisRaw(verticalName));

	public void OnUpdate()
	{
		jumpButtonDown |= Input.GetButtonDown(jumpName);
		attackButtonDown |= Input.GetButtonDown(attackName);
		chargeButtonDown |= Input.GetButtonDown(chargeName);
		dashButtonDown |= Input.GetButtonDown(dashName);
	}

	public void OnFixUpdateEnd()
	{
		jumpButtonDown = false;
		attackButtonDown = false;
		chargeButtonDown = false;
		dashButtonDown = false;
	}
}
