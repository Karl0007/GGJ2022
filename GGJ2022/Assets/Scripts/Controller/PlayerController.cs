using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public enum EPlayerStates
	{
		Locomotion,
		Dash,
		Attack,
		Charge,
		ChargeEndAttack,
	}

	[SerializeField]
	private PlayerConfig m_PlayerConfig;
	[SerializeField]
	private InputConfig m_InputConfig;
	[SerializeField]
	private Rigidbody2D m_Rigidbody;
	[SerializeField]
	private Collider2D m_FootCollider;


	private float m_StateLastTime;
	private float m_Energy;
	private bool m_CanJump;
	private bool m_StillDash;
	private bool m_StateChanged;
	private bool m_JumpUp;
	private Vector2 m_Veclocity;
	private Vector2 m_DashDir;
	private Vector2 m_FaceDir;
	public float energy { get => m_Energy; set => m_Energy = value; }
	public bool onGround { get; set; }
	public bool touchGround { get; set; }
	public EPlayerStates currentState { get; private set; }



	private void FixedUpdate()
	{
		InitCollider();
		m_Veclocity = m_Rigidbody.velocity;

		StateUpdate(currentState);

		m_Rigidbody.velocity = m_Veclocity;
		m_StateLastTime += Time.fixedDeltaTime;
		m_InputConfig.OnFixUpdateEnd();
	}

	private void ChangeState(EPlayerStates next)
	{
		m_StateLastTime = 0;
		StateEnd(currentState);
		StateStart(next);
		currentState = next;
	}

	private void StateEnd(EPlayerStates cur)
	{
		switch (cur)
		{
			case EPlayerStates.Locomotion:
				break;
			case EPlayerStates.Dash:
				break;
			case EPlayerStates.Attack:
				break;
			case EPlayerStates.Charge:
				break;
			case EPlayerStates.ChargeEndAttack:
				break;
		}
	}

	private void StateStart(EPlayerStates next)
	{
		switch (next)
		{
			case EPlayerStates.Locomotion:
				break;
			case EPlayerStates.Dash:
				m_JumpUp = false;
				m_DashDir = m_InputConfig.move == Vector2.zero ? m_FaceDir : m_InputConfig.move;
				m_StillDash = m_PlayerConfig.GetDashVelocity(ref m_Veclocity, m_DashDir, m_InputConfig.move.x, m_StateLastTime, true, Time.fixedDeltaTime);
				break;
			case EPlayerStates.Attack:
				break;
			case EPlayerStates.Charge:
				break;
			case EPlayerStates.ChargeEndAttack:
				break;
		}
	}

	private void StateUpdate(EPlayerStates cur)
	{
		Debug.Log(cur);
		switch (cur)
		{
			case EPlayerStates.Locomotion:
				m_PlayerConfig.Recovering(ref m_Energy, Time.fixedDeltaTime);
				UpdateLocomotionVelocity();
				TestDash();
				TestAttack();
				TestCharge();
				break;
			case EPlayerStates.Dash:
				m_StillDash = m_PlayerConfig.GetDashVelocity(ref m_Veclocity, m_DashDir, m_InputConfig.move.x, m_StateLastTime, false, Time.fixedDeltaTime);
				if (m_StateLastTime > m_PlayerConfig.dashTime)
				{
					ChangeState(EPlayerStates.Locomotion);
				}
				break;
			case EPlayerStates.Attack:
				UpdateLocomotionVelocity();
				if (m_StateLastTime > m_PlayerConfig.attackTime)
				{
					ChangeState(EPlayerStates.Locomotion);
				}
				break;
			case EPlayerStates.Charge:
				m_PlayerConfig.Charging(ref m_Energy, Time.fixedDeltaTime);
				UpdateLocomotionVelocity();
				if (!m_InputConfig.chargeButton)
				{
					ChangeState(EPlayerStates.ChargeEndAttack);
				}
				break;
			case EPlayerStates.ChargeEndAttack:
				if (m_StateLastTime > m_PlayerConfig.chargeEndTime)
				{
					ChangeState(EPlayerStates.Locomotion);
				}
				break;
		}
	}

	public void Die()
	{

	}

	public void TestAttack()
	{
		if (m_InputConfig.attackButtonDown && m_PlayerConfig.TryDash(ref m_Energy))
		{
			ChangeState(EPlayerStates.Attack);
		}
	}

	public void TestCharge()
	{
		if (m_InputConfig.chargeButton && m_PlayerConfig.TryDash(ref m_Energy))
		{
			ChangeState(EPlayerStates.Charge);
		}
	}

	public void TestDash()
	{
		if (m_InputConfig.dashButtonDown && m_PlayerConfig.TryDash(ref m_Energy))
		{
			ChangeState(EPlayerStates.Dash);
		}
	}

	private void UpdateLocomotionVelocity()
	{
		if (onGround)
		{
			m_StillDash = false;
			m_CanJump = true;
		}

		if (m_Veclocity.y < 0)
		{
			m_JumpUp = false;
		}

		var jump = m_InputConfig.jumpButtonDown && m_CanJump;

		if (m_StillDash)
		{
			m_StillDash = m_PlayerConfig.GetDashVelocity(ref m_Veclocity, m_DashDir, m_InputConfig.move.x, float.PositiveInfinity, false, Time.fixedDeltaTime);
		}
		else
		{
			m_PlayerConfig.GetMoveVelocity(ref m_Veclocity.x, m_InputConfig.move.x, Time.fixedDeltaTime);
			m_PlayerConfig.GetJumpVelocity(ref m_Veclocity.y, onGround, jump, m_JumpUp && m_InputConfig.jumpButton, Time.fixedDeltaTime);
		}

		if (m_Veclocity.x > 0)
		{
			m_FaceDir = Vector2.right;
		}
		else if (m_Veclocity.x < 0)
		{
			m_FaceDir = Vector2.left;
		}

		if (jump)
		{
			m_CanJump = false;
			m_JumpUp = true;
		}
	}

	private void InitCollider()
	{
		var tmp = m_FootCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
		touchGround = tmp && !onGround;
		onGround = tmp;
	}

	void Start()
    {
		energy = m_PlayerConfig.maxEnergy;
		m_FaceDir = Vector2.right;
	}

    void Update()
    {
		m_InputConfig.OnUpdate();
    }
}
