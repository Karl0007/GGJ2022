using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
	public Collider2D m_Small;
	public Collider2D m_Big;
	public SpriteRenderer m_SpriteRenderer;
	public Animator m_Animator;
	public PlayerController m_Follow;
	public AtokinEnergyFactory m_Factory;
	public float m_FlyTime;
	Vector3 m_CurVelocity;

	private void Awake()
	{
		m_Follow = null;
		m_SpriteRenderer.transform.localScale *= Random.Range(0.7f, 1.5f);
		m_Animator.speed *= Random.Range(0.7f, 1.5f);
		m_CurVelocity = Vector3.zero;
	}

	public void Collect(PlayerController player)
	{
		m_Follow = player;
		m_Follow.GetAtokinEnergy();
	}

	private void FixedUpdate()
	{ 
		if (m_Follow != null)
		{
			transform.position = Vector3.SmoothDamp(transform.position, m_Follow.transform.position, ref m_CurVelocity, m_FlyTime, 100, Time.fixedDeltaTime);
			m_SpriteRenderer.transform.localScale *= 0.88f;
			if (Vector3.Distance(m_Follow.transform.position, transform.position) < 0.2f)
			{
				Destroy(this.gameObject);
				m_Factory.OnEnergyDestroy();
			}
		}

	}
}
