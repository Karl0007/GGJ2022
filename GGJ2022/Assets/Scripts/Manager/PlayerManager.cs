using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager Instance { get; private set; }

	public List<GameObject> m_Players;
	public Dictionary<int, GameObject> m_PlayersMap;

	public float minSize = 6;
	public float maxSize = 50;

	public Vector3 CameraCenter()
	{
		var res = Vector3.zero;
		var cnt = 0;
		for(int i = 0; i < m_Players.Count; i++)
		{
			if (m_Players[i] != null)
			{
				res += m_Players[i].transform.position;
				cnt++;
			}
		}
		return res / cnt;
	}

	public float CameraSize(float WHScale)
	{
		Vector2 min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
		Vector2 max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
		for (int i = 0; i < m_Players.Count; i++)
		{
			if (m_Players[i] != null)
			{
				min.x = Mathf.Min(m_Players[i].transform.position.x, min.x);
				max.x = Mathf.Max(m_Players[i].transform.position.x, max.x);
				min.y = Mathf.Min(m_Players[i].transform.position.y, min.y);
				max.y = Mathf.Max(m_Players[i].transform.position.y, max.y);
			}
		}
		max -= min;
		return Mathf.Clamp(Mathf.Max(max.x / WHScale, max.y) / 1.4f, minSize, maxSize);
	}

	public void OnAttack(int attack,int beAttack)
	{

	}

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
