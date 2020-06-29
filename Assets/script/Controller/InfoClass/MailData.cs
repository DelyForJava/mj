using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailData {
    public List<MailDataRow> data;
   public class MailDataRow 
    {
        public int id;
        public int friendId;
        public int memberId;
        public string msg;
        public string createTime;
        public int status;
    }
    public int code;
    public string message;

}
