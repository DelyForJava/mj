using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBackJie : MonoBehaviour {

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
	#region 手牌信息
	public List<int> MyPaiCard;
	public List<int> YouPaiCard;
	public List<int> ZuoPaiCard;
	public List<int> ShangPaiCard;
	#endregion

	//连接场景破产
	public bool isOverGame;
	PlayerBackGame gc;
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

	void OnEnable()
	{
		//PaiInit();
		//开始结算时让牌数恢复到13张
		//for (int i = 0; i < HuDesAim.Count; i++)
		//{
		//	HuDesAim[i].text = null;
		//}
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
			if (object_ == Pai)
			{
				GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/MypaiInfo/huPai").gameObject;
				(go.transform as RectTransform).anchoredPosition -= new Vector2(60 * a, 0);
			}
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
			if (i + 1 == mySeatNum)
			{
				number[0].text = "<color=#bab6b0><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>";
				if (goldChangeList[i] > 0)
				{
					if (goldChangeList[i] > 0) { number[0].text = "<color=#f5dd69><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>"; }
				}
			}
			else if (i + 1 == youSeatNum)
			{
				number[1].text = "<color=#bab6b0><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>";
				if (goldChangeList[i] > 0)
				{
					if (goldChangeList[i] > 0) { number[1].text = "<color=#f5dd69><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>"; }
				}
			}
			else if (i + 1 == shangSeatNum)
			{
				number[2].text = "<color=#bab6b0><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>";
				if (goldChangeList[i] > 0)
				{
					if (goldChangeList[i] > 0) { number[2].text = "<color=#f5dd69><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>"; }
				}
			}
			else if (i + 1 == zuoSeatNum)
			{
				number[3].text = "<color=#bab6b0><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>";
				if (goldChangeList[i] > 0)
				{
					if (goldChangeList[i] > 0) { number[3].text = "<color=#f5dd69><size=84>" + /*(*/goldChangeList[i] /*+ gangPointsList[i])*/.ToString() + "</size></color>"; }
				}
			}
			BookPic[i].gameObject.SetActive(false);
			NoMoney[i].gameObject.SetActive(false);
			HuDesAim[i].text = null;
		}

		 gc = GameObject.Find("Gold_Game").GetComponent<PlayerBackGame>();
		Bookmaker = gc.Bookmaker;
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
				if (UidASeatNum[playerMessage.data.id] == mySeatNum)
				{
					NameList[0].text = playerMessage.data.nickname;
					if (playerMessage.data.gold - goldChangeList[mySeatNum - 1] < 0)
					{
						isOverGame = true;
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
				else if (UidASeatNum[playerMessage.data.id] == youSeatNum)
				{
					JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, playerHeads[1]);
					if (playerMessage.data.gold - goldChangeList[youSeatNum - 1] < 0)
					{
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
				else if (UidASeatNum[playerMessage.data.id] == shangSeatNum)
				{
					JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, playerHeads[2]);
					NameList[2].text = playerMessage.data.nickname;
					if (playerMessage.data.gold - goldChangeList[shangSeatNum - 1] < 0)
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
				else if (UidASeatNum[playerMessage.data.id] == zuoSeatNum)
				{
					JX.HttpCallSever.One().DownPic(playerMessage.data.avatar, playerHeads[3]);
					NameList[3].text = playerMessage.data.nickname;
					HeadFamer[3].sprite = Resources.Load<Sprite>("shop" + "/" + "head_" + playerMessage.data.headFrame);
					if (playerMessage.data.gold - goldChangeList[zuoSeatNum - 1] < 0)
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
			transform.Find("Bg/BackGroud").GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "DefultGroup");
			Transform icon1 = transform.Find("Bg/icon");
			icon1.Find("icon_Defult").gameObject.SetActive(false);
			icon1.Find("icon_Win").gameObject.SetActive(false);
			icon1.Find("icon_place").gameObject.SetActive(true);
			ObjectCard(Pai, GamePai, "majiang#Pai");
			Debug.Log(You.childCount);Debug.Log(GameYou.childCount);
			OtherCard(You, GameYou, "majiang#Pai");
			Debug.Log(Zuo.childCount); Debug.Log(GameZuo.childCount);
			OtherCard(Zuo, GameZuo, "majiang#Pai");
			Debug.Log(shang.childCount); Debug.Log(Gameshang.childCount);
			OtherCard(shang, Gameshang, "majiang#Pai");
			return;
		}

		#region 手牌显示
		if (HuTheme == mySeatNum)
		{
			#region icon
			transform.Find("Bg/BackGroud").GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "Win");
			Transform icon = transform.Find("Bg/icon");
			icon.Find("icon_Defult").gameObject.SetActive(false);
			icon.Find("icon_Win").gameObject.SetActive(true);
			icon.Find("icon_place").gameObject.SetActive(false);
			#endregion

			#region 将胡牌显示出来
			GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/MypaiInfo/huPai").gameObject;
			transform.Find("Bg/Scroll View/Viewport/Content/YouInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/ShangInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/ZuoInfo/huPai").gameObject.SetActive(false);
			HuDesAim[0].text = HuDescrite;
			go.SetActive(true);
			if (HuCard==0)
			{
				HuCard = rascalCard;
			}
			go.transform.GetChild(0).GetComponent<Image>().sprite = listCard[HuCard];
			#endregion

			#region 除去胡的牌
			//if (HuCard == rascalCard)
			//{
				go.SetActive(false);
			//}
			//else
			//{
			//	MyPaiCard.Remove(HuCard);
			//}
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

			Debug.Log("Pai" + Pai.childCount);
			Debug.Log("MyPaiCard" + MyPaiCard.Count);
			#region 将牌换掉
			ChangCard(Pai, MyPaiCard);
			ChangCard(You, YouPaiCard);
			ChangCard(shang, ShangPaiCard);
			ChangCard(Zuo, ZuoPaiCard);
			#endregion
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
			go.SetActive(true);
			go.transform.GetChild(0).GetComponent<Image>().sprite = listCard[HuCard];
			#endregion

			#region icon
			transform.Find("Bg/BackGroud").GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "lost");
			Transform icon = transform.Find("Bg/icon");
			icon.Find("icon_Defult").gameObject.SetActive(true);
			icon.Find("icon_Win").gameObject.SetActive(false);
			icon.Find("icon_place").gameObject.SetActive(false);
			#endregion

			//YouPaiCard.Remove(HuCard);
			go.SetActive(false);
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
						gg.transform.SetSiblingIndex(Pai.childCount - 1);
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
						gg.transform.SetSiblingIndex(You.childCount - 1);
					}
					(go.transform as RectTransform).anchoredPosition += new Vector2(120, 0);
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
						gg1.transform.SetSiblingIndex(Zuo.childCount - 1);
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
				}
			}

			#endregion

			ChangCard(Pai, MyPaiCard);
			ChangCard(You, YouPaiCard);
			ChangCard(shang, ShangPaiCard);
			ChangCard(Zuo, ZuoPaiCard);
			ChiIII();
		}
		else if (HuTheme == shangSeatNum)
		{
			GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/ShangInfo/huPai").gameObject;
			transform.Find("Bg/Scroll View/Viewport/Content/MypaiInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/YouInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/ZuoInfo/huPai").gameObject.SetActive(false);
			HuDesAim[2].text = HuDescrite;
			go.SetActive(true);
			go.transform.GetChild(0).GetComponent<Image>().sprite = listCard[HuCard];

			#region icon
			transform.Find("Bg/BackGroud").GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "lost");
			Transform icon = transform.Find("Bg/icon");
			icon.Find("icon_Defult").gameObject.SetActive(true);
			icon.Find("icon_Win").gameObject.SetActive(false);
			icon.Find("icon_place").gameObject.SetActive(false);
			#endregion

			go.SetActive(false);
			//ShangPaiCard.Remove(HuCard);

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
						gg.transform.SetSiblingIndex(Pai.childCount - 1);
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
						gg.transform.SetSiblingIndex(You.childCount - 1);
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
						gg1.transform.SetSiblingIndex(Zuo.childCount - 1);
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
					(go.transform as RectTransform).anchoredPosition += new Vector2(120, 0);
				}
				
			}
			#endregion

			ChangCard(Pai, MyPaiCard);
			ChangCard(You, YouPaiCard);
			ChangCard(shang, ShangPaiCard);
			ChangCard(Zuo, ZuoPaiCard);
			ChiIII();
		}
		else if (HuTheme == zuoSeatNum)
		{
			GameObject go = transform.Find("Bg/Scroll View/Viewport/Content/ZuoInfo/huPai").gameObject;
			transform.Find("Bg/Scroll View/Viewport/Content/MypaiInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/ShangInfo/huPai").gameObject.SetActive(false);
			transform.Find("Bg/Scroll View/Viewport/Content/YouInfo/huPai").gameObject.SetActive(false);
			HuDesAim[3].text = HuDescrite;
			go.SetActive(true);
			go.transform.GetChild(0).GetComponent<Image>().sprite = listCard[HuCard];

			#region icon
			transform.Find("Bg/BackGroud").GetComponent<Image>().sprite = Bridge._instance.LoadAbDateSprite(LoadAb.Pic, "lost");
			Transform icon = transform.Find("Bg/icon");
			icon.Find("icon_Defult").gameObject.SetActive(true);
			icon.Find("icon_Win").gameObject.SetActive(false);
			icon.Find("icon_place").gameObject.SetActive(false);
			#endregion
			go.SetActive(false);
			//ZuoPaiCard.Remove(HuCard);

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
						gg.transform.SetSiblingIndex(Pai.childCount - 1);
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
						gg.transform.SetSiblingIndex(You.childCount - 1);
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
						gg1.transform.SetSiblingIndex(Zuo.childCount - 1);
					}
					(go.transform as RectTransform).anchoredPosition += new Vector2(120, 0);
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

				}
				
			}
			#endregion

			ChangCard(Pai, MyPaiCard);
			ChangCard(You, YouPaiCard);
			ChangCard(shang, ShangPaiCard);
			ChangCard(Zuo, ZuoPaiCard);
			ChiIII();
		}
		#endregion
		CheckPoint(GameYou,YouPaiCard);
		CheckPoint(Gameshang,ShangPaiCard);
		CheckPoint(GameZuo,ZuoPaiCard);
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
			JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() +"/api/member/getinfo", json, PlayerInfo);
		}
	}
	public void CheckPoint(Transform Obj,List<int> CardList)
	{
		if (Obj.childCount>CardList.Count)
		{
			int Obj_ = Obj.childCount - CardList.Count;
			for (int i = 0; i < Obj_; i++)
			{
				Destroy(Obj.GetChild(Obj.childCount-1-i).gameObject);
			}
		}
	}
	public void PlayerInfo(string data)
	{
		PlayerMessageInfo playerMessage = JsonMapper.ToObject<PlayerMessageInfo>(data);
		Debug.Log(playerMessage.data.gold + goldChangeList[mySeatNum - 1]);
		if (mySeatNum == playerMessage.data.id)
		{
			UserId.goldCount = playerMessage.data.gold;
			if (playerMessage.data.gold< 0)
			{
				isOverGame = true;
				NoMoney[0].gameObject.SetActive(true);
			}
			else { isOverGame = false; }
		}
		else if (youSeatNum == playerMessage.data.id)
		{
			if (playerMessage.data.gold < 0)
			{
				NoMoney[1].gameObject.SetActive(true);
			}
			else { }
		}
		else if (shangSeatNum == playerMessage.data.id)
		{
			if (playerMessage.data.gold < 0)
			{
				NoMoney[2].gameObject.SetActive(true);
			}
			else { }
		}
		else if (zuoSeatNum == playerMessage.data.id)
		{
			if (playerMessage.data.gold< 0)
			{
				NoMoney[3].gameObject.SetActive(true);
			}
			else { }
		}
	}
	public void ObjectCard(Transform pai, Transform GameO, string Res)
	{
		for (int i = 0; i < pai.childCount; i++)
		{
			pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite = GameO.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
		}
	}
	public void ChiIII()
	{
		Check(Pai);
		Check(You);
		Check(Zuo);
		Check(shang);
	}
	public void Check(Transform obj)
	{
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
	public void OtherCard(Transform pai, Transform Game, string Res)
	{
		if (pai.childCount!=Game.childCount)
		{
			GameObject gg = Bridge._instance.LoadAbDate(LoadAb.Game, "Pai", pai);
			RectTransform r = gg.transform as RectTransform;
			r.anchoredPosition = (pai.GetChild(pai.childCount - 2) as RectTransform).anchoredPosition;
			r.anchoredPosition += new Vector2(60, 0);
			gg.transform.SetSiblingIndex(pai.childCount - 1);
		}
		if (isFlowBurea == true)
		{
			isFlowBurea = false;
			
			for (int i = 0; i < pai.childCount; i++)
			{
				if (Game.GetChild(i).childCount>=2)
				{
					pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Game.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite;
				}
				else
				{
					pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Game.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite;
				}
				
			}
			return;
		}
		for (int i = 0; i < pai.childCount; i++)
		{
			if (Game.GetChild(i).childCount >= 2)
			{
				pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Game.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>().sprite;
			}
			else
			{
				pai.GetChild(i).GetChild(0).GetComponent<Image>().sprite = Game.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite;
			}
		}
	}
	public void ChangCard(Transform parent, List<int> handCard)
	{
		if (parent.childCount != handCard.Count)
		{
			Debug.Log("结算界面数据有问题");
		}
		ContrastPai(parent.childCount,handCard.Count,parent);
		for (int i = 0; i < parent.childCount; i++)
		{
			parent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = listCard[handCard[i]];
		}
	}
	public void CleanData()
	{
		RectTransform frist = GamePai.GetChild(0) as RectTransform;
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
			(GamePai.GetChild(i) as RectTransform).anchoredPosition = frist.anchoredPosition;
		}
		for (int i = 0; i < GamePai.childCount; i++)
		{
			(GamePai.GetChild(i) as RectTransform).anchoredPosition += new Vector2(105*i, 0);
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
		PlayerBackGame game = GameObject.Find("Gold_Game").GetComponent<PlayerBackGame>();
		game.HungUpData.Clear();
		game.StatrFristPai = true;
		game.reascalPic.transform.parent.gameObject.SetActive(false);
		game.OtherPaiShow.transform.parent.gameObject.SetActive(false);
		game.reascalPic.transform.parent.gameObject.SetActive(true);
		game.OtherPaiShow.transform.parent.gameObject.SetActive(true);
		game.playerInfo.Clear();
		game.isGameStart = false; game.isOwn = false; game.isSkillUse = false;
		 game.activePTrusteeship = false;
		game.HangUpData = null;  game.one.Clear();
		game.twotwo.Clear(); game.three.Clear();
		game.isMingGang = false; game.isYouMingGang = false; game.isShangMingGang = false; game.isZuoMingGang = false;
		game.OverDarw = false; game.CurrentDrawCount = 0; game.TotalTime = 15;
		#endregion
		for (int i = 0; i < BookPicGameSceen.Count; i++)
		{
			BookPicGameSceen[i].gameObject.SetActive(false);
			game.HeadFamer[i].gameObject.SetActive(true);
		}

	}
	public void ReallyStartGame(Transform parent)
	{
		RectTransform rect = parent.GetChild(0) as RectTransform;
		int a = parent.childCount - 13;
		for (int i = 0; i < a; i++)
		{
			Destroy(parent.GetChild(parent.childCount - 1 - i).gameObject);
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			(parent.GetChild(i) as RectTransform).anchoredPosition = rect.anchoredPosition;
			parent.GetChild(i).gameObject.SetActive(true);
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			(parent.GetChild(i) as RectTransform).anchoredPosition += new Vector2(67*i,0);
		}
	}
	public void ReallyStartGameZY(Transform parent)
	{
		RectTransform rect = parent.GetChild(0) as RectTransform;
		int a = parent.childCount - 13;
		for (int i = 0; i < a; i++)
		{
			Destroy(parent.GetChild(parent.childCount - 1 - i).gameObject);
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			(parent.GetChild(i) as RectTransform).anchoredPosition = rect.anchoredPosition;
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			(parent.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(0,47*i);
		}
	}
	public void DeletChildTranform(Transform object_)
	{
		for (int i = object_.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(object_.GetChild(i).gameObject);
		}
	}
	public void ContrastPai(int PaiCard, int DataCard, Transform obj)
	{
	
	 if (obj == Pai)
		{
			Vector2 distance = new Vector2((gc.Pai.GetChild(Pai.childCount - 1) as RectTransform).anchoredPosition.x, 0);
			Vector2 distance1 = new Vector2((gc.Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition.x, 0);
			float data = Vector2.Distance(distance, distance1);
			if (data > 109)
			{
				Debug.Log(data);
				(gc.Pai.GetChild(Pai.childCount - 1) as RectTransform).anchoredPosition = (gc.Pai.GetChild(Pai.childCount - 2) as RectTransform).anchoredPosition;
				(gc.Pai.GetChild(Pai.childCount - 1) as RectTransform).anchoredPosition += new Vector2(108, 0);
			}
		}
		if (PaiCard != DataCard)
		{
			int num = PaiCard - DataCard;

			if (num > 0)
			{
				if (num > 5)
				{
					Debug.LogError("Error");
					return;
				}
				for (int i = 0; i < num; i++)
				{
					DestroyImmediate(obj.GetChild(obj.childCount - 1 - i).gameObject);
				}
			}
			else if (num < 0)
			{
				int num1 = DataCard - PaiCard;
				if (obj == Pai)
				{
					for (int i = 0; i < num1; i++)
					{
						GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "PaiRes_My", gc.Pai);
						#region 点击事件的添加
						go.name = "XP";
						Transform childT = go.transform.GetChild(0);
						if (gc.DrawCPai != 0)
						{
							childT.GetComponent<Image>().sprite = listCard[gc.DrawCPai];
						}
						//添加点击事件	
						#endregion
					}
				}
				if (obj == gc.Shang)
				{
					Debug.Log("ShangAdd++");
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "ShangRes", gc.Shang);
					(go.transform as RectTransform).anchoredPosition = (gc.Shang.GetChild(gc.Shang.childCount - 2) as RectTransform).anchoredPosition;
					(go.transform as RectTransform).anchoredPosition += new Vector2(68, 0);
					go.transform.SetAsLastSibling();
				}
				if (obj == Zuo)
				{
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "ZuoRes", gc.Zuo);
					(go.transform as RectTransform).anchoredPosition = (gc.Zuo.GetChild(Zuo.childCount - 2) as RectTransform).anchoredPosition;
					(go.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);
					go.transform.SetAsLastSibling();
				}
				if (obj == You)
				{
					GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "YouRes", gc.You);
					(go.transform as RectTransform).anchoredPosition = (You.GetChild(You.childCount - 2) as RectTransform).anchoredPosition;
					(go.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);
					go.transform.SetAsLastSibling();
				}
			}
		}
	}
	public void PlayBack()
	{
		CleanData();
		gameObject.SetActive(false);
		PlayerBackGame gc = GameObject.Find("Gold_Game").GetComponent<PlayerBackGame>();
		gc.PlayerBack();
	}
	public void OutPlayBack()
	{
		GameMapManager.Instance.NormalLoadScene("liang");
	}
}
