using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTableInfo
{
	public int tableNum;
	public string actionCode;
	public List<Data> players;
	public int round;
	public bool IsAutoPut;
	public class Data
	{
		public int tableNum;
		public int gameStatus;
		public string nickName;
		public int rascalCount;
		public int hu;
		public int point;
		public List<int> skillCards;
		public int frontNum;
		public int uid;
		public int seatNum;
		public int readyFlag;
		public List<int> skillMap;
		public int playerStatus;
		public int flyRascalCount;		
	}
	
}
