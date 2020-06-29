
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllReallyActionController : MonoBehaviour {

	public Game_Controller Game_;
	public void AllReallyAction(string edate)
	{
		AllReadActionInfo ARA = JsonMapper.ToObject<AllReadActionInfo>(edate);
		//
		ParameterSetting(ARA);
		//庄家显示
		BookMaskerShow();
		//显示轮到
		Game_.TurnNum(ARA.turnNum);
		//摸牌判断
		DrawHandCard(ARA);
	}
	//参数设定
	public void ParameterSetting(AllReadActionInfo ARA)
	{
		Game_.Bookmaker = ARA.turnNum;
		Game_.jie.tableNum = ARA.players[0].tableNum;
		Game_.newCord = Game_.list[ARA.newCard];
		Game_.DrawCPai = ARA.newCard;
		Game_.rascalCard = ARA.rascalCard;
		Game_.reascalPic.sprite = Game_.list[Game_.rascalCard];
		int TG = 0;
		for (int i = 0; i < ARA.players.Count; i++)
		{
			//机器人数的获取
			if (ARA.players[i].playerStatus == 2)
			{
				Game_.robCount++;
			}
			//托管人数记录
			if (ARA.players[i].playerStatus == 0)
			{
				TG++;
			}
			//庄家显示暂时关闭
			Game_.Bookpic[i].gameObject.SetActive(false);
			GetSelfHand(ARA,i);
		}

		//当前托管人数
		Game_.TGCount = TG;
	}
	//手牌刷新没加摸到的牌
	public void GetSelfHand(AllReadActionInfo ARA,int i)
	{
		if (ARA.players[i].uid == UserId.memberId)
			{
				SeatNumDetermine(ARA,i);
				List<int> MyPai_ = Game_.OtherResh(ARA.players[i].handCards, ARA.players[i].skillCards, Game_.reascalcount);
				Game_.DarwPai = ARA.players[i].handCards;
			    //如果是自己先出牌先将补到的牌除去在刷新手牌
			    if(ARA.turnNum == Game_.seatNum)
				{
					MyPai_.Remove(ARA.newCard);
					Debug.Log("MyPai" + MyPai_.Count);
					if(ARA.newCard == Game_.rascalCard)
					{
						MyPai_.Remove(Game_.rascalCard);
					}
				}
				RefashCard(MyPai_, ARA.players[i].skillCards,0);
			}
		if (ARA.players[i].seatNum == Game_.youplaycount)
		{
			ObtainOtherCard(ARA, i, Game_.one);
		}
		if (ARA.players[i].seatNum == Game_.zuoplaycount)
		{
			ObtainOtherCard(ARA, i, Game_.three);
		}
		if (ARA.players[i].seatNum == Game_.shangplaycount)
		{
			ObtainOtherCard(ARA, i, Game_.twotwo);
		}
	}
	//天胡判断
	public void HuJudge(AllReadActionInfo ARA)
	{
		Game_.skillMap.Find("skillone/guo").gameObject.SetActive(true);
		GameObject go = Game_.skillMap.Find("hu").gameObject;
		go.SetActive(true);
		go.GetComponent<Button>().onClick.RemoveAllListeners();
		go.GetComponent<Button>().onClick.AddListener(() => {
		Game_.StopCountDown();
			#region 指令—胡牌
			Game_.HuPaiCard = ARA.newCard;
			ActionInfo hu = new ActionInfo();
			hu.actionCode = "HuAction";
			hu.Params = new ActionInfo.Data();
			string hus = JsonMapper.ToJson(hu);
			WebSoketCall.One().SendToWeb(hus);
			#endregion
		});
	}
	//根据id来获取自己与其他人的座位号
	public void SeatNumDetermine(AllReadActionInfo ARA,int i)
	{
			Game_.tableNum = ARA.players[i].tableNum;
			Game_.seatNum = ARA.players[i].seatNum;
			Game_.zuoplaycount = ARA.players[i].frontNum;
			if (Game_.seatNum == 4)
			{
				Game_.youplaycount = 1;
				Game_.shangplaycount = 2;
			}
			else
				Game_.youplaycount = Game_.seatNum + 1;
			if (Game_.youplaycount == 4)
				Game_.shangplaycount = 1;
			else
				Game_.shangplaycount = Game_.youplaycount + 1;
			Game_.reascalcount = ARA.players[i].rascalCount;	
	}
	//获取其他玩家手牌信息
	public void ObtainOtherCard(AllReadActionInfo ARA,int i,List<int> ObjList)
	{
		ObjList = ARA.players[i].handCards;
		if (ARA.players[i].rascalCount > 0)
		{
			for (int j = 0; j < ARA.players[i].rascalCount; j++)
			{
				ObjList.Add(ARA.rascalCard);
			}
		}
	}
	//庄家显示
	public void BookMaskerShow()
	{
		if (Game_.Bookmaker == Game_.seatNum)
		{
			Game_.Bookpic[0].gameObject.SetActive(true);
		}
		else if (Game_.Bookmaker == Game_.youplaycount) { Game_.Bookpic[1].gameObject.SetActive(true); }
		else if (Game_.Bookmaker == Game_.shangplaycount) { Game_.Bookpic[2].gameObject.SetActive(true); }
		else if (Game_.Bookmaker == Game_.zuoplaycount) { Game_.Bookpic[3].gameObject.SetActive(true); }
	}
	//开始摸牌
	public void DrawHandCard(AllReadActionInfo ARA)
	{
		//自己开始摸牌后的操作
		if (Game_.seatNum == ARA.turnNum)
		{
			Game_.activePTrusteeship = true;
			int num = int.Parse(Game_.OtherPaiShow.text);
			Game_.OtherPaiShow.text = (num - 1).ToString();
			Game_.isOwn = true;
			for (int i = 0; i < Game_.reascalcount; i++)
			{
				Game_.Pai.GetChild(Game_.Pai.childCount - 1 - i).GetChild(2).gameObject.SetActive(true);
			}
			Game_.BuPai(Game_.newCord);
			if (ARA.players[Game_.seatNum-1].hu > 0)
			{
				HuJudge(ARA);
			}
			GangJuge(ARA);
			if (ARA.newCard == ARA.rascalCard)
			{
				Game_.Pai.GetChild(Game_.Pai.childCount - 1).GetChild(2).gameObject.SetActive(true);
			}
			else { Game_.Pai.GetChild(Game_.Pai.childCount - 1).GetChild(2).gameObject.SetActive(false);}
			//if (Game_.robCount >= 3)
			//{
			//	Game_.RobDrawCardok = true;
			//	if (Game_.StartTG)
			//	{
			//		Game_.isPlayerIng = true;
			//		Game_.StartFoolHangUp(Game_.DrawCPai);
			//	}
			//}
		}
	    else if(ARA.turnNum== Game_.youplaycount)
		{
			OtherBuCard(Game_.You, "YouRes");
		}
		else if (ARA.turnNum == Game_.shangplaycount)
		{
			OtherBuCard(Game_.Shang, "ShangRes");
		}
		else if (ARA.turnNum == Game_.zuoplaycount)
		{
			OtherBuCard(Game_.Zuo, "ZuoRes");
		}
	}
	//开始时手牌有杠
	public void GangJuge(AllReadActionInfo ARA)
	{
		if (ARA.players[Game_.seatNum - 1].skillMap.ContainsKey("gang"))
		{
			if (ARA.players[Game_.seatNum - 1].skillMap["gang"] == 2)
			{
				List<int> point = Game_.CheckPoint(Game_.DarwPai);
				if (point != null&&point.Count>0)
				{
					if (point.Count > 0)
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
								info.Params.card = ARA.players[Game_.seatNum - 1].handCards[point[0]];
								string josn = JsonMapper.ToJson(info);
								WebSoketCall.One().SendToWeb(josn);
							});			
						go.SetActive(true);
					}
				}
				else
				{
					Game_.skillMap.Find("skillone/guo").gameObject.SetActive(false);
					GameObject go = Game_.skillMap.Find("skillone/gang").gameObject;
					go.SetActive(false);
				}
			}
		}
	}
	//其他人补牌
	public void OtherBuCard(Transform ObjGame,string Res)
	{
		Game_.activePTrusteeship = false;
		int num = int.Parse(Game_.OtherPaiShow.text);
		Game_.OtherPaiShow.text = (num - 1).ToString();
		Game_.OtherBuPai(ObjGame, Game_.newCord, Res);
		for (int i = 0; i < Game_.reascalcount; i++)
		{
			Game_.Pai.GetChild(Game_.Pai.childCount - 1 - i).GetChild(2).gameObject.SetActive(true);
		}
	}
	//刷新方法
	public List<int> RefashCard(List<int> HandCard, List<int> skillCard, int reascalCount_)
	{
		
		List<int> ShuangCard = new List<int>();
		if (skillCard.Count > 0&&skillCard!=null)
		{
			for (int i = 0; i < skillCard.Count; i++)
			{
				ShuangCard.Add(skillCard[i]);
			}
		}
		if (HandCard.Count > 0&&HandCard!=null)
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
		Debug.Log("AllreadyShuang"+ShuangCard.Count);
		Debug.Log("Pai + "+Game_.Pai.childCount);
		for (int i = 0; i < ShuangCard.Count; i++)
		{
			Game_.Pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Game_.list[ShuangCard[i]];
			Game_.Pai.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[ShuangCard[i]];
		}
		return ShuangCard;
	}
}
