using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Textureload : MonoBehaviour
{
    public string url = "https://bchy.oss-cn-qingdao.aliyuncs.com/Work/5_2_0.png";
    public GameObject go;
    string path;
    void Start()
    {
        StartCoroutine(UploadPNG("5_2_0.png", "Work"));
    }

    /// <summary>
    /// 下载图片
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private IEnumerator UploadPNG(string fileName, string dic)
    {
        WWW www = new WWW(url);
        yield return www;
        byte[] bytes = www.texture.EncodeToPNG();
        path = PathForFile(fileName, dic);//移动平台的判断

        print("文件" + path);

        SaveNativeFile(bytes, path);//保存图片到本地
    }

    /// <summary>
    /// 判断平台
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public string PathForFile(string filename, string dic)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            path = Path.Combine(path, "Documents");
            path = Path.Combine(path, dic);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, filename);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            path = Path.Combine(path, dic);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, filename);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            path = Path.Combine(path, dic);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, filename);
        }
    }
    /// <summary>
    /// 在本地保存文件
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="path"></param>
    public void SaveNativeFile(byte[] bytes, string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        fs.Write(bytes, 0, bytes.Length);
        fs.Flush();
        fs.Close();
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(200, 200, 120, 30), "Click"))
        {
            ShowNativeTexture();
        }
    }

    /// <summary>
    /// 显示图片
    /// </summary>
    public void ShowNativeTexture()
    {
        go.GetComponent<MeshRenderer>().material.mainTexture = GetNativeFile(path);
    }
    /// <summary>
    /// 获取到本地的图片
    /// </summary>
    /// <param name="path"></param>
    public Texture2D GetNativeFile(string path)
    {
        try
        {
            var pathName = path;
            var bytes = ReadFile(pathName);
            int width = Screen.width;
            int height = Screen.height;
            var texture = new Texture2D(width, height);
            texture.LoadImage(bytes);
            return texture;
        }
        catch (Exception c)
        {


        }
        return null;
    }
    public byte[] ReadFile(string filePath)
    {
        var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        fs.Seek(0, SeekOrigin.Begin);
        var binary = new byte[fs.Length];
        fs.Read(binary, 0, binary.Length);
        fs.Close();
        return binary;
    }
}
