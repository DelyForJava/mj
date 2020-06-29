using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangUpCallInfo{
	public string actionCode;
	public HangUpStart Params;
	public class HangUpStart
    {
		public int playerStatus;
		public int HangUpType;
    }
}
