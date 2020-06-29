using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class MD5Manage : Singleton<MD5Manage>
{
    public string BuildFileMd5(string fliePath)
    {
        string filemd5 = null;
        try
        {
            using (var fileStream = File.OpenRead(fliePath))
            {
                var md5 = MD5.Create();
                var fileMD5Bytes = md5.ComputeHash(fileStream);//计算指定Stream 对象的哈希值                                     
                filemd5 = FormatMD5(fileMD5Bytes);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }
        return filemd5;
    }

    public string FormatMD5(Byte[] data)
    {
        return System.BitConverter.ToString(data).Replace("-", "").ToLower();//将byte[]装换成字符串
    }
}
