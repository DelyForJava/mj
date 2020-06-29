using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JX;
using UnityEngine.UI;
using LitJson;
public class YHXYController : MonoBehaviour {
	public Text WText;
	string url= "http://"+Bridge.GetHostAndPort()+"/api/staticuserProtocol";
	string jsonPost;

	// Use this for initialization
	void Start () {
		jsonPost = "{ \"phone\":\"18370806388\"}";
		HttpCallSever.One().GetCallSetver(url,RegisterCallBack);
	}
	
	void RegisterCallBack(string json)
	{
		JsonData jsom = JsonMapper.ToObject(json);
		WText.text= (string)jsom["data"];
	}
	public void Close()
    {
		Destroy(gameObject);
    }
}
