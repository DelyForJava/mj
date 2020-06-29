using DG.Tweening;
using UnityEngine;

public class BeatAnim : MonoBehaviour {
	public float scope;
	// Use this for initialization
	void Start () {
		transform.DOShakeScale(2.5f,0.1f,5).SetLoops(-1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
