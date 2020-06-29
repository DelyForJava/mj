using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using JX;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HotUI:Window
{
    private HotPanel m_HotPanel;
    private float m_SumTime;
    private string tokenurl = "http://"+Bridge.GetHostAndPort()+"/api/member/info";
    private bool hot = false;
    public override void Awake(object param1 = null, object param2 = null, object param3 = null)
    {
        m_HotPanel = GameObject.GetComponent<HotPanel>();
        HotPatchManager.Instance.ServerInfoError += ServerInfoError;
        HotPatchManager.Instance.ItemError += ItemError;
        HotPatchManager.Instance.HotApk += HotApk;
        HotFix();
    }
    private void HotApk()
    {
        GameStart.OpenCommonConfirm("新版安装包下载成功", "请前往文件管理安装！！", () => {

            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

            jo.Call("installApk", PathInfo.DownLoadPath + "/tianhu.apk", "com.yfkj.majiangshang");

        }, Application.Quit); ;
    }
    //资源下载失败
    private void ItemError(string all)
    {
        GameStart.OpenCommonConfirm("资源下载失败", string.Format("{0}个资源下载失败或资源丢失，请重新尝试下载！", all), AnewDownload, Application.Quit);
    }
    //服务器列表获取失败
    private void ServerInfoError()
    {
        GameStart.OpenCommonConfirm("服务器列表获取失败", "服务器列表获取失败，请检查网络链接是否正常？尝试重新下载！", CheckVersion, Application.Quit);
    }
    //热更确定
    private void CheckVersion()
    {
        HotPatchManager.Instance.CheckVersion((hot) =>
        {
            if (hot)
            {
                GameStart.OpenCommonConfirm("热更确定", string.Format("当前版本为{0},有需要下载的热更包，是否确定下载？", HotPatchManager.Instance.HotVersion), OnClickStartDownLoad, OnClickCancleDownLoad);
                //提示玩家是否确定热更下载
                //GameStart.OpenCommonConfirm("热更确定", string.Format("当前版本为{0},有{1:F}M大小热更包，是否确定下载？", HotPatchManager.Instance.HotVersion, HotPatchManager.Instance.LoadSumSize / 1024.0f), OnClickStartDownLoad, OnClickCancleDownLoad);
            }
            else
            {
                StartOnFinish();
            }
        });
    }

    void OnClickCancleDownLoad()
    {
        Application.Quit();
    }
    //下载确定
    void OnClickStartDownLoad()
    {

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                GameStart.OpenCommonConfirm("下载确认", "当前使用的是手机流量，是否继续下载？", StartDownLoad, OnClickCancleDownLoad);
            }
            else if(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                StartDownLoad();
            }
        }
        else
        {
            StartDownLoad();
        }
    }
    //下载完成
    void StartOnFinish()
    {
        
        if (HotPatchManager.Instance.VerifyLoaderAsset() != null&&HotPatchManager.Instance.VerifyLoaderAsset().Count>0)
        {
            ItemError(HotPatchManager.Instance.VerifyLoaderAsset().Count.ToString());
        }
        else
        {
            Debug.Log("++++++++++++++++++++++++++Ttttttttttttttttttt");
            GameStart.Instance.StartCoroutine(OnFinish());
        }
    }
    //下载完成执行协成
    IEnumerator OnFinish()
    {
        Debug.Log("++++++++++++++++++++++++++UUUUUUUUUUUUUU");
        //加载场景
        m_HotPanel.hit.text = "正在检测版本更新......";
       // yield return new WaitForSeconds(2);
        yield return new WaitForSeconds(1);
        //GameMapManager.Instance.LoadScene("loadsceen");

        Debug.Log("+Token" + Bridge._instance.token);
        if (string.IsNullOrEmpty(Bridge._instance.token)) { 
            Debug.Log("++++++++++++++++++++++++++zzzzzzzzzzzzzzzzz");
            SceneManager.LoadSceneAsync("loadsceen");
        }
        else { 
            Debug.Log("++++++++++++++++++++++++++VVVVVVVVVVVVVVVVVVVVVVV");
        HttpCallSever.One().PostCallServer(tokenurl,"", PhoneLoadCallBack);
        }

        //GameMapManager.Instance.LoadScene("loadsceen");
        yield return new WaitForSeconds(2);
        
        UIManager.Instance.CloseWnd(PathInfo.HotFixPanel,false,true);
    
    }
    //登录检测
    void PhoneLoadCallBack(string json)
    {
        JsonData data = JsonMapper.ToObject(json);
        if ((int)data["code"] == 200)
        {
            Bridge._instance.loginCallBack(data,false);
        }
        else
        {
            SceneManager.LoadSceneAsync("loadsceen");
            Prefabs.PopBubble("登录信息已过期，请重新登录");
        }
    }
    //资源下载失败  执行二次下载
    void AnewDownload()
    {
        HotPatchManager.Instance.CheckVersion((hot) =>
        {
            if (hot)
            {
                StartDownLoad();
            }
            else
            {
                StartOnFinish();
            }
        });
    }
    //开始下载
    private void StartDownLoad()
    {
        hot = true;
        m_HotPanel.hit.text = "下载中...";
        m_HotPanel.hotFixHit.SetActive(true);
        m_HotPanel.hotText.text = HotPatchManager.Instance.CurrentPatches.Des;
        GameStart.Instance.StartCoroutine(HotPatchManager.Instance.StartDownLoadAB(StartOnFinish));

    }

    public override void OnClose()
    {
        HotPatchManager.Instance.ServerInfoError -= ServerInfoError;
        HotPatchManager.Instance.ItemError -= ItemError;
       
    }
    //热更新网络检测
    void HotFix()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //提示网络错误，检测网络链接是否正常
            GameStart.OpenCommonConfirm("网络链接失败", "网络链接失败，请检查网络链接是否正常？", () =>
            { Application.Quit(); }, () => { Application.Quit(); });
        }
        else
        {
            CheckVersion();
        }
    }
   
    public override void OnUpdate()
    {
        if (HotPatchManager.Instance.StartDownload)
        {
            m_SumTime += Time.deltaTime;
            m_HotPanel.loader.fillAmount = HotPatchManager.Instance.GetProgress();
            float speed = HotPatchManager.Instance.GetLoadSize()/1024f;
            m_HotPanel.loaderSpeed.text = string.Empty;
        }
    }

}