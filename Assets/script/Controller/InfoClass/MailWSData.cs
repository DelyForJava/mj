
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemMailData 
{
    public string actionCode;
    public List<MaillData> mailList;
    //public List<AnnounceList> announceList;
}

public class MaillData
{
    public string createTime;
    public int id;
    //public int memberId;
    public string msg;
    public int status;
    public int type;
    public int goldCount;
    public string remark;
}
public class AnnounceList
{
    public string createTime;
    public int id;
    //public int memberId;
    public string msg;
    public int status;
    public int type;
}
public class Draw {
    public int id;
    public string category;
    public int type; 
}


