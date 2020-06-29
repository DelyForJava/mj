using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PengActionController : MonoBehaviour {
	public Game_Controller Game_;
	public void PengAction(string edata)
	{
		PengActionInfo peng = JsonMapper.ToObject<PengActionInfo>(edata);
		//显示碰牌对象
		PengSkillshow(peng);
		if (peng.seatNum == Game_.seatNum)
			Game_.activePTrusteeship = true;
		else
			Game_.activePTrusteeship = true;
		Destroy(Game_.DaoChuObject);
		//现在轮到对象的显示
		Game_.TurnNum(peng.seatNum);
		//音效
		Audiocontroller.Instance.MajiangManPlayer(62);
		//碰技能的显示
		PengSkillShowCard(peng);
	}
	//碰技能对象的显示
	void PengSkillShowCard(PengActionInfo peng)
	{
		if (peng.seatNum == Game_.seatNum)
		{
			for (int i = 0; i < Game_.Pai.childCount; i++)
			{
				Game_.Pai.GetChild(i).GetChild(2).gameObject.SetActive(false);
			}
			Game_.CallAmByother = true;
			Game_.DarwPai = peng.handCards;
			Game_.MySkillCount = peng.skillCards.Count;
			//生成一张牌来替换因技能牌而少的手牌
			PengCreatedCard(peng);
			//将手牌刷新一遍
			FreshHandCard(peng);
			Game_.isOwn = true;
			Game_.isSkillUse = false;
			//玩家出牌到谁；
			Game_.PlayerNum = Game_.seatNum;
			Game_.SKO_();
		}
		else if (peng.seatNum == Game_.youplaycount)
		{
			OtherPengShow(peng, Game_.youplaycount, Game_.You);
		}
		else if (peng.seatNum == Game_.shangplaycount)
		{
			OtherPengShow(peng, Game_.shangplaycount, Game_.Shang);
		}
		else if(peng.seatNum == Game_.zuoplaycount)
		{
			OtherPengShow(peng, Game_.zuoplaycount, Game_.Zuo);
		}
	}
	//碰后为了防止手牌数不够先生成一张
	public void PengCreatedCard(PengActionInfo peng)
	{
		GameObject go;
		if (peng.handCards.Count== 0)
		{
			go = Game_.BuPai(Game_.list[peng.handCards[Game_.rascalCard]]);
		}
		else
		{
			go = Game_.BuPai(Game_.list[peng.handCards[peng.handCards.Count - 1]]);
		}
			(go.transform as RectTransform).anchoredPosition = (Game_.Pai.GetChild(Game_.Pai.childCount - 2) as RectTransform).anchoredPosition;
		(go.transform as RectTransform).anchoredPosition += new Vector2(108, 0);
	}
	//刷新手牌
	public void FreshHandCard(PengActionInfo peng)
	{
		Game_.RefashCard(peng.handCards, peng.skillCards, Game_.reascalcount);
		Game_.RefreshSkill(peng.skillCards, Game_.Pai);
		//碰的显示位置
		ShowPengSkillPosition(peng);
	}
	//技能展示后位置的变化
	public void ShowPengSkillPosition(PengActionInfo peng)
	{
		for (int i = peng.skillCards.Count - 2; i < peng.skillCards.Count; i++)
		{
			if (i == peng.skillCards.Count - 2)
			{
				(Game_.Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(14, 0);
			}
			else
			{
				(Game_.Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(28, 0);
			}
		}
	}
	//其他玩家的碰显示
	public void OtherPengShow(PengActionInfo peng,int ObjCount,Transform objGame)
	{
		Game_.StartCountDown();
		for (int i = 0; i < peng.skillCards.Count; i++)
		{
			if (peng.seatNum == ObjCount)
			{
				Transform tf = objGame.GetChild(i);
				tf.GetChild(0).gameObject.SetActive(false);
				tf.GetChild(1).gameObject.SetActive(true);
				tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[peng.skillCards[i]];
			}
		}
		Game_.CurrentDuixiang = null;
		Game_.otherChiPeng = true;
		Game_.PlayerNum = ObjCount;
	}
    
	public void PengSkillshow(PengActionInfo peng)
	{
		if (peng.seatNum==Game_.youplaycount)
			StartCoroutine(Game_.SkillTShow("You","peng"));
		else if (peng.seatNum == Game_.zuoplaycount)
			StartCoroutine(Game_.SkillTShow("Zuo", "peng"));
		else if (peng.seatNum == Game_.shangplaycount)
			StartCoroutine(Game_.SkillTShow("Shang", "peng"));
	}
}
