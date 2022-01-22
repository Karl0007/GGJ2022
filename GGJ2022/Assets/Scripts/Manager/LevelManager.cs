using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance { get; private set; }

	public Transform leftUp;
	public Transform rightDown;
	public Rect levelRect => new Rect(leftUp.transform.position.x, rightDown.transform.position.y, rightDown.transform.position.x - leftUp.transform.position.x, -rightDown.transform.position.y + leftUp.transform.position.y);

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
