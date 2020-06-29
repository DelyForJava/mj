using JX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class KeFuController : MonoBehaviour {
	private InputField inputField;
	private Button proposal;
	private Button close;
	// Use this for initialization
	void Start () {
		inputField = transform.Find("BG").Find("InputField").GetComponent<InputField>();
		proposal = transform.Find("BG").Find("TiJiao").GetComponent<Button>();

		close = transform.Find("BG").Find("close").GetComponent<Button>();
		close.onClick.AddListener(() => { Destroy(this.gameObject); });
		proposal.onClick.AddListener(() =>
		{
			if (inputField.text != string.Empty)
			{
				//string jsonStr = JsonConvert.SerializeObject(new Dictionary<object, object>() { { "type", 1 }, { "content", inputField.text } });
				//Debug.Log(jsonStr);
				Debug.Log(inputField.text);
				HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() +"/api/feedback/up", JsonMapper.ToJson(new Kefu(1,inputField.text)), (string str) => {
					JsonData json = JsonMapper.ToObject(str);
					if ((int)json["code"] == 200)
					{
						Prefabs.Buoy("反馈成功");
					}
					else
					{
						Prefabs.Buoy("反馈失败");
					}
				});
			}
			else
			{
				Prefabs.Buoy("反馈信息不能为空");
			}
			
			Destroy(gameObject);
		});
	}
	public class Kefu
    {
		public int type;
		public string content;
		public Kefu(int t,string c)
        {
			this.type = t;
			this.content = c;
        }
	}
}
