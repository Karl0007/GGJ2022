using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

	[Header("计时器")]
	public float fixDeltaTime = 0.02f;
	public float timeScale = 1f;
	public float Speed = 1f;

	private List<Coroutine> gameSpeedCoroutine = new List<Coroutine>();
	private Action callBack;
	private PeriodFloat TimeChangePeriod;

	public Material Rope;

	//初始化单例
	public static TimeManager Instance = null;
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

		#region 时间初始化
		Time.timeScale = timeScale;
		Time.fixedDeltaTime = fixDeltaTime;
		#endregion

		TimeChangePeriod = gameObject.AddComponent<PeriodFloat>();
	}

	//时间速度控制
	private void SetTime(float speed)
	{
		Speed = speed;
		Time.timeScale = timeScale * speed;
		//Time.fixedDeltaTime = fixDeltaTime * speed;
	}

	private void SetTime(float speed,float smooth)
	{
		float cur = Speed;
		if (TimeChangePeriod.isOn) TimeChangePeriod.SetUp(false, 1);
		if (smooth > 0) TimeChangePeriod.SetUp((f) => { SetTime(Mathf.Lerp(cur, speed, f)); return f; }, smooth).SetUp(true); else SetTime(speed);
		StartCoroutine(WorkAfterRealTime(() => { if (TimeChangePeriod.isOn) TimeChangePeriod.SetUp(false, 1); }, smooth));
	}

	private void ResetTime()
	{
		Speed = 1f;
		Time.timeScale = timeScale;
		Time.fixedDeltaTime = fixDeltaTime;
		gameSpeedCoroutine.Clear();
	}

	private void BeforeChangedSpeed()
	{
		if (gameSpeedCoroutine.Count != 0)
		{
			foreach (var item in gameSpeedCoroutine)
			{
				StopCoroutine(item);
			}
			callBack?.Invoke();
		}
	}

	public void ChangeGameSpeed(float speed,System.Func<bool> fun)
	{
		SetTime(speed);
		if (gameSpeedCoroutine.Count != 0)
		{
			foreach (var item in gameSpeedCoroutine)
			{
				StopCoroutine(item);
			}
		}
		gameSpeedCoroutine.Add(StartCoroutine(WorkUntil(ResetTime, fun)));
	}

	public void ChangeGameSpeed(float speed,float time,Action fun = null, float smooth = 0)
	{
		BeforeChangedSpeed();
		SetTime(speed, smooth);
		callBack = fun;
		gameSpeedCoroutine.Add(StartCoroutine(WorkAfterRealTime(ResetTime, time)));
		if (fun!=null) gameSpeedCoroutine.Add(StartCoroutine(WorkAfterRealTime(fun, time)));
	}

	public void ChangeGameSpeedWithOutReset(float speed, float smooth = 0)
	{
		BeforeChangedSpeed();
		SetTime(speed, smooth);
		callBack = null;
		gameSpeedCoroutine.Clear();
	}

	public void EStopTime(float time) {
		ChangeGameSpeed(0f, time, null, 0);
	}

	#region 协程函数
	public static IEnumerator WorkAfterTime(Action fun, float time)
	{
		yield return new WaitForSeconds(time);
		fun();
	}

	public static IEnumerator WorkAfterRealTime(Action fun, float time)
	{
		yield return new WaitForSecondsRealtime(time);
		fun();
	}

	public static IEnumerator WorkAfterUpdate(Action fun)
	{
		yield return new WaitForEndOfFrame();
		fun();
	}
	public static IEnumerator WorkUntil(Action fun, System.Func<bool> b)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitUntil(b);
		fun();
	}

	public static IEnumerator RealTimeUpdate(Action fun, Action done, float time, int repeat)
	{
		return RealTimeUpdate(fun, done, time, time / repeat);
	}

	public static IEnumerator RealTimeUpdate(Action fun, Action done, float time, float slice)
	{
		while (time > 0)
		{
			fun();
			time -= slice;
			yield return new WaitForSecondsRealtime(slice);
		}
		done?.Invoke();
	}

	public static IEnumerator RepeatWorkUntil(Action fun, Action done, System.Func<bool> b)
	{
		while (!b())
		{
			yield return new WaitForEndOfFrame();
			fun();
		}
		done?.Invoke();
	}
	#endregion

}
