using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DrawAni : MonoBehaviour {
	
	//是消息还是礼物
	public static bool isMessage;
    void Awake() {

		if (!isMessage)		
			transform.Find("ani").localScale = Vector2.zero;
		
		
		transform.gameObject.AddComponent<Button>().onClick.AddListener(()=> {
			Destroy(this.gameObject);
		});

	}
	void Start () {

		if (!isMessage)
			transform.Find("ani").DOScale(Vector3.one,1f);
	}
	
}
