using ILRuntime.Mono.Cecil;
using JX;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoSonPanel : MonoBehaviour {
    public Button close;
    public Toggle amendPictureFrameToggle;
    public Toggle amendHeadPortraitToggle;

    List<GameObject> itemObjs = new List<GameObject>();
    List<Item> items = new List<Item>();
    void Start () {
        HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/goods/owned", "{}", ShowItem);

        amendPictureFrameToggle.onValueChanged.AddListener((isOn) =>
        {
            amendHeadPortraitToggle.isOn = !isOn;
            if(isOn)
                HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/goods/owned", "{}", ShowItem);
        });
        amendHeadPortraitToggle.onValueChanged.AddListener((isOn) =>
        {
            amendPictureFrameToggle.isOn = !isOn;
            if(isOn)
                HttpCallSever.One().GetCallSetver("http://" + Bridge.GetHostAndPort() + "/api/member/avatar/system", ShowHeadPortrait);
        });
        close.onClick.AddListener(() =>
        {
            Destroy(this.gameObject);
        });
    }
   
    
    private void ShowItem(string data)
    {
        foreach (var item in itemObjs)
        {
            Destroy(item.gameObject);
        }
        itemObjs.Clear();
        items.Clear();
        JsonData json = JsonMapper.ToObject(data);
        if ((int)json["code"] == 200)
        {
            for (int i = 0; i < json["data"]["goods"].Count; i++)
            {
                string category = (string)json["data"]["goods"][i]["category"];
                if (category == "head")
                {
                    int type = (int)json["data"]["goods"][i]["type"];
                    items.Add(new Item(category, type));
                }
            }
            ShowItem();
        }
        else if ((int)json["code"] == 300)
        {
            Debug.Log("修改成功！");
        }
        else
        {
            Debug.Log("修改失败！");
        }
    }
    private void ShowItem()
    {
        foreach (var item in items)
        {

            GameObject itemObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/Item"), GameObject.Find("Content").gameObject.transform);
            itemObjs.Add(itemObj);
            itemObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(string.Format("shop/head_{0}", item.type));
            itemObj.gameObject.name = (item.type).ToString();
            itemObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log(item.type);
                //string jsonStr = JsonConvert.SerializeObject(new Dictionary<string, int>() { { "avatarId", item.type } });
                HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/member/update/headframe", JsonMapper.ToJson(new ItemClass2(item.type)), Call); ;
                UserId.PictureFrame = item.type;
                UiController._instance.SetPictureFrame(item.type);
            });
        }

    }
    private void ShowHeadPortrait(string data)
    {
        Debug.Log(data);
        Debug.Log(items.Count);
        Debug.Log(itemObjs.Count);
        foreach (var item in itemObjs)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < JsonMapper.ToObject(data)["data"]["avatarList"].Count; i++)
        {
            GameObject itemObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/Item"), GameObject.Find("Content").gameObject.transform);
            itemObjs.Add(itemObj);
            HttpCallSever.One().DownPic((string)JsonMapper.ToObject(data)["data"]["avatarList"][i], itemObj.GetComponent<Image>());
            itemObj.name = (i + 1).ToString();
            itemObj.GetComponent<Button>().onClick.AddListener(() => {
                string url = string.Format("http://" + Bridge.GetHostAndPort() + "/images/avatar_{0}.png", itemObj.name);
                //string jsonStr = JsonConvert.SerializeObject(new Dictionary<string, int>() { { "avatarId", int.Parse(itemObj.name) } });
                HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/member/update/avatar/system", JsonMapper.ToJson(new ItemClss(itemObj.name)), Call);
                HttpCallSever.One().DownPic(string.Format("http://" + Bridge.GetHostAndPort() + "/images/avatar_{0}.png", itemObj.name), GameObject.Find("Quad").transform.Find("tx").GetComponent<Image>());
                UserId.avatar = url;
            });
        }

    }

    private void Call(string obj)
    {
        
    }

    public class Item
    {
        public string category;
        public int type;
        public Item(string ca, int ty)
        {
            this.category = ca;
            this.type = ty;
        }
    }
}
