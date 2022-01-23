using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance { get; private set; }

	public float EnergyInterval;
	public Transform leftUp;
	public Transform rightDown;
	public Rect levelRect => new Rect(leftUp.transform.position.x, rightDown.transform.position.y, rightDown.transform.position.x - leftUp.transform.position.x, -rightDown.transform.position.y + leftUp.transform.position.y);
	public List<Transform> playerPoints = new List<Transform>();
	private int m_lastRandom;

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

	private void Start()
	{
		InitFactory();
	}

	public Transform GetPlayerPoint()
	{
		var random = UnityEngine.Random.Range(0, playerPoints.Count);
		while(random == m_lastRandom)
		{
			random = UnityEngine.Random.Range(0, playerPoints.Count);
		}
		m_lastRandom = random;
		return playerPoints[random];
	}

	private void InitFactory()
	{
		var rect = levelRect;
		for (float x = rect.x; x < rect.xMax; x += EnergyInterval)
		{
			for (float y = rect.y; y< rect.yMax;y+= EnergyInterval)
			{
				Instantiate(ResourcesManager.Instance.Get(ResourcesManager.PrefabsName.AtokinEnergyFactory), new Vector3(x, y, 0), Quaternion.identity, transform).GetComponent<AtokinEnergyFactory>().Creat();
			}
		}
	}
}
