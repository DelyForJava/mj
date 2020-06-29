using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllReadActionInfo{
	public string actionCode;
	public int turnNum;
	public int newCard;
    public int rascalCard;
    public List<Data> players;

    public class Data
    {
        public int flyRascalCount;
        public int point;
        /// <summary>
        /// 名字
        /// </summary>
        //public string nickName;
        /// <summary>
        /// 游戏状态
        /// </summary>
        public int gameStatus;
        /// <summary>
        /// 玩家状态
        /// </summary>
        public int playerStatus;
        /// <summary>
        /// 癞子数
        /// </summary>
        public int rascalCount;
        /// <summary>
        /// 胡牌
        /// </summary>
        public int hu;
        /// <summary>
        /// 碰杠吃的牌
        /// </summary>
        public List<int> skillCards;
        /// <summary>
        /// 上家
        /// </summary>
        public int frontNum;
        /// <summary>
        /// 玩家账户
        /// </summary>
        public int uid;
        /// <summary>
        /// 座位号
        /// </summary>
        public int seatNum;
        public int readyFlag;
        public int tableNum;
        /// <summary>
        /// 技能碰吃杠
        /// </summary>
        public Dictionary<string,int> skillMap;
        /// <summary>
        /// 手牌
        /// </summary>
        public List<int> handCards;
    }
   
}
