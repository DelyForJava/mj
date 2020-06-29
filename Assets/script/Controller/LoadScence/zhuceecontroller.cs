using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JX;
using LitJson;
using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

public class zhuceecontroller : MonoBehaviour {

    public Sprite sp1;
    public Sprite sp2;

    private InputField Phone;
    private InputField YZM;
    private bool timeStart=false;
    public Text time;
    private Transform bg;

    private float timeScon = 60.0f;

    Info userInfo = new Info();

    private void Start()
    {
        time.gameObject.SetActive(false);
        bg= transform.Find("zheche_BG").transform;
        YZM = bg.transform.Find("yanzhenma").GetComponent<InputField>();    
        Phone = bg.transform.Find("zhanghao").GetComponent<InputField>();

        if (!string.IsNullOrEmpty(UserId.QiHuiPhone))       
            Phone.text = UserId.QiHuiPhone;

        
    }

    ///// <summary>
    ///// 获取验证码
    ///// </summary>
    public void GetYZZ_()
    {

        if (Phone.text == null)
        {
            Prefabs.PopBubble("账户未输入");
            return;
        }
        if (Phone.text.Length!=11)
        {
            Prefabs.PopBubble("手机号位数不正确");
            return;
        }

        //string jsonData = "b117df30868bd1f5f280c36d50fdf21a" + Phone.text + GetBeiJingTime(DateTime.Now);
        //string senddata = GetMD5Hash(jsonData);  //MD5加密
        //SendMess se = new SendMess();

        SendMess1 se = new SendMess1();
        se.phone = Phone.text;
        //se.tsign = senddata;
        // se.ts = GetBeiJingTime(DateTime.Now);
        //se.mobile = Phone.text;

        string date = JsonMapper.ToJson(se);  //格式转化

        GetYZM(date);   //获取验证码
    }




    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="jsonPostData">"{\"phone\":\"username\"}"</param>
    public void GetYZM(string jsonPostData)
    {
        //之前调用的企惠发送短信接口 暂时不用了 
        //string url = "https://api.yfkjqhw.com/api/apps/auth/sendauthsms.html";
        string url = "http://" + Bridge.GetHostAndPort() + "/api/member/sendSms";
        HttpCallSever.One().PostCallServer(url, jsonPostData, YZMCallback);
    }

    /// <summary>
    /// 获取验证码回调方法
    /// </summary>
    /// <param name="date"></param>
    private void YZMCallback(string date) {

        JsonData Yzm = JsonMapper.ToObject(date);
        if ((int)Yzm["code"] == 200)
        {
            YZM.transform.Find("SendMa").GetComponent<Image>().sprite = sp2;
            YZM.transform.Find("SendMa").GetComponent<Button>().enabled = false;
            time.gameObject.SetActive(true);
            timeStart = true;
        }
    }

    /// <summary>
    /// 发送验证码后的1分钟计时
    /// </summary>
    private void Update()
    {     
        if (timeStart)
        {         
            time.text = ((int)(timeScon -= Time.deltaTime)).ToString()+"秒后重试";
           // Debug.Log("timeScon=====" + timeScon);

            if (timeScon <= 0)
            {
                YZM.transform.Find("SendMa").GetComponent<Image>().sprite = sp1;
                YZM.transform.Find("SendMa").GetComponent<Button>().enabled = true;
                time.text = "";
                timeStart = false;
                time.gameObject.SetActive(false);
                timeScon = 60.0f;
            }
        }
    }


    /// <summary>
    /// 注册类
    /// </summary>
    class Info
    {
        //public int gender;
        //public string nickName;
        //public string password;
        public string phone;       
        public string smscode;
        public string wechatModel;
    }
    /// <summary>
    /// 注册
    /// </summary>
    public void Register() {

        string yzm = YZM.text;
        //防止数据为空
        if (Phone.text=="" || yzm =="")  //|| password == ""  || username == "" 
        {
            Prefabs.Buoy("数据不能为空");
            //Prefabs.PopBubble("数据不能为空");
            return;
        }


        userInfo.phone = Phone.text;
        userInfo.smscode = YZM.text;
        UserId.phone = Phone.text;
        UserId.yzm = YZM.text;
        userInfo.wechatModel = UserId.WeChatData;

Debug.Log("UserId.WeChatData==="+ UserId.WeChatData);        string info=JsonMapper.ToJson(userInfo);

        PhoneZhuCe(info);
    }
    /// <summary>
    /// 注册号码
    /// </summary>
    /// <param name="jsonPost">"{\"password\":\"password\",\"phone\":\"username\",\"verifyCode\":\"rpassword\"}"</param>
    public void PhoneZhuCe(string jsonPost)
    {     
        string url = "http://"+Bridge.GetHostAndPort()+"/api/app/member/signIn/sms";
        HttpCallSever.One().PostCallServer(url, jsonPost, RegisterCallBack);
    }

    void RegisterCallBack(string json)
    {
        Debug.Log("RegisterCallBack===" + json.ToString());

        JsonData jD = JsonMapper.ToObject(json);
       // JsonData  a= jD["data"];
        if ((int)jD["code"] == 500)
        {
            Prefabs.Buoy("验证码错误");
            //Prefabs.PopBubble("验证码错误");
        }
        if ((int)jD["code"] == 400)
        {
            Prefabs.Buoy("该账号已注册");
            //Prefabs.PopBubble("该账号已注册");
        }

        if ((int)jD["code"]==200)
        {
            Bridge._instance.loginCallBack(jD,true);
        }
    }

    bool ishaveZM;
    bool isHaveDXZM;




    /// <summary>
    /// 关闭窗口
    /// </summary>
	public void Close()
	{
		Audiocontroller.Instance.PlayAudio("Back");
	    Destroy(gameObject);
	}

    /// <summary>
    /// MD5加密
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string GetMD5Hash(string input)
    {
        // 创建一个MD5CryptoServiceProvider对象的新实例。
        MD5 md5Hasher = MD5.Create();
        // 将输入的字符串转换为一个字节数组并计算哈希值。
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
        //创建一个StringBuilder对象，用来收集字节数组中的每一个字节，然后创建一个字符串。
        StringBuilder sBuilder = new StringBuilder();
        // 遍历字节数组，将每一个字节转换为十六进制字符串后，追加到StringBuilder实例的结尾
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        // 返回一个十六进制字符串
        return sBuilder.ToString();
    }


    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <returns></returns>
    private int GetBeiJingTime(System.DateTime time)
    {
        System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (int)(time - startTime).TotalSeconds;
    }

}



class SendMess1
{
   public string phone;
}
/// <summary>
/// 发送验证码类
/// </summary>
class SendMess{
    public string mobile;
    public string tsign;
    public int ts;
 }

/// <summary>
/// 修改账号密码类
/// </summary>
class UpdatePassWord {
   public string password;
   public string phone;
   public string smscode;
}
