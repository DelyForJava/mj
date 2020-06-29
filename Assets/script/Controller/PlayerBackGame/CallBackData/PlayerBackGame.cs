using DG.Tweening;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBackGame : MonoBehaviour {
	#region 参数
	//服务器字对图片
	Dictionary<int, Sprite> list = new Dictionary<int, Sprite>();
	[Header("玩家名字")]
	public List<Text> handTuoName = new List<Text>();
	[Header("头像框数据")]
	public List<Image> HeadFamer = new List<Image>();
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
	[Header("右边的牌")]
	public List<int> one = new List<int>();
	[Header("上边的牌")]
	public List<int> twotwo = new List<int>();
	[Header("左边的牌")]
	public List<int> three = new List<int>();
	[Header("庄家标志")]
	public List<Image> Bookpic = new List<Image>();
	[Header("自己的牌")]
	public Transform Pai;
	//自己出牌的放点
	[Header("出牌放点")]
	public Transform parent;
	[Header("当前摸牌的对象")]
	public Transform CurrentDuixiang;
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
	[Header("打出牌对象")]
	public GameObject DaoChuObject;
	[Header("牌动画_M")]
	public Animator anima;
	public Animator Yanima;
	public Animator Zanima;
	public Animator Sanima;
	public PlayerBackJie jie;
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
	[Header("当前打出的牌")]
	public int CurrentPlayerCard;
	[Header("创建房间的轮数")]
	public int Createround;
	[Header("自己技能的数量")]
	public int MySkillCount;
	//癞子牌
	public int rascalCard;
	public int MyrascalCard;
	public int yourascalCard;
	public int zourascalCard;
	public int shangrascalcard;
	//左边座位号
	public int zuoplaycount;
	//右边的座位号
	public int youplaycount;
	//上边的座位号
	public int shangplaycount;
	//当前摸牌对象；
	[Header("当前摸牌对象")]
	public int CurrentObject;
	public int DrawHuAction;
	int chuCard;
	//player现在对象
	[Header("Player现在的对象")]
	public int PlayerNum;
	//自己拥有的癞子数；
	public int reascalcount;
	[Header("进入房间存下的局数")]
	public int Rount_;
	public int CurrentSpeed;
	//胡牌的卡
	[Header("胡的牌")]
	public int HuPaiCard;
	[Header("倒计时秒数")]
	public int TotalTime = 15;
	[Header("摸牌阶段摸到的牌")]
	public int DrawCPai;
	[Header("存下的数据")]
	public string GameData;
	[Header("托管数据")]
	public string HangUpData;
	[Header("全部准备的信息")]
	public string AllReallyActionData;
	[Header("结束数据")]
	public string EndData;
	[Header("开始游戏")]
	public bool isGameStart;
	[Header("是否是吃碰要准备的事")]
	public bool CallAmByother;
	//是否是自己出牌
	public bool isOwn;
	[Header("G3位置是否使用")]
	public bool G3bool;
	public bool StatrFristPai;
	public bool isSkillUse;
	[Header("最后一局是否开始")]
	public bool isEndAction;
	[Header("游戏已经开局")]
	public bool SureGame;
	[Header("是否是碰牌后有杠情况")]
	public bool GangT3;
	[Header("右是否是碰牌后有杠情况")]
	public bool YouGangT3;
	[Header("左是否是碰牌后有杠情况")]
	public bool ZuoGangT3;
	[Header("上是否是碰牌后有杠情况")]
	public bool ShangGangT3;
	[Header("是否出牌携程执行完毕")]
	public bool isOverStartPlayer = true;
	[Header("是否摸牌携程执行完毕")]
	public bool isOverStartDraw = true;
	[Header("是否是执行出牌回调")]
	public bool isPlayerAction;
	[Header("是否是摸牌回调")]
	public bool isDrawAction;
	[Header("是否进入主动进入托管")]
	public bool activePTrusteeship;
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
	bool otherChiPeng;
	[Header("创建房间桌子的显示")]
	public Text TableNumShow;
	//牌的数量
	[Header("牌的数量显示")]
	public Text OtherPaiShow;
	[Header("当前轮数")]
	public Text CurrentRound;
	[Header("倒计时显示对象")]
	public Text TimeText;
	//癞子的图片
	public Image reascalPic;
	public Sprite newCord;
	public CreateLumSum CLS;
	public List<int> playerHandCard=new List<int>();
	#endregion
	void Awake()
	{
		//if (UserId.memberId < 1000)
		//{
		//	UserId.memberId = 11715;
		//}
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
		reascalPic.transform.parent.gameObject.SetActive(false);
		OtherPaiShow.transform.parent.gameObject.SetActive(false);
	}
	void Start()
	{
		GameData = UserId.CombatPlayback;
		UserId.CombatPlayback = null;
		Audiocontroller.GetSound();   //声音控制
		DarwPai = new List<int>();
		TableNumShow.gameObject.SetActive(false);
		CurrentRound.gameObject.SetActive(false);
		PlayerActionDataOfLink = new List<string>();
		DrawActionData = new List<string>();
		jie.maJiangPai = PaiSprite;
		//string s=File.ReadAllText("D:/新建文件夹/HuiFang.txt");
		PlayerBackData data =JsonMapper.ToObject<PlayerBackData>(GameData);
		if (data.data==null)
		{
			Prefabs.PopBubble("数据出错无法回放");
			GameMapManager.Instance.NormalLoadScene("liang");
			return;
		}
		PlayerBack playerBack = JsonMapper.ToObject<PlayerBack>(data.data[0].replayText);
		StartCoroutine(PlayerCall(playerBack.replayList));
	}
	public void PlayerBack()
	{
		PlayerBackData data = JsonMapper.ToObject<PlayerBackData>(GameData);
		PlayerBack playerBack = JsonMapper.ToObject<PlayerBack>(data.data[0].replayText);
		StartCoroutine(PlayerCall(playerBack.replayList));
	}
	public void Suspend()
	{
		Time.timeScale=Time.timeScale=1;
		CurrentSpeed= (int)Time.timeScale;
		GameObject.Find("DoubleSpeed").transform.GetChild(0).GetComponent<Text>().text = CurrentSpeed.ToString();
	}
	public void Stop()
	{
		Time.timeScale = 0;
		CurrentSpeed = (int)Time.timeScale;
		GameObject.Find("DoubleSpeed").transform.GetChild(0).GetComponent<Text>().text = CurrentSpeed.ToString();
	}
	public IEnumerator PlayerCall(List<string> Pc)
	{
		for (int i = 0; i < Pc.Count; i++)
		{
			JsonData data = JsonMapper.ToObject(Pc[i]);
			string saction = (string)data["actionCode"];
			switch (saction)
			{
				case "CreateTableAction":
				case "EnterTableAction":
				case "PassAction":
				case "ReadyAction":
				case "ReConnectAction":
				case "GameMessage":
				case "OutTableAction":
				case "error":
				//case "EndAction":
					continue;
			}
			WebSocketCallBack(Pc[i]);
			yield return new WaitForSeconds(1.5f);
		}
	}
	void WebSocketCallBack(string edata)
	{
		Debug.Log(edata);
		JsonData data = JsonMapper.ToObject(edata);
		string saction = (string)data["actionCode"];
		switch (saction)
		{
			case "ChiAction":
				ChAction(edata);
				break;
			case "DrawAction":
				DrawAction(edata);
				break;
			case "GangAction":
				GangAction(edata);
				break;
			case "HangUpAction":
				HangUpAction(edata);
				break;
			case "PengAction":
				PengAction(edata);
				break;
			case "PlayAction":
				PlayAction(edata);
				break;
			case "AllReadyAction":
				AllReallyAction(edata);
				break;
			case "HuAction":
				HuAction(edata);
				break;
			case "OutTableAction":
				break;
			case "EndAction":
				EndAction(edata);
				break;
		}
	}
	void EndAction(string edate)
	{
		CurrentRound.gameObject.SetActive(false);
		CLS.HuList.Add(WebSoketCall.One().HuDateInfo);
		CLS.TableNum = tableNum;
		CLS.Id = playerId;
		jie.StartAllCombute();
	}
	void HuAction(string edata)
	{
		TurnNum(0);
		TotalTime = 15; TimeText.text = TotalTime.ToString();
		StopCoroutine("CountDown"); TotalTime = 15; TimeText.text = TotalTime.ToString();
		SureGame = false;
		isSkillUse = false;		
		HuclassInfo huclass = JsonMapper.ToObject<HuclassInfo>(edata);
		jie.Initle();
		jie.listCard = list;
		jie.mySeatNum = seatNum;
		jie.youSeatNum = youplaycount;
		jie.zuoSeatNum = zuoplaycount;
		jie.shangSeatNum = shangplaycount;
		jie.HuTheme = huclass.seatNum;
		jie.rascalCard = rascalCard;
		jie.HuDescrite = huclass.description;
		jie.gangPointsList = huclass.gangPointsList;
		jie.goldChangeList = huclass.goldChangeList;
		List<int> MyCard = new List<int>();	
		for (int i = 0; i < huclass.players.Count; i++)
		{
			if (huclass.players[i].seatNum == seatNum)
			{
				int ShowCount = huclass.players[i].skillCards.Count + huclass.players[i].handCards.Count+huclass.players[i].rascalCount;
				KeepDateReally(ShowCount, Pai);
				if (jie.HuCard == 0) { jie.HuCard = DrawCPai;}
				List<int> hu = new List<int>();
				hu = huclass.players[i].handCards;
				for (int j = 0; j < hu.Count; j++)
				{
					if (playerHandCard.Contains(hu[j]))
					{
						continue;
					}
					jie.HuCard = hu[j];
				}
				reascalcount = huclass.players[i].rascalCount;
				MyCard = ShowSkillCard(huclass.players[i].handCards, huclass.players[i].skillCards, Pai);
				int count = 0;
				for (int j = huclass.players[i].skillCards.Count; j < MyCard.Count; j++)
				{
					Image pic = Pai.GetChild(j).GetComponent<Image>();
					pic.enabled = false;
					Pai.GetChild(j).GetChild(0).gameObject.SetActive(false);
					Transform t1 = Pai.GetChild(j).Find("skillpai");
					t1.gameObject.SetActive(true);
					t1.GetChild(0).GetComponent<Image>().sprite = list[MyCard[j]];
				}
				for (int j = huclass.players[i].skillCards.Count; j < Pai.childCount; j++)
				{
					(Pai.GetChild(j) as RectTransform).anchoredPosition -= new Vector2(count * 13, 0);
					count++;
				}
			}
			if (huclass.players[i].seatNum == youplaycount)
			{
				int ShowCount = huclass.players[i].skillCards.Count + huclass.players[i].handCards.Count + huclass.players[i].rascalCount;
				KeepDateReally(ShowCount, You);
				if (jie.HuCard == 0) { jie.HuCard = DrawCPai; }
				reascalcount = huclass.players[i].rascalCount;
				one= ShowSkillCard(huclass.players[i].handCards, huclass.players[i].skillCards, You);
			}
			if (huclass.players[i].seatNum == zuoplaycount)
			{
				int ShowCount = huclass.players[i].skillCards.Count + huclass.players[i].handCards.Count + huclass.players[i].rascalCount;
				KeepDateReally(ShowCount, Zuo);
				if (jie.HuCard == 0) { jie.HuCard = DrawCPai; }
				reascalcount = huclass.players[i].rascalCount;
				three= ShowSkillCard(huclass.players[i].handCards, huclass.players[i].skillCards, Zuo);	
			}
			if (huclass.players[i].seatNum == shangplaycount)
			{
				int ShowCount = huclass.players[i].skillCards.Count + huclass.players[i].handCards.Count + huclass.players[i].rascalCount;
				KeepDateReally(ShowCount, Shang);
				if (jie.HuCard == 0) { jie.HuCard = DrawCPai; }
				reascalcount = huclass.players[i].rascalCount;
				twotwo= ShowSkillCard(huclass.players[i].handCards, huclass.players[i].skillCards, Shang);
			}
		}
		jie.MyPaiCard = MyCard;
		jie.YouPaiCard = one;
		jie.ShangPaiCard = twotwo;
		jie.ZuoPaiCard = three;
		if (huclass.seatNum == 0)
		{
			for (int i = 0; i < huclass.players.Count; i++)
			{
				if (huclass.players[i].seatNum == seatNum)
				{
					int count = 0;
					for (int j = huclass.players[i].skillCards.Count; j < Pai.childCount; j++)
					{
						(Pai.GetChild(j) as RectTransform).anchoredPosition -= new Vector2(count * 10, 0);
						count++;
					}
					for (int j = 0; j < Pai.childCount; j++)
					{
						Pai.GetChild(j).GetChild(2).gameObject.SetActive(false);
					}
				}
			}
			FlowBureau();
			jie.isFlowBurea = true;
			return;
		}
		
		for (int i = 0; i < Pai.childCount; i++)
		{
			Pai.GetChild(i).GetChild(2).gameObject.SetActive(false);
		}
		//StopAllCoroutines();
		Invoke("SetActive_", 1);
	}
	void FlowBureau()
	{
		StopCoroutine("CountDown");
		for (int i = 0; i < Pai.childCount; i++)
		{
			Pai.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
			Pai.GetChild(i).GetChild(1).gameObject.SetActive(true);
		}
		Invoke("SetActive_", 1);
	}
	public void SetActive_()
	{
		for (int i = 0; i < HeadFamer.Count; i++)
		{
			HeadFamer[i].gameObject.SetActive(false);
		}
		if (PlayerActionDataOfLink != null) { PlayerActionDataOfLink.Clear(); }
		if (DrawActionData != null) { DrawActionData.Clear(); }
		if (UserId.JieCreateRoom)
		{
			Createround -= 1;
			if (Createround > 0)
			{
				CurrentRound.text = Createround.ToString();
				JieTrabsform.gameObject.SetActive(true);
				jie.StartJieSuang();
			}
			else
			{
				UserId.JieCreateRoom = false;
				return;
			}
		}
		else {
			StopAllCoroutines(); 
			jie.StartJieSuang(); JieTrabsform.gameObject.SetActive(true); }
	}
	void AllReallyAction(string jsonData)
	{
		StatrFristPai = true;
		reascalPic.transform.parent.gameObject.SetActive(true);
		OtherPaiShow.transform.parent.gameObject.SetActive(true);
		SureGame = true;
		//打开动画
		for (int i = 0; i < maJiangpai.Length; i++)
		{
			maJiangpai[i].gameObject.SetActive(true);
		}
		isGameStart = true;
		AllReadActionInfo ARA = JsonMapper.ToObject<AllReadActionInfo>(jsonData);
		OtherPaiShow.text = "84";
		Bookmaker = ARA.turnNum;
		jie.tableNum = ARA.players[0].tableNum;
		List<Sprite> MyPai = new List<Sprite>();
		newCord = list[ARA.newCard];
		DrawCPai = ARA.newCard;
		rascalCard = ARA.rascalCard;
		reascalPic.sprite = list[rascalCard];
		for (int i = 0; i < ARA.players.Count; i++)
		{
			if (!dic.ContainsKey(ARA.players[i].uid))
			{
				dic.Add(ARA.players[i].uid, ARA.players[i].seatNum);
				playerId.Add(ARA.players[i].uid);
				Member member = new Member();
				member.memberId = ARA.players[i].uid.ToString();
				string json = JsonMapper.ToJson(member);
				JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() +"/api/member/getinfo", json, PlayerInfo);
			}
			Bookpic[i].gameObject.SetActive(false);
			if (ARA.players[i].uid == UserId.memberId)
			{
				MyrascalCard = ARA.players[i].rascalCount;
				tableNum = ARA.players[i].tableNum;
				seatNum = ARA.players[i].seatNum;
				zuoplaycount = ARA.players[i].frontNum;
				if (seatNum == 4)
				{
					youplaycount = 1;
					shangplaycount = 2;
				}
				else
					youplaycount = seatNum + 1;
				if (youplaycount == 4)
					shangplaycount = 1;
				else
					shangplaycount = youplaycount + 1;
				reascalcount = ARA.players[i].rascalCount;
				DarwPai = ARA.players[i].handCards;
				if (ARA.turnNum == seatNum) { ARA.players[i].handCards.Remove(ARA.newCard); }
				if (rascalCard == ARA.newCard) { reascalcount -= 1; }
				List<int> IntPai = ShowSkillCard(ARA.players[i].handCards, ARA.players[i].skillCards,Pai);
				DarwPai.Add(ARA.newCard);
			}
		}
		for (int i = 0; i < ARA.players.Count; i++)
		{
			if (ARA.players[i].hu > 0)
			{
				if (seatNum == ARA.players[i].seatNum)
				{
					isHu = true;
				}
			}
			if (ARA.players[i].seatNum == youplaycount)
			{
				yourascalCard = ARA.players[i].rascalCount;
				if (ARA.turnNum==youplaycount){ ARA.players[i].handCards.Remove(ARA.newCard);}
				reascalcount = ARA.players[i].rascalCount;
				if (rascalCard == ARA.newCard) { reascalcount -= 1; }
				one =ShowSkillCard(ARA.players[i].handCards, ARA.players[i].skillCards,You);
			}
			else if (ARA.players[i].seatNum == zuoplaycount)
			{
				zourascalCard=ARA.players[i].rascalCount;
				if (ARA.turnNum == zuoplaycount) { ARA.players[i].handCards.Remove(ARA.newCard); }
				reascalcount = ARA.players[i].rascalCount;
				if (rascalCard==ARA.newCard){reascalcount -= 1;}
				three = ShowSkillCard(ARA.players[i].handCards, ARA.players[i].skillCards, Zuo);
			}
			else if (ARA.players[i].seatNum == shangplaycount)
			{
				shangrascalcard = ARA.players[i].rascalCount;
				if (ARA.turnNum == shangplaycount) { ARA.players[i].handCards.Remove(ARA.newCard); }
				reascalcount = ARA.players[i].rascalCount;
				if (rascalCard == ARA.newCard) { reascalcount -= 1; }
				twotwo = ShowSkillCard(ARA.players[i].handCards, ARA.players[i].skillCards, Shang);
			}
		}
		//庄家显示
		if (Bookmaker == seatNum)
		{
			Bookpic[0].gameObject.SetActive(true);
		}
		else if (Bookmaker == youplaycount) { Bookpic[1].gameObject.SetActive(true); }
		else if (Bookmaker == shangplaycount) { Bookpic[2].gameObject.SetActive(true); }
		else if (Bookmaker == zuoplaycount) { Bookpic[3].gameObject.SetActive(true); }
		TurnNum(ARA.turnNum);
		if (seatNum == ARA.turnNum)
		{
			int num = int.Parse(OtherPaiShow.text);
			OtherPaiShow.text = (num - 1).ToString();
			isOwn = true;
			BuPai(newCord);
			for (int i = 0; i < reascalcount; i++)
			{
				Pai.GetChild(Pai.childCount - 2 - i).GetChild(2).gameObject.SetActive(true);
			}
		}
		else if (ARA.turnNum == youplaycount)
		{
			activePTrusteeship = false;
			int num = int.Parse(OtherPaiShow.text);
			OtherPaiShow.text = (num - 1).ToString();
			OtherBuPai(You, newCord, "YouRes");
			for (int i = 0; i < reascalcount; i++)
			{
				Pai.GetChild(Pai.childCount - 1 - i).GetChild(2).gameObject.SetActive(true);
			}
		}
		else if (ARA.turnNum == shangplaycount)
		{
			activePTrusteeship = false;
			int num = int.Parse(OtherPaiShow.text);
			OtherPaiShow.text = (num - 1).ToString();
			OtherBuPai(Shang, newCord, "ShangRes");
			for (int i = 0; i < reascalcount; i++)
			{
				Pai.GetChild(Pai.childCount - 1 - i).GetChild(2).gameObject.SetActive(true);
			}
		}
		else
		{
			activePTrusteeship = false;
			int num = int.Parse(OtherPaiShow.text);
			OtherPaiShow.text = (num - 1).ToString();
			OtherBuPai(Zuo, newCord, "ZuoRes");
			for (int i = 0; i < reascalcount; i++)
			{
				Pai.GetChild(Pai.childCount - 1 - i).GetChild(2).gameObject.SetActive(true);
			}
		}
	}
	/// <summary>
	/// 玩家信息
	/// </summary>
	/// <param name="data"></param>
	public void PlayerInfo(string data)
	{
		PlayerMessageInfo playerMessage = JsonMapper.ToObject<PlayerMessageInfo>(data);
		playerInfo.Add(data);
		if (seatNum == dic[playerMessage.data.id])
		{
			JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, HandPic[0]);
			handTuoName[0].text = playerMessage.data.nickname;
			HeadFamer[0].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
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
	}
	/// <summary>
	/// 玩家动作产生的参数
	/// </summary>
	/// <param name="jsonData"></param>
	void PlayAction(string jsonData)
	{
		playerActionInfo player = JsonMapper.ToObject<playerActionInfo>(jsonData);
		//Audiocontroller.Instance.MajiangManPlayer(player.card);
		PlayerNum = player.turnNum;
		chuCard = player.card;
		//上家打出的牌
		for (int i = 0; i < player.players.Count; i++)
		{
			if (player.players[i].seatNum == player.turnNum)
			{
				//上一家打出牌
				if (player.players[i].frontNum == youplaycount)
				{
					if (otherChiPeng) {
						DestroyImmediate(You.GetChild(You.childCount - 1).gameObject);
						otherChiPeng = false;
					}
					else
					{
						DestroyImmediate(CurrentDuixiang.gameObject);
					}
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "You_MaJiang", YP);
					DaoChuObject = go;
					go.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 270));
					go.transform.GetChild(0).rotation *= Quaternion.Euler(new Vector3(0, 0, 180));
					go.transform.GetChild(0).GetComponent<Image>().sprite = list[chuCard];
				}
				else if (player.players[i].frontNum == shangplaycount)
				{
					if (otherChiPeng) { 
						DestroyImmediate(Shang.GetChild(Shang.childCount - 1).gameObject);
						otherChiPeng = false;
					}
					else
					{
						DestroyImmediate(CurrentDuixiang.gameObject);
					}
					GameObject go2 = Bridge._instance.LoadAbDate(LoadAb.Game, "ShangMajiangRes", SP);
					DaoChuObject = go2;
					go2.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 0));
					go2.transform.GetChild(0).rotation *= Quaternion.Euler(new Vector3(0, 0, 0));
					go2.transform.GetChild(0).GetComponent<Image>().sprite = list[chuCard];
					//(go2 as RectTransform).
					if (player.card == rascalCard)
					{
						go2.transform.GetChild(1).gameObject.SetActive(true);
					}
				}
				else if (player.players[i].frontNum == zuoplaycount)
				{
					if (otherChiPeng)
					{
						DestroyImmediate(Zuo.GetChild(Zuo.childCount - 1).gameObject);
						otherChiPeng = false;
					}
					else
					{
						Destroy(CurrentDuixiang.gameObject);
					}
					GameObject go1 = Bridge._instance.LoadAbDate(LoadAb.Game, "Zuo_Majiang", ZP);
					DaoChuObject = go1;
					go1.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 90));
					go1.transform.GetChild(0).rotation *= Quaternion.Euler(new Vector3(0, 0, -90));
					go1.transform.GetChild(0).GetComponent<Image>().sprite = list[chuCard];
					if (player.card == rascalCard)
					{
						go1.transform.GetChild(1).gameObject.SetActive(true);
					}
				}
				else if (player.players[i].frontNum == seatNum)
				{
					playerHandCard = player.players[i].handCards;
					ChoseCardS(player.card);
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "MajiangRes", parent);
					go.transform.GetChild(0).GetComponent<Image>().sprite = list[player.card];
					DaoChuObject = go;
				}
			}
		}
		List<Sprite> MyPai = new List<Sprite>();
		//关闭计时将时间恢复到15秒
		StopCoroutine("CountDown");
		TotalTime = 15; TimeText.text = TotalTime.ToString();
		CurrentZXCount++;
		//刷新存储别的玩家的手牌
		for (int i = 0; i < player.players.Count; i++)
		{
			if (player.players[i].uid == UserId.memberId)
			{
				reascalcount = player.players[i].rascalCount;
				MyrascalCard = player.players[i].rascalCount;
				ShowSkillCard(player.players[i].handCards, player.players[i].skillCards, Pai);
			}
			else if (player.players[i].seatNum == youplaycount) {
				reascalcount = player.players[i].rascalCount;
				yourascalCard = player.players[i].rascalCount;
				ShowSkillCard(player.players[i].handCards, player.players[i].skillCards, You);}
			else if (player.players[i].seatNum == shangplaycount)
			{
				reascalcount = player.players[i].rascalCount;
				shangrascalcard = player.players[i].rascalCount;
				ShowSkillCard(player.players[i].handCards, player.players[i].skillCards, Shang);
			}
			else if (player.players[i].seatNum == zuoplaycount)
			{
				reascalcount = player.players[i].rascalCount;
				zourascalCard = player.players[i].rascalCount;
				ShowSkillCard(player.players[i].handCards, player.players[i].skillCards, Zuo);
			}
		}
		//没有技能时
		if (player.skillPlayerCount>0)
		{
			for (int i = 0; i < player.players.Count; i++)
			{
				if (player.players[i].skillMap.Count > 0)
				{
					foreach (var key in player.players[i].skillMap.Keys)
					{
						if (player.players[i].skillMap[key] == 1)
						{//当为吃时；
							if (key == "chi")
							{
								if (player.players[i].seatNum == seatNum)
								{
									PlayerNum = seatNum;
								}
								else if (player.players[i].seatNum == youplaycount)
								{
									PlayerNum = youplaycount;
								}
								else if (player.players[i].seatNum == zuoplaycount) { PlayerNum = zuoplaycount; }
								else if (player.players[i].seatNum == shangplaycount) { PlayerNum = shangplaycount; }
							}
							else if (key == "peng")
							{
								if (player.players[i].seatNum == seatNum)
								{
									PlayerNum = seatNum;
								}
								else if (player.players[i].seatNum == youplaycount)
								{
									PlayerNum = youplaycount;
								}
								else if (player.players[i].seatNum == zuoplaycount) { PlayerNum = zuoplaycount; }
								else if (player.players[i].seatNum == shangplaycount) { PlayerNum = shangplaycount; }
							}
							else if (key == "gang")
							{
								GrabCards = player.card;
								if (player.players[i].seatNum == seatNum)
								{
									PlayerNum = seatNum;
								}
								else if (player.players[i].seatNum == youplaycount)
								{
									PlayerNum = youplaycount;
								}
								else if (player.players[i].seatNum == zuoplaycount) { PlayerNum = zuoplaycount; }
								else if (player.players[i].seatNum == shangplaycount) { PlayerNum = shangplaycount; }
							}
						}
					}
				}
			}
			StartCoroutine("CountDown");
		}	
	}
	/// <summary>
	/// 碰产生的回调
	/// </summary>
	/// <param name="jsonData"></param>
	void PengAction(string jsonData)
	{
		isSkillUse = false;
		StopCoroutine("CountDown"); TotalTime = 15; TimeText.text = TotalTime.ToString();
		PengActionInfo peng = JsonMapper.ToObject<PengActionInfo>(jsonData);
		Destroy(DaoChuObject);
		TurnNum(peng.seatNum);
		//Audiocontroller.Instance.MajiangManPlayer(62);
		if (peng.seatNum == seatNum)
		{
			StartCoroutine(SkillShow("skillone", "peng"));
			CallAmByother = true;
			DarwPai = peng.handCards;
			MySkillCount = peng.skillCards.Count;
			GameObject go = BuPai(list[peng.handCards[peng.handCards.Count - 1]]);
			(go.transform as RectTransform).anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
			(go.transform as RectTransform).anchoredPosition += new Vector2(108, 0);
			reascalcount = MyrascalCard;
			ShowSkillCard(peng.handCards, peng.skillCards,Pai);
			//显示
			for (int i = peng.skillCards.Count - 2; i < peng.skillCards.Count; i++)
			{
				if (i == peng.skillCards.Count - 2)
				{
					(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(14, 0);
				}
				else
				{
					(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(28, 0);
				}
			}
			isSkillUse = false;
			//玩家出牌到谁；
			PlayerNum = seatNum;
			//OverDarw = true;
		}
		else if (peng.seatNum == youplaycount)
		{
			StartCoroutine(SkillShow("youskills", "youpeng"));
			for (int i = 0; i < peng.skillCards.Count; i++)
			{
				if (peng.seatNum == youplaycount)
				{
					Transform tf = You.GetChild(i);
					if (tf.childCount >= 2)
					{
						tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[peng.skillCards[i]];
					}
					else
					{
					tf.GetChild(0).GetChild(0).GetComponent<Image>().sprite = list[peng.skillCards[i]];
					}
				}
			}
			AllPai("YouRes", You, peng.handCards, peng.skillCards, yourascalCard);
			CurrentDuixiang = null;
			otherChiPeng = true;
			//玩家出牌到谁；
			PlayerNum = youplaycount;
			//OverDarw = true;
		}
		else if (peng.seatNum == shangplaycount)
		{
			StartCoroutine(SkillShow("shangskills", "shangpeng"));
			for (int i = 0; i < peng.skillCards.Count; i++)
			{
				if (peng.seatNum == shangplaycount)
				{
					Transform tf = Shang.GetChild(i);
					if (tf.childCount >= 2)
					{
						tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[peng.skillCards[i]];
					}
					else
					{
					tf.GetChild(0).GetChild(0).GetComponent<Image>().sprite = list[peng.skillCards[i]];
					}
					tf.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
				}
			}
			AllPai("ShangRes", Shang, peng.handCards, peng.skillCards, shangrascalcard);
			CurrentDuixiang = null;
			otherChiPeng = true;
			PlayerNum = shangplaycount;
			//OverDarw = true;
		}
		else
		{
			StartCoroutine(SkillShow("zuoskills", "zuopeng"));
			for (int i = 0; i < peng.skillCards.Count; i++)
			{
				if (peng.seatNum == zuoplaycount)
				{
					Transform tf = Zuo.GetChild(i);
					if (tf.childCount >= 2)
					{
						tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[peng.skillCards[i]];
					}
					else
					{
						tf.GetChild(0).GetChild(0).GetComponent<Image>().sprite = list[peng.skillCards[i]];
					}
				}
			}
			AllPai("ZuoRes", Zuo, peng.handCards, peng.skillCards, zourascalCard);
			//OverDarw = true;
			CurrentDuixiang = null;
			PlayerNum = zuoplaycount;
			otherChiPeng = true;
		}
	}
	public void AllPai(string CardRes, Transform Obj, List<int> handCards, List<int> skillCards, int rascalcount_)
	{
		GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, CardRes, Obj);
		go.transform.GetChild(0).gameObject.SetActive(false);
		go.transform.GetChild(1).gameObject.SetActive(true);
		go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[handCards[handCards.Count - 1]];
		(go.transform as RectTransform).anchoredPosition = (Obj.GetChild(Obj.childCount - 2) as RectTransform).anchoredPosition;
		if (Obj!=Shang)
		{
			if (Obj==Zuo)
			{
				(go.transform as RectTransform).anchoredPosition -= new Vector2(0, 30);
			}
			else { (go.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);}
		}
		else
		{
			(go.transform as RectTransform).anchoredPosition +=new Vector2(68, 0);
		}
		reascalcount = rascalcount_;
		ShowSkillCard(handCards, skillCards, Obj);
	}
	List<int> ShowSkillCard(List<int> card,List<int> skillCard,Transform Pai_)
	{
		List<int> IS = new List<int>();
		//有癞子牌先加入到手牌中
		if (Pai_ == Pai) {for (int i = 0; i < Pai_.childCount; i++){Pai_.GetChild(i).GetChild(2).gameObject.SetActive(false);}}
		if (reascalcount > 0)
		{
			Debug.Log(":p"+reascalcount);
			for (int i = 0; i < reascalcount; i++)
			{
				if (Pai==Pai_)
				{
					Pai_.GetChild(Pai_.childCount - 1 - i).GetChild(2).gameObject.SetActive(true);
				}	
				card.Add(rascalCard);
			}
		}
		int ShowCount = card.Count + skillCard.Count;
		for (int i = 0; i < skillCard.Count; i++)
		{
			IS.Add(skillCard[i]);
			if (Pai_==Pai)
			{	
				Image pic = Pai_.GetChild(i).GetComponent<Image>();
				pic.enabled = false;
				Pai_.GetChild(i).GetChild(0).gameObject.SetActive(false);
				Pai_.GetChild(i).GetChild(0).GetComponent<Image>().sprite = list[skillCard[i]];
				Transform t1 = Pai_.GetChild(i).Find("skillpai");
				t1.gameObject.SetActive(true);
				t1.GetChild(0).GetComponent<Image>().sprite = list[skillCard[i]];
			}
			else
			{
				if (Pai_.GetChild(i).childCount >=2) 
				{
					Pai_.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[skillCard[i]];
				}
				else
				{ Pai_.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = list[skillCard[i]]; }
				
			}
		}
		for (int j = 0; j < card.Count; j++)
		{
			IS.Add(card[j]);
			int childNum = j + skillCard.Count;
			if (Pai==Pai_)
			{
				Pai_.GetChild(childNum).GetChild(0).GetComponent<Image>().sprite = list[card[j]];			
			}
			else {
				if (Pai_.childCount - 1 < childNum)
				{
					Debug.Log(Pai_.childCount - 1);
					childNum = Pai_.childCount - 1;
				}
				if (Pai_.GetChild(childNum).childCount>=2)
				{
					Pai_.GetChild(childNum).GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[card[j]];
				}
				else
				{
					Pai_.GetChild(childNum).GetChild(0).GetChild(0).GetComponent<Image>().sprite = list[card[j]];
				}   			
			}
				
		}
		return IS;
	}
	public void KeepDateReally(int showCount,Transform obj)
	{
		if (showCount!=obj.childCount)
		{
		
			if ((showCount-obj.childCount)>0)
			{
				Debug.Log("I say aaa you say zuo 生成 ");
				if (Pai == obj)
				{
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "PaiRes_My", Pai);
					RectTransform pos = go.transform as RectTransform;
					(pos.transform as RectTransform).anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
					(pos.transform as RectTransform).anchoredPosition += new Vector2(108, 0);
				}
				else if (You == obj) { Progression("YouRes", You); }
				else if (Zuo == obj) { Progression("ZuoRes", Zuo); }
				else if (Shang == obj) { Progression("ShangRes", Shang); }
			}
			else if ((obj.childCount - showCount) > 0)
			{
				Debug.Log("I say bububu you say awsl  消除对象");
				int ti = obj.childCount - showCount;
				for (int i = 0; i < ti; i++)
				{
					Destroy(obj.GetChild(obj.childCount - 1 - i).gameObject);
				}
			}
		}
	}
	void Progression(string s,Transform obj)
	{
		GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, s, obj);
		go.transform.GetChild(0).gameObject.SetActive(false);
		go.transform.GetChild(1).gameObject.SetActive(true);
		(go.transform as RectTransform).anchoredPosition = (obj.GetChild(obj.childCount - 2) as RectTransform).anchoredPosition;
		(go.transform as RectTransform).anchoredPosition -= new Vector2(0, 30);
	}
	/// <summary>
	/// 杠产生的回调
	/// </summary>
	/// <param name="jsonData"></param>
	void GangAction(string jsonData)
	{
		isSkillUse = false;
		//Audiocontroller.Instance.MajiangManPlayer(63);
		GangInfo d = JsonMapper.ToObject<GangInfo>(jsonData);
		if (d.seatNum == seatNum)
		{
			StartCoroutine(SkillShow("skillone", "gang"));
			PlayerNum = seatNum;
			if (GangT3)
			{
				Transform tar = Pai.GetChild(Pai.childCount - 1);
				tar.GetComponent<Image>().enabled = false;
				tar.GetChild(0).gameObject.SetActive(false);
				tar.GetChild(1).gameObject.SetActive(true);
				tar.GetChild(1).GetChild(0).GetComponent<Image>().sprite = tar.GetChild(0).GetComponent<Image>().sprite;
				int SkillGangPoint = 0;
				for (int i = 0; i < d.skillCards.Count; i++)
				{
					if (d.skillCards[i] == DrawCPai)
					{
						SkillGangPoint = i;
						break;
					}
				}
				
				int NG = SkillGangPoint;
				////找到要磊的目标
				RectTransform rect = (Pai.GetChild(NG + 1) as RectTransform);
				//生成了一个直接放在碰牌上的;
				tar.transform.SetSiblingIndex(NG + 2);
				(tar as RectTransform).anchoredPosition = rect.anchoredPosition;
				(tar as RectTransform).anchoredPosition += new Vector2(0, 29);
				GangT3 = false;
			}
			else
			{
				if (isMingGang)
				{
					(DaoChuObject.transform as RectTransform).anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
					(DaoChuObject.transform as RectTransform).anchoredPosition += new Vector2(109, 0);
					isMingGang = false;
				}
				else
				{
					Destroy(DaoChuObject);
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "PaiRes_My", Pai);
					#region 点击事件的添加
					go.name = "XP";
					Transform childT = go.transform.GetChild(0);
					childT.GetComponent<Image>().sprite = DaoChuObject.transform.GetChild(0).GetComponent<Image>().sprite;
					#endregion
					RectTransform pos = go.transform as RectTransform;
					(pos.transform as RectTransform).anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
					(pos.transform as RectTransform).anchoredPosition += new Vector2(108, 0);
				}
				reascalcount = MyrascalCard;
				ShowSkillCard(d.handCards, d.skillCards, Pai);
				for (int i = d.skillCards.Count - 3; i < d.skillCards.Count; i++)
				{
					if (i == d.skillCards.Count - 1)
					{
						break;
					}
					else if (i == d.skillCards.Count - 3)
					{
						(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(14, 0);
					}
					else
					{
						(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(28, 0);
					}
				}
				RectTransform rect = (Pai.GetChild(d.skillCards.Count - 1) as RectTransform);
				rect.anchoredPosition = (Pai.GetChild(d.skillCards.Count - 3) as RectTransform).anchoredPosition;
				rect.SetSiblingIndex(d.skillCards.Count - 2);
				rect.anchoredPosition += new Vector2(0, 29);
				isMyGang = true;
			}
		}
		if (d.seatNum == youplaycount)
		{
			StartCoroutine(SkillShow("youskills", "yougang"));
			if (YouGangT3)
			{
				//生成了一个直接放在碰牌上的
				GameObject go = You.GetChild(You.childCount - 1).gameObject;
				go.transform.GetChild(0).gameObject.SetActive(false);
				go.transform.GetChild(1).gameObject.SetActive(true);
				go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[DrawCPai];
				int SkillGangPoint = 0;
				for (int i = 0; i < d.skillCards.Count; i++)
				{
					if (d.skillCards[i] == DrawCPai)
					{
						SkillGangPoint = i;
						break;
					}
				}
				int NG = SkillGangPoint;
				//找到要磊的目标
				RectTransform rect = (You.GetChild(NG + 1) as RectTransform);
				go.transform.SetSiblingIndex(NG + 3);
				(go.transform as RectTransform).anchoredPosition = rect.anchoredPosition;
				YouGangT3 = false;
			}
			else
			{
				if (isYouMingGang)
				{
					(DaoChuObject.transform as RectTransform).anchoredPosition = (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition;
					(DaoChuObject.transform as RectTransform).anchoredPosition -= new Vector2(0,50);
					DaoChuObject.transform.GetChild(0).gameObject.SetActive(false);
					DaoChuObject.transform.GetChild(1).gameObject.SetActive(true);
					DaoChuObject.transform.GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
					DaoChuObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite= list[d.handCards[d.handCards.Count - 1]];
					isYouMingGang = false;
				}
				else
				{
					Destroy(DaoChuObject);
					//GameObject go = Prefabs.LoadCell("majiang#YouRes", You);
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "YouRes", You);
					go.transform.GetChild(0).gameObject.SetActive(false);
					go.transform.GetChild(1).gameObject.SetActive(true);
					go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
					(go.transform as RectTransform).anchoredPosition = (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition;
					(go.transform as RectTransform).anchoredPosition -= new Vector2(0, 30);
				}
				reascalcount = yourascalCard;
				ShowSkillCard(d.handCards, d.skillCards, You);
				if (d.skillCards != null)
				{
					for (int i = 0; i < d.skillCards.Count; i++)
					{
						Transform tf = You.GetChild(i);
						tf.GetChild(0).GetChild(0).GetComponent<Image>().sprite = list[d.skillCards[i]];
					}

				}
				RectTransform rect = (You.GetChild(d.skillCards.Count - 1) as RectTransform);
				rect.anchoredPosition = (You.GetChild(d.skillCards.Count - 3) as RectTransform).anchoredPosition;
			}
		}
		if (d.seatNum == shangplaycount)
		{
			StartCoroutine(SkillShow("shangskills", "shanggang"));
			if (ShangGangT3)
			{
				GameObject go = Shang.GetChild(Shang.childCount - 1).gameObject;
				go.transform.GetChild(0).gameObject.SetActive(false);
				go.transform.GetChild(1).gameObject.SetActive(true);
				go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[DrawCPai];
				go.transform.GetChild(1).GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, 360));
				int SkillGangPoint = 0;
				for (int i = 0; i < d.skillCards.Count; i++)
				{
					if (d.skillCards[i] == DrawCPai)
					{
						SkillGangPoint = i;
						break;
					}
				}
				int NG = SkillGangPoint;
				//找到要磊的目标
				RectTransform rect = (Shang.GetChild(NG + 1) as RectTransform);
				(go.transform as RectTransform).anchoredPosition = rect.anchoredPosition;
				(go.transform as RectTransform).anchoredPosition += new Vector2(0, 18);
				go.transform.SetSiblingIndex(NG + 3);
				ShangGangT3 = false;
			}
			else
			{
				if (isShangMingGang)
				{
					(Shang.GetChild(Shang.childCount - 1).transform as RectTransform).anchoredPosition = (Shang.GetChild(Shang.childCount - 2) as RectTransform).anchoredPosition;
					(Shang.GetChild(Shang.childCount - 1).transform as RectTransform).anchoredPosition += new Vector2(68, 0);
					Shang.GetChild(Shang.childCount - 1).GetChild(0).gameObject.SetActive(false);
					Shang.GetChild(Shang.childCount - 1).GetChild(1).gameObject.SetActive(true);
					Shang.GetChild(Shang.childCount - 1).transform.GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
					Shang.GetChild(Shang.childCount-1).transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
					isShangMingGang = false;
				}
				else
				{
					Destroy(DaoChuObject);
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "ShangRes", Shang);
					go.transform.GetChild(0).gameObject.SetActive(false);
					go.transform.GetChild(1).gameObject.SetActive(true);
					go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
					(go.transform as RectTransform).anchoredPosition = (Shang.GetChild(Shang.childCount - 2) as RectTransform).anchoredPosition;
					(go.transform as RectTransform).anchoredPosition += new Vector2(68, 0);
				}
				reascalcount = shangrascalcard;
				ShowSkillCard(d.handCards, d.skillCards, Shang);
				if (d.skillCards != null)
				{
					for (int i = 0; i < d.skillCards.Count; i++)
					{
						Transform tf = Shang.GetChild(i);
						tf.GetChild(0).GetChild(0).GetComponent<Image>().sprite = list[d.skillCards[i]];
					}
				}
				RectTransform rect = (Shang.GetChild(d.skillCards.Count - 1) as RectTransform);
				rect.anchoredPosition = (Shang.GetChild(d.skillCards.Count - 3) as RectTransform).anchoredPosition;
				rect.anchoredPosition += new Vector2(0, 20);
			}
		}
		if (d.seatNum == zuoplaycount)
		{
			StartCoroutine(SkillShow("zuoskills", "zuogang"));
			if (ZuoGangT3)
			{
				GameObject go1 = Zuo.GetChild(Zuo.childCount - 1).gameObject;
				go1.transform.GetChild(0).gameObject.SetActive(false);
				go1.transform.GetChild(1).gameObject.SetActive(true);
				go1.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[DrawCPai];
				int SkillGangPoint = 0;
				for (int i = 0; i < d.skillCards.Count; i++)
				{
					if (d.skillCards[i] == DrawCPai)
					{
						SkillGangPoint = i;
						break;
					}
				}
				
				int NG = SkillGangPoint;
				//找到要磊的目标
				RectTransform rect = (Zuo.GetChild(NG) as RectTransform);
				(go1.transform as RectTransform).anchoredPosition = rect.anchoredPosition;
				go1.transform.SetSiblingIndex(NG + 3);
				ZuoGangT3 = false;
			}
			else
			{
				if (isZuoMingGang)
				{
					(DaoChuObject.transform as RectTransform).anchoredPosition = (Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition;
					(DaoChuObject.transform as RectTransform).anchoredPosition -= new Vector2(0, 30);
					DaoChuObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
					DaoChuObject.transform.GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
					DaoChuObject.transform.GetChild(0).gameObject.SetActive(false);
					DaoChuObject.transform.GetChild(1).gameObject.SetActive(true);
					isZuoMingGang = false;
				}
				else
				{
					Destroy(DaoChuObject);
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "ZuoRes", Zuo);
					go.transform.GetChild(0).gameObject.SetActive(false);
					go.transform.GetChild(1).gameObject.SetActive(true);
					go.transform.GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
					go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
					(go.transform as RectTransform).anchoredPosition = (Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition;
					(go.transform as RectTransform).anchoredPosition -= new Vector2(0, 30);
				}
				reascalcount = zourascalCard;
				ShowSkillCard(d.handCards, d.skillCards, Zuo);
				if (d.skillCards != null)
				{
					for (int i = 0; i < d.skillCards.Count; i++)
					{
						Transform tf = Zuo.GetChild(i);
						tf.GetChild(0).GetChild(0).GetComponent<Image>().sprite = list[d.skillCards[i]];
					}
				}
				RectTransform rect1 = (Zuo.GetChild(d.skillCards.Count - 1) as RectTransform);
				rect1.anchoredPosition = (Zuo.GetChild(d.skillCards.Count - 3) as RectTransform).anchoredPosition;
			}
		}
		if (d.rob > 0)
		{
			if (d.rob == seatNum)
			{
				StartCoroutine(SkillShow("skillone", "qianggang"));
				jie.HuCard = GrabCards;
				HuPaiCard = GrabCards;
				QianGangShow(d.seatNum, d.skillCards);
			}
			else if (d.rob == youplaycount)
			{
				StartCoroutine(SkillShow("youskills", "youqianggang"));
				jie.HuCard = GrabCards;
				GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "YouRes", You);
				go.transform.GetChild(0).gameObject.SetActive(false);
				go.transform.GetChild(1).gameObject.SetActive(true);
				go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
				(go.transform as RectTransform).anchoredPosition = (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition;
				(go.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);
				QianGangShow(d.seatNum, d.skillCards);
			}
			else if (d.rob == zuoplaycount)
			{
				StartCoroutine(SkillShow("zuoskills", "zuoqianggang"));
				jie.HuCard = GrabCards;
				GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "ZuoRes", Zuo);
				go.transform.GetChild(0).gameObject.SetActive(false);
				go.transform.GetChild(1).gameObject.SetActive(true);
				go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
				(go.transform as RectTransform).anchoredPosition = (Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition;
				(go.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);
				QianGangShow(d.seatNum, d.skillCards);
			}
			else
			{
				StartCoroutine(SkillShow("shangskills", "shangqianggang"));
				jie.HuCard = GrabCards;
				GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "ShangRes", Shang);
				go.transform.GetChild(0).gameObject.SetActive(false);
				go.transform.GetChild(1).gameObject.SetActive(true);
				go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[d.handCards[d.handCards.Count - 1]];
				(go.transform as RectTransform).anchoredPosition = (Shang.GetChild(Shang.childCount - 2) as RectTransform).anchoredPosition;
				(go.transform as RectTransform).anchoredPosition += new Vector2(50,0);
				QianGangShow(d.seatNum, d.skillCards);
			}
		}
	}
	void QianGangShow(int seatNum_,List<int> skillcards)
	{
		if (seatNum_ == youplaycount)
		{
			if (!G3bool)
			{
				You.GetChild(skillcards.Count - 1).gameObject.SetActive(false);
			}
			else
			{
				G3bool = false;
				You.GetChild(G3Position).gameObject.SetActive(false);
			}
		}
		else if (seatNum_ == seatNum)
		{
			if (!G3bool)
			{
				Pai.GetChild(skillcards.Count - 1).gameObject.SetActive(true);
			}
			else
			{
				G3bool = false;
				Pai.GetChild(G3Position).gameObject.SetActive(true);
			}
		}
		else if (seatNum_ == shangplaycount)
		{
			if (!G3bool)
			{
				Shang.GetChild(skillcards.Count - 1).gameObject.SetActive(false);
			}
			else
			{
				G3bool = false;
				Shang.GetChild(G3Position).gameObject.SetActive(false);
			}
		}
		else if (seatNum_ == zuoplaycount)
		{
			if (!G3bool)
			{
				Zuo.GetChild(skillcards.Count - 1).gameObject.SetActive(false);
			}
			else
			{
				G3bool = false;
				Zuo.GetChild(G3Position).gameObject.SetActive(false);
			}
		}
	}
	/// <summary>
	/// 摸牌回调
	/// </summary>
	/// <param name="jsonData"></param>
	void DrawAction(string jsonData)
	{
		DrawActionInfo da = JsonMapper.ToObject<DrawActionInfo>(jsonData);
		DrawCPai = da.newCard;
		PlayerNum = da.seatNum;
		StopCoroutine("CountDown"); TotalTime = 15; TimeText.text = TotalTime.ToString();
		CurrentDrawCount++;
		TurnNum(da.seatNum);
		if (da.player.hu > 0)
		{
			jie.HuTheme = da.seatNum;
			jie.HuCard = da.newCard;
			DrawHuAction = da.newCard;
			jie.maJiangPai = PaiSprite;
			if (da.seatNum == seatNum)
			{
				isSkillUse = true;
				isHu = true;
			}
		}
		newCord = list[da.newCard];
		if (da.player.skillMap.Count != 0)
		{
			GrabCards = da.newCard;
			if (da.player.skillMap["gang"] == 2)
				{
					if (da.seatNum == seatNum)
					{
						isSkillUse = true;
						isMingGang = true;
					}
					else if (da.seatNum == youplaycount)
					{
						isYouMingGang = true;
					}
					else if (da.seatNum == shangplaycount)
					{
						isShangMingGang = true;
					}
					else
					{
						isZuoMingGang = true;
					}
				}
			if (da.player.skillMap["gang"] == 3)
				{
					G3bool = true;
					if (da.seatNum == seatNum)
					{
						G3Position = ScreenPosition(da.newCard, da.player.skillCards);
						isSkillUse = true; GangT3 = true;
					}
					else if (da.seatNum == youplaycount)
					{
						G3Position = ScreenPosition(da.newCard, da.player.skillCards);
						YouGangT3 = true;
					}
					else if (da.seatNum == shangplaycount)
					{
						G3Position = ScreenPosition(da.newCard, da.player.skillCards);
						ShangGangT3 = true;
					}
					else
					{
						G3Position = ScreenPosition(da.newCard, da.player.skillCards);
						ZuoGangT3 = true;
					}
				}
		}	
		#region 摸牌判定
		if (da.seatNum == seatNum)
		{
			DarwPai = da.player.handCards; MySkillCount = da.player.skillCards.Count;
			activePTrusteeship = true;
			int num = int.Parse(OtherPaiShow.text);
			OtherPaiShow.text = (num - 1).ToString();
			BuPai(newCord);
			if (isMyGang)
			{
				#region My杠
				for (int i = da.player.skillCards.Count; i < Pai.childCount; i++)
				{
					(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(100, 0);
				}
				#endregion
				isMyGang = false;
			}
		}
		else if (da.seatNum == youplaycount)
		{
			int num = int.Parse(OtherPaiShow.text);
			OtherPaiShow.text = (num - 1).ToString();
			one.Add(da.newCard);
			OtherBuPai(You, newCord, "YouRes");
		}
		else if (da.seatNum == shangplaycount)
		{
			int num = int.Parse(OtherPaiShow.text);
			OtherPaiShow.text = (num - 1).ToString();
			twotwo.Add(da.newCard);
			OtherBuPai(Shang, newCord, "ShangRes");
		}
		else if (da.seatNum == zuoplaycount)
		{
			int num = int.Parse(OtherPaiShow.text);
			OtherPaiShow.text = (num - 1).ToString();
			three.Add(da.newCard);
			OtherBuPai(Zuo, newCord, "ZuoRes");
		}
		#endregion
	}
	int ScreenPosition(int card, List<int> skillCards)
	{
		int SameCard = 0;
		for (int i = 0; i < skillCards.Count; i++)
		{
			if (skillCards[i] == card)
			{
				SameCard = i;
				break;
			}
		}
		return SameCard;
	}
	/// <summary>
	/// 吃牌
	/// </summary>
	/// <param name="jsonData"></param>
	void ChAction(string jsonData)
	{
		StopCoroutine("CountDown"); TotalTime = 15; TimeText.text = TotalTime.ToString();
		//Audiocontroller.Instance.MajiangManPlayer(61);
		ChipaiInfo chi = JsonMapper.ToObject<ChipaiInfo>(jsonData);
		DarwPai = chi.handCards; MySkillCount = chi.skillCards.Count;
		Destroy(DaoChuObject);
		TurnNum(chi.seatNum);
		if (chi.seatNum == seatNum)
		{
			StartCoroutine(SkillShow("skillone", "chi"));
			CallAmByother = true;
			GameObject go = BuPai(list[chi.handCards[chi.handCards.Count - 1]]);
			(go.transform as RectTransform).anchoredPosition = (Pai.GetChild(Pai.childCount - 2).transform as RectTransform).anchoredPosition;
			(go.transform as RectTransform).anchoredPosition += new Vector2(108, 0);
			reascalcount = MyrascalCard;
			ShowSkillCard(chi.handCards, chi.skillCards,Pai);
			for (int i = chi.skillCards.Count - 2; i < chi.skillCards.Count; i++)
			{
				if (i == chi.skillCards.Count - 2)
				{
					(Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(14, 0);
				}
				else { (Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(28, 0); }
			}
			//OverDarw = true;
			//玩家出牌到谁；
			PlayerNum = seatNum;
			isSkillUse = false;
		}
		else if (chi.seatNum == youplaycount)
		{
			StartCoroutine(SkillShow("youskills", "youchi"));
			for (int i = 0; i < chi.skillCards.Count; i++)
			{
				Transform tf = You.GetChild(i);
				if (tf.childCount >= 2) {
					tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[chi.skillCards[i]];
				}
				else
				{
				tf.GetChild(0).GetChild(0).GetComponent<Image>().sprite = list[chi.skillCards[i]];
				}
			}
			AllPai("YouRes", You, chi.handCards, chi.skillCards, yourascalCard);
			PlayerNum = youplaycount;
			//OverDarw = true;
			otherChiPeng = true;
			CurrentDuixiang = null;
		}
		else if (chi.seatNum == shangplaycount)
		{
			StartCoroutine(SkillShow("shangskills", "shangchi"));
			for (int i = 0; i < chi.skillCards.Count; i++)
			{
				Transform tf = Shang.GetChild(i);
				if (tf.childCount >= 2)
				{
					tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[chi.skillCards[i]];
				}
				else
				{
					tf.GetChild(0).GetChild(0).GetComponent<Image>().sprite = list[chi.skillCards[i]];
				}
			}
			AllPai("ShangRes", Shang, chi.handCards, chi.skillCards, shangrascalcard);
			PlayerNum = shangplaycount;
			//OverDarw = true;
			CurrentDuixiang = null;
			otherChiPeng = true;
		}
		else if (chi.seatNum == zuoplaycount)
		{
			StartCoroutine(SkillShow("zuoskills", "zuochi"));
			for (int i = 0; i < chi.skillCards.Count; i++)
			{
				Transform tf = Zuo.GetChild(i);
				if (tf.childCount >= 2)
				{
					tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = list[chi.skillCards[i]];
				}
				else
				{
					tf.GetChild(0).GetChild(0).GetComponent<Image>().sprite = list[chi.skillCards[i]];
				}
			}
			AllPai("ZuoRes", Zuo, chi.handCards, chi.skillCards, zourascalCard);
			PlayerNum = zuoplaycount;
			CurrentDuixiang = null;
			otherChiPeng = true;
		}
	}
	IEnumerator  SkillShow(string obj,string objSkill)
	{
		Transform SM = GameObject.Find(obj).transform.Find(objSkill);
		SM.gameObject.SetActive(true);
		yield return new WaitForSeconds(1);
		SM.gameObject.SetActive(false);
	}
	public void GoSetting()
	{
		//Audiocontroller.Instance.PlayAudio("Back");
		Bridge._instance.LoadAbDate(LoadAb.MainTwo, "set");
	}
	public GameObject BuPai(Sprite sprite)
	{
		RectTransform childPos = Pai.GetChild(Pai.childCount - 1) as RectTransform;
		GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "PaiRes_My", Pai);
		if (sprite == list[rascalCard]){go.transform.GetChild(2).gameObject.SetActive(true);}
		go.name = "XP";
		Transform childT = go.transform.GetChild(0);
		childT.GetComponent<Image>().sprite = sprite;
		RectTransform pos = go.transform as RectTransform;
		pos.position = childPos.position;
		//Audiocontroller.Instance.MajiangManPlayer(66);
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
	{
		GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, paiRes, pai);
		go.transform.GetChild(0).gameObject.SetActive(false);
		go.transform.GetChild(1).gameObject.SetActive(true);
		go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = sprite;
		CurrentDuixiang = go.transform;
		RectTransform rect = go.transform as RectTransform;
		if (isZuoMingGang || isYouMingGang || isShangMingGang)
		{
			DaoChuObject = go;
		}
		RectTransform childPos = pai.Find("Frist") as RectTransform;
		if (pai == Zuo || pai == You)
		{
			rect.anchoredPosition = childPos.anchoredPosition;
			if (isGameStart)
			{
				StartCoroutine(BuPaiStart(pai, rect));
			}
			else
			{
				rect.anchoredPosition = (pai.GetChild(pai.childCount - 2) as RectTransform).anchoredPosition;		
				rect.anchoredPosition -= new Vector2(0, 120);
				isGameStart = false;
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
			else
			{
				rect.anchoredPosition += new Vector2(120, 0);
			}
		}
		StartCoroutine("CountDown");
	}
	void TurnNum(int turnnum)
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
		else{return;}
	}
	void TurnAnimator()
	{
		TweenP[0].DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
		TweenP[1].DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
		TweenP[2].DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
		TweenP[3].DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
	}
	void ChoseCardS(int card)
	{
		int NeedPut = 0;
		int NeedInt = 0;
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
		NeedPut = DarwPai.IndexOf(DrawCPai)+ MySkillCount;
		if (reascalcount > 0)
		{
		if (NeedPut == Pai.childCount)
		{
			NeedPut -= reascalcount;
		}
		else if (NeedPut > (Pai.childCount - reascalcount)) { NeedPut = Pai.childCount - reascalcount; }
		}
		//需要合拢的位置和准备变的层级
		for (int i = Pai.childCount-1; i >0; i--)
		{

			if (Pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite==list[card])
			{
				NeedInt = i + 1;
				break;
			}
		}
		if (NeedPut >= Pai.childCount)
		{
			NeedPut = Pai.childCount - 1;
		}
		if (NeedInt==0){ NeedInt += 1;}
		Transform game = Pai.GetChild(NeedInt-1);
		//找到要变的位置
		Vector2 pos = (Pai.GetChild(NeedPut) as RectTransform).anchoredPosition;
		RectTransform rr = Pai.GetChild(Pai.childCount - 1).transform as RectTransform;
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
		DestroyImmediate(game.gameObject);
	}
	void HangUpAction(string jsonData)
	{
		HangUpActionData hang = JsonMapper.ToObject<HangUpActionData>(jsonData);
		if (hang.playerStatus <= 0)
		{
			if (hang.seatNum == seatNum)
			{
				GameObject.Find("SkillMap").transform.Find("TuoGuang").gameObject.SetActive(true);
			}
			if (hang.seatNum == youplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("YouTrusteeship").gameObject.SetActive(true);
			}
			else if (hang.seatNum == zuoplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("ZuoTrusteeship").gameObject.SetActive(true);
			}
			else if (hang.seatNum == shangplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("ShangTrusteeship").gameObject.SetActive(true);
			}
		}
		else
		{
			if (hang.seatNum == seatNum)
			{
				GameObject.Find("SkillMap").transform.Find("TuoGuang").gameObject.SetActive(false);
			}
			if (hang.seatNum == youplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("YouTrusteeship").gameObject.SetActive(false);
			}
			else if (hang.seatNum == zuoplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("ZuoTrusteeship").gameObject.SetActive(false);
			}
			else if (hang.seatNum == shangplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("ShangTrusteeship").gameObject.SetActive(false);
			}
		}
	}	
	public void SpeedController()
	{
		if (CurrentSpeed<=1)
		{CurrentSpeed++;}
		else { CurrentSpeed--;}	
		Time.timeScale = CurrentSpeed;
		GameObject.Find("DoubleSpeed").transform.GetChild(0).GetComponent<Text>().text = CurrentSpeed.ToString();
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
		//方便刷新先改变层级
		rect.SetSiblingIndex(NeedPut);
		yield return new WaitForSeconds(0.3f);
		for (int i = NeedPut; i < Pai.childCount; i++)
		{
			(Pai.GetChild(i) as RectTransform).anchoredPosition += new Vector2(108, 0);
		}
		rect.anchoredPosition += new Vector2(80, 0);
		yield return new WaitForSeconds(0.3f);
		if (NeedPut == Pai.childCount)
		{
			rect.anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
			rect.anchoredPosition += new Vector2(108, 0);
		}
		else
		{
			rect.anchoredPosition += new Vector2(0, 140);
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
		yield return new WaitForSeconds(1f);
		rect.gameObject.SetActive(true);
		rect.anchoredPosition += new Vector2(151, 70);
		yield return new WaitForSeconds(0.2f);
		rect.anchoredPosition -= new Vector2(0, 70);
	}
	IEnumerator BuPaiStart(Transform pai, RectTransform rect)
	{
		if (pai == Zuo || pai == You)
		{
			rect.anchoredPosition -= new Vector2(0, 700);
			isGameStart = false;
			yield return new WaitForSeconds(0.3f);
			rect.anchoredPosition -= new Vector2(0, 25);
		}
		else if (pai == Shang)
		{
			rect.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.3f);
			rect.gameObject.SetActive(true);
			rect.anchoredPosition += new Vector2(1020, 0);
			rect.anchoredPosition += new Vector2(0, 40);
			isGameStart = false;
			yield return new WaitForSeconds(0.3f);
			rect.anchoredPosition -= new Vector2(0, 40);
		}
	}
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
	}
}
