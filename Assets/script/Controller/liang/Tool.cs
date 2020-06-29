using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using LitJson;
using JX;

public class Tool : MonoBehaviour {

    public static Tool _instance = null;

    private Transform btn1;
    private Transform btn2;

    private Transform button;

    private void Awake()
    {
         _instance=this;

        button = transform.Find("Button").transform;
        btn1 = transform.Find("btn1").transform;
        btn2 = transform.Find("btn2").transform;

        transform.Find("close").GetComponent<Button>().onClick.AddListener(()=> {
            Audiocontroller.Instance.PlayAudio("Back");
            Destroy(this.gameObject);
        });
    }
   
    
    /// <summary>
    /// Tool工具方法
    /// </summary>
    /// <param name="cause">显示的文本</param>
    /// <param name="action">确定按钮的事件</param>
    public void ShowTool(string cause, Action action) {

        btn2.gameObject.SetActive(false);
        btn1.gameObject.SetActive(false);
        if (this.transform)
        {
           Text t= this.transform.Find("Text").GetComponent<Text>();
            t.font = UIManager.Instance.font;
            t.text = cause;
        }

        transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate () {
            action();
            Destroy(this.gameObject);
        });
    }

    /// <summary>
    /// Tool工具方法  重载
    /// </summary>
    /// <param name="cause">显示的文本</param>
    /// <param name="action1">第一个方法</param>
    /// <param name="action2">第二个方法</param>
    public void ShowTool(string cause, Action action1,Action action2)
    {
        button.gameObject.SetActive(false);
        if (this.transform)
        {
            Text t = this.transform.Find("Text").GetComponent<Text>();
            t.font = UIManager.Instance.font;
            t.text = cause;
        }

        btn1.GetComponent<Button>().onClick.AddListener(delegate () {
            action1();
            Destroy(this.gameObject);
        });

        btn2.GetComponent<Button>().onClick.AddListener(delegate () {
            action2();
            Destroy(this.gameObject);
        });
    }
}
