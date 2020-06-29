using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterTableActionController : MonoBehaviour {
	public Game_Controller Game_;
	public void EnterTableAction(string edate)
	{
		EnterTableInfo enterTable = JsonMapper.ToObject<EnterTableInfo>(edate);
		//根据玩家进入时带的游戏类型进行分类
		//参数设置
		ParameterSetting(enterTable);
		//数据获取
		GetParameterSetting(enterTable);
		//玩家数据
		PlayerDateShow(enterTable);
		//玩家状态的监测
		MonitorReallyStaus(enterTable);
	}
	//参数设置
	public void ParameterSetting(EnterTableInfo enterTable)
	{
		Game_.IsAutoPut = enterTable.IsAutoPut; //获取创建房间时候房主是否设置了能否托管
		//显示当前轮数
		Game_.CurrentRound.text = enterTable.round.ToString();
		//记录下当前轮数用于可用于判断自己是否显示结算
		Game_.Createround = enterTable.round;
		if (UserId.JieCreateRoom != true)
		{
			if (enterTable.players.Count >= 4)
			{
				Game_.WaitTipPeople.gameObject.SetActive(false);
				Game_.WaitTipPeople.StopWaitPeople();
				Game_.WaitTipPeople.TimeDownCount = 0;
			}
			else
			{
				Game_.WaitTipPeople.gameObject.SetActive(true);
			}
		}
		if (!UserId.JieCreateRoom)
		{
			Game_.YQHY.transform.parent.GetChild(2).gameObject.SetActive(true);
		}
		if (Game_.isReConnection)
		{
			Game_.YQHY.gameObject.SetActive(false);
		}
		if (Game_.isJoinRoom)
		{
			Game_.YQHY.gameObject.SetActive(false);
			Game_.YQHY.transform.parent.GetChild(2).gameObject.SetActive(false);
			Game_.isJoinRoom = false;
		}
		if (UserId.isJoinRoom)
		{
			Game_.TableNumShow.text = enterTable.tableNum.ToString();
			Game_.TableNumShow.gameObject.SetActive(true);
		}
		else { Game_.CurrentRound.gameObject.SetActive(false); }
		Game_.tableNum = enterTable.tableNum;
		UserId.TableNum = enterTable.tableNum;
	}

	public void GetParameterSetting(EnterTableInfo enterTable)
	{
		for (int i = 0; i <Game_.HandPic.Count; i++)
		{
			if (i == 0) { continue; }
			Game_.HandPic[i].gameObject.GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "CSTX");
			Game_.handTuoName[i].text = null;
			Game_.HeadFamer[i].sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "CSTXK");
		}
		Game_.dic.Clear();
		Game_.SeatTID.Clear();
		Game_.playerId.Clear();
		Game_.playerInfo.Clear();
		SeatNumGet(enterTable);
	}
	public  void SeatNumGet(EnterTableInfo enterTable)
	{
		for (int i = 0; i < enterTable.players.Count; i++)
		{
			if (enterTable.players[i].uid == UserId.memberId)
			{
				if (enterTable.players[i].readyFlag == 0)
				{
					GameObject.Find("ReallyButton").transform.Find("MyReally").gameObject.SetActive(true);
				}
				SeatPosit(enterTable, i);		
			}
		}
		for (int i = 0; i < enterTable.players.Count; i++)
		{
			//记录下当前状态并存值
			if (!Game_.dic.ContainsKey(enterTable.players[i].uid))
			{
				Game_.dic.Add(enterTable.players[i].uid, enterTable.players[i].seatNum);
				Game_.SeatTID.Add(enterTable.players[i].seatNum, enterTable.players[i].uid);
			}
		}
	}
	//根据位置来定座位号
	public void SeatPosit(EnterTableInfo enterTable,int i)
	{
			Game_.seatNum = enterTable.players[i].seatNum;
			Game_.zuoplaycount = enterTable.players[i].frontNum;
			if (Game_.seatNum == 4)
			{
				Game_.youplaycount = 1;
				Game_.shangplaycount = 2;
				Game_.zuoplaycount = 3;
			}
			else
				Game_.youplaycount = Game_.seatNum + 1;
			if (Game_.youplaycount == 4)
			{
				Game_.shangplaycount = 1;
				Game_.zuoplaycount = 2;
			}
			else
				Game_.shangplaycount = Game_.youplaycount + 1;		
	}
	//玩家数据
	public void PlayerDateShow(EnterTableInfo enterTable)
	{
		for (int i = 0; i < enterTable.players.Count; i++)
		{
			if (Game_.playerId.Contains(enterTable.players[i].uid))
			{
				continue;
			}
			Game_.playerId.Add(enterTable.players[i].uid);
			Member member = new Member();
			member.memberId = enterTable.players[i].uid.ToString();
			string json = JsonMapper.ToJson(member);
			Debug.Log("http://" + Bridge.GetHostAndPort() + "/ api/member/getinfo");
			JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/member/getinfo", json, Game_.PlayerInfo);
		}
	}
	//玩家准备状态的监测
	public void MonitorReallyStaus(EnterTableInfo enterTable)
	{
		if (enterTable.players.Count == 4)
		{
			List<int> check = new List<int>();
			for (int i = 0; i < enterTable.players.Count; i++)
			{
				if (enterTable.players[i].readyFlag == 0)
				{
					check.Add(enterTable.players[i].uid);
				}
				else if (enterTable.players[i].readyFlag == 1)
				{
					if (enterTable.players[i].uid == Game_.SeatTID[Game_.seatNum])
					{ GameObject.Find("turnImage").transform.Find("SelfReally").gameObject.SetActive(true); }
					else if (enterTable.players[i].uid == Game_.SeatTID[Game_.youplaycount])
					{ GameObject.Find("turnImage").transform.Find("YouReally").gameObject.SetActive(true); }
					else if (enterTable.players[i].uid == Game_.SeatTID[Game_.shangplaycount])
					{ GameObject.Find("turnImage").transform.Find("ShangReally").gameObject.SetActive(true); }
					else { GameObject.Find("turnImage").transform.Find("ZuoReally").gameObject.SetActive(true); }
				}
			}
			if (check.Count > 0)
			{
				if (!UserId.JieCreateRoom)
				{
					Game_.YQHY.transform.parent.GetChild(2).gameObject.SetActive(true);
				}
				for (int i = 0; i < check.Count; i++)
				{
					if (check[i] == Game_.SeatTID[Game_.seatNum])
					{
						Debug.Log("要打开准备开关的SeatNum" + Game_.dic[check[i]]);
						//GameObject.Find("ReallyButton").transform.Find("MyReally").gameObject.SetActive(true);
					}
				}
			}
		}
	}
}
