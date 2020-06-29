using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JX;
using LitJson;

public class LoadUIController : MonoBehaviour {

	public  AudioSource m_Audio;

	public InputField Username;
	public InputField Password;


    void Start()
	{
        m_Audio = Camera.main.GetComponent<AudioSource>();
		m_Audio.Stop();
	}

	/// <summary>
	/// 注册
	/// </summary>
	public void ZhangHuZhuChe()
	{
		Audiocontroller.Instance.PlayAudio("ZhangHuZhuChe");
 
        Bridge._instance.LoadAbDate(LoadAb.Login, "ZhuChe");

    }

 
    /*
	public void ZhangHuDengRu()
	{
		//Audiocontroller.Instance.PlayAudio("ZhangHuDengRu");
		string username = Username.text;
		string password = Password.text;

        
		if (username == "" || password == "")
		{
            GameObject obj = Bridge._instance.LoadAbDate(LoadAb.Main,"Bubble");
            obj.GetComponent<BubbleUIController>().InfoTxt.text = "数据不完整";
            //Prefabs.PopBubble("数据不完整");
			return;
		}
        
		
	    string jsonPost=PLData(username, password);
		PhoneLoad(jsonPost);

	}
    */

	class jsonData_ 
	{
		 public  string phone;
		 public  string password;
	}

	public string  PLData(string phone,string password)
	{
		jsonData_ data = new jsonData_();
		data.phone = phone;
		data.password = password;		
		string  Jon =JsonMapper.ToJson(data);
		return Jon;
		
	}

    /// <summary>
    /// 忘记密码
    /// </summary>
    public void ForgetPassw() {

        Bridge._instance.LoadAbDate(LoadAb.Login,"forget");
    }

	public void WeiXiDengRu()
	{
		Audiocontroller.Instance.PlayAudio("WeiXiDengRu");
	}
}
