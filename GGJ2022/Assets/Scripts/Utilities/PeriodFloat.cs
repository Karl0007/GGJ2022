using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodFloat : MonoBehaviour
{
    private int On;
    public float Period;
    public System.Func<float, float> SetFunc;
    private float Timer;
    private float LastTime;
    private float st, ed;

	private void Awake()
	{
		On = 0;
	}

    public bool isOn => On > 0;

	public PeriodFloat SetUp(System.Func<float, float> fun , float p,float s = 0,float e = 1)
	{
        st = s;
        ed = e;
        SetFunc = fun;
        Period = p;
        return this;
	}

    public PeriodFloat SetUp(bool on,float now = 0)
	{
        On += on ? 1 : -1;
        if (on && On == 1) { 
            Timer = 0; 
            LastTime = Time.realtimeSinceStartup;
        }
        On = Mathf.Max(On, 0);
        SetFunc?.Invoke(now);
        return this;
	}

	public void Update()
	{

        if (On > 0)
        {
            Timer += Time.realtimeSinceStartup - LastTime;
            LastTime = Time.realtimeSinceStartup;
            if (Timer > Period)
			{
				(st, ed) = (ed, st);
                Timer = 0;
			}
			SetFunc?.Invoke(Mathf.Lerp(st, ed, Timer / Period));
        }

    }
}
