using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReallyActionController : MonoBehaviour {

	public Game_Controller Game_;
	public void ReadyAction(string edata)
	{
		ReallyActionData data = JsonMapper.ToObject<ReallyActionData>(edata);
		ViewImage(data);
	}
	public void ViewImage(ReallyActionData data)
	{
		if (Game_.YP.childCount > 0 || Game_.SP.childCount > 0 || Game_.ZP.childCount > 0 || Game_.parent.childCount > 0)
		{
				Game_.jie.CleanData();
		}
		if (Game_.dic[data.memberId] == Game_.seatNum)
		{
			GameObject.Find("ReallyButton").transform.GetChild(0).gameObject.SetActive(false);
			GameObject.Find("turnImage").transform.Find("SelfReally").gameObject.SetActive(true);
			Transform GameChangerTable = GameObject.Find("CreatRoom").transform.Find("HuangZhu (1)");
			(GameChangerTable as RectTransform).anchoredPosition = new Vector2(-954, 255);
		}
		else if (Game_.dic[data.memberId] == Game_.youplaycount)
		{
			GameObject.Find("turnImage").transform.Find("YouReally").gameObject.SetActive(true);
		}
		else if (Game_.dic[data.memberId] == Game_.shangplaycount)
		{
			GameObject.Find("turnImage").transform.Find("ShangReally").gameObject.SetActive(true);
		}
		else if (Game_.dic[data.memberId] == Game_.zuoplaycount)
		{
			GameObject.Find("turnImage").transform.Find("ZuoReally").gameObject.SetActive(true);
		}
	}
}
