using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ZhanjiController : MonoBehaviour {

	void Start() {

	     transform.Find("BG/close").GetComponent<Button>().onClick.AddListener( () => {
			 Close();
		 });
		
	}

	public void  Close()
    {
		Audiocontroller.Instance.PlayAudio("Back");			
		Destroy(gameObject);
	}
}
