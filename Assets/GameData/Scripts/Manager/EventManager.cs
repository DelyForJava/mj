using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public delegate void Callback(params object[] objs);
public class EventManager : Singleton<EventManager>
{
    private Dictionary<string, List<Callback>> map = new Dictionary<string, List<Callback>>();
    public void AddListener(string name, Callback cb)
    {
        if (string.IsNullOrEmpty(name) || cb == null)
        {
            YouFu.Debug.Log("EventManager AddListener failed,the name IsNullOrEmpty or the listener to add is null");
            return;
        }

        List<Callback> cbs = null;
        if (map.ContainsKey(name))
        {
            cbs = map[name];
        }
        else
        {
            cbs = new List<Callback>();
            map.Add(name,cbs);
        }
        cbs.Add(cb);
    }

    public void RemoveListener(string name, Callback cb)
    {
        if (string.IsNullOrEmpty(name) || cb == null)
        {
            YouFu.Debug.Log("EventManager RemoveListener failed,the name IsNullOrEmpty or the listener to add is null");
            return;
        }

        if (!map.ContainsKey(name))
        {
            YouFu.Debug.Log("EventManager RemoveListener failed,the name is already removed");
            return;
        }
        var cbs = map[name];
        if (!cbs.Contains(cb))
        {
            YouFu.Debug.Log("EventManager RemoveListener failed,the callback is already removed");
            return;
        }
        cbs.Remove(cb);
    }

    public void RemoveListener(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            YouFu.Debug.Log("EventManager RemoveListener failed,the name IsNullOrEmpty");
            return;
        }

        if (!map.ContainsKey(name))
        {
            YouFu.Debug.Log("EventManager RemoveListener failed,the name is already removed");
            return;
        }
        map.Remove(name);
    }

    public void Brocast(string name,params object[] objs)
    {
        if (string.IsNullOrEmpty(name))
        {
            YouFu.Debug.Log("EventManager Brocast failed,the name IsNullOrEmpty");
            return;
        }

        if (!map.ContainsKey(name))
        {
            YouFu.Debug.Log("EventManager Brocast failed,the name to brocast is not exist");
            return;
        }
        var cbs = map[name];
        foreach (var cb in cbs)
        {
            cb(objs);
        }

    }

    public static string LoginToMain = "LoginToMain";

}