using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCollectorController : MonoBehaviour
{
	PlayerController m_PlayerController;

	private void Awake()
	{
		m_PlayerController = GetComponentInParent<PlayerController>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision);
		collision.GetComponentInParent<EnergyController>()?.Collect(m_PlayerController);
	}
}
