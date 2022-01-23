using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager Instance { get; private set; }

	public int m_PlayerNum;
	public List<InputConfig> m_InputConfgs;
	public List<Color> m_Colors = new List<Color> { Color.white, Color.cyan };
	public List<GameObject> m_Players;
	public Dictionary<int, int> m_PlayerScore = new Dictionary<int, int>();
	public List<Text> m_Board;

	public float minSize = 6;
	public float maxSize = 50;

	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			foreach (var item in m_PlayerScore.Keys)
			{
				m_PlayerScore[item] = 1;
			}
		}
	}

	private void Start()
	{
		StartGame();
	}

	public GameObject Creat(int id)
	{
		var newList = new List<GameObject>();
		foreach (var item in m_Players)
		{
			if (item != null && item.GetComponent<PlayerController>().ID != id)
			{
				newList.Add(item);
			}
			else
			{
				Destroy(item);
			}
		}
		m_Players = newList;

		var player = Instantiate(ResourcesManager.Instance.Get(ResourcesManager.PrefabsName.Player)).GetComponent<PlayerController>();
		player.ID = id;
		player.GetComponentInChildren<SpriteRenderer>().color = m_Colors[id];
		player.m_InputConfig = m_InputConfgs[id];
		m_Players.Add(player.gameObject);
		player.transform.position = LevelManager.Instance.GetPlayerPoint().position;

		return player.gameObject;
	}

	public void StartGame()
	{
		for (int i=0;i< m_PlayerNum; i++)
		{
			m_PlayerScore[i] = 1;
			Creat(i);
			m_Board[i].text = "1";
		}
	}

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

	public void OnAttack(int attack,int beAttack,bool flyAttack)
	{
		m_PlayerScore[attack]++;
		if (flyAttack)
		{
			m_PlayerScore[attack]++;
		}
		m_Board[attack].text = m_PlayerScore[attack].ToString();
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
