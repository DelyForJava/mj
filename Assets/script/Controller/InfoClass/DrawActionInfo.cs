
using System.Collections.Generic;

public class DrawActionInfo 
{
    public string actionCode;
    public int newCard;
    public int seatNum;
    public Data player;
    public int gangCard;
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
