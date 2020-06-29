using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using LitJson;
using WebSocketSharp;

public class CreatRoomController : MonoBehaviour {
	private enum JuShu
	{
		FourGu,
		SixGu,
		Twelve
	}
	private enum Chips
	{
		TwoChips,
		FourChips
	}
	private enum TopingOff
	{
		NoOff,
		FiftyOff,//50
		HundRedOff,//100
		OneHundredAndFiftyOff,//150
		TwoHundRedOff//200
	}
	private bool VerifyBasics(TopingOff state, int score)
	{
		switch (state)
		{
			case TopingOff.NoOff:
				return true;
			case TopingOff.FiftyOff:
				return score > 0 && score <= 50;
			case TopingOff.HundRedOff:
				return score > 0 && score <= 100;
			case TopingOff.OneHundredAndFiftyOff:
				return score > 0 &&  score <=150;
			case TopingOff.TwoHundRedOff:
				return score > 0 && score <=200;
			default:
				return false;
		}
		
	}
	private JuShu Gender;
	private Chips Gender1;
	private TopingOff Gender2;

	public ToggleGroup Group;
	public ToggleGroup Group1;
	public ToggleGroup Group2;
	public Text DiFei;
	int DiFeiNum=0;
	//局数
	public int Innings;
	//底分
	public int EndPoints;
	public int maxpoint=-1;

	public Transform IsAutoPut;

	private bool IsAuto;
	void Start () {
		DiFeiNum = 1;
		DiFei.GetComponent<InputField>().text = DiFeiNum.ToString();
		Gender = JuShu.FourGu;
		Gender1 = Chips.TwoChips;
		Gender2 = TopingOff.NoOff;
		Innings = 4;
		EndPoints = 0;
		Transform t=GameObject.Find("jushu").transform;
		for (int i = 0; i < t.childCount; i++)
		{
			if (i == 3) { continue;}
			t.GetChild(i).GetComponent<Toggle>().onValueChanged.AddListener(OnGenderChanged);
		}
		Transform fengding = GameObject.Find("feiding").transform;
		for (int i = 0; i < fengding.childCount; i++)
		{
			if (fengding.GetChild(i).GetComponent<Toggle>() != null)
			{
				fengding.GetChild(i).GetComponent<Toggle>().onValueChanged.AddListener(OnGender2Changed);
			}
		}
		DiFeiNum = int.Parse(DiFei.GetComponent<InputField>().text) == 1 ? 0 : int.Parse(DiFei.GetComponent<InputField>().text);

		IsAutoPut.GetComponent<Button>().onClick.AddListener(setAutoPut);
	}

	void setAutoPut() {

		if (IsAutoPut.Find("close").gameObject.activeSelf)		
			IsAutoPut.Find("close").gameObject.SetActive(false);		
		else		
			IsAutoPut.Find("close").gameObject.SetActive(true);

		IsAuto = IsAutoPut.Find("close").gameObject.activeSelf;

	}


	public void AddDiFei()
	{
		
		if(VerifyBasics(Gender2, int.Parse(DiFei.GetComponent<InputField>().text)))
		{
			DiFeiNum += 10;
			if (Gender2 == TopingOff.NoOff)
			{
				DiFei.GetComponent<InputField>().text = DiFeiNum.ToString();
			}else if (DiFeiNum>maxpoint)
			{
				DiFeiNum = maxpoint;
			}

			DiFei.GetComponent<InputField>().text = DiFeiNum.ToString();
		}
		else
		{
			//TODO 提示根据底分限制来增加分数
			Prefabs.Buoy("提示根据限制输入分数！");
		}

	}
	public void DeleteDiFei()
	{
		DiFeiNum = int.Parse(DiFei.GetComponent<InputField>().text);
		DiFeiNum -=10;
		if (DiFeiNum<=0)
		{
			Prefabs.PopBubble("底分不能低于1!");
			DiFeiNum = 1;
		}
		DiFei.GetComponent<InputField>().text = DiFeiNum.ToString();
	}
	public void OnGenderChanged(bool isOn)
	{
		if (!isOn)
		{
			return;
		}
		foreach (Toggle t in Group.ActiveToggles())
		{
			switch (t.name)
			{
				case "FourGu":
					Debug.Log("dfsfa");
					Innings = 4;
					break;
				case "SixGu":
					Innings = 6;
					break;
				case "Twelve":
					Innings = 12;
					break;				
			}
		}		
	}
	public void OnGender1Changed(bool isOn)
	{
		if (!isOn)
		{
			return;
		}
		foreach (Toggle t in Group.ActiveToggles())
		{
			switch (t.name)
			{
				case "TwoChips":
					break;
				case "FourChips":
					break;
			}
		}

	}
	public void OnGender2Changed(bool isOn)
	{
		if (!isOn)
		{
			return;
		}
		Debug.Log("isOn========="+ isOn);
		foreach (Toggle t in Group2.ActiveToggles())
		{
			Debug.Log("t.name======" + t.name);
			switch (t.name)
			{
				case "NoOff":
					maxpoint = -1;
					Gender2 = TopingOff.NoOff;
					break;
				case "FiftyOff":
					Debug.Log("50bei");
					maxpoint = 50;
					Gender2 = TopingOff.FiftyOff;
					break;
				case "HundRedOff":
					maxpoint = 100;
					Gender2 = TopingOff.HundRedOff;
					break;
				case "OneHundredAndFiftyOff":
					maxpoint = 150;
					Gender2 = TopingOff.OneHundredAndFiftyOff;
					break;
				case "TwoHundRedOff":
					maxpoint = 200;
					Gender2 = TopingOff.TwoHundRedOff;
					break;				
			}
		}

	}
	
	public void CloseWindown()
	{
		Audiocontroller.Instance.PlayAudio("Back");
	    transform.Find("bg").transform.DOMoveY(1.4f, 0.8f).OnComplete(() => { Destroy(gameObject); });
	}
	public void CreateTable()
	{
		if (LoadManager.Instance.async != null)
		{
			if (LoadManager.Instance.async.progress > 0)
			{
				Debug.Log("场景跳转中.....");
				return;
			}
		}
		EndPoints = VerifyBasics(Gender2, int.Parse(DiFei.text)) ? int.Parse(DiFei.text) : 0;
		if (EndPoints == 0)
		{
			//TODO 提示根据限制输入分数
			Prefabs.Buoy("提示根据限制输入分数！");
		}
		else
		{
			if (WebSoketCall.One().ws.IsConnected) { 
			UserId.isCreateRoom = true;
			UserId.JieCreateRoom = true;
			LoadManager.Instance.LoadScene("mjGameScreen", CreateRoom, EndPoints,maxpoint);
			}
			else
			{
				Prefabs.PopBubble("与服务器连接不佳");
			}
		}
		

	}
	public void CreateRoom(int data,int maxpoint)
	{
		Debug.Log(data);
		Debug.Log("maxpoint" + maxpoint);
		Debug.Log(DiFeiNum);
		Debug.Log("inning"+Innings);
		UserId.GameType = Innings;

		Debug.Log("是否可以托管============="+ IsAuto);

		GameObject.Find("Gold_Game").GetComponent<Game_Controller>().Createround = Innings;
		GameObject.Find("Gold_Game").GetComponent<Game_Controller>().IsAutoPut= IsAuto;
		GameObject.Find("Gold_Game").GetComponent<Game_Controller>().Rount_ = Innings;
		GameObject.Find("Gold_Game").GetComponent<Game_Controller>().CreateRoom(data,maxpoint);
		GameObject.Find("Gold_Game").GetComponent<Game_Controller>().TableNumShow.gameObject.SetActive(true);
		GameObject.Find("Gold_Game").GetComponent<Game_Controller>().isCreateRoom = true;
	}

	
}
