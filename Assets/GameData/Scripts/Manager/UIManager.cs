using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class UIManager:Singleton<UIManager>
{
    public Font font;
    public static Delegate Delegate;
    private Transform UI_ROOT;
    public Transform m_WndRoot {
        get { return UI_ROOT; }
    }

    //注册
    public Dictionary<string, System.Type> m_RegisterDic = new Dictionary<string, System.Type>();
    //打开的窗口列表
    private List<Window> m_WindowList=new List<Window>();
    //所有打开的窗口
    private Dictionary<string, Window> m_WindowDic = new Dictionary<string, Window>();
    public void Init(Transform parrent)
    {
        UI_ROOT = parrent;
        font= Resources.Load<Font>("Font/PingFang Bold");
    }

    public void Register<T>(string name) where T : Window
    {
        m_RegisterDic[name] = typeof(T);
    }
    public Window PopUpWnd(string wndName, bool bTop = true, bool resource = false, object param1 = null,object param2 = null,object param3 = null)
    {
        TestInfo.Instance.ShowTxt("UImanager init");
        Window wnd = FindWndByName<Window>(wndName);
        TestInfo.Instance.ShowTxt("sfdgisndfgiusndfigunsdfi" + wnd);
        if (wnd == null)
        {
          
            System.Type tp = null;
            if (m_RegisterDic.TryGetValue(wndName, out tp))
            {
                if (resource)
                {
                    wnd = System.Activator.CreateInstance(tp) as Window;
                }
                else
                {
                    string hotName = "HotFix_Project." + "HotFix"+ wndName.Replace("Panel.prefab", "UI");
                    TestInfo.Instance.ShowTxt("sfdgisndfgiusndfigunsdfi" + hotName);
                    wnd = ILRuntimeManager.Instance.ILRunAppDomain.Instantiate<Window>(hotName);
                    wnd.IsHotFix = true;
                    wnd.HotFixClassName = hotName;
                }
            }
            else
            {
                Debug.LogError("找不到窗口对应的脚本，窗口名是：" + wndName);
                return null;
            }
            GameObject wndObj = null;
            if (resource)
            {
                wndObj = GameObject.Instantiate(Resources.Load<GameObject>(wndName.Replace(".prefab", ""))) as GameObject;
            }
            else
            {
                TestInfo.Instance.ShowTxt("实例化面板");
              //  if (GameStart.Instance.isYYB)
              //  {
                    wndObj = GameObject.Instantiate(ResourceManager.Instance.HotFixLoaderAssetBundle<GameObject>(wndName,false));
              //  }
              //  wndObj =GameObject.Instantiate(ResourceManager.Instance.HotFixLoaderAssetBundle<GameObject>(wndName));
                TestInfo.Instance.ShowTxt("实例化面板成功-----------");
            }
            if (wndObj == null)
            {
                Debug.Log("创建窗口Prefab失败：" + wndName);
                return null;
            }

            if (!m_WindowDic.ContainsKey(wndName))
            {
                m_WindowList.Add(wnd);
                m_WindowDic.Add(wndName, wnd);
            }
            
            wnd.GameObject = wndObj;
            wnd.Transform = wndObj.transform;
            wnd.Name = wndName;
            wndObj.transform.SetParent(m_WndRoot, false);
            if (wnd.IsHotFix)
            {
                ILRuntimeManager.Instance.ILRunAppDomain.Invoke(wnd.HotFixClassName, "Awake", wnd, param1, param2,param3);
            }
            else
            {
                wnd.Awake(param1,param2,param3);
            }

            wnd.Resource = resource;
            if (bTop)
            {
                wndObj.transform.SetAsLastSibling();
            }

            if (wnd.IsHotFix)
            {
                ILRuntimeManager.Instance.ILRunAppDomain.Invoke(wnd.HotFixClassName, "OnShow", wnd, param1,param2,param3);
            }
            else
            {
                wnd.OnShow(param1,param2,param3);
            }
        }
        else
        {
            ShowWnd(wndName, bTop, param1,param2,param3);
        }
        TestInfo.Instance.ShowTxt("fanhui" + wnd);
        return wnd;
    }
    

    private void ShowWnd(string name, bool bTop, object param1,object paraml2, object param3)
    {
        Window wnd = FindWndByName<Window>(name);
        ShowWnd(wnd, bTop, param1,paraml2,param3);
    }
    public void ShowWnd(Window wnd, bool bTop = true, object param1 = null, object param2 = null, object param3=null)
    {
        if (wnd != null)
        {
            if (wnd.GameObject != null && !wnd.GameObject.activeSelf) wnd.GameObject.SetActive(true);
            if (bTop) wnd.Transform.SetAsLastSibling();
            if (wnd.IsHotFix)
            {
                ILRuntimeManager.Instance.ILRunAppDomain.Invoke(wnd.HotFixClassName, "OnShow", wnd, param1,param2,param3);
            }
            else
            {
                wnd.OnShow(param1,param2,param3);
            }
        }
    }

    public void CloseWnd(string windowName,bool isHotFix, bool destory = false)
    {
        Window window = m_WindowDic[windowName];
        if (window != null)
        {
            m_WindowList.Remove(window);

            if (destory)
            {
                if (isHotFix)
                {
                    ILRuntimeManager.Instance.ILRunAppDomain.Invoke(window.HotFixClassName, "OnDisable", window);
                    ILRuntimeManager.Instance.ILRunAppDomain.Invoke(window.HotFixClassName, "OnClose", window);
                }
                else
                {
                    window.OnDisable();
                    window.OnClose();
                }

                m_WindowDic.Remove(window.Name);
                GameObject.Destroy(window.GameObject);
                window.GameObject = null;
            }
            else
            {
                window.GameObject.SetActive(false);
            }
           
        }
    }


    public T FindWndByName<T>(string name) where T : Window
    {
        Window wnd = null;
        if (m_WindowDic.TryGetValue(name, out wnd))
        {
            return (T)wnd;
        }

        return null;
    }

    public void OnUpdate()
    {
        for (int i = 0; i < m_WindowList.Count; i++)
        {
            Window window = m_WindowList[i];
            if (window != null)
            {
                if (window.IsHotFix)
                {
                    ILRuntimeManager.Instance.ILRunAppDomain.Invoke(window.HotFixClassName, "OnUpdate", window);
                }
                else
                {
                    window.OnUpdate();
                }
            }
        }
    }
}

