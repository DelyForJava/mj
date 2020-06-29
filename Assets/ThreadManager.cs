
using System.Threading;

using System;
using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
	public static ThreadManager Current;
	public List<Action<string>> actions = new List<Action<string>>();
	private static Thread mainThread;
	public static string errorData;
	public static string jsondata;
	public static bool isMainThread()
	{
		return Thread.CurrentThread == mainThread;
	}
	void Awake()
	{
		Current = this;
		mainThread = Thread.CurrentThread;
	}
	void Update()
	{
		var currentActions = new List<Action<string>>();
		lock (actions)
		{
			currentActions.AddRange(actions);
			foreach (var item in currentActions)
			{
				actions.Remove(item);
			}
		}

		foreach (var action in currentActions)
		{
	
				action(jsondata);
			
		}
	}
	public static void QueueOnMainThread(Action<string> action)
	{
		if (isMainThread())
		{	
			action(jsondata);
			return;
		}
		lock (Current.actions)
		{
			Current.actions.Add(action);
		}
	}
	public static void QueueOnThreadPool(WaitCallback callback, object state = null)
	{
		ThreadPool.QueueUserWorkItem(callback, state);

	}
}
