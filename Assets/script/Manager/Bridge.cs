using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;

public enum LoadAb
{
    Login,
    Main,
    Game,
    Pic,
    Face,
    MainTwo
}

public class Bridge : MonoBehaviour
{ 
    // use this for initialization

    public static Bridge _instance=null;

    public  AssetBundle LoginAb;
    public  AssetBundle MainAb;
    public  AssetBundle MainTwoAb;
    public  AssetBundle GameAb;
    public  AssetBundle PicAb;
    public  AssetBundle FaceAb;

    public  string token;

    //正式服
    //private const string Host = "47.92.172.214";
    //测试服
      private const string Host= "39.104.87.202";
    //端口
    private const string Port= "8080";

    /// <summary>
    /// 端口
    /// </summary>
    /// <returns></returns>
    public static string GetHostAndPort() {
        return Host + ":" + Port;
    }

    /// <summary>
    /// Host
    /// </summary>
    /// <returns></returns>
    public static string GetHost()
    {
        return Host;
    }



    /// <summary>
    /// yl
    /// 单例  以便在场景中方便调用
    /// </summary>
    private void Awake()
    {
        _instance = this;
        token = PlayerPrefs.GetString("UserId.token");
        UserId.token= PlayerPrefs.GetString("UserId.token");
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    //void start()
    //{
    //    DontDestroyOnLoad(gameObject);

    //    //loginab = assetbundle.loadfromfile(pathinfo.downloadpath + "/login");   //加载依赖ab
    //    // mainab = assetbundle.loadfromfile(pathinfo.downloadpath + "/main");
    //    //gameab = assetbundle.loadfromfile(pathinfo.downloadpath + "/game");

    //    //ab包事先加好
    //    // string abdir = application.datapath.substring(0, application.datapath.length - 7) + "/ab";/*"d:/新建文件夹/ab"*/;
    //    // string abdir = application.datapath.substring(0, application.datapath.length - 7) + "/ab";


    //    // string abdir = "d:/新建文件夹/majiangshang/ab";
    //    //debug.log("abdir=====" + abdir);

    //    //string abdir = "c:/users/dell/desktop/ab";


    //    //resourcemanager.instance.loadassetbunle<gameobject>("ugui", "loadbar");

    //    // abmanager.instance.loadfile("ugui");
    //    //abmanager.instance.loadfile("bubble");
    //    //      abmanager.instance.loadfile("buoy");
    //    //      abmanager.instance.loadfile("liang/jiazai");
    //    //abmanager.instance.loadfile("liang/creatroom");
    //    //abmanager.instance.loadfile("liang/e_mail");
    //    //abmanager.instance.loadfile("liang/feixiang");

    //    //abmanager.instance.loadfile("liang/houdong");
    //    //abmanager.instance.loadfile("liang/joinroom");
    //    //abmanager.instance.loadfile("liang/kefu");
    //    //abmanager.instance.loadfile("liang/paihangbang");
    //    ////abmanager.instance.loadfile("liang/qiandao");
    //    //abmanager.instance.loadfile("liang/qinyouquang");
    //    //abmanager.instance.loadfile("liang/set");
    //    //abmanager.instance.loadfile("liang/shop");
    //    //abmanager.instance.loadfile("liang/yonghuxieyi");
    //    //abmanager.instance.loadfile("liang/zhanji");
    //    //abmanager.instance.loadfile("liang/zhanweikaifang");
    //    //      abmanager.instance.loadfile("liang/renzhen");
    //    //      //abmanager.instance.loadfile("liang/tool");
    //    //      abmanager.instance.loadfile("liang/faceview");
    //    //      abmanager.instance.loadfile("liang/giftview");
    //    //      abmanager.instance.loadfile("liang/wareview");
    //    //      abmanager.instance.loadfile("liang/txview");
    //    //      abmanager.instance.loadfile("liang/playerinfo");
    //    //      abmanager.instance.loadfile("liang/forget");



    //    //      abmanager.instance.loadfile("loadsceen/zhuce");
    //    //abmanager.instance.loadfile("paihangbangres");
    //    //abmanager.instance.loadfile("mailres");
    //    //abmanager.instance.loadfile("sendgold");
    //    ////abmanager.instance.loadfile("shopres");
    //    //abmanager.instance.loadfile("friendres");
    //    //abmanager.instance.loadfile("writemail");
    //    //abmanager.instance.loadfile("majiangpai");
    //    //abmanager.instance.loadfile("majiang");
    //    //abmanager.instance.loadfile("application");
    //    //abmanager.instance.loadfile("friendapplic");
    //    //abmanager.instance.loadfile("majiangtip");
    //    //abmanager.instance.loadfile("talk");
    //    //abmanager.instance.loadfile("playinfo");
    //    //abmanager.instance.loadfile("biaoqin");
    //    //abmanager.instance.loadfile("exicon");
    //}


        /// <summary>
        /// yl
        /// 这是一个单例   避免重复加载报错
        /// 用哪个预设体就加载哪个
        /// </summary>
        /// <param name="abname">包名</param>
        /// <param name="name">预设体名</param>
        /// <returns></returns>
    public GameObject LoadAbDate(LoadAb abname, string name)
    {
        //Debug.Log("====================");
        GameObject obj = null;
        switch (abname)
        {
            case LoadAb.Login:
                if (LoginAb == null)
                {
                    LoginAb = ResourceManager.Instance.LoadAssetBunle("login", false); 
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(LoginAb, name);                  
                }
                else
                {
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(LoginAb, name);
                }

                break;
            case LoadAb.Main:
                if (MainAb == null)
                {
                    MainAb = ResourceManager.Instance.LoadAssetBunle("main", false);
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(MainAb, name);
                }
                else
                {
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(MainAb, name);
                }
                break;

            case LoadAb.MainTwo:
                if (MainTwoAb == null)
                {
                    MainTwoAb = ResourceManager.Instance.LoadAssetBunle("maintwo", false);
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(MainTwoAb, name);
                }
                else
                {
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(MainTwoAb, name);
                }
                break;
            case LoadAb.Game:
                if (GameAb == null)
                {
                    GameAb = ResourceManager.Instance.LoadAssetBunle("game", false);
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(GameAb, name);
                }
                else
                {
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(GameAb, name);
                }
                break;
            default:
                break;
        }

        return obj;
    }

    public List<Sprite> LoadFace(string faceName)
    {
        Sprite[] ss = null;
        List<Sprite> sprite = new List<Sprite>();
        if (FaceAb == null)
        {
             //FaceAb = AssetBundle.LoadFromFile(PathInfo.DownLoadPath + "/face");
            if (GameStart.Instance.isYYB)
            {
                FaceAb = ResourceManager.Instance.LoadAssetBunle("face", false);
            }
            else
            {
                FaceAb = AssetBundle.LoadFromFile(PathInfo.DownLoadPath + "/face");
            }
            ss = FaceAb.LoadAllAssets<Sprite>();
            for (int i = 0; i < ss.Length; i++)
            {
                if (ss[i].name.Contains(faceName))
                {
                    sprite.Add(ss[i]);
                }
            }
            return sprite;
        }
        ss = FaceAb.LoadAllAssets<Sprite>();
        for (int i = 0; i < ss.Length; i++)
        {
            if (ss[i].name.Contains(faceName))
            {
                sprite.Add(ss[i]);
            }
        }
        return sprite;
    }

    public GameObject LoadAbDate(LoadAb abname, string name,Transform grid)
    {
        //Debug.Log("====================");
        GameObject obj = null;
        switch (abname)
        {
            case LoadAb.Login:
                if (LoginAb == null)
                {
                    LoginAb = ResourceManager.Instance.LoadAssetBunle("login", false);
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(LoginAb, name,grid);
                }
                else
                {
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(LoginAb, name,grid);
                }

                break;
            case LoadAb.Main:
                if (MainAb == null)
                {
                    MainAb = ResourceManager.Instance.LoadAssetBunle("main", false);
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(MainAb, name,grid);
                }
                else
                {
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(MainAb, name,grid);
                }
                break;

            case LoadAb.MainTwo:
                if (MainTwoAb == null)
                {
                    MainTwoAb = ResourceManager.Instance.LoadAssetBunle("maintwo", false);
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(MainTwoAb, name, grid);
                }
                else
                {
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(MainTwoAb, name, grid);
                }
                break;
            case LoadAb.Game:
                if (GameAb == null)
                {
                    GameAb = ResourceManager.Instance.LoadAssetBunle("game", false);
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(GameAb, name,grid);
                }
                else
                {
                    obj = ResourceManager.Instance.LoadAssetBunle<GameObject>(GameAb, name,grid);
                }
                break;
            default:
                break;
        }

        return obj;
    }

    public Sprite LoadAbDateSprite(LoadAb abname, string name)
    {
       
        Sprite obj = null;
        switch (abname)
        {
            case LoadAb.Pic:
                if (PicAb == null)
                {
                    PicAb = ResourceManager.Instance.LoadAssetBunle("pic", false);
                    obj = ResourceManager.Instance.LoadAssetBunlePic<Sprite>(PicAb, name);
                }
                else
                {
                    obj = ResourceManager.Instance.LoadAssetBunlePic<Sprite>(PicAb, name);
                }
                break;
            case LoadAb.Face:
                if (FaceAb == null)
                {
                    FaceAb = ResourceManager.Instance.LoadAssetBunle("face", false);
                    obj = ResourceManager.Instance.LoadAssetBunlePic<Sprite>(FaceAb, name);
                }
                else
                {
                    obj = ResourceManager.Instance.LoadAssetBunlePic<Sprite>(FaceAb, name);
                }
                break;
            default:
                break;
        }
        return obj;
    }

    /// <summary>
    ///yl 2020.4.17 登录回调 
    /// </summary>
    /// <param name="data"></param>
    public void loginCallBack(JsonData data ,bool isRegister) {

        JsonData d2 = data["data"];
        JsonData d3 = isRegister ? d2["member"]:d2;
        UserId.memberId = (int)d3["id"];
        UserId.name = (string)d3["nickname"];
        UserId.goldCount = (int)d3["gold"];
        UserId.avatar = (string)d3["avatar"];
        UserId.PictureFrame = (int)d3["headFrame"];
        UserId.IsIdTrue = (int)d3["IsIdTrue"];
        string s= JsonMapper.ToJson(data);
        UserId.NeedWXCallBackData = s;
        WebSoketCall.One().isLinkWS = false;
        WebSoketCall.One().isGame = false;
        if (isRegister)
        {
            //UserId.token = (string)d2["token"];    //这个用不到  暂时屏蔽
            PlayerPrefs.SetString("UserId.token", (string)d2["token"]);
            Bridge._instance.token = (string)d2["token"];
            Debug.Log("+Token1" + Bridge._instance.token);
        }

        //SceneManager.LoadSceneAsync("liang");
        //Bridge._instance. LoadAbDate(LoadAb.Login, "loadbar");
        //LoadManager.Instance.LoadScene("liang");
        GameMapManager.Instance.NormalLoadScene("liang");

    } 
}
