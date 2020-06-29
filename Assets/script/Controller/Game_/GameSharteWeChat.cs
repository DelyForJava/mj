using JX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameSharteWeChat : MonoBehaviour {

    public Button WeChatFriend;
    public Button WeChatMember;
    public string TableNum;
    void Start()
    {
        WeChatFriend.GetComponent<Button>().onClick.AddListener(ShaterFriend);
        WeChatMember.GetComponent<Button>().onClick.AddListener(ShaterMember);
    }
    void ShaterFriend()
    {
        string url = "http://" + Bridge.GetHostAndPort() +"/page/share/wechat" + "/" + TableNum;
        WeChatfriend(url);
    }
    void ShaterMember()
    {
        string url = "http://" + Bridge.GetHostAndPort() +"/page/share/wechat" + "/" + TableNum;
        WeChatMember_(url);
    }
    void WeChatfriend(string url)
    {
        gameObject.SetActive(false);
        ShartSDKControlle.Instance.SharteWebPage(url);
    }
    void WeChatMember_(string url)
    {
        gameObject.SetActive(false);
        ShartSDKControlle.Instance.SharteWeChatMoments(url);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
