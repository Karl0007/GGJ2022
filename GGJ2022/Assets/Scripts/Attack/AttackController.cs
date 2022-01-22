using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
	public Animator m_Animator;
	public Vector2 m_Dir;
	public Collider2D m_Collider;
	public int m_PlayerID;
	public int m_Level;
	public float m_Velocity;
	public bool m_isFly;
	public PlayerController m_Player;
	public ResourcesManager.AudioNames m_EndAudio;
	public ResourcesManager.AudioNames m_StartAudio;
	public float m_lifeTime = 10;

	public static Vector2 RandomVec(float len)
	{
		return new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized * len;
	}

	public static Vector2 Ang2Vec(float f)
	{
		f *= Mathf.Deg2Rad;
		return new Vector2(Mathf.Cos(f), Mathf.Sin(f));
	}
	public static float Vec2Ang(Vector2 v)
	{
		return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
	}
	public static Quaternion Vec2Qua(Vector2 v)
	{
		return Quaternion.Euler(new Vector3(0, 0, Vec2Ang(v)));
	}

	public void Init(PlayerController player,Vector2 dir)
	{
		m_Dir = dir.normalized;
		m_Player = player;
		m_PlayerID = player.ID;
		AudioManager.Instance.PlaySound(m_StartAudio);
	}

	private void Awake()
	{
		m_Animator = GetComponent<Animator>();
		Destroy(gameObject,m_lifeTime);
	}

	private void FixedUpdate()
	{
		transform.position += (Vector3)m_Dir * m_Velocity * Time.fixedDeltaTime;
		if (m_isFly)
		{
			transform.rotation = Vec2Qua(m_Dir);
		}
	}

	private void End()
	{
		Debug.Log("end");
		AudioManager.Instance.PlaySound(m_EndAudio);
		m_Animator.SetTrigger("Hit");
		m_Velocity = 0;
		Destroy(this.m_Collider);
		Destroy(this.gameObject, 0.5f);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			var otherPlayer = collision.GetComponentInParent<PlayerController>();
			if (otherPlayer.ID != m_PlayerID)
			{
				otherPlayer.Hit();
				PlayerManager.Instance.OnAttack(m_PlayerID, otherPlayer.ID);
				if (m_Level <= 1 && m_isFly)
				{
					End();
				}
			}
		}

		if (collision.tag == "Attack")
		{
			var other = collision.GetComponentInParent<AttackController>();
			if (m_PlayerID != other.m_PlayerID)
			{
				if (m_Level <= other.m_Level && m_isFly)
				{
					End();
				}
				if (m_Level >= other.m_Level && !m_isFly)
				{
					if (other.m_isFly)
					{
						m_Player?.HitAtokin();
					}
					else
					{
						m_Player?.HitAttack();
					}
				}
			}
		}

		if (collision.tag == "Wall")
		{
			if (m_isFly && m_Level < 2)
			{
				End();
			}
			else if (!m_isFly)
			{
				m_Player?.HitAttack();
			}
		}
	}
}
