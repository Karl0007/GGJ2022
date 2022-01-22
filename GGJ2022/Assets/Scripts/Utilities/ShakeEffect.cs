using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//镜头抖动
public class ShakeEffect : MonoBehaviour
{
	// Start is called before the first frame update


	public Transform camTransform;    //transform震动对象
	public float shakeTime = 0.7f;    //震动时间
	public bool shakeEffect = false;    //震动标志
	public float shakePower = 1.0f;     //震动强度

	private float timeTool;     //工具计时
	private float RealTime;
	public Vector3 lastShake;      //初始位置

	private void Update()
	{
		if (shakeEffect)
		{
			if (timeTool > 0)
			{
				transform.position -= lastShake;
				lastShake = Random.insideUnitCircle * shakePower;
				//camTransform.localPosition = PlayerManager.Instance.CameraCenter.position + Random.insideUnitSphere * shakePower;
				transform.position += lastShake;
				timeTool -= (Time.realtimeSinceStartup - RealTime);
			}
			else
			{
				shakeEffect = false;
				timeTool = shakeTime;
				//camTransform.localPosition = originPos;
			}
		}
		RealTime = Time.realtimeSinceStartup;
	}

	//提供开始的接口：time表示shake时间,默认是0.7
	public ShakeEffect BeginShake(float time = 0.7f,float power = 1f)
	{
		lastShake = Vector3.zero;
		RealTime = Time.realtimeSinceStartup;
		shakeTime = time;
		timeTool = time;
		shakePower = power;
		shakeEffect = true;
		return this;
	}
	//设置震动力度
	public void SetShakePower(float power)
	{
		shakePower = power;
	}
	//设置震动的时间
	public void SetShakeTime(float time)
	{
		shakeTime = time;
	}
}