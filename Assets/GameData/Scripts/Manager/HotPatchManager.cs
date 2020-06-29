using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HotPatchManager : Singleton<HotPatchManager>
{
    private bool downloadOver = false;
    private bool loaderLoaclAsset = false;
    private MonoBehaviour m_Mono;
    private string m_UnPackPath = PathInfo.UnPackPath;
    private string m_DownLoadPath = PathInfo.DownLoadPath;
    private string m_CurVersion;
    public string CurVersion
    {
        get { return m_CurVersion; }
    }
    private string m_HotVersion;
    public string HotVersion
    {
        get { return m_HotVersion; }
    }
    private string m_CurPackName;
    private string m_ServerXmlPath = PathInfo.ServerXMLPath;
    private string m_LocalXmlPath = PathInfo.LoaclXMLPath;
    private ServerInfo m_ServerInfo;
    private ServerInfo m_LocalInfo;
    private VersionInfo m_GameVersion;
    //当前热更Patches
    private Pathces m_CurrentPatches;
    public Pathces CurrentPatches
    {
        get { return m_CurrentPatches; }
    }
    //所有热更的东西
    private Dictionary<string, Patch> m_HotFixDic = new Dictionary<string, Patch>();
    //所有需要下载的东西
    private List<Patch> m_DownLoadList = new List<Patch>();
    //所有需要下载的东西的Dic
    private Dictionary<string, Patch> m_DownLoadDic = new Dictionary<string, Patch>();

    //计算需要解压的文件
    private List<string> m_UnPackedList = new List<string>();
    //原包记录的MD5码
    private Dictionary<string, ABMD5Base> m_PackedMd5 = new Dictionary<string, ABMD5Base>();
    //服务器列表获取错误回调
    public Action ServerInfoError;
    //文件下载出错回调
    public Action<string> ItemError;

    public Action HotApk;
    //下载完成回调
    public Action LoadOver;
    //储存已经下载的资源
    public List<Patch> m_AlreadyDownList = new List<Patch>();
    //是否开始下载
    public bool StartDownload = false;
    //尝试重新下载次数
    private int m_TryDownCount = 0;
    private const int DOWNLOADCOUNT = 4;
    //当前正在下载的资源
    private DownLoadAssetBundle m_CurDownload = null;

    private int m_loadFileCount;
    // 需要下载的资源总个数
    public int LoadFileCount
    {
        get { return m_loadFileCount; }
        private set { m_loadFileCount = value; }
    }

    // 需要下载资源的总大小 KB
    private float loadSunSize;
    public float LoadSumSize
    {
        get { return loadSunSize; }
        private set { loadSunSize = value; }
    }
    //是否开始解压
    public bool StartUnPack = false;
    //解压文件总大小
    private float unPackSumSize;
    public float UnPackSumSize
    {
        get { return unPackSumSize; }
        set { unPackSumSize = value; }
    }
    //已解压大小
    private float alreadyUnPackSize;
    public float AlreadyUnPackSize
    {
        get { return alreadyUnPackSize; }
        set { alreadyUnPackSize = value; }
    }
    public void Init(MonoBehaviour mono)
    {
        m_Mono = mono;
        if (!Directory.Exists(m_DownLoadPath))
        {
            Directory.CreateDirectory(m_DownLoadPath);
        }

    }
    //开始更新
    public void CheckVersion(Action<bool> hotCallBack = null)
    {
        m_TryDownCount = 0;
        m_HotFixDic.Clear();
        ReadVersion();
        m_Mono.StartCoroutine(ReadXml(() =>
        {
            if (m_ServerInfo == null)
            {
                TestInfo.Instance.ShowTxt("不为空");
                if (ServerInfoError != null)
                {
                    ServerInfoError();
                }
                return;
            }
            Debug.Log(m_ServerInfo.GameVersion.Length);
            TestInfo.Instance.ShowTxt(m_ServerInfo.GameVersion.Length.ToString());
            foreach (VersionInfo version in m_ServerInfo.GameVersion)
            {
                Debug.Log(m_CurVersion);
                Debug.Log(version.Version);
                if (version.Version == m_CurVersion)
                {

                    m_GameVersion = version;
                    TestInfo.Instance.ShowTxt(m_GameVersion.Pathces[0].Version.ToString());
                    break;
                }
            }
            GetHotAB();
            if (CheckLocalAndServerPatch())
            {
                ComputeDownload();
            }
            LoadFileCount = m_DownLoadList.Count;
            LoadSumSize = m_DownLoadList.Sum(x => x.Size);

            if (hotCallBack != null)
            {
                hotCallBack(m_DownLoadList.Count > 0);
            }
        }));
    }

    
    //更新校验
    private bool CheckLocalAndServerPatch()
    {

        Debug.Log("当前是否查找到loaclXML" + File.Exists(m_LocalXmlPath));
        if (!File.Exists(m_LocalXmlPath))
        {
            Debug.Log("直接热更新");
            return true;
        }
           
        m_LocalInfo = BinarySerializeOpt.XmlDeserialize(m_LocalXmlPath, typeof(ServerInfo)) as ServerInfo;
        VersionInfo localGameVesion = null;

        if (m_LocalInfo != null)
        {
            foreach (VersionInfo version in m_LocalInfo.GameVersion)
            {
                Debug.Log(version.Version);
                Debug.Log(version.Pathces);
                Debug.Log(version.Pathces[0].Version);
                Debug.Log(version.Pathces[0].Des);
                Debug.Log(version.Pathces[0].Files);
                Debug.Log(m_CurVersion);
                if (version.Version == m_CurVersion)
                {
                    Debug.Log("1111111111111111");
                    localGameVesion = version;
                    break;
                }
            }
        }
        Debug.Log("localGameVesion" + localGameVesion != null);
        Debug.Log("m_GameVersion.Pathces.Length" + m_GameVersion.Pathces.Length);
        Debug.Log("localGameVesion.Pathces" + localGameVesion.Pathces.Length);
        Debug.Log("m_GameVersion.Pathces.Length > 0" + (m_GameVersion.Pathces.Length > 0));
        Debug.Log("m_GameVersion.Pathces[m_GameVersion.Pathces.Length - 1].Version != localGameVesion.Pathces[localGameVesion.Pathces.Length - 1].Version" + (m_GameVersion.Pathces[m_GameVersion.Pathces.Length - 1].Version != localGameVesion.Pathces[localGameVesion.Pathces.Length - 1].Version));
        if (localGameVesion != null && m_GameVersion.Pathces != null && localGameVesion.Pathces != null && m_GameVersion.Pathces.Length > 0 && m_GameVersion.Pathces[m_GameVersion.Pathces.Length - 1].Version != localGameVesion.Pathces[localGameVesion.Pathces.Length - 1].Version)
        {
            TestInfo.Instance.ShowTxt("热更新资源");
            return true;
        }
        return false;
    }

    //计算要更新的数据
    private void ComputeDownload()
    {
        m_DownLoadList.Clear();
        m_DownLoadDic.Clear();
        if (m_GameVersion != null && m_GameVersion.Pathces != null && m_GameVersion.Pathces.Length > 0)
        {
            m_CurrentPatches = m_GameVersion.Pathces[m_GameVersion.Pathces.Length - 1];//获得需要热更的路径
            if (m_CurrentPatches.Files != null && m_CurrentPatches.Files.Count > 0)
            {
                foreach (Patch patch in m_CurrentPatches.Files)
                {
                    Debug.Log(Application.platform);
                    Debug.Log(patch.Platform.Contains("StandaloneWindows")); 
                    if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) && patch.Platform.Contains("StandaloneWindows"))
                    {
                        AddDownLoadList(patch);
                    }
                    else if ((Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) && patch.Platform.Contains("Android"))
                    {
                        AddDownLoadList(patch);
                    }
                    else if ((Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WindowsEditor) && patch.Platform.Contains("iOS"))
                    {
                        AddDownLoadList(patch);
                    }
                }
            }
        }
    }
    //添加要更新的数据
    private void AddDownLoadList(Patch patch)
    {

        string filePath = m_DownLoadPath + "/" + patch.Name;
        Debug.Log(filePath);
        if (!File.Exists(filePath))
        {
            m_DownLoadList.Add(patch);
            m_DownLoadDic.Add(patch.Name, patch);
        }
    }

    //获取需要热更的包信息
    private void GetHotAB()
    {
        Debug.Log(m_GameVersion);
        Debug.Log(m_GameVersion.Pathces);
        Debug.Log(m_GameVersion.Pathces.Length);
        
        if (m_GameVersion != null && m_GameVersion.Pathces != null && m_GameVersion.Pathces.Length > 0)
        {
            Pathces lastPatches = m_GameVersion.Pathces[m_GameVersion.Pathces.Length-1];
            if (lastPatches != null && lastPatches.Files != null)
            {
                foreach (Patch patch in lastPatches.Files)
                {
                    m_HotFixDic.Add(patch.Name, patch);
                }
            }
        }
    }
    //下载ServerInfo
    private IEnumerator ReadXml(Action callBack)
    {
        TestInfo.Instance.ShowTxt("开始下载");
        string xmlUrl = "";
#if UNITY_EDITOR_WIN || UNITY_EDITOR ||UNITY_EDITOR_OSX
        xmlUrl = URL.EDITOR_ServerInfo;
#elif UNITY_ANDROID
        xmlUrl = URL.ANDROID_ServerInfo;
#elif UNITY_IOS
        xmlUrl = URL.IOS_ServerInfo;
#endif

        UnityWebRequest webRequest = UnityWebRequest.Get(xmlUrl);
#if UNITY_5_6
        yield return webRequest.Send();
#else
        yield return webRequest.SendWebRequest();
#endif
        if (webRequest.isDone)
        {

            FileTool.CreateFile(m_ServerXmlPath, webRequest.downloadHandler.data);
            TestInfo.Instance.ShowTxt("m_ServerInfo下载成功:" + m_ServerInfo);
            if (File.Exists(m_ServerXmlPath))
            {
                m_ServerInfo = BinarySerializeOpt.XmlDeserialize(m_ServerXmlPath, typeof(ServerInfo)) as ServerInfo;
                m_HotVersion = m_ServerInfo.GameVersion[m_ServerInfo.GameVersion.Length - 1].Pathces[m_ServerInfo.GameVersion[m_ServerInfo.GameVersion.Length - 1].Pathces.Length - 1].Version.ToString();
                TestInfo.Instance.ShowTxt("m_ServerInfo反序列化成功:" + m_ServerInfo);
            }
            else
            {
                Debug.LogError("热更配置读取错误！");
                TestInfo.Instance.ShowTxt("热更配置读取错误！");
            }

        }
        else
        {
            TestInfo.Instance.ShowTxt("Download 失败：" + webRequest.error);
            Debug.Log("Download Error" + webRequest.error);
        }

        if (callBack != null)
        {
            callBack();
        }
    }
    //校验版本
    private void ReadVersion()
    {
        TextAsset versionTex = Resources.Load<TextAsset>("Version");
        if (versionTex == null)
        {
            TestInfo.Instance.ShowTxt("未读到本地版本");
            Debug.LogError("未读到本地版本！");
            return;
        }
        string[] all = versionTex.text.Split('\n');
        if (all.Length > 0)
        {
            string[] infoList = all[0].Split(';');
            if (infoList.Length >= 2)
            {
                m_CurVersion = infoList[0].Split('|')[1];
                m_CurPackName = infoList[1].Split('|')[1];
                Debug.Log("当前版本" + m_CurVersion);
                TestInfo.Instance.ShowTxt("当前版本" + m_CurVersion);
            }
        }
    }
    //开始下载
    internal IEnumerator StartDownLoadAB(Action callBack, List<Patch> allPatch = null)
    {
        m_AlreadyDownList.Clear();
        StartDownload = true;
        if (allPatch == null)
        {
            allPatch = m_DownLoadList;
        }
        if (!Directory.Exists(m_DownLoadPath))
        {
            Directory.CreateDirectory(m_DownLoadPath);
        }
        List<DownLoadAssetBundle> downLoadAssetBundles = new List<DownLoadAssetBundle>();
        for (int i = 0; i < allPatch.Count; i++)
        {
            downLoadAssetBundles.Add(new DownLoadAssetBundle(allPatch[i].Url, m_DownLoadPath));
            downLoadAssetBundles[i].FileLength = (long)allPatch[i].Size;
        }

        foreach (DownLoadAssetBundle downLoad in downLoadAssetBundles)
        {
            if (!File.Exists(PathInfo.DownLoadPath + "/" + downLoad.FileName))
                m_CurDownload = downLoad;
            yield return m_Mono.StartCoroutine(downLoad.Download(() =>
            {

                TestInfo.Instance.ShowTxt("源文件大小：" + downLoad.FileLength);
                TestInfo.Instance.ShowTxt("下载文件大小：" + downLoad.CurLength);
                if (Math.Abs(downLoad.FileLength - downLoad.CurLength) > 1024)
                {
                    File.Delete(PathInfo.DownLoadPath + "/" + downLoad.FileName);
                    TestInfo.Instance.ShowTxt("下载资源失败-----------------------" + downLoad.FileName);
                }
                else
                {
                    if (downLoad.FileLength > 50000)
                    {
                         HotApk();
                    }
                }
            }));
            Patch patch = FindPatchByGamePath(downLoad.FileName);
            if (patch != null)
            {
                m_AlreadyDownList.Add(patch);
            }
            downLoad.Destory();
        }

        if (File.Exists(m_ServerXmlPath))
        {
            if (File.Exists(m_LocalXmlPath))
            {

                File.Delete(m_LocalXmlPath);
            }
            File.Move(m_ServerXmlPath, m_LocalXmlPath);
        }
        callBack();
    }
    //获取path
    private Patch FindPatchByGamePath(string name)
    {
        Patch patch = null;
        m_DownLoadDic.TryGetValue(name, out patch);
        return patch;
    }
    //获取下载进度
    public float GetProgress()
    {
        return GetLoadSize() / LoadSumSize;
    }
    //获取资源大小
    public float GetLoadSize()
    {
        float alreadySize = m_AlreadyDownList.Sum(x => x.Size);
        float curAlreadySize = 0;
        if (m_CurDownload != null)
        {
            Patch patch = FindPatchByGamePath(m_CurDownload.FileName);
            if (patch != null && !m_AlreadyDownList.Contains(patch))
            {
                curAlreadySize = m_CurDownload.GetProcess() * patch.Size;
            }

        }

        return alreadySize + curAlreadySize;
    }
    //资源校验
    public List<Patch> VerifyLoaderAsset()
    {
        if (m_HotFixDic.Count == 0) return null;

        m_DownLoadList.Clear();
        m_DownLoadDic.Clear();
        Debug.Log("下载的资源错误");
        if (m_DownLoadList.Count > 0) return m_DownLoadList;
        Pathces anewLoader = new Pathces();
        anewLoader.Version = 0;
        anewLoader.Des = "正在下载有错误的资源";
        List<Patch> anewPatch = new List<Patch>();
        
        m_LocalInfo = BinarySerializeOpt.XmlDeserialize(m_LocalXmlPath, typeof(ServerInfo)) as ServerInfo;
        Debug.Log(m_LocalInfo);
        List<Patch> patchs = m_LocalInfo.GameVersion[m_LocalInfo.GameVersion.Length - 1].Pathces[m_LocalInfo.GameVersion[m_LocalInfo.GameVersion.Length - 1].Pathces.Length - 1].Files;
        
        for (int i = 0; i < patchs.Count; i++)
        {
            //if (patchs[i].Name == "天胡十三浪.apk") break;
            if (!File.Exists(PathInfo.DownLoadPath + "/" + patchs[i].Name))
            {
                m_DownLoadList.Add(patchs[i]);
                m_DownLoadDic.Add(patchs[i].Name, patchs[i]);
                anewPatch.Add(patchs[i]);
            }
           
        }
        anewLoader.Files = anewPatch;
        m_CurrentPatches = anewLoader;
        LoadSumSize = m_DownLoadList.Sum(x => x.Size);
        Debug.Log(default(List<Patch>));
        return m_DownLoadList.Count > 0 ? m_DownLoadList : default(List<Patch>);
    }
    
}
