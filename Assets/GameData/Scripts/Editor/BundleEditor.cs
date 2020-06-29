using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Profiling;
using System;

public class BundleEditor
{
    private static Dictionary<string, ABMD5Base> m_PackedMd5 = new Dictionary<string, ABMD5Base>();
    private static string m_HotPath = Application.dataPath + "/../Hot/" + EditorUserBuildSettings.activeBuildTarget.ToString();
    private static string m_BunleTargetPath = Application.dataPath + "/../AssetBundle/" + EditorUserBuildSettings.activeBuildTarget.ToString();
    //过滤的list
    private static List<string> m_AllFileAB = new List<string>();
    //储存所有有效路径
    private static List<string> m_ConfigFil = new List<string>();
    //单个prefab的ab包
    private static Dictionary<string, List<string>> m_AllPrefabDir = new Dictionary<string, List<string>>();
    //key是ab包名，value是路径，所有文件夹ab包dic
    private static Dictionary<string, string> m_AllFileDir = new Dictionary<string, string>();
    private static string ABCOGFIGPATH = "Assets/GameData/Scripts/Config/ABConfig.asset";
    private static string m_VersionMd5Path = Application.dataPath + "/../Version/" + EditorUserBuildSettings.activeBuildTarget.ToString();
    private static string ABBYTEPATH = "Assets/GameData/Data/AssetBundleConfig.bytes";
    private static bool hotfix = false;
    private static string hotCount = "1";


    [MenuItem("Tools/打包")]
    public static void Build()
    {
        m_ConfigFil.Clear();
        m_AllFileAB.Clear();
        m_AllFileDir.Clear();
        m_AllPrefabDir.Clear();
        ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(ABCOGFIGPATH);
        Debug.Log(abConfig);
        foreach (ABConfig.FileDirABName item in abConfig.m_AllFileDirAB)
        {
            if (m_AllFileDir.ContainsKey(item.ABName))
            {
                Debug.LogError("AB包配置名字重复");
            }
            else
            {
                m_AllFileDir.Add(item.ABName, item.Path);
                m_AllFileAB.Add(item.Path);
                m_ConfigFil.Add(item.Path);
            }
        }

        string[] allStr = AssetDatabase.FindAssets("t:Prefab", abConfig.m_AllPrefabPath.ToArray());
        for (int i = 0; i < allStr.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
            EditorUtility.DisplayProgressBar("查找Prefab", "Prefab:" + path, i * 1.0f / allStr.Length);

            if (!ContainAllFileAB(path))
            {
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                string[] allDepend = AssetDatabase.GetDependencies(path);
                List<string> allDependPath = new List<string>();
                for (int j = 0; j < allDepend.Length; j++)
                {
                    if (!allDepend[j].EndsWith(".cs"))
                    {
                        m_AllFileAB.Add(allDepend[j]);
                        allDependPath.Add(allDepend[j]);
                    }
                }
                if (m_AllPrefabDir.ContainsKey(obj.name))
                {
                    Debug.LogError("存在相同名字的Prefab:" + obj.name);
                }
                else
                {
                    m_AllPrefabDir.Add(obj.name, allDependPath);
                }
            }
        }
        foreach (string name in m_AllFileDir.Keys)
        {

            SetABName(name, m_AllFileDir[name]);
        }

        foreach (string name in m_AllPrefabDir.Keys)
        {
            SetABName(name, m_AllPrefabDir[name]);
        }

        BunildAssetBundle();

        string[] oldABNames = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < oldABNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(oldABNames[i], true);
            EditorUtility.DisplayProgressBar("清除AB包名", "名字：" + oldABNames[i], i * 1.0f / oldABNames.Length);
        }

        GeneratXML();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }
    //打包
    private static void BunildAssetBundle()
    {
        string[] allBundles = AssetDatabase.GetAllAssetBundleNames();
        //key为全路径，value为包名
        Dictionary<string, string> resPathDic = new Dictionary<string, string>();
        for (int i = 0; i < allBundles.Length; i++)
        {
            string[] allBundlePath = AssetDatabase.GetAssetPathsFromAssetBundle(allBundles[i]);
            for (int j = 0; j < allBundlePath.Length; j++)
            {
                if (allBundlePath[j].EndsWith(".cs"))
                    continue;

                resPathDic.Add(allBundlePath[j], allBundles[i]);
            }
        }

        if (!Directory.Exists(m_BunleTargetPath))
        {
            Directory.CreateDirectory(m_BunleTargetPath);
        }

        DeleteAB();
        //WriteData(resPathDic);
        SaveVersion(PlayerSettings.bundleVersion, PlayerSettings.applicationIdentifier);
        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(m_BunleTargetPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        if (manifest == null)
        {
            Debug.LogError("AssetBundle 打包失败！");
        }
        else
        {
            Debug.Log("AssetBundle 打包完毕");
        }
        DeleteMainfest();
    }

    static void WriteData(Dictionary<string, string> resPathDic)
    {
        AssetBundleConfig config = new AssetBundleConfig();
        config.ABList = new List<ABBase>();
        foreach (string path in resPathDic.Keys)
        {
            if (!ValidPath(path))
                continue;

            ABBase abBase = new ABBase();
            abBase.Path = path;
            abBase.ABName = resPathDic[path];
            abBase.AssetName = path.Remove(0, path.LastIndexOf("/") + 1);
            abBase.ABDependce = new List<string>();
            string[] resDependce = AssetDatabase.GetDependencies(path);
            for (int i = 0; i < resDependce.Length; i++)
            {
                string tempPath = resDependce[i];
                if (tempPath == path || path.EndsWith(".cs"))
                    continue;

                string abName = "";
                if (resPathDic.TryGetValue(tempPath, out abName))
                {
                    if (abName == resPathDic[path])
                        continue;

                    if (!abBase.ABDependce.Contains(abName))
                    {
                        abBase.ABDependce.Add(abName);
                    }
                }
            }
            config.ABList.Add(abBase);
        }

        //写入xml
        string xmlPath = Application.dataPath + "/AssetbundleConfig.xml";
        if (File.Exists(xmlPath)) File.Delete(xmlPath);
        FileStream fileStream = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xs = new XmlSerializer(config.GetType());
        xs.Serialize(sw, config);
        sw.Close();
        fileStream.Close();

        //写入二进制
        foreach (ABBase abBase in config.ABList)
        {
            abBase.Path = "";
        }
        FileStream fs = new FileStream(ABBYTEPATH, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        fs.Seek(0, SeekOrigin.Begin);
        fs.SetLength(0);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, config);
        fs.Close();
        AssetDatabase.Refresh();
        SetABName("assetbundleconfig", ABBYTEPATH);
    }

    private static bool ValidPath(string path)
    {
        for (int i = 0; i < m_ConfigFil.Count; i++)
        {
            if (path.Contains(m_ConfigFil[i]))
            {
                return true;
            }
        }
        return false;
    }

    //校验AB包
    static bool ContainAllFileAB(string path)
    {
        for (int i = 0; i < m_AllFileAB.Count; i++)
        {
            if (path == m_AllFileAB[i] || (path.Contains(m_AllFileAB[i]) && (path.Replace(m_AllFileAB[i], "")[0] == '/')))
                return true;
        }

        return false;
    }
    //设置AB包的名字
    static void SetABName(string name, string path)
    {
        AssetImporter assetImporter = AssetImporter.GetAtPath(path);
        if (assetImporter == null)
        {
            Debug.LogError("不存在此路径文件：" + path);
        }
        else
        {
            assetImporter.assetBundleName = name;
        }
    }

    static void SetABName(string name, List<string> paths)
    {
        for (int i = 0; i < paths.Count; i++)
        {
            SetABName(name, paths[i]);
        }
    }
    /// <summary>
    /// 删除无用AB包
    /// </summary>
    static void DeleteAB()
    {
        string[] allBundlesName = AssetDatabase.GetAllAssetBundleNames();
        DirectoryInfo direction = new DirectoryInfo(m_BunleTargetPath);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (ConatinABName(files[i].Name, allBundlesName) || files[i].Name.EndsWith(".meta") || files[i].Name.EndsWith(".manifest") || files[i].Name.EndsWith("assetbundleconfig"))
            {
                continue;
            }
            else
            {
                Debug.Log("此AB包已经被删或者改名了：" + files[i].Name);
                if (File.Exists(files[i].FullName))
                {
                    File.Delete(files[i].FullName);
                }
                if (File.Exists(files[i].FullName + ".manifest"))
                {
                    File.Delete(files[i].FullName + ".manifest");
                }
            }
        }
    }

    /// <summary>
    /// 遍历文件夹里的文件名与设置的所有AB包进行检查判断
    /// </summary>
    /// <param name="name"></param>
    /// <param name="strs"></param>
    /// <returns></returns>
    static bool ConatinABName(string name, string[] strs)
    {
        for (int i = 0; i < strs.Length; i++)
        {
            if (name == strs[i])
                return true;
        }
        return false;
    }
    //删除文件
    static void DeleteMainfest()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(m_BunleTargetPath);
        FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".manifest"))
            {
                File.Delete(files[i].FullName);
            }
        }
    }
    //生成AB包
    static void GeneratXML()
    {
        CopyAssetToHotPath();
        //生成服务器Patch
        DirectoryInfo directory = new DirectoryInfo(m_HotPath);
        FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
        Pathces pathces = new Pathces();
        pathces.Version = 1;
        pathces.Files = new List<Patch>();
        for (int i = 0; i < files.Length; i++)
        {
            Patch patch = new Patch();
            patch.Md5 = MD5Manage.Instance.BuildFileMd5(files[i].FullName);
            patch.Name = files[i].Name;
            patch.Size = files[i].Length / 1024.0f;
            patch.Platform = EditorUserBuildSettings.activeBuildTarget.ToString();
            patch.Url = "http://" + Bridge.GetHostAndPort() +"/upload/up/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/" + PlayerSettings.bundleVersion + "/" + files[i].Name;
            Debug.Log("url=====" + patch.Url);
            //patch.Url = "http://nanchang.3vftp.com/" + files[i].Name;
            pathces.Files.Add(patch);
        }
        BinarySerializeOpt.Xmlserialize(m_HotPath + "/Patch.xml", pathces);
    }
    public static void DeleteAllFile(string fullPath)
    {
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo directory = new DirectoryInfo(fullPath);
            FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                File.Delete(files[i].FullName);
            }
        }
    }
    //复制AB包到HOt文件夹下
    public static void CopyAssetToHotPath()
    {
        if (!Directory.Exists(m_HotPath))
        {
            Directory.CreateDirectory(m_HotPath);
        }

        DeleteAllFile(m_HotPath);
        DirectoryInfo directory = new DirectoryInfo(m_BunleTargetPath);
        FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].Name.EndsWith(".meta") && !files[i].Name.EndsWith(".manifest"))
            {
                File.Copy(m_BunleTargetPath + "/" + files[i].Name, m_HotPath + "/" + files[i].Name);
            }
        }
    }
    //保存版本信息
    static void SaveVersion(string version, string package)
    {
        string content = "Version|" + version + ";PackageName|" + package + ";";
        string savePath = Application.dataPath + "/Resources/Version.txt";
        string oneLine = "";
        string all = "";
        using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
            {
                all = sr.ReadToEnd();
                oneLine = all.Split('\r')[0];
            }
        }
        using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate))
        {
            using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
            {
                if (string.IsNullOrEmpty(all))
                {
                    all = content;
                }
                else
                {
                    all = all.Replace(oneLine, content);
                }
                sw.Write(all);
            }
        }
    }

}
