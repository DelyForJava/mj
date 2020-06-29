using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventsMatch : MonoBehaviour {

	public List<Sprite> signSpri=new List<Sprite>();

	public GameObject GridParent;
	public GameObject Item;

	public GameObject rulePanel;

	Button FishMatchBtn;
	Button MyMatchBtn;
	Button MatchBtn;

	//public EventTrigger butt;

	string selectUrl = "http://" + Bridge.GetHostAndPort() + "/api/game/getEvents";
	string selectSignUrl = "http://" + Bridge.GetHostAndPort() + "/api/game/getMemberEvents";
	public static string SignUrl = "http://" + Bridge.GetHostAndPort() + "/api/game/addEventsMember";

	string exitEventsUrl = "http://"+ Bridge.GetHostAndPort() + "/api/game/toRetire";

	//快速赛赛场数据
	List<MatchData> FishMatchData = new List<MatchData>();
	//比赛赛场数据
	List<MatchData> MatchData = new List<MatchData>();
	//我的赛场数据
	List<MatchData> MyMatchData = new List<MatchData>();

	//赛场数据
	ListMathData listMathData = new ListMathData();

	//刷新时间  
	float Heartbeattime = 0.0f;

	//已经报了名的赛场的evntId的list
	List<string> signEventData = new List<string>();

	//是否报名成功
	 bool isSignSuccess=false;

	//是否报名成功
	bool isExitSuccess = false;

	void Awake() {
		getMatchData();  //获取赛场比赛数据
		getGold();    //获取金币
	}

	void Start() {
		Invoke("AddBtn", 0.5f);  //添加点击事件
		//addbtn();  
	}


	//获取所有比赛场和我的比赛
	void getMatchData() {
		//获取已经报名了的比赛
		JX.HttpCallSever.One().PostCallServer(selectSignUrl, " ", selectSignCallback);
		//获取所有比赛
		JX.HttpCallSever.One().PostCallServer(selectUrl, " ", selectCallback);
	}

	//获取金币进行显示
	void getGold() {

		transform.Find("tip/gold/Text").transform.GetComponent<Text>().text = UserId.goldCount.ToString();
	}

	void AddBtn() {

		transform.Find("tip/close").GetComponent<Button>().onClick.AddListener(close);

		//transform.Find("tip/shop").GetComponent<Button>().onClick.AddListener(UiController._instance.GoShop);

		MatchBtn=transform.Find("typePanel/match").GetComponent<Button>();
		FishMatchBtn=transform.Find("typePanel/fishmatch").GetComponent<Button>();
		MyMatchBtn=transform.Find("typePanel/mymatch").GetComponent<Button>();

		MatchBtn.onClick.AddListener(() => { typeOnClick("match"); }) ;
		FishMatchBtn.onClick.AddListener(() => { typeOnClick("fishmatch"); });
		MyMatchBtn.onClick.AddListener(() => { typeOnClick("mymatch"); });


		typeOnClick("match");   //默认进入比赛场

	   //EventTrigger eventTrigger = butt;

		//EventTrigger.Entry entry = new EventTrigger.Entry();
		//entry.eventID = EventTriggerType.Drag;
		//entry.callback = new EventTrigger.TriggerEvent();
		//entry.callback.AddListener(debug);
		//eventTrigger.triggers.Add(entry);

	}

	void typeOnClick(string btnName) {
		
        if (btnName== "match")
        {
			MatchBtn.interactable = false;			
			FishMatchBtn.interactable = true;
			MyMatchBtn.interactable = true;

			refreshItem(MatchData);
		}
		if (btnName == "fishmatch")
		{
			MatchBtn.interactable = true;
			FishMatchBtn.interactable = false;
			MyMatchBtn.interactable = true;

			refreshItem(FishMatchData);
		}
		if (btnName == "mymatch")
		{
			MatchBtn.interactable = true;
			FishMatchBtn.interactable = true;
			MyMatchBtn.interactable = false;

			refreshItem(MyMatchData);
		}
	}

	//刷新比赛场
	void refreshItem(List<MatchData> ListData) {

		//Debug.Log("ListData.Count------------" + ListData.Count);
        if (GridParent.transform.childCount!=0)
        {
			for (int i = 0; i < GridParent.transform.childCount; i++)
			{
				Destroy(GridParent.transform.GetChild(i).gameObject);
			}
		}

		#region 之前的代码 先不用了


		//      if (ListData== FishMatchData)
		//      {
		//	for (int i = 0; i < ListData.Count; i++)
		//	{
		//		GameObject gameObject = GameObject.Instantiate(Item) as GameObject;

		//		gameObject.name = "match" + i;
		//		gameObject.transform.Find("name").GetComponent<Text>().text = ListData[i].evntNm.ToString();
		//		gameObject.transform.Find("type").GetComponent<Text>().text = ListData[i].gamePlay == 1 ? "淘汰赛,上饶麻将" : "淘汰赛";

		//		gameObject.transform.Find("time/now").GetComponent<Text>().text = ListData[i].startTm;

		//		gameObject.transform.Find("count").GetComponent<Text>().text = ListData[i].memberCount.ToString();

		//		gameObject.transform.Find("goldcount").GetComponent<Text>().text = ListData[i].appConditions.ToString();

		//		//转换string格式
		//		string signParameter = "{\"evntId\": \"" + ListData[i].evntId + "\"}";


		//		bool isSign = false;
		//		for (int j = 0; j < signEventData.Count; j++)
		//		{
		//			isSign = ListData[i].evntId == signEventData[j] ? true : false;

		//			if (isSign)
		//				break;
		//		}

		//		if (isSign)
		//		{
		//			//这里还没有封装 调用更换后的tool预制体和方法就可以了
		//			string exitParameter = signParameter;
		//			gameObject.transform.Find("btn").GetComponent<Image>().sprite = signSpri[1];
		//			gameObject.transform.Find("btn").GetComponent<Button>().onClick.AddListener(() => {
		//				//Debug.Log("-------------发送exitParameter--------" + exitParameter);
		//				JX.HttpCallSever.One().PostCallServer(exitEventsUrl, exitParameter, exitCallback);
		//			});
		//		}
		//		else
		//		{
		//			//这里还没有封装 调用更换后的tool预制体和方法就可以了
		//			gameObject.transform.Find("btn").GetComponent<Image>().sprite = signSpri[0];
		//			gameObject.transform.Find("btn").GetComponent<Button>().onClick.AddListener(() => {
		//				//Debug.Log("-------------发送--------" + signParameter);
		//				JX.HttpCallSever.One().PostCallServer(SignUrl, signParameter, SignCallback);
		//				//报名成功的话把button事件变为退赛状态 参赛和退赛传入的参数是一样的
		//				StartCoroutine(SignEventsCallBack(gameObject, signParameter));
		//			});
		//		}

		//		//获取规则需要的参数
		//		string ruleParameter = "{\"evtType\":" + ListData[i].evntTyp + ",\"gamePlay\":" + ListData[i].gamePlay + "}";
		//		MatchData tempMatchData = ListData[i];
		//		gameObject.AddComponent<Button>().onClick.AddListener(delegate () {
		//			rulePanel.gameObject.SetActive(true);
		//			rulePanel.GetComponent<Details>().getDetails(ruleParameter, signParameter, tempMatchData);
		//		});

		//		gameObject.transform.SetParent(GridParent.transform);
		//	}
		//}

		#endregion

		if (ListData == MyMatchData)
		{
			for (int i = 0; i < ListData.Count; i++)
			{
				GameObject gameObject = GameObject.Instantiate(Item) as GameObject;

				gameObject.name = "match" + i;
				gameObject.transform.Find("name").GetComponent<Text>().text = ListData[i].evntNm.ToString();
				gameObject.transform.Find("type").GetComponent<Text>().text = ListData[i].gamePlay == 1 ? "淘汰赛,上饶麻将" : "淘汰赛";

				//string[] temp = ListData[i].startTm.Split(' ');
				//string[] temptime = temp[1].Split(':');
				//ListData[i].startTm.ToString();
				gameObject.transform.Find("time/now").GetComponent<Text>().text = ListData[i].startTm;
				gameObject.transform.Find("count").GetComponent<Text>().text = ListData[i].memberCount.ToString();

				gameObject.transform.Find("goldcount").GetComponent<Text>().text = ListData[i].appConditions.ToString();
				gameObject.transform.SetParent(GridParent.transform);

				//这里还没有封装 调用更换后的tool预制体和方法就可以了
				string exitParameter = "{\"evntId\": \"" + ListData[i].evntId + "\"}"; 

				string EventID = ListData[i].evntId;
				gameObject.transform.Find("btn").GetComponent<Image>().sprite = signSpri[1];
				gameObject.transform.Find("btn").GetComponent<Button>().onClick.AddListener(() => {
					//Debug.Log("-------------发送exitParameter--------" + exitParameter);
					JX.HttpCallSever.One().PostCallServer(exitEventsUrl, exitParameter, exitCallback);

					//从我的比赛中退赛直接销毁   和其他赛场中的赛场退赛调用的不是同一个方法
					StartCoroutine(exitMeEventsCallback(gameObject));
				});

			}
        }
		// 快速赛和其他淘汰赛
        else
        {
			for (int i = 0; i < ListData.Count; i++)
			{
				GameObject gameObject = GameObject.Instantiate(Item) as GameObject;

				gameObject.name = "match" + i;
				gameObject.transform.Find("name").GetComponent<Text>().text = ListData[i].evntNm.ToString();
				gameObject.transform.Find("type").GetComponent<Text>().text = ListData[i].gamePlay == 1 ? "淘汰赛,上饶麻将" : "淘汰赛";

				gameObject.transform.Find("time/now").GetComponent<Text>().text = ListData[i].startTm;

				gameObject.transform.Find("count").GetComponent<Text>().text = ListData[i].memberCount.ToString();

				gameObject.transform.Find("goldcount").GetComponent<Text>().text = ListData[i].appConditions.ToString();

				//转换string格式
				string signParameter = "{\"evntId\": \"" + ListData[i].evntId + "\"}";


				bool isSign = false;				
				for (int j = 0; j < signEventData.Count; j++)
				{
					isSign = ListData[i].evntId == signEventData[j] ? true : false;

					if (isSign)
						break;
				}


				string EventID = ListData[i].evntId;
				//已经报了名的赛场
				if (isSign)
				{
					//这里还没有封装 调用更换后的tool预制体和方法就可以了
					string exitParameter = signParameter;
					gameObject.transform.Find("btn").GetComponent<Image>().sprite = signSpri[1];
					gameObject.transform.Find("btn").GetComponent<Button>().onClick.AddListener(() => {
						//Debug.Log("-------------发送exitParameter--------" + exitParameter);
						JX.HttpCallSever.One().PostCallServer(exitEventsUrl, exitParameter, exitCallback);

						StartCoroutine(exitEventsCallback(gameObject, EventID));
					});
				}
				//没有报名的赛场
				else
				{
					//这里还没有封装 调用更换后的tool预制体和方法就可以了
					gameObject.transform.Find("btn").GetComponent<Image>().sprite = signSpri[0];
					gameObject.transform.Find("btn").GetComponent<Button>().onClick.AddListener(() => {
						//Debug.Log("-------------发送--------" + signParameter);
						JX.HttpCallSever.One().PostCallServer(SignUrl, signParameter, SignCallback);

						//报名成功的话把button事件变为退赛状态 参赛和退赛传入的参数是一样的
						StartCoroutine(SignEventsCallBack(gameObject, EventID));
					});
				}

				//获取规则需要的参数
				string ruleParameter = "{\"evtType\":" + ListData[i].evntTyp + ",\"gamePlay\":" + ListData[i].gamePlay + "}";
				MatchData tempMatchData = ListData[i];
				gameObject.AddComponent<Button>().onClick.AddListener(delegate () {
					rulePanel.gameObject.SetActive(true);
					rulePanel.GetComponent<Details>().getDetails(ruleParameter, signParameter, tempMatchData);
				});

				gameObject.transform.SetParent(GridParent.transform);
			}
		}
	}

	//退赛回调
	void exitCallback(string Data) {

		//Debug.Log("退赛回调-------------------"+Data);
		JsonData jsonData = JsonMapper.ToObject(Data);
        if ((int)jsonData["code"]==200)
        {		
			Debug.Log("退赛成功-------------");
			//刷新金币
			Prefabs.PopBubble("退赛成功!金币已退回");
			UserId.goldCount = (int)jsonData["data"]["eventsMember"]["gold"];
			getGold();
			isExitSuccess = true;
		}
	}

	//退赛成功的话将button改为报名状态
	IEnumerator  exitEventsCallback(GameObject obj, string EventID)
	{		
		yield return new WaitForSeconds(0.5f);
		
		if (isExitSuccess)
		{		
			obj.transform.Find("btn").GetComponent<Button>().onClick.RemoveAllListeners();

			obj.transform.Find("btn").GetComponent<Image>().sprite = signSpri[0];

			string signParameter = "{\"evntId\": \"" + EventID + "\"}";

			obj.transform.Find("btn").GetComponent<Button>().onClick.AddListener(() => {
				//参赛
				JX.HttpCallSever.One().PostCallServer(exitEventsUrl, signParameter, SignCallback);

				StartCoroutine(SignEventsCallBack(obj, EventID));
			});

			//刷新 已经报名的比赛场
			JX.HttpCallSever.One().PostCallServer(selectSignUrl, " ", selectSignCallback);

			isExitSuccess = false;
		}
	}


	//从我的比赛里面点退赛直接消除Item
	IEnumerator exitMeEventsCallback(GameObject obj)
	{
		yield return new WaitForSeconds(0.5f);

		if (isExitSuccess)
		{
			//刷新 已经报名的比赛场
			JX.HttpCallSever.One().PostCallServer(selectSignUrl, " ", selectSignCallback);

			Destroy(obj);
			isExitSuccess = false;
		}
	}


	//参赛回调
	void SignCallback(string Data) {

		Debug.Log("------------SignCallback Data---" + Data);

		JsonData jsonData = JsonMapper.ToObject(Data);
        if ((int)jsonData["code"]==200)
        {
			signEventData.Add(jsonData["data"]["eventsMember"]["evntId"].ToString());

			isSignSuccess = true;

			Prefabs.PopBubble("报名成功!");
			//刷新金币
			UserId.goldCount = (int)jsonData["data"]["eventsMember"]["gold"];
			getGold();
		}
		if ((int)jsonData["code"] == 500)
		{
			Prefabs.PopBubble(jsonData["message"].ToString());
			//Debug.Log("金币不足----------------------");
		}
	}

	//报名成功的话将button改为退赛状态
	IEnumerator SignEventsCallBack(GameObject obj,string EventID) {

		yield return new WaitForSeconds(0.5f);
        if (isSignSuccess)
        {
			string exitParameter = "{\"evntId\": \"" + EventID + "\"}";

			obj.transform.Find("btn").GetComponent<Button>().onClick.RemoveAllListeners();

			obj.transform.Find("btn").GetComponent<Image>().sprite= signSpri[1];

			obj.transform.Find("btn").GetComponent<Button>().onClick.AddListener(()=>{
				//退赛
				JX.HttpCallSever.One().PostCallServer(exitEventsUrl, exitParameter, exitCallback);

				StartCoroutine(exitEventsCallback(obj, EventID)); 
			});

			//刷新 已经报名的比赛场
			JX.HttpCallSever.One().PostCallServer(selectSignUrl, " ", selectSignCallback);

			isSignSuccess = false;
		}
	}



	//查找所有比赛场数据的回调
	void selectCallback(string Data) {

		//Debug.Log("--------------获取数据------------"+ Data.ToString());
        try
        {
            listMathData = JsonMapper.ToObject<ListMathData>(Data);
        }
        catch (System.Exception)
        {
            throw;
        }

        if (listMathData.code==200)
        {
            if (FishMatchData.Count!=0)           
				FishMatchData.Clear();
            if (MatchData.Count!=0)           
				MatchData.Clear();
			
			for (int i = 0; i < listMathData.data.Count; i++)
			{
                if (listMathData.data[i].evntTyp == 2)              
                    FishMatchData.Add(listMathData.data[i]);              
                else if (listMathData.data[i].evntTyp == 1)              
                    MatchData.Add(listMathData.data[i]);
                
            }
		}
	}

	//查找自己报名了的赛场
	void selectSignCallback(string Data) {

		ListMathData MeSignEventsData = JsonMapper.ToObject<ListMathData>(Data);

		signEventData.Clear();
		MyMatchData.Clear();

		if (MeSignEventsData.code == 200)
		{
			//已经报名了的比赛场的ID
			for (int i = 0; i < MeSignEventsData.data.Count; i++)
			{
				signEventData.Add(MeSignEventsData.data[i].evntId.ToString());
			}
			//我已经报名的赛场其他数据
			for (int i = 0; i < MeSignEventsData.data.Count; i++)
			{
				MyMatchData.Add(MeSignEventsData.data[i]);
			}
		}

		//Debug.Log("---获取已经报名的比赛--------" + signEventData.Count);
	}

    //public void debug(BaseEventData baseEventData)
    //{
    //    PointerEventData pointerEventData = baseEventData as PointerEventData;
    //    Debug.Log("------------pointerEventData.position----" + pointerEventData.position);
    //}

    void close() {
		SceneManager.LoadScene("liang");
		//Destroy(this.gameObject);
	}

	
	void Update () {

        if ((Heartbeattime += Time.deltaTime) >=10)
        {
			//暂时没有主动刷新  被动刷新  只是刷新了数据  还没有对现在显示的数据刷新
			getMatchData();			
			//Debug.Log("-----------心跳置零");
			Heartbeattime = 0.0f;
		}
	}
}


//public class signEventsData{
   
	
	
//}


public class ListMathData {

	public int code;
	public string message;
	public int count;
	public List<MatchData> data;
}

public class MatchData{

	public int id;
	public string evntNm;
	public int evntTyp;
	//条件
	public int appConditions;

	public int memberCount;
	public int gamePlay;
	public int minNum;
	public int maxNum;
	public string startTm;
	public string endTm;
	public string creationTm;
	public string updateTm;
	public string evntId;
}
