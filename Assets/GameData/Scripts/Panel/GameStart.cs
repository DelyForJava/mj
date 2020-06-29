using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using JX;
using LitJson;

public class GameStart : MonoSingleton<GameStart>
{
    //用于热更新时执行对应场景的函数
    public static Action MainAction=null;
    public static Action LoadAction = null;
    public static Action LiangAction = null;
    public static Action GoldGroundAction = null;
    public static Action mjGameScreenAction = null;
    public static Action PlaybackGameAction = null;
    public bool isYYB=true;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Application.runInBackground = true;//后台运行
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//禁用休眠
        Application.targetFrameRate = 40;//限制帧率
        Screen.fullScreen = true;//启用全屏

        HotPatchManager.Instance.Init(this);
        UIManager.Instance.Init(GameObject.Find("UIRoot").transform);
        GameMapManager.Instance.Init(this);
        RegisterUI();
        
        PopWindowManager.Instance.Init();
    }
    private void Start()
    {
        ILRuntimeManager.Instance.Init(this);
        if (MainAction != null) MainAction();
#if UNITY_ANDROID
        Handheld.PlayFullScreenMovie("Cartoon.mp4", Color.white, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.AspectFill);
#endif
        StartCoroutine(StartIE());
    }
    IEnumerator StartIE()
    {
        yield return null;
        GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Bulletin"), GameObject.Find("UIRoot").transform);
        yield return new WaitForSeconds(2.0f);
        UIManager.Instance.PopUpWnd(PathInfo.HotFixPanel, resource: true);
        Destroy(obj);
    }
    void Update()
    {
        //执行UI的updata函数
        UIManager.Instance.OnUpdate();
    }
    //打开提示面板
    public static void OpenCommonConfirm(string title, string str, UnityEngine.Events.UnityAction confirmAction, UnityEngine.Events.UnityAction cancleAction)
    {
        GameObject commonObj = GameObject.Instantiate(Resources.Load<GameObject>("CommonConfirm")) as GameObject;
        commonObj.transform.SetParent(GameObject.Find("UIRoot").transform, false);
        CommonConfirm commonItem = commonObj.GetComponent<CommonConfirm>();
        commonItem.Show(title, str, confirmAction, cancleAction);
    }

    //注册UI面板
    void RegisterUI()
    {
        UIManager.Instance.Register<HotUI>(PathInfo.HotFixPanel);
    }
}
