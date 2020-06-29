using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using LitJson;

public class JoinRoomController : MonoBehaviour {

	//数字显示
	public Text[] numString;
	public bool StartGame;
	public string EnterTableMessage;
	public bool  IsPress_;
	//放在第几位
	int numCount=-1;	
	public void ShowNum(int num)
	{

		if (numCount >= 5)
        {
			return;
        }
		numCount++;
		ShowHideText(numCount < 0);
		numString[numCount].text = num.ToString();
	}
	void Start()
	{
		Button JoinButton=transform.Find("bg/jiangpan/sure").GetComponent<Button>();
	}
	public void DeleNumString()
	{
		if (numCount < 0) return;
		numString[numCount].text = null;
		numCount--;
		ShowHideText(numCount < 0);

	}
	public void closeJoinRoom()
	{
		Audiocontroller.Instance.PlayAudio("Back");
	    transform.Find("bg").transform.DOMoveY(1.4f, 0.8f).OnComplete(() => { Destroy(gameObject); });
	}

	public void JoinRoom()
	{
		string table="";
		for (int i = 0; i < numString.Length; i++)
		{
			if (numString[i].text==null)
			{
				Prefabs.PopBubble("您输入的信息不全请重新输入");
				//Debug.LogError("数据不全");
				return;
			}
			table+=numString[i].text;
 		}
		if (table=="")
		{
			table = "0";
		}
		int tn=int.Parse(table);
		if (tn<100000)
		{
			Prefabs.PopBubble("您输入的信息不全请重新输入");
			return;
		}
		UserId.JieCreateRoom = true;
		UserId.isJoinRoom = true;
		WebSocketInfo web = new WebSocketInfo();
		web.actionCode = "EnterTableAction";
		web.Params = new WebSocketInfo.data();
		Debug.Log(table);
		web.Params.code = int.Parse(table);
		web.Params.type = 0;
		string json = JsonMapper.ToJson(web);
		Debug.Log(json);
		WebSoketCall.One().SendToWeb(json);
		UserId.LoadWSEnterTable = true;
		//Invoke("CheckInfo",0.1f);
	}
	//void CheckInfo()
	//{
	//	JsonData js = JsonMapper.ToObject(WebSoketCall.One().eData);
	//	string edata1 = (string)js["actionCode"];
	//	if (edata1 == "EnterTableAction")
	//	{
	//		UserId.isJoinRoom = true;
	//		UserId.JieCreateRoom = true;
	//		EnterTableMessage = WebSoketCall.One().eData;
	//		LoadManager.Instance.LoadScene("mjGameScreen",EnterTableUser,WebSoketCall.One().eData);
	//		//Bridge._instance.LoadAbDate(LoadAb.Login, "loadbar");
	//	}
	//	else if (edata1 == "error")
	//	{
	//		//Prefabs.PopBubble("房间号不存在请重新输入");
	//	}
	//}
	void EnterTableUser(string data)
	{
		GameObject.Find("Gold_Game").GetComponent<Game_Controller>().EnterTableAction(data);
	}
	
	void ShowHideText(bool showOrHide)
    {
        foreach (var item in numString)
        {
			item.transform.GetChild(0).gameObject.SetActive(showOrHide);
        }

    }

}
