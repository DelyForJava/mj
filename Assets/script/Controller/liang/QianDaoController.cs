using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JX;
using UnityEngine.UI;
using LitJson;
using System;

/// <summary>
/// yl
/// </summary>
public class QianDaoController : MonoBehaviour {

	class QianDaoMemberId
	{
		public string memberId;
        //public string signCount;
	}

    Transform view;

    int signNum=0;//签到次数

    void Awake () {
      
        signNum = UserId.QianDaoCount;//签到次数

       transform.Find("Middle").transform.DOMoveY(0.1f, 0.8f).OnComplete(()=> {    //出现动画
           transform.Find("Middle").transform.DOMoveY(0.227f, 1.5f);          
        });       
        
    }

    
    void Start()
    {
        view = this.transform.Find("Middle/Content").transform;

        //if (view)
        //{
        //    if (indexOffset > 1)
        //        view.localPosition = new Vector3(view.localPosition.x - (float)(indexOffset * 260), view.localPosition.y, view.localPosition.z);

        //    if (indexOffset >= 4)
        //        view.localPosition = new Vector3(view.localPosition.x - (float)(4 * 260), view.localPosition.y, view.localPosition.z);  
        //}

        //indexOffset
        RefreshView();
        //添加点击事件
        AddButtFunc();
    }


    void RefreshItem(int index)
    {
        var day = view.Find("Day" + index);

        var unget = day.Find("IconUnget");
        var gettable = day.Find("IconGettable");
        var getted = day.Find("IconGetted");

        unget.gameObject.SetActive(index> UserId.QianDaoCount+1);
        getted.gameObject.SetActive( index < UserId.QianDaoCount + 1);
        gettable.gameObject.SetActive(false);
        if(index== UserId.QianDaoCount + 1)
        {
            unget.gameObject.SetActive(false);
            getted.gameObject.SetActive(UserId.IsQianDaoToDay);
            gettable.gameObject.SetActive(!UserId.IsQianDaoToDay);
            day.GetComponent<Button>().interactable = !UserId.IsQianDaoToDay;
        }
        else
        {
            day.GetComponent<Button>().interactable = false;
        }

    }

    //刷新视图
    void RefreshView()
    {
        //if (UserId.IsQianDaoToDay)
        {
            Debug.Log("UserId.QianDaoCount===="+ UserId.QianDaoCount);

            for (int i = 1; i < 8; i++)
            {
                RefreshItem(i);
            }

            //for (int i = 1; i <= UserId.QianDaoCount+1; i++)
            //{
            //    //toggles[i].isOn = true;
            //    view.Find("Day" + i).GetComponent<Image>().color = Color.gray;

            //        if (view.Find("Day" + i).GetComponent<Button>())
            //        {
            //            view.Find("Day" + i).GetComponent<Button>().enabled = false;
            //            //Destroy(view.Find("Icon_otherD" + i).GetComponent<Button>().transform);
            //        }            
            //}

        }

    }


    /// <summary>
    /// 给签到按钮添加点击事件
    /// </summary>
    private void AddButtFunc() {

        //添加关闭事件
        transform.Find("Middle").Find("Close").GetComponent<Button>().onClick.AddListener(CloseQianDao);

        Debug.Log("signNum===="+signNum);
        for (int i = 1; i < 8; i++)
        {
            RefreshItem(i);
        }

    }
	
    /// <summary>
    /// 关闭签到页面的方法
    /// </summary>
	private void CloseQianDao()
	{
        //Audiocontroller.Instance.PlayAudio("Back");
        transform.Find("Middle").transform.DOMoveY(1.4f, 0.8f).OnComplete(()=> { 
            Destroy(gameObject); 
            PopWindowManager.Instance.Step();
        });

    }
    /// <summary>
    /// 签到
    /// </summary>
    /// <param name="jsonPostData">"{\"memberId\":0}"</param>
    public void QianDao()
	{
		string url= "http://"+Bridge.GetHostAndPort()+"/api/member/sign";
		HttpCallSever.One().PostCallServer(url, "", QanDaoCallBack);
	}

    /// <summary>
    /// 获取签到信息的回调方法
    /// </summary>
    /// <param name="json"></param>
	void QanDaoCallBack(string json)
	{
		Debug.Log("签到回调json==="+json);
		JsonData data = JsonMapper.ToObject(json);
		JsonData data1 = data["data"];

        if ((int)data["code"] == 500)
        {
            Prefabs.PopBubble("今天已签过了");
            //Prefabs.Load("liang/tool#tool");
            //Tool._instance.GetTool(json);
            //Tool._instance.BuyFunc(0, false);
        }
		if ((int)data["code"]==200)
		{
            Prefabs.Buoy("签到成功");
            //Prefabs.Load("liang/tool#tool");
            //Tool._instance.GetTool(json);
            //Tool._instance.BuyFunc(0, false);

            UserId.IsQianDaoToDay = true;
            RefreshView();
            //金币增加
            UserId.goldCount = (int)data1["totalGold"];
            UiController._instance.goldCount.text = data1["totalGold"].ToString();
        }
    }

    string GetMemberInfo(string json)
    {
        QianDaoMemberId qDMI = new QianDaoMemberId();
        qDMI.memberId = json;
        string data = JsonMapper.ToJson(json);
        return data;
    }
}
