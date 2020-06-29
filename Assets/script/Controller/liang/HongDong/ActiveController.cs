using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JX;
using LitJson;
using UnityEngine.UI;
using DG.Tweening;

public class ActiveController : MonoBehaviour {
	public Image BG;
	public Text WZ;
	public ToggleGroup Group;
	private Type Gender = Type.Active;
	private enum Type
	{
		Active,
		FeiXiang,
		TuiGuang,
		JiaMeng,
		GongGao
	}

	private float scaleDuration = 0.3f;
	private Transform middle;
	private Ease ease;
	void Start () {
		middle = transform.GetChild(0);
		ease = Ease.InOutQuad;
		middle.DOScale(Vector3.one, scaleDuration).OnComplete(()=> { 
		}).SetEase(ease);
		OnGenderChanged(true);
	}

	public void OnGenderChanged(bool isOn)
	{
        BG.gameObject.SetActive(true);
        if (!isOn)
		{
			return;
		}
		foreach(Toggle t in Group.ActiveToggles())
		{
			switch (t.name)
			{
				case "Active":				
					Gender = Type.Active;
					qieHuang("ActivityContent1");
					break;
				case "FeiXiang":					
					Gender = Type.FeiXiang;
					qieHuang("ActivityContent2");
					break;
				case "TuiGuang":					
					Gender = Type.TuiGuang;
					qieHuang("ActivityContent3");
					break;
				case "JiaMeng":					
					Gender = Type.JiaMeng;
					qieHuang("ActivityContent1");
					break;
				case "GongGao":
					Gender = Type.GongGao;
					GetStaticGongGao();
					break;
			}
		}
		
	}
	public void qieHuang(string needName)
	{
		if (WZ.isActiveAndEnabled)
		{
			WZ.gameObject.SetActive(false);
		}
		
		//Sprite pic =
		Sprite pic=	Bridge._instance.LoadAbDateSprite(LoadAb.Pic,needName);
			//ABManager.Instance.LoadAsset<Sprite>(needName);
		BG.sprite = pic;
	}

	/// <summary>
	/// 添加公告的方法
	/// </summary>
	/// <param name="jsonpost"></param>
	public void AddGongGao(string jsonpost)
	{
		BG.gameObject.SetActive(false);
		string url = "http://" + Bridge.GetHostAndPort() +"/api/manager/addAnnounce";
		HttpCallSever.One().PostCallServer(url, jsonpost, RegristCallBack);
	}
	/// <summary>
	/// 添加公告的回调函数
	/// </summary>
	/// <param name="json">//jsonPostData= "{\"content\":\"string\",\"htmlContent\":\"string\",\"status\":0,\"title\":\"string\"}";</param>
	void RegristCallBack(string json)
	{
		Debug.Log("1"+json);
	}

	/// <summary>
	/// 获取静态公告的方法
	/// </summary>
	/// <param name="json"></param>
	public void GetStaticGongGao()
	{
		WZ.gameObject.SetActive(true);
		BG.gameObject.SetActive(false);		
		string url = "http://" + Bridge.GetHostAndPort() +"/api/static/publicAnnounce";
		HttpCallSever.One().GetCallSetver(url, StaticInfoCallBack); 
	}

	/// <summary>
	/// 获取静态公告的回调
	/// </summary>
	/// <param name="json"></param>
	void StaticInfoCallBack(string json)
	{
		Debug.Log("2"+json);
		JsonData data = JsonMapper.ToObject(json);
		WZ.text = (string)data["data"];
		VerticalLayoutGroup v= WZ.transform.parent.GetComponent<VerticalLayoutGroup>();
		v.enabled = true;
	}

	/// <summary>
	/// 获取公告
	/// </summary>
	/// <param name="jsonpost"></param>
	public void GetGonggao(string jsonpost)
	{
		string urlGet = "http://" + Bridge.GetHostAndPort() +"/api/manager/getAnnounce";
		//获取公告
		HttpCallSever.One().GetCallSetver(urlGet, GetInfo);
	}
	/// <summary>
	/// 获取公告的回调方法
	/// </summary>
	/// <param name="json"></param>
	void GetInfo(string json)
	{
		Debug.Log("3"+json);
	}
	/// <summary>
	/// 修改公告
	/// </summary>
	/// <param name="jsonpost">//jsonPostData= "{\"announceId\":0，\"content\":\"string\",\"htmlContent\":\"string\",\"status\":0,\"title\":\"string\"}";</param>
	public void XiuGaiGongGao(string jsonpost)
	{
		string url= "http://" + Bridge.GetHostAndPort() +"/api/manager/updateAnnounce";
		HttpCallSever.One().PostCallServer(url, jsonpost, XiuGaiInfo);
	}
	/// <summary>
	/// 修改公告的回调的方法
	/// </summary>
	/// <param name="json"></param>
	void XiuGaiInfo(string json)
	{
		Debug.Log("4" + json);
	}
	public void Close()
	{
		middle.DOScale(Vector3.zero, scaleDuration).OnComplete(() => {
			Destroy(gameObject);
			PopWindowManager.Instance.Step();
		}).SetEase(ease);
	}
}
