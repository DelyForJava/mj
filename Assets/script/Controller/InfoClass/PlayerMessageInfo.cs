using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMessageInfo {
    public Info data;
    public int code;
    public string message;
    public class Info
    {
        public int gold;
        public int gameCount;
        public string createTime;
        public string nickname;
        public int id;
        public string avatar;
        public int headFrame;
        public int gender;
    }


}
