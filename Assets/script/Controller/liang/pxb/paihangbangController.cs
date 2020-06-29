using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using JX;
using LitJson;
public class paihangbangController : MonoBehaviour {

    //前三名的图标
    public List<Sprite> PhSprites = new List<Sprite>();
    //滚动窗口的父物体
    public Transform parent;
    public Transform top1Parent;
    public Transform top2Parent;
    public Transform top3Parent;

    void Start () {

        //获取排行榜信息
		GetPaiHengInfo("{}");
		
	}
	/// <summary>
	/// 获取排行榜信息方法
	/// </summary>
	/// <param name="jsonPostData">"{ }"</param>
	public void GetPaiHengInfo(string jsonPostData)
	{  
		string url = "http://" + Bridge.GetHostAndPort() + "/api/member/rank";
	    HttpCallSever.One().PostCallServer(url, jsonPostData, RegisterCallback);
	}

    /// <summary>
    /// 获取到之后的回调方法
    /// 给面板添加数据
    /// </summary>
    /// <param name="json">获取到的数据</param>
	void RegisterCallback(string json) 
	{
		
		PaiHangBangClass ps= DynamicModel.ReadAll(json);

        for (int i = 0; i < ps.data.Count; i++)
        {
            if (i >= 3)
                break;

            int index = i + 1;
            GameObject go = Bridge._instance.LoadAbDate(LoadAb.MainTwo, "RankTop3Item");
            var parent = transform.Find("Top" + index);
            go.transform.SetParent(parent);
            go.transform.localPosition = Vector3.zero;

            var seat1Tran = go.transform.Find("Seat1");
            seat1Tran.gameObject.SetActive(seat1Tran.name == "Seat" + index);
            var seat2Tran = go.transform.Find("Seat2");
            seat2Tran.gameObject.SetActive(seat2Tran.name == "Seat" + index);
            var seat3Tran = go.transform.Find("Seat3");
            seat3Tran.gameObject.SetActive(seat3Tran.name == "Seat" + index);
            var top1Tran = go.transform.Find("Top1");
            top1Tran.gameObject.SetActive(top1Tran.name == "Top" + index);
            var top2Tran = go.transform.Find("Top2");
            top2Tran.gameObject.SetActive(top2Tran.name == "Top" + index);
            var top3Tran = go.transform.Find("Top3");
            top3Tran.gameObject.SetActive(top3Tran.name == "Top" + index);

            go.transform.Find("name").GetComponent<Text>().text = ps.data[i].nickname;                    //姓名
            go.transform.Find("id").GetComponent<Text>().text = ps.data[i].id.ToString();                 //ID
            //go.transform.Find("ranking").gameObject.GetComponent<Text>().text = (i + 1).ToString();       //排名
            go.transform.Find("goldcount").GetComponent<Text>().text = ps.data[i].gold.ToString() + "金币"; //金币数          
            Image pic = go.transform.Find("tonxiang").GetComponent<Image>();
            HttpCallSever.One().DownPic(ps.data[i].avatar, pic);                                            //头像
        }

        for (int i = 0; i < ps.data.Count; i++)
        {
            if (i < 3)
                continue;
            GameObject go = Bridge._instance.LoadAbDate(LoadAb.MainTwo, "RankItem");
            go.transform.SetParent(parent);
            go.transform.Find("name").GetComponent<Text>().text = ps.data[i].nickname;                    //姓名
            go.transform.Find("id").GetComponent<Text>().text = ps.data[i].id.ToString();                 //ID
            go.transform.Find("ranking").gameObject.GetComponent<Text>().text = (i + 1).ToString();       //排名
            go.transform.Find("goldcount").GetComponent<Text>().text = ps.data[i].gold.ToString()+"金币"; //金币数          
            Image pic = go.transform.Find("tonxiang").GetComponent<Image>();                    
            HttpCallSever.One().DownPic(ps.data[i].avatar, pic);                                            //头像

            //前三名添加另外的图标
            if (i > 2)           
                go.transform.Find("icon").gameObject.SetActive(false);                         
            else           
               go.transform.Find("icon").GetComponent<Image>().sprite=PhSprites[i];
            
        }
    }

    //关闭
    public void Close()
	{
		Audiocontroller.Instance.PlayAudio("Back");
         Destroy(this.gameObject);
	}
}

//排行榜数据类
public class PaiHangBangClass
{
    /// <summary>
    /// 服务器数据
    /// </summary>
    public List<PHBCData> data;
   
    public string message;
    public int code;
}
//每条数据的信息
public class PHBCData
{
    public int id;
    public int gold;
    public int status;
    public string nickname;
    public string username;
    public int gameCount;
    //public string password;
    //public string phone;
    public string avatar;
    // public long createTime;
    // public long lastLoginTime;
    public PHBCData() { }
    public PHBCData(int id_, string username_, string nickname_, int gamecount_, string avatar_, int gold_, int status_)
    {
        id = id_;
        username = username_;
        nickname = nickname_;
        // password = password_;
        // phone = phone_;
        avatar = avatar_;
        // createTime = createTime_;
        gold = gold_;
        //lastLoginTime = lastLoginTime_;
        status = status_;
        gameCount = gamecount_;
    }
}