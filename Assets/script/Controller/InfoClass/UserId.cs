using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JX;
using System;
using UnityEngine.UI;
using LitJson;

public  class UserId  {
	public static string token;    
	public static string name;
    public static string avatar;
	public static string WeChatData;
	public static string GetData;
	public static string NeedWXCallBackData;
	public static string phone;
	public static string yzm;
	public static double dianment;
    public static int memberId;
	public static int goldCount;
    public static int QianDaoCount;
    public static int IsIdTrue;
	public static int sex;
	public static int PictureFrame;
	public static int GameType;
	public static int TableNum;
	public static bool GameState;
	
	public static bool IsQianDaoToDay;
	public static bool isJoinRoom;
	public static bool isCreateRoom;
	public static bool isGame;
	public static bool JieCreateRoom;
	public static bool ChangeTable;
	public static bool GetEntableAction;
	public static bool LoadWSEnterTable;
	public static string  CombatPlayback;
	public static string QiHuiPhone;
	//个人邮件
	public static List<MaillData> MailList = new List<MaillData>();

	public static List<string> EnterWSData=new List<string>();
	public static void GetUserId(string jsonPostData,Action<string> callBack)
	{
		string url = "http://"+Bridge.GetHostAndPort()+"/api/member/getinfo";
		HttpCallSever.One().PostCallServer(url, jsonPostData, callBack);
	}
}
