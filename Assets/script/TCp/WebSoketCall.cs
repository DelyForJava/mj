using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using WebSocketSharp;


public class WebSoketCall : MonoBehaviour {
	private WebSoketCall() {		
	}

	public static WebSoketCall _Instance = null;

	public static WebSoketCall One()
	{
		if (_Instance == null)
		{
			GameObject obj = new GameObject("WebSoketCall");
			_Instance = obj.AddComponent<WebSoketCall>();
			DontDestroyOnLoad(obj);
		}
		return _Instance;
	}
	private string ServerIp; //连接 websocket 的 ip

	private string Token; //连接 websocket 的令牌
	public string eData;
	public string EnterTableData;
	public string HuDateInfo;
	public bool isGame;
	public bool FristLink;
	public bool OffLink;
	public bool isGoldEnterTable;
	public bool isLinkWS;
	public bool GameIDisLink;
	public bool SentToSeverTG;
	public int ReonnectType ;
	public int Count;
	//private string host = "ws://39.104.87.202:8787/ws?token=6001&say=hello";
	public  WebSocket ws;

	private List<JAData> ResponseMessage=new List<JAData>();    //语音消息

	public delegate void MicPhoneCall(JAData audioDate);    //建立麦克风接收消息委托

	public MicPhoneCall MicPhoneCallback;             //麦克风的回调


	public void IpAndToken(/*string ip,*/string token)
	{
		//ServerIp = ip;	
		Token = token;
	}
	public void StartTXun(Action<String> callback)
	{		
		ServerIp = Bridge.GetHost()+":8787";
		callBack = callback;
		if (!isLinkWS)
		{
			Debug.Log("Succent Link WS");
			isLinkWS = true;
			GameRequset();
		}
	}
	public void StartGame(Action<string> callback)
	{
		Debug.Log("Game_");
		Debug.Log("Game_"+ isGame);
		ServerIp = Bridge.GetHost()+":8787";
		callBack = callback;
		if (!isGame)
		{
			Debug.Log("Succent Link IsGame");
			GameRequset();
			isGame = true;
			GameIDisLink = true;
		}
	}
	public bool Check()
	{
		return ws.IsConnected;
	}
    public void SendToWeb(string json)
	{
		ws.SendAsync(json,CallBack);   		
	}
	public void SendByte(Byte[] by)
	{
		ws.Send(by);
	}

	public Action<string> callBack;
	public void WebSocketCallBack(string data)
	{
		callBack(data);
	}
	public class Connection
	{
		public string actionCode;
		public Data Params;
		public class Data
		{
			public int ReConnectType;
		}

	}
	/// <summary>
	/// 创建  websocket 
	/// </summary>
	public void GameRequset()
	{
		if (ws!=null)
		{
			if (!ws.IsConnected)
			{
				Debug.Log("NewWS");
			//创建服务器连接：
			ws = new WebSocket("ws://" + ServerIp + "/ws?token="+Token+"&say=hello");
			}
		}
		else
		{
			Debug.Log("NewWS_");
			ws = new WebSocket("ws://" + ServerIp + "/ws?token=" + Token + "&say=hello");
		}
		if (OffLink)
		{
			ws = null;
			OffLink = false;
			ws = new WebSocket("ws://" + ServerIp + "/ws?token=" + Token + "&say=hello");
		}
		//当 web socket连接已经建立时发生这里事件。
		Debug.Log("FristLink"+FristLink);
		if (FristLink == false) { 
		ws.OnOpen += (sender, e) => {
			//Debug.Log("EEEEEEE");
			Invoke("ConUpAction", 0.2f);
			Debug.Log(" web socket 连接已经建立" + "ws://" + ServerIp + "/websocket.do?token=" + Token);
		};
		}
		//当 WebSocket 收到消息时发生此事件
		if (FristLink==false)
		{
		ws.OnMessage += (sender, e) =>
		{
			//isGame = true;
			//Debug.Log("GHHHHDDDGGGSS");
			eData = e.Data;
			JsonData data = JsonMapper.ToObject(e.Data);
			callBack(e.Data);
			if ((string)data["actionCode"] == "EnterTableAction")
			{	
				EnterTableData = e.Data;
			}
			else if ((string)data["actionCode"] == "HuAction") {
				HuDateInfo = e.Data;
			}
			else if ((string)data["actionCode"] == "AudioAction")
			{
				readBuffer(e.Data);
				//HuDateInfo = e.Data;
			}
			Debug.Log(" Web Socket 收到新消息 Data: " + e.Data);
		};
		}
		if (FristLink==false)
		{
		//当 WebSocket 获得错误时发生这里事件
		ws.OnError += (sender, e) => {
			Debug.Log(" Web Socket 获得错误" + e.Message);
		};
		}
		if (FristLink==false)
		{
		//当 web socket连接关闭时发生这里事件。
		 ws.OnClose += (sender, e) => {
			Debug.Log(" web socket 连接关闭" + e.Reason);
		 };
		}

		//连接到 web socket服务器。

		//ws.ConnectAsync();
		if (!ws.IsConnected)
		{
			Debug.Log(DateTime.Now + "WSStartLine");
			ws.Connect();
		}

		//将数据发送到 web socket服务器。
		//ws .Send(jsonData);

		//如果希望异步发送数据，则应使用 WebSocket.SendAsync 方法。
		//ws.SendAsync(jsonData,CallBack);

		//关闭 web socket连接。
		//ws.Close (code, reason);

	}
	void Update()
	{
		//音频消息的接收，有的话就给它的委托添加消息，然后删除
		while (ResponseMessage.Count > 0)
		{
			MicPhoneCallback(ResponseMessage[0]);
			ResponseMessage.RemoveAt(0);
		}

		//if (isGame)
		//{
		//	isGame = false;
		//	callBack(eData);
		//}
	}
	public void ConUpAction()
	{
		Connection info = new Connection();
		info.Params = new Connection.Data();
		info.actionCode = "ConnectAction";
		info.Params.ReConnectType = ReonnectType;
		string infoData = JsonMapper.ToJson(info);
		Debug.Log("RE" + ReonnectType);
		ws.Send(infoData);
	}


	/// <summary>
	/// 解析语音消息
	/// </summary>
	/// <param name="audiodata"></param>
	private void readBuffer(string  jsonData)
	{
		JsonData newJD = JsonMapper.ToObject(jsonData);

		JAData jAData = new JAData();
		jAData.memberId =(int) newJD["memberId"];
		jAData.clipTime = (int)newJD["clipTime"];
		jAData.audioData = Convert.FromBase64String((string)newJD["audioData"]);
		ResponseMessage.Add(jAData);		
	}


	public void CallBack(bool isok)
	{
	}


	public int ReadInt(byte[] intbytes)
	{
		Array.Reverse(intbytes);
		return BitConverter.ToInt32(intbytes, 0);
	}

	public void DuangKai( )
	{
		Debug.Log(1);
		//创建服务器连接：
		if (ws!=null)
		{
		  ws.Close(1001);
			Debug.Log(1000);
			//当 web socket连接关闭时发生这里事件。
			ws.OnClose += (sender, e) => {
				Debug.Log(" web socket 连接关闭" + e.Reason);
			};
		}
	}

	/// <summary>
	/// 定义游戏服务器消息格式josn数据
	/// </summary>
	/// <returns></returns>
	private string JsonData()
	{
		StringBuilder sb = new StringBuilder();
		JsonWriter writer = new JsonWriter(sb);

		writer.WriteObjectStart();
		writer.WritePropertyName("tableNum");
		writer.Write(-1);

		writer.WritePropertyName("actionCode");
		string str =/* "{" + "24" + "}";*/"EnterTableAction";
		writer.Write(str);
		
		writer.WritePropertyName("params");
		//string str1 = "{\"code\":-1,\"type\":1}";
		
		writer.Write(jsonData1());
		writer.WriteObjectEnd();

		return sb.ToString();
	}
	private string jsonData1()
	{
		StringBuilder sb1 = new StringBuilder();
		JsonWriter writer1 = new JsonWriter(sb1);
		writer1.WriteObjectStart();
		writer1.WritePropertyName("code");
		writer1.Write(-1);
		writer1.WritePropertyName("type");
		writer1.Write(1);
		writer1.WriteObjectEnd();
		return sb1.ToString();
	}
}
