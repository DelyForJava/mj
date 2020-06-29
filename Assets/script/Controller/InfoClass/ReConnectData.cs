using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReConnectData {
	public int tableNum;
	public int rascalCard;
	public int gameType;
	public string actionCode;
	public int darkCardsSize;
	public int round;
	public int turnNum;
	public List<int> riverCards;
	public List<int> handCardsSizeList;
	public List<int> handCards;
	public List<int> rascalCountSizeList;
	public List<List<int>> skillCardsList;
	public List<int> idList;
	public bool IsAutoPut;
    public Data player;
    public int robotSize;
    public class Data
    {
        public int flyRascalCount;
        public int point;
        public int gameStatus;
        public string nickName;
        public int rascalCount;
        public int hu;
        public List<int> skillCards;
        public int frontNum;
        public int uid;
        public int seatNum;
        public int readyFlag;
        public int tableNum;
        public Dictionary<string, int> skillMap;
        public List<int> handCards;
        public int playerStatus;
        public int accumulatePoint;
    }
}
