using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorActionController : MonoBehaviour {

	public Game_Controller Game_;
	public void ErrorAction(string edate)
	{
		ErrorDataMessage er = JsonMapper.ToObject<ErrorDataMessage>(edate);
		Debug.Log(er.msg);
		if (er.msg == "房间不存在！")
		{
			GameMapManager.Instance.NormalLoadScene("liang");
			UserId.GameState = false;
		}
		if (er.msg == "房主解散牌局，已做流局处理！")
		{
			Prefabs.PopBubble(er.msg);
			UserId.JieCreateRoom = false;
			UserId.isCreateRoom = false;
			UserId.isJoinRoom = false;
			Invoke("OutTable", 2);
		}
		if (er.msg == "玩家账号在别处登录！")
		{
			//StopCoroutine("DetecConnection");
			StopAllCoroutines();
			LoadLineWS.One().StopDes();
			WebSoketCall.One().isLinkWS = false;
			WebSoketCall.One().isGame = false;
			PlayerPrefs.SetString("UserId.token", "");
			LoadManager.Instance.LoadScene("loadsceen", Offline, 1);
		}
		if (er.msg == "异常！")
		{
			Prefabs.PopBubble("异常！");
			GameMapManager.Instance.NormalLoadScene("liang");
			UserId.GameState = false;
			//LoadManager.Instance.LoadScene("liang");
		}
		if (er.msg == "群主已解散牌局！")
		{
			Prefabs.PopBubble(er.msg);
			Invoke("OutTable", 2);
			Invoke("LoadLiang", 2.5f);
		}
		if (er.msg == "玩家在游戏中！")
		{
			//GameMapManager.Instance.NormalLoadScene("liang");
			LoadManager.Instance.LoadScene("liang", SceenLoadToError, "玩家在游戏中！");
		}
		if (er.msg == "已是准备状态或不在牌局中！")
		{
			Debug.Log(":" + Game_.tableNum);
		}
		if (er.msg == "房间不存在！")
		{
			Debug.Log("+++房间不存在+++");
			Prefabs.PopBubble("+++房间不存在+++");
			UserId.GameState = false;
			GameMapManager.Instance.NormalLoadScene("liang");
		}
	}
	//713
	public void Offline(int a)
	{
		Prefabs.PopBubble("玩家账号在别处登录！");
		WebSoketCall.One().FristLink = false;
		WebSoketCall.One().isGame = false;
		WebSoketCall.One().ws.Close();
	}
	//704
	void SceenLoadToError(string edate)
	{
		Prefabs.PopBubble(edate);
	}
}
