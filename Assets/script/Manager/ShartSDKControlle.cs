using cn.sharesdk.unity3d;
using JX;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShartSDKControlle : MonoBehaviour {
	public static ShartSDKControlle Instance;
	private ShareSDK sharesdk;
    private void Awake()
	{
		//Instance = this;//this:当前代码组建对象
		Instance = gameObject.GetComponent<ShartSDKControlle>();
        sharesdk = GameObject.Find("QiHuiReceiveObj").GetComponent<ShareSDK>();
        //sharesdk.authHandler += OnAuthResultHandler;
        //sharesdk.showUserHandler += OnGetUserInfoResultHandler;
        sharesdk.shareHandler += ShareResultHandler;
        DontDestroyOnLoad(gameObject);
	}
	// Use this for initialization
    public void Load()
    {
        //sharesdk.Authorize(PlatformType.WeChat);
    }
    void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        
        if (state == ResponseState.Success)
        {
            sharesdk.GetUserInfo(type);
            // GameObject.Find("MessageText").GetComponent<Text>().text = "授权成功";
        }
        else if (state == ResponseState.Fail)
        {
            Prefabs.PopBubble("登入失败");
        }
        else if (state == ResponseState.Cancel)
        {
            Prefabs.PopBubble("取消了微信登入");
            print("cancel !");
        }
    }
    void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            switch (type)
            {
                case PlatformType.WeChat:
                    if (sharesdk.IsAuthorized(PlatformType.WeChat))
                    {
                        string json = MiniJSON.jsonEncode(result);
                        UserId.WeChatData = json;
                        string url = "http://" + Bridge.GetHostAndPort() + "/api/member/signIn/wechat";
                        HttpCallSever.One().PostCallServer(url, json, WXVCallBack);
                    }
                    // GameObject.Find("MessageText").GetComponent<Text>().text = "获取用户信息成功";
                    //SceneManager.LoadSceneAsync("MainScene");
                    break;
            }
        }
        else if (state == ResponseState.Fail)
        {
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
            Prefabs.PopBubble("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
            Prefabs.PopBubble("cancel!");
        }
        //if (state == ResponseState.Success)
        //{
        //    print("get user info result :");
        //    print(MiniJSON.jsonEncode(result));
        //    GameObject.Find("MessageText").GetComponent<Text>().text = ("AuthInfo:" + MiniJSON.jsonEncode(sharesdk.GetAuthInfo(PlatformType.WeChat)));
        //    print("Get userInfo success !Platform :" + type);
        //}
        //else if (state == ResponseState.Fail)
        //{
        //    #if UNITY_ANDROID
        //                print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
        //    #elif UNITY_IPHONE
        //                print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
        //    #endif
        //}
        //else if (state == ResponseState.Cancel)
        //{
        //    print("cancel !");
        //}
    }

    void roomIdMethod(string code)
    {
        UserId.GetEntableAction = true;
        UserId.GetData = code;
        Prefabs.PopBubble(code);
    }
    class Phone_
    {
       public string user_number;
    }
    void extraMethod(string code)
    {
        Phone_ phone = JsonUtility.FromJson<Phone_>(code);
        UserId.QiHuiPhone = phone.user_number;
       // Prefabs.PopBubble("USerID" + UserId.QiHuiPhone);
        Debug.Log(UserId.QiHuiPhone);
    }
    //public void WXLogin()
    //{
    //    // 获取 Android 对象
    //    jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    //    jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    //    // 通过 Android 对象调用 WechatLogin 方法，启动微信登入
    //    jo.Call("getRoomId");
    //}
    public class WeChatDataInfo
    {
        public Friend data;

        public int code;
        public string message;

        public class Friend
        {
            public Data member;
        }
        public class Data
        {
            public int id;
            public int gold;
            public int status;
            public string nickname;
            public string username;
            public string password;
            public string phone;
            public string avatar;
            public string createTime;
            public string lastLoginTime;
            public Data() { }
        }
    }
    public void WXVCallBack(string edata)
    {
        WeChatDataInfo d = JsonUtility.FromJson<WeChatDataInfo>(edata);
        if (d.code == 200)
        {
            Prefabs.PopBubble(edata);
             LoadManager.Instance.LoadScene("liang");
            Bridge._instance.LoadAbDate(LoadAb.Login, "loadbar");
            //GameMapManager.Instance.NormalLoadScene("liang");
        }
        else if (d.code == 400)
        {
            Prefabs.PopBubble("请您先邦定手机号");
            Debug.Log(UserId.WeChatData);
            
            Bridge._instance.LoadAbDate(LoadAb.Login, "ZhuChe");
        }
        else if (d.code == 500)
        {
            Prefabs.PopBubble("Mistake+++=======:");
        }
    }
    /// <summary>
    /// 获取截图分享朋友
    /// data为分享文本
    /// </summary>
    public void ShateImage()
    {
        StartCoroutine(GetScreenshot());
        ShareContent content = new ShareContent();
        content.SetTitle("天胡十三浪");//title标题，印象笔记、邮箱、信息、微信、人人网和QQ空间使用
        content.SetImagePath(Application.persistentDataPath + "/" + "Screenshot.jpg"); //imagePath是图片的本地路径，Linked - In以外的平台都支持此参数, 使用 imagePath 必须保证SDcard下面存在此张图片
       //content.SetImageUrl("http://" + Bridge.GetHostAndPort() +"/upload/up/Sky13wave.png");//imagePath与imageUrl 必须保留一个，否则微信不能分享
        content.SetText("全新的体验，好玩的上饶麻将");//text是分享文本，所有平台都需要这个字段
        content.SetUrl("http://47.92.172.214:8080/page/share/wechat/1");
        //url仅在微信（包括好友和朋友圈）中使用
        //content.SetTitleUrl("");// titleUrl是标题的网络链接，仅在人人网和QQ空间使用
        //content.SetComment("我是测试评论文本");    // comment是我对这条分享的评论，仅在人人网和QQ空间使用
        //content.SetSite("");  // site是分享此内容的网站名称，仅在QQ空间使用
        //content.SetSiteUrl("");  // siteUrl是分享此内容的网站地址，仅在QQ空间使用
        content.SetShareType(ContentType.Image);
        sharesdk.ShareContent(PlatformType.WeChat, content);
    }
    public void SharteWebPage()
    {
        ShareContent content = new ShareContent();
        content.SetTitle(" 天胡十三浪 ");
        content.SetImageUrl("http://" + Bridge.GetHostAndPort() +"/upload/up/Sky13wave.png");
        //content.SetTitleUrl("https://www.baidu.com/");
        content.SetText(" 全新的体验，好玩的上饶麻将 ");
       //content.SetSite("Mob-ShareSDK");
      // content.SetSiteUrl("https://www.baidu.com/");
        content.SetUrl("http://" + Bridge.GetHostAndPort() +"/page/share/wechat/0");
       // content.SetComment("test description");
        //content.SetMusicUrl("http://fjdx.sc.chinaz.com/Files/DownLoad/sound1/201807/10300.mp3");
        content.SetShareType(ContentType.Webpage);
        sharesdk.ShareContent(PlatformType.WeChat, content);
       // sharesdk.ShowPlatformList(null, content, 100, 100);
    }
    /// <summary>
    /// 邀请好友
    /// </summary>
    /// <param name="URL"></param>
    public void SharteWebPage(string URL)
    {
        ShareContent content = new ShareContent();
        //string s = URL.Substring(0, URL.Length);
        string  s = UserId.TableNum.ToString();
        Debug.Log(s);
        content.SetTitle(" 天胡十三浪 ");
        content.SetImageUrl("http://" + Bridge.GetHostAndPort() +"/upload/up/Sky13wave.png");
        content.SetText("come in 房间号 ："+s+"一起愉快的游玩吧！");
        content.SetUrl(URL);
        content.SetShareType(ContentType.Webpage);
        sharesdk.ShareContent(PlatformType.WeChat, content);
    }
    IEnumerator GetScreenshot()
    {
        // 截屏1帧后再呼起微信
        yield return null;
        Debug.Log(111);
        string imgPath = System.IO.Path.Combine(Application.persistentDataPath, "Screenshot.jpg");
        Debug.Log(System.IO.Path.Combine(Application.persistentDataPath, "Screenshot.jpg"));
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
        tex.Apply();
        System.IO.File.WriteAllBytes(imgPath, tex.EncodeToJPG());
        Texture2D texThumb = new Texture2D(250, 250);
        Color[] destPix = new Color[texThumb.width * texThumb.height];
        float warpFactor = 1.0f;
        int y = 0;
        while (y < texThumb.height)
        {
            int x = 0;
            while (x < texThumb.width)
            {
                float xFrac = x * 1.0F / (texThumb.width - 1);
                float yFrac = y * 1.0F / (texThumb.height - 1);
                float warpXFrac = Mathf.Pow(xFrac, warpFactor);
                float warpYFrac = Mathf.Pow(yFrac, warpFactor);
                Color c0 = tex.GetPixelBilinear(warpXFrac, warpYFrac);

                xFrac = (x - 1) * 1.0F / (texThumb.width - 1);
                yFrac = y * 1.0F / (texThumb.height - 1);
                warpXFrac = Mathf.Pow(xFrac, warpFactor);
                warpYFrac = Mathf.Pow(yFrac, warpFactor);
                Color c1 = tex.GetPixelBilinear(warpXFrac, warpYFrac);

                xFrac = (x + 1) * 1.0F / (texThumb.width - 1);
                yFrac = y * 1.0F / (texThumb.height - 1);
                warpXFrac = Mathf.Pow(xFrac, warpFactor);
                warpYFrac = Mathf.Pow(yFrac, warpFactor);
                Color c2 = tex.GetPixelBilinear(warpXFrac, warpYFrac);

                xFrac = (x + 1) * 1.0F / (texThumb.width - 1);
                yFrac = (y - 1) * 1.0F / (texThumb.height - 1);
                warpXFrac = Mathf.Pow(xFrac, warpFactor);
                warpYFrac = Mathf.Pow(yFrac, warpFactor);
                Color c3 = tex.GetPixelBilinear(warpXFrac, warpYFrac);

                xFrac = (x - 0) * 1.0F / (texThumb.width - 1);
                yFrac = (y - 1) * 1.0F / (texThumb.height - 1);
                warpXFrac = Mathf.Pow(xFrac, warpFactor);
                warpYFrac = Mathf.Pow(yFrac, warpFactor);
                Color c4 = tex.GetPixelBilinear(warpXFrac, warpYFrac);

                xFrac = (x - 1) * 1.0F / (texThumb.width - 1);
                yFrac = (y - 1) * 1.0F / (texThumb.height - 1);
                warpXFrac = Mathf.Pow(xFrac, warpFactor);
                warpYFrac = Mathf.Pow(yFrac, warpFactor);
                Color c5 = tex.GetPixelBilinear(warpXFrac, warpYFrac);

                xFrac = (x + 1) * 1.0F / (texThumb.width - 1);
                yFrac = (y + 1) * 1.0F / (texThumb.height - 1);
                warpXFrac = Mathf.Pow(xFrac, warpFactor);
                warpYFrac = Mathf.Pow(yFrac, warpFactor);
                Color c6 = tex.GetPixelBilinear(warpXFrac, warpYFrac);

                xFrac = (x - 0) * 1.0F / (texThumb.width - 1);
                yFrac = (y + 1) * 1.0F / (texThumb.height - 1);
                warpXFrac = Mathf.Pow(xFrac, warpFactor);
                warpYFrac = Mathf.Pow(yFrac, warpFactor);
                Color c7 = tex.GetPixelBilinear(warpXFrac, warpYFrac);

                xFrac = (x - 1) * 1.0F / (texThumb.width - 1);
                yFrac = (y + 1) * 1.0F / (texThumb.height - 1);
                warpXFrac = Mathf.Pow(xFrac, warpFactor);
                warpYFrac = Mathf.Pow(yFrac, warpFactor);
                Color c8 = tex.GetPixelBilinear(warpXFrac, warpYFrac);

                Color cr = c0 * 0.25f + c1 * 0.125f + c2 * 0.125f + c3 * 0.0625f + c4 * 0.125f + c5 * 0.0625f + c6 * 0.0625f + c7 * 0.125f + c8 * 0.0625f;

                destPix[y * texThumb.width + x] = cr;

                x++;
            }
            y++;
        }
        texThumb.SetPixels(destPix);
        texThumb.Apply();
        byte[] bytesThumb = texThumb.EncodeToPNG();
        string imgPath1 = System.IO.Path.Combine(Application.persistentDataPath, "Screenshot.jpg");
        //System.IO.File.WriteAllBytes(imgPath1, texThumb.EncodeToJPG());
    }
    /// <summary>
    /// 邀请朋友圈好友
    /// </summary>
    public void SharteWeChatMoments(string url)
    {
        ShareContent content = new ShareContent();
        //string s = url.Substring(0,url.Length);
        string s = UserId.TableNum.ToString();
        content.SetTitle("天胡十三浪");
        content.SetImageUrl("http://" + Bridge.GetHostAndPort() +"/upload/up/Sky13wave.png");
        content.SetText("你的好友邀请你来游玩麻将在"+ "房间号：" + s+"等你");
        content.SetText("房间号：" + s + "来啊决战到破产！！！");
        content.SetUrl(url);
        content.SetShareType(ContentType.Webpage);
        sharesdk.ShareContent(PlatformType.WeChatMoments, content);
    }
    /// <summary>
    /// 分享到朋友圈
    /// </summary>
    public void SharteWeChatMoments()
    {
        ShareContent content = new ShareContent();
        content.SetTitle("天胡十三浪");
        content.SetImageUrl("http://" + Bridge.GetHostAndPort() +"/upload/up/Sky13wave.png");
        content.SetText("全新的体验，好玩的上饶麻将");
        content.SetUrl("http://47.92.172.214:8080/page/share/wechat/1");
        content.SetShareType(ContentType.Webpage);
        sharesdk.ShareContent(PlatformType.WeChatMoments, content);
    }
    void ShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        switch (state)
        {
            case ResponseState.Success:
                HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/member/share", "{}", (obj) =>
                {
                    JsonData json = JsonMapper.ToObject(obj);
                    if ((int)json["code"] == 200)
                    {
                        UiController._instance.goldCount.text = ((int)json["data"]["totalGold"]).ToString();
                        Debug.Log((int)json["data"]["count"]);
                        UiController._instance.shareCGroup.alpha = (int)json["data"]["count"] == 2 ? 0 : 1;
                        Prefabs.Buoy("分享成功！");
                    }else
                    {
                        Prefabs.Buoy("今天已分享2次！");
                    }
                }); 
                break;
            case ResponseState.Fail:
                print("分享失败");
                break;
            case ResponseState.Cancel:
                print("分享取消");
                break;
        }
    }
    public void CancelLogin()//取消授权
    {
        if (sharesdk.IsAuthorized(PlatformType.WeChat))//IsAuthorized可以判断授权是否有效
        {
            sharesdk.CancelAuthorize(PlatformType.WeChat);
        }
        else
        {
            Prefabs.PopBubble("授权" + sharesdk.IsAuthorized(PlatformType.WeChat));
        }
    }
}
