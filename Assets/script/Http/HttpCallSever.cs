using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace JX
{
    public class HttpCallSever : MonoBehaviour
    {

        #region Singleton

        private HttpCallSever() { }

        public static HttpCallSever _Instance = null;

        public static HttpCallSever One()
        {
            if (_Instance == null)
            {
                GameObject obj = new GameObject("HttpDriver");
                _Instance = obj.AddComponent<HttpCallSever>();
                DontDestroyOnLoad(obj);
            }
            return _Instance;
        }
        #endregion


        /// <summary>
        /// 用post命令向服务器请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postData">上传的数据</param>
        /// <param name="callback">回调函数</param>
        public void PostCallServer(string url, string postData, Action<string> callback)
        {
            StartCoroutine(PostUrl(url, postData, callback));
        }
        public void PostCallServer(string url, WWWForm form, Action<string> callback)
        {
            StartCoroutine(PostUrl(url, form, callback));
        }
        /// <summary>
        /// 用Get命令向服务器请求
        /// </summary>
        /// <param name="url">地址</param>
        public void GetCallSetver(string url, Action<string> callback)
        {
            StartCoroutine(SendUrl(url, callback));
        }

        public void DownPic(string url, Image pic)
        {
            StartCoroutine(DownLoadPic(url, pic));
        }
        public void DownTextPic(string url, Material pic)
        {
            StartCoroutine(DownLoadPic(url, pic));
        }
        public IEnumerator PostUrl(string url, string postData, Action<string> callback)
        {

            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
            {
                if (!string.IsNullOrEmpty(postData))
                {
                    byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(postData);
                    www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
                }
              
                www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");
                 

                if (!String.IsNullOrEmpty(PlayerPrefs.GetString("UserId.token")))              
                    www.SetRequestHeader("token",PlayerPrefs.GetString("UserId.token"));

                

                yield return www.Send();
                if (www.isNetworkError)
                {
                    Debug.LogError("www.error========" + www.error);
                }
                else
                {
                    if (www.responseCode == 200)
                    {
                        //Debug.Log(www.downloadHandler.text);
                        callback(www.downloadHandler.text);
                    }
                    else
                    {
                        //Debug.Log(www.downloadHandler.text);
                    }
                }
            }
        }

        private IEnumerator PostUrl(string url, WWWForm form, Action<string> getResult)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                if (!String.IsNullOrEmpty(PlayerPrefs.GetString("UserId.token")))
                {
                    // www.SetRequestHeader("token",/*Bridge._instance.token*/UserId.token);
                    www.SetRequestHeader("token",/*Bridge._instance.token*/PlayerPrefs.GetString("UserId.token"));
                }

                yield return www.Send();

                if (www.isNetworkError)
                {
                    yield return www.error;
                }
                else
                {
                    getResult(www.downloadHandler.text);
                    if (www.responseCode == 200)
                    {

                    }
                }
            }
        }

        private IEnumerator SendUrl(string url, Action<string> callback)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                www.SetRequestHeader("token",/*Bridge._instance.token*/PlayerPrefs.GetString("UserId.token"));
                yield return www.Send();
                if (www.error != null)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    if (www.responseCode == 200)//200表示接受成功
                    {
                        callback(www.downloadHandler.text);
                    }
                    else
                    {
                        Debug.Log(www.downloadHandler.text);
                    }
                }
            }
        }

        private static WWWForm CreatePostData(Dictionary<string, object> formData)
        {
            WWWForm form = new WWWForm();
            if (formData != null && formData.Count > 0)
            {
                foreach (var item in formData)
                {
                    if (item.Value is byte[])
                        form.AddBinaryData(item.Key, item.Value as byte[]);
                    else
                        form.AddField(item.Key, item.Value.ToString());
                }
            }
            return form;
        }

        public IEnumerator DownLoadPic(string url, Image myRaw)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.Send();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
                yield break;
            }
            if (www.isDone)
            {
                Texture2D textture = DownloadHandlerTexture.GetContent(www);
                if (textture != null)
                {
                    Sprite sprite = Sprite.Create(textture, new Rect(0, 0, textture.width, textture.height), Vector2.zero);
                    if (myRaw != null)
                    {
                        myRaw.sprite = sprite;
                        //myRaw.SetNativeSize();
                    }
                }
            }
        }
        public IEnumerator DownLoadPic(string url, Material myRaw)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.Send();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
                yield break;
            }
            if (www.isDone)
            {
                Texture2D textture = DownloadHandlerTexture.GetContent(www);
                if (textture != null)
                {
                    if (myRaw != null)
                    {
                        myRaw.mainTexture = textture;
                    }
                }
            }
        }
    }
}






