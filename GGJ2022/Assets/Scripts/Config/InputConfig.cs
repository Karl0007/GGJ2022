using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/InputConfig")]
public class InputConfig : ScriptableObject
{
	public string horizontalName;
	public string verticalName;
	public string jumpName;
	public KeyCode jumpCode = KeyCode.Joystick1Button0;
	public string attackName;
	public KeyCode attackCode = KeyCode.Joystick1Button1;
	public string chargeName;
	public string chargeAxis;
	public string dashName;
	public KeyCode dashCode = KeyCode.Joystick1Button1;

	public bool jumpButtonDown { get; private set; }
	public bool attackButtonDown { get; private set; }
	public bool chargeButtonDown { get; private set; }
	public bool dashButtonDown { get; private set; }
	public bool jumpButton => Input.GetButton(jumpName) | Input.GetKey(jumpCode);
	public bool chargeButton => (chargeName != "" && Input.GetButton(chargeName)) | (chargeAxis != "" && Mathf.Abs(Input.GetAxisRaw(chargeAxis)) > 0.1f);
	public Vector2 move => new Vector2(GetSign(Input.GetAxisRaw(horizontalName)), 0);
	public Vector2 skillDir => new Vector2(GetSign(Input.GetAxisRaw(horizontalName)), GetSign(- Input.GetAxisRaw(verticalName))).normalized;

	public float GetSign(float x)
	{
		if (Mathf.Abs(x) < 0.7f)
		{
			return 0;
		}
		else
		{
			return Mathf.Sign(x);
		}
	}

	public void OnUpdate()
	{
		jumpButtonDown |= Input.GetButtonDown(jumpName) | Input.GetKeyDown(jumpCode);
		attackButtonDown |= Input.GetButtonDown(attackName) | Input.GetKeyDown(attackCode);
		dashButtonDown |= Input.GetButtonDown(dashName) | Input.GetKeyDown(dashCode);
		chargeButtonDown |= (chargeName != "" && Input.GetButtonDown(chargeName)) | (chargeAxis != "" && Mathf.Abs(Input.GetAxisRaw(chargeAxis)) > 0.1f);
	}

	public void OnFixUpdateEnd()
	{
		jumpButtonDown = false;
		attackButtonDown = false;
		chargeButtonDown = false;
		dashButtonDown = false;
	}
}
