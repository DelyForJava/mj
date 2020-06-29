using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ParameterDataCollerent : MonoBehaviour {
	#region 参数
	[Header("玩家名字")]
	public List<Text> handTuoName = new List<Text>();
	[Header("头像框数据")]
	public List<Image> HeadFamer = new List<Image>();
	[Header("Yuyi")]
	public List<GameObject> ShowChat = new List<GameObject>();
	[Header("骰盘动画")]
	public List<Image> TweenP = new List<Image>();
	[Header("手牌暂存")]
	public List<int> DarwPai = new List<int>();
	[Header("所有牌")]
	public Sprite[] PaiSprite;
	[Header("玩家ID数据")]
	public List<int> playerId = new List<int>();
	[Header("玩家信息数据")]
	public List<string> playerInfo = new List<string>();
	[Header("玩家头像从左道右为我右上左")]
	public List<Image> HandPic = new List<Image>();
	[Header("字典key为IdValue为Seatnum")]
	public Dictionary<int, int> dic = new Dictionary<int, int>();
	[Header("玩家出牌信息")]
	public List<string> PlayerActionDataOfLink = new List<string>();
	[Header("玩家摸牌信息")]
	public List<string> DrawActionData = new List<string>();
	[Header("托管信息组")]
	public List<string> HungUpData = new List<string>();
	[Header("进入桌子的数据集")]
	public List<string> EnterTableIEData = new List<string>();
	public List<string> OutTableIEData = new List<string>();
	[Header("右边的牌")]
	public List<int> one = new List<int>();
	[Header("上边的牌")]
	public List<int> twotwo = new List<int>();
	[Header("左边的牌")]
	public List<int> three = new List<int>();
	[Header("准备数据集")]
	public List<string> ReallyInfo = new List<string>();
	[Header("庄家标志")]
	public List<Image> Bookpic = new List<Image>();
	[Header("玩家有碰的id")]
	#region chi牌数组
	//要吃的牌在中间
	[Header("要吃中间的牌")]
	public List<int> chiType_Zhong = new List<int>();
	//要吃的牌在上边
	[Header("要吃上的牌")]
	public List<int> chiType_shang = new List<int>();
	//要吃的牌在下边
	[Header("要吃下边的牌")]
	public List<int> chiType_xia = new List<int>();
	#endregion
	[Header("自己的牌")]
	public Transform Pai;
	//自己出牌的放点
	[Header("出牌放点")]
	public Transform parent;
	[Header("杠的按钮")]
	public Transform GongnengButton;
	[Header("游戏对话")]
	public Transform Talk;
	[Header("当前摸牌的对象")]
	public Transform CurrentDuixiang;
	[Header("位置对应")]
	public Transform weastenTuoZi;
	[Header("吃牌技能显示对象")]
	public Transform pic;
	//开始游戏需要打开
	public Transform[] maJiangpai;
	[Header("结算的物体")]
	public Transform JieTrabsform;
	[Header("左牌、右牌、上牌")]
	public Transform Zuo, You, Shang;
	[Header("左牌、右牌、上牌的父物体")]
	public Transform ZP, YP, SP;
	[Header("显示技能地")]
	public Transform skillMap;
	[Header("断线重连动画")]
	public Transform ConnectAnimator;
	[Header("打出牌对象")]
	public GameObject DaoChuObject;
	[Header("游戏玩法对象")]
	public GameObject WanFai;
	[Header("牌动画_M")]
	public Animator anima;
	public Animator Yanima;
	public Animator Zanima;
	public Animator Sanima;
	public JieSuangController jie;
	public int OutTabeCount;
	[Header("庄家序号")]
	public int Bookmaker;
	[Header("用来记录G3情况下放的位置")]
	public int G3Position;
	//房间桌子号
	public int tableNum;
	[Header("杠的牌")]
	public int GrabCards;
	[Header("玩家当前执行的对象")]
	public int CurrentZXCount;
	[Header("当前摸牌次数")]
	public int CurrentDrawCount = 0;
	[Header("自己座位号")]
	public int seatNum;
	[Header("当前机器人状况")]
	public int robPepleCount;
	[Header("当前打出的牌")]
	public int CurrentPlayerCard;
	[Header("创建房间的轮数")]
	public int Createround;
	[Header("自己技能的数量")]
	public int MySkillCount;
	//癞子牌
	public int rascalCard;
	public int robCount;
	//左边座位号
	public int zuoplaycount;
	//右边的座位号
	public int youplaycount;
	//上边的座位号
	public int shangplaycount;
	[Header("托管人数")]
	public int TGCount;
	//当前摸牌对象；
	[Header("当前摸牌对象")]
	public int CurrentObject;
	int chuCard;
	//player现在对象
	[Header("Player现在的对象")]
	public int PlayerNum;
	//自己拥有的癞子数；
	public int reascalcount;
	[Header("进入房间存下的局数")]
	public int Rount_;
	//胡牌的卡
	[Header("胡的牌")]
	public int HuPaiCard;
	[Header("倒计时秒数")]
	public int TotalTime = 15;
	[Header("摸牌阶段摸到的牌")]
	public int DrawCPai;
	public int SHowTableCount;
	[Header("错误信息")]
	public string errorData;
	[Header("托管数据")]
	public string HangUpData;
	[Header("信息数据")]
	public string MessageData;
	[Header("胡数据")]
	public string HuDataInfo;
	[Header("全部准备的信息")]
	public string AllReallyActionData;
	[Header("断线重连数据")]
	public string ReConnectionData;
	[Header("结束数据")]
	public string EndData;
	[Header("是否出桌子")]
	public bool isOutTable;
	[Header("是否是断线重连")]
	public bool isReconnection;
	[Header("开始游戏")]
	public bool isGameStart;
	public bool isPass;
	public bool isChiButtonOn;
	[Header("是否全部准备好了")]
	public bool isAllRellyAction;
	[Header("是否是吃碰要准备的事")]
	public bool CallAmByother;
	[Header("rob>3时用于判断")]
	public bool RobDrawCardok;
	[Header("huActionISok")]
	public bool IsHuAction;
	bool cChi;
	public bool isSTGPutCard;
	public bool isPlayerIng;
	[Header("EndConnectIS")]
	public bool EndConnectIs;
	//是否是自己出牌
	public bool isOwn;
	[Header("G3位置是否使用")]
	public bool G3bool;
	public bool ReconncctShow;
	public bool GameGetLiang;
	public bool StatrFristPai;
	public bool isOverOutTableIE;
	public bool isSkillUse;
	[Header("是否执行完玩家数据")]
	public bool isOverPlayerAction;
	[Header("最后一局是否开始")]
	public bool isEndAction;
	[Header("游戏已经开局")]
	public bool SureGame;
	[Header("是否是碰牌后有杠情况")]
	public bool GangT3;
	[Header("右是否是碰牌后有杠情况")]
	public bool YouGangT3;
	[Header("是否是要接收错误信息")]
	public bool errorBool;
	[Header("左是否是碰牌后有杠情况")]
	public bool ZuoGangT3;
	[Header("上是否是碰牌后有杠情况")]
	public bool ShangGangT3;
	[Header("是否是创建房间")]
	public bool isCreateRoom;
	[Header("是否进入托管")]
	public bool isHungUp;
	[Header("是否出牌携程执行完毕")]
	public bool isOverStartPlayer = true;
	[Header("是否摸牌携程执行完毕")]
	public bool isOverStartDraw = true;
	[Header("是否是断线重连")]
	public bool isReConnection;
	//是否发送表情
	public bool isSendMessage;
	[Header("是否是执行出牌回调")]
	public bool isPlayerAction;
	[Header("是否是摸牌回调")]
	public bool isDrawAction;
	[Header("是否进入主动进入托管")]
	public bool activePTrusteeship;
	[Header("托管携程是否完成")]
	public bool isHangUpOver;
	[Header("是否进入桌子")]
	public bool isEnterTable;
	[Header("是否进入桌子数据处理完")]
	public bool isOverEnterTable;
	[Header("是否是准备")]
	public bool isReallyShow;
	[Header("准备数据处理是否完成")]
	public bool isReallyShowOver;
	[Header("是否是加入房间")]
	public bool isJoinRoom;
	public bool NoGameStart;
	#region 杠的判定
	[Header("自己是否是明杠")]
	public bool isMingGang;
	public bool isYouMingGang;
	public bool isShangMingGang;
	public bool isZuoMingGang;
	public bool isMyGang;
	public bool isHu;
	public bool OverDarw;
	#endregion
	//碰添加过了点击事件吗？
	public bool isPengAddlist;
	//碰添加过了点击事件吗？
	public bool isChiAddlist;
	//碰添加过了点击事件吗？
	public bool isGangAddlist;
	public bool isHuAddlist;
	[Header("是否是其他玩家回合")]
	public bool isOther;
	[Header("是否是其他人吃碰")]
	bool otherChiPeng;
	[Header("是否是谈话对象显示")]
	public bool isTalk;
	[Header("功能开关是否打开")]
	public bool isShow;
	[Header("是否进入托管")]
	public bool StartTG;
	[Header("是否需要断线重连")]
	public bool ConnectAnimatorLine;

	[Header("创建房间桌子的显示")]
	public Text TableNumShow;
	//牌的数量
	[Header("牌的数量显示")]
	public Text OtherPaiShow;
	[Header("当前轮数")]
	public Text CurrentRound;
	[Header("倒计时显示对象")]
	public Text TimeText;
	[Header("显示的轮数")]
	public Text ShowTable;
	//癞子的图片
	public Image reascalPic;
	public Sprite newCord;
	[Header("邀请好友")]
	public Button YQHY;
	public CreateLumSum CLS;
	public TipController WaitTipPeople;
	public PlayerBack playerData = new PlayerBack();

	//房间是否可以托管
	public bool IsAutoPut = true;

	#endregion
}
