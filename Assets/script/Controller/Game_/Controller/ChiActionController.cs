using LitJson;
using UnityEngine;
using UnityEngine.UI;
public class ChiActionController : MonoBehaviour {

	public Game_Controller Game_;

	public void ChiAction(string edata)
	{
		ChipaiInfo chi = JsonMapper.ToObject<ChipaiInfo>(edata);
		//吃动画
		ChiAnimator(chi);
		Game_.DarwPai = chi.handCards; Game_.MySkillCount = chi.skillCards.Count;
		if (chi.seatNum == Game_.seatNum)
			Game_.activePTrusteeship = true;
		else
			Game_.activePTrusteeship = false;
		Game_.TurnNum(chi.seatNum);
		Destroy(Game_.DaoChuObject);
		Game_.TurnNum(chi.seatNum);
		//吃牌的显示
		ChiSkillShow(chi);
	}
	//Chi牌显示
	public void ChiAnimator(ChipaiInfo chi)
	{
		if (chi.seatNum == Game_.youplaycount)
			StartCoroutine(Game_.SkillTShow("You", "chi"));
		else if (chi.seatNum == Game_.zuoplaycount)
			StartCoroutine(Game_.SkillTShow("Zuo", "chi"));
		else if (chi.seatNum == Game_.shangplaycount)
			StartCoroutine(Game_.SkillTShow("Shang", "chi"));
	}
	void ChiSkillShow(ChipaiInfo chi)
	{
		if (chi.seatNum == Game_.seatNum)
		{
			Game_.CallAmByother = true;;
			//生成一个对象
			ChiCreateCard(chi);
			//吃后位置的变换
			ReFashHangCard(chi);
			//玩家出牌到谁；
			Game_.SKO_();
			Game_.PlayerNum = Game_.seatNum;
			Game_.isOwn = true;
			Game_.isSkillUse = false;
		}
		else if(chi.seatNum == Game_.youplaycount)
		{
			OtherChiSkillCardShow(chi, Game_.youplaycount, Game_.You);
		}
		else if (chi.seatNum == Game_.shangplaycount)
		{
			OtherChiSkillCardShow(chi, Game_.shangplaycount, Game_.Shang);
		}
		else if (chi.seatNum == Game_.zuoplaycount)
		{
			OtherChiSkillCardShow(chi, Game_.zuoplaycount, Game_.Zuo);
		}
	}
	//生成为吃生成一张牌来补替
	void ChiCreateCard(ChipaiInfo chi)
	{
		GameObject go;
		if (chi.handCards.Count==0)
		{
			go = Game_.BuPai(Game_.list[Game_.rascalCard]);
		}
		else
		{
			go = Game_.BuPai(Game_.list[chi.handCards[chi.handCards.Count - 1]]);
		}
		(go.transform as RectTransform).anchoredPosition = (Game_.Pai.GetChild(Game_.Pai.childCount - 2).transform as RectTransform).anchoredPosition;
		(go.transform as RectTransform).anchoredPosition += new Vector2(108, 0);
	}
	//刷新手牌
	void ReFashHangCard(ChipaiInfo chi)
	{
		Game_.RefashCard(chi.handCards, chi.skillCards, Game_.reascalcount);
		Game_.RefreshSkill(chi.skillCards, Game_.Pai);
		ShowChiPostive(chi);
	}
	//变换位置
	void ShowChiPostive(ChipaiInfo chi)
	{
		for (int i = chi.skillCards.Count - 2; i < chi.skillCards.Count; i++)
		{
			if (i == chi.skillCards.Count - 2)
			{
				(Game_.Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(14, 0);
			}
			else { (Game_.Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(28, 0); }
		}
	}
	//其他玩家吃技能的显示
	void OtherChiSkillCardShow(ChipaiInfo chi,int GameSeatNumCount, Transform ObjGame)
	{
		Game_.StartCountDown();
		for (int i = 0; i < chi.skillCards.Count; i++)
		{
			Transform tf =ObjGame.GetChild(i);
			tf.GetChild(0).gameObject.SetActive(false);
			tf.GetChild(1).gameObject.SetActive(true);
			tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[chi.skillCards[i]];
		}
		Game_.PlayerNum = GameSeatNumCount;
		Game_.otherChiPeng = true;
		Game_.CurrentDuixiang = null;
	}
}
