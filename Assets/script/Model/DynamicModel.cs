using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public class DynamicModel : MonoBehaviour {
    
    public static PaiHangBangClass ReadAll(string jsonData)
    {
        //读文件，取JSON
        //JSON -> C#
        string json = jsonData;      
        return JsonMapper.ToObject<PaiHangBangClass>(json);
    }

}
