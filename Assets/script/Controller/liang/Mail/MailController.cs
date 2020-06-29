using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using JX;
using LitJson;
using System.IO;
using ILRuntime.Mono.Cecil.Cil;
using ILRuntime.Runtime;
using UnityEngine.EventSystems;

public class MailController : MonoBehaviour
{
    public Transform grid;
    public Toggle toggle1;
    public Toggle toggle2;
    private int witch = 1; //1 toggle1 2 toggle2

    private string url = "http://" + Bridge.GetHostAndPort() + "/api/message/mail/read";

    void Start()
    {
        SetButtonFunc();

        if (UserId.MailList.Count > 0)
        {
            witch = 1;
        }
        //else if (UiController._instance.MailList.Count > 0)
        //{
        //    witch = 2;
        //}
        OnSwitchToggle();
    }

    void SetButtonFunc()
    {
        toggle1.onValueChanged.AddListener((isOn) =>
        {
            witch = isOn ? 1 : 2;
            OnSwitchToggle();
        });
        toggle2.onValueChanged.AddListener((isOn) =>
        {
            witch = isOn ? 2 : 1;
            OnSwitchToggle();
        });

    }

    void OnSwitchToggle()
    {
        if (witch == 1)
        {
            SetSystemMail();
        }
        else if (witch == 2)
        {
            toggle1.isOn = false;
            toggle2.isOn = true;
            Clean();

            //下面的方法不用了  到时候加别的消息再调用
            // SetMail();
        }

    }

    void Clean()
    {
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    ///获取到邮件数据后  添加邮件
    /// </summary>
    void SetSystemMail()
    {
        toggle1.isOn = true;
        toggle2.isOn = false;
        Clean();
        if (UserId.MailList!=null)
        {
            for (int i = 0; i < UserId.MailList.Count; i++)
            {

                GameObject obj = Bridge._instance.LoadAbDate(LoadAb.MainTwo, "MailItem", grid);

                //SystemMailData systemMailData = UiController._instance.MailList;
                MaillData mailData = UserId.MailList[i];
                obj.transform.Find("message").gameObject.GetComponent<Text>().text = mailData.msg;
                if (UserId.MailList[i].type == 1)
                {
                    var getTran = obj.transform.Find("drawbtn");
                    var watchTran = obj.transform.Find("Watch");
                    getTran.gameObject.SetActive(false);
                    watchTran.gameObject.SetActive(true);
                    watchTran.gameObject.GetComponent<Button>().onClick.AddListener(() => {

                        OnClickSystemMailWatch(obj, mailData);                     
                    });
                }
                else
                {
                    var getTran = obj.transform.Find("drawbtn");
                    var watchTran = obj.transform.Find("Watch");
                    getTran.gameObject.SetActive(true);
                    watchTran.gameObject.SetActive(false);
                    getTran.gameObject.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OnClickSystemMailGet(obj, mailData);
                    });
                }
            }
        }    
    }


    //查看邮件
    void OnClickSystemMailWatch(GameObject go, MaillData mailData)
    {
        GameObject item = Bridge._instance.LoadAbDate(LoadAb.Main, "showdraw");
        DrawAni.isMessage = true;
        //obj.transform.Find("drawbtn/Text").GetComponent<Text>().text = "查看";

        item.transform.Find("game").gameObject.SetActive(false);
        item.transform.Find("ani").gameObject.SetActive(false);
        item.transform.Find("mess").GetComponent<Text>().text = mailData.msg;

        HttpCallSever.One().PostCallServer(url, "{\"messageId\":" + mailData.id + "}", Debug.Log);
        //go.transform.Find("game").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop/" + msg);
        //UiController._instance.MailList.Remove(mailData);
        UserId.MailList.Remove(mailData);
        if (UserId.MailList.Count == 0)
        {
            //UiController._instance.MailList = null;
            UserId.MailList = null;
        }
        Destroy(go.gameObject);
    }

    //领取邮件  
    void OnClickSystemMailGet(GameObject go, MaillData mailData)
    {
        //obj.transform.Find("drawbtn/Text").GetComponent<Text>().text = "领取";
        //Debug.Log("maildata.mailList[i].remark==" + maildata.mailList[i].remark);
        Draw dremark = JsonMapper.ToObject<Draw>(mailData.remark);

        int goldcount = mailData.goldCount;
        GameObject item = Bridge._instance.LoadAbDate(LoadAb.Main, "showdraw");
        DrawAni.isMessage = false;

        item.transform.Find("bg").gameObject.SetActive(false);
        item.transform.Find("mess").gameObject.SetActive(false);

        if (goldcount != 0)
            item.transform.Find("game").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop/goldcount");
        else
            item.transform.Find("game").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop/" + dremark.category + "_" + dremark.type);


        item.transform.Find("game").GetComponent<Image>().SetNativeSize();
        HttpCallSever.One().PostCallServer(url, "{\"messageId\":" + mailData.id + "}", DrawCallBack);

        //UiController._instance.MailList.Remove(mailData);
        UserId.MailList.Remove(mailData); 
       // mailData.mailList.Remove(mailData);
        if (UserId.MailList.Count == 0)
        {
           // UiController._instance.MailList = null;
            UserId.MailList = null;
        }
        Destroy(go.gameObject);
    }

    void SetMail()
    {
        toggle1.isOn = false;
        toggle2.isOn = true;
        Clean();

        for (int i = 0; i < UserId.MailList.Count; i++)
        {
            GameObject obj = Bridge._instance.LoadAbDate(LoadAb.MainTwo, "MailItem", grid);

            obj.transform.Find("message").gameObject.GetComponent<Text>().text = UserId.MailList[i].msg.ToString();

            int id = (int)UserId.MailList[i].id;

            //obj.transform.Find("drawbtn/Text").GetComponent<Text>().text = "查看";

            string msg = UserId.MailList[i].msg.ToString();

            int index = i;
            var watchTran = obj.transform.Find("drawbtn");
            var getTran = obj.transform.Find("Watch");
            getTran.gameObject.SetActive(true);
            watchTran.gameObject.SetActive(false);
            getTran.gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {

                GameObject go = Bridge._instance.LoadAbDate(LoadAb.Main, "showdraw");
                DrawAni.isMessage = true;

                go.transform.Find("game").gameObject.SetActive(false);
                go.transform.Find("ani").gameObject.SetActive(false);
                go.transform.Find("mess").GetComponent<Text>().text = msg;

                HttpCallSever.One().PostCallServer(url, "{\"messageId\":" + id + "}", Debug.Log);
                //go.transform.Find("game").GetComponent<Image>().sprite = Resources.Load<Sprite>("shop/" + msg);
                UserId.MailList.RemoveAt(index);

                Destroy(obj.gameObject);
            });
        }

    }

    /// <summary>
    /// 邮件领取成功
    /// </summary>
    /// <param name="data"></param>
    private void DrawCallBack(string data)
    {

        JsonData Maildata = JsonMapper.ToObject(data);

        if ((int)Maildata["code"] == 200)
        {
            UserId.goldCount = (int)Maildata["data"]["gold"];
            UiController._instance.goldCount.text = Maildata["data"]["gold"].ToString();
            YouFu.Debug.Log("已读，删除成功");
        }

    }

    public void Close()
    {
        Audiocontroller.Instance.PlayAudio("Back");
        transform.Find("Middle").DOMoveY(1.4f, 0.8f).OnComplete(() =>
        {
            Destroy(gameObject);
        });

    }

}