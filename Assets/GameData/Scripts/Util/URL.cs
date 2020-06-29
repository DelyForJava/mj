public static class URL
{
    private static string GameVersion = "1.1";
    private static string Head = "http://" + Bridge.GetHostAndPort() + "/";

    //ServerInfo 
    public static readonly string EDITOR_ServerInfo = Head + "upload/up/StandaloneWindows/" + GameVersion + "/ServerInfo.xml";
    public static readonly string ANDROID_ServerInfo = Head + "upload/up/Android/" + GameVersion + "/ServerInfo.xml";
    public static readonly string IOS_ServerInfo = Head + "upload/up/iOS/" + GameVersion + "/ServerInfo.xml";

    //HotFix_Project.dll.txt
    public static readonly string EDITORORWIN_HOTFIX = Head + "upload/up/StandaloneWindows/" + GameVersion + "/HotFix_Project.dll.txt";
    public static readonly string ANDROID_HOTFIX = Head + "upload/up/Android/" + GameVersion + "/HotFix_Project.dll.txt";
    public static readonly string IOS_HOTFIX = Head + "upload/up/iOS/" + GameVersion + "/HotFix_Project.dll.txt";
}
