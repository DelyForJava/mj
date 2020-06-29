using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DownLoadAssetBundle : DownloadItem
{
    UnityWebRequest m_WebRequest;

    public DownLoadAssetBundle(string url, string path) : base(url, path)
    {

    }

    public override IEnumerator Download(Action callback = null)
    {
        m_WebRequest = UnityWebRequest.Get(m_Url);
        m_StartDownLoad = true;
        m_WebRequest.timeout = 30000;
        yield return m_WebRequest.Send();
        m_StartDownLoad = false;
        if (m_WebRequest.isDone)
        {
            byte[] bytes = m_WebRequest.downloadHandler.data;
            FileTool.CreateFile(m_SaveFilePath, bytes);
            m_CurLength = m_WebRequest.downloadHandler.data.Length;
            if (callback != null)
            {
                callback();
            }
        }
        else
        {
            Debug.LogError("Download Error" + m_WebRequest.error);
        }
    }

    public override void Destory()
    {
        if (m_WebRequest != null)
        {
            m_WebRequest.Dispose();
            m_WebRequest = null;
        }
    }

    public override long GetCurLength()
    {
        if (m_WebRequest != null)
        {
            return (long)m_WebRequest.downloadedBytes;
        }
        return 0;
    }

    public override long GetLength()
    {
        return 0;
    }

    public override float GetProcess()
    {
        if (m_WebRequest != null)
        {
            return m_WebRequest.downloadProgress;
        }
        return 0.0f;
    }
}