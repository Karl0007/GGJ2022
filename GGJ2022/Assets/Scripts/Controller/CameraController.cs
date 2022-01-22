using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float FollowTime;
	public float ZoomTime;
	public float targetSize;
	private float tempSize;
	private float WHScale;
	private float RealTime;
	private Vector3 tmpV;
	public float CameraWidth => Camera.main.orthographicSize * WHScale;
	public float CameraHeight => Camera.main.orthographicSize;
	public ShakeEffect shakeEffect;


	public void ResetCameraToPlayer()
	{
		Camera.main.orthographicSize = targetSize = 6;
		Camera.main.transform.position = PlayerManager.Instance.CameraCenter();
		tmpV = Vector3.zero;
		tempSize = 0;
	}

	public Vector2 rectContain(Rect big,Rect small)
	{
		if (small.x < big.x)
		{
			small.center += new Vector2(big.x - small.x, 0);
		}
		if (small.xMax > big.xMax)
		{
			small.center += new Vector2(big.xMax - small.xMax, 0);
		}
		if (small.y < big.y)
		{
			small.center += new Vector2(0, big.y - small.y);
		}
		if (small.yMax > big.yMax)
		{
			small.center += new Vector2(0, big.yMax - small.yMax);
		}
		return small.center;
	}

	private Vector2 GetCenter()
	{
		var rect = new Rect(Vector2.zero,new Vector2(targetSize * 2 * WHScale, targetSize * 2));
		rect.center = PlayerManager.Instance.CameraCenter();
		return rectContain(LevelManager.Instance.levelRect, rect);
	}

	public void Shake(float time = 0.7f, float scale = 1)
	{
		shakeEffect.BeginShake(time, scale);
	}

	private void Start()
	{
		WHScale = 1.0f * Screen.width / Screen.height;
	}

	private void FixedUpdate()
	{
		float delta = Time.unscaledDeltaTime;
		targetSize = PlayerManager.Instance.CameraSize(WHScale);
		Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, targetSize, ref tempSize, ZoomTime, Mathf.Infinity, delta);
		var v = Vector3.SmoothDamp(transform.position, GetCenter(), ref tmpV, FollowTime, Mathf.Infinity, delta);
		v.z = -10;
		transform.position = v;
	}

}

