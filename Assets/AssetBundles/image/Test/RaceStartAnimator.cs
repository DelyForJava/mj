using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RaceStartAnimator : MonoBehaviour {
	public Image twinkle;
	public Image GoldTwinkle;
	public List<Image> LevelNum=new List<Image>();
	public List<Image> Point = new List<Image>();
	public Image Move;
	public int count;
	void Start () {
		//Animator(count);
	}
	public void Animator(int Level)
	{
		twinkle.DOFade(0.5f, 1).SetLoops(-1, LoopType.Yoyo);
		GoldTwinkle.DOFade(0.5f, 1).SetLoops(-1, LoopType.Yoyo);
		GoldTwinkle.transform.DORotate(new Vector3(0, 0, 270), 1).SetLoops(-1, LoopType.Yoyo);
		StartCoroutine(MovePos(Level));
	}
	IEnumerator MovePos(int k)
	{
		for (int i = 0; i < k; i++)
		{
			while (true)
			{
				(Move.transform as RectTransform).anchoredPosition += new Vector2(12.5f, 0);
				LevelNum[i].GetComponent<Image>().fillAmount +=0.05f;
				Debug.Log(LevelNum[i].GetComponent<Image>().fillAmount);
				if (LevelNum[i].GetComponent<Image>().fillAmount >= 1)
				{
					Point[i].gameObject.SetActive(false);
					GameObject.Find("Icon_").transform.Find(Point[i].name + "_").gameObject.SetActive(true);
					break;	
				}
			yield return new WaitForSeconds(0.2f);
			}
		}

	}

	public void EximaneOther()
	{
		JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/game/historyEvents", "", PlayerInfo); 
	}
	public void PlayerInfo(string edate)
	{

	}
	// Update is called once per frame
	void Update () {
		
	}
}
