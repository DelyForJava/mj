using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerActionInfo{
    public string actionCode;
    public int turnNum;
    public int skillPlayerCount;
    public List<Data> players;
    public int card;
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
