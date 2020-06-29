using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LitJson;
using UnityEngine.SceneManagement;

[SerializeField]
public class Game_Controller : MonoBehaviour
{
	#region 参数
	//服务器字对图片
	public  Dictionary<int, Sprite> list = new Dictionary<int, Sprite>();
	public  Dictionary<int, int> SeatTID = new Dictionary<int, int>();
	public List<Sprite> MyOutPutCard = new List<Sprite>();
	public List<Sprite> YouOutPutCard = new List<Sprite>();
	public List<Sprite> ShangOutPutCard = new List<Sprite>();
	public List<Sprite> ZuoOutPutCard = new List<Sprite>();
	[Header("玩家名字")]
	public List<Text> handTuoName = new List<Text>();
	[Header("头像框数据")]
	public List<Image> HeadFamer = new List<Image>();
	[Header("Yuyi")]
	public List<GameObject> ShowChat = new List<GameObject>();
	[Header("骰盘动画")]
	public List<Image> TweenP = new List<Image>();
	[Header("手牌暂存")]
	public List<int> DarwPai = new List<int>();
	[Header("所有牌")]
	public Sprite[] PaiSprite;
	[Header("玩家ID数据")]
	public List<int> playerId = new List<int>();
	[Header("玩家信息数据")]
	public List<string> playerInfo = new List<string>();
	[Header("玩家头像从左道右为我右上左")]
	public List<Image> HandPic = new List<Image>();
	[Header("字典key为IdValue为Seatnum")]
	public Dictionary<int, int> dic = new Dictionary<int, int>();
	[Header("玩家出牌信息")]
	public List<string> PlayerActionDataOfLink = new List<string>();
	[Header("玩家摸牌信息")]
	public List<string> DrawActionData = new List<string>();
	[Header("托管信息组")]
	public List<string> HungUpData = new List<string>();
	[Header("进入桌子的数据集")]
	public List<string> EnterTableIEData = new List<string>();
	public List<string> OutTableIEData = new List<string>();
	[Header("右边的牌")]
	public List<int> one = new List<int>();
	[Header("上边的牌")]
	public List<int> twotwo = new List<int>();
	[Header("左边的牌")]
	public List<int> three = new List<int>();
	[Header("准备数据集")]
	public List<string> ReallyInfo = new List<string>();
	[Header("庄家标志")]
	public List<Image> Bookpic = new List<Image>();
	[Header("玩家有碰的id")]
	#region chi牌数组
	//要吃的牌在中间
	[Header("要吃中间的牌")]
	public List<int> chiType_Zhong = new List<int>();
	//要吃的牌在上边
	[Header("要吃上的牌")]
	public List<int> chiType_shang = new List<int>();
	//要吃的牌在下边
	[Header("要吃下边的牌")]
	public List<int> chiType_xia = new List<int>();
	#endregion
	[Header("自己的牌")]
	public Transform Pai;
	//自己出牌的放点
	[Header("出牌放点")]
	public Transform parent;
	[Header("杠的按钮")]
	public Transform GongnengButton;
	[Header("游戏对话")]
	public Transform Talk;
	[Header("当前摸牌的对象")]
	public Transform CurrentDuixiang;
	[Header("位置对应")]
	public Transform weastenTuoZi;
	[Header("吃牌技能显示对象")]
	public Transform pic;
	//开始游戏需要打开
	public Transform[] maJiangpai;
	[Header("结算的物体")]
	public Transform JieTrabsform;
	[Header("左牌、右牌、上牌")]
	public Transform Zuo, You, Shang;
	[Header("左牌、右牌、上牌的父物体")]
	public Transform ZP, YP, SP;
	[Header("显示技能地")]
	public Transform skillMap;
	[Header("断线重连动画")]
	public Transform ConnectAnimator;
	[Header("打出牌对象")]
	public GameObject DaoChuObject;
	[Header("游戏玩法对象")]
	public GameObject WanFai;
	[Header("牌动画_M")]
	public Animator anima;
	public Animator Yanima;
	public Animator Zanima;
	public Animator Sanima;
	public JieSuangController jie;
	public int OutTabeCount;
	[Header("庄家序号")]
	public int Bookmaker;
	[Header("用来记录G3情况下放的位置")]
	public int G3Position;
	//房间桌子号
	public int tableNum;
	[Header("杠的牌")]
	public int GrabCards;
	[Header("玩家当前执行的对象")]
	public int CurrentZXCount;
	[Header("当前摸牌次数")]
	public int CurrentDrawCount = 0;
	[Header("自己座位号")]
	public int seatNum;
	[Header("当前机器人状况")]
	public int robPepleCount;
	[Header("当前打出的牌")]
	public int CurrentPlayerCard;
	[Header("创建房间的轮数")]
	public int Createround;
	[Header("自己技能的数量")]
	public int MySkillCount;
	[Header("杠牌的记录")]
	public int GangCard;
	//癞子牌
	public int rascalCard;
	public int robCount;
	//左边座位号
	public int zuoplaycount;
	//右边的座位号
	public int youplaycount;
	//上边的座位号
	public int shangplaycount;
	[Header("托管人数")]
	public int TGCount;
	//当前摸牌对象；
	[Header("当前摸牌对象")]
	public int CurrentObject;
	public int chuCard;
	//player现在对象
	[Header("Player现在的对象")]
	public int PlayerNum;
	//自己拥有的癞子数；
	public int reascalcount;
	[Header("进入房间存下的局数")]
	public int Rount_;
	//胡牌的卡
	[Header("胡的牌")]
	public int HuPaiCard;
	[Header("倒计时秒数")]
	public int TotalTime = 15;
	[Header("摸牌阶段摸到的牌")]
	public int DrawCPai;
	public int SHowTableCount;
	[Header("错误信息")]
	public string errorData;
	[Header("托管数据")]
	public string HangUpData;
	[Header("信息数据")]
	public string MessageData;
	[Header("胡数据")]
	public string HuDataInfo;
	[Header("全部准备的信息")]
	public string AllReallyActionData;
	[Header("断线重连数据")]
	public string ReConnectionData;
	[Header("结束数据")]
	public string EndData;
	[Header("是否出桌子")]
	public bool isOutTable;
	[Header("是否是断线重连")]
	public bool isReconnection;
	[Header("开始游戏")]
	public bool isGameStart;
	public bool isPass;
	[Header("是否全部准备好了")]
	public bool isAllRellyAction;
	[Header("是否是吃碰要准备的事")]
	public bool CallAmByother;
	[Header("rob>3时用于判断")]
	public bool RobDrawCardok;
	[Header("huActionISok")]
	public bool IsHuAction;
	[Header("网络波动为T是网络波动为F是掉线回来")]
	public bool WLBD;
	bool cChi;
	public bool isSTGPutCard;
	[Header("是否正在出牌")]
	public bool isPlayerIng;
	[Header("EndConnectIS")]
	public bool EndConnectIs;
	//是否是自己出牌
	public bool isOwn;
	[Header("G3位置是否使用")]
	public bool G3bool;
	public bool ReconncctShow;
	public bool GameGetLiang;
	public bool StatrFristPai;
	public bool isOverOutTableIE;
	public bool isSkillUse;
	[Header("是否执行完玩家数据")]
	public bool isOverPlayerAction;
	[Header("最后一局是否开始")]
	public bool isEndAction;
	[Header("游戏已经开局")]
	public bool SureGame;
	[Header("是否是碰牌后有杠情况")]
	public bool GangT3;
	[Header("右是否是碰牌后有杠情况")]
	public bool YouGangT3;
	[Header("是否是要接收错误信息")]
	public bool errorBool;
	[Header("左是否是碰牌后有杠情况")]
	public bool ZuoGangT3;
	[Header("上是否是碰牌后有杠情况")]
	public bool ShangGangT3;
	[Header("是否是创建房间")]
	public bool isCreateRoom;
	[Header("是否进入托管")]
	public bool isHungUp;
	[Header("是否出牌携程执行完毕")]
	public bool isOverStartPlayer = true;
	[Header("是否摸牌携程执行完毕")]
	public bool isOverStartDraw = true;
	[Header("是否是断线重连")]
	public bool isReConnection;
	//是否发送表情
	public bool isSendMessage;
	[Header("是否是执行出牌回调")]
	public bool isPlayerAction;
	[Header("是否是摸牌回调")]
	public bool isDrawAction;
	[Header("是否进入主动进入托管")]
	public bool activePTrusteeship;
	[Header("托管携程是否完成")]
	public bool isHangUpOver;
	[Header("是否进入桌子")]
	public bool isEnterTable;
	[Header("是否进入桌子数据处理完")]
	public bool isOverEnterTable;
	[Header("是否是准备")]
	public bool isReallyShow;
	[Header("准备数据处理是否完成")]
	public bool isReallyShowOver;
	[Header("是否是加入房间")]
	public bool isJoinRoom;
	[Header("游戏是否开始")]
	public bool NoGameStart;
	#region 杠的判定
	[Header("自己是否是明杠")]
	public bool isMingGang;
	public bool isYouMingGang;
	public bool isShangMingGang;
	public bool isZuoMingGang;
	public bool isMyGang;
	public bool isHu;
	public bool OverDarw;
	#endregion
	[Header("是否是其他玩家回合")]
	public bool isOther;
	[Header("是否是其他人吃碰")]
	public bool otherChiPeng;
	[Header("是否是谈话对象显示")]
	public bool isTalk;
	[Header("功能开关是否打开")]
	public bool isShow;
	[Header("是否进入托管")]
	public bool StartTG;
	[Header("是否需要断线重连")]
	public bool ConnectAnimatorLine;

	[Header("创建房间桌子的显示")]
	public Text TableNumShow;
	//牌的数量
	[Header("牌的数量显示")]
	public Text OtherPaiShow;
	[Header("当前轮数")]
	public Text CurrentRound;
	[Header("倒计时显示对象")]
	public Text TimeText;
	[Header("显示的轮数")]
	public Text ShowTable;
	//癞子的图片
	public Image reascalPic;
	public Sprite newCord;
	[Header("邀请好友")]
	public Button YQHY;
	public CreateLumSum CLS;
	public TipController WaitTipPeople;
	public PlayerBack playerData = new PlayerBack();
	//房间是否可以托管T不可托管
	public bool IsAutoPut = true;
	[Header("游戏类型")]
	public EnumGameStatus GSStatus;
	#endregion

	public AllReallyActionController AllReadyAction;
	public EnterTableActionController EnterTableAction_;
	public ReallyActionController ReadyAction_;
	public PlayerActionController PlayerAction;
	public DrawActionController DrawAction_;
	public PengActionController PengActionController;
	public ChiActionController ChiActionController;
	public OutTableActionController OutTableController;
	public GangActionController GangController;
	public HuActionController HuController;
	public ReConnectController ReController;
	public ErrorActionController ErrorController;
	public HangUpController HangUpController;
	public GameMessageControlle GameMessage;
	void Awake()
	{
		#region ...
		list.Add(1, PaiSprite[0]);
		list.Add(2, PaiSprite[1]);
		list.Add(3, PaiSprite[2]);
		list.Add(4, PaiSprite[3]);
		list.Add(5, PaiSprite[4]);
		list.Add(6, PaiSprite[5]);
		list.Add(7, PaiSprite[6]);
		list.Add(8, PaiSprite[7]);
		list.Add(9, PaiSprite[8]);
		list.Add(11, PaiSprite[9]);
		list.Add(12, PaiSprite[10]);
		list.Add(13, PaiSprite[11]);
		list.Add(14, PaiSprite[12]);
		list.Add(15, PaiSprite[13]);
		list.Add(16, PaiSprite[14]);
		list.Add(17, PaiSprite[15]);
		list.Add(18, PaiSprite[16]);
		list.Add(19, PaiSprite[17]);
		list.Add(21, PaiSprite[18]);
		list.Add(22, PaiSprite[19]);
		list.Add(23, PaiSprite[20]);
		list.Add(24, PaiSprite[21]);
		list.Add(25, PaiSprite[22]);
		list.Add(26, PaiSprite[23]);
		list.Add(27, PaiSprite[24]);
		list.Add(28, PaiSprite[25]);
		list.Add(29, PaiSprite[26]);
		list.Add(32, PaiSprite[27]);
		list.Add(35, PaiSprite[28]);
		list.Add(38, PaiSprite[29]);
		list.Add(41, PaiSprite[30]);
		list.Add(44, PaiSprite[31]);
		list.Add(47, PaiSprite[32]);
		list.Add(50, PaiSprite[33]);
		#endregion
		TurnAnimator();
		playerInfo = new List<string>();
		Initle();
		reascalPic.transform.parent.gameObject.SetActive(false);
		OtherPaiShow.transform.parent.gameObject.SetActive(false);
		WebSoketCall.One().StartGame(WebSocketCallBack);
		WebSoketCall.One().isGame = true;
		UserId.GameState = true;
		playerData = new PlayerBack();
		playerData.actionCode = "PlayerBackAction";
		playerData.replayList = new List<string>();
	}
	public void Initle()
	{
		PlayerActionDataOfLink = new List<string>();
		DrawActionData = new List<string>();
		EnterTableIEData = new List<string>();
		OutTableIEData = new List<string>();
		ReallyInfo = new List<string>();
		HungUpData = new List<string>();
	}
	void Start()
	{
		Audiocontroller.GetSound();   //声音控制
		SHowTableCount = 1;
		UserId.GameState = true;
		PlayerPrefs.SetString("Reconnect", "Two");
		isReconnection = UserId.isJoinRoom;
		isCreateRoom = UserId.isCreateRoom;
		isJoinRoom = UserId.isJoinRoom;
		activePTrusteeship = false;
		isOverStartDraw = true;
		isOverOutTableIE = true;
		isOverStartPlayer = true;
		isHangUpOver = true;
		isOverEnterTable = true;
		isReallyShowOver = true;
		DarwPai = new List<int>();
		TableNumShow.gameObject.SetActive(false);
		CurrentRound.gameObject.SetActive(false);
		PlayerActionDataOfLink = new List<string>();
		DrawActionData = new List<string>();
		jie.maJiangPai = PaiSprite;
		Debug.Log(WebSoketCall.One().ws.IsConnected);
		//当长连接没有连接时继续连接
		StartCoroutine(DetecConnection());
		NoGameStart = true;
		LoadLineWS.One().StopDes();
		WebSoketCall._Instance.MicPhoneCallback += MicPhone;
		Audiocontroller.Instance.reginsVoice();
	}
	public void EntableInfoData(int obe)
	{
		WebSocketInfo info = new WebSocketInfo();
		info.tableNum = -1;
		info.actionCode = "EnterTableAction";
		info.Params = new WebSocketInfo.data();
		info.Params.code = -1;
		info.Params.type = obe;
		string s = JsonMapper.ToJson(info);
		jie.Type = obe;
		WebSoketCall.One().SendToWeb(s);
	}
	/// <summary>
	/// 进入其他人房间
	/// </summary>
	/// <param name="obe"></param>
	public void EntableOtherData(int obe)
	{
		WebSocketInfo info = new WebSocketInfo();
		info.tableNum = -1;
		info.actionCode = "EnterTableAction";
		info.Params = new WebSocketInfo.data();
		info.Params.code = obe;
		string s = JsonMapper.ToJson(info);
		jie.Type = obe;
		WebSoketCall.One().SendToWeb(s);
	}
	public void CreateRoom(int s, int ss)
	{
		WebSocketInfo web = new WebSocketInfo();
		web.actionCode = "CreateTableAction";
		web.Params = new WebSocketInfo.data();
		jie.Type = s;
		web.Params.type = s;
		web.Params.round = Createround;
		web.Params.maxPoints = ss;
		web.Params.IsAutoPut = IsAutoPut;
		string json = JsonMapper.ToJson(web);
		WebSoketCall.One().SendToWeb(json);
		TableNumShow.text = tableNum.ToString();
	}
	public void Serr(string edata) { }
	/// <summary>
	/// 断线重连的回调
	/// </summary>
	/// <param name="edata"></param>
	public void ReConnect(string edata)
	{
		Debug.Log("Reconnect");
		ReconncctShow = true;
		ControllerAnimator(false);
		SureGame = true;
		YQHY.transform.parent.GetChild(2).gameObject.SetActive(false);
		for (int i = 0; i < maJiangpai.Length; i++)
		{
			if (maJiangpai[i].name == "touzi")
			{
				continue;
			}
			maJiangpai[i].gameObject.SetActive(true);
		}
		Invoke("InvokeSkill", 1);
		ReController.ReConnectAction(edata);
	}
	public void InvokeSkill()
	{
		YQHY.gameObject.SetActive(false);
		NoGameStart = false;
	}
	public IEnumerator ie()
	{
		HangUpCallInfo hang = new HangUpCallInfo();
		hang.actionCode = "HangUpAction";
		hang.Params = new HangUpCallInfo.HangUpStart();
		hang.Params.playerStatus = 1;
		if (isOwn == true) { hang.Params.HangUpType = 1; }
		string json11 = JsonMapper.ToJson(hang);
		WebSoketCall.One().SendToWeb(json11);
		StartTG = true; 
		yield return new WaitForSeconds(1f);
		StartTG = false;
	}
	public void CleanData(Transform Obj)
	{
		for (int i = Obj.childCount-1; i>=0; i--)
		{
			Destroy(Obj.GetChild(i).gameObject);
		}
	}
	public void Really()
	{
		WebSocketInfo wscb = new WebSocketInfo();
		Debug.Log(""+tableNum);
		wscb.tableNum = tableNum;
		wscb.actionCode = "ReadyAction";
		wscb.Params = new WebSocketInfo.data();
		string json = JsonMapper.ToJson(wscb);
		NoGameStart = true;
		WebSoketCall.One().SendToWeb(json);
		GameObject.Find("ReallyButton").transform.Find("MyReally").gameObject.SetActive(false);
	}
	void WebSocketCallBack(string edata)
	{
		Debug.Log("My" + ":" + edata);
		JsonData data = JsonMapper.ToObject(edata);
		string saction = (string)data["actionCode"];
		switch (saction)
		{
			case "ChiAction":
				ThreadManager.jsondata = edata;
				ThreadManager.QueueOnMainThread(ChAction);				
				break;
			case "CreateTableAction":
				ThreadManager.jsondata = edata;
				ThreadManager.QueueOnMainThread(CreateTableAction);
				break;
			case "DrawAction":
				isDrawAction = true;
				DrawActionData.Add(edata);
				break;
			case "EnterTableAction":
				isEnterTable = true;
				EnterTableIEData.Add(edata);
				break;
			case "GangAction":
				ThreadManager.jsondata = edata;
				ThreadManager.QueueOnMainThread(GangAction);
				break;
			case "HangUpAction":
				isHungUp = true;
				HungUpData.Add(edata);
				break;
			case "PassAction":
				isPass = true;
				break;
			case "PengAction":
				ThreadManager.jsondata = edata;
				ThreadManager.QueueOnMainThread(PengAction);
				break;
			case "PlayAction":
				isPlayerIng = true;
				isPlayerAction = true;
				PlayerActionDataOfLink.Add(edata);
				break;
			case "ReadyAction":
				isReallyShow = true;
				ReallyInfo.Add(edata);
				break;
			case "AllReadyAction":
				isAllRellyAction = true;
				AllReallyActionData = edata;
				break;
			case "HuAction":
				ThreadManager.jsondata = edata;
				ThreadManager.QueueOnMainThread(HuAction);
				HuDataInfo = edata;
				break;
			case "ReConnectAction":
				isReConnection = true;
				ReConnectionData = edata;
				break;
			case "GameMessage":
				isSendMessage = true;
				MessageData = edata;
				break;
			case "OutTableAction":
				isOutTable = true;
				OutTableIEData.Add(edata);
				break;
			case "error":
				errorBool = true;
				errorData = edata;
				break;
			case "EndAction":
				isEndAction = true;
				EndData = edata;
				break;
			case "EndConnectAction":
				Debug.Log("End");
				EndConnectIs = true;
				break;
		}
	}
	class EndActionData
	{
		public List<int> endPointList;
		public string actionCode;
		public List<int> memberIdList;
	}
	void EndAction(string edate)
	{
		EndActionData json =JsonMapper.ToObject<EndActionData>(edate);
		jie.gameObject.SetActive(false);
		CurrentRound.gameObject.SetActive(false);
		if (!CLS.HuList.Contains(WebSoketCall.One().HuDateInfo))
		{CLS.HuList.Add(WebSoketCall.One().HuDateInfo);}
		CLS.TableNum = tableNum;
		CLS.Id = playerId;
		//暂时关闭
		CLS.goldpoint_ = json.endPointList;
		CLS.GameIdList = json.memberIdList;
		jie.StartAllCombute();
		SHowTableCount++;
		ShowTable.text = SHowTableCount.ToString();
		StartCoroutine(OUTTable());
	}
	IEnumerator OUTTable()
	{
		Prefabs.PopBubble("牌局结束,房间将在15秒后自动解散 ≖‿≖✧ o‿≖✧(๑•̀ㅂ•́)و✧"); 
		yield return new WaitForSeconds(17);
		if (UserId.JieCreateRoom)
		{
			OutTable();
		}
	}
	void ErrorAction(string edate)
	{
		ErrorController.ErrorAction(edate);
	}
	public void LoadLiang()
	{
		UserId.GameState = false;
		SceneManager.LoadScene("liang");
	}
	void HuAction(string edata)
	{
		Debug.Log("HuAction" + edata);
		if (edata == null)
		{
			Debug.Log("edate 为空");
			edata = WebSoketCall.One().HuDateInfo;
		}
		if (PlayerAction.Point_ != null) { Destroy(PlayerAction.Point_); }
		SkillTranSlate();
		TurnNum(0);
		StopCountDown();
		SureGame = false;
		isSkillUse = false;
		if (UserId.JieCreateRoom)
		{
			if (!CLS.HuList.Contains(edata))
			{
				CLS.HuList.Add(edata);
			}
		}
		HuclassInfo huclass = JsonMapper.ToObject<HuclassInfo>(edata);
		HuController.ShowHuHandCard(huclass);
		HuController.HuLevel1Animator(huclass);
		//HuController.HuAction(edata);
		StartCoroutine(HuShowHuAction(edata));
	}
	IEnumerator HuShowHuAction(string edata)
	{
		yield return new WaitForSeconds(0.8f);
		HuController.HuAction(edata);
	}
	public void SendToWeb(string s)
	{
		if (WebSoketCall.One().Check())
		{
			WebSoketCall.One().SendToWeb(s);
		}
		else
		{
			Prefabs.PopBubble("与服务器链接不佳");
			GameMapManager.Instance.NormalLoadScene("liang");
			UserId.GameState = true;
		}
	}
	//收到语音消息
	public void MicPhone(JAData audioDate) {
		ShowChatTime(audioDate.memberId,audioDate.clipTime);
	}
	GameObject showChat;
	//显示的时长
	public void ShowChatTime(int memberid,int cliptime ) {
		int index = 0;
		if (dic!=null && dic.ContainsKey(memberid))
		{
		  index = dic[memberid];
		}
		//GameObject showChat = null;
		try
		{
			if (ShowChat[0] == null) { 
			ShowChat[0] = GameObject.Find("Gold_Game").transform.Find("SelfHandpic/chat").gameObject;
			ShowChat[1] = GameObject.Find("Gold_Game").transform.Find("YouHead/chat").gameObject;
			ShowChat[2] = GameObject.Find("Gold_Game").transform.Find("ShangHead/chat").gameObject;
			ShowChat[3] = GameObject.Find("Gold_Game").transform.Find("ZuoHead/chat").gameObject;
			}
			if (seatNum == index) 
				showChat = ShowChat[0];
			else if (youplaycount == index)
			showChat = ShowChat[1];
			else if (shangplaycount == index)
				showChat = ShowChat[2];
			else if (zuoplaycount == index)
				showChat = ShowChat[3];
		}
		catch (System.Exception)
		{
			throw;
		}

		if (showChat!=null)
		{
			showChat.SetActive(true);
		}	
		Debug.Log("gamector.show==========="+ cliptime);
		Invoke("StartContro", 2);
	}
	void StartContro()
	{
		StartCoroutine(waittime(2, showChat));
	}
	IEnumerator waittime(int cliptime, GameObject showChat) {
		yield return new WaitForSeconds(cliptime+1);
		showChat.SetActive(false);
	}
	void AllReallyAction(string jsonData)
	{
		ControllerAnimator(true);
		StatrFristPai = true;
		NoGameStart = false;
		EnterTableIEData.Clear();
		StartTG = false;
		Transform GameChangerTable = GameObject.Find("CreatRoom").transform.Find("HuangZhu (1)");
		(GameChangerTable as RectTransform).anchoredPosition = new Vector2(-505, 255);
		Shang.GetChild(Shang.childCount - 1).name = "GameObject (12)";
		You.GetChild(You.childCount-1).name = "GameObject (12)";
		(Zuo.GetChild(3) as RectTransform).anchoredPosition = new Vector2(111, -132.9f);
		Zuo.GetChild(Zuo.childCount - 1).name = "GameObject (12)";
		YQHY.gameObject.SetActive(false);
		YQHY.transform.parent.GetChild(2).gameObject.SetActive(false);
		WaitTipPeople.gameObject.SetActive(false);
		WaitTipPeople.StopWaitPeople();
		WaitTipPeople.TimeDownCount = 0;
		//开始加入动画
		if (UserId.JieCreateRoom)
		{ CurrentRound.gameObject.SetActive(true); }
		else { CurrentRound.gameObject.SetActive(false); }
		GameObject.Find("ReallyButton").transform.Find("MyReally").gameObject.SetActive(false);
		reascalPic.transform.parent.gameObject.SetActive(true);
		OtherPaiShow.transform.parent.gameObject.SetActive(true);
		GameObject reallybutton = GameObject.Find("turnImage");
		SureGame = true;
		UserId.TableNum = tableNum;
		//准备消失
		for (int i = 0; i < reallybutton.transform.childCount; i++)
		{
			reallybutton.transform.GetChild(i).gameObject.SetActive(false);
		}
		//打开动画
		for (int i = 0; i < maJiangpai.Length; i++)
		{
			maJiangpai[i].gameObject.SetActive(true);
		}
		isGameStart = true;
		OtherPaiShow.text = "85";
		AllReadyAction.AllReallyAction(jsonData);
	}
	public int BuGang(List<int> SkillCard,int card)
	{
		int Seat=0;
		for (int i = SkillCard.Count-1; i >=0 ; i--)
		{
			if (card == SkillCard[i])
			{
				Seat = i;
				break;
			}
		}
		return Seat;
	}
	/// <summary>
	/// 将杠牌位置找到
	/// </summary>
	/// <param name="Card"></param>
	/// <returns></returns>
	public List<int> CheckPoint(List<int> Card)
	{
		List<int> Gang = new List<int>();
			if (Card.Count > 0)
			{
				int CurrentPoint = Card[0];
				int CurrentIndex = 0;
				int SameCount = 0;
				int OtherCount = 0;
				for (int k = 0; k < Card.Count; k++)
				{
					if (CurrentPoint != Card[k])
					{
						switch (SameCount)
						{
							case 3:
								Gang.Add(CurrentIndex - 3);
								SameCount = 0;
								break;
							default:
								SameCount = 0;
								break;
						}
						if ((CurrentPoint + 1) == Card[k])
						{
							OtherCount++;
						}
						CurrentPoint = Card[k];
						CurrentIndex = k;
					}
					else
					{
						CurrentIndex = k;
						SameCount++;
						if (k == 0) { SameCount -= 1; }
					}
					if (k == Card.Count - 1)
					{
						if (SameCount == 2)
						{
							SameCount = 0;
						}
						else if (SameCount == 3)
						{
							Gang.Add(CurrentIndex - 3);
							SameCount = 0;
						}
						else if (OtherCount == 2)
						{
							OtherCount = 0;
						}
						else if (OtherCount == 3)
						{
							OtherCount = 0;
						}
					}
				}
			}
		return Gang;
	}
	/// <summary>
	/// 进入房间
	/// </summary>
	/// <param name="jsonData">服务器传入的参数</param>
	public void EnterTableAction(string jsonData)
	{
		EnterTableAction_.EnterTableAction(jsonData);
	}
	/// <summary>
	/// 玩家信息
	/// </summary>
	/// <param name="data"></param>
	public void PlayerInfo(string data)
	{
		//Debug.Log(data);
		PlayerMessageInfo playerMessage = JsonMapper.ToObject<PlayerMessageInfo>(data);
		if (seatNum == dic[playerMessage.data.id])
		{
			UserId.memberId = playerMessage.data.id;
			JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, HandPic[0]);
			handTuoName[0].text = playerMessage.data.nickname;
			HeadFamer[0].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
			UserId.goldCount = playerMessage.data.gold;
		}
		else if (youplaycount == dic[playerMessage.data.id])
		{
			JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, HandPic[1]);
			handTuoName[1].text = playerMessage.data.nickname;
			HeadFamer[1].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
		}
		else if (shangplaycount == dic[playerMessage.data.id])
		{
			JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, HandPic[2]);
			handTuoName[2].text = playerMessage.data.nickname;
			HeadFamer[2].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
		}
		else if (zuoplaycount == dic[playerMessage.data.id])
		{
			JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, HandPic[3]);
			handTuoName[3].text = playerMessage.data.nickname;
			HeadFamer[3].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
		}
		if (!playerInfo.Contains(data))
		{
			playerInfo.Add(data);
		}
	}
	//监测牌的误差
	public void ContrastPai(int PaiCard,int DataCard,Transform obj)
	{
		if (obj == Shang)
		{
			Vector2 distance = new Vector2((Shang.GetChild(Shang.childCount - 1) as RectTransform).anchoredPosition.x, 0);
			Vector2 distance1 = new Vector2((Shang.GetChild(Shang.childCount - 2) as RectTransform).anchoredPosition.x, 0);
			float data = Vector2.Distance(distance, distance1);
			if (data > 70)
			{
				//Prefabs.PopBubble("位置有问题");
				(Shang.GetChild(Shang.childCount - 1) as RectTransform).anchoredPosition = (Shang.GetChild(Shang.childCount - 2) as RectTransform).anchoredPosition;
				(Shang.GetChild(Shang.childCount - 1) as RectTransform).anchoredPosition += new Vector2(68, 0);
			}
		}
		else if (obj == Zuo)
		{
			Vector2 distance = new Vector2(0,(Zuo.GetChild(Zuo.childCount - 1) as RectTransform).anchoredPosition.y);
			Vector2 distance1 = new Vector2(0,(Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition.y);
			float data = Vector2.Distance(distance, distance1);
			data = data < 0 ? -data : data;
			if (data > 50)
			{
				//Prefabs.PopBubble("位置有问题");
				(Zuo.GetChild(Zuo.childCount - 1) as RectTransform).anchoredPosition = (Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition;
				(Zuo.GetChild(Zuo.childCount - 1) as RectTransform).anchoredPosition -= new Vector2(0,50);
			}
		}
		else if (obj == You)
		{
			Vector2 distance = new Vector2(0, (You.GetChild(You.childCount - 1) as RectTransform).anchoredPosition.y);
			Vector2 distance1 = new Vector2(0, (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition.y);
			float data = Vector2.Distance(distance, distance1);
			data = data < 0 ? -data : data;
			if (data > 50)
			{
				//Prefabs.PopBubble("位置有问题");
				(You.GetChild(You.childCount - 1) as RectTransform).anchoredPosition = (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition;
				(You.GetChild(You.childCount - 1) as RectTransform).anchoredPosition -= new Vector2(0,50);
			}
		}
		else if (obj == Pai)
		{
			Vector2 distance = new Vector2((Pai.GetChild(Pai.childCount - 1) as RectTransform).anchoredPosition.x, 0);
			Vector2 distance1 = new Vector2((Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition.x, 0);
			float data = Vector2.Distance(distance, distance1);
			if (data > 109)
			{
				Debug.Log(data);
				(Pai.GetChild(Pai.childCount - 1) as RectTransform).anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
				(Pai.GetChild(Pai.childCount - 1) as RectTransform).anchoredPosition += new Vector2(108, 0);
			}
		}
		if (PaiCard!=DataCard)
		{
			int num = PaiCard - DataCard;
			
			if (num>0)
			{
				if (num>5)
				{
					//Debug.LogError("Error");
					return;
				}
					for (int i = 0; i < num; i++)
					{				
						DestroyImmediate(obj.GetChild(obj.childCount - 1 - i).gameObject);
					}
			}
			else if (num < 0)
			{
				int num1 = DataCard - PaiCard;
				if (obj==Pai)
				{
					for (int i = 0; i < num1; i++)
					{
						GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "PaiRes_My", Pai);
						#region 点击事件的添加
						go.transform.GetChild(0).gameObject.AddComponent<EventTrigger>();
						go.transform.GetChild(0).gameObject.AddComponent<DragMajiang>();
						go.name = "XP";
						Transform childT = go.transform.GetChild(0);
						if (DrawCPai != 0)
						{
						childT.GetComponent<Image>().sprite = list[DrawCPai];
						}
						//添加点击事件
						EventTrigger trigger = go.transform.GetChild(0).GetComponent<EventTrigger>();
						EventTrigger.Entry entry = new EventTrigger.Entry();
						entry.eventID = EventTriggerType.PointerClick;
						entry.callback = new EventTrigger.TriggerEvent();
						entry.callback.AddListener(OnClick);
						trigger.triggers.Add(entry);
						#endregion
						RectTransform pos = go.transform as RectTransform;
						(pos.transform as RectTransform).anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
						(pos.transform as RectTransform).anchoredPosition += new Vector2(108, 0);
					}
				}
				if (obj == Shang)
				{
					Debug.Log("ShangAdd++");
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "ShangRes", Shang);
					(go.transform as RectTransform).anchoredPosition = (Shang.GetChild(Shang.childCount - 2) as RectTransform).anchoredPosition;
					(go.transform as RectTransform).anchoredPosition += new Vector2(68, 0);
					go.transform.SetAsLastSibling();
				}
				if (obj ==Zuo)
				{
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "ZuoRes", Zuo);
					(go.transform as RectTransform).anchoredPosition = (Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition;
					(go.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);
					go.transform.SetAsLastSibling();
				}
				if (obj == You)
				{
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "YouRes", You);
					(go.transform as RectTransform).anchoredPosition = (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition;
					(go.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);
					go.transform.SetAsLastSibling();
				}
			}
		}
	}
	/// <summary>
	/// 玩家动作产生的参数
	/// </summary>
	/// <param name="jsonData"></param>
	void PlayAction(string jsonData)
	{
		isPlayerIng = true;
		ControllerAnimator(true);
		UserId.GameState = true;
		WLBD = true;
		if (ReconncctShow)
		{
			ReconncctShow = false;
			transform.Find("rascalCard").gameObject.SetActive(true);
			transform.Find("CardNum").gameObject.SetActive(true);
			if (UserId.JieCreateRoom)
			{
				GameObject.Find("Round").transform.GetChild(0).gameObject.SetActive(true);
				GameObject.Find("SkillMap").transform.Find("tableShow").gameObject.SetActive(true);
			}
		}
		SkillTranSlate();
		for (int i = 0; i < Pai.childCount; i++)
		{
			if ((Pai.GetChild(i) as RectTransform).anchoredPosition.y != 21)
			{
				if ((Pai.GetChild(i) as RectTransform).anchoredPosition.y==50)
				{
					continue;
				}
				(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(0, ((Pai.GetChild(i) as RectTransform).anchoredPosition.y - 21));
			}
			MaJiang.Instance.Majiang_ = null;
		}
		activePTrusteeship = false;
		YQHY.gameObject.SetActive(false);
		//调整位置防止出错
		jie.SorePeople(Pai);
		PlayerAction.PlayerAction(jsonData);
	}
	//动画关闭
	public void ControllerAnimator(bool CoAnimator)
	{
	   anima.enabled = CoAnimator;
		Yanima.enabled=CoAnimator;
		Sanima.enabled=CoAnimator;
		Zanima.enabled=CoAnimator;
	   StatrFristPai = CoAnimator;
	}
	//skill显示
	public IEnumerator SkillTShow(string choseN,string skillname)
	{
		Transform go=null;
		switch (choseN)
		{
			case "Shang":
				go = GameObject.Find("ShangSkillCards").transform.Find(skillname);
				break;
			case "Zuo":
				go = GameObject.Find("ZuoSkillCards").transform.Find(skillname);
				break;
			case "You":
				go = GameObject.Find("YouSkillCards").transform.Find(skillname);
				break;
			default:
				break;
		}
		go.gameObject.SetActive(true);
		yield return new WaitForSeconds(1f);
		go.gameObject.SetActive(false);

	}
	public void StartCountDown()
	{
		StartCoroutine("CountDown");
	}
	public void StopCountDown()
	{
		StopCoroutine("CountDown");
		TotalTime = 15; TimeText.text = TotalTime.ToString();
	}
	#region 过牌方法
	public void GuoPai()
	{
		isSkillUse = false;
		if (PlayerNum == seatNum)
		{
			isOwn = true;
		}
		//Audiocontroller.Instance.MajiangManPlayer("");
		ActionInfo info = new ActionInfo();
		info.actionCode = "PassAction";
		info.Params = new ActionInfo.Data();
		info.Params.card = chuCard;
		string json = JsonMapper.ToJson(info);
		WebSoketCall.One().SendToWeb(json);
		Transform t = skillMap.Find("skillone");
		for (int i = 0; i < t.childCount; i++)
		{
			t.GetChild(i).gameObject.SetActive(false);
		}
		for (int i = pic.childCount - 1; i >= 0; i--)
		{
			Destroy(pic.GetChild(i).gameObject);
		}
		pic.gameObject.SetActive(false);
		for (int i = 0; i < skillMap.childCount; i++)
		{
			if (skillMap.GetChild(i).name == "skillone")
			{
				return;
			}
			skillMap.GetChild(i).gameObject.SetActive(false);
		}
	}
	#endregion
	/// <summary>
	/// 玩家准备
	/// </summary>
	/// <param name="jsonData"></param>
	void ReadyAction(string jsonData)
	{
		reascalPic.transform.parent.gameObject.SetActive(false);
		OtherPaiShow.transform.parent.gameObject.SetActive(false);
		for (int i = 0; i < maJiangpai.Length; i++)
		{
			maJiangpai[i].gameObject.SetActive(false);
		}
		if (UserId.JieCreateRoom)
		{
			CurrentRound.text = Createround.ToString();
			CurrentRound.gameObject.SetActive(true);
		}
		else { CurrentRound.gameObject.SetActive(false); }
		ReallyActionData data = JsonMapper.ToObject<ReallyActionData>(jsonData);
		ReadyAction_.ReadyAction(jsonData);
	}
	/// <summary>
	/// 碰产生的回调
	/// </summary>
	/// <param name="jsonData"></param>
	void PengAction(string jsonData)
	{
		StopCoroutine("CountDown"); TotalTime = 15; TimeText.text = TotalTime.ToString();
		SkillTranSlate();
		Transform CP = skillMap.Find("skillone/chi");
		CP.GetChild(0).gameObject.SetActive(false);
		PengActionController.PengAction(jsonData);
	}
	public void SKO_()
	{
		for (int i = 0; i < Pai.childCount; i++)
		{
			if (Pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite == list[rascalCard])
			{
				Pai.GetChild(i).GetChild(2).gameObject.SetActive(true);
			}
			else
			{
				Pai.GetChild(i).GetChild(2).gameObject.SetActive(false);
			}
		}
	}
	#region 过牌回调
	/// <summary>
	/// 过牌产生的回调
	/// </summary>
	/// <param name="jsonData"></param>
	void PassAction(string jsonData) {
		Transform CP = skillMap.Find("skillone/chi");
		CP.GetChild(0).gameObject.SetActive(false);
	}
	#endregion
	/// <summary>
	/// 托管回调
	/// </summary>
	/// <param name="jsonData"></param>
	void HangUpAction(string jsonData)
	{
		HangUpController.HangUpAction(jsonData);
	}
	/// <summary>
	/// 杠产生的回调
	/// </summary>
	/// <param name="jsonData"></param>
	void GangAction(string jsonData)
	{
		SkillTranSlate();
		isSkillUse = false;
		Audiocontroller.Instance.MajiangManPlayer(63);
		Transform CP = skillMap.Find("skillone/chi");
		CP.GetChild(0).gameObject.SetActive(false);
		DrawActionInfo da = JsonMapper.ToObject<DrawActionInfo>(jsonData);
		GangController.GangAction(jsonData);
	}
	//刷新技能牌
	public void OtherSkillCardFresh(List<int> skillCards,Transform ObjGame)
	{
		for (int i = 0; i < skillCards.Count; i++)
		{
			Transform tf = ObjGame.GetChild(i);
			tf.GetChild(0).gameObject.SetActive(false);
			tf.GetChild(1).gameObject.SetActive(true);
			tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[skillCards[i]];
		}
	}
	/// <summary>
	/// 摸牌回调
	/// </summary>
	/// <param name="jsonData"></param>
	void DrawAction(string jsonData)
	{
		activePTrusteeship = false;
		UserId.GameState = true;
		WLBD = true;
		SkillTranSlate();
		DrawAction_.DrawAction(jsonData);
	}
	//技能显示关闭
	public void SkillTranSlate()
	{
		skillMap.Find("hu").gameObject.SetActive(false);
		skillMap.Find("skillone/gang").gameObject.SetActive(false);
		skillMap.Find("skillone/chi").gameObject.SetActive(false);
		skillMap.Find("skillone/peng").gameObject.SetActive(false);
		skillMap.Find("skillone/guo").gameObject.SetActive(false);
		skillMap.Find("qianggang").gameObject.SetActive(false);
	}
	 public void StartFoolHangUp(int key)
	{
		StartCoroutine(PlayerActionCard(key));
	}
	public IEnumerator PlayerActionCard(int key)
	{
		yield return new WaitForSeconds(1f);
		WebSocketInfo wsi = new WebSocketInfo();
		wsi.actionCode = "PlayAction";
		wsi.tableNum = tableNum;
		wsi.Params = new WebSocketInfo.data();
		wsi.Params.card = key;
		string json = JsonMapper.ToJson(wsi);
		//StartCoroutine(SentCardInfo(json));
		WebSoketCall.One().SendToWeb(json);
		StopCoroutine("CountDown");
		isOwn = false;
		Debug.Log("Out()+OOOOOOOOOOOOOOOOO");
	}
	public List<int> RefashCard(List<int> HandCard,List<int> skillCard,int reascalCount_)
	{
		Debug.Log("RefashCard");
		List<int> ShuangCard = new List<int>();
		if (skillCard.Count>0)
		{
		for (int i = 0; i <skillCard.Count ; i++)
		{
			ShuangCard.Add(skillCard[i]);
		}
		}
		if (HandCard.Count>0)
		{
			for (int i = 0; i < HandCard.Count; i++)
			{
				ShuangCard.Add(HandCard[i]);
			}
		}
		for (int i = 0; i < reascalCount_; i++)
		{
			ShuangCard.Add(rascalCard);
		}
		int CountNum = HandCard.Count + skillCard.Count + reascalCount_;
		ContrastPai(Pai.childCount, CountNum, Pai);
		for (int i = 0; i < ShuangCard.Count; i++)
		{
			Pai.GetChild(i).GetChild(2).gameObject.SetActive(false);
			Pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite = list[ShuangCard[i]];
			Pai.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[ShuangCard[i]];
		}
		for (int i = 0; i < reascalCount_; i++)
		{
			Pai.GetChild(Pai.childCount - 1 - i).GetChild(2).gameObject.SetActive(true);
		}
		//StartCoroutine("CountDown");
		return ShuangCard;
	}
	/// <summary>
	/// 获取手牌
	/// </summary>
	/// <param name="HandCard"></param>
	/// <param name="skillCard"></param>
	/// <param name="reascalCount_"></param>
	/// <returns></returns>
	public List<int> OtherResh(List<int> HandCard, List<int> skillCard, int reascalCount_)
	{
		List<int> ShuangCard = new List<int>();
		for (int i = 0; i < skillCard.Count; i++)
		{
			ShuangCard.Add(skillCard[i]);
		}
		for (int i = 0; i < HandCard.Count; i++)
		{
			ShuangCard.Add(HandCard[i]);
		}
		for (int i = 0; i < reascalCount_; i++)
		{
			ShuangCard.Add(rascalCard);
		}
		return ShuangCard;
	}
	public int ScreenPosition(int card,List<int> skillCards)
	{
		int SameCard = 0;
		for (int i = 0; i < skillCards.Count; i++)
		{
			if (skillCards[i]==card)
			{
				SameCard = i;
				break;
			}
		}
		return SameCard;
	}
	/// <summary>
	/// 创建牌局产生的回调
	/// </summary>
	/// <param name="jsonData"></param>
	void CreateTableAction(string jsonData)
	{
		CurrentRound.gameObject.SetActive(true);
		CreateDataInfo data = JsonMapper.ToObject<CreateDataInfo>(jsonData);
		CurrentRound.gameObject.SetActive(true);
		CurrentRound.text = Createround.ToString();
		YQHY.gameObject.SetActive(true);
		if (TableNumShow == null)
		{
			TableNumShow = GameObject.Find("SkillMap").transform.Find("tableShow").GetComponent<Text>();
		}
		tableNum = data.tableNum;
		UserId.TableNum = data.tableNum;
		TableNumShow.text = data.tableNum.ToString();
		seatNum = 1;
		youplaycount = 2;
		shangplaycount = 3;
		zuoplaycount = 4;
		dic = new Dictionary<int, int>();
		if (!dic.ContainsKey(1))
		{
			dic.Add(UserId.memberId, 1);
		}

		Member member = new Member();
		member.memberId = UserId.memberId.ToString();
		string json = JsonMapper.ToJson(member);
		JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/member/getinfo", json, PlayerInfo);

		YQHY.transform.parent.GetChild(2).gameObject.SetActive(false);

	}
	/// <summary>
	/// 吃牌
	/// </summary>
	/// <param name="jsonData"></param>
	void ChAction(string jsonData)
	{
		StopCoroutine("CountDown"); TotalTime = 15; TimeText.text = TotalTime.ToString();
		SkillTranSlate();
		Audiocontroller.Instance.MajiangManPlayer(61);
		ChiActionController.ChiAction(jsonData);
	}

	void OutTableAction(string edata)
	{
		OutTabeCount = 0;
		OutTableData data = JsonMapper.ToObject<OutTableData>(edata);
		OutTableController.OutTableAction(edata);
	}
	public  IEnumerator OutSceen()
	{
		yield return new WaitForSeconds(0.5f);
		GameMapManager.Instance.NormalLoadScene("liang");
		UserId.GameState = false;
	}
	public void GoSetting()
	{
		Audiocontroller.Instance.PlayAudio("Back");
		Bridge._instance.LoadAbDate(LoadAb.MainTwo, "set");
	}

	public void OnClickMaJiang(GameObject game)
	{
		if (StatrFristPai)
		{
			StatrFristPai = false;
			anima.enabled = false;
		}
		//需要放到的位置
		int NeedPut = 0;
		int NeedInt = 0;
		if (MaJiang.Instance.Majiang_ != null)
		{
			//判断是否是第二次点击
			if (MaJiang.Instance.Majiang_ == game.transform.parent.gameObject)
			{
				ShowSameCard(game.transform,false,true);
				jie.SorePeople(Pai);
				//没到自己回合不能出牌
				if (!isOwn)
				{
					RectTransform rect1 = MaJiang.Instance.Majiang_.transform as RectTransform;
					rect1.anchoredPosition -= new Vector2(0, 50);
					MaJiang.Instance.Majiang_ = null;
					return;
				}
				if (rascalCard == DrawCPai)
				{
					DarwPai.Add(DrawCPai);
				}
				for (int i = 1; i < DarwPai.Count; ++i)
				{
					int t = DarwPai[i];
					int j = i;
					while ((j > 0) && (DarwPai[j - 1] > t))
					{
						DarwPai[j] = DarwPai[j - 1];
						--j;
					}
					DarwPai[j] = t;
				}
				NeedPut = DarwPai.IndexOf(DrawCPai) + MySkillCount;
				if (reascalcount > 0)
				{
					if (NeedPut == Pai.childCount)
					{
						NeedPut -= reascalcount;
					}
					else if (NeedPut > (Pai.childCount - reascalcount)) { NeedPut = Pai.childCount - reascalcount; }
				}
				//需要合拢的位置和准备变的层级
				for (int i = 0; i < Pai.childCount; i++)
				{
					if (Pai.GetChild(i) == game.transform.parent)
					{
						NeedInt = i + 1;
					}
				}
				if (NeedPut>=Pai.childCount)
				{
					NeedPut = Pai.childCount - 1;
				}
				//找到要变的位置
				Vector2 pos = (Pai.GetChild(NeedPut) as RectTransform).anchoredPosition;
				//当前的位置
				RectTransform rr = Pai.GetChild(Pai.childCount - 1).transform as RectTransform;
				Debug.Log("NP" + NeedPut);
				Debug.Log("NI" + NeedInt);
				if (!CallAmByother)
				{
					if (NeedInt != Pai.childCount)
					{
						StartCoroutine(BuPaiHL(NeedInt, NeedPut, rr, pos));
					}
				}
				else
				{
					CallAmByother = false;
					for (int i = NeedInt; i < Pai.childCount; i++)
					{
						//合拢
						if (NeedInt != Pai.childCount)
						{
							(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(108, 0);
						}
					}
				}
				DestroyImmediate(game.transform.parent.gameObject);
				//将对应的牌放入到指定位置
				//rr.anchoredPosition = pos;
				MaJiang.Instance.Majiang_ = null;
				//打出牌的数
				int DaiChuPai = 0;
				foreach (var key in list.Keys)
					{
						if (list[key] == MaJiang.Instance.Impic)
						{
							DaiChuPai = key;
							WebSocketInfo wsi = new WebSocketInfo();
							wsi.actionCode = "PlayAction";
							wsi.tableNum = tableNum;
							wsi.Params = new WebSocketInfo.data();
							wsi.Params.card = key;
							string json = JsonMapper.ToJson(wsi);
							//StartCoroutine(SentCardInfo(json));
							WebSoketCall.One().SendToWeb(json);
							StopCoroutine("CountDown");
							Debug.Log("OutPut Card OnClick +()+" + key);
							isOwn = false;
						}
					}
				GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "MajiangRes", parent);
				go.transform.GetChild(0).GetComponent<Image>().sprite = MaJiang.Instance.Impic;
				DaoChuObject = go;
				Invoke("LocationConfirmation", 2.2f);
				//LocationConfirmation();
			}
			//不是就把之前那张下降，在把新对象赋予后把新对象升起
			else
			{
				ShowSameCard(MaJiang.Instance.Majiang_.transform.GetChild(0),false,false);		
				RectTransform rect = MaJiang.Instance.Majiang_.transform as RectTransform;
				rect.anchoredPosition -= new Vector2(0, 50);
				GameObject go = MaJiang.Instance.SelectGameObject(game.transform.parent.gameObject);
				RectTransform r = go.transform as RectTransform;
				r.anchoredPosition += new Vector2(0, 50);
				ShowSameCard(game.transform,true,false);
			}
		}
		else
		{
			ShowSameCard(game.transform,true,false);
			//空选择时自己给他并使其上升
			GameObject go = MaJiang.Instance.SelectGameObject(game.transform.parent.gameObject);
			RectTransform r = go.transform as RectTransform;
			r.anchoredPosition += new Vector2(0, 50);
		}
	}
	//显示场上存在的相同牌
	public void ShowSameCard(Transform showTarGet,bool isShow,bool Recove)
	{
		ShowGroupCard(parent, showTarGet, isShow, Recove);
		ShowGroupCard(SP, showTarGet, isShow,Recove);
		ShowGroupCard(YP, showTarGet, isShow,Recove);
		ShowGroupCard(ZP, showTarGet, isShow,Recove);
	}
	public void ShowGroupCard(Transform objGame,Transform showTarGet,bool isShow,bool Recove)
	{
		Sprite s=showTarGet.GetComponent<Image>().sprite;
		for (int i = 0; i < objGame.childCount; i++)
		{
			if (Recove == false) { 
			if (s== objGame.GetChild(i).GetChild(0).GetComponent<Image>().sprite)
			{
				//显示
				if (isShow) { objGame.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.yellow; }
				else { objGame.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white; }
			}
			}
			else { objGame.GetChild(i).GetChild(0).GetComponent<Image>().color=Color.white;}
		}
	}
	//位置确认
	public void LocationConfirmation()
	{
		Vector2 distance = new Vector2((Pai.GetChild(Pai.childCount - 1) as RectTransform).anchoredPosition.x, 0);
		Vector2 distance1 = new Vector2((Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition.x, 0);
		float data = Vector2.Distance(distance, distance1);
		Debug.Log("OPpppee");
		if (data > 109)
		{
			Debug.Log("OnClickPosError");
			Debug.Log(data);
			(Pai.GetChild(Pai.childCount - 1) as RectTransform).anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
			(Pai.GetChild(Pai.childCount - 1) as RectTransform).anchoredPosition += new Vector2(108, 0);
		}
	}
	void Update()
	{
		if (isPlayerAction)
		{
			if (isOverStartPlayer)
			{
				isPlayerAction = false;
				if (PlayerActionDataOfLink == null)
				{
					Debug.Log("PlayerActionDataOfLink is Null");
					return;
				}
				StartCoroutine(PlayerTry());
			}
		}
		if (isDrawAction)
		{
			if (isOverStartDraw)
			{
				if (DrawActionData == null)
				{
					Debug.Log("DrawActionData is Null");
				}
				isDrawAction = false;
				StartCoroutine(DrawPerform());
			}
		}
		if (isSendMessage)
		{
			isSendMessage = false;
			GameMessageCallBack(MessageData);
		}
		if (isReConnection)
		{
			isReConnection = false;
			YQHY.transform.parent.GetChild(2).gameObject.SetActive(false);
			ReConnect(ReConnectionData);
		}
		if (isHungUp)
		{
			isHungUp = false;
			if (isHangUpOver)
			{
				StartCoroutine(HangUp());
			}
		}
		if (isCreateRoom)
		{
			isCreateRoom = false;
			TableNumShow.gameObject.SetActive(true);
			YQHY.gameObject.SetActive(true);
			YQHY.GetComponent<Button>().onClick.RemoveAllListeners();
			YQHY.GetComponent<Button>().onClick.AddListener(() => {
				Transform g= YQHY.transform.parent.GetChild(1);
				g.GetComponent<GameSharteWeChat>().TableNum = tableNum.ToString();
				g.gameObject.SetActive(true);
			});
		}
		if (isOutTable)
		{
			if (isOverOutTableIE)
			{
				isOutTable = false;
				StartCoroutine(OutTableIE());
			}
		}
		if (isReallyShow)
		{
			isReallyShow = false;
			if (isReallyShowOver)
			{
				StartCoroutine(ReallyShow());
			}
		}
		if (isEnterTable)
		{
			isEnterTable = false;
			if (isOverEnterTable)
			{
				StartCoroutine(ETable());
			}
		}
		if (isAllRellyAction)
		{
			isAllRellyAction = false;
			AllReallyAction(AllReallyActionData);
		}
		if (errorBool)
		{
			errorBool = false;
			ErrorAction(errorData);
		}
		if (isEndAction)
		{
			isEndAction = false;
			EndAction(EndData);
		}
		if (UserId.EnterWSData.Count>0)
		{
			//有新数据先清头像
			for (int i = 0; i < HandPic.Count; i++)
			{
				HandPic[i].gameObject.GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "CSTX");
				handTuoName[i].text = null;
				HeadFamer[i].sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "CSTXK");
			}
			//换新数据
			for (int i = 0; i < UserId.EnterWSData.Count; i++)
			{
				EnterTableAction(UserId.EnterWSData[i]);
			}
			UserId.EnterWSData.Clear();
		}
		if (isPass)
		{
			isPass = false;
			PassAction("Pass");
		}
		if (EndConnectIs)
		{
			EndConnectIs = false;
			Prefabs.PopBubble("EndActionCome in");
			SceneManager.LoadScene("liang");
			LoadManager.Instance.LoadScene("liang", EndConnection, 1);
			UserId.GameState = false;
		}
		//判断 返回键
		if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape)) // 返回键
		{
			if (!Tool._instance)
			{
				//GameMapManager.Instance.NormalLoadScene("liang");
				Bridge._instance.LoadAbDate(LoadAb.Login, "Tool");
				Tool._instance.ShowTool("是否返回主界面或者退出游戏", delegate () { Application.Quit(); }, delegate () { OutTable();UserId.GameState = false; });
			}
		}
	}
	void EndConnection(int a)
	{
		Prefabs.PopBubble("牌局结束，返回大厅");
	}
	void GameMessageCallBack(string edata)
	{
		GameMessage.GameMessage(edata);
	}
	public void CloseExpressio(GameObject go)
	{
		go.SetActive(false);
	}
	public void OnChi(string name)
	{
		cChi = cChi == false ? true : false;
		Transform pic = GameObject.Find("skillshowPai").transform.Find(name);
		if (pic.childCount==1)
		{
			ChiPaiClass chi = new ChiPaiClass();
			chi.Params = new ChiPaiClass.Data();
			chi.actionCode = "ChiAction";
			string name_=pic.GetChild(0).name;
			switch (name_)
			{
				case "zhong":
					chi.Params.card2 = chiType_Zhong[0];
					chi.Params.card3 = chiType_Zhong[1];
					break;
				case "shang":
					chi.Params.card2 = chiType_shang[0];
					chi.Params.card3 = chiType_shang[1];
					break;
				case "xia":
					chi.Params.card2 = chiType_xia[0];
					chi.Params.card3 = chiType_xia[1];
					break;
			}
			string jsondata = JsonMapper.ToJson(chi);
			WebSoketCall.One().SendToWeb(jsondata);
			Transform t = skillMap.Find("skillone");
			for (int i = 0; i < t.childCount; i++)
			{
				t.GetChild(i).gameObject.SetActive(false);
			}
			for (int i = pic.childCount - 1; i >= 0; i--)
			{
				Destroy(pic.GetChild(i).gameObject);
			}
			StopCoroutine("CountDown"); TotalTime = 15; TimeText.text = TotalTime.ToString();
			chiType_shang.Clear(); chiType_xia.Clear(); chiType_Zhong.Clear();
			return;
		}
		pic.gameObject.SetActive(cChi);
	}
	public void OnClick(BaseEventData pointData)
	{
		PointerEventData pointD = pointData as PointerEventData;
		OnClickMaJiang(pointD.pointerPress);
	}
	public GameObject BuPai(Sprite sprite)
	{
		RectTransform childPos = Pai.GetChild(Pai.childCount - 1) as RectTransform;
		GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "PaiRes_My", Pai);
		if (sprite == list[rascalCard])
		{
			go.transform.GetChild(2).gameObject.SetActive(true);
		}
		go.transform.GetChild(0).gameObject.AddComponent<EventTrigger>();
		go.transform.GetChild(0).gameObject.AddComponent<DragMajiang>();
		go.name = "XP";
		Transform childT = go.transform.GetChild(0);
		childT.GetComponent<Image>().sprite = sprite;

		//添加点击事件
		EventTrigger trigger = go.transform.GetChild(0).GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback = new EventTrigger.TriggerEvent();
		entry.callback.AddListener(OnClick);
		trigger.triggers.Add(entry);

		RectTransform pos = go.transform as RectTransform;
		pos.position = childPos.position;
		Audiocontroller.Instance.MajiangManPlayer(66);
		if (isGameStart)
		{
			StartCoroutine(BuPaiAnimator(pos));
			isGameStart = false;
		}
		else
		{
			pos.anchoredPosition = childPos.anchoredPosition;
			pos.anchoredPosition += new Vector2(151, 0);
		}

		if (isMingGang)
		{
			DaoChuObject = go;
		}

		StartCoroutine("CountDown");
		return go;
	}

	public void OtherBuPai(Transform pai, Sprite sprite, string paiRes)
	{//在指定位置生成
		GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, paiRes, pai);
		CurrentDuixiang = go.transform;
		RectTransform rect = go.transform as RectTransform;
		if (isZuoMingGang || isYouMingGang || isShangMingGang)
		{
			DaoChuObject = go;
		}
		RectTransform childPos = pai.Find("Frist") as RectTransform;
		if (pai == Zuo || pai == You)
		{
			rect.anchoredPosition = (pai.GetChild(pai.childCount - 2) as RectTransform).anchoredPosition;
			if (isGameStart)
			{
				rect.anchoredPosition = childPos.anchoredPosition;
				StartCoroutine(BuPaiStart(pai, rect));
				isGameStart = false;
			}
			else
			{
				rect.anchoredPosition -= new Vector2(0, 85);
				
			}

		}
		else if (pai == Shang)
		{
			rect.anchoredPosition = (pai.GetChild(pai.childCount - 2) as RectTransform).anchoredPosition;
			if (isGameStart)
			{
				rect.anchoredPosition = childPos.anchoredPosition;
				StartCoroutine(BuPaiStart(pai, rect));
			}
			else {
				
				rect.anchoredPosition += new Vector2(120, 0);
			}
		}
		StartCoroutine("CountDown");
	}
	public void ShowGongNeng()
	{
		isShow = isShow == false ? true : false;
		GongnengButton.GetChild(0).gameObject.SetActive(isShow);
		if (isShow)
		{
			transform.parent.Find("talk").gameObject.SetActive(false);
			isTalk = false;
		}
	}
	public void IntTG()
	{
		isShow = false;
		GongnengButton.GetChild(0).gameObject.SetActive(isShow);
		if (isSkillUse)
		{
			isSkillUse = false;
			GuoPai();
		}
		
	    if (SureGame && IsAutoPut)
		{
			return;
		}
		StartTG = true;
		SendSeverTG(0);
	}
	public void TuiGuangButton()
	{
		Transform tg = skillMap.Find("TuoGuang");
		//防止功能
		if (isPlayerIng)
		{
			return;
		}
		SendSeverTG(1);
		StartTG = false;
		tg.gameObject.SetActive(false);
	}
	public void OutTable()
	{
		if (OutTabeCount > 0)
		{
			return;
		}
		OutTabeCount++;
		OutTableData outTable = new OutTableData();
		outTable.actionCode = "OutTableAction";
		outTable.seatNum = seatNum;
		outTable.memberId = UserId.memberId;
		string json = JsonMapper.ToJson(outTable);
		WebSoketCall.One().SendToWeb(json);
		jie.CleanData();
	}
	public void WanFaiWay()
	{
		Bridge._instance.LoadAbDate(LoadAb.Main, "rule");
		return;
	}

	public void GetWanFaiData(string data)
	{
		JsonData jsom = JsonMapper.ToObject(data);
		string json = (string)jsom["data"];
		WanFai.SetActive(true);
		WanFai.transform.Find("Scroll View/Viewport/Content/WContent").GetComponent<Text>().text = json;
		WanFai.transform.Find("Close").GetComponent<Button>().onClick.AddListener(() => { WanFai.SetActive(false); });
	}
	public void SendSeverTG(int status)
	{
		if (SureGame && !IsAutoPut)
		{
			HangUpCallInfo hang = new HangUpCallInfo();
			hang.actionCode = "HangUpAction";
			hang.Params = new HangUpCallInfo.HangUpStart();
			hang.Params.playerStatus = status;
			string json = JsonMapper.ToJson(hang);
			WebSoketCall.One().SendToWeb(json);
			Transform tg = skillMap.Find("TuoGuang");
			tg.gameObject.SetActive(true);
		}
		else if (SureGame && IsAutoPut)
		{
			return;
		}
		else
		{
			Transform tg = skillMap.Find("TuoGuang");
			tg.gameObject.SetActive(false);
			Prefabs.PopBubble("无法托管请等待进入游戏");
			return;
		}
	}

	public void Talks()
	{
		isTalk = isTalk == false ? true : false;
		transform.parent.Find("talk").gameObject.SetActive(isTalk);
		if (isShow)
		{
			GameObject.Find("gongneng").transform.GetChild(0).gameObject.SetActive(false);
			isShow = false;
		}
	}
	public void TurnNum(int turnnum)
	{
		for (int i = 0; i < TweenP.Count; i++)
		{
			TweenP[i].color = new Color(1, 1, 1, 1);
			TweenP[i].DOPause();
			TweenP[i].gameObject.SetActive(false);
		}
		if (turnnum == seatNum)
		{
			TweenP[0].gameObject.SetActive(true);
			TweenP[0].DOPlay();
		}
		else if (turnnum == youplaycount)
		{
			TweenP[1].gameObject.SetActive(true);
			TweenP[1].DOPlay();
		}
		else if (turnnum == zuoplaycount)
		{
			TweenP[3].gameObject.SetActive(true);
			TweenP[3].DOPlay();
		}
		else if (turnnum == shangplaycount)
		{
			TweenP[2].gameObject.SetActive(true);
			TweenP[2].DOPlay();
		}
		else
		{
			return;
		}
	}
	void TurnAnimator()
	{
		TweenP[0].DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
		TweenP[1].DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
		TweenP[2].DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
		TweenP[3].DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
	}
	//拖拽出牌
	public void DragMajiang(Transform obj,Image ObjImage)
	{
		int NeedPut = 0;
		int NeedInt = 0;
		if (!isOwn){return;}
			if (rascalCard == DrawCPai)
			{
				DarwPai.Add(DrawCPai);
			}
			for (int i = 1; i < DarwPai.Count; ++i)
			{
				int t = DarwPai[i];
				int j = i;
				while ((j > 0) && (DarwPai[j - 1] > t))
				{
					DarwPai[j] = DarwPai[j - 1];
					--j;
				}
				DarwPai[j] = t;
			}
			NeedPut = DarwPai.IndexOf(DrawCPai) + MySkillCount;
			if (reascalcount > 0)
			{
				if (NeedPut == Pai.childCount)
				{
					NeedPut -= reascalcount;
				}
				else if (NeedPut>(Pai.childCount - reascalcount)) { NeedPut = Pai.childCount - reascalcount; }
			}
			//需要合拢的位置和准备变的层级
			for (int i = 0; i < Pai.childCount; i++)
			{
				if (Pai.GetChild(i) == obj)
				{
					NeedInt = i + 1;
				}
			}
			if (NeedPut >= Pai.childCount)
			{
				NeedPut = Pai.childCount - 1;
			}
			//找到要变的位置
			Vector2 pos = (Pai.GetChild(NeedPut) as RectTransform).anchoredPosition;
			StartCoroutine(HelpAnim());
			//当前的位置
			RectTransform rr = Pai.GetChild(Pai.childCount - 1).transform as RectTransform;
			//if (NeedPut == NeedInt && NeedPut + 1 < Pai.childCount)
			//{ NeedPut += 1; }
			if (!CallAmByother)
			{
				if (NeedInt != Pai.childCount)
				{
					StartCoroutine(BuPaiHL(NeedInt, NeedPut, rr, pos));
				}
			}
			else
			{
				CallAmByother = false;
				for (int i = NeedInt; i < Pai.childCount; i++)
				{
					//合拢
					if (NeedInt != Pai.childCount)
					{
						(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(108, 0);
					}
				}
			}
			DestroyImmediate(obj.gameObject);
			//将对应的牌放入到指定位置
			//rr.anchoredPosition = pos;
			MaJiang.Instance.Majiang_ = null;
			//打出牌的数
			int DaiChuPai = 0;
			foreach (var key in list.Keys)
			{
				if (list[key] == ObjImage.sprite)
				{
					DaiChuPai = key;
					WebSocketInfo wsi = new WebSocketInfo();
					wsi.actionCode = "PlayAction";
					wsi.tableNum = tableNum;
					wsi.Params = new WebSocketInfo.data();
					wsi.Params.card = key;
					string json = JsonMapper.ToJson(wsi);
					WebSoketCall.One().SendToWeb(json);
					StopCoroutine("CountDown");
					Debug.Log("OutPut Card DragAction +()+" + key);
					isOwn = false;
				}
			}
			GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "MajiangRes", parent);
			go.transform.GetChild(0).GetComponent<Image>().sprite = ObjImage.sprite;
			DaoChuObject = go;
			Invoke("LocationConfirmation", 2.2f);
	}
	public List<int> ShowSkillCard(List<int> card, List<int> skillCard, Transform Pai_,bool GangShow)
	{
		List<int> IS = new List<int>();
		//有癞子牌先加入到手牌中
		if (Pai_ == Pai) { for (int i = 0; i < Pai_.childCount; i++) { Pai_.GetChild(i).GetChild(2).gameObject.SetActive(false); } }
		if (reascalcount > 0)
		{
			for (int i = 0; i < reascalcount; i++)
			{
			Pai_.GetChild(Pai_.childCount - 1 - i).GetChild(2).gameObject.SetActive(true);
			}
		}
		if (skillCard.Count>0&&skillCard!=null)
		{
			for (int i = 0; i < skillCard.Count; i++)
			{
				IS.Add(skillCard[i]);
				Image pic = Pai_.GetChild(i).GetComponent<Image>();
				pic.enabled = false;
				Pai_.GetChild(i).GetChild(0).gameObject.SetActive(false);
				Pai_.GetChild(i).GetChild(0).GetComponent<Image>().sprite = list[skillCard[i]];
				Transform t1 = Pai_.GetChild(i).Find("skillpai");
				t1.gameObject.SetActive(true);
				t1.GetChild(0).GetComponent<Image>().sprite = list[skillCard[i]];
				Pai.GetChild(i).gameObject.SetActive(true);
			}
		}
		//int DataCountCard = skillCard.Count + card.Count + reascalcount;
		//ContrastPai(Pai_.childCount, DataCountCard,Pai);
		if (card!=null&&card.Count>0)
		{
			for (int j = 0; j < card.Count; j++)
			{
				IS.Add(card[j]);
				int childNum = j + skillCard.Count;
				Pai_.GetChild(childNum).GetChild(0).GetComponent<Image>().sprite = list[card[j]];
			}
		}
		if (GangShow == true)
		{
		if (skillCard!=null&&skillCard.Count>0)
		{
			if (skillCard.Count >= 3)
			{
					int gangPos = BuGang(skillCard,GangCard);
					Debug.Log("GangCard" + GangCard);
					Debug.Log("GangPos"+gangPos);
					if (gangPos == (skillCard.Count - 1)) {
					Debug.Log(" peng peng peng +++++++++");
					//防止重复移动位置导致位置重合
					float a = Vector2.Distance((Pai.GetChild(skillCard.Count-1) as RectTransform).anchoredPosition, (Pai.GetChild(skillCard.Count-2) as RectTransform).anchoredPosition);
					Debug.Log(a);
						if (a<109) { 
					for (int i = skillCard.Count - 3; i < skillCard.Count; i++)
					{
					if (i == skillCard.Count - 1)
					{
						break;
					}
					else if (i == skillCard.Count - 3)
					{
						(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(14, 0);
					}
					else
					{
						(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(28, 0);
					}
					}
						}
					RectTransform rect = (Pai.GetChild(skillCard.Count - 1) as RectTransform);
					if (rect.anchoredPosition != (Pai.GetChild(skillCard.Count - 3) as RectTransform).anchoredPosition)
					{
						rect.anchoredPosition = (Pai.GetChild(skillCard.Count - 3) as RectTransform).anchoredPosition;
						rect.SetSiblingIndex(skillCard.Count - 2);
						rect.anchoredPosition += new Vector2(0, 29);
							//暗杠的显示
							if (isMingGang) {
								isMingGang = false; 
								GangController.DarkCardShow(rect, "prefabs/DarkGang",Pai); }
						for (int i = skillCard.Count; i < Pai.childCount; i++)
						{
						(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(100, 0);
						}
					}
					}
					else
					{
						Debug.Log("one shout +++++++++");
						RectTransform rect = (Pai.GetChild(skillCard.Count-1) as RectTransform);
						rect.GetComponent<Image>().enabled = false;
						rect.GetChild(0).gameObject.SetActive(false);
						rect.GetChild(1).gameObject.SetActive(true);
						rect.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[skillCard[gangPos]];
						rect.gameObject.SetActive(true);
						if (rect.anchoredPosition != (You.GetChild(gangPos-2) as RectTransform).anchoredPosition)
						{
							rect.anchoredPosition = (Pai.GetChild(gangPos-2) as RectTransform).anchoredPosition;
							rect.SetSiblingIndex(gangPos-1);
							rect.anchoredPosition += new Vector2(0, 29);
							for (int i = skillCard.Count; i < Pai.childCount; i++)
							{
								(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(100, 0);
							}
						}
						//暗杠的显示
						if (isMingGang)
						{
							isMingGang = false;
							GangController.DarkCardShow(rect, "prefabs/DarkGang",Pai);
						}
					}
			}
		}
		}
		Debug.Log(IS.Count);
		return IS;
	}
	public void CheckGangPoint(List<int> skillCard,int GangCard)
	{
		int gangPos = BuGang(skillCard, GangCard);
	}
	public void RefreshSkill(List<int> skillCard, Transform Pai_)
	{
		if (Pai_ == Pai) { for (int i = 0; i < Pai_.childCount; i++) { Pai_.GetChild(i).GetChild(2).gameObject.SetActive(false); } }
		if (reascalcount > 0)
		{
			for (int i = 0; i < reascalcount; i++)
			{
				Pai_.GetChild(Pai_.childCount - 1 - i).GetChild(2).gameObject.SetActive(true);
			}
		}
		for (int i = 0; i < skillCard.Count; i++)
		{
			Image pic = Pai_.GetChild(i).GetComponent<Image>();
			pic.enabled = false;
			Pai_.GetChild(i).GetChild(0).gameObject.SetActive(false);
			Pai_.GetChild(i).GetChild(0).GetComponent<Image>().sprite = list[skillCard[i]];
			Transform t1 = Pai_.GetChild(i).Find("skillpai");
			t1.gameObject.SetActive(true);
			t1.GetChild(0).GetComponent<Image>().sprite = list[skillCard[i]];
			Pai.GetChild(i).gameObject.SetActive(true);
		}
	}
	IEnumerator HelpAnim()
	{
		for (int i = 0; i < Pai.childCount; i++)
		{
			Pai.GetChild(i).GetChild(0).GetComponent<DragMajiang>().enabled=false;
		}
		yield return new WaitForSeconds(1.5f);
		for (int i = 0; i < Pai.childCount; i++)
		{
			Pai.GetChild(i).GetChild(0).GetComponent<DragMajiang>().enabled = true;
			yield return new WaitForSeconds(0.1f);
		}
	}
	IEnumerator BuPaiHL(int NeedInt, int NeedPut, RectTransform rect, Vector2 pos)
	{
		for (int i = NeedInt; i < Pai.childCount; i++)
		{
			//合拢
			if (NeedInt != Pai.childCount)
			{
				(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(108, 0);
			}
		}
		Vector2 poss = (Pai.GetChild(NeedPut) as RectTransform).anchoredPosition;
		Debug.Log(poss.x + ":" + poss.y);
		 //方便刷新先改变层级
		rect.SetSiblingIndex(NeedPut);
		yield return new WaitForSeconds(1f);
		//Vector2 pos = (Pai.GetChild(NeedPut) as RectTransform).anchoredPosition;
		for (int i = NeedPut; i < Pai.childCount; i++)
		{
			Debug.Log("NP" + i);
			(Pai.GetChild(i) as RectTransform).anchoredPosition += new Vector2(108, 0);
			Debug.Log(Pai.GetChild(i).name + ":" + (Pai.GetChild(i) as RectTransform).anchoredPosition.x);
		}
		rect.anchoredPosition += new Vector2(80, 0); 
		yield return new WaitForSeconds(1f);
		if (NeedPut == Pai.childCount)
		{
			rect.anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
			rect.anchoredPosition += new Vector2(108, 0);
		}
		else
		{
			rect.anchoredPosition += new Vector2(0, 140);
			pos += new Vector2(0, 140);
			poss += new Vector2(0, 140);
			rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, poss, 10);
			yield return new WaitForSeconds(0.1f);
			rect.anchoredPosition -= new Vector2(0, 140);
			rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 21);
		}
	}
	IEnumerator BuPaiAnimator(RectTransform rect)
	{
		rect.anchoredPosition = new Vector2(1277.4f, 21);
		rect.gameObject.SetActive(false);
		yield return new WaitForSeconds(1.8f);
		rect.gameObject.SetActive(true);
		rect.anchoredPosition += new Vector2(151, 70);
		yield return new WaitForSeconds(0.5f);
		rect.anchoredPosition -= new Vector2(0, 70);
	}
	IEnumerator OutTableIE()
	{
		isOverOutTableIE = false;
		int OutTableIEData_ = OutTableIEData.Count;
		for (int i = 0; i <OutTableIEData_; i++)
		{
			OutTableAction(OutTableIEData[i]);
			yield return null;
		}
		yield return null;
		if (OutTableIEData.Count > 0)
		{
			OutTableIEData.RemoveAt(OutTableIEData_-1);
			isOverOutTableIE = true;
		}
	}
	IEnumerator DetecConnection()
	{
		while (true)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				WebSoketCall.One().ws.Close();
			}
			if (!WebSoketCall.One().Check())
			{	
				if (ConnectAnimatorLine == false)
				{
					ConnectAnimatorLine = true;
					ConnectAnimator.gameObject.SetActive(true);
				}
				WebSoketCall.One().isLinkWS = false;
				WebSoketCall.One().isGame = false;
				WebSoketCall.One().FristLink = false;
				//Debug.Log("Game_" );
				WebSoketCall.One().ReonnectType = 1;
				if (NoGameStart == true)
				{
					GameGetLiang = true;
					NoGameStart = false;
					WebSoketCall.One().ReonnectType =0;	
				}
				if (robCount + TGCount >= 3)
				{
					WebSoketCall.One().ReonnectType = 0;
				}
				Debug.Log("Web"+WebSoketCall.One().ReonnectType);
				WebSoketCall.One().StartGame(WebSocketCallBack);
				Debug.Log(WebSoketCall.One().FristLink);
				if (WebSoketCall.One().FristLink == false)
				{
					WebSoketCall.One().FristLink = true;
				}
				
			}
			else
			{
				//ToDo网络延迟
				WebSoketCall.One().SendToWeb("{\"actionCode\":\"heartbeat\"}");
				if (ConnectAnimatorLine)
				{
					if (PlayerPrefs.GetString("Reconnect") == "Two")
					{
						PlayerPrefs.SetString("Reconnect", "one");
						Debug.Log("jiang");
						WebSoketCall.One().ws.Close();
						Member id = new Member();
						id.memberId =UserId.memberId.ToString();
						string json = JsonMapper.ToJson(id);
						JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/game/channel/close", json, Serr);
					}
					else
					{
						PlayerPrefs.SetString("Reconnect", "Two");
						Debug.Log("xin");
					}
					if (GameGetLiang == true)
					{
						GameGetLiang = false;
						Prefabs.PopBubble("网络不稳定请重新进入房间");
						StartCoroutine(OutLine());
					}
					//Prefabs.PopBubble("连接成功:.ﾟヽ(｡◕‿◕｡)ﾉﾟ.:｡+ﾟ");
					ConnectAnimatorLine = false;
					WebSoketCall.One().ReonnectType = 0;
					ConnectAnimator.gameObject.SetActive(false);
				}
			}
			yield return new WaitForSeconds(1f);
		}
	}
	IEnumerator OutLine()
	{
		yield return new WaitForSeconds(0.5f);
		UserId.GameState = false;
		SceneManager.LoadScene("liang");
	}
	public IEnumerator Expression(GameObject go)
	{
		yield return new WaitForSeconds(1);
		CloseExpressio(go);
	}
	/// <summary>
	/// 出牌信息
	/// </summary>
	/// <returns></returns>
	IEnumerator PlayerTry()
	{
		isOverStartPlayer = false;
		int PlayerActionDataofLink_ = PlayerActionDataOfLink.Count;
		for (int i = CurrentZXCount; i <PlayerActionDataOfLink.Count; i++)
		{
			PlayAction(PlayerActionDataOfLink[i]);
			yield return new WaitForSeconds(1f);
		}
		yield return null;
		isOverStartPlayer = true;
	}
	IEnumerator DrawPerform()
	{
		isOverStartDraw = false;
		int DrawActionData_ = DrawActionData.Count;
		for (int i = CurrentDrawCount; i < DrawActionData_; i++)
		{
			DrawAction(DrawActionData[i]);
			yield return new WaitForSeconds(1f);
		}
		yield return null;
		isOverStartDraw = true;
	}
	IEnumerator BuPaiStart(Transform pai, RectTransform rect)
	{
		yield return new WaitForSeconds(1.5f);
		if (pai == Zuo || pai == You)
		{
			if (rect != null) { 
			rect.anchoredPosition -= new Vector2(0, 700);
			isGameStart = false;
			yield return new WaitForSeconds(0.1f);
			rect.anchoredPosition -= new Vector2(0, 25);
			}
		}
		else if (pai == Shang)
		{
			if (rect != null) { 
			rect.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.2f);
			rect.gameObject.SetActive(true);
			rect.anchoredPosition += new Vector2(1020, 0);
			rect.anchoredPosition += new Vector2(0, 40);
			isGameStart = false;
			yield return new WaitForSeconds(0.1f);
			rect.anchoredPosition -= new Vector2(0, 40);
			}
		}
	}
	/// <summary>
	/// 时间结束要重连的事与时间倒计时
	/// </summary>
	/// <returns></returns>
	IEnumerator CountDown()
	{
		while (TotalTime >= 0)
		{
			TimeText.text = TotalTime.ToString();
			yield return new WaitForSeconds(1);
			if (TotalTime <= 7)
			{
				if (Audiocontroller.Instance.aso.clip == null)
				{
					Audiocontroller.Instance.aso.clip = Audiocontroller.Instance.ManCliplist[38];
					Audiocontroller.Instance.aso.Play();
				}
			}
			TotalTime--;
		}
		Audiocontroller.Instance.aso.Stop();
		Audiocontroller.Instance.aso.clip = null;
		TotalTime = 15;
		TimeText.text = TotalTime.ToString();
		StopCoroutine("CountDown");
		if (activePTrusteeship)
		{
			isOther = false;
			//自己回合进入托管自己不能出牌
			//等待别的;
			if (isSkillUse)
			{
				isSkillUse = false;
				GuoPai();
			}
			for (int i = pic.childCount - 1; i >= 0; i--)
			{
				Destroy(pic.GetChild(i).gameObject);
			}
			pic.gameObject.SetActive(false);
			if (!StartTG)
			{
				if (!IsAutoPut) {
					SendSeverTG(0);
					StartTG = true;
				}
			}
			activePTrusteeship = false;
		}
		else
		{
			activePTrusteeship = false;
			isOther = true;
		}
	}
	/// <summary>
	/// 用携程使多个托管信息能够同时显示
	/// </summary>
	/// <returns></returns>
	IEnumerator HangUp()
	{
		isHangUpOver = false;
		int HungUpData_ = HungUpData.Count;
		for (int i = 0; i < HungUpData.Count; i++)
		{
			HangUpAction(HungUpData[i]);
			yield return null;
		}
		yield return null;
		if (HungUpData.Count > 0)
		{
			HungUpData.RemoveAt(HungUpData_-1);
		}
		isHangUpOver = true;
	}
	IEnumerator ReallyShow()
	{
		isReallyShowOver = false;
		int ReallyInfo_ = ReallyInfo.Count;
		for (int i = 0; i < ReallyInfo_; i++)
		{
			ReadyAction(ReallyInfo[i]);
			yield return null;
		}
		yield return null;
		if(ReallyInfo.Count>0)
		ReallyInfo.RemoveAt(ReallyInfo_-1);
		isReallyShowOver = true;
	}
	IEnumerator ETable()
	{
		isOverEnterTable = false;
		int EnterTableIEData_ = EnterTableIEData.Count;
		for (int i =0; i < EnterTableIEData_; i++)
		{
			EnterTableAction(EnterTableIEData[i]);
			yield return null;
		}
		yield return null;
		if (EnterTableIEData.Count > 0)
		{
		EnterTableIEData.RemoveAt(EnterTableIEData_-1);
		}
		isOverEnterTable = true;
	}
	IEnumerator SentCardInfo(string jsonData)
	{
		yield return new WaitForSeconds(1);
		WebSoketCall.One().SendToWeb(jsonData);
	}
}
