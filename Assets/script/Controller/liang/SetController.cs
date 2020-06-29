using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using LitJson;

public class SetController : MonoBehaviour {
	
	public AudioSource Audio;
	public AudioSource YinxiaoA;
    
	public  bool isActive;
	public  bool isActive2;
	// Use this for initialization
	void Start () {
		
		Audio = Camera.main.GetComponent<AudioSource>();
		YinxiaoA = GameObject.Find("Audiocontroller").GetComponent<AudioSource>();

	    string IsMuOp=PlayerPrefs.GetString("IsMusicOpen");
	    string IsEffectOp = PlayerPrefs.GetString("IsEffectOpen");
		//if (IsMuOp=="Open")		
		isActive = IsMuOp == "Open"||string.IsNullOrEmpty(IsMuOp) ? true:false;
		isActive2 = IsEffectOp == "Open" || string.IsNullOrEmpty(IsEffectOp) ? true : false;


		transform.Find("music").Find("On").gameObject.SetActive(isActive);
        transform.Find("music").Find("Off").gameObject.SetActive(!isActive);

        transform.Find("effect").Find("On").gameObject.SetActive(isActive2);
        transform.Find("effect").Find("Off").gameObject.SetActive(!isActive2);

		Scene scen = SceneManager.GetActiveScene();		
		if (scen.name != "liang")
		{
			transform.Find("Quit").gameObject.SetActive(false);
			transform.Find("Switch").gameObject.SetActive(false);
		}
		else
		{
			transform.Find("Quit").gameObject.SetActive(true);
			transform.Find("Switch").gameObject.SetActive(true);
			transform.Find("Quit").GetComponent<Button>().onClick.AddListener(TuiChu);
			transform.Find("Switch").GetComponent<Button>().onClick.AddListener(Switch);
		}	
	}

	
	public void SoundController()
	{
		isActive= isActive == true ? false : true;
		
		if (isActive)
		{
			Audio.Play();
			Audio.volume = 1;

			transform.Find("music").Find("On").gameObject.SetActive(true);
            transform.Find("music").Find("Off").gameObject.SetActive(false);

			PlayerPrefs.SetString("IsMusicOpen","Open");
		}
		else
		{
			Audio.Stop();
            transform.Find("music").Find("On").gameObject.SetActive(false);
            transform.Find("music").Find("Off").gameObject.SetActive(true);

			 PlayerPrefs.SetString("IsMusicOpen","Close");
		}

	}
	public void YinxiaoController()
	{
		isActive2 = isActive2 == true ? false : true;	
		if (isActive2)
		{			
			YinxiaoA.volume = 1;
            transform.Find("effect").Find("On").gameObject.SetActive(true);
            transform.Find("effect").Find("Off").gameObject.SetActive(false);

			PlayerPrefs.SetString("IsEffectOpen","Open");
		}
		else
		{
			YinxiaoA.volume = 0;
            transform.Find("effect").Find("On").gameObject.SetActive(false);
            transform.Find("effect").Find("Off").gameObject.SetActive(true);

			PlayerPrefs.SetString("IsEffectOpen","Close");
		}
	}

  
	public void Close()
	{
		Audiocontroller.Instance.PlayAudio("Back");
        Destroy(this.gameObject);
		//gameObject.SetActive(false);
	}
	public void TuiChu()
	{
		Application.Quit();
	}

	public void Switch() {
		PlayerPrefs.DeleteKey("UserId.token");
		//Bridge._instance.LoadAbDate(LoadAb.Login,"loadbar");
		//SceneManager.LoadSceneAsync("loadsceen");
		LoadLineWS.One().StopAllIE();
		//Debug.LogError("0.0");
		WebSoketCall.One().isGame = false;
		WebSoketCall.One().isLinkWS = false;
		UserId.GameState = false;
		GameMapManager.Instance.NormalLoadScene("loadsceen");
		Member id = new Member();
		id.memberId = UserId.memberId.ToString();
		string json=JsonMapper.ToJson(id);
		JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/game/channel/close", json, CloseLink);
		WebSoketCall.One().FristLink = false;
		//WebSoketCall.One().ws.Close();
	} 
    void CloseLink(string edate) { Debug.Log(edate);}
}
