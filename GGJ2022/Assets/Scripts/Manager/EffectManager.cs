using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
	public static EffectManager Instance = null;
	//初始化单例
	private void Awake()
	{
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
	}

	public void PlayEffect(ResourcesManager.PrefabsName name, Vector3 v, Vector3 a, float t)
	{
		v.z = -5;
		Destroy(Instantiate(ResourcesManager.Instance.Get(name), v, Quaternion.Euler(a)), t);
	}

	public void CameraShake(float time = 0.2f,float power = 0.2f)
	{
		Camera.main.GetComponentInParent<ShakeEffect>().BeginShake(time, power);
	}

	public void TimeStop()
	{
		TimeManager.Instance.EStopTime(0.2f);
	}
}
