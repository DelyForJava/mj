using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DrawActionController : MonoBehaviour {

    public Game_Controller Game_;

    public void DrawAction(string jsonData)
    {
		int num = int.Parse(Game_.OtherPaiShow.text);
		Game_.OtherPaiShow.text = (num - 1).ToString();
		DrawActionInfo da = JsonMapper.ToObject<DrawActionInfo>(jsonData);
		//监测	
		CheckCurrentStatus(da);

		Game_.DrawCPai = da.newCard;
		Game_.PlayerNum = da.seatNum;

		if (da.seatNum == Game_.seatNum)
			Game_.activePTrusteeship = true;
		else
			Game_.activePTrusteeship = false;

		Game_.StopCountDown(); 
		Game_.CurrentDrawCount++;
		Game_.TurnNum(da.seatNum);
		Game_.newCord = Game_.list[da.newCard];
		Game_.newCord = Game_.list[da.newCard];
		//杠牌监测
		SkillGangJuger(da);
		//胡牌监测
		SkillShowHu(da);
		//摸牌判断
		DrawCardJuger(da);
	}
	//监测当前状态并修复显示
    void CheckCurrentStatus(DrawActionInfo da)
    {
		if (da.player.playerStatus == 0)
		{
			if (Game_.robCount < 3) { 
			if (da.player.uid == UserId.memberId)
			{
				Game_.StartTG = true;
				Transform tg = Game_.skillMap.Find("TuoGuang");
				tg.gameObject.SetActive(true);
			}
			}
		}
		else if (da.player.playerStatus == 1)
		{
			if (Game_.robCount < 3)
			{
				if (da.player.uid == UserId.memberId)
				{
					Game_.StartTG = false;
					Transform tg = Game_.skillMap.Find("TuoGuang");
					tg.gameObject.SetActive(false);
				}
			}
			else if (Game_.dic.ContainsKey(da.player.uid))
			{
				if (Game_.youplaycount == Game_.dic[da.player.uid])
				{
					if (da.player.playerStatus == 1)
					{
						GameObject.Find("TrusteeshipStatus").transform.Find("YouTrusteeship").gameObject.SetActive(false);
					}
				}
			}
			else if (Game_.dic.ContainsKey(da.player.uid))
			{
				if (Game_.zuoplaycount == Game_.dic[da.player.uid])
				{
					if (da.player.playerStatus == 1)
					{
						GameObject.Find("TrusteeshipStatus").transform.Find("ZuoTrusteeship").gameObject.SetActive(false);
					}
				}
			}
			else if (Game_.dic.ContainsKey(da.player.uid))
			{
				if (Game_.shangplaycount == Game_.dic[da.player.uid])
				{
					if (da.player.playerStatus == 1)
					{
						GameObject.Find("TrusteeshipStatus").transform.Find("ShangTrusteeship").gameObject.SetActive(false);
					}
				}
			}
		}
	}
	//胡技能的监测
    void SkillShowHu(DrawActionInfo da)
	{
		if (da.player.hu > 0)
		{
			if (Game_.StartTG)
			{
			}
			else
			{
				Game_.jie.HuTheme = da.seatNum;
				Game_.jie.HuCard = da.newCard;
				Game_.jie.maJiangPai = Game_.PaiSprite;
				if (da.seatNum == Game_.seatNum)
				{
					Game_.isSkillUse = true;
					Game_.skillMap.Find("skillone/guo").gameObject.SetActive(true);
					GameObject go = Game_.skillMap.Find("hu").gameObject;
					go.SetActive(true);
					go.GetComponent<Button>().onClick.RemoveAllListeners();
					go.GetComponent<Button>().onClick.AddListener(() => {
						Game_.StopCountDown();
						#region 指令—胡牌
						Game_.HuPaiCard = da.newCard;
						ActionInfo hu = new ActionInfo();
						hu.actionCode = "HuAction";
						hu.Params = new ActionInfo.Data();
						string hus = JsonMapper.ToJson(hu);
						WebSoketCall.One().SendToWeb(hus);
						#endregion
					});
					Game_.isHu = true;
				}
			}
		}	
	}
	//杠技能的监测
	void SkillGangJuger(DrawActionInfo da)
	{
		if (!Game_.StartTG)
		{
			if (da.player.skillMap.Count != 0)
			{
				Game_.GrabCards = da.newCard;
				if (da.player.skillMap.ContainsKey("peng"))
				{
					if (da.player.skillMap["peng"] == 1)
					{
						Game_.skillMap.Find("skillone/guo").gameObject.SetActive(false);
						GameObject go = Game_.skillMap.Find("skillone/peng").gameObject;
						go.gameObject.SetActive(false);
						go.GetComponent<Button>().onClick.RemoveAllListeners();
					}
				}
				if (da.player.skillMap["gang"] == 2)
				{
					List<int> gang = Game_.CheckPoint(da.player.handCards);
					int GangPai = da.newCard;
					if (gang.Count > 0)
					{
						if (gang[0] != da.newCard)
						{
							Debug.Log("好像是一开始有杠");
							if (gang[0] != 0)
							{
								GangPai = da.player.handCards[gang[0]];
							}
						}
					}
					if (da.seatNum == Game_.seatNum)
					{
						Game_.GangCard = da.gangCard;
						if (da.gangCard == 0)
						{
							da.gangCard = da.newCard;
							if (gang.Count > 0)
							{
								if (gang[0] != da.newCard)
								{
									Debug.Log("好像是一开始有杠");
									if (gang[0] != 0)
									{
										da.gangCard = da.player.handCards[gang[0]];
									}
								}
							}
							Game_.GangCard = da.gangCard;
							Debug.Log("da.gangCaar" + Game_.GangCard);
						}
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
								info.Params.card = /*da.newCard;*//*GangPai*/da.gangCard;
								string josn = JsonMapper.ToJson(info);
								WebSoketCall.One().SendToWeb(josn);
								Transform t = Game_.skillMap.Find("skillone");
								for (int j = 0; j < t.childCount; j++)
								{
									t.GetChild(j).gameObject.SetActive(false);
								}
							});
						go.SetActive(true);
						Game_.isMingGang = true;
					}
					else if (da.seatNum == Game_.youplaycount)
					{
						Game_.isYouMingGang = true;
					}
					else if (da.seatNum == Game_.shangplaycount)
					{
						Game_.G3Position = Game_.ScreenPosition(da.newCard, da.player.skillCards);
						Game_.isShangMingGang = true;
					}
					else
					{
						Game_.isZuoMingGang = true;
					}
				}
				if (da.player.skillMap["gang"] == 3)
				{
					Game_.G3bool = true;
					if (da.seatNum == Game_.seatNum)
					{
						Game_.G3Position = Game_.ScreenPosition(da.newCard, da.player.skillCards);
						Game_.GangCard = da.newCard;
						Game_.isSkillUse = true;
						Game_.skillMap.Find("skillone/guo").gameObject.SetActive(true);
						GameObject go = Game_.skillMap.Find("skillone/gang").gameObject;
							go.GetComponent<Button>().onClick.RemoveAllListeners();				
							go.GetComponent<Button>().onClick.AddListener(() =>
							{
								Game_.GangT3 = true;
								Game_.StopCountDown();
								ActionInfo info = new ActionInfo();
								info.actionCode = "GangAction";
								info.Params = new ActionInfo.Data();
								info.Params.card = da.newCard;
								string josn = JsonMapper.ToJson(info);
								WebSoketCall.One().SendToWeb(josn);
								Transform t = Game_.skillMap.Find("skillone");
								for (int j = 0; j < t.childCount; j++)
								{
									t.GetChild(j).gameObject.SetActive(false);
								}
							});
						go.SetActive(true);
					}
					else if (da.seatNum == Game_.youplaycount)
					{
						Game_.YouGangT3 = true;
						Game_.G3Position = Game_.ScreenPosition(da.newCard, da.player.skillCards);
					}
					else if (da.seatNum == Game_.shangplaycount)
					{
						Game_.ShangGangT3 = true;
						Game_.G3Position = Game_.ScreenPosition(da.newCard, da.player.skillCards);
					}
					else
					{
						Game_.ZuoGangT3 = true;
						Game_.G3Position = Game_.ScreenPosition(da.newCard, da.player.skillCards);
					}
				}
			}
		}
	}
	void DrawCardJuger(DrawActionInfo da)
	{
		if (da.seatNum == Game_.seatNum)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			   { Debug.Log("NoRRRRRR"); return; }
			Game_.isSTGPutCard = false;
			Game_.DarwPai = da.player.handCards; Game_.MySkillCount = da.player.skillCards.Count;
			Game_.StopCountDown();
			Game_.activePTrusteeship = true;
			KeepCardWay();
			KeepObjCardWay_(Game_.Pai, 109, new Vector2(108, 0));
			Game_.BuPai(Game_.newCord);
			if (Game_.isMyGang)
			{
				#region My杠
				//for (int i = 0; i < da.player.skillCards.Count; i++)
				//{
				//	Image pic = Game_.Pai.GetChild(i).GetComponent<Image>();
				//	pic.enabled = false;
				//	Game_.Pai.GetChild(i).GetChild(0).gameObject.SetActive(false);
				//	Transform t1 = Game_.Pai.GetChild(i).Find("skillpai");
				//	t1.gameObject.SetActive(true);
				//	t1.GetChild(0).GetComponent<Image>().sprite = Game_.list[da.player.skillCards[i]];
				//	Game_.Pai.GetChild(i).gameObject.SetActive(true);
				//}
				//if (Game_.reascalcount > 0)
				//{
				//	for (int i = 0; i < Game_.Pai.childCount; i++)
				//	{
				//		Game_.Pai.GetChild(i).GetChild(2).gameObject.SetActive(false);
				//	}
				//	for (int i = 0; i < Game_.reascalcount; i++)
				//	{
				//		Game_.Pai.GetChild(Game_.Pai.childCount - 1 - i).GetChild(2).gameObject.SetActive(true);
				//		da.player.handCards.Add(Game_.rascalCard);
				//	}
				//}
				//if (da.player.handCards.Count>0)
				//{
				//	for (int j = 0; j < da.player.handCards.Count; j++)
				//	{
				//		int childNum = j + da.player.skillCards.Count;
				//		Game_.Pai.GetChild(childNum).GetChild(0).GetComponent<Image>().sprite = Game_.list[da.player.handCards[j]];
				//	}
				//}
				Game_.isMyGang = false;
				#endregion
			}
			if (Game_.isHu)
				Game_.isHu = false;
			else
				Game_.isOwn = true;
			//if (Game_.robCount >= 3)
			//{
			//	if (Application.internetReachability == NetworkReachability.NotReachable) 
			//	{ Debug.Log("NoRRRRRR"); return; }
			//	Game_.RobDrawCardok = true;
			//	if (Game_.StartTG)
			//	{
			//		Debug.Log("StTG");
			//		Game_.isPlayerIng = true;
			//		Game_.StartFoolHangUp(da.newCard);
			//	}
			//}
		}
		else if(da.seatNum==Game_.youplaycount)
		{
			Game_.RobDrawCardok = false;
			KeepObjCardWay_(Game_.You, 51, new Vector2(0, -50));
			Game_.activePTrusteeship = false;
			Game_.one.Add(da.newCard);
			Game_.OtherBuPai(Game_.You, Game_.newCord, "YouRes");
		}
		else if(da.seatNum == Game_.shangplaycount)
		{
			Game_.RobDrawCardok = false;
			KeepObjCardWay_(Game_.Shang, 69, new Vector2(68, 0));
			Game_.activePTrusteeship = false;
			Game_.twotwo.Add(da.newCard);
			Game_.OtherBuPai(Game_.Shang, Game_.newCord, "ShangRes");
		}
		else if (da.seatNum == Game_.zuoplaycount)
		{
			Game_.RobDrawCardok = false;
			KeepObjCardWay_(Game_.Zuo, 51, new Vector2(0, -50));
			Game_.activePTrusteeship = false;
			Game_.three.Add(da.newCard);
			Game_.OtherBuPai(Game_.Zuo, Game_.newCord, "ZuoRes");
		}
	}
	void KeepCardWay()
	{
		//维持手牌在21的位置
		for (int i = 0; i < Game_.Pai.childCount; i++)
		{
			if ((Game_.Pai.GetChild(i) as RectTransform).anchoredPosition.y != 21)
			{
				if ((Game_.Pai.GetChild(i) as RectTransform).anchoredPosition.y == 50)
				{
					continue;
				}
				(Game_.Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(0, ((Game_.Pai.GetChild(i) as RectTransform).anchoredPosition.y - 21));
			}
			MaJiang.Instance.Majiang_ = null;
			//将探索的牌显示关闭
			Game_.ShowSameCard(Game_.Pai.GetChild(0).GetChild(0),false,true);
		}
	}
	void KeepObjCardWay_(Transform objGame,float Dis,Vector2 NeedHangPos)
	{
		Vector2 distance, distance1;
		if (objGame == Game_.Pai || objGame == Game_.Shang) { 
		 distance = new Vector2((objGame.GetChild(objGame.childCount - 1) as RectTransform).anchoredPosition.x, 0);
		 distance1 = new Vector2((objGame.GetChild(objGame.childCount - 2) as RectTransform).anchoredPosition.x, 0);
		}
		else
		{
		  distance = new Vector2(0,(objGame.GetChild(objGame.childCount - 1) as RectTransform).anchoredPosition.y);
		  distance1 = new Vector2(0,(objGame.GetChild(objGame.childCount - 2) as RectTransform).anchoredPosition.y);
		}
		float data = Vector2.Distance(distance, distance1);
		if (data > Dis)
		{
			(objGame.GetChild(objGame.childCount - 1) as RectTransform).anchoredPosition = (objGame.GetChild(objGame.childCount - 2) as RectTransform).anchoredPosition;
			(objGame.GetChild(objGame.childCount - 1) as RectTransform).anchoredPosition += NeedHangPos;
		}
	}
}
