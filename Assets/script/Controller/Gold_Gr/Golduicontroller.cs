using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JX;
using LitJson;
using System;

public class Golduicontroller : MonoBehaviour {

	public Text NickName;
	public Text goldCount;
	public Text num;
	public Text dianment;
	public Image Icon_Touxiang;
    public string EnterTableData;
    public Image pictureFrame;
    //Image touPic;
    private string tokenurl = "http://" + Bridge.GetHostAndPort() + "/api/member/info";
    private void Awake()
    {
        AddOnclick();
        Member member = new Member();
        member.memberId = UserId.memberId.ToString();
        string GMD=JsonMapper.ToJson(member);
        HttpCallSever.One().PostCallServer(tokenurl, GMD,CallBack);
    }
    void CallBack(string edate)
    {
        JsonData data=JsonMapper.ToObject(edate);
        JsonData da2= data["data"];
        UserId.goldCount=(int)da2["gold"];
        HttpCallSever.One().DownPic(UserId.avatar, Icon_Touxiang);
    }
    void Start()
    {
        if (GameStart.GoldGroundAction != null) GameStart.GoldGroundAction();
        Audiocontroller.GetSound();   //声音控制

        if (UserId.avatar == null)
        {
            UserId.avatar = "https://q2.qlogo.cn/headimg_dl?spec=100&dst_uin=2689659610";
        }
        Debug.Log(UserId.avatar);
        //touPic = Icon_Touxiang.GetComponent<MeshRenderer>().material;
        HttpCallSever.One().DownPic(UserId.avatar, Icon_Touxiang);
        pictureFrame.sprite = Resources.Load<Sprite>(string.Format("shop/head_{0}", UserId.PictureFrame));
        pictureFrame.SetNativeSize();
        NickName.text = UserId.name;
        goldCount.text = UserId.goldCount.ToString();
        NickName.text = UserId.name;
        num.text = UserId.memberId.ToString();
        // dianment.text = UserId.dianment.ToString();

    }


    void Update() {

        //判断 返回键
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape)) // 返回键
        {
             GameMapManager.Instance.NormalLoadScene("liang");
            //Bridge._instance.LoadAbDate(LoadAb.Login, "Tool");
            //Tool._instance.ShowTool("是否退出游戏", delegate () { Application.Quit(); });
        }
    }
    /// <summary>
    /// 添加点击按钮
    /// </summary>
    void AddOnclick() {
        transform.Find("back").GetComponent<Button>().onClick.AddListener(() => {
            Audiocontroller.Instance.PlayAudio("Back");
            //LoadManager.Instance.LoadScene("liang");
            GameMapManager.Instance.NormalLoadScene("liang");
            //Bridge._instance.LoadAbDate(LoadAb.Login, "loadbar");
            //GameObject go = Prefabs.Load("liang/jiazai#loadbar");
            //Instantiate(go);
        });

    }
    public void PuTong()
    {
        if (LoadManager.Instance.async!=null)
        {
            if (LoadManager.Instance.async.progress > 0)
            {
                Debug.Log("场景跳转中.....");
                return;
            }
        } 
        if (UserId.goldCount>=100)
        {
            LoadLineWS.One().isGoldTable = true;
            
            UserId.GameType = 30;
            //string s = EnterTableDataT(100);
            if (WebSoketCall.One().Check())
            {
                string s = EnterTableDataT(30);
                LoadManager.Instance.LoadScene("mjGameScreen", EnterTableAction, s);
            }
            //LoadManager.Instance.LoadScene("mjGameScreen", EnterTableAction, s);
            //Invoke("InvokeEnter", 0.2f);
        }
        else
        {
            Prefabs.PopBubble("进入需要100金币╮(๑•́ ₃•̀๑)╭，分享可得金币");
        }
        
    }
    public void HaoHuang()
    {
        if (LoadManager.Instance.async != null)
        {
            if (LoadManager.Instance.async.progress > 0)
            {
                Debug.Log("场景跳转中.....");
                return;
            }
        }
        if (UserId.goldCount >= 1200)
        {
            LoadLineWS.One().isGoldTable = true;
            UserId.GameType = 100;          
            if (WebSoketCall.One().Check())
            {
                string s = EnterTableDataT(100);
                LoadManager.Instance.LoadScene("mjGameScreen", EnterTableAction, s);
            }
            //LoadManager.Instance.LoadScene("mjGameScreen", EnterTableAction, s);
            //Invoke("InvokeEnter", 0.2f);
        }
        else
        {

            Prefabs.PopBubble("进入需要1200金币╮(๑•́ ₃•̀๑)╭分享可得金币");
        }
    }
    public void TuHao()
    {
        if (LoadManager.Instance.async != null)
        {
            if (LoadManager.Instance.async.progress > 0)
            {
                Debug.Log("场景跳转中.....");
                return;
            }
        }
        if (UserId.goldCount >= 3000)
        {
            UserId.GameType = 300;
            if (WebSoketCall.One().Check())
            {     
            string s= EnterTableDataT(300);
            LoadManager.Instance.LoadScene("mjGameScreen", EnterTableAction, s);
            }
            //Invoke("InvokeEnter", 0.2f);
        }
        else
        {
            Prefabs.PopBubble("进入需要3000金币╮(๑•́ ₃•̀๑)╭分享可得金币");
        }
    }
    public void EnterTableAction(string data)
    {
        UserId.isCreateRoom = false;
        GameObject.Find("Gold_Game").GetComponent<Game_Controller>().SendToWeb(data);
    }
    public string EnterTableDataT(int obe)
    {
        WebSocketInfo info = new WebSocketInfo();
        info.tableNum = -1;
        info.actionCode = "EnterTableAction";
        info.Params = new WebSocketInfo.data();
        info.Params.code = -1;
        info.Params.type = obe;
        string s = JsonMapper.ToJson(info);
        return s;
        //WebSoketCall.One().SendToWeb(s);
    }
	
	public void AddCoin()
	{
		Audiocontroller.Instance.PlayAudio("Back");
        Bridge._instance.LoadAbDate(LoadAb.Main, "Shop");
       // Prefabs.Load("liang/shop#Shop");
	}

	
}
