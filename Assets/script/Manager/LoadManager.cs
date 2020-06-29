using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadManager : MonoBehaviour {

    public static LoadManager Instance;
    public   Action<int> CallBack;
    private void Awake()
    {
        //Instance = this;//this:当前代码组建对象
        Instance = gameObject.GetComponent<LoadManager>();
        DontDestroyOnLoad(gameObject);
    }
    public delegate void LoadCallback();//回调
    IEnumerator loadSceneIE = null;

    public void LoadScene(string sceneName)
    {
        loadSceneIE = LoadSceneIE(sceneName);
        StartCoroutine(loadSceneIE);//启动协程
    }
    public void LoadScene(string sceneName,Action<int> CallBack,int data)
    {
        loadSceneIE = LoadSceneIE(sceneName,CallBack,data);
        StartCoroutine(loadSceneIE);//启动协程
    }
    public void LoadScene(string sceneName, Action<int,int> CallBack, int data,int maxPoint)
    {
        loadSceneIE = LoadSceneIE(sceneName, CallBack, data,maxPoint);
        StartCoroutine(loadSceneIE);//启动协程
    }
    public void LoadScene(string sceneName, Action<string> CallBack, string data)
    {
        loadSceneIE = LoadSceneIE(sceneName, CallBack, data);
        StartCoroutine(loadSceneIE);//启动协程
    }
    public  AsyncOperation async = null;
 
    IEnumerator LoadSceneIE(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        yield return async;       
        async = null;
        StopCoroutine(loadSceneIE);
    }
    IEnumerator LoadSceneIE(string sceneName,Action<int> callBack,int data)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        //async.allowSceneActivation = false;
        yield return async;
        callBack(data);
        async = null;
        StopCoroutine(loadSceneIE);
    }
    IEnumerator LoadSceneIE(string sceneName, Action<int,int> callBack, int data,int pointData)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        //async.allowSceneActivation = false;
        yield return async;
        callBack(data,pointData);
        async = null;
        StopCoroutine(loadSceneIE);
    }
    IEnumerator LoadSceneIE(string sceneName, Action<string> callBack, string data)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        //async.allowSceneActivation = false;
        yield return async;
        callBack(data);
        async = null;
        StopCoroutine(loadSceneIE);
    }
}
