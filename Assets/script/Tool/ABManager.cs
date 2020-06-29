using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABManager : MonoBehaviour
{
    #region Singleton

    //Mono单例
    //私有的静态变量存实例
    //公有的静态成员属性获得单例
    //实例的创建应该由AddComponent创建
    //防止切换场景时丢失管理器

    private static ABManager _Instance = null;
    public static ABManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject go = new GameObject("ABManager");
                _Instance = go.AddComponent<ABManager>();
                DontDestroyOnLoad(go);
            }

            return _Instance;
        }
    }

    #endregion

    #region Initialize

    //所有AB包的存储路径
    private string _ABPath = "";
    //主配置文件
    public  AssetBundleManifest _Manifest = null;

    //初始化方法
    //path：所有AB包的存储路径
    public void Initialize(string path, string mainName)
    {
        //防止多次初始化
        if (_ABPath != "")
        {
            return;
        }

        _ABPath = path;
        //主AB包路径
        string mainPath = path + "/" + mainName;
        //加载主AB包
        AssetBundle mainAB = AssetBundle.LoadFromFile(mainPath);
        _Manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }

    public void res(string path, string mainName)
    {

        //ResourceManager.Instance.LoadAssetBunle<GameObject>();
        //_LoadedAB.Add(abName, AssetBundle.LoadFromFile(_ABPath + "/" + abName));
        // ResourceManager.Instance.LoadAssetBunle<GameObject>();
    }

    #endregion

    #region LoadFile

    //用于防止ab包重复加载的结构（键：ab包名称，值：ab包对象）
    public  Dictionary<string, AssetBundle> _LoadedAB = new Dictionary<string, AssetBundle>();

    //加载AB包
    //name：需要加载的ab包名称
    public void LoadFile(string abName)
    {
        //先加载依赖
        string[] files = _Manifest.GetAllDependencies(abName);
        for (int i = 0; i < files.Length; i++)
        {
            //防止依赖文件之前加载过
            if (!_LoadedAB.ContainsKey(files[i]))
            {
                _LoadedAB.Add(files[i], AssetBundle.LoadFromFile(_ABPath+ "/" + files[i]));
            }
        }

        if (!_LoadedAB.ContainsKey(abName))
        {
            //再加载AB包本身
            _LoadedAB.Add(abName, AssetBundle.LoadFromFile(_ABPath+ "/" + abName));
        }
    }

    public void newLoadFile(string abName)
    {
        //先加载依赖
        string[] files = _Manifest.GetAllDependencies(abName);
        for (int i = 0; i < files.Length; i++)
        {
            //防止依赖文件之前加载过
            if (!_LoadedAB.ContainsKey(files[i]))
            {
                _LoadedAB.Add(files[i], AssetBundle.LoadFromFile(_ABPath + "/" + files[i]));
            }
        }

        if (!_LoadedAB.ContainsKey(abName))
        {

            //再加载AB包本身
           // _LoadedAB.Add(abName, AssetBundle.LoadFromFile(_ABPath + "/" + abName));
        }
    }

    #endregion

    #region LoadAsset
    //加载资源
    //rule：ab包名#资源名
    public T LoadAsset<T>(string rule) where T : Object
    {
        //分割包名和资源名
        string[] names = rule.Split('#');

        //如果ab包已经被加载过
        //Debug.Log("包名==" + names[0] + "资源名===" + names[1]);

        if (_LoadedAB.ContainsKey(names[0]))
        {
            return _LoadedAB[names[0]].LoadAsset<T>(names[1]);
        }
        else
        {
            Debug.LogError(names[0] + "还没有被加载！");
            return null;
        }
    }



    public Object LoadAsset(string rule)
    {
        //分割包名和资源名
        string[] names = rule.Split('#');

        //如果ab包已经被加载过
        if (_LoadedAB.ContainsKey(names[0]))
        {
            return _LoadedAB[names[0]].LoadAsset(names[1]);
        }
        else
        {
            Debug.LogError(names[0] + "还没有被加载！");
            return null;
        }
    }
    #endregion
}
