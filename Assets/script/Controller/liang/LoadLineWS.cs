using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class LoadLineWS : MonoBehaviour {

	public bool isBoolErrorCome;
	public bool isReconnctActoin;
	public bool ismailAction;
	public bool isEnterTable;
	public bool ConnectAnimatorLine;
	public bool isGoldTable;
	public string EnterTableData;
	public string MailActionData;
	public string ErrorData;
	public string ReconnectData;
	private AndroidJavaClass jc;
	private AndroidJavaObject jo;


	private LoadLineWS() { }

	public static LoadLineWS _Instance = null;

	public static LoadLineWS One()
	{
		if (_Instance == null)
		{
			GameObject obj = new GameObject("LoadLineWS");
			_Instance = obj.AddComponent<LoadLineWS>();
			DontDestroyOnLoad(obj);
		}
		return _Instance;
	}
	public void StartWS()
	{
		Debug.Log("user.id====="+ UserId.memberId);
		WebSoketCall.One().IpAndToken(UserId.memberId.ToString());
		Debug.Log("++++++++++++++++++++++++" + UserId.memberId);
		////开始长链接
		WebSoketCall.One().StartTXun(WebSocketCallBack);
		StartCoroutine(DetecConnection());
		if (WebSoketCall.One().FristLink == false)
		{
			WebSoketCall.One().FristLink = true;
		}
	}
	public void StartDes()
	{
		StartCoroutine(DetecConnection());
	}
	public void StopAllIE()
	{
		StopAllCoroutines();
	}
	public void StopDes()
	{
		StopCoroutine("DetecConnection");
	}
	void WebSocketCallBack(string edata)
	{
		//Debug.Log("LoadLine====" + edata.ToString());

		JsonData data = JsonMapper.ToObject(edata);
		string saction = (string)data["actionCode"];
		switch (saction)
		{
			case "error":
				isBoolErrorCome = true;
				ErrorData = edata;
				break;
			case "mailAction":
				UiController._instance._MailAction(edata);
				break;
			case "notifyAction":
				UiController._instance._MailAction(edata);
				break;
			case "ReConnectAction":
				isReconnctActoin = true;
				ReconnectData = edata;
				break;
			case "EnterTableAction":
				Debug.Log("Enter");
				isEnterTable = true;
				EnterTableData = edata;	
				break;
			default:
				break;
		}
	}




	void Update()
	{
		if (isBoolErrorCome)
		{
			isBoolErrorCome = false;
			ErroeDataAction(ErrorData);
		}
		if (isReconnctActoin)
		{
			isReconnctActoin = false;
			LoadManager.Instance.LoadScene("mjGameScreen", Reconnecet, ReconnectData);
			//Bridge._instance.LoadAbDate(LoadAb.Login, "loadbar");
		}
		if (isEnterTable)
		{
			isEnterTable = false;
			if (UserId.LoadWSEnterTable)
			{
				UserId.LoadWSEnterTable = false;
				LoadManager.Instance.LoadScene("mjGameScreen",EnterTableUser,EnterTableData);
			}
			UserId.EnterWSData.Add(EnterTableData);
		}
		if (UserId.GetEntableAction)
		{
			UserId.GetEntableAction = false;
			UserId.JieCreateRoom = true;
			UserId.isJoinRoom = true;
			if (UserId.GetData==null)
			{
				Prefabs.PopBubble("得到的进入房间请求为空");
				return;
			}
			int TableNum=int.Parse(UserId.GetData);
			//Prefabs.PopBubble(UserId.GetData);
			WebSocketInfo info = new WebSocketInfo();
			info.tableNum = -1;
			info.actionCode = "EnterTableAction";
			info.Params = new WebSocketInfo.data();
			info.Params.code = TableNum;
			string s = JsonMapper.ToJson(info);
			WebSoketCall.One().SendToWeb(s);
			Invoke("CheckInfo", 2);
			UserId.GetData = null;
		}
	}
	void CheckInfo()
	{
		JsonData js = JsonMapper.ToObject(WebSoketCall.One().eData);
		//Prefabs.PopBubble(WebSoketCall.One().eData);
		string edata1 = (string)js["actionCode"];
		if (edata1 == "EnterTableAction")
		{
			if (WebSoketCall.One().ws.IsConnected) { 
			LoadManager.Instance.LoadScene("mjGameScreen", JoinEnterData, WebSoketCall.One().eData);
			}
			else { Prefabs.PopBubble("与服务器连接信号不佳"); }
			//Bridge._instance.LoadAbDate(LoadAb.Login, "loadbar");
		}
		else if (edata1 == "error")
		{
			//Prefabs.PopBubble("房间号不存");
		}
	}
	
    void Reconnecet(string edate)
	{
		GameObject.Find("Gold_Game").GetComponent<Game_Controller>().ReConnect(edate);
	}
	void ErroeDataAction(string edata)
	{
		Debug.Log(edata); 

		ErrorDataMessage error=JsonMapper.ToObject<ErrorDataMessage>(edata);
		if(error.msg!= "状态转换错误！") { Prefabs.PopBubble(error.msg); }
		if (error.msg == "玩家账号在别处登录！")
		{
			Debug.Log("+1++++");
			PlayerPrefs.SetString("UserId.token", "");
			WebSoketCall.One().isLinkWS = false;
			WebSoketCall.One().isGame = false;
			StopCoroutine("DetecConnection");
			StopAllCoroutines();
			LoadManager.Instance.LoadScene("loadsceen", Offline, 1);
		}
		//if (error.msg == "房间不存在！")
		//{
		//	if (LoadManager.Instance.async != null) {
		//	if (LoadManager.Instance.async.progress <= 0)
		//	{
		//		Debug.Log("WW122222WW");
		//		GameMapManager.Instance.NormalLoadScene("liang");
		//		}
		//	}
		//}
		//if (error.msg == "异常！")
		//{
		//	if (LoadManager.Instance.async != null)
		//	{
		//		//GameMapManager.Instance.NormalLoadScene("liang");
		//		if (LoadManager.Instance.async.progress <= 0)
		//		{
		//			LoadManager.Instance.LoadScene("liang");
		//		}
		//	}
		//}
	}
	public void Offline(int a)
	{
		WebSoketCall.One().ws.Close();
		WebSoketCall.One().FristLink = false;
		Prefabs.PopBubble("玩家账号在别处登录！");
	}
	public void EnterTableAction(string data)
	{
		UserId.isCreateRoom = false;
		GameObject.Find("Gold_Game").GetComponent<Game_Controller>().EnterTableAction(data);
	}
	void EnterTableUser(string data)
	{
		GameObject.Find("Gold_Game").GetComponent<Game_Controller>().EnterTableAction(data);
	}
	public void JoinEnterData(string edata)
	{
		UserId.isCreateRoom = false;
		GameObject.Find("Gold_Game").GetComponent<Game_Controller>().EnterTableAction(edata);
	}
	IEnumerator DetecConnection()
	{
		while (true)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				//if (GameObject.Find("Canvas").transform.Find("Callll") != null)
				//{
				//	GameObject.Find("Canvas").transform.Find("Callll").gameObject.SetActive(true);
				//}
				Prefabs.PopBubble("网络断开 ◔ ‸◔？ （⊙.⊙）！");
				WebSoketCall.One().ws.Close();
			}
			if (!WebSoketCall.One().Check())
			{
				if (UserId.GameState == false)
				{
					if (ConnectAnimatorLine == false)
					{
						ConnectAnimatorLine = true;
					}
					WebSoketCall.One().isGame = false;
					WebSoketCall.One().isLinkWS = false;
					WebSoketCall.One().FristLink = false;
					WebSoketCall.One().StartTXun(WebSocketCallBack);
					Debug.Log(WebSoketCall.One().FristLink);
					if (WebSoketCall.One().FristLink == false)
					{
						WebSoketCall.One().FristLink = true;
					}
				}
			}	
			else
			{
				//ToDo网络延迟
				WebSoketCall.One().SendToWeb("{\"actionCode\":\"heartbeat\"}");
				if (ConnectAnimatorLine)
				{
					if (UserId.GameState == false) { 
					Prefabs.PopBubble("连接成功:.ﾟヽ(｡◕‿◕｡)ﾉﾟ.:｡+ﾟ");
					}
					ConnectAnimatorLine = false;
					//GameObject.Find("Canvas").transform.Find("ConnectAnimator").gameObject.SetActive(false);
					WebSoketCall.One().isLinkWS = true;
				}
			}
			
			yield return new WaitForSeconds(7f);
		}
	}
}
