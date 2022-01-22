using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowController : MonoBehaviour
{
	Transform obj;
	Vector3 Target;
	float time;
	Vector3 temp;
	float z;

	public FollowController SetUp(Vector3 v, float t)
	{
		Target = v;
		time = t;
		z = transform.position.z;
		return this;
	}

	public FollowController SetUp(Transform o, float t)
	{
		obj = o;
		time = t;
		z = transform.position.z;
		return this;
	}

	private void Update()
	{
		var v = obj ? Vector3.SmoothDamp(transform.position, obj.position, ref temp, time) : Vector3.SmoothDamp(transform.position, Target, ref temp, time);
		v.z = z;
		transform.position = v;
	}
}
