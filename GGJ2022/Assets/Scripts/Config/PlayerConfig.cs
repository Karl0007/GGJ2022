using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
	#region Move
	[Header("Move")]
	public float moveThreshold;
	public float moveVelocity;
	public float moveAcceleration;
	public float moveStopAcceleration;

	public void GetMoveVelocity(ref float cur,float input,float deltaTime,float slowScale = 1)
	{
		if (input > moveThreshold)
		{
			if (cur < 0)
				cur += moveStopAcceleration * deltaTime;
			cur += moveAcceleration * deltaTime;
			if (cur > moveVelocity * slowScale)
				cur = moveVelocity * slowScale;
		}
		else if (input < -moveThreshold)
		{
			if (cur > 0)
				cur -= moveStopAcceleration * deltaTime;
			cur -= moveAcceleration * deltaTime;
			if (cur < -moveVelocity * slowScale)
				cur = -moveVelocity * slowScale;
		}
		else
		{
			if (cur > 0)
			{
				cur -= moveStopAcceleration * deltaTime;
				if (cur < 0)
					cur = 0;
			}
			else if (cur < 0)
			{
				cur += moveStopAcceleration * deltaTime;
				if (cur > 0)
					cur = 0;
			}
		}
	}
	#endregion

	#region Jump
	[Header("Jump")]
	public float jumpVelocity;
	public float gravityDefault;
	public float gravityUp;
	public float gravityDown;
	public float maxFall;

	public void GetJumpVelocity(ref float cur,bool onGround,bool startJump,bool buttonDown,float deltaTime)
	{
		if (startJump)
		{
			cur = jumpVelocity;
		}else if (onGround)
		{
			cur = 0;
		}
		else if (cur > 0)
		{
			GravityCtrl(ref cur, buttonDown ? gravityUp : gravityDefault, deltaTime);
		}
		else
		{
			GravityCtrl(ref cur, gravityDown, deltaTime);
		}
	}

	public void GravityCtrl(ref float cur,float g,float deltaTime)
	{
		cur -= g * deltaTime;
		if (cur < -maxFall)
			cur = -maxFall;
	}
	#endregion

	#region Dash
	[Header("Dash")]
	public float dashVelocity;
	public float dashTime;
	public float dashAfterTime;
	public Vector2 dashDecline;

	public bool GetDashVelocity(ref Vector2 cur, Vector2 dir, float inputX, float lastTime,bool startDash, float deltaTime, float slowScale = 1)
	{
		if (startDash)
		{
			cur = dir.normalized * dashVelocity;
			return true;
		}
		if (lastTime > dashTime && lastTime < dashAfterTime + dashTime)
		{
			cur *= dashDecline;
			return true;
		}
		if (lastTime > dashAfterTime + dashTime)
		{
			return false;
		}
		return true;
	}

	#endregion

	#region Num
	public float maxEnergy;
	public float attackEnergy;
	public float dashEnergy;
	public float startChargeEnergy;
	public float chargeEnergy;
	public float recoverEnergy;
	public float recoverEnergyTime;

	public float maxAtokinEnergy;
	public float atokinEnergy;

	public float collectMaxSize;
	public float collectMaxTime;

	public bool TryAttack(ref float cur)
	{
		if (cur > attackEnergy)
		{
			cur -= attackEnergy;
			return true;
		}
		return false;
	}

	public bool TryDash(ref float cur)
	{
		if (cur > dashEnergy)
		{
			cur -= dashEnergy;
			return true;
		}
		return false;
	}

	public bool TryCharge(ref float cur)
	{
		if (cur > startChargeEnergy)
		{
			cur -= startChargeEnergy;
			return true;
		}
		return false;
	}

	public void Charging(ref float cur,float deltaTime)
	{
		cur -= chargeEnergy * deltaTime;
		cur = Mathf.Clamp(cur, 0, maxEnergy);
	}

	public void Recovering(ref float cur, float deltaTime)
	{
		cur += recoverEnergy * deltaTime;
		cur = Mathf.Clamp(cur, 0, maxEnergy);
	}

	public void GetAtokinEnergy(ref float cur)
	{
		cur = Mathf.Min(cur + atokinEnergy, maxAtokinEnergy);
	}
	#endregion

	#region Animation
	public float attackTime;
	public float chargeEndTime;
	public float chargeCanDashTime;
	public float attackVelocity;
	public float atokinVelocity;

	public void GetAttackVelocity(ref Vector2 cur, Vector2 dir, bool startAttack, float deltaTime, float slowScale = 1)
	{
		if (startAttack)
		{
			cur.x = dir.normalized.x * attackVelocity;
			return;
		}
		else
		{
			cur *= dashDecline;
			return;
		}
	}
	#endregion
}
