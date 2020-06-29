﻿#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class PrefabSetFont
{
    ////替换场景内的所有字体
    //[MenuItem("Tools/PrefabSetFont")]
    //public static void ChangeAllFontOfScene()
    //{
    //    //加载目标字体
    //    Font targetFont = Resources.Load<Font>("Lato");
    //    //获取场景所有激活物体
    //    //GameObject[] objs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
    //    //获取场景所有物体
    //    GameObject[] allObj = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
    //    Text tmpText;
    //    int textCount = 0;
    //    for (int i = 0; i < allObj.Length; i++)
    //    {
    //        //带有Text组件的GameObject，替换字体
    //        tmpText = allObj[i].GetComponent<Text>();
    //        if (tmpText != null)
    //        {
    //            textCount++;
    //            tmpText.font = targetFont;
    //            //在此扩展，可以给添加外边框，也可以根据需求进行其他操作
    //            allObj[i].AddComponent<Outline>();
    //        }
    //    }
    //    Debug.Log("<color=green> 当前场景共有：物体 </color>" + allObj.Length + "<color=green> 个，Text组件 </color>" + textCount + "<color=green> 个 </color>");
    //}
    //替换资源文件夹中全部Prefab的字体
    [MenuItem("Tools/PrefabSetFont")]
    public static void ChangeAllFontOfPrefab()
    {
        Font targetFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        List<Text[]> textList = new List<Text[]>();
        Dictionary<string,GameObject> copies = new Dictionary<string, GameObject>();
        //获取Asset文件夹下所有Prefab的GUID
        string[] ids = AssetDatabase.FindAssets("t:Prefab");
        string tmpPath;
        GameObject tmpObj;
        Text[] tmpArr;
        for (int i = 0; i < ids.Length; i++)
        {
            tmpObj = null;
            tmpArr = null;
            //根据GUID获取路径
            tmpPath = AssetDatabase.GUIDToAssetPath(ids[i]);
            if (!string.IsNullOrEmpty(tmpPath))
            {
                //根据路径获取Prefab(GameObject)
                tmpObj = AssetDatabase.LoadAssetAtPath(tmpPath, typeof(GameObject)) as GameObject;
                GameObject copyObj = PrefabUtility.InstantiatePrefab(tmpObj) as GameObject;
                if (copyObj != null)
                {
                    //获取Prefab及其子物体孙物体···的所有Text组件
                    tmpArr = copyObj.GetComponentsInChildren<Text>(true);
                    if (tmpArr != null && tmpArr.Length > 0)
                        textList.Add(tmpArr);

                    copies.Add(tmpPath, copyObj);
                }
            }
        }
        //替换所有Text组件的字体
        int textCount = 0;
        for (int i = 0; i < textList.Count; i++)
        {
            for (int j = 0; j < textList[i].Length; j++)
            {
                textCount++;
                textList[i][j].font = targetFont;
            }
        }

        foreach (var kv in copies)
        {
            var newprefab = PrefabUtility.CreateEmptyPrefab(kv.Key);
            PrefabUtility.ReplacePrefab(kv.Value,newprefab);
        }
        foreach (var kv in copies)
        {
            GameObject.DestroyImmediate(kv.Value);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("<color=green> 当前ProJect共有：Prefab </color>" + ids.Length + "<color=green> 个，带有Text组件Prefab </color>" + textList.Count + "<color=green> 个，Text组件 </color>" + textCount + "<color=green> 个 </color>");
    }
}
#endif