using UnityEngine.SceneManagement;
using UnityEngine;


enum Bundle
{
    ugui,
    texture
}

public class ResourceManager : Singleton<ResourceManager>
{
    public AssetBundle ab = null;
    private AssetBundle m_UIAssetBundle;
    private AssetBundle m_SpriteAssetBundle;
    //加载面板
    public GameObject LoadAssetBunle(string abName, string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        AssetBundle ab = AssetBundle.LoadFromFile(PathInfo.DownLoadPath + "/" + abName);
        return GameObject.Instantiate(ab.LoadAsset<GameObject>(name));
    }

    //加载指定ab包里指定的资源
    public T LoadAssetBunle<T>(AssetBundle ab, string name) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(name)) return null;

        GameObject obj = ab.LoadAsset(name) as GameObject;

        GameObject pre = GameObject.Instantiate<GameObject>(obj);

        pre.name = name;

        RectTransform rtf = pre.transform as RectTransform;

        Scene scene = SceneManager.GetActiveScene();
        //显示在Canvas下
        if (scene.name=="Main")
        {
            rtf.SetParent(GameObject.Find("GameStart/Canvas").transform);
        }
        else
        {
            rtf.SetParent(GameObject.Find("/Canvas").transform);
        }
        
       
        //Transform初始化一下
        rtf.localPosition = Vector3.zero;
        rtf.localRotation = Quaternion.identity;
        rtf.localScale = Vector3.one;
        //把当前物体放在同级最后一个
        rtf.SetAsLastSibling();

        //四锚点归零
        rtf.offsetMin = Vector2.zero;
        rtf.offsetMax = Vector2.zero;

        return pre as T;
    }

    public T LoadAssetBunle<T>(AssetBundle ab, string name,Transform grid) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(name)) return null;

        GameObject obj = ab.LoadAsset(name) as GameObject;

        GameObject pre = GameObject.Instantiate<GameObject>(obj);

        pre.name = name;

        RectTransform rtf = pre.transform as RectTransform;

        //显示在grid下
        rtf.SetParent(grid);
        //Transform初始化一下
        rtf.localPosition = Vector3.zero;
        rtf.localRotation = Quaternion.identity;
        rtf.localScale = Vector3.one;
        //把当前物体放在同级最后一个
        rtf.SetAsLastSibling();
        return pre as T;
    }

    public AssetBundle LoadAssetBunle(string name,bool isLocal)
    {
        if (isLocal)
        {
            return AssetBundle.LoadFromFile(string.Format(Application.streamingAssetsPath + "/{0}", name));
        }
        else
        {
             return AssetBundle.LoadFromFile( string.Format(PathInfo.DownLoadPath + "/{0}",name));
        }
    }
    /// <summary>
    /// 加载AssetBudle的图片 sprite格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ab"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public T LoadAssetBunlePic<T>(AssetBundle ab, string name) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(name)) return null;

        Sprite obj = ab.LoadAsset(name, typeof(Sprite)) as Sprite;
        Sprite pre = GameObject.Instantiate<Sprite>(obj);

        return obj as T;
    }

    //加载指定ab包里指定的资源
    public T LoadAssetBunle<T>(string abName, string name) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(name)) return null;

        if (!ab)
        {
            ab = AssetBundle.LoadFromFile(PathInfo.DownLoadPath + "/" + abName);   //加载依赖Ab
        }

        GameObject obj = ab.LoadAsset(name) as GameObject;

        GameObject pre = GameObject.Instantiate<GameObject>(obj);

        pre.name = name;

        RectTransform rtf = pre.transform as RectTransform;

        //显示在Canvas下
        rtf.SetParent(GameObject.Find("/Canvas").transform);
        //Transform初始化一下
        rtf.localPosition = Vector3.zero;
        rtf.localRotation = Quaternion.identity;
        rtf.localScale = Vector3.one;
        //把当前物体放在同级最后一个
        rtf.SetAsLastSibling();

        //四锚点归零
        rtf.offsetMin = Vector2.zero;
        rtf.offsetMax = Vector2.zero;

        return pre as T;
    }




    #region 热更新加载
    public T HotFixLoaderAssetBundle<T>(string name) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(name)) return default(T);
        TestInfo.Instance.ShowTxt("实例化" + name);
        if (m_UIAssetBundle == null) m_UIAssetBundle = AssetBundle.LoadFromFile(PathInfo.DownLoadPath + "/ugui");
        TestInfo.Instance.ShowTxt("实例化" + name+"成功");
        return m_UIAssetBundle.LoadAsset<T>(name);
    }
    public Sprite HotFixLoaderSprite(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        m_SpriteAssetBundle = AssetBundle.LoadFromFile(PathInfo.DownLoadPath + "/hotPic" );
        return m_SpriteAssetBundle.LoadAsset<Sprite>(name);
    }
    public T HotFixLoaderAssetBundle<T>(string name, bool local) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(name)) return default(T);
        TestInfo.Instance.ShowTxt("实例化" + name);
        if (m_UIAssetBundle == null) m_UIAssetBundle =local? LoadAssetBunle("ugui",local): AssetBundle.LoadFromFile(string.Format(PathInfo.DownLoadPath + "/{0}", "ugui"));
        TestInfo.Instance.ShowTxt("实例化" + name + "成功");
        return m_UIAssetBundle.LoadAsset<T>(name);
    }
    public Sprite HotFixLoaderSprite(string name,bool local)
    {
        if (string.IsNullOrEmpty(name)) return null;
        m_SpriteAssetBundle = local? LoadAssetBunle(name, local) : AssetBundle.LoadFromFile(PathInfo.DownLoadPath + "/hotPic");
        return m_SpriteAssetBundle.LoadAsset<Sprite>(name);
    }
    #endregion
}
