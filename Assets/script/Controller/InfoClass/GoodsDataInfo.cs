using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsDataInfo {
    public Data data;
	public class Data
    {
        public List<Info> goods; 
    } 
    public class Info
    {
        public int id;
        public int memberId;
        public int shopGoodsId;
        public string category;
        public int type;
    }
}
