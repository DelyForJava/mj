using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JX;
using LitJson;
using System;
using JetBrains.Annotations;

public class ShopController : MonoBehaviour {

    public Text goldText;
    public Text diamondText;

    public enum Status
    {
        ware,
        face,
        tx,
        gift
    }

    private JsonData shopdata;
    private JsonData giftdata;
    private JsonData facedata;
    private JsonData txkdata;
    private JsonData waredata;

   // private GameObject prf;
    private Transform bg;
    private Transform grid;

    //已拥有的物品
    private List<int> Own;

    //当前正在购买的物品
    private GameObject currentBuyShop;
    //当前正在购买物品的ID
    private int currentBuyShopID;

    /// <summary>
    /// 初始化
    /// </summary>
    private void Start()
    {
        giftdata = new JsonData();
        facedata = new JsonData();
        txkdata = new JsonData();
        waredata = new JsonData();

        Own = new List<int>();

        bg = transform.Find("bg").transform;
        grid = bg.transform.Find("Scroll View/grid").transform;
        AddButtFunc();   //左边切换按钮 添加button方法

        //获取商城物品
        RefreshGoldItem();
        RefreshDiamondItem();
        GetShops();
    }


    void GetShops() {
        //获取商城商品
        string url = "http://" + Bridge.GetHostAndPort() + "/api/goods/shop";
        HttpCallSever.One().GetCallSetver(url, GetShopContent);

        //获取账号已拥有物品
        GetOwns();
    }

    //获取账号已拥有物品
    void GetOwns() {       
        Member m = new Member();
        m.memberId = UserId.memberId.ToString();
        string json = JsonMapper.ToJson(m);
        JX.HttpCallSever.One().PostCallServer("http://"+Bridge.GetHostAndPort()+"/api/goods/owned", json, owncallback);
    }



    //已拥有的物品
    void owncallback(string JsonData) {
        GoodsDataInfo goods = JsonMapper.ToObject<GoodsDataInfo>(JsonData);
        for (int i = 0; i < goods.data.goods.Count; i++)
        {       
           Own.Add(goods.data.goods[i].shopGoodsId);
        }
    }

    /// <summary>
    /// 左侧类别切换 button方法
    /// </summary>
    void AddButtFunc() {

        bg.Find("Type/gold").gameObject.GetComponent<Toggle>().onValueChanged.AddListener((bool isOn) => {

            bg.Find("Type/gift").gameObject.GetComponent<Toggle>().isOn = false;
            bg.Find("Type/face").gameObject.GetComponent<Toggle>().isOn = false;
            bg.Find("Type/txk").gameObject.GetComponent<Toggle>().isOn = false;

            DelectChilden(bg.Find("Scroll View/grid").transform);
            //SelectContent(Status.ware);
            Assignment(waredata);

        });

        bg.Find("Type/gift").gameObject.GetComponent<Toggle>().onValueChanged.AddListener((bool isOn) => {

            bg.Find("Type/gold").gameObject.GetComponent<Toggle>().isOn = false;
            bg.Find("Type/face").gameObject.GetComponent<Toggle>().isOn = false;
            bg.Find("Type/txk").gameObject.GetComponent<Toggle>().isOn = false;
            DelectChilden(bg.Find("Scroll View/grid").transform);
            //SelectContent(Status.gift);
            Assignment(giftdata);
        });

        bg.Find("Type/face").gameObject.GetComponent<Toggle>().onValueChanged.AddListener((bool isOn) => {

            bg.Find("Type/gold").gameObject.GetComponent<Toggle>().isOn = false;
            bg.Find("Type/gift").gameObject.GetComponent<Toggle>().isOn = false;
            bg.Find("Type/txk").gameObject.GetComponent<Toggle>().isOn = false;
            DelectChilden(bg.Find("Scroll View/grid").transform);
            //SelectContent(Status.face);
            Assignment(facedata);
        });

        bg.Find("Type/txk").gameObject.GetComponent<Toggle>().onValueChanged.AddListener((bool isOn) => {

            bg.Find("Type/gift").gameObject.GetComponent<Toggle>().isOn = false;
            bg.Find("Type/face").gameObject.GetComponent<Toggle>().isOn = false;
            bg.Find("Type/gold").gameObject.GetComponent<Toggle>().isOn = false;
            DelectChilden(bg.Find("Scroll View/grid").transform);
            //SelectContent(Status.tx);
            Assignment(txkdata);
        });

        transform.Find("close").gameObject.GetComponent<Button>().onClick.AddListener(()=> {
            Audiocontroller.Instance.PlayAudio("Back");
            Destroy(this.gameObject);
        });
    }


    /// <summary>
    /// //删除子物体方法
    /// </summary>
    /// <param name="tra"></param>
    private void DelectChilden(Transform tra) {
        if (tra.childCount != 0)
        {
            for (int i = 0; i < tra.childCount; i++)
            {
                Destroy(tra.GetChild(i).gameObject);
            }
        }
    }


    /// <summary>
    /// 从服务器获取商城商品信息回调方法
    /// </summary>
    /// <param name="json"></param>
    private void GetShopContent(string json) {

        JsonData data = JsonMapper.ToObject(json);

        if ((int)data["code"] == 200)
        {
            shopdata = data["data"]["shop"];

            DelectChilden(bg.Find("Scroll View/grid").transform);
            SelectContent();
        }
    }


  /// <summary>
  /// 筛选数据 类型
  /// </summary>
    private void SelectContent() {

        for (int i = 0; i < shopdata.Count; i++)
        {       
               if (shopdata[i]["category"].ToString() == "goldcount")
               {
                   waredata.Add(shopdata[i]);
               }

               if (shopdata[i]["category"].ToString() == "brow")
               {                     
                  facedata.Add(shopdata[i]);
               }
                                  
               if (shopdata[i]["category"].ToString() == "gift")
               {
                    giftdata.Add(shopdata[i]);                        
               }
              
               if (shopdata[i]["category"].ToString() == "head")
               {
                    txkdata.Add(shopdata[i]);
               }
        }

        Assignment(waredata);   //初始 默认商品 (银两兑换)
    }

 


  /// <summary>
  /// 给每个商品添加图片和购买事件
  ///yl </summary>
  /// <param name="data"></param>
    private void Assignment(JsonData data) {
     
       //Debug.Log("data.Count===" + data.Count);     
       for (int i = 0; i < data.Count; i++)
       {
            //新手礼包里面有的不在售卖
            if ((int)data[i]["id"] == 7 || (int)data[i]["id"] == 20)
            {
                continue;
            }
            GameObject obj= Bridge._instance.LoadAbDate(LoadAb.MainTwo, "ShopItem",grid);
            obj.name = "ShopItem" + i;
            
            //判断当前物品是否拥有
            bool isown = false;
            for (int j = 0; j < Own.Count; j++)
            {
                if (Own[j]==(int)data[i]["id"])
                {
                    isown = true;
                }
            }

            //给已拥有物品和商品添加不同信息和点击事件
            if (isown)
            {
                obj.transform.Find("price").GetComponent<Text>().text = "已拥有";
            }
            else
            {
                obj.transform.Find("price").GetComponent<Text>().text = data[i]["price"].ToString();

                int shopID = (int)data[i]["id"];
                obj.transform.Find("price").GetComponent<Button>().onClick.AddListener(() =>
                {
                    Bridge._instance.LoadAbDate(LoadAb.Login, "Tool");
                    Tool._instance.ShowTool("确定兑换吗!", delegate () { BuyFunc(shopID,obj); });
                });
            }
           
            //头像框
            if (data[i]["category"].ToString()=="head")
            {
                obj.transform.Find("Text").GetComponent<Text>().text = data[i]["name"].ToString()+"x1";
                //obj.transform.Find("Text").GetComponent<Text>().fontSize = 26;
                obj.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop/" + data[i]["category"] + "_" + data[i]["type"]);
                //obj.transform.Find("Image").GetComponent<Image>().SetNativeSize();
                //obj.transform.Find("Image").localScale =new Vector3(0.8f,0.8f);
                obj.transform.Find("bg/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop/price_gold");
            }
            //表情包
            else if (data[i]["category"].ToString() == "brow")          
            {
                obj.transform.Find("Text").GetComponent<Text>().text = data[i]["name"].ToString()+"x1";
                //obj.transform.Find("Text").GetComponent<Text>().fontSize = 26;
                obj.transform.Find("Image").GetComponent<Image>().sprite= Resources.Load<Sprite>("shop/" + data[i]["category"] + "_" + data[i]["type"]);
                //obj.transform.Find("Image").GetComponent<Image>().SetNativeSize();
                //obj.transform.Find("Image").localScale = new Vector3(0.5f, 0.5f, 0.5f);
                obj.transform.Find("bg/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop/price_gold");
            }
            //银两兑换金币
            else if (data[i]["category"].ToString() == "goldcount")
            {
                obj.transform.Find("Text").GetComponent<Text>().text = data[i]["amount"].ToString()+"金币";
                //obj.transform.Find("Text").GetComponent<Text>().fontSize = 25;
                obj.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop/" + data[i]["category"]);
                //obj.transform.Find("Image").GetComponent<Image>().SetNativeSize();
                obj.transform.Find("Image").localScale = Vector3.one;
                obj.transform.Find("bg/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop/price");
            }
            //礼包
            else
            {
                obj.transform.Find("Text").GetComponent<Text>().text = data[i]["name"].ToString();
                //obj.transform.Find("Text").GetComponent<Text>().fontSize = 20;
                obj.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop/" + data[i]["category"] + "_" + data[i]["type"]);
                obj.transform.Find("Image").GetComponent<Image>().SetNativeSize();
                obj.transform.Find("bg/Image").GetComponent<Image>().sprite= Resources.Load<Sprite>("shop/price_gold");
            }
                               
       }      
    }

    /// <summary>
    ///yl 商城  购买
    /// </summary>
    /// <param name="callback"></param>
    public void BuyFunc(int shopID,GameObject obj)
    {
         Debug.Log("点击shopID========="+ shopID);
         BuyBut buybut = new BuyBut();
         buybut.memberId = UserId.memberId;
         buybut.shopGoodsId = shopID;

         string postData = JsonMapper.ToJson(buybut);
         string url = "http://" + Bridge.GetHostAndPort() + "/api/goods/buy";
         HttpCallSever.One().PostCallServer(url, postData, BuyCallback);

        currentBuyShop = obj;
        currentBuyShopID = shopID;
    }

    //yl 购买后的回调
    private void BuyCallback(string str)
    {
        JsonData data = JsonMapper.ToObject(str);
        Debug.Log("111兑换信息返回str====" + str);
        if ((int)data["code"] == 400)    //
        {
            Prefabs.Buoy("金币不足");
        }
        if ((int)data["code"] == 300)    //
        {
            Prefabs.Buoy("银两不足");
        }
        if ((int)data["code"] == 500)    //物品已拥有
        {
            Prefabs.Buoy("物品已拥有!");
        }
        if ((int)data["code"] == 200)     //兑换成功
        {
            //刷新数据
            UiController._instance.goldCount.text = data["data"].ToString();
            UserId.goldCount = (int)data["data"];

            //如果是兑换金币就刷新银两数
            if (currentBuyShopID <= 4) 
            {
                UiController._instance.getDiamond(RefreshDiamondItem); 
            }
            else
            {
                RefreshGoldItem();
            }

            Prefabs.Buoy("兑换成功");

            //银两兑换的ID是1-4  不做已拥有处理
            if (currentBuyShopID>4) {
                currentBuyShop.transform.Find("price").GetComponent<Text>().text = "已拥有";
                currentBuyShop.transform.Find("price").GetComponent<Button>().enabled = false;
            }

            //已拥有物品重新获取 进行实时刷新
            GetOwns();

            currentBuyShop = null;
            currentBuyShopID = 0;
        }   
        

    }
    public void RefreshGoldItem()
    {
        goldText.text = UserId.goldCount.ToString();
    }
    public void RefreshDiamondItem()
    {
        diamondText.text = UserId.dianment.ToString();
    }
    private class BuyBut
    {
        public int memberId;
        public int shopGoodsId;
    }

}
