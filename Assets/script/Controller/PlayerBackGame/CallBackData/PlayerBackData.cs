using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerBackData {
	public List<Data> data;
	public int code;
	public string message;
	public class Data
	{
		public string gameId;
		public string replayText;
	}
}
public class PlayerBack
{
	public List<string> replayList;
	public string actionCode;
}
