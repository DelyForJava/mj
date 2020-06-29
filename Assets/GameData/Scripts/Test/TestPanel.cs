using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Testbaifu
{
    private string UploadRequest(string url, string path, string name)
    {
        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        CookieContainer cookieContainer = new CookieContainer();
        request.CookieContainer = cookieContainer;
        request.AllowAutoRedirect = true;
        request.MaximumResponseHeadersLength = 1024;
        request.Method = "POST";
        string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
        request.ContentType = "multipart/form-data;charset=utf-8;boundary=----WebKitFormBoundary" + boundary;
        byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n------WebKitFormBoundary" + boundary + "\r\n");
        byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n------WebKitFormBoundary" + boundary + "--\r\n");
        int pos = path.LastIndexOf("\\");
        string fileName = path.Substring(pos + 1);
        //请求头部信息 
        StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
        StringBuilder sbHeader2 = new StringBuilder("Content-Disposition:form-data;name=\"name\"\r\n\r\n");
        byte[] namebyte = Encoding.UTF8.GetBytes(name);
        byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());
        byte[] postHeaderBytes2 = Encoding.UTF8.GetBytes(sbHeader2.ToString());
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] bArr = new byte[fs.Length];
        fs.Read(bArr, 0, bArr.Length);
        fs.Close();
        Stream postStream = request.GetRequestStream();
        postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
        postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
        postStream.Write(bArr, 0, bArr.Length);
        postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
        postStream.Write(postHeaderBytes2, 0, postHeaderBytes2.Length);
        postStream.Write(namebyte, 0, namebyte.Length);
        postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
        string header = "\r\n----WebKitFormBoundary" + boundary + "\r\n" + sbHeader.ToString() + " xx " + "\r\n----WebKitFormBoundary" + boundary + "\r\n" + sbHeader2.ToString() + "test234" + "\r\n----WebKitFormBoundary" + boundary + "--\r\n";
        postStream.Close();
        HttpWebResponse res;
        try
        {
            res = (HttpWebResponse)request.GetResponse();
        }
        catch (WebException ex)
        {
            res = (HttpWebResponse)ex.Response;
        }
        StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
        string content = sr.ReadToEnd();
        return content;
    }
}


