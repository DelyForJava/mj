using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestWebsocketLink : MonoBehaviour
{
    public bool isRoll;
    public int playNum;
    public GameObject messagePanel;
    public RectTransform messageRect;
    public float width;
    public Queue messages = new Queue();
    public Text MTA;
    public Transform OneCard;
    public Transform TwoCard;
    public EnumGameStatus gs=new EnumGameStatus();
    void Start()
    {
       Debug.Log(EnumGameStatus.Race);
    }
    public void Test()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("installApk", PathInfo.DownLoadPath + "/天胡十三浪.apk", "com.yfkj.majiangshang");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject go = Resources.Load<GameObject>("prefabs/DarkGang");
            Transform t = GameObject.Instantiate(go).transform;
            t.SetParent(OneCard);
            RectTransform rect = t as RectTransform;
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
            (rect.GetChild(0) as RectTransform).sizeDelta = new Vector2(70.7f, 91.5f);
            (rect.GetChild(0) as RectTransform).anchoredPosition = new Vector2(19.9f, -16.9f);
        }
    }
    /// <summary>
    /// 播放公告
    /// </summary>
    /// <param name="str">公告内容</param>
    /// <param name="runnum">循环播放次数</param>
    public void RollMessage(string str, int runnum = 1)
    {
        if (isRoll)   //判断当前是否有公告在播放，有就把待播放公告插到队列中等待播放。
        {
           // Messages message = new Messages()
            //{
            //    message = str,
            //    num = runnum
            //};
            //messages.Enqueue(message);
            return;
        }
        isRoll = true;
        playNum = runnum;
        messagePanel = GameObject.Instantiate(Resources.Load("Prefabs/WorldMessage")) as GameObject;
        GameObject obj = GameObject.Find("bottomCanvas");
        messagePanel.transform.SetParent(obj.transform, false);
        messageRect = messagePanel.transform.Find("massage").GetComponent<RectTransform>();
        Text text = messageRect.transform.Find("text").GetComponent<Text>();
        text.text = str;
        width = text.preferredWidth;    //获取文字的长度
        messageRect.anchoredPosition = new Vector3(0, 0, 0);  //让文字从在最右边开始移动


        RollAnimation();
    }
    /// <summary>
    /// 播放动画
    /// </summary>
    public void RollAnimation()
    {                                		//循环播放
        //MTA.transform.DOLocalMoveY(width+540, 5).SetEase(Ease.Linear).OnComplete(()=> {
        //    Destroy(messagePanel);
        //    isRoll = false;
        //    if (messages.Count > 0)             //判断队列中有木有公告，有继续播放
        //    {
        //        Messages message = messages.Dequeue();
        //        RollMessage(message.message, message.num);
        //    }
        //});
    }

    /// <summary>
    /// 结束播放
    /// </summary>
    private void CompleteRoll()
    {
        //Destroy(messagePanel);
        //isRoll = false;
        //if (messages.Count > 0)				//判断队列中有木有公告，有继续播放
        //{
        //    Messages message = messages.Dequeue();
        //    RollMessage(message.message, message.num);
        //}
    }
    /// <summary>
    /// 通过AddEvent()添加监听事件
    /// 通过调用DoEvent()触发事件
    /// 消息系统内置一个消息队列，通过不断轮询调用
    /// </summary>
    public abstract class IEventManager
    {
        //定义委托
        public delegate void EventDelegate(/*byte[] stream*/ string Stream);
        //用来接收委托的字典
        Dictionary<int, Dictionary<int, List<EventDelegate>>> mainDict = new Dictionary<int, Dictionary<int, List<EventDelegate>>>();
        //定义一个队列类型
        private Queue<CallData> callDataList = new Queue<CallData>();

        private int _delayTime = 0;
        public int DelayTime
        {
            get
            {
                return _delayTime;
            }
        }

        struct CallData
        {
            public int _mainId;
            public int _assistId;
            //public byte[] _stream;
            public string _stream;
        }

        public IEventManager(int delayTime = 0)
        {
            _delayTime = delayTime;
        }
        //通过AddEvent()添加监听事件
        public virtual void AddEvent(int mainId, int assistId, EventDelegate callback)
        {
            Dictionary<int, List<EventDelegate>> temp;
            List<EventDelegate> eventList;
            if (!mainDict.ContainsKey(mainId))
            {
                temp = new Dictionary<int, List<EventDelegate>>();
                eventList = new List<EventDelegate>();
                eventList.Add(callback);
                temp.Add(assistId, eventList);
                mainDict.Add(mainId, temp);
            }
            else
            {
                temp = mainDict[mainId];
                if (!temp.ContainsKey(assistId))
                {
                    eventList = new List<EventDelegate>();
                    eventList.Add(callback);
                    temp.Add(assistId, eventList);
                }
                else
                {
                    eventList = temp[assistId];
                    eventList.Add(callback);
                    temp[assistId] = eventList;
                }
                mainDict[mainId] = temp;
            }

        }

        public void RemoveEvent(int mainId, int assistId)
        {
            if (mainDict.ContainsKey(mainId))
            {
                if (mainDict[mainId].ContainsKey(assistId))
                    mainDict[mainId].Remove(assistId);
            }
        }
        //通过调用DoEvent()触发事件
        public virtual void DoEvent(int mainId, int assistId, /*byte[] stream*/string stream)
        {
            CallData data = new CallData
            {
                _mainId = mainId,
                _assistId = assistId,
                _stream = stream
            };
            callDataList.Enqueue(data);
        }

        public void Update()
        {
            if (callDataList != null && callDataList.Count > 0)
            {
                CallData data = callDataList.Dequeue();
                Do(data);
            }
        }

        private void Do(CallData data)
        {
            if (mainDict.ContainsKey(data._mainId))
            {
                Dictionary<int, List<EventDelegate>> temp = mainDict[data._mainId];
                if (temp.ContainsKey(data._assistId))
                {
                    List<EventDelegate> eventList = temp[data._assistId];
                    foreach (EventDelegate ed in eventList)
                    {
                        ed(data._stream);
                    }
                }
            }
        }

    }
}


