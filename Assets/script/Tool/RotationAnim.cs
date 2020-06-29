using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAnim : MonoBehaviour {

	public float ratio = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(-Vector3.forward,ratio*Time.unscaledDeltaTime);
	}


}
