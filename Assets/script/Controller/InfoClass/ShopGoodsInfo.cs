using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGoodsInfo  {

    public Data data;
    public int code;
    public string message;
    public class Data
    {
        public List<Info> shop;
    }
    public class Info
    {
        public int id;
        public string category;
        public int type;
        public int amount;
        public int price;
        public string name;
       
    }

}
