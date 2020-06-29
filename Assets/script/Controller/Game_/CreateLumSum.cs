using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class CreateLumSum : MonoBehaviour {
	//接收存储的信息
	public List<string> HuList = new List<string>();
	public List<Image> UIHead = new List<Image>();
	public List<Image> HeadFamer = new List<Image>();
	public List<Text> NName = new List<Text>();
	public List<Text> GangPoint = new List<Text>();
	public List<Text> GoldPoint = new List<Text>();
	public List<Text> TableNumList = new List<Text>();
	public List<Text> HuData = new List<Text>();
	public List<int> Id = new List<int>();
	public List<int> point = new List<int>();
	public List<Image> WinPic = new List<Image>();
	public List<int> goldpoint_ = new List<int>();
	public List<int> GameIdList = new List<int>();
	
	//胡分 
	public int Id0, Id1, Id2, Id3;
	public int GangId0,GangId1,GangId2,GangId3;
	public int AllPoint0, AllPoint1, AllPoint2, AllPoint3;
	public int TableNum;


	private void Start()
	{
		GameObject.Find("Shake").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("Shake").GetComponent<Button>().onClick.AddListener(()=> { Sharke();});
	}
	public void DataDell()
	{
		Game_Controller gc = GameObject.Find("Gold_Game").GetComponent<Game_Controller>();
		
		for (int i = 0; i < HuList.Count; i++)
		{
			HuclassInfo huclass = JsonMapper.ToObject<HuclassInfo>(HuList[i]);
			for (int j = 0; j < huclass.players.Count; j++)
			{
				if (!Id.Contains(huclass.players[j].uid))
				{
					Id.Add(huclass.players[j].uid);
				}
				Member member = new Member();
				member.memberId =/* GameIdList[i].ToString()*/Id[j].ToString();
				string json = JsonMapper.ToJson(member);
				JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/member/getinfo", json, PlayerInfo);
				if (huclass.seatNum==0)
				{
					continue;
				}
				if (/*GameIdList[j]*/ Id[j]== huclass.memberId)
				{
					switch (j)
					{
						case 0:
							Id0++;
							break;
						case 1:
							Id1++;
							break;
						case 2:
							Id2++;
							break;
						case 3:
							Id3++;
							break;
					}
				}
			for (int k = 0; k < Id.Count; k++)
			{
				 if (/*GameIdList[k]*/Id[k] == huclass.players[j].uid)
				{
						switch (j)
						{
							case 0:
								GangId0 += huclass.gangPointsList[huclass.players[j].seatNum-1];
								AllPoint0 += huclass.goldChangeList[huclass.players[j].seatNum - 1];
								break;
							case 1:
								GangId1 += huclass.gangPointsList[huclass.players[j].seatNum - 1];
								AllPoint1 += huclass.goldChangeList[huclass.players[j].seatNum - 1];
								break;
							case 2:
								GangId2 += huclass.gangPointsList[huclass.players[j].seatNum - 1];
								AllPoint2 += huclass.goldChangeList[huclass.players[j].seatNum - 1];
								break;
							case 3:
								GangId3 += huclass.gangPointsList[huclass.players[j].seatNum - 1];
								AllPoint3 += huclass.goldChangeList[huclass.players[j].seatNum - 1];
								break;
						}
					}
				}
			}
		}
		point.Add(Id0);
		point.Add(Id1);
		point.Add(Id2);
		point.Add(Id3);
		int max = point[0];
		int index = 0;
		for (int i = 0; i < point.Count; i++)
		{
			if (max<point[i])
			{
				max = point[i];
				index = i;
			}
		}
		switch (index)
		{
			case 0:
				break;
			case 1:
				//WinPic[1].gameObject.SetActive(true);
				//ChangeData(1);
				break;
			case 2:
				//WinPic[2].gameObject.SetActive(true);
				//ChangeData(2);
				break;
			case 3:
				//WinPic[3].gameObject.SetActive(true);
				//ChangeData(3);
				break;
			default:
				break;
		}
	}
	public void PlayerInfo(string data)
	{
		Debug.Log(data);
		PlayerMessageInfo playerMessage = JsonMapper.ToObject<PlayerMessageInfo>(data);
		if (playerMessage.data.id==UserId.memberId)
		{
			UserId.goldCount = playerMessage.data.gold;
		}
		if (playerMessage.data.id==Id[0])
		{
			JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, UIHead[0]);
			NName[0].text = playerMessage.data.nickname;
			HeadFamer[0].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
			GoldPoint[0].text = /*AllPoint0.ToString()*/goldpoint_[0].ToString();
			GangPoint[0].text = GangId0.ToString();
			TableNumList[0].text = TableNum.ToString();
			HuData[0].text = Id0.ToString();
		}
		else if (playerMessage.data.id == Id[1])
		{
			JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, UIHead[1]);
			NName[1].text = playerMessage.data.nickname;
			HeadFamer[1].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
			GoldPoint[1].text =/* AllPoint1.ToString()*/goldpoint_[1].ToString();
			GangPoint[1].text = GangId1.ToString();
			TableNumList[1].text = TableNum.ToString();
			HuData[1].text = Id1.ToString();
		}
		else if (playerMessage.data.id == Id[2])
		{
			JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, UIHead[2]);
			NName[2].text = playerMessage.data.nickname;
			HeadFamer[2].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
			GoldPoint[2].text = /*AllPoint2.ToString()*/goldpoint_[2].ToString();
			GangPoint[2].text = GangId2.ToString();
			TableNumList[2].text = TableNum.ToString();
			HuData[2].text = Id2.ToString();
		}
		else if (playerMessage.data.id == Id[3])
		{
			JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, UIHead[3]);
			NName[3].text = playerMessage.data.nickname;
			HeadFamer[3].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
			GoldPoint[3].text = /*AllPoint3.ToString()*/goldpoint_[3].ToString();
			GangPoint[3].text = GangId3.ToString();
			TableNumList[3].text = TableNum.ToString();
			HuData[3].text = Id3.ToString();
		}
	}
	public void ChangeData(int Uid)
	{
		Sprite headFame = UIHead[0].sprite;
		string name = NName[0].text;
		Sprite HeadFamers = HeadFamer[0].sprite;
		string goldPoint = GoldPoint[0].text;
		string gangPoint = GangPoint[0].text;
		string huData = HuData[0].text;

		UIHead[0].sprite  =		UIHead[Uid].sprite;
		NName[0].text	  =		NName[Uid].text;
		HeadFamer[0].sprite =	HeadFamer[Uid].sprite;
		GoldPoint[0].text =		GoldPoint[Uid].text;
		GangPoint[0].text  =	GangPoint[Uid].text;
		HuData[0].text     =	HuData[Uid].text;

		UIHead[Uid].sprite   = headFame;
		NName[Uid].text = name;
		HeadFamer[Uid].sprite = HeadFamers;
		GoldPoint[Uid].text = goldPoint;
		GangPoint[Uid].text = gangPoint;
		HuData[Uid].text = huData;
	}
	public void Back()
	{
		gameObject.SetActive(false);
		OutTableData data = new OutTableData();
		data.actionCode = "OutTableAction";
		string outData = JsonMapper.ToJson(data);
		WebSoketCall.One().SendToWeb(outData);
	}
	public void Sharke()
	{
		ShartSDKControlle.Instance.ShateImage();
	}
}
