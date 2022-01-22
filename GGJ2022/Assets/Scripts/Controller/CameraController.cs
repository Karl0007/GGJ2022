using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float FollowTime;
	public float ZoomTime;
	public float targetSize;
	public float baseSize;
	public float CameraSize => targetSize + baseSize;
	private float tempSize;
	private float WHScale;
	private float RealTime;
	private Vector3 tmpV;
	public float CameraWidth => Camera.main.orthographicSize * WHScale;
	public float CameraHeight => Camera.main.orthographicSize;


	private FollowController follow;

	public void Focus(Transform o, float t)
	{
		Destroy(follow);
		follow.SetUp(o, t);
	}

	public float SetSize(float sz, float time = 0.2f)
	{
		targetSize = sz;
		ZoomTime = time;
		tempSize = 0;
		return targetSize;
	}

	public void SetFollowTime(float time)
	{
		FollowTime = time;
	}

	public void ResetCameraToPlayer()
	{
		Camera.main.orthographicSize = targetSize = 6;
		Camera.main.transform.position = PlayerManager.Instance.CameraCenter;
		tmpV = Vector3.zero;
		tempSize = 0;
	}

	private void Start()
	{
		baseSize = 0;
		WHScale = 1.0f * Screen.width / Screen.height;
	}

	private void Update()
	{
		float delta = Time.unscaledDeltaTime;
		Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, CameraSize, ref tempSize, ZoomTime, Mathf.Infinity, delta);
		var v = Vector3.SmoothDamp(transform.position, PlayerManager.Instance.CameraCenter, ref tmpV, FollowTime, Mathf.Infinity, delta);
		v.z = -10;
		Camera.main.transform.position = v;
	}
}

