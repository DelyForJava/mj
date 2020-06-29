using System;
using UnityEngine;

public static class PathInfo
{
    public static readonly string PersistentDataPath = Application.persistentDataPath;
    public static readonly string UnPackPath = PersistentDataPath + "/Origin";
    public static readonly string DownLoadPath = PersistentDataPath + "/DownLoad";

    public static readonly string ServerXMLPath = PersistentDataPath + "/ServerInfo.xml";
    public static readonly string LoaclXMLPath = PersistentDataPath + "/LocalInfo.xml";

    public static readonly string HotFix_Project = PersistentDataPath + "/DownLoad";



    public static readonly string LoaderPanel = "LoaderPanel.prefab";
    public static readonly string HotFixPanel = "HotPanel.prefab";

    public static readonly string TestScene = "Test";
}
