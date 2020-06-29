using DG.Tweening;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class GameMessageControlle : MonoBehaviour {

	public Game_Controller Game_;
	public void GameMessage(string edata)
	{
		GameMessageData game = JsonMapper.ToObject<GameMessageData>(edata);
		if (game.seatNum == Game_.seatNum)
		{
			GameObject go = GameObject.Find("MyExpression");
			if (game.selection == 0)
			{
				Sprite pic = Bridge._instance.LoadAbDateSprite(LoadAb.Face, game.msg);
				go.transform.GetChild(0).GetComponent<Image>().sprite = pic;
				go.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
				go.transform.GetChild(0).gameObject.SetActive(true);
				StartCoroutine(Game_.Expression(go.transform.GetChild(0).gameObject));
			}
			else if (game.selection == 1)
			{
				go.transform.GetChild(1).gameObject.SetActive(true);
				go.transform.GetChild(2).GetComponent<Text>().DOText(game.msg, 0.7f).OnComplete(() => { go.transform.GetChild(2).GetComponent<Text>().text = null; go.transform.GetChild(1).gameObject.SetActive(false); });
			}
		}
		else if (game.seatNum == Game_.youplaycount)
		{
			GameObject go = GameObject.Find("youExpression");
			if (game.selection == 0)
			{
				Sprite pic = Bridge._instance.LoadAbDateSprite(LoadAb.Face, game.msg);
				go.transform.GetChild(0).GetComponent<Image>().sprite = pic;
				go.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
				go.transform.GetChild(0).gameObject.SetActive(true);
				StartCoroutine(Game_.Expression(go.transform.GetChild(0).gameObject));
			}
			else
			{
				go.transform.GetChild(1).gameObject.SetActive(true);
				go.transform.GetChild(2).GetComponent<Text>().DOText(game.msg, 1).OnComplete(() => { go.transform.GetChild(2).GetComponent<Text>().text = null; go.transform.GetChild(1).gameObject.SetActive(false); });
			}
		}
		else if (game.seatNum == Game_.shangplaycount)
		{
			GameObject go = GameObject.Find("shangExpression");
			if (game.selection == 0)
			{
				Sprite pic = Bridge._instance.LoadAbDateSprite(LoadAb.Face, game.msg);
				go.transform.GetChild(0).GetComponent<Image>().sprite = pic;
				go.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
				go.transform.GetChild(0).gameObject.SetActive(true);
				StartCoroutine(Game_.Expression(go.transform.GetChild(0).gameObject));
			}
			else
			{
				go.transform.GetChild(1).gameObject.SetActive(true);
				go.transform.GetChild(2).GetComponent<Text>().DOText(game.msg, 1).OnComplete(() => { go.transform.GetChild(2).GetComponent<Text>().text = null; go.transform.GetChild(1).gameObject.SetActive(false); });
			}
		}
		else if (game.seatNum == Game_.zuoplaycount)
		{
			GameObject go = GameObject.Find("zuoExpression");
			if (game.selection == 0)
			{
				Sprite pic = Bridge._instance.LoadAbDateSprite(LoadAb.Face, game.msg);
				go.transform.GetChild(0).GetComponent<Image>().sprite = pic;
				go.transform.GetChild(0).GetComponent<Image>().SetNativeSize();
				go.transform.GetChild(0).gameObject.SetActive(true);
				StartCoroutine(Game_.Expression(go.transform.GetChild(0).gameObject));
			}
			else
			{
				go.transform.GetChild(1).gameObject.SetActive(true);
				go.transform.GetChild(2).GetComponent<Text>().DOText(game.msg, 1).OnComplete(() => { go.transform.GetChild(2).GetComponent<Text>().text = null; go.transform.GetChild(1).gameObject.SetActive(false); });
			}
		}
	}
}
