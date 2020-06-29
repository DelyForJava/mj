using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
	public Text loadingText;
	public Image progressBar;
	private int curProgressValue = 0;
	void FixedUpdate()
	{
		int progressValue = 100;

		if(curProgressValue < progressValue)
		{
			curProgressValue++;
		}
		loadingText.text = "正在努力加载中..." + curProgressValue + "%";//实时更新进度百分比的文本显示

		progressBar.fillAmount = curProgressValue / 100f;//实时更新滑动进度图片的fillAmount值

		if(curProgressValue == 100)
		{
			loadingText.text = "OK";//文本显示完成ok
									//SceneManager.LoadScene("Menu");

			if (LoadManager.Instance.async!=null)
			{
				LoadManager.Instance.async.allowSceneActivation = true;
			}
			Scene scence = SceneManager.GetActiveScene();		
			if (scence.name=="Main")
			{				
				Invoke("delect", 0.5f);
			}
			
		}
	}

	void delect() {
		Destroy(this.gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
