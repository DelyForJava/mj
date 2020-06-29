using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipController : MonoBehaviour {
	public Image Picmanager;
	public Text Showtip;
	public int TimeDownCount;
	public int TimObj=40;
	// Use this for initialization
	void OnEnable()
	{
		StatWaitPeople();
	}
	public void StatWaitPeople()
	{
		StartCoroutine("StartTimeDown");
	}
	public void StopWaitPeople()
	{
		StopCoroutine("StartTimeDown");
	}
	IEnumerator StartTimeDown()
	{
		Picmanager.fillAmount = 0;
		while (TimeDownCount<40) 
		{
			TimeDownCount++;
			Picmanager.fillAmount+=0.025f;
			Showtip.text = TimeDownCount + "/40";
			yield return new WaitForSeconds(1);		
		}
		yield return null;
		if (TimeDownCount >= 40)
		{
			gameObject.SetActive(false);
		}
	}
	public void QueDing()
	{

	}
	public void QueXiao()
	{
		Destroy(gameObject);
	}
}
