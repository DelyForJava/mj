using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharePanel : MonoBehaviour {

	void Start () {
		transform.Find("ShareBut").GetComponent<Button>().onClick.AddListener(() => { Bridge._instance.LoadAbDate(LoadAb.Main, "feixiang"); Destroy(this.gameObject); });
		transform.Find("Close").GetComponent<Button>().onClick.AddListener(() => { Destroy(this.gameObject); });


	}
}
