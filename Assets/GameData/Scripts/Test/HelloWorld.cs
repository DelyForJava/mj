
using ILRuntime.Runtime.Enviorment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class HelloWorld : MonoBehaviour
{
    TextAsset ass;
    //AppDomain是ILRuntime的入口，最好是在一个单例类中保存，整个游戏全局就一个，这里为了示例方便，每个例子里面都单独做了一个
    //大家在正式项目中请全局只创建一个AppDomain
    ILRuntime.Runtime.Enviorment.AppDomain appdomain;

    System.IO.MemoryStream fs;
    System.IO.MemoryStream p;
    void Start()
    {
        StartCoroutine(LoadModelFromLocal());
    }
    private IEnumerator LoadModelFromLocal()
    {
        string uri = PathInfo.HotFix_Project;
        Debug.Log("正在从本地加载模型: " + uri);
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri);
        Debug.Log("从本地加载模型" + uri);
        yield return request.Send();                // 任务: 抛出异常如何处理?
        try
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            //bundle.Load<TextAsset>("HotFix_Project.dll");
            TextAsset ass = bundle.LoadAsset<TextAsset>("HotFix_Project.dll");
            Debug.Log(ass.text);
            // GameObject obj = bundle.LoadAsset<GameObject>(GetLastPartOfPath(assetBundleName) + ".prefab");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
   

    void InitializeILRuntime()
    {
        //这里做一些ILRuntime的注册，HelloWorld示例暂时没有需要注册的
    }

    void OnHotFixLoaded()
    {
        //HelloWorld，第一次方法调用
        appdomain.Invoke("HotFix_Project.InstanceClass", "TestFun", null, null);
        //appdomain.Invoke("HotFix_Project.TestClass", "TestFun", null, null);
    }

    private void OnDestroy()
    {
        if (fs != null)
            fs.Close();
        if (p != null)
            p.Close();
        fs = null;
        p = null;
    }

    void Update()
    {

    }
}
