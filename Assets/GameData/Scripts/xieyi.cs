using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class xieyi : MonoBehaviour {

	string url = "http://" + Bridge.GetHostAndPort() + "/api/privacyPolicy";


	void Awake() {

		//是否同意过  存在本地
		string IsXieYi = PlayerPrefs.GetString("xieyi");
		if (!string.IsNullOrEmpty(IsXieYi))
		{
			this.gameObject.SetActive(false);
		}
	}
	void Start () {

		transform.Find("bg/agree").GetComponent<Button>().onClick.AddListener(()=> { agree(); });
		transform.Find("bg/refuse").GetComponent<Button>().onClick.AddListener(()=> { cancel(); });

		transform.Find("cancel/bg/sure").GetComponent<Button>().onClick.AddListener(() => { trueCancel(); });
		transform.Find("cancel/bg/cancel").GetComponent<Button>().onClick.AddListener(() => { falseCancel(); });

		transform.Find("bg/yonghuxie").GetComponent<Button>().onClick.AddListener(()=> { XieYi();});

		transform.Find("bg/yinsizhgce").GetComponent<Button>().onClick.AddListener(() => { ZhenCe(); });

	}

	//协议 用户协议在游戏内有了预制体  所以这里直接调用就行
	void XieYi() {
		Bridge._instance.LoadAbDate(LoadAb.MainTwo, "YongHuXieYi");
	}
	//政策
	void ZhenCe()
	{
		JX.HttpCallSever.One().GetCallSetver(url, ZhenCeCanback);
		//Bridge._instance.LoadAbDate(LoadAb.MainTwo, "YongHuXieYi");
	}

	//获取隐私政策的回调
    void ZhenCeCanback(string str) {

		JsonData js = JsonMapper.ToObject(str);
		if ((int)js["code"]==200)
		{
			transform.Find("zhengceXieYi").gameObject.SetActive(true);
			transform.Find("zhengceXieYi/BG_XieYi/close").GetComponent<Button>().onClick.AddListener(()=> {
				transform.Find("zhengceXieYi").gameObject.SetActive(false);
			});
			transform.Find("zhengceXieYi/BG_XieYi/Scroll View/Content/Text").GetComponent<Text>().text = (string)js["data"];
		}	
	}

	//同意协议
	void agree() {
		PlayerPrefs.SetString("xieyi", "true");
		this.transform.gameObject.SetActive(false);
	}

	//不同意用户协议
	void cancel() {
		transform.Find("cancel").gameObject.SetActive(true);

	}

	//确定推出
	void trueCancel() {
		Application.Quit();
	}

	//取消退出
	void falseCancel() {
		transform.Find("cancel").gameObject.SetActive(false);
	}

}
