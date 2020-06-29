using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TestInfo :MonoSingleton<TestInfo>
{
    public Image imag;
    public Button butt;
    public Text text;
    string str1 = "";
    private string str;
    public Toggle toggle;
    public VideoClip videoClip;
    void Start()
    {
    //    foreach (ItemSonPanel item in GameObject.Find("Son").transform.GetComponentsInChildren<ItemSonPanel>())
    //    {
    //       item.
    //    }
        Debug.Log(Application.persistentDataPath);
        //imag.sprite = ResourceManager.Instance.LoadAssetBunle<Sprite>("texture", "1.jpg");
        //GameMapManager.Instance.Init(GameStart.instance);
        //ILRuntimeManager.Instance.Init(GameStart.instance);
       // Debug.Log(Application.persistentDataPath);
        //ILRuntimeManager.Instance.Init(this);
        //StartCoroutine(ReadXml1());
        //StartCoroutine(ReadXml());
        //StartCoroutine(DownFile("http://" + Bridge.GetHostAndPort() +"/images/HotFix_Project.dll.txt"));
    }

    public void ShowTxt(string str)
    {
        text.text = str1 += str + "    ";
    }

    private IEnumerator ReadXml1()
    {
        yield return str = "1";

        Debug.Log(str);
        yield return str = "2";
        Debug.Log(str);

    }
    private IEnumerator ReadXml()
    {
        string xmlUrl = "http://" + Bridge.GetHostAndPort() +"/images/HotFix_Project.dll.txt";
        UnityWebRequest webRequest = UnityWebRequest.Get(xmlUrl);
        yield return webRequest.Send();
        if (webRequest.isDone)
        {
            ShowTxt("UnityWebRequest下载：" + webRequest.downloadHandler.data.Length);
        }
        else
        {
            ShowTxt("Download 失败：" + webRequest.error);
        }

    }
    IEnumerator DownFile(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.isDone)
        {
            byte[] bytes = www.bytes;
            ShowTxt("www下载：" + bytes.Length.ToString());
        }
    }

}
