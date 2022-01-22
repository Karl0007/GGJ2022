using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	private void Awake()
	{
		#region 单例初始化
		//初始化
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		//Main 物体 跨场景
		DontDestroyOnLoad(gameObject);
		#endregion
	}
}
