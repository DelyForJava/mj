using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangUpController : MonoBehaviour {
	public Game_Controller Game_;
	public void HangUpAction(string edate)
	{
		HangUpActionData hang = JsonMapper.ToObject<HangUpActionData>(edate);
		if (hang.playerStatus <= 0)
		{
			if (hang.seatNum == Game_.seatNum)
			{
				Game_.StartTG = true;
				Transform tg = Game_.skillMap.Find("TuoGuang");
				tg.gameObject.SetActive(true);
				if (Game_.CallAmByother)
				{
					Game_.CallAmByother = false;
				}
				Game_.isOwn = false;
			}
			if (hang.seatNum == Game_.youplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("YouTrusteeship").gameObject.SetActive(true);
			}
			else if (hang.seatNum == Game_.zuoplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("ZuoTrusteeship").gameObject.SetActive(true);
			}
			else if (hang.seatNum == Game_.shangplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("ShangTrusteeship").gameObject.SetActive(true);
			}
		}
		else
		{
			if (hang.seatNum == Game_.seatNum)
			{
				Game_.StartTG = false;
				Transform tg = Game_.skillMap.Find("TuoGuang");
				tg.gameObject.SetActive(false);
				if (Game_.PlayerNum == Game_.seatNum)
				{
					if (!Game_.isSTGPutCard)
					{
						Game_.isOwn = true;
					}
				}
			}
			if (hang.seatNum == Game_.youplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("YouTrusteeship").gameObject.SetActive(false);
			}
			else if (hang.seatNum == Game_.seatNum) { }
			else if (hang.seatNum == Game_.zuoplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("ZuoTrusteeship").gameObject.SetActive(false);
			}
			else if (hang.seatNum == Game_.shangplaycount)
			{
				GameObject.Find("TrusteeshipStatus").transform.Find("ShangTrusteeship").gameObject.SetActive(false);
			}
		}
	}
}
