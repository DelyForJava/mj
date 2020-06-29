using UnityEngine;

public class YuYinScript : MonoBehaviour
{
	public float WholeTime = 0.0f;
	public GameObject InputGameObject;
	private bool btnDown ;
	private bool isCancel;

	public GameObject Cancel;

	void Start() {
		btnDown = false;
		isCancel = false;
	}

	void FixedUpdate()
	{
		if (btnDown)
		{
			WholeTime += Time.deltaTime;
			if (WholeTime>=5)
			{
				OnPointerUp();
			}
		}
	}

	//按下 开始录音
	public void OnPointerDown()
	{
		if (!Audiocontroller.GetEffectAudio())
		{
			Prefabs.Buoy("使用语音请在设置打开音效开关");
			return;
		}
		btnDown = true;
		isCancel = false;
		InputGameObject.SetActive(true);
		Audiocontroller.Instance.MicrophoneDown();
	}

	//抬起  结束录音 发送
	public void OnPointerUp()
	{
		if (btnDown)
		{
			btnDown = false;
			InputGameObject.SetActive(false);
			Debug.Log("WholeTime========" + WholeTime);
			//取消发送
			if (isCancel)
			{
				Cancel.gameObject.SetActive(true);
				Invoke("setCancelFalse",1);
				Audiocontroller.Instance.SetPlayBgm();
			}
			else
			{
				Audiocontroller.Instance.MicrophoneUp(WholeTime);
			}		
			WholeTime = 0.0f;
		}
	}

	//取消发送
	void setCancelFalse() {
		Cancel.gameObject.SetActive(false);
	}

	//取消
	public void OnPointerMove()
	{
		isCancel = true;
		Debug.Log("iscancel============="+isCancel);
	}
}
