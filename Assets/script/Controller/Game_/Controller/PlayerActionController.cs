using DG.Tweening;
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerActionController : MonoBehaviour {

	public Game_Controller Game_;
	public GameObject Point_;
	public void PlayerAction(string edate)
	{
		playerActionInfo player = JsonMapper.ToObject<playerActionInfo>(edate);
		int TG = 0;
		for (int i = 0; i < player.players.Count; i++)
		{
			if (player.players[i].playerStatus == 1)
			{
				TG++;
			}
			if (player.players[i].uid == UserId.memberId)
			{
				if (player.players[i].playerStatus == 0)
				{
					if (Game_.robCount < 3)
					{
						Game_.StartTG = true;
					}
				}
				else if (player.players[i].playerStatus == 1)
				{
					if (Game_.robCount < 3)
					{
						Game_.StartTG = false;
					}
				}
			}
		}
		Game_.TGCount = TG;
		Audiocontroller.Instance.MajiangManPlayer(player.card);
		Game_.PlayerNum = player.turnNum;
		Game_.chuCard = player.card;
		//上家打出的Pai
		FrontNumOutTable(player);
		Game_.anima.enabled = false;
		Game_.Yanima.enabled = false;
		Game_.Sanima.enabled = false;
		Game_.Zanima.enabled = false;
		Game_.StopCountDown();
		Game_.CurrentZXCount++;
		//刷新和存储玩家手牌
		Reflish(player);
		Game_.isPlayerIng = false;
		Game_.ShowSameCard(Game_.Pai.GetChild(0).GetChild(0), false, true);
		AddOutPut();
		//技能与摸牌
		SkillShow(player);
	}
	public void SkillShow(playerActionInfo player)
	{
		if (player.skillPlayerCount<=0)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable) 
			{ Debug.Log("No interNet"); 
			  return; }
			if (Game_.StartTG)
			{
				//if (Game_.robCount >= 3)
				//{
				//	if (player.turnNum == Game_.seatNum)
				//	{
				//		Debug.Log("Ko Ko Da You");
				//		ActionInfo action = new ActionInfo();
				//		action.actionCode = "DrawAction";
				//		action.Params = new ActionInfo.Data();
				//		string json = JsonMapper.ToJson(action);
				//		WebSoketCall.One().SendToWeb(json);
				//	}
				//}
			}
			else
			{
				if (player.turnNum == Game_.seatNum)
				{
					Debug.Log("eee  www  www ");
					ActionInfo action = new ActionInfo();
					action.actionCode = "DrawAction";
					action.Params = new ActionInfo.Data();
					string json = JsonMapper.ToJson(action);
					WebSoketCall.One().SendToWeb(json);
				}
			}
		}
		else
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{ Debug.Log("SkillNo"); 
				return; }
			if (Game_.StartTG)
			{
				Game_.StopCountDown();
				//if (Game_.robCount >= 3)
				//{
				//	for (int i = 0; i < player.players.Count; i++)
				//	{
				//		if (player.players[i].uid == UserId.memberId)
				//		{
				//			if (player.players[i].skillMap.ContainsKey("chi"))
				//			{
				//				Debug.Log("chi" + player.players[i].skillMap["chi"]);
				//				if (player.players[i].skillMap["chi"] == 1) { Game_.GuoPai(); return; }
				//			}
				//			if (player.players[i].skillMap.ContainsKey("peng")) { Debug.Log("peng" + player.players[i].skillMap["peng"]); if (player.players[i].skillMap["peng"] == 1) { Game_.GuoPai(); return; } }
				//			if (player.players[i].skillMap.ContainsKey("gang")) { Debug.Log("gang" + player.players[i].skillMap["gang"]); if (player.players[i].skillMap["gang"] == 1) { Game_.GuoPai(); return; } }
				//		}
				//		else { continue; }
				//	}
				//}
				return;
			}
			for (int i = 0; i < player.players.Count; i++)
			{
				if (player.players[i].skillMap.Count > 0)
				{
					if (player.players[i].skillMap.ContainsKey("chipeng"))
					{
						if (player.players[i].skillMap["chipeng"] > 0)
						{
							if (player.players[i].seatNum == Game_.seatNum)
							{
								Transform CP = Game_.skillMap.Find("skillone/chi");
								CP.GetChild(0).gameObject.SetActive(true);
							}
						}
					}
					foreach (var key in player.players[i].skillMap.Keys)
					{
						if (player.players[i].skillMap[key] == 1)
						{//当为吃时；
							if (key == "chi")
							{
								if (player.players[i].seatNum == Game_.seatNum)
								{
									Game_.activePTrusteeship = true;
									Game_.isSkillUse = true;
									GameObject go = Game_.skillMap.Find("skillone/chi").gameObject;
									go.SetActive(true);
									#region chi判断情况
									if (Game_.pic.childCount > 0)
									{
										int picCount = Game_.pic.childCount;
										for (int j = 0; j < picCount; j++)
										{
											Destroy(Game_.pic.GetChild(j).gameObject);
										}
									}
									List<string> s = JugeChi(player.players[i].handCards, player.card, "My");
									for (int j = 0; j < s.Count; j++)
									{
										switch (s[j])
										{
											case "zhong":
												GameObject game = Bridge._instance.LoadAbDate(LoadAb.Game, "chi", Game_.pic);
												game.name = "zhong";
												ChiAddEvent(game, Game_.chiType_Zhong, "zhong", player.card);
												break;
											case "shang":
												GameObject game1 = Bridge._instance.LoadAbDate(LoadAb.Game, "chi", Game_.pic);
												game1.name = "shang";
												ChiAddEvent(game1, Game_.chiType_shang, "shang", player.card);
												break;
											case "xia":
												GameObject game2 = Bridge._instance.LoadAbDate(LoadAb.Game, "chi", Game_.pic);
												game2.name = "xia";
												ChiAddEvent(game2, Game_.chiType_xia, "xia", player.card);
												break;
										}
									}
									#endregion
									Game_.skillMap.Find("skillone/guo").gameObject.SetActive(true);
								}
								else if (player.players[i].seatNum == Game_.youplaycount)
								{
									Game_.PlayerNum = Game_.youplaycount;
								}
								else if (player.players[i].seatNum == Game_.zuoplaycount) { Game_.PlayerNum = Game_.zuoplaycount; }
								else if (player.players[i].seatNum == Game_.shangplaycount) { Game_.PlayerNum = Game_.shangplaycount; }
							}
							else if (key == "peng")
							{
								if (player.players[i].seatNum == Game_.seatNum)
								{
									Game_.activePTrusteeship = true;
									Game_.isSkillUse = true;
									Game_.skillMap.Find("skillone/guo").gameObject.SetActive(true);
									GameObject go = Game_.skillMap.Find("skillone/peng").gameObject;
										go.GetComponent<Button>().onClick.RemoveAllListeners();
										go.GetComponent<Button>().onClick.AddListener(() =>
										{
											Game_.PlayerNum = Game_.seatNum;
											Game_.StopCountDown();
											 ActionInfo info = new ActionInfo();
											info.actionCode = "PengAction";
											info.Params = new ActionInfo.Data();
											info.Params.card = player.card;
											string josn = JsonMapper.ToJson(info);
											WebSoketCall.One().SendToWeb(josn);
										});
									go.SetActive(true);
								}
								else if (player.players[i].seatNum == Game_.youplaycount)
								{
									Game_.PlayerNum = Game_.youplaycount;
								}
								else if (player.players[i].seatNum == Game_.zuoplaycount) { Game_.PlayerNum = Game_.zuoplaycount; }
								else if (player.players[i].seatNum == Game_.shangplaycount) { Game_.PlayerNum = Game_.shangplaycount; }
							}
							else if (key == "gang")
							{
								Game_.GrabCards = player.card;
								Game_.GangCard = player.card;
								if (player.players[i].seatNum == Game_.seatNum)
								{
									Game_.activePTrusteeship = true;
									Game_.isSkillUse = true;
									Game_.skillMap.Find("skillone/guo").gameObject.SetActive(true);
									GameObject go = Game_.skillMap.Find("skillone/gang").gameObject;		
										go.GetComponent<Button>().onClick.RemoveAllListeners();
										go.GetComponent<Button>().onClick.AddListener(() =>
										{
											Game_.StopCountDown();
											ActionInfo info = new ActionInfo();
											info.actionCode = "GangAction";
											info.Params = new ActionInfo.Data();
											info.Params.card = player.card;
											string josn = JsonMapper.ToJson(info);
											WebSoketCall.One().SendToWeb(josn);
										});
									go.SetActive(true);
								}
								else if (player.players[i].seatNum == Game_.youplaycount)
								{
									Game_.PlayerNum = Game_.youplaycount;
								}
								else if (player.players[i].seatNum == Game_.zuoplaycount) { Game_.PlayerNum = Game_.zuoplaycount; }
								else if (player.players[i].seatNum == Game_.shangplaycount) { Game_.PlayerNum = Game_.shangplaycount; }
							}

						}
					}
				}
			}
			Game_.StartCountDown();
		}
	}
	/// <summary>
	/// 存储与自己手牌刷新
	/// </summary>
	/// <param name="player"></param>
    public void Reflish(playerActionInfo player)
	{	
		int robcount_ = 0;
		for (int i = 0; i < player.players.Count; i++)
		{
			if (player.players[i].playerStatus == 2)
			{
				robcount_++;
			}
			if (player.players[i].uid == UserId.memberId)
			{
				Game_.reascalcount = player.players[i].rascalCount;
				//将手牌刷新
				List<int> MyPai=RefashCard(player.players[i].handCards, player.players[i].skillCards, player.players[i].rascalCount);
				//将技能牌显示
				for (int j = 0; j < player.players[i].skillCards.Count; j++)
				{
					Image pic = Game_.Pai.GetChild(j).GetComponent<Image>();
					pic.enabled = false;
					Game_.Pai.GetChild(j).GetChild(0).gameObject.SetActive(false);
					Transform t1 = Game_.Pai.GetChild(j).Find("skillpai");
					t1.gameObject.SetActive(true);
					t1.GetChild(0).GetComponent<Image>().sprite = Game_.list[player.players[i].skillCards[j]];
				}
				//正对于当前手牌进行调整
				//Game_.ContrastPai(Game_.Pai.childCount, MyPai.Count, Game_.Pai);
				ContrastPai(Game_.Pai.childCount, MyPai.Count, Game_.Pai);
			}
			if (player.players[i].seatNum == Game_.youplaycount)
			{
				Game_.one = null;
				Game_.one = player.players[i].handCards;
				AddOtherHandCard(player, i, Game_.You, Game_.one);
			}
			else if (player.players[i].seatNum == Game_.zuoplaycount)
			{
				Game_.three = null;
				Game_.three = player.players[i].handCards;
				AddOtherHandCard(player,i,Game_.Zuo,Game_.three);
			}
			else if (player.players[i].seatNum == Game_.shangplaycount)
			{
				Game_.twotwo = null;
				Game_.twotwo = player.players[i].handCards;
				AddOtherHandCard(player, i, Game_.Shang, Game_.twotwo);
			}
		}
		Game_.robCount = robcount_;
	}
	public void AddOtherHandCard(playerActionInfo player,int i,Transform ObjGame,List<int> ObjGameList)
	{
		if (player.players[i].rascalCount > 0)
		{
			for (int j = 0; j < player.players[i].rascalCount; j++)
			{
				ObjGameList.Add(Game_.rascalCard);
			}
		}
		if (player.players[i].skillCards.Count > 0)
		{
			for (int j = 0; j < player.players[i].skillCards.Count; j++)
			{
				ObjGame.GetChild(j).gameObject.SetActive(true);
				ObjGameList.Add(player.players[i].skillCards[j]);
				Game_.OtherSkillCardFresh(player.players[i].skillCards, ObjGame);
			}
		}
		Game_.ContrastPai(ObjGame.childCount, ObjGameList.Count, ObjGame);
	}
	/// <summary>
	/// 上家打出的牌
	/// </summary>
	/// <param name="player"></param>
	public void FrontNumOutTable(playerActionInfo player)
	{
		if (Point_ != null) { Destroy(Point_);}
		for (int i = 0; i < player.players.Count; i++)
		{
			if (player.players[i].seatNum == player.turnNum)
			{
				if (player.players[i].frontNum == Game_.seatNum)
				{//记录下打出的牌
					if (Game_.StartTG)
					{
						Game_.isSTGPutCard = true;
						DestroyImmediate(Game_.Pai.GetChild(Game_.Pai.childCount - 1).gameObject);
						GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "MajiangRes", Game_.parent);
						go.transform.GetChild(0).GetComponent<Image>().sprite = Game_.list[player.card];
						Game_.DaoChuObject = go;
						ShowReascalCard(player.card);
					}
					else { ShowReascalCard(player.card); }
					PointWayProDoudt(Game_.DaoChuObject.transform.GetChild(0),Game_.Pai);
				}
				else if (player.players[i].frontNum == Game_.youplaycount)
				{//记录下打出的牌
					if (Game_.otherChiPeng)
						Game_.otherChiPeng = false;
					else
					{
						if (Game_.CurrentDuixiang != null)
						{
							DestroyImmediate(Game_.CurrentDuixiang.gameObject);
						}
					}
					Prodrogrees(player.card, new Vector3(0, 0, 270), new Vector3(0, 0, 180), "You_MaJiang", Game_.YP);
					PointWayProDoudt(Game_.DaoChuObject.transform.GetChild(0), Game_.You);
				}
				else if (player.players[i].frontNum == Game_.shangplaycount)
				{
					//记录下打出的牌
					if (Game_.otherChiPeng)
						Game_.otherChiPeng = false;
					else
					{
						if (Game_.CurrentDuixiang != null)
						{
							DestroyImmediate(Game_.CurrentDuixiang.gameObject);
						}
					}
					Prodrogrees(player.card, new Vector3(0, 0, 0), new Vector3(0, 0, 0), "ShangMajiangRes", Game_.SP);
					PointWayProDoudt(Game_.DaoChuObject.transform.GetChild(0), Game_.Shang);
				}
				else if (player.players[i].frontNum == Game_.zuoplaycount)
				{//记录下打出的牌
					if (Game_.otherChiPeng)
						Game_.otherChiPeng = false;
					else
					{
						if (Game_.CurrentDuixiang != null)
						{
							DestroyImmediate(Game_.CurrentDuixiang.gameObject);
						}
					}
					Prodrogrees(player.card, new Vector3(0, 0, 90), new Vector3(0, 0, -90), "Zuo_Majiang", Game_.ZP);
					PointWayProDoudt(Game_.DaoChuObject.transform.GetChild(0), Game_.Zuo);
				}
			}
		}
	}
	//生成位置显示
	public void PointWayProDoudt(Transform ObjGame, Transform ObjType)
	{
		GameObject gg = Resources.Load<GameObject>("prefabs/PointWay");
		GameObject g = GameObject.Instantiate<GameObject>(gg);
		Point_ = g;
		g.transform.SetParent(ObjGame);
		RectTransform r = g.transform as RectTransform;
		if (ObjType == Game_.You ) 
		{
			r.anchoredPosition = new Vector2(70, 0);
			r.transform.DOLocalMoveX(40, 0.5f).SetLoops(-1, LoopType.Yoyo);
		}
		else if(ObjType == Game_.Zuo)
		{
			r.anchoredPosition = new Vector2(-70, 0);
			r.transform.DOLocalMoveX(-40, 0.5f).SetLoops(-1, LoopType.Yoyo);
		}
		else {
			r.anchoredPosition = new Vector2(0, 30);
			r.transform.DOLocalMoveY(60, 0.5f).SetLoops(-1, LoopType.Yoyo);
		}
		
	}
	public void DelectPointWay(Transform ObjGame)
	{
		if (ObjGame.childCount > 0)
		{
			Destroy(ObjGame.gameObject);
		}
	}

    /// <summary>
	/// 打出牌癞子显示
	/// </summary>
	/// <param name="playerCard"></param>
	public void ShowReascalCard(int playerCard)
	{
		if (playerCard == Game_.rascalCard)
		{
			Game_.DaoChuObject.transform.GetChild(1).gameObject.SetActive(true);
		}
	}
	/// <summary>
	/// 生成打出对象
	/// </summary>
	/// <param name="Card"></param>
	/// <param name="rotal"></param>
	/// <param name="roatal1"></param>
	/// <param name="proName"></param>
	/// <param name="parent"></param>
	public void Prodrogrees(int Card,Vector3 rotal,Vector3 roatal1,string proName,Transform parent)
	{
		GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, proName, parent);
		Game_.DaoChuObject = go;
		go.transform.rotation *= Quaternion.Euler(rotal);
		go.transform.GetChild(0).rotation *= Quaternion.Euler(roatal1);
		go.transform.GetChild(0).GetComponent<Image>().sprite = Game_.list[Game_.chuCard];
		ShowReascalCard(Card);
	}
	//吃牌数据的添加
	public void ChiAddEvent(GameObject game, List<int> objChiType, string Typestring, int PCard)
	{
		EventTrigger t1 = game.GetComponent<EventTrigger>();
		if (t1 == null)
		{
			t1 = game.AddComponent<EventTrigger>();
		}
		EventTrigger trigger = game.GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback = new EventTrigger.TriggerEvent();
		entry.callback.AddListener(OnChiClick);
		trigger.triggers.Add(entry);
		switch (Typestring)
		{
			case "zhong":
				game.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Game_.list[objChiType[0]];
				game.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[PCard];
				game.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = Game_.list[objChiType[1]];
				break;
			case "shang":
				game.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Game_.list[objChiType[0]];
				game.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[objChiType[1]];
				game.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = Game_.list[PCard];
				break;
			case "xia":
				game.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Game_.list[PCard];
				game.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[objChiType[0]];
				game.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = Game_.list[objChiType[1]];
				break;
		}
	}
	//吃牌点击事件
	public void OnChiClick(BaseEventData pointData)
	{
		PointerEventData pointD = pointData as PointerEventData;
		string sname = pointD.pointerPress.transform.name;
		switch (sname)
		{
			case "zhong":
				ChiPaiClass chi = new ChiPaiClass();
				chi.Params = new ChiPaiClass.Data();
				chi.actionCode = "ChiAction";
				chi.Params.card2 = Game_.chiType_Zhong[0];
				chi.Params.card3 = Game_.chiType_Zhong[1];
				string jsondata = JsonMapper.ToJson(chi);
				WebSoketCall.One().SendToWeb(jsondata);
				Transform t = Game_.skillMap.Find("skillone");
				for (int i = 0; i < t.childCount; i++)
				{
					t.GetChild(i).gameObject.SetActive(false);
				}
				break;
			case "shang":
				ChiPaiClass chi1 = new ChiPaiClass();
				chi1.Params = new ChiPaiClass.Data();
				chi1.actionCode = "ChiAction";
				chi1.Params.card2 = Game_.chiType_shang[0];
				chi1.Params.card3 = Game_.chiType_shang[1];
				string jsondata1 = JsonMapper.ToJson(chi1);
				WebSoketCall.One().SendToWeb(jsondata1);
				Transform t1 = Game_.skillMap.Find("skillone");
				for (int i = 0; i < t1.childCount; i++)
				{
					t1.GetChild(i).gameObject.SetActive(false);
				}
				break;
			case "xia":
				ChiPaiClass chi2 = new ChiPaiClass();
				chi2.Params = new ChiPaiClass.Data();
				chi2.actionCode = "ChiAction";
				chi2.Params.card2 = Game_.chiType_xia[0];
				chi2.Params.card3 = Game_.chiType_xia[1];
				string jsondata2 = JsonMapper.ToJson(chi2);
				WebSoketCall.One().SendToWeb(jsondata2);
				Transform t2 = Game_.skillMap.Find("skillone");
				for (int i = 0; i < t2.childCount; i++)
				{
					t2.GetChild(i).gameObject.SetActive(false);
				}
				break;

		}
		for (int i = Game_.pic.childCount - 1; i > 0; i--)
		{
			Destroy(Game_.pic.GetChild(i).gameObject);
		}
		Game_.pic.gameObject.SetActive(false);
		Game_.StopCountDown();
		Game_.chiType_shang.Clear(); Game_.chiType_xia.Clear(); Game_.chiType_Zhong.Clear();
	}
	/// <summary>
	/// 用来给可能吃的牌分组
	/// </summary>
	/// <param name="handCards">手牌数组</param>
	/// <param name="card">要分类的牌</param>
	/// <param name="seatname">座位号</param>
	/// <returns></returns>
	public List<string> JugeChi(List<int> handCards, int card, string seatname)
	{
		List<string> ss = new List<string>();
		List<int> zhong = new List<int>();
		List<int> shang = new List<int>();
		List<int> xia = new List<int>();
		for (int i = 0; i < handCards.Count; i++)
		{
			//中间情况
			if (card - 1 == handCards[i] || card + 1 == handCards[i])
			{
				if (zhong.Contains(handCards[i]))
				{
					continue;
				}
				zhong.Add(handCards[i]);
			}
		}
		for (int i = 0; i < handCards.Count; i++)
		{
			//上边情况
			if (card - 1 == handCards[i] || card - 2 == handCards[i])
			{
				if (shang.Contains(handCards[i]))
				{
					continue;
				}
				shang.Add(handCards[i]);
			}

		}

		for (int i = 0; i < handCards.Count; i++)
		{
			//下边情况
			if (card + 1 == handCards[i] || card + 2 == handCards[i])
			{
				if (xia.Contains(handCards[i]))
				{
					continue;
				}
				xia.Add(handCards[i]);
			}
		}

		if (zhong.Count >= 2)
		{
			ss.Add("zhong");
		}
		if (xia.Count >= 2)
		{
			ss.Add("xia");
		}
		if (shang.Count >= 2)
		{
			ss.Add("shang");
		}
		switch (seatname)
		{
			case "My":
				Game_.chiType_Zhong = zhong;
				Game_.chiType_shang = shang;
				Game_.chiType_xia = xia;
				break;
		}
		return ss;
	}
	//添加打出牌的方法
	public void AddOutPutCardWay(Transform objGame,List<Sprite> simage)
	{
		List<Sprite> s = new List<Sprite>();
		for (int i = 0; i < objGame.childCount; i++)
		{
			s.Add(objGame.GetChild(i).GetChild(0).GetComponent<Image>().sprite);
		}
		simage = s;
	}
	//添加牌的方法
	public void AddOutPut()
	{
		AddOutPutCardWay(Game_.parent, Game_.MyOutPutCard);
		AddOutPutCardWay(Game_.YP, Game_.YouOutPutCard);
		AddOutPutCardWay(Game_.SP, Game_.ShangOutPutCard);
		AddOutPutCardWay(Game_.ZP, Game_.ZuoOutPutCard);
	}

	public List<int> RefashCard(List<int> HandCard, List<int> skillCard, int reascalCount_)
	{
		Debug.Log("RefashCard");
		List<int> ShuangCard = new List<int>();
		if (skillCard.Count > 0)
		{
			for (int i = 0; i < skillCard.Count; i++)
			{
				ShuangCard.Add(skillCard[i]);
			}
		}
		if (HandCard.Count > 0)
		{
			for (int i = 0; i < HandCard.Count; i++)
			{
				ShuangCard.Add(HandCard[i]);
			}
		}
		for (int i = 0; i < reascalCount_; i++)
		{
			ShuangCard.Add(Game_.rascalCard);
		}
		int CountNum = HandCard.Count + skillCard.Count + reascalCount_;
		ContrastPai(Game_.Pai.childCount, CountNum, Game_.Pai);
		for (int i = 0; i < ShuangCard.Count; i++)
		{
			Game_.Pai.GetChild(i).GetChild(2).gameObject.SetActive(false);
			Game_.Pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Game_.list[ShuangCard[i]];
			Game_.Pai.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[ShuangCard[i]];
		}
		for (int i = 0; i < reascalCount_; i++)
		{
			Game_.Pai.GetChild(Game_.Pai.childCount - 1 - i).GetChild(2).gameObject.SetActive(true);
		}
		//StartCoroutine("CountDown");
		return ShuangCard;
	}
	public void ContrastPai(int PaiCard, int DataCard, Transform obj)
	{
		if (PaiCard != DataCard)
		{
			int num = PaiCard - DataCard;

			if (num > 0)
			{
				if (num > 5)
				{
					Debug.LogError("Error");
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
				if (obj ==Game_.Pai)
				{
					for (int i = 0; i < num1; i++)
					{
						GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "PaiRes_My", Game_.Pai);
						#region 点击事件的添加
						go.transform.GetChild(0).gameObject.AddComponent<EventTrigger>();
						go.transform.GetChild(0).gameObject.AddComponent<DragMajiang>();
						go.name = "XP";
						Transform childT = go.transform.GetChild(0);
						if (Game_.DrawCPai != 0)
						{
							childT.GetComponent<Image>().sprite = Game_.list[Game_.DrawCPai];
						}
						//添加点击事件
						EventTrigger trigger = go.transform.GetChild(0).GetComponent<EventTrigger>();
						EventTrigger.Entry entry = new EventTrigger.Entry();
						entry.eventID = EventTriggerType.PointerClick;
						entry.callback = new EventTrigger.TriggerEvent();
						entry.callback.AddListener(Game_.OnClick);
						trigger.triggers.Add(entry);
						#endregion
						RectTransform pos = go.transform as RectTransform;
						(pos.transform as RectTransform).anchoredPosition = (Game_.Pai.GetChild(Game_.Pai.childCount - 2) as RectTransform).anchoredPosition;
						(pos.transform as RectTransform).anchoredPosition += new Vector2(108, 0);
					}
				}
			}
		}
	}
}
