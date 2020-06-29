using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class feixiangcontroller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find("peiyouquang").GetComponent<Button>().onClick
		.AddListener(ShartSDKControlle.Instance.SharteWeChatMoments);
		GameObject.Find("weiixin").GetComponent<Button>().onClick
		.AddListener(ShartSDKControlle.Instance.SharteWebPage);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void close()
	{
		Audiocontroller.Instance.PlayAudio("Back");
	    transform.Find("feixiang_BG").transform.DOMoveY(1.4f, 0.8f).OnComplete(() => { Destroy(gameObject); });
	}
}
