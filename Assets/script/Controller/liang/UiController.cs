using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using JX;
using LitJson;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using ILRuntime.Mono.Cecil.Cil;



/// <summary>
/// yl 2020.4.26
/// </summary>
public class UiController : MonoBehaviour
{
    public CanvasGroup shareCGroup;
    public Button share;
    public Text NickName;
    public Text goldCount;
    public Text num;
    public Text dianment;
    private Button showIcon;
    public Image Icon_Touxiang;

    //有新邮件时候的提示
    private GameObject mailtag;

    //单例
    public static UiController _instance = null;

    //邮件的委托
    public Action<string> _MailAction;

    //企惠银两接口
    private static string diamondurl = "http://" + Bridge.GetHostAndPort() + "/api/member/info/qihui";

    //账号的信息接口    -----------try方法里面还有一个tokenurl的局部变量  不要搞混了
    private string tokenurl = "http://" + Bridge.GetHostAndPort() + "/api/member/info";

    private void Awake()
    {
        _instance = this;
        //微信登录的时候
        WeChatData();
    }

    void Start()
    {
        if (GameStart.LiangAction != null) GameStart.LiangAction();

        //开始长连接
        LoadLineWS.One().StartWS();
        LoadLineWS.One().StopDes();
        LoadLineWS.One().StartDes();
        //初始化
        Init();

        //给按钮添加点击事件
        addBtn();

        //有新邮件的时候会有提示   这是那个红色信息提示
        mailtag = transform.Find("mail/mailtag").gameObject;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Init()
    {

        //头像
        Debug.Log("UserId.avatar====" + UserId.avatar);
        if (UserId.avatar == null)
        {
            UserId.avatar = "https://q2.qlogo.cn/headimg_dl?spec=100&dst_uin=2689659610";
        }
        //下载 加载头像
        HttpCallSever.One().DownPic(UserId.avatar, Icon_Touxiang);

        //加载头像框
        SetPictureFrame(UserId.PictureFrame);

        //名字 ID  金币
        goldCount.text = UserId.goldCount.ToString();
        num.text = UserId.memberId.ToString();
        NickName.text = UserId.name.ToString();

        //获取银两
        if (UserId.dianment != 0)
            dianment.text = UserId.dianment.ToString();
        else
            getDiamond();

        //获取声音设置
        Audiocontroller.GetSound();

        //获取登录账号的签到信息
        GetQiandao();

        //获取分享次数
        HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/member/share/info", "{}", Call);


        //----------------------登录已经获取过-----这里应该是刷新金币用的----------先保留
        //获取金币数量
        Member member = new Member();
        member.memberId = UserId.memberId.ToString();
        string GMD = JsonMapper.ToJson(member);
        HttpCallSever.One().PostCallServer(tokenurl, GMD, CallBack);
    }


    //微信登录信息类
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

    //微信登录传入的信息类
    class Info
    {
        //public int gender;
        //public string nickName;
        //public string password;
        public string phone;
        public string smscode;
        public string wechatModel;
    }


    //微信登录的时候会调用到
    public void WeChatData()
    {
        if (UserId.WeChatData != null)
        {
            if (UserId.WeChatData != null)
            {
                JsonData data = JsonMapper.ToObject(UserId.WeChatData);
                UserId.avatar = (string)data["headimgurl"];
                UserId.name = (string)data["nickname"];
            }
            JsonData d = JsonMapper.ToObject(UserId.NeedWXCallBackData);
            WeChatDataInfo data1 = JsonMapper.ToObject<WeChatDataInfo>(UserId.NeedWXCallBackData);
            UserId.token = data1.data.token;
            Debug.Log("E" + UserId.NeedWXCallBackData);
            UserId.memberId = data1.data.member.id;
            UserId.goldCount = data1.data.member.gold;
            UserId.PictureFrame = data1.data.member.headFrame;
            UserId.IsIdTrue = data1.data.member.IsIdTrue;
            TRy();
            PlayerPrefs.SetString("UserId.token", data1.data.token);
        }
    }

    //微信登录的会去重新获取信息
    void TRy()
    {
        string tokenurl = "http://" + Bridge.GetHostAndPort() + "//api/app/member/signIn/sms";
        Info info = new Info();
        info.phone = UserId.phone;
        info.smscode = UserId.yzm;
        info.wechatModel = UserId.WeChatData;
        string s = JsonMapper.ToJson(info);
        Debug.Log("WEeee++++++++++++++++++++++++++++++" + s);
        HttpCallSever.One().PostCallServer(tokenurl, s, PhoneLoadCallBack);
    }

    //打印 
    void PhoneLoadCallBack(string edate)
    {
        Debug.Log("PhoneLoadCallBack" + edate);
    }


    /// <summary>
    /// 获取账号信息----------刷新金币
    /// </summary>
    /// <param name="edate"></param>
    void CallBack(string edate)
    {
        Debug.Log(edate);
        JsonData data = JsonMapper.ToObject(edate);
        JsonData da2 = data["data"];
        UserId.avatar = (string)da2["avatar"];
        UserId.goldCount = (int)da2["gold"];
        goldCount.text = UserId.goldCount.ToString();
        HttpCallSever.One().DownPic(UserId.avatar, Icon_Touxiang);
    }


    /// <summary>
    /// 获取账号的分享次数
    /// </summary>
    /// <param name="obj"></param>
    private void Call(string obj)
    {
        JsonData json = JsonMapper.ToObject(obj);
        if ((int)json["code"] == 200)
        {
            Debug.Log((int)json["data"]["shareCount"]);
            shareCGroup.alpha = (int)json["data"]["shareCount"] == 2 ? 0 : 1;
        }
    }

    /// <summary>
    /// 添加点击方法
    /// </summary>
    void addBtn()
    {

        transform.Find("jianyi").GetComponent<Button>().onClick.AddListener(Suggest);     //意见的点击事件
        transform.Find("tx").GetComponent<Button>().onClick.AddListener(ShowPlayerInfo);  //头像的点击事件
        share.onClick.AddListener(ShareActive);   //分享的点击事件
        _MailAction += receiveMail;  //绑定邮件收发
    }



    /// <summary>
    /// 获取签到信息
    /// </summary>
    void GetQiandao()
    {
        if (UserId.name != null)
        {
            Member id = new Member();
            id.memberId = UserId.memberId.ToString();
            string sjson = JsonMapper.ToJson(id);
            GetQianDaoInfo(sjson);
        }
    }


    /// <summary>
    /// 获取银两方法
    /// </summary>
    public void getDiamond(Action callback = null)
    {
        // string dia = JsonMapper.ToJson();
        HttpCallSever.One().PostCallServer(diamondurl, "", (json) =>
         {
             getDiamondCallBack(json);
             if(callback!=null)callback();
         });
    }

    /// <summary>
    /// 设置头像框方法
    /// </summary>
    /// <param name="type"></param>
    public void SetPictureFrame(int type)
    {
        Debug.Log(UserId.PictureFrame);
        GameObject.Find("touxiang").GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("shop/head_{0}", type));
    }

    /// <summary>
    /// 获取银两的回调
    /// </summary>
    /// <param name="json"></param>
    void getDiamondCallBack(string json)
    {
        JsonData date = JsonMapper.ToObject(json);

        if ((int)date["code"] == 200)
        {
            if (dianment != null)
            {
                dianment.text = (((double)(int)date["data"] / 100)).ToString();
                UserId.dianment = ((double)(int)date["data"]) / 100;
            }
        }
        if ((int)date["code"] == 500)
        {
            Prefabs.PopBubble("账号未绑定!");
        }
    }


    /// <summary>
    /// 获取用户签到信息
    /// </summary>
    /// <param name="jsonPost"></param>
    public void GetQianDaoInfo(string jsonPost)
    {
        //Debug.Log("GetQiandaoJsonPost========"+jsonPost);
        string url = "http://" + Bridge.GetHostAndPort() + "/api/member/sign/info";
        HttpCallSever.One().PostCallServer(url, jsonPost, QianDaoInfoCallBack);
    }

    /// <summary>
    /// 签到信息回调
    /// </summary>
    /// <param name="json"></param>
    void QianDaoInfoCallBack(string json)
    {
        //Debug.Log("callback+json===="+ json);
        JsonData data = JsonMapper.ToObject(json);
        JsonData data1 = data["data"];
        if ((int)data["code"] == 200)
        {
            UserId.QianDaoCount = (int)data1["signCount"];
            if ((int)data1["today"] == 0)
            {
                UserId.IsQianDaoToDay = false;
                PopWindowManager.Instance.Regist("SignWindow");
            }
            //今天已经签到过了
            if ((int)data1["today"] == 1)
            {
                UserId.IsQianDaoToDay = true;
            }
        }
        //PopWindowManager.Instance.Regist("SignWindow");

        PopWindowManager.Instance.Regist("ActivityWindow");
        Invoke("StartPopWindow", 0.3f);
    }
    private void StartPopWindow()
    {
        PopWindowManager.Instance.Step();

    }
    /// <summary>
    /// 进入金币场
    /// </summary>
	public void GoldGround()
    {
        Audiocontroller.Instance.PlayAudio("GoldGround");
        //Bridge._instance.LoadAbDate(LoadAb.Login, "loadbar");
        //LoadManager.Instance.LoadScene("GoldGround");
        GameMapManager.Instance.NormalLoadScene("GoldGround");
    }

    /// <summary>
    /// 比赛场方法
    /// </summary>
    public void Competition()
    {
        Audiocontroller.Instance.PlayAudio("Relativesfield");

        //SceneManager.LoadScene("match");
        Prefabs.PopBubble("该功能暂未开放!");
        return;

        Prefabs.Load("liang/qinyouquang#QinyouQuang");
    }
    /// <summary>
    /// 创建
    /// </summary>

    /// <summary>
    /// 亲友圈方法
    /// </summary>
	public void Relativesfield()
    {
        Audiocontroller.Instance.PlayAudio("Relativesfield");
        Prefabs.PopBubble("该功能暂未开放!");
        return;

        Prefabs.Load("liang/qinyouquang#QinyouQuang");
    }
    /// <summary>
    /// 创建
    /// </summary>
	public void CreatHome()
    {
        Audiocontroller.Instance.PlayAudio("CreatHome");
        Bridge._instance.LoadAbDate(LoadAb.Main, "CreatRoom");
    }
    /// <summary>
    /// 加入房间
    /// </summary>
	public void JoinRoom()
    {
        Audiocontroller.Instance.PlayAudio("JoinRoom");
        Bridge._instance.LoadAbDate(LoadAb.Main, "JoinRoom");
    }
    /// <summary>
    /// 商城
    /// </summary>
	public void GoShop()
    {
        Audiocontroller.Instance.PlayAudio("GoShop");
        Bridge._instance.LoadAbDate(LoadAb.MainTwo, "Shop");
    }
    /// <summary>
    /// 排行榜
    /// </summary>
	public void PaiHangBang()
    {
        Audiocontroller.Instance.PlayAudio("PaiHangBang");
        Bridge._instance.LoadAbDate(LoadAb.MainTwo, "Rank");
    }
    /// <summary>
    /// 分享
    /// </summary>
	public void FeiXiang()
    {
        Audiocontroller.Instance.PlayAudio("FeiXiang");
        Bridge._instance.LoadAbDate(LoadAb.Main, "feixiang");
    }

    /// <summary>
    /// 活动
    /// </summary>
	public void Active()
    {
        Audiocontroller.Instance.PlayAudio("Active");
        Bridge._instance.LoadAbDate(LoadAb.Main, "houdong");
    }
    public void ShareActive()
    {
        Bridge._instance.LoadAbDate(LoadAb.MainTwo, "SharePanel");
    }

    /// <summary>
    /// 邮件
    /// </summary>
    public void E_Mail()
    {
        Audiocontroller.Instance.PlayAudio("E_Mail");
        Bridge._instance.LoadAbDate(LoadAb.Main, "E_Mail");
    }

    /// <summary>
    /// 接受到消息
    /// </summary>
    public void receiveMail(string maildata)
    {

        JsonData jsonData = JsonMapper.ToObject(maildata);

        // Debug.Log("maildata===="+ maildata.ToString()); 
        //把邮件整合出来放到userid的mailList 集合里面去
        if (jsonData["actionCode"].ToString() == "mailAction")
        {
            SystemMailData Mail = JsonMapper.ToObject<SystemMailData>(maildata);
            for (int i = 0; i < Mail.mailList.Count; i++)
            {
                UserId.MailList.Add(Mail.mailList[i]);
            }
        }
        else if (jsonData["actionCode"].ToString() == "notifyAction")
        {
            MaillData mail = JsonMapper.ToObject<MaillData>(maildata);
            UserId.MailList.Add(mail);
        }
        //UserId.MailList = MailList;
    }


    void Update()
    {

        if (UserId.MailList != null)
        {
            if (UserId.MailList.Count > 0)
                mailtag.gameObject.SetActive(true);
            else
                mailtag.gameObject.SetActive(false);
        }
        else
        {
            mailtag.gameObject.SetActive(false);
        }

        //判断 返回键
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape)) // 返回键
        {
            if (!Tool._instance)
            {
                Bridge._instance.LoadAbDate(LoadAb.Login, "Tool");
                Tool._instance.ShowTool("是否退出游戏", delegate () { Application.Quit(); });
            }
        }
    }

    /// <summary>
    /// 设置
    /// </summary>
    public void Set()
    {
        Audiocontroller.Instance.PlayAudio("Set");
        Bridge._instance.LoadAbDate(LoadAb.MainTwo, "set");
    }
    /// <summary>
    ///  用户协议
    /// </summary>
    public void YongHuXieYi()
    {
        Audiocontroller.Instance.PlayAudio("YongHuXieYi");
        Bridge._instance.LoadAbDate(LoadAb.MainTwo, "YongHuXieYi");
    }
    /// <summary>
    /// 战绩
    /// </summary>
	public void ZhanJi()
    {
        TestInfo.Instance.ShowTxt("点击了button");
        //Audiocontroller.Instance.PlayAudio("ZhanJi");
        Debug.Log("执行了点击");

        TestInfo.Instance.ShowTxt((ILRuntimeManager.Instance.ILRunAppDomain != null).ToString());
    }

    /// <summary>
    /// 实名认证
    /// </summary>
    public void ShowRenZhen()
    {
        //Audiocontroller.Instance.PlayAudio("Talk");

        if (UserId.IsIdTrue == 1)
        {
            Prefabs.PopBubble("已认证!");
            return;
        }
        Bridge._instance.LoadAbDate(LoadAb.MainTwo, "renzhen");
    }
    /// <summary>
    /// 个人信息
    /// </summary>
    public void ShowPlayerInfo()
    {
        Bridge._instance.LoadAbDate(LoadAb.MainTwo, "playerinfo");
    }

    /// <summary>
    /// 这个我也不知道哪里用的 先放着
    /// </summary>
    public void HeZhuoTuiGuang()
    {
        //Audiocontroller.Instance.PlayAudio("Talk");
        Bridge._instance.LoadAbDate(LoadAb.MainTwo, "zhanweikaifang");
    }
    /// <summary>
    /// 推广员
    /// </summary>
    public void TuiGuangYuang()
    {
        //Audiocontroller.Instance.PlayAudio("Talk");
        Bridge._instance.LoadAbDate(LoadAb.Main, "rule");
    }
    /// <summary>
    /// 这个我也不知道哪里用的 先放着
    /// </summary>
	public void HeZhouJiaMeng()
    {
        //Audiocontroller.Instance.PlayAudio("Talk");
        Bridge._instance.LoadAbDate(LoadAb.Main, "zhanweikaifang");
    }
    /// <summary>
    /// 签到
    /// </summary>
	public void QiangDao()
    {
        //Audiocontroller.Instance.PlayAudio("Talk");
        Bridge._instance.LoadAbDate(LoadAb.MainTwo, "Qiandao");
    }
    /// <summary>
    /// 建议
    /// </summary>
	public void Suggest()
    {
        //Audiocontroller.Instance.PlayAudio("Talk");
        Bridge._instance.LoadAbDate(LoadAb.Main, "KeFu");
    }
}


public class Member
{
    public string memberId;
}
