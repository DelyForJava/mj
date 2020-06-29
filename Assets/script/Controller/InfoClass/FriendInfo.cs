using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendInfo
{
	public Friend data;
	
	public int code;
	public string message;

	public class Friend
	{
	   public List<Data> friends;
	}
	public class Data
	{
		public int id;
		public int gold;
		public int status;
		public string nickname;
		public string username;
		public string password;
		public string phone;
		public string avatar;
		public string createTime;
		public string lastLoginTime;
		public Data() { }
	}
}