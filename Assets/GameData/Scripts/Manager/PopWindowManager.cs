using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class PopWindowManager : Singleton<PopWindowManager>
{
    private bool isFinished = false;
    private List<string> windows = new List<string>();

    public void Enqueue(string name)
    {
        if(windows.Contains(name))
        {
            windows.Remove(name);
        }
        windows.Add(name);
    }

    public string Dequeue()
    {
        string ret = null;
        if(windows.Count<=0)
        {
            YouFu.Debug.Log("no window to pop");
            return ret;
        }
        ret = windows[0];
        windows.Remove(ret);
        return ret;
    }



    public void Init()
    {
        EventManager.Instance.AddListener(EventManager.LoginToMain, LoginToMain);
        //EventManager.Instance.Brocast(EventManager.LoginToMain);
        //Enqueue("SignWindow");
        //Enqueue("ActivityWindow");
    }

    public void Step()
    {
        if (isFinished)
            return;

        var windowName = Dequeue();
        if (string.IsNullOrEmpty(windowName))
            return;

        if (windowName == "SignWindow")
        {
            Bridge._instance.LoadAbDate(LoadAb.MainTwo, "Qiandao");

        }
        else if (windowName == "ActivityWindow")
        {
            Bridge._instance.LoadAbDate(LoadAb.Main, "houdong");
        }
        if (windows.Count <= 0)
            isFinished = true;
    }

    private void LoginToMain(params object[] objs)
    {
        //YouFu.Debug.Log("PopWindow LoginToMain");
        //Step();
    }

    public void Regist(string name)
    {
        if (isFinished)
            return;

        Enqueue(name);
    }

}