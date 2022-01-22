using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public enum EPlayerStates
	{
		Locomotion = 0,
		Dash = 1,
		Attack = 2,
		Charge = 3,
		ChargeEndAttack = 4,
		Die = 5,
	}

	public int ID;
	[SerializeField]
	private PlayerConfig m_PlayerConfig;
	[SerializeField]
	private InputConfig m_InputConfig;
	[SerializeField]
	private Rigidbody2D m_Rigidbody;
	[SerializeField]
	private Animator m_Animator;
	[SerializeField]
	private Collider2D m_FootCollider;
	[SerializeField]
	private Transform m_HandPoint;
	[SerializeField]
	private Transform m_AttackPoint;
	[SerializeField]
	private ShakeEffect m_ShakeEffect;

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
		m_StateChanged = false;

		StateUpdate(currentState);
		AnimationUpdate();

		m_Rigidbody.velocity = m_Veclocity;
		m_StateLastTime += Time.fixedDeltaTime;
		m_InputConfig.OnFixUpdateEnd();
	}

	private void AnimationUpdate()
	{
		m_Animator.SetBool("onGround", onGround);
		m_Animator.SetFloat("velocityX", Mathf.Abs(m_Veclocity.x));
		m_Animator.SetInteger("state", (int)currentState);
	}

	private void ChangeState(EPlayerStates next)
	{
		if (m_StateChanged)
		{
			return;
		}
		m_StateChanged = true;
		m_StateLastTime = 0;
		m_Animator.SetTrigger("stateChange");
		ExitState(currentState);
		EnterState(next);
		currentState = next;
	}

	private void ExitState(EPlayerStates cur)
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
				m_Veclocity = Vector2.zero;
				break;
		}
	}

	private void EnterState(EPlayerStates next)
	{
		switch (next)
		{
			case EPlayerStates.Locomotion:
				break;
			case EPlayerStates.Dash:
				AudioManager.Instance.PlaySound(ResourcesManager.AudioNames.Dash);
				m_JumpUp = false;
				m_DashDir = m_InputConfig.move == Vector2.zero ? m_FaceDir : m_InputConfig.move;
				m_StillDash = m_PlayerConfig.GetDashVelocity(ref m_Veclocity, m_DashDir, m_InputConfig.move.x, m_StateLastTime, true, Time.fixedDeltaTime);
				break;
			case EPlayerStates.Attack:
				Attack(ResourcesManager.PrefabsName.Attack);
				//AudioManager.Instance.PlaySound(ResourcesManager.AudioNames.Attack);
				m_DashDir = m_InputConfig.move == Vector2.zero ? m_FaceDir : m_InputConfig.move;
				m_PlayerConfig.GetAttackVelocity(ref m_Veclocity, m_DashDir, true, Time.fixedDeltaTime);
				break;
			case EPlayerStates.Charge:
				AudioManager.Instance.PlayUntil(ResourcesManager.AudioNames.Charge, () => currentState == EPlayerStates.Charge, true);
				break;
			case EPlayerStates.ChargeEndAttack:
				Attack(ResourcesManager.PrefabsName.Atokin1);
				//AudioManager.Instance.PlaySound(ResourcesManager.AudioNames.Atokin);
				m_DashDir = m_InputConfig.move == Vector2.zero ? -m_FaceDir : -m_InputConfig.move;
				m_PlayerConfig.GetAttackVelocity(ref m_Veclocity, m_DashDir, true, Time.fixedDeltaTime);
				break;
		}
	}

	private void StateUpdate(EPlayerStates cur)
	{
		switch (cur)
		{
			case EPlayerStates.Locomotion:
				if (onGround)
				{
					m_PlayerConfig.Recovering(ref m_Energy, Time.fixedDeltaTime);
				}
				UpdateLocomotionVelocity();
				TestDash();
				TestAttack();
				TestCharge();
				break;
			case EPlayerStates.Dash:
				m_StillDash = m_PlayerConfig.GetDashVelocity(ref m_Veclocity, m_DashDir, m_InputConfig.move.x, m_StateLastTime, false, Time.fixedDeltaTime);
				TestAttack();
				TestCharge();
				if (m_StateLastTime > m_PlayerConfig.dashTime)
				{
					ChangeState(EPlayerStates.Locomotion);
				}
				break;
			case EPlayerStates.Attack:
				m_PlayerConfig.GetAttackVelocity(ref m_Veclocity, m_DashDir, false, Time.fixedDeltaTime);
				TestDash();
				TestCharge();
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
				m_PlayerConfig.GetAttackVelocity(ref m_Veclocity, m_DashDir, false, Time.fixedDeltaTime);
				if (m_StateLastTime > m_PlayerConfig.chargeCanDashTime)
				{
					TestDash();
					TestAttack();
				}
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

	public bool Hit()
	{
		m_ShakeEffect.BeginShake(0.1f, 0.1f);
		StartCoroutine(TimeManager.WorkAfterRealTime(() => { EffectManager.Instance.CameraShake(); EffectManager.Instance.TimeStop(); }, 0.04f));
		
		ChangeState(EPlayerStates.Die);
		return false;
	}

	public void HitAttack()
	{
	}

	public void HitAtokin()
	{
	}

	public void Attack(ResourcesManager.PrefabsName name)
	{
		if (name != ResourcesManager.PrefabsName.Attack)
		{
			AttackController attack = Instantiate(ResourcesManager.Instance.Get(name), m_HandPoint.position, Quaternion.identity).GetComponent<AttackController>();
			attack?.Init(this, m_InputConfig.move == Vector2.zero ? m_FaceDir : m_InputConfig.move);
		}
		else
		{
			AttackController attack = Instantiate(ResourcesManager.Instance.Get(name), m_AttackPoint).GetComponent<AttackController>();
			attack?.Init(this, m_FaceDir);
		}
	}

	public void TestAttack()
	{
		if (!m_StateChanged && m_InputConfig.attackButtonDown && m_PlayerConfig.TryAttack(ref m_Energy))
		{
			ChangeState(EPlayerStates.Attack);
		}
	}

	public void TestCharge()
	{
		if (!m_StateChanged && m_InputConfig.chargeButton && m_PlayerConfig.TryCharge(ref m_Energy))
		{
			ChangeState(EPlayerStates.Charge);
		}
	}

	public void TestDash()
	{
		if (!m_StateChanged && m_InputConfig.dashButtonDown && m_PlayerConfig.TryDash(ref m_Energy))
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
		transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * m_FaceDir.x, transform.localScale.y,transform.localScale.z);

		if (jump)
		{
			m_CanJump = false;
			m_JumpUp = true;
			m_Animator.SetTrigger("jumpUp");
			AudioManager.Instance.PlaySound(ResourcesManager.AudioNames.Jump);
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
