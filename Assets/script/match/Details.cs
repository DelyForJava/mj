using ILRuntime.Mono.Cecil.Cil;
using ILRuntime.Runtime;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Details : MonoBehaviour {

	//显示 排名和奖励 用到的Item
	public GameObject rankingItem;

	//排名的前三和后续的图片
	public List<Sprite> rankSprite = new List<Sprite>();

	//显示内容的面板
	GameObject xqPanel;
	GameObject rulePanel;
	GameObject awardPanel;
	GameObject rankingPanel;

	//左边type类型切换按钮
	Button Detail;
	Button Ranking;
	Button Award;
	Button Rule;

	DateilsDate dateilsDate;

	//排名Item的父物体
	GameObject backgroundParent;

	//排名数据
	List<RankingData> rangkData = new List<RankingData>();
	RankingData meRankData = new RankingData();

	//前20名的排名
	string RankingUrl = "http://" + Bridge.GetHostAndPort() + "/api/game/getEventsP";
	string ruleUrl = "http://" + Bridge.GetHostAndPort() + "/api/game/getRuleRewards";

	//自己的排名
	string MeRanking = "http://" + Bridge.GetHostAndPort() + "/api/game/getYourOwnRanking";

	// Use this for initialization
	void Awake() {

		//获取面板
		getPanel();
		//添加点击事件
		AddBtn();
	}

	void getPanel() {

		xqPanel=transform.Find("back/xiangqingPanel").gameObject;
		rulePanel= transform.Find("back/rulePanel").gameObject;
		awardPanel= transform.Find("back/awardPanel").gameObject;
		rankingPanel= transform.Find("back/rankingPanel").gameObject;

		backgroundParent = rankingPanel.transform.Find("back/content").gameObject;
	}

	void AddBtn() {

        transform.Find("back/close").GetComponent<Button>().onClick.AddListener(()=> {
			this.gameObject.gameObject.SetActive(false);
		});

		Detail=transform.Find("back/typePanel/xq").GetComponent<Button>();
        Ranking=transform.Find("back/typePanel/ranking").GetComponent<Button>();
		Award=transform.Find("back/typePanel/award").GetComponent<Button>();
		Rule=transform.Find("back/typePanel/rule").GetComponent<Button>();

		Detail.onClick.AddListener( () =>{ TypeOnClick(Detail); });
		Ranking.onClick.AddListener( () =>{ TypeOnClick(Ranking); });
		Award.onClick.AddListener( () =>{ TypeOnClick(Award); });
		Rule.onClick.AddListener( () =>{ TypeOnClick(Rule); });

		TypeOnClick(Detail); //默认
	}


	void TypeOnClick(Button button) {

        if (button== Detail)
        {
			Detail.interactable = false;
			Ranking.interactable = true;
			Award.interactable = true;
			Rule.interactable = true;

			xqPanel.gameObject.SetActive(true);
			awardPanel.gameObject.SetActive(false);
			rulePanel.gameObject.SetActive(false);
			rankingPanel.gameObject.SetActive(false);
		}
		if (button == Ranking)
		{
			Detail.interactable = true;
			Ranking.interactable = false;
			Award.interactable = true;
			Rule.interactable = true;

			xqPanel.gameObject.SetActive(false);
			awardPanel.gameObject.SetActive(false);
			rulePanel.gameObject.SetActive(false);
			rankingPanel.gameObject.SetActive(true);

			AddRankData();

		}
		if (button == Award)
		{
			Detail.interactable = true;
			Ranking.interactable = true;
			Award.interactable = false;
			Rule.interactable = true;

			xqPanel.gameObject.SetActive(false);
			awardPanel.gameObject.SetActive(true);
			rulePanel.gameObject.SetActive(false);
			rankingPanel.gameObject.SetActive(false);

			AddAwardData();
		}
		if (button == Rule)
		{
			Detail.interactable = true;
			Ranking.interactable = true;
			Award.interactable = true;
			Rule.interactable = false;

			xqPanel.gameObject.SetActive(false);
			awardPanel.gameObject.SetActive(false);
			rulePanel.gameObject.SetActive(true);
			rankingPanel.gameObject.SetActive(false);

			AddRuleData();
		}
	}


	//奖励显示内容 添加
	void AddAwardData() {

		//如果已经添加过数据就不再添加
        if (awardPanel.transform.Find("back/content").childCount!=0)      
			return;
        
		string[] rank = dateilsDate.data.reward.Split('\n');

        for (int i = 0; i < rank.Length; i++)
        {
			GameObject game = GameObject.Instantiate(rankingItem) as GameObject;
			game.transform.Find("rank").gameObject.SetActive(false);

			//这里是用的中文的分号 嗯 后端用的这个 
			string[] temp = rank[i].Split('：');

			game.transform.Find("num").GetComponent<Text>().text = temp[1].ToString();
			game.transform.Find("name").GetComponent<Text>().text = temp[0].ToString();

			game.transform.SetParent(awardPanel.transform.Find("back/content"));
		}
		//awardPanel.transform.Find("awardText").GetComponent<Text>().text = dateilsDate.data.reward.ToString();
	}


	//规则显示内容 添加
	void AddRuleData()
	{
		rulePanel.transform.Find("bg/Scroll View/Content/Text").GetComponent<Text>().text = dateilsDate.data.ruleText.ToString();
	}

	//排名显示数据 添加
	void AddRankData() {

        if (rankingPanel.transform.Find("back/content").childCount!=0)
			return;

        
        for (int i = 0; i < rangkData.Count; i++)
        {
			GameObject game = GameObject.Instantiate(rankingItem) as GameObject;

			game.transform.Find("name").GetComponent<Text>().text = rangkData[i].id.ToString();
			game.transform.Find("num").GetComponent<Text>().text = rangkData[i].evntPoints.ToString();

			game.transform.SetParent(rankingPanel.transform.Find("back/content"));

			game.transform.Find("rank/Text").GetComponent<Text>().text = (i + 1).ToString();

			if (i<3)          
				game.transform.Find("rank").GetComponent<Image>().sprite = rankSprite[i];
            else
            {
				game.transform.Find("rank").GetComponent<Image>().sprite = rankSprite[3];
				game.transform.Find("rank").GetComponent<Image>().SetNativeSize();
				game.transform.Find("rank/Text").transform.localPosition = game.transform.Find("rank/Text").transform.localPosition + new Vector3(0,10,0);
			}
		}
	}

	/// <summary>
	/// 默认显示赛事的详情
	/// </summary>
	/// <param name="ruleParameter">规则参数</param>
	/// <param name="signParameter">报名参数</param>
	/// <param name="matchData">当前被点击的赛事数据</param>
	public void getDetails(string ruleParameter, string signParameter, MatchData matchData ) {

		xqPanel.transform.Find("signterm").GetComponent<Text>().text = matchData.appConditions.ToString() + "金币";
		//string []temptime = matchData.startTm.Split(' ');
		xqPanel.transform.Find("time").GetComponent<Text>().text = matchData.startTm.ToString();

		xqPanel.transform.Find("rule").GetComponent<Text>().text = matchData.gamePlay == 1 ? "上饶麻将" : "南昌麻将";

		xqPanel.transform.Find("num").GetComponent<Text>().text = matchData.minNum.ToString() + "人";
		xqPanel.transform.Find("signnumber").GetComponent<Text>().text = matchData.memberCount.ToString() + "人";

		xqPanel.transform.Find("sign").GetComponent<Button>().onClick.AddListener( ()=> {
			JX.HttpCallSever.One().PostCallServer(EventsMatch.SignUrl, signParameter, Debug.Log);
			Debug.Log("报名------------");
		});

		JX.HttpCallSever.One().PostCallServer(ruleUrl, ruleParameter, getDetailsCallback);

		//-------------前20名排名---------
		string rankingParameter = signParameter;
		JX.HttpCallSever.One().PostCallServer(RankingUrl, rankingParameter, getRankingCallback);

		//-------------自己的排名---------
		JX.HttpCallSever.One().PostCallServer(MeRanking, rankingParameter, getMeRankingCallback);

	}

	//前20名
	void getRankingCallback(string Data) {

		Debug.Log("getRankingCallback---------------");
		JsonData jsonData = JsonMapper.ToObject(Data);

		for (int i = 0; i < jsonData["data"].Count; i++)
        {
			RankingData rankingData = new RankingData();

			rankingData.id = jsonData["data"][i]["id"].ToString();
			rankingData.evntPoints = (int)jsonData["data"][i]["evntPoints"];

			rangkData.Add(rankingData);
		}
	}

	//自己的排名
	void getMeRankingCallback(string Data)
	{

	}

	// Update is called once per frame
	void Update () {
		
	}

	void getDetailsCallback(string Data) {
		//Debug.Log("data------------"+Data.ToString());
		dateilsDate = JsonMapper.ToObject<DateilsDate>(Data);
	}
}


class RankingData {

	public string  id;
	public int evntPoints;
}

class DateilsDate
{

	public int code;
	public string message;
	public int count;
	public data data;
}

class data {
	public string reward;
	public string ruleText;
}
