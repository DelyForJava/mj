using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSocketInfo  {
	public int tableNum;
	public string actionCode;
	public data Params;
	public class data
	{
		public int round;
		public int code;
		public int type;
		public int card;
		public int maxPoints;
		public bool IsAutoPut;
	}

}
