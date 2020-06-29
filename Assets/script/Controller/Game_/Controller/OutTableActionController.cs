using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutTableActionController : MonoBehaviour {

	public Game_Controller Game_;
	public void OutTableAction(string edate)
	{
		Game_.OutTabeCount = 0;
		OutTableData data = JsonMapper.ToObject<OutTableData>(edate);
		if (UserId.ChangeTable) 
		{
			ChangeTable();
		}
		else
		{   //其他玩家数据清理
			OutTableDate(data);
		}
	}
	//换桌的处理桌子上玩家数据
	public void ChangeTable()
	{
		UserId.ChangeTable = false;
		for (int i = 0; i < Game_.HandPic.Count; i++)
		{
			if (i == 0) { continue; }
			Game_.HandPic[i].gameObject.GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "CSTX");
			Game_.handTuoName[i].text = null;
			Game_.HeadFamer[i].sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "CSTXK");
		}
		LoadManager.Instance.LoadScene("mjGameScreen", Game_.EntableInfoData, UserId.GameType);
	}
	//玩家数据清理
	public void OutTableDate(OutTableData data)
	{
		if (UserId.JieCreateRoom)
		{
			//Todo create TableRound恢复初始round
			//出房间后之前结算的数据先去除
			Game_.CurrentRound.text = Game_.CurrentRound.text;
			Game_.CLS.HuList.Clear();
		}
		//自己退出房间数据清除
		if (data.memberId == UserId.memberId)
		{
			MySetOutTable(data);
		}
		else
		{
			//其他人退出在房间厂打开邀请好友
			if (UserId.isCreateRoom == true)
			{
				Game_.YQHY.gameObject.SetActive(true);
			}
			//将准备先去掉
			Transform tn = GameObject.Find("turnImage").transform;
			for (int i = 0; i < tn.childCount; i++)
			{
				tn.GetChild(i).gameObject.SetActive(false);
			}
		}
		int youId = 0, zuoId = 0, shangId = 0;
		//key为idvalue为Uid
		Dictionary<int, int> IdTUid = new Dictionary<int, int>();
		foreach (var Key in Game_.dic.Keys)
		{
			if (Game_.dic[Key] == Game_.youplaycount) { youId = Key; }
			else if (Game_.dic[Key] == Game_.zuoplaycount) { zuoId = Key; }
			else if (Game_.dic[Key] == Game_.shangplaycount) { shangId = Key; }
			else { if (!IdTUid.ContainsKey(Key)) { IdTUid.Add(Game_.seatNum, Key); } }
		}
		if (Game_.dic.ContainsKey(data.memberId))
		{
			if (Game_.dic[data.memberId] == Game_.youplaycount)
			{
				DeleOtherPlayerDate(1, "YouTrusteeship", youId);
			}
			if (Game_.dic[data.memberId] == Game_.zuoplaycount)
			{
				DeleOtherPlayerDate(3, "ZuoTrusteeship", zuoId);
			}
			if (Game_.dic[data.memberId] == Game_.shangplaycount)
			{
				DeleOtherPlayerDate(2, "ShangTrusteeship", shangId);
			}
		}
		if (UserId.JieCreateRoom != true)
		{
			Game_.WaitTipPeople.gameObject.SetActive(true);
		}
	}
	//自己退出
	public void MySetOutTable(OutTableData data)
	{
		Debug.Log("yu ++++ Ott");
		WebSoketCall._Instance.MicPhoneCallback -= Game_.MicPhone;
		Audiocontroller.Instance.delectVoice();
		Game_.IsAutoPut = false;
		UserId.GameState = false;
		UserId.JieCreateRoom = false;
		UserId.isJoinRoom = false;
		Game_.CurrentRound.gameObject.SetActive(false);
		Game_.YQHY.gameObject.SetActive(false);
		UserId.isCreateRoom = false;
		Game_.jie.CleanData();
		for (int i = 0; i < Game_.HandPic.Count; i++)
		{
			Game_.HandPic[i].sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "CSTX");
			Game_.handTuoName[i].text = null;
			Game_.HeadFamer[i].sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "CSTXK");
		}
		if (Game_.robCount >= 3)
		{
			StartCoroutine(Game_.OutSceen());
		}
		else { GameMapManager.Instance.NormalLoadScene("liang"); }
		return;
	}
	//其他玩家清理数据的方法
	public void DeleOtherPlayerDate(int SeatNum,string ObjString,int id)
	{
		Game_.HandPic[SeatNum].gameObject.GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "CSTX");
		Game_.handTuoName[SeatNum].text = null;
		Game_.HeadFamer[SeatNum].sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "CSTXK");
		GameObject.Find("TrusteeshipStatus").transform.Find(ObjString).gameObject.SetActive(false);
		Game_.playerId.Remove(id);
	}
}
