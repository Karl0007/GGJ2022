using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class StopwatchCache<T>
{
	T defaultFalse;
	T cache;
	int limitTime;
	Stopwatch stopwatch;

	public T Value
	{
		get
		{
			if (stopwatch.Elapsed.Milliseconds >= limitTime)
			{
				return defaultFalse;
			}
			else
			{
				return cache;
			}
		}

		set
		{
			stopwatch.Restart();
			cache = value;
		}
	}

	public void setLimitTime(int time)
	{
		limitTime = time;
	}

	public void Clear()
	{
		cache = defaultFalse;
	}

	public StopwatchCache(T defFalse, int limit)
	{
		stopwatch = new Stopwatch();
		cache = defaultFalse = defFalse;
		limitTime = limit;
	}

	public int getTime()
	{
		return stopwatch.Elapsed.Milliseconds;
	}
}