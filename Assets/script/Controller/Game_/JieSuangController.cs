using DG.Tweening;
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JieSuangController : MonoBehaviour {
	#region 对象
	public Transform Pai;
	public Transform You;
	public Transform Zuo;
	public Transform shang;
	#endregion

	#region 游戏对象
	public Transform GamePai;
	public Transform GameYou;
	public Transform GameZuo;
	public Transform Gameshang;
	#endregion
	//牌数量显示
	public Text OtherPaiShow;
	//癞子牌
	public int rascalCard;
	public int HuCard;
	public int tableNum;
	//谁胡了牌 
	public int HuTheme;
	public int Type;
	//T是不可以嗯F是可以嗯
	public bool IsPullOk;
	//换房type
	#region 座位号
	public int mySeatNum;
	public int youSeatNum;
	public int zuoSeatNum;
	public int shangSeatNum;
	#endregion

	#region 癞子信息
	public int MyrascalCount;
	#endregion
	public bool isJieCreate;
	//麻将图片
	public Sprite[] maJiangPai;
	public Dictionary<int, Sprite> listCard = new Dictionary<int, Sprite>();
	//玩家头像0为直接1为右边2为上边3为左边
	public List<Image> playerHeads = new List<Image>();
	//玩家个人信息
	public List<int> playerInfo = new List<int>();
	public List<string> PlayerHeadAddess = new List<string>();
	public List<int> goldChangeList = new List<int>();
	public List<int> gangPointsList = new List<int>();
	[Header("破产的显示")]
	public List<Image> NoMoney = new List<Image>();


	public string HuDescrite;
	public bool isFlowBurea;
	public bool isPlayerBack;
	public bool isNeedShowInfo;
	public int Bookmaker;
	//分数显示位置
	public List<Text> HuDesAim;
	public List<Text> number = new List<Text>();
	public List<Text> Des = new List<Text>();
	//Key_Uid Value_SeatNum
	public Dictionary<int, int> UidASeatNum = new Dictionary<int, int>();
	//庄家图片
	public List<Image> BookPic = new List<Image>();
	public List<Text> NameList = new List<Text>();
	[Header("头像框显示")]
	public List<Image> HeadFamer = new List<Image>();
	public Transform LumSum;
	public List<Image> BookPicGameSceen = new List<Image>();

	public int MyRasacal;
	public int YouRascal;
	public int ShangRasCal;
	public int ZuoRasCal;
	#region 手牌信息
	public List<int> MyPaiCard;
	public List<int> YouPaiCard;
	public List<int> ZuoPaiCard;
	public List<int> ShangPaiCard;
	#endregion

	//连接场景破产
	public bool isOverGame;
	#region 技能信息
	public List<int> MySkillCard;
	public List<int> YouSkillCard;
	public List<int> ZuoSkillCard;
	public List<int> ShangSkillCard;
	#endregion

	public void Initle()
	{
		MyPaiCard = new List<int>();
		YouPaiCard = new List<int>();
		ZuoPaiCard = new List<int>();
		ShangPaiCard = new List<int>();
		playerInfo = new List<int>();
		//PlayerHeadAddess = new List<string>();
		goldChangeList = new List<int>();
		gangPointsList = new List<int>();
	}


	void GetInitialStause(Transform object_)
	{
		if (object_.childCount > 13)
		{
			int a = object_.childCount - 13;
			for (int i = 0; i < a; i++)
			{
				DestroyImmediate(object_.GetChild(object_.childCount - 1 - i).gameObject);
			}
			if (object_ == Pai && HuTheme == mySeatNum)
			{
				GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/MypaiInfo/huPai").gameObject;
				(go.transform as RectTransform).anchoredPosition -= new Vector2(160, 0);
			}
			else if (object_ == You && HuTheme == youSeatNum)
			{
				GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/YouInfo/huPai").gameObject;
				(go.transform as RectTransform).anchoredPosition -= new Vector2(160, 0);
			}
			else if (object_ == shang && HuTheme == shangSeatNum)
			{
				GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/ShangInfo/huPai").gameObject;
				(go.transform as RectTransform).anchoredPosition -= new Vector2(160, 0);
			}
			else if (object_ == Zuo && HuTheme == zuoSeatNum)
			{
				GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/ZuoInfo/huPai").gameObject;
				(go.transform as RectTransform).anchoredPosition -= new Vector2(160, 0);
			}
		}
	}
	//引用层级
	public void SorePeople(Transform obj)
	{
		for (int i = 0; i < obj.childCount; i++)
		{
			obj.GetChild(i).SetSiblingIndex(i);
		}
	}
	bool isAllOutTable;
	int count = 0;
	public class MebreeID
	{
		public int Member;
	}

	public void StartJieSuang()
	{
		for (int i = 0; i < 4; i++)
		{
			if (i + 1 == mySeatNum) {
				number[0].text = "<color=#bab6b0><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>";
				if (goldChangeList[i] > 0)
				{
					if (goldChangeList[i] > 0) { number[0].text = "<color=#f5dd69><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>"; }
				}
			}
			else if (i + 1 == youSeatNum) { number[1].text = "<color=#bab6b0><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>";
				if (goldChangeList[i] > 0)
				{
					if (goldChangeList[i] > 0) { number[1].text = "<color=#f5dd69><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>"; }
				}
			}
			else if (i + 1 == shangSeatNum) { number[2].text = "<color=#bab6b0><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>";
				if (goldChangeList[i] > 0)
				{
					if (goldChangeList[i] > 0) { number[2].text = "<color=#f5dd69><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>"; }
				}
			}
			else if (i + 1 == zuoSeatNum) { number[3].text = "<color=#bab6b0><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>";
				if (goldChangeList[i] > 0)
				{
					if (goldChangeList[i] > 0) { number[3].text = "<color=#f5dd69><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>"; }
				}
			}
			BookPic[i].gameObject.SetActive(false);
			NoMoney[i].gameObject.SetActive(false);
			HuDesAim[i].text = null;
		}
		ReacalCareShow(Pai, MyRasacal, true);
		ReacalCareShow(You, YouRascal, true);
		ReacalCareShow(shang, ShangRasCal, true);
		ReacalCareShow(Zuo, ZuoRasCal, true);
		SorePeople(Pai);
		SorePeople(You);
		SorePeople(shang);
		SorePeople(Zuo);
		GameObject go0 = transform.Find("Bg/Scroll View/Viewport/Content/ZuoInfo/huPai").gameObject;
		GameObject go01 = transform.Find("Bg/Scroll View/Viewport/Content/MypaiInfo/huPai").gameObject;
		GameObject go02 = transform.Find("Bg/Scroll View/Viewport/Content/ShangInfo/huPai").gameObject;
		GameObject go03 = transform.Find("Bg/Scroll View/Viewport/Content/YouInfo/huPai").gameObject;
		go0.transform.GetChild(1).gameObject.SetActive(false);
		go01.transform.GetChild(1).gameObject.SetActive(false);
		go02.transform.GetChild(1).gameObject.SetActive(false);
		go03.transform.GetChild(1).gameObject.SetActive(false);
		Game_Controller gc = GameObject.Find("Gold_Game").GetComponent<Game_Controller>();
		gc.NoGameStart = true;
		gc.SHowTableCount++;
		gc.ShowTable.text = gc.SHowTableCount.ToString();
		Bookmaker = gc.Bookmaker;
		if (HuTheme == 0) { isFlowBurea = true; }
		if (HuCard == 0) { HuCard = gc.DrawCPai; }
		UidASeatNum = gc.dic;
		playerInfo = gc.playerId;
		if (gc.playerInfo != null)
		{
			isNeedShowInfo = true;
			PlayerHeadAddess = new List<string>();
			PlayerHeadAddess = gc.playerInfo;
		}
		else { /*Debug.LogError("当前没有打开isNeedShowInfo");*/}
		if (isNeedShowInfo)
		{
			isNeedShowInfo = false;
			#region 玩家信息补全
			for (int i = 0; i < PlayerHeadAddess.Count; i++)
			{
				PlayerMessageInfo playerMessage = JsonMapper.ToObject<PlayerMessageInfo>(PlayerHeadAddess[i]);
				if (UidASeatNum[playerMessage.data.id] == mySeatNum) {
					NameList[0].text = playerMessage.data.nickname;
					if (playerMessage.data.gold < 0) { isOverGame = true;
						NoMoney[0].gameObject.SetActive(true);
					}
					else { isOverGame = false; }
					if (UidASeatNum[playerMessage.data.id] == Bookmaker)
					{
						BookPic[0].gameObject.SetActive(true);
					}

					JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, playerHeads[0]);
					HeadFamer[0].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
					Debug.Log("HeadFamer" + "shop" + "/" + "head_" + playerMessage.data.headFrame);
				}
				else if (UidASeatNum[playerMessage.data.id] == youSeatNum) {
					JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, playerHeads[1]);
					if (playerMessage.data.gold < 0) {
						NoMoney[1].gameObject.SetActive(true);
					}
					else { }
					HeadFamer[1].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
					Debug.Log("HeadFamer1" + "shop" + "/" + "head_" + playerMessage.data.headFrame);
					if (youSeatNum == Bookmaker)
					{
						BookPic[1].gameObject.SetActive(true);
					}
					NameList[1].text = playerMessage.data.nickname;
				}
				else if (UidASeatNum[playerMessage.data.id] == shangSeatNum) { JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, playerHeads[2]);
					NameList[2].text = playerMessage.data.nickname;
					if (playerMessage.data.gold < 0)
					{
						NoMoney[2].gameObject.SetActive(true);
						//ToDoPoiChang
					}
					else { }
					HeadFamer[2].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
					Debug.Log("HeadFamer" + "shop" + "/" + "head_" + playerMessage.data.headFrame);
					if (shangSeatNum == Bookmaker)
					{
						BookPic[2].gameObject.SetActive(true);
					}
				}
				else if (UidASeatNum[playerMessage.data.id] == zuoSeatNum) { JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, playerHeads[3]);
					NameList[3].text = playerMessage.data.nickname;
					HeadFamer[3].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
					if (playerMessage.data.gold < 0)
					{
						NoMoney[3].gameObject.SetActive(true);
						//ToDoPoiChang
					}
					else { }
					if (zuoSeatNum == Bookmaker)
					{
						BookPic[3].gameObject.SetActive(true);
					}
				}
			}
			#endregion
		}
		else
		{
			if (mySeatNum == Bookmaker)
			{
				BookPic[0].gameObject.SetActive(true);
			}
			else if (youSeatNum == Bookmaker)
			{
				BookPic[1].gameObject.SetActive(true);
			}
			else if (shangSeatNum == Bookmaker)
			{
				BookPic[2].gameObject.SetActive(true);
			}
			else if (youSeatNum == Bookmaker)
			{
				BookPic[3].gameObject.SetActive(true);
			}
		}
		GetplayerInfo();
		if (isFlowBurea)
		{
			isFlowBurea = false;
			Transform icon = transform.Find("Bg/icon");
			icon.Find("icon_Defult").gameObject.SetActive(false);
			transform.Find("Bg/WinLight").gameObject.SetActive(false);
			icon.Find("icon_Win").gameObject.SetActive(false);
			icon.Find("icon_place").gameObject.SetActive(true);
			for (int i = 0; i < Des.Count; i++)
			{
				Des[i].text = HuDescrite;
			}
			if (HuTheme == mySeatNum) { MyPaiCard.Remove(HuCard); }
			else if (HuTheme == youSeatNum) { YouPaiCard.Remove(HuCard); }
			else if (HuTheme == zuoSeatNum) { ZuoPaiCard.Remove(HuCard); }
			else if (HuTheme == shangSeatNum) { ShangPaiCard.Remove(HuCard); }
			if (MyPaiCard.Count > 13)
			{
				int num = MyPaiCard.Count - Pai.childCount;
				for (int i = 0; i < num; i++)
				{
					//GameObject gg = Prefabs.LoadCell("majiang#Pai", Pai);
					GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", Pai);
					RectTransform r = gg.transform as RectTransform;
					r.anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
					r.anchoredPosition += new Vector2(60, 0);
				}
			}
			if (YouPaiCard.Count > 13)
			{
				int num1 = YouPaiCard.Count - You.childCount;
				for (int i = 0; i < num1; i++)
				{
					//GameObject gg = Prefabs.LoadCell("majiang#Pai", You);
					GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", You);
					RectTransform r = gg.transform as RectTransform;
					r.anchoredPosition = (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition;
					r.anchoredPosition += new Vector2(60, 0);
				}
			}
			if (ZuoPaiCard.Count > 13)
			{
				int num = ZuoPaiCard.Count - Zuo.childCount;
				for (int i = 0; i < num; i++)
				{
					//GameObject gg = Prefabs.LoadCell("majiang#Pai", Zuo);
					GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", Zuo);
					RectTransform r = gg.transform as RectTransform;
					r.anchoredPosition = (Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition;
					r.anchoredPosition += new Vector2(60, 0);
				}
			}
			if (ShangPaiCard.Count > 13)
			{
				int num = ShangPaiCard.Count - shang.childCount;
				for (int i = 0; i < num; i++)
				{
					//GameObject gg = Prefabs.LoadCell("majiang#Pai", shang);
					GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", shang);
					RectTransform r = gg.transform as RectTransform;
					r.anchoredPosition = (shang.GetChild(shang.childCount - 2) as RectTransform).anchoredPosition;
					r.anchoredPosition += new Vector2(60, 0);
				}
			}
			//Todo将icon 换成和局
			transform.Find("Bg/BackGroud").GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "lost");
			transform.Find("lostanimator").gameObject.SetActive(false);
			Transform icon1 = transform.Find("Bg/icon");
			icon1.Find("icon_Defult").gameObject.SetActive(false);
			icon1.Find("icon_Win").gameObject.SetActive(false);
			icon1.Find("icon_place").gameObject.SetActive(true);
			ObjectCard(Pai, GamePai, "majiang#Pai");
			OtherCard(You, GameYou, "majiang#Pai");
			OtherCard(Zuo, GameZuo, "majiang#Pai");
			OtherCard(shang, Gameshang, "majiang#Pai");

			return;
		}

		#region 手牌显示
		if (HuTheme == mySeatNum)
		{
			#region icon
			transform.Find("Bg/BackGroud").GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "Win");
			Transform icon = transform.Find("Bg/icon");
			transform.Find("Bg/WinLight").gameObject.SetActive(true);
			transform.Find("Bg/WinLight").GetComponent<Image>().DOFade(0.5f, 1).SetLoops(-1, LoopType.Yoyo);
			icon.Find("icon_Defult").gameObject.SetActive(false);
			transform.Find("lostanimator").gameObject.SetActive(false);
			icon.Find("icon_Win").gameObject.SetActive(true);
			icon.Find("icon_place").gameObject.SetActive(false);
			#endregion

			#region 将胡牌显示出来
			GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/MypaiInfo/huPai").gameObject;
			transform.Find("Bg/Scroll View/Viewport/Content/YouInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/ShangInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/ZuoInfo/huPai").gameObject.SetActive(false);
			HuDesAim[0].text = HuDescrite;
			Debug.Log(HuDescrite);
			go.SetActive(true);
			go.transform.GetChild(0).GetComponent<Image>().sprite = listCard[HuCard];
			#endregion

			#region 除去胡的牌

			MyPaiCard.Remove(HuCard);
			#endregion

			#region 判定是否杠了
			if (MyPaiCard.Count != Pai.childCount)
			{
				Debug.Log("ChangTT");
				if (MyPaiCard.Count > Pai.childCount)
				{
					int Num = Pai.childCount;
					for (int i = 0; i < (MyPaiCard.Count - Num); i++)
					{
						Debug.Log("My");
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", Pai);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetAsLastSibling();
					}
				(go.transform as RectTransform).anchoredPosition += new Vector2(160, 0);
				}
			}
			if (YouPaiCard.Count != You.childCount)
			{
				if (YouPaiCard.Count > You.childCount)
				{
					int Num = You.childCount;
					for (int i = 0; i < (YouPaiCard.Count - Num); i++)
					{
						Debug.Log("You");
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", You);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetAsLastSibling();
					}
				}

			}
			if (ZuoPaiCard.Count != Zuo.childCount) {
				if (ZuoPaiCard.Count > Zuo.childCount)
				{
					Debug.Log(ZuoPaiCard.Count);
					Debug.Log("Zuo");
					int Num = Zuo.childCount;
					for (int i = 0; i < (ZuoPaiCard.Count - Num); i++)
					{
						GameObject gg1 = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", Zuo);
						RectTransform r1 = gg1.transform as RectTransform;
						r1.anchoredPosition = (Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition;
						r1.anchoredPosition += new Vector2(60, 0);
						gg1.transform.SetAsLastSibling();
					}
				}

			}
			if (ShangPaiCard.Count != shang.childCount) {
				if (ShangPaiCard.Count > shang.childCount)
				{
					Debug.Log(ShangPaiCard.Count);
					Debug.Log("Shang");
					int Num = shang.childCount;
					for (int i = 0; i < (ShangPaiCard.Count - Num); i++)
					{
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", shang);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (shang.GetChild(shang.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetAsLastSibling();
					}
				}

			}
			#endregion

			Debug.Log("Pai" + Pai.childCount);
			Debug.Log("MyPaiCard" + MyPaiCard.Count);
			#region 将牌换掉
			ChangCard(Pai, MyPaiCard);
			ChangCard(You, YouPaiCard);
			ChangCard(shang, ShangPaiCard);
			ChangCard(Zuo, ZuoPaiCard);
			#endregion
			if (HuCard == rascalCard)
			{
				go.transform.GetChild(1).gameObject.SetActive(true);
				MyRasacal -= 1;
			}
			Debug.Log("MyRascal" + MyRasacal);
			Debug.Log("YouRascal" + YouRascal);
			Debug.Log("ShangRascal" + ShangRasCal);
			Debug.Log("ZuoRasCal" + ZuoRasCal);
			ReacalCareShow(Pai, MyRasacal, false);
			ReacalCareShow(You, YouRascal, false);
			ReacalCareShow(shang, ShangRasCal, false);
			ReacalCareShow(Zuo, ZuoRasCal, false);
			ChiIII();
		}
		else if (HuTheme == youSeatNum)
		{
			#region 将胡牌显示出来
			GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/YouInfo/huPai").gameObject;
			transform.Find("Bg/Scroll View/Viewport/Content/MypaiInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/ShangInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/ZuoInfo/huPai").gameObject.SetActive(false);
			HuDesAim[1].text = HuDescrite;
			Debug.Log(HuDescrite);
			go.SetActive(true);
			go.transform.GetChild(0).GetComponent<Image>().sprite = listCard[HuCard];
			#endregion

			#region icon
			transform.Find("Bg/BackGroud").GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "lost");
			Transform icon = transform.Find("Bg/icon");
			transform.Find("Bg/WinLight").gameObject.SetActive(false);
			
			icon.Find("icon_Defult").gameObject.SetActive(true);
			icon.Find("icon_Win").gameObject.SetActive(false);
			icon.Find("icon_place").gameObject.SetActive(false);
			transform.Find("lostanimator").gameObject.SetActive(true);
			#endregion

			YouPaiCard.Remove(HuCard);

			#region 判定是否杠了
			if (MyPaiCard.Count != Pai.childCount)
			{
				if (MyPaiCard.Count > Pai.childCount)
				{
					int Num = Pai.childCount;
					for (int i = 0; i < (MyPaiCard.Count - Num); i++)
					{
						Debug.Log("My");
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", Pai);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetAsLastSibling();
					}

				}
			}
			if (YouPaiCard.Count != You.childCount)
			{
				if (YouPaiCard.Count > You.childCount)
				{
					int Num = You.childCount;
					for (int i = 0; i < (YouPaiCard.Count - Num); i++)
					{
						Debug.Log("You");
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", You);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetAsLastSibling();
					}
					(go.transform as RectTransform).anchoredPosition += new Vector2(160, 0);
				}

			}
			if (ZuoPaiCard.Count != Zuo.childCount)
			{
				if (ZuoPaiCard.Count > Zuo.childCount)
				{
					Debug.Log(ZuoPaiCard.Count);
					Debug.Log("Zuo");
					int Num = Zuo.childCount;
					for (int i = 0; i < (ZuoPaiCard.Count - Num); i++)
					{
						GameObject gg1 = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", Zuo);
						RectTransform r1 = gg1.transform as RectTransform;
						r1.anchoredPosition = (Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition;
						r1.anchoredPosition += new Vector2(60, 0);
						gg1.transform.SetAsLastSibling();
					}
				}
			}
			if (ShangPaiCard.Count != shang.childCount)
			{
				if (ShangPaiCard.Count > shang.childCount)
				{
					Debug.Log(ShangPaiCard.Count);
					Debug.Log("Shang");
					int Num = shang.childCount;
					for (int i = 0; i < (ShangPaiCard.Count - Num); i++)
					{
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", shang);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (shang.GetChild(shang.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetAsLastSibling();
					}
				}
			}

			#endregion

			ChangCard(Pai, MyPaiCard);
			ChangCard(You, YouPaiCard);
			ChangCard(shang, ShangPaiCard);
			ChangCard(Zuo, ZuoPaiCard);

			if (HuCard == rascalCard)
			{
				go.transform.GetChild(1).gameObject.SetActive(true);
				YouRascal -= 1;
			}
			ReacalCareShow(Pai, MyRasacal, false);
			ReacalCareShow(You, YouRascal, false);
			ReacalCareShow(shang, ShangRasCal, false);
			ReacalCareShow(Zuo, ZuoRasCal, false);
			ChiIII();
		}
		else if (HuTheme == shangSeatNum)
		{
			GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/ShangInfo/huPai").gameObject;
			transform.Find("Bg/Scroll View/Viewport/Content/MypaiInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/YouInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/ZuoInfo/huPai").gameObject.SetActive(false);
			HuDesAim[2].text = HuDescrite;
			Debug.Log(HuDescrite);
			go.SetActive(true);
			go.transform.GetChild(0).GetComponent<Image>().sprite = listCard[HuCard];

			#region icon
			transform.Find("Bg/BackGroud").GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "lost");
			Transform icon = transform.Find("Bg/icon");
			transform.Find("Bg/WinLight").gameObject.SetActive(false);
			icon.Find("icon_Defult").gameObject.SetActive(true);
			icon.Find("icon_Win").gameObject.SetActive(false);
			icon.Find("icon_place").gameObject.SetActive(false);
			transform.Find("lostanimator").gameObject.SetActive(true);
			#endregion


			ShangPaiCard.Remove(HuCard);

			#region 判定是否杠了
			if (MyPaiCard.Count != Pai.childCount)
			{
				if (MyPaiCard.Count > Pai.childCount)
				{
					int Num = Pai.childCount;
					for (int i = 0; i < (MyPaiCard.Count - Num); i++)
					{
						Debug.Log("My");
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", Pai);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetAsLastSibling();
					}

				}
			}
			if (YouPaiCard.Count != You.childCount)
			{
				if (YouPaiCard.Count > You.childCount)
				{
					int Num = You.childCount;
					for (int i = 0; i < (YouPaiCard.Count - Num); i++)
					{
						Debug.Log("You");
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", You);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetAsLastSibling();
					}

				}
			}
			if (ZuoPaiCard.Count != Zuo.childCount)
			{
				if (ZuoPaiCard.Count > Zuo.childCount)
				{
					Debug.Log(ZuoPaiCard.Count);
					Debug.Log("Zuo");
					int Num = Zuo.childCount;
					for (int i = 0; i < (ZuoPaiCard.Count - Num); i++)
					{
						GameObject gg1 = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", Zuo);
						RectTransform r1 = gg1.transform as RectTransform;
						r1.anchoredPosition = (Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition;
						r1.anchoredPosition += new Vector2(60, 0);
						gg1.transform.SetAsLastSibling();
					}
				}

			}
			if (ShangPaiCard.Count != shang.childCount)
			{
				if (ShangPaiCard.Count > shang.childCount)
				{
					Debug.Log(ShangPaiCard.Count);
					Debug.Log("Shang");
					int Num = shang.childCount;
					for (int i = 0; i < (ShangPaiCard.Count - Num); i++)
					{
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", shang);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (shang.GetChild(shang.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetSiblingIndex(shang.childCount - 1);
					}
					(go.transform as RectTransform).anchoredPosition += new Vector2(160, 0);
				}
			}
			#endregion

			ChangCard(Pai, MyPaiCard);
			ChangCard(You, YouPaiCard);
			ChangCard(shang, ShangPaiCard);
			ChangCard(Zuo, ZuoPaiCard);

			if (HuCard == rascalCard)
			{
				go.transform.GetChild(1).gameObject.SetActive(true);
				ShangRasCal -= 1;
			}
			ReacalCareShow(Pai, MyRasacal, false);
			ReacalCareShow(You, YouRascal, false);
			ReacalCareShow(shang, ShangRasCal, false);
			ReacalCareShow(Zuo, ZuoRasCal, false);
			ChiIII();
		}
		else if (HuTheme == zuoSeatNum)
		{
			GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/ZuoInfo/huPai").gameObject;
			transform.Find("Bg/Scroll View/Viewport/Content/MypaiInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/ShangInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/YouInfo/huPai").gameObject.SetActive(false);
			HuDesAim[3].text = HuDescrite;
			Debug.Log(HuDescrite);
			go.SetActive(true);
			go.transform.GetChild(0).GetComponent<Image>().sprite = listCard[HuCard];

			#region icon
			transform.Find("Bg/BackGroud").GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "lost");
			transform.Find("Bg/WinLight").gameObject.SetActive(false);
			Transform icon = transform.Find("Bg/icon");
			icon.Find("icon_Defult").gameObject.SetActive(true);
			icon.Find("icon_Win").gameObject.SetActive(false);
			icon.Find("icon_place").gameObject.SetActive(false);
			transform.Find("lostanimator").gameObject.SetActive(true);
			#endregion

			ZuoPaiCard.Remove(HuCard);

			#region 判定是否杠了
			if (MyPaiCard.Count != Pai.childCount)
			{
				if (MyPaiCard.Count > Pai.childCount)
				{
					int Num = Pai.childCount;
					for (int i = 0; i < (MyPaiCard.Count - Num); i++)
					{
						Debug.Log("My");
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", Pai);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetAsLastSibling();
					}

				}
			}
			if (YouPaiCard.Count != You.childCount)
			{
				if (YouPaiCard.Count > You.childCount)
				{
					int Num = You.childCount;
					for (int i = 0; i < (YouPaiCard.Count - Num); i++)
					{
						Debug.Log("You");
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", You);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetAsLastSibling();
					}

				}
			}
			if (ZuoPaiCard.Count != Zuo.childCount)
			{
				if (ZuoPaiCard.Count > Zuo.childCount)
				{
					Debug.Log(ZuoPaiCard.Count);
					Debug.Log("Zuo");
					int Num = Zuo.childCount;
					for (int i = 0; i < (ZuoPaiCard.Count - Num); i++)
					{
						GameObject gg1 = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", Zuo);
						RectTransform r1 = gg1.transform as RectTransform;
						r1.anchoredPosition = (Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition;
						r1.anchoredPosition += new Vector2(60, 0);
						gg1.transform.SetAsLastSibling();
					}
					(go.transform as RectTransform).anchoredPosition += new Vector2(160, 0);
				}
			}
			if (ShangPaiCard.Count != shang.childCount)
			{
				if (ShangPaiCard.Count > shang.childCount)
				{
					Debug.Log(ShangPaiCard.Count);
					Debug.Log("Shang");
					int Num = shang.childCount;
					for (int i = 0; i < (ShangPaiCard.Count - Num); i++)
					{
						GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", shang);
						RectTransform r = gg.transform as RectTransform;
						r.anchoredPosition = (shang.GetChild(shang.childCount - 2) as RectTransform).anchoredPosition;
						r.anchoredPosition += new Vector2(60, 0);
						gg.transform.SetAsLastSibling();
					}

				}
			}
			#endregion

			ChangCard(Pai, MyPaiCard);
			ChangCard(You, YouPaiCard);
			ChangCard(shang, ShangPaiCard);
			ChangCard(Zuo, ZuoPaiCard);

			if (HuCard == rascalCard)
			{
				go.transform.GetChild(1).gameObject.SetActive(true);
				ZuoRasCal -= 1;
			}
			ReacalCareShow(Pai, MyRasacal, false);
			ReacalCareShow(You, YouRascal, false);
			ReacalCareShow(shang, ShangRasCal, false);
			ReacalCareShow(Zuo, ZuoRasCal, false);
			ChiIII();
		}
		#endregion
		Invoke("CLD", 5);
	}
	public void CLD() {
		Game_Controller gc = GameObject.Find("Gold_Game").GetComponent<Game_Controller>();
		if (gc.NoGameStart != false && gc.YP.childCount <= 0)
		{
			#region 打出牌清空
			GameObject My = GameObject.Find("My_PaiDui");
			GameObject Zuo = GameObject.Find("Zuo_PaiDui");
			GameObject You = GameObject.Find("You_PaiDui");
			GameObject Shang = GameObject.Find("Shang_PaiDui");
			DeletChildTranform(My.transform);
			DeletChildTranform(Zuo.transform);
			DeletChildTranform(You.transform);
			DeletChildTranform(Shang.transform);
			#endregion
			#region 游戏数据恢复
			MaJiang.Instance.Majiang_ = null;
			Game_Controller game = GameObject.Find("Gold_Game").GetComponent<Game_Controller>();
			game.HungUpData.Clear(); game.isHangUpOver = true;
			game.StatrFristPai = true;
			game.OutTableIEData.Clear(); game.isOverOutTableIE = true;
			game.AllReallyActionData = null;
			game.robCount = 0;
			game.EnterTableIEData.Clear(); game.isOverEnterTable = true;
			game.ReallyInfo.Clear(); game.isReallyShowOver = true;
			game.reascalPic.transform.parent.gameObject.SetActive(false);
			game.OtherPaiShow.transform.parent.gameObject.SetActive(false);
			game.reascalPic.transform.parent.gameObject.SetActive(true);
			game.OtherPaiShow.transform.parent.gameObject.SetActive(true);
			game.playerInfo.Clear();
			game.isGameStart = false; game.isOwn = false; game.isSkillUse = false; game.isCreateRoom = false;
			game.PlayerActionDataOfLink.Clear(); game.CurrentZXCount = 0;
			game.DrawActionData.Clear(); game.CurrentDrawCount = 0;
			game.isHungUp = false; game.isOverPlayerAction = true; game.isOverStartDraw = true;
			game.isReconnection = false; game.isSendMessage = false; game.activePTrusteeship = false;
			game.HangUpData = null; game.ReConnectionData = null; game.one.Clear();
			game.twotwo.Clear(); game.three.Clear();
			game.isMingGang = false; game.isYouMingGang = false; game.isShangMingGang = false; game.isZuoMingGang = false;
			game.OverDarw = false; game.CurrentDrawCount = 0; game.isOverPlayerAction = false; game.TotalTime = 15;
			game.isShow = false; game.StartTG = false; game.isTalk = false;
			game.anima.enabled = true; game.Yanima.enabled = true; game.Sanima.enabled = true; game.Zanima.enabled = true;
			#endregion
		}
	}
	public void ReacalCareShow(Transform Theme, int rnum, bool isJie)
	{
		if (isJie)
		{
			for (int i = 0; i < Theme.childCount; i++)
			{
				Theme.GetChild(Theme.childCount - 1 - i).GetChild(1).gameObject.SetActive(false);
			}
		}
	}
	public void Check(Transform obj)
	{
		if (rascalCard == 0)
		{
			Game_Controller gc = GameObject.Find("Gold_Game").GetComponent<Game_Controller>();
			rascalCard = gc.rascalCard;
		}
		for (int i = 0; i < obj.childCount; i++)
		{
			if (obj.GetChild(i).GetChild(0).GetComponent<Image>().sprite != listCard[rascalCard])
			{
				obj.GetChild(i).GetChild(1).gameObject.SetActive(false);
			}
			else
			{
				obj.GetChild(i).GetChild(1).gameObject.SetActive(true);
			}
		}
	}
	public void ChiIII()
	{
		Check(Pai);
		Check(You);
		Check(Zuo);
		Check(shang);
	}
	/// <summary>
	/// 创建房间开始大结算
	/// </summary>
	public void StartAllCombute()
	{
		gameObject.SetActive(false);
		//todo大结算
		//Prefabs.PopBubble("我是大结算！！！！！");
		LumSum.gameObject.SetActive(true);
		CreateLumSum Lum = LumSum.GetComponent<CreateLumSum>();
		//获取用户id
		Lum.DataDell();
	}
	public void PaiInit()
	{
		SorePeople(Pai);
		SorePeople(You);
		SorePeople(shang);
		SorePeople(Zuo);
		GetInitialStause(Pai);
		GetInitialStause(You);
		GetInitialStause(shang);
		GetInitialStause(Zuo);
	}
	public void GetplayerInfo()
	{
		for (int i = 0; i < playerInfo.Count; i++)
		{
			Member member = new Member();
			member.memberId = playerInfo[i].ToString();
			string json = JsonMapper.ToJson(member);
			JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/member/getinfo", json, PlayerInfo);
		}
	}
	public void PlayerInfo(string data)
	{
		PlayerMessageInfo playerMessage = JsonMapper.ToObject<PlayerMessageInfo>(data);
		Debug.Log(playerMessage.data.gold + goldChangeList[mySeatNum - 1]);

		if (UserId.memberId == playerMessage.data.id)
		{
			UserId.goldCount = playerMessage.data.gold;
			if (playerMessage.data.gold < 0)
			{
				isOverGame = true;
				NoMoney[0].gameObject.SetActive(true);
			}
			else { isOverGame = false; }
		}
		if (UidASeatNum.ContainsKey(playerMessage.data.id)) {
			if (youSeatNum == UidASeatNum[playerMessage.data.id])
			{
				if (playerMessage.data.gold < 0)
				{
					NoMoney[1].gameObject.SetActive(true);
				}
				else { }
			}
			else if (shangSeatNum == UidASeatNum[playerMessage.data.id])
			{
				if (playerMessage.data.gold < 0)
				{
					NoMoney[2].gameObject.SetActive(true);
				}
				else { }
			}
			else if (zuoSeatNum == UidASeatNum[playerMessage.data.id])
			{
				if (playerMessage.data.gold < 0)
				{
					NoMoney[3].gameObject.SetActive(true);
				}
				else { }
			}
		}
	}
	public void ObjectCard(Transform pai, Transform GameO, string Res)
	{
		Debug.Log(pai.name+":"+pai.childCount);
		Debug.Log(GameO.name+":"+GameO.childCount);
		for (int i = 0; i < pai.childCount; i++)
		{
			pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite = GameO.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
		}
	}
	public void OtherCard(Transform pai, Transform Game, string Res)
	{
		if (isFlowBurea == true)
		{
			isFlowBurea = false;
			int a = Pai.childCount - Game.childCount;
			for (int i = 0; i < pai.childCount - a; i++)
			{
				pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Game.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite;
			}
			return;
		}
		for (int i = 0; i < pai.childCount; i++)
		{
			pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Game.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite;
		}
	}
	public void ChangCard(Transform parent, List<int> handCard)
	{
		if (parent.childCount != handCard.Count)
		{
			Debug.Log(parent.childCount);
			Debug.Log(handCard.Count);
			Debug.Log("结算界面数据有问题");
			handCard.Add(HuCard);
			//int a = parent.childCount - handCard.Count;
			//AdministrationCard(parent,a);
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			if (handCard[i] == rascalCard)
			{
				parent.GetChild(i).GetChild(1).gameObject.SetActive(true);
			}
			parent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = listCard[handCard[i]];
		}
	}
	public void AdministrationCard(Transform obj, int DorJ)
	{
		if (DorJ > 0)
		{
			for (int i = 0; i < DorJ; i++)
			{
				GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", obj);
				RectTransform r = gg.transform as RectTransform;
				r.anchoredPosition = (obj.GetChild(obj.childCount - 2) as RectTransform).anchoredPosition;
				r.anchoredPosition += new Vector2(60, 0);
				gg.transform.SetSiblingIndex(obj.childCount - 1);
			}
		}
	}
	public void CleanData()
	{
		#region 将玩家位置重制
		Transform mDingwei = GamePai.GetChild(0);
		RectTransform myDingWei = mDingwei as RectTransform;
		int a = GamePai.childCount - 13;
		//将手牌数恢复到13
		for (int i = 0; i < a; i++)
		{
			Destroy(GamePai.GetChild(GamePai.childCount - 1 - i).gameObject);
		}
		//将位置恢复
		for (int i = 0; i < GamePai.childCount; i++)
		{
			Image pic = GamePai.GetChild(i).GetComponent<Image>();
			pic.enabled = true;
			GamePai.GetChild(i).GetChild(0).gameObject.SetActive(true);
			GamePai.GetChild(i).GetChild(1).gameObject.SetActive(false);
			GamePai.GetChild(i).GetChild(2).gameObject.SetActive(false);
			(GamePai.GetChild(i).transform as RectTransform).anchoredPosition = new Vector2(-13, 21);
			GamePai.gameObject.SetActive(false);
			if (GamePai.GetChild(i).childCount >3) { Destroy(GamePai.GetChild(i).GetChild(3).gameObject);}
			if (i == 0)
			{
				GamePai.GetChild(i).name = "PaiRes_My";
			}
			else { GamePai.GetChild(i).name = "Image" + " " + "(" + i + ")"; }
		}
		ReallyStartGameZY(GameYou);
		ReallyStartGameZY(GameZuo);
		ReallyStartGame(Gameshang);
		#endregion
		#region 打出牌清空
		GameObject My = GameObject.Find("My_PaiDui");
		GameObject Zuo = GameObject.Find("Zuo_PaiDui");
		GameObject You = GameObject.Find("You_PaiDui");
		GameObject Shang = GameObject.Find("Shang_PaiDui");
		DeletChildTranform(My.transform);
		DeletChildTranform(Zuo.transform);
		DeletChildTranform(You.transform);
		DeletChildTranform(Shang.transform);
		#endregion
		#region 游戏数据恢复
		MaJiang.Instance.Majiang_ = null;
		Game_Controller game = GameObject.Find("Gold_Game").GetComponent<Game_Controller>();
		game.HungUpData.Clear(); game.isHangUpOver = true;
		game.StatrFristPai = true;
		game.OutTableIEData.Clear(); game.isOverOutTableIE = true;
		game.AllReallyActionData = null;
		game.robCount = 0;
		game.EnterTableIEData.Clear(); game.isOverEnterTable = true;
		game.ReallyInfo.Clear(); game.isReallyShowOver = true;
		game.reascalPic.transform.parent.gameObject.SetActive(false);
		game.OtherPaiShow.transform.parent.gameObject.SetActive(false);
		game.reascalPic.transform.parent.gameObject.SetActive(true);
		game.OtherPaiShow.transform.parent.gameObject.SetActive(true);
		game.playerInfo.Clear();
		game.isGameStart = false; game.isOwn = false; game.isSkillUse = false; game.isCreateRoom = false;
		game.PlayerActionDataOfLink.Clear(); game.CurrentZXCount = 0;
		game.DrawActionData.Clear(); game.CurrentDrawCount = 0;
		game.isHungUp = false; game.isOverPlayerAction = true; game.isOverStartDraw = true;
		game.isReconnection = false; game.isSendMessage = false; game.activePTrusteeship = false;
		game.HangUpData = null; game.ReConnectionData = null; game.one.Clear();
		game.twotwo.Clear(); game.three.Clear(); 
		game.isMingGang = false; game.isYouMingGang = false; game.isShangMingGang = false; game.isZuoMingGang = false;
		game.OverDarw = false; game.CurrentDrawCount = 0; game.isOverPlayerAction = false; game.TotalTime = 15;
		game.isShow = false; game.StartTG = false; game.isTalk = false;
		game.anima.enabled = true; game.Yanima.enabled = true; game.Sanima.enabled = true; game.Zanima.enabled = true;
		#endregion
		for (int i = 0; i < BookPicGameSceen.Count; i++)
		{
			BookPicGameSceen[i].gameObject.SetActive(false);
			game.HeadFamer[i].gameObject.SetActive(true);
		}

	}
	public void MorePlayGame()
	{
		//if (!UserId.JieCreateRoom)
		//{
		if (isOverGame)
		{
			OutTableData data = new OutTableData();
			data.actionCode = "OutTableAction";
			data.seatNum = mySeatNum;
			string outData = JsonMapper.ToJson(data);
			WebSoketCall.One().SendToWeb(outData);
			return;
		}
		//}
		CleanData();
		PaiInit();
		OtherPaiShow.text = "100";
		WebSocketInfo wscb = new WebSocketInfo();
		wscb.tableNum = tableNum;
		wscb.actionCode = "ReadyAction";
		wscb.Params = new WebSocketInfo.data();
		string json = JsonMapper.ToJson(wscb);
		WebSoketCall.One().SendToWeb(json);
		gameObject.SetActive(false);
		transform.Find("Bg/WinLight").DOKill();
		Game_Controller game = GameObject.Find("Gold_Game").GetComponent<Game_Controller>();
		game.NoGameStart = true;
		game.Initle();
	}
	public void Back()
	{
		gameObject.SetActive(false);
		CleanData();
		transform.Find("Bg/WinLight").DOKill();
		OutTableData data = new OutTableData();
		data.actionCode = "OutTableAction";
		data.seatNum = mySeatNum;
		string outData = JsonMapper.ToJson(data);
		WebSoketCall.One().SendToWeb(outData);
	}
	public void ChangeTable()
	{
		if (IsPullOk != false)
		{	
			return;
		}
		IsPullOk = true;
		Invoke("TimePull",3);
		if (!UserId.JieCreateRoom) {
			if (isOverGame)
			{
				OutTableData data1 = new OutTableData();
				data1.actionCode = "OutTableAction";
				data1.seatNum = mySeatNum;
				string out1 = JsonMapper.ToJson(data1);
				WebSoketCall.One().SendToWeb(out1);
				return;
			}
		}
		if (UserId.JieCreateRoom)
		{
			Prefabs.PopBubble("您正在他人或自己创建房间内不能换房");
			return;
		}
		Game_Controller game = GameObject.Find("Gold_Game").GetComponent<Game_Controller>();
		CleanData();
		PaiInit();
		//TO
		UserId.ChangeTable = true;
		//出牌桌的指令
		OutTableData data = new OutTableData();
		data.actionCode = "OutTableAction";
		data.seatNum = mySeatNum;
		string outData = JsonMapper.ToJson(data);
		WebSoketCall.One().SendToWeb(outData);
		//Invoke("EnterTableAfterOut", 0.5f);
	}
	public void EnterTableAfterOut()
	{//进入牌桌的指令
		WebSocketInfo info = new WebSocketInfo();
		info.tableNum = -1;
		info.actionCode = "EnterTableAction";
		info.Params = new WebSocketInfo.data();
		info.Params.code = -1;
		info.Params.type = UserId.GameType;
		string s = JsonMapper.ToJson(info);
		WebSoketCall.One().SendToWeb(s);
	}
	public void TimePull()
	{
		IsPullOk = false;
	}
	public void ReallyStartGame(Transform parent)
	{
		Transform DW = parent.GetChild(0);
		RectTransform VDW = DW as RectTransform;
		int a = parent.childCount - 13;
		for (int i = 0; i < a; i++)
		{
			if (parent.GetChild(i).childCount >2) { Destroy(parent.GetChild(i).GetChild(2).gameObject); }
			Destroy(parent.GetChild(parent.childCount - 1 - i).gameObject);
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			if (i == 0)
			{
				parent.GetChild(i).name = "Frist";
			}
			else if (i==12)
			{
				parent.GetChild(i).name = "GameObject (12)";
			} else{ parent.GetChild(i).name = "GameObject" + " " + "(" + i + ")"; }	
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			parent.GetChild(i).GetChild(0).gameObject.SetActive(true);
			parent.GetChild(i).GetChild(1).gameObject.SetActive(false);
			parent.GetChild(i).gameObject.SetActive(true);
			(parent.GetChild(i) as RectTransform).anchoredPosition = VDW.anchoredPosition;
		}
		GameYou.gameObject.SetActive(false);
		GameZuo.gameObject.SetActive(false);
		Gameshang.gameObject.SetActive(false);
	}
	public void ReallyStartGameZY(Transform parent)
	{
		Transform DW = parent.GetChild(0);
		RectTransform VDW = DW as RectTransform;
		int a = parent.childCount - 13;
		for (int i = 0; i <a ; i++)
		{
			if (parent.GetChild(i).childCount >2) { Destroy(parent.GetChild(i).GetChild(2).gameObject); }
			Destroy(parent.GetChild(parent.childCount - 1 - i).gameObject);
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			if (i == 0)
			{
				parent.GetChild(i).name = "Frist";
			}
			else if (i == 1) 
			{
				parent.GetChild(i).name = "GameObject";
			}
			else if (i == 12)
			{
				parent.GetChild(i).name = "GameObject (12)";
			}
			else { parent.GetChild(i).name = "GameObject" + " " + "(" + (i-1)+ ")"; }
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			parent.GetChild(i).GetChild(0).gameObject.SetActive(true);
			parent.GetChild(i).GetChild(1).gameObject.SetActive(false);
			parent.GetChild(i).gameObject.SetActive(true);
			(parent.GetChild(i) as RectTransform).anchoredPosition = VDW.anchoredPosition;
		}
		GameYou.gameObject.SetActive(false);
		GameZuo.gameObject.SetActive(false);
		Gameshang.gameObject.SetActive(false);
	}
	public void DeletChildTranform(Transform object_)
	{
		for (int i = object_.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(object_.GetChild(i).gameObject);
		}
	}

}
