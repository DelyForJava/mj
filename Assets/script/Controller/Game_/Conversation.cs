using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Conversation : MonoBehaviour {

	public enum SelectionState
	{
		Expression,
		phrase,
		dialogue
	}
	public Transform BiaoQingParent;
	public Transform phraseParent;
	public Transform dialogueParent;
	public Transform ExpressionShowParent;
	public ToggleGroup Group;
	public SelectionState CurrentState;
	public Game_Controller game_;
	public string ExpressionABName;
	//商品信息字典
	private Dictionary<int, string> GoodsInfo;
	void Awake()
	{		
		GoodsInfo = new Dictionary<int, string>();	
	}
	void Start () {
		
		//短语字典初始化
		Initle();
		//获取当前账号拥有的表情包
		StartCoroutine(ShopIE());
		//快捷短语
		Phrase();
	}

	/// <summary>
	/// 获取商城的商品  
	/// 获取当前账号拥有的表情包
	/// 进行对比赋值
	/// </summary>
	/// <returns></returns>
	IEnumerator ShopIE()
	{
		//获取商城商品
		JX.HttpCallSever.One().GetCallSetver("http://" + Bridge.GetHostAndPort() + "/api/goods/shop", ShopGoodsCallBack);
		yield return new WaitForSeconds(0.5f);

		//-----------------------------------------------
		Member m = new Member();
		m.memberId = UserId.memberId.ToString();
		string json = JsonMapper.ToJson(m);
		//获取当前账号拥有的物品
		JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/goods/owned", json, CallBack);
	}

	/////
	/// <summary>
	/// 每种表情包的切换
	/// </summary>
	public void Expression(string expressionName)
	{
		for (int i = BiaoQingParent.childCount - 1; i >= 0; i--){Destroy(BiaoQingParent.GetChild(i).gameObject);}
	
		List<Sprite> ss = Bridge._instance.LoadFace(expressionName);
		for (int i = 0; i < ss.Count; i++)
		{
			//GameObject g = Prefabs.LoadCell("talk#biaoqingRes", BiaoQingParent);
			GameObject g = Bridge._instance.LoadAbDate(LoadAb.Game, "biaoqingRes", BiaoQingParent);
			g.transform.GetChild(0).GetComponent<Image>().sprite = ss[i];
			EventTrigger trigger = g.transform.GetChild(0).GetComponent<EventTrigger>();
			if (trigger == null)			
				trigger = g.transform.GetChild(0).gameObject.AddComponent<EventTrigger>();
			
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback = new EventTrigger.TriggerEvent();
			entry.callback.AddListener(OnClick);
			trigger.triggers.Add(entry);
		}
		
	}


	/// <summary>
	/// 获取当前账号拥有的表情包
	/// </summary>
	/// <param name="data"></param>
	void CallBack(string data)
	{
		GoodsDataInfo goods= JsonMapper.ToObject<GoodsDataInfo>(data);

		for (int i = 0; i < goods.data.goods.Count; i++)
		{
			if (goods.data.goods[i].category == "brow")
			{	
				ExpressionABName = GoodsInfo[goods.data.goods[i].shopGoodsId];
				
				GameObject express = Bridge._instance.LoadAbDate(LoadAb.Game, "ExpressionIcon", ExpressionShowParent);
				List<Sprite> ss = Bridge._instance.LoadFace(ExpressionABName);

			    express.transform.GetComponent<Image>().sprite = ss[0];				
				express.transform.GetComponent<Image>().SetNativeSize();

				express.name = ExpressionABName;

				EventTrigger trigger = express.transform.GetComponent<EventTrigger>();
				if (trigger == null)				
					trigger = express.transform.gameObject.AddComponent<EventTrigger>();
				
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerClick;
				entry.callback = new EventTrigger.TriggerEvent();
				entry.callback.AddListener(ExpressionIconClick);    //切换表情
				trigger.triggers.Add(entry);	
			}			
		}
		Expression("麻将宝");  //默认出现的第一套表情包
	}


	/// <summary>
	/// 默认的表情包
	/// </summary>
	//void AddDef() {

	//	ExpressionABName = "麻将宝";

	//	GameObject express = Bridge._instance.LoadAbDate(LoadAb.Game, "ExpressionIcon", ExpressionShowParent);
	//	List<Sprite> ss = Bridge._instance.LoadFace(ExpressionABName);

	//	express.transform.GetComponent<Image>().sprite = ss[0];

	//	express.name = ExpressionABName;

	//	EventTrigger trigger = express.transform.GetComponent<EventTrigger>();
	//	if (trigger == null)
	//		trigger = express.transform.gameObject.AddComponent<EventTrigger>();

	//	EventTrigger.Entry entry = new EventTrigger.Entry();
	//	entry.eventID = EventTriggerType.PointerClick;
	//	entry.callback = new EventTrigger.TriggerEvent();
	//	entry.callback.AddListener(ExpressionIconClick);    //切换表情
	//	trigger.triggers.Add(entry);
	//}


	/// <summary>
	/// 获取商城商品
	/// </summary>
	/// <param name="data"></param>
	void ShopGoodsCallBack(string data)
	{
		ShopGoodsInfo shop = JsonMapper.ToObject<ShopGoodsInfo>(data);
		for (int i = 0; i < shop.data.shop.Count; i++){GoodsInfo.Add(shop.data.shop[i].id,shop.data.shop[i].name);}
	}

	/// <summary>
	/// 切换表情包
	/// </summary>
	/// <param name="pointData"></param>
	void ExpressionIconClick(BaseEventData pointData)
	{
		PointerEventData pointD = pointData as PointerEventData;
		string spriteName = pointD.pointerPress.transform.name;
		Expression(spriteName);    
	}

	/// <summary>
	/// 发送表情
	/// </summary>
	/// <param name="pointData"></param>
	void OnClick(BaseEventData pointData)
	{
		PointerEventData pointD = pointData as PointerEventData;
		string  spriteName=pointD.pointerPress.transform.GetComponent<Image>().sprite.name;
		GameMessageData game = new GameMessageData();
		game.actionCode = "GameMessage";
		game.tableNum = game_.tableNum;
		game.seatNum = game_.seatNum;
		game.selection = 0;
		game.msg = spriteName;
		string json=JsonMapper.ToJson(game);
		JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/game/table/message", json,ActionCode);
	}


	void ActionCode(string data)
	{
		//Debug.Log(data);
	}
	/// <summary>
	/// Key_String 是对象名 Value 是要显示的文字
	/// </summary>
	public Dictionary<string, string> ParaseDic = new Dictionary<string, string>();
	/// <summary>
	/// 初始化快捷短语
	/// </summary>
	public void Initle()
	{
		ParaseDic.Add("language1", "你太牛了");
		ParaseDic.Add("language2", "手气真好");
		ParaseDic.Add("language3", "快点出牌哦");
		ParaseDic.Add("language4", "今天真高兴");
		ParaseDic.Add("language5", "这个吃的好");
		ParaseDic.Add("language6", "你放炮我不胡");
		ParaseDic.Add("language7", "你家是开银行的吧");
		ParaseDic.Add("language8", "不好意思，我有事先走一步了");
		ParaseDic.Add("language9", "你的牌打的也太好了啦");
	}
	/// <summary>
	/// 快捷短语
	/// </summary>
	public void Phrase()
	{
		for (int i =BiaoQingParent.childCount-1; i >=0 ; i--){DestroyImmediate(BiaoQingParent.GetChild(i).gameObject);}			
		
		phraseParent.gameObject.SetActive(true);
		Transform parent = phraseParent.Find("Content");

		for (int i = 0; i < parent.childCount; i++)
		{
			string s = parent.GetChild(i).name;
			parent.GetChild(i).transform.Find("text").GetComponent<Button>().onClick.AddListener(() =>
			{
				string NeedInfo = ParaseDic[s];
				GameMessageData game = new GameMessageData();
				game.actionCode = "GameMessage";
				game.seatNum = game_.seatNum;
				game.tableNum = game_.tableNum;
				Debug.Log(game_.tableNum);
				game.selection = 1;
				game.msg = NeedInfo;
				string json = JsonMapper.ToJson(game);
				JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/game/table/message", json, ActionCode);

			});
		}
	}

	public void PhrasePressEvent(string name)
	{
		Audiocontroller.Instance.PlayAudio(name);
	}
}
