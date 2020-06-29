using JX;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cn.sharesdk.unity3d;
using UnityEngine.SceneManagement;

public class WXLoad
{
    public int phone;
    public int smscode;
    public string code;
}
public class TestWXLink : MonoBehaviour {

    private ShareSDK ssdk;
    private string WechatCode;
    void Start()
    {
        ssdk = GameObject.Find("QiHuiReceiveObj").GetComponent<ShareSDK>();
        ssdk.authHandler += AuthResultHandler;
        //ssdk.shareHandler += OnShareResultHandler;
        ssdk.showUserHandler += OnGetUserInfoResultHandler;
       // ssdk.getFriendsHandler += OnGetFriendsResultHandler;
        //ssdk.followFriendHandler += OnFollowFriendResultHandler;
       
    }
    public void OnBtnClickHandler()
    {
        ssdk.Authorize(PlatformType.WeChat);
    }   
    void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        //Prefabs.PopBubble("授权成功");
        if (state == ResponseState.Success)
        {
            Prefabs.PopBubble("授权成功");
            ssdk.GetUserInfo(type);
        }
        else if (state == ResponseState.Fail)
        {
            print("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }
    void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            string json = MiniJSON.jsonEncode(result);
            string url = "http://" + Bridge.GetHostAndPort() + "/api/member/signIn/wechat";
            UserId.WeChatData = json;
            Debug.Log(json);
            HttpCallSever.One().PostCallServer(url, json, WXVCallBack);
            print("get user info result :");
            print(MiniJSON.jsonEncode(result));
            print("AuthInfo:" + MiniJSON.jsonEncode(ssdk.GetAuthInfo(PlatformType.QQ)));
            print("Get userInfo success !Platform :" + type);
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }
    public class WeChatDataInfo
    {
        public Friend data;

        public int code;
        public string message;

        public class Friend
        {
            public Data member;
            public string token;
        }
        public class Data
        {
            public int gameCount;
            public int gender;
            public string avatar;
            public int IsIdTrue;
            public int gold;
            public string lastLoginTimeStr;
            public string lastLoginTime;
            public string phone;
            public string createTime;
            public string nickname;
            public int headFrame;
            public int id;
            public string createTimeStr;
            public Data() { }
        }
    }
    void WXVCallBack(string edata)
    {
        WeChatDataInfo d =JsonUtility.FromJson<WeChatDataInfo>(edata);
        Debug.Log("===============12312222222222222222222222"+ edata);
       
        if (d.code == 200)
        {
            UserId.NeedWXCallBackData = edata;
            Debug.Log("===============WXCallBack_code200");
            SceneManager.LoadSceneAsync("liang");
            // LoadManager.Instance.LoadScene("liang");
           // Bridge._instance.loginCallBack(edata,true);
            // Bridge._instance.LoadAbDate(LoadAb.Login, "loadbar");
        }
        else if (d.code == 400)
        {
            Prefabs.PopBubble("请您先邦定手机号");
            
            Invoke("loadPhone", 1);
        }
        else if (d.code == 500)
        {
            Prefabs.PopBubble("Mistake+++=======:");
        }
    }
    public void loadPhone()
    {
        Bridge._instance.LoadAbDate(LoadAb.Login, "ZhuChe");
    }
    void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("share successfully - share result :");
            print(MiniJSON.jsonEncode(result));
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }

    void OnGetFriendsResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("get friend list result :");
            print(MiniJSON.jsonEncode(result));
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }

    void OnFollowFriendResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("Follow friend successfully !");
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }
}
