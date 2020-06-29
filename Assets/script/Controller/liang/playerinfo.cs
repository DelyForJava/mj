using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JX;
using LitJson;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using System.Text;
using ILRuntime.Runtime;
using System.Text.RegularExpressions;
using System.Linq;

public class playerinfo : MonoBehaviour {
    public Button amendNameButt;
    public InputField inputField;
    List<Item> items = new List<Item>();
    List<string> strs = new List<string>();
    private Image headPortrait;
    private Text userName;
    private Text userId;
    private Image PictureFrame;
    public Button close;
    private readonly string amendName = "http://" + Bridge.GetHostAndPort() +"/api/member/update/nickname";


    private readonly string str = "http://" + Bridge.GetHostAndPort() +"/api/game/history";
    Action<string> call;
    List<GameObject> itemObjs = new List<GameObject>();
    private static char[] FiltrationChinese = new char[] {'草','操' };
    private void Start()
    {
       
        headPortrait  = transform.Find("tx").GetComponent<Image>();
        PictureFrame = headPortrait.transform.Find("PictureFrame").GetComponent<Image>();

        userName = transform.Find("Name").GetComponent<Text>();
        userName.text = UserId.name;

        userId = transform.Find("Id").GetComponent<Text>();
        userId.text = UserId.memberId.ToString();
        userName.font = UIManager.Instance.font;
        userId.font = UIManager.Instance.font;
        transform.Find("close").GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(this.gameObject);
        });


        PictureFrame.sprite = Resources.Load<Sprite>(string.Format("shop/head_{0}", UserId.PictureFrame));
        HttpCallSever.One().DownPic(UserId.avatar, headPortrait);
        //更换名字
        amendNameButt = userName.transform.Find("amendNameButt").GetComponent<Button>();
        amendNameButt.onClick.AddListener(() =>
        {
            inputField.text = UserId.name;
            if (!inputField.IsActive())
            {
                inputField.gameObject.SetActive(!inputField.gameObject.activeSelf);
            }
            inputField.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!string.IsNullOrEmpty(inputField.text))
                {
                    AmendPlayerName(amendName, inputField.text);
                    UserId.name = inputField.text;

                    GameObject.Find("Quad").transform.Find("tx/nickname").GetComponent<Text>().text = inputField.text;
                    inputField.text = string.Empty;
                }
                inputField.gameObject.SetActive(false);
            });
           
        });
        headPortrait.GetComponent<Button>().onClick.AddListener(() =>
        {
            Bridge._instance.LoadAbDate(LoadAb.MainTwo, "PlayerInfoSonPanel");
           // Instantiate<GameObject>(Resources.Load<GameObject>("PlayerInfoSonPanel"), UIManager.Instance.m_WndRoot);
            Destroy(this.gameObject);
        });
    }

    private void AmendPlayerName(string url, string str)
    {
        if (verifyChinese(str))
        {
            //string jsonStr = JsonConvert.SerializeObject(new Dictionary<string, string>() { { "nickName", str } });
            HttpCallSever.One().PostCallServer(url, JsonMapper.ToJson(new ItemClas1(str)), AmendCall);
        }
        else
        {
            inputField.text = string.Empty;
            Prefabs.Buoy("请重新输！");
        } 
    }
    private void AmendCall(string data)
    {
        JsonData json = JsonMapper.ToObject(data);
        if ((int)json["code"] == 200)
        {
            userName.text = (string)json["data"]["newName"];
            //TODO  提示昵称修改成功
            //Prefabs.PopBubble((string)json["message"]);
        }else if ((int)json["code"] == 300)
        {
            Debug.Log("修改成功！");
            //name.text = (string)json["data"]["newName"];
            //TODO  提示昵称修改成功
            //Prefabs.PopBubble((string)json["message"]);
        }
        else
        {
            Debug.Log("修改失败！");
        }
    }

   
    public bool verifyChinese(string oriText)
    {
        string x = @"[\u4E00-\u9FFF]+";
        MatchCollection Matches = Regex.Matches
        (oriText, x, RegexOptions.IgnoreCase);
        StringBuilder sb = new StringBuilder();
        foreach (Match NextMatch in Matches)
        {
            sb.Append(NextMatch.Value);
        }

        return verify(sb.ToString());
    }
    public bool verify(string oriText)
    {
        foreach (var chinese in oriText.ToCharArray())
        {
            foreach (var ch in FiltrationChinese)
            {
                if (chinese.Equals(ch)) {
                    return false;
                }
            }
        }
        return true;
    }
    #region 获取系统图片
    private void Loader()
    {
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "图片文件(*.jpg*.png)\0*.jpg;*.png";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        //默认路径
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        //默认路径
        //ofn.initialDir = "G:\\wenshuxin\\test\\HuntingGame_Test\\Assets\\StreamingAssets";
        ofn.initialDir = path;
        ofn.title = "Open Project";
        ofn.defExt = "JPG";//显示文件的类型
                           //注意 一下项目不一定要全选 但是0x00000008项不要缺少
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
                                                                                   //点击Windows窗口时开始加载选中的图片
        if (WindowDll.GetOpenFileName(ofn))
        {
            Debug.Log("Selected file with full path: " + ofn.file);
            StartCoroutine(Load(ofn.file));
            UnLoader(ofn.file);
        }
    }
    IEnumerator Load(string path)
    {
        UnityWebRequest wr = new UnityWebRequest("file:///" + path);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);
        wr.downloadHandler = texDl;
        yield return wr.Send();
        if (!wr.isNetworkError)
        {
            Texture2D t = texDl.texture;
            Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height),
                                     Vector2.zero, 1f);
            headPortrait.sprite = s;
        }
        
    }
    private void UnLoader(string url)
    {
        byte[] videoByte = File.ReadAllBytes(url);
        WWWForm form = new WWWForm();

        form.AddBinaryData("file", videoByte);
        //Debug.Log(form.data.Length);
       // HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() +"/api/member/update/avatar", form, AmendCall);
       // HttpCallSever.One().PostCallServer("http://192.168.0.116:8080/api/member/update/avatar", form, AmendCall);
    }
    #endregion

    public class Item
    {
        public string category;
        public int type;
        public Item(string ca,int ty)
        {
            this.category = ca;
            this.type = ty;
        }
    }
    public void delect()
    {

        Destroy(this.gameObject);
    }

}
public class ItemClss
{
    public string avatarId;
    public ItemClss(string id)
    {
        this.avatarId = id;
    }
}
public class ItemClas1
{
    public string nickName;
    public ItemClas1(string name)
    {
        this.nickName = name;
    }
}
public class ItemClass2
{
    public int avatarId;
    public ItemClass2(int a)
    {
        this.avatarId = a;
    }
}

