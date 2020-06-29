using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LitJson;

#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
public class PlatformScript : MonoBehaviour
{
    private struct PlatformMsg
    {
        public int iMsgId;
        public int iPararm1;
        public int iPararm2;
        public int iPararm3;
        public string strParam1;
        public string strParam2;
        public string strParam3;
    }

    private Queue<PlatformMsg> msgQueue = new Queue<PlatformMsg>();
    protected void OnMessage(string param)
    {
        JsonData jd = JsonMapper.ToObject(param);
        PlatformMsg msg;
        msg.iMsgId = (int)jd["iMsgId"];
        msg.iPararm1 = (int)jd["iPararm1"];
        msg.iPararm2 = (int)jd["iPararm2"];
        msg.iPararm3 = (int)jd["iPararm3"];
        msg.strParam1 = (string)jd["strParam1"];
        msg.strParam2 = (string)jd["strParam2"];
        msg.strParam3 = (string)jd["strParam3"];

        msgQueue.Enqueue(msg);
    }
    void Update()
    {
        while (msgQueue.Count > 0)
        {
            PlatformMsg msg = msgQueue.Dequeue();
            switch (msg.iMsgId)
            {
                //case PLATFORM_MSG_QQLOGINCALLBACK:
                //    JsonData qqJd = JsonMapper.ToObject(msg.strParam1);
                //    PhongManager.Instance.LoginCallBack(qqJd, PlatEnum.QQ, msg.iPararm1);
                //    break;
                //case PLATFORM_MSG_WXLOGINCALLBACK:
                //    JsonData wxJd = JsonMapper.ToObject(msg.strParam1);
                //    PhongManager.Instance.LoginCallBack(wxJd, PlatEnum.WX, msg.iPararm1);
                //    break;
                default:
                    break;
            }
        }
    }
}
#endif