using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager Instance { get; private set; }

	public GameObject player;
	public Vector3 CameraCenter => player.gameObject.transform.position;

	private void Awake()
	{
		#region ������ʼ��
		//��ʼ��
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		//Main ���� �糡��
		DontDestroyOnLoad(gameObject);
		#endregion
	}
}
