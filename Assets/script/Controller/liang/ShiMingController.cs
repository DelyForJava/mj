using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LitJson;
using JX;

using System.Text.RegularExpressions;
using ILRuntime.Mono.Cecil.Cil;

/// <summary>
/// 实名认证
/// </summary>
public class ShiMingController : MonoBehaviour {

    private string IDName;
    private string IDNumber;

    public class IDData
    {   
        public string idName;
        public string idNum;
        public int memberId;
    }


  
    /// <summary>
    /// yl点击认证 后的回调
    /// </summary>
    /// <param name="str"></param>
    public void ToolCallback() {
     
            IDData Iddata = new IDData();

            Iddata.idName = IDName;
            Iddata.idNum = IDNumber;
            Iddata.memberId = UserId.memberId;

            string jsdata = JsonMapper.ToJson(Iddata);

            string url = "http://" + Bridge.GetHostAndPort() + "/api/member/certification/update";
           // Prefabs.Load("liang/tool#tool");
            HttpCallSever.One().PostCallServer(url, jsdata, RenZhenCallba);
          
    }



    void RenZhenCallba(string jsdata) {

        JsonData json = JsonMapper.ToObject(jsdata);

        if ((int)json["code"]==200)
        {
            UserId.IsIdTrue = 1;
            Prefabs.Buoy("认证成功！");
        }

    }

    /// <summary>
    /// 认证需要对名字进行识别，正则表达式识别
    /// </summary>
    public void RenZhenClick() {

         IDNumber = transform.Find("BG").Find("IDNum").Find("numtext").GetComponent<Text>().text.ToString();
         IDName = transform.Find("BG").Find("IDName").Find("nametext").GetComponent<Text>().text.ToString();
     
        string rege = "[0-9]|[A-Z]|[a-z]";        //筛选范围  正则表达式

        Regex re = new Regex(rege);
        //re.IsMatch(IDName);

        if (IDName.Length<2|| re.IsMatch(IDName))
        {
            Prefabs.Buoy("姓名不合法！");          
            return;
        }

      
        if (IDName==""||IDNumber=="") {
            Prefabs.Buoy("姓名或身份证号为空！");          
            return;
        }

      
        if (IDNumber.Length != 18)
        {
            Prefabs.Buoy("身份证号位数错误");
            return;
        }

        //将所输入的身份证号码进行拆分，拆分为17位，最后一位留着待用
            int Num0 = int.Parse(IDNumber.Substring(0, 1)) * 7;
            int Num1 = int.Parse(IDNumber.Substring(1, 1)) * 9;
            int Num2 = int.Parse(IDNumber.Substring(2, 1)) * 10;
            int Num3 = int.Parse(IDNumber.Substring(3, 1)) * 5;
            int Num4 = int.Parse(IDNumber.Substring(4, 1)) * 8;
            int Num5 = int.Parse(IDNumber.Substring(5, 1)) * 4;
            int Num6 = int.Parse(IDNumber.Substring(6, 1)) * 2;
            int Num7 = int.Parse(IDNumber.Substring(7, 1)) * 1;
            int Num8 = int.Parse(IDNumber.Substring(8, 1)) * 6;
            int Num9 = int.Parse(IDNumber.Substring(9, 1)) * 3;
            int Num10 = int.Parse(IDNumber.Substring(10, 1)) * 7;
            int Num11 = int.Parse(IDNumber.Substring(11, 1)) * 9;
            int Num12 = int.Parse(IDNumber.Substring(12, 1)) * 10;
            int Num13 = int.Parse(IDNumber.Substring(13, 1)) * 5;
            int Num14 = int.Parse(IDNumber.Substring(14, 1)) * 8;
            int Num15 = int.Parse(IDNumber.Substring(15, 1)) * 4;
            int Num16 = int.Parse(IDNumber.Substring(16, 1)) * 2;
            int allNum = Num0 + Num1 + Num2 + Num3 + Num4 + Num5 + Num6 + Num7 + Num8 + Num9 + Num10 + Num11 + Num12 + Num13 + Num14 + Num15 + Num16;
            Judge(allNum, IDNumber.Substring(17, 1));

            //throw ;
       // }

    }

    /// <summary>
    /// 验证方法
    /// </summary>
    /// <param name="AllnumSum"></param>
    /// <param name="LastNum"></param>
    void Judge(int AllnumSum,string LastNum) {

        int Remainder = AllnumSum % 11;

        string agree = "您的身份证合法，已通过验证!";
        string disagree= "您的身份证不合法，请重新输入！";

        switch (Remainder) {
                case 0:
                  if (int.Parse(LastNum) == 1)
                  {           
                    Close();                  
                    ToolCallback();
                   // GetTool(agree);
                    return;
                  }
                  else
                  {
                    Prefabs.Buoy(disagree);
                  }

                break;
                case 1:
                  if (int.Parse(LastNum) == 0)
                  {                
                    Close();
                    ToolCallback();
                    //GetTool(agree);
                    return;
                  }
                  else
                  {
                    Prefabs.Buoy(disagree);
                  }
                break;
                case 2:
                  if (LastNum != "x" && LastNum != "X")
                  {
                    Prefabs.Buoy("身份证填写错误");                 
                  }
                  else
                  {                  
                    Close();
                    ToolCallback();
                    //GetTool(agree);
                    return;
                  }

                break;
                case 3:
                if (int.Parse(LastNum) == 9)
                {                  
                    Close();
                    ToolCallback();
                   // GetTool(agree);
                    return;
                }
                else
                {
                    Prefabs.Buoy(disagree);
                }

                break;
                case 4:

                  if (int.Parse(LastNum) == 8)
                  {                 
                    Close();
                    ToolCallback();
                    //GetTool(agree);
                    return;
                  }
                  else
                  {
                    Prefabs.Buoy(disagree);
                  }
                break;
                case 5:
                  if (int.Parse(LastNum) == 7)
                  {
                   
                    Close();
                    ToolCallback();
                    //GetTool(agree);
                    return;
                }
                  else
                  {
                    Prefabs.Buoy(disagree);
                  }
                break;
                case 6:
                   if (int.Parse(LastNum) == 6)
                   {
                    
                    Close();
                    ToolCallback();
                   // GetTool(agree);
                    return;
                    }
                    else
                    {
                       Prefabs.Buoy(disagree);
                    }
                break;
                case 7:
                   if (int.Parse(LastNum) == 5)
                   {                
                       Close();
                      ToolCallback();
                    //GetTool(agree);
                       return;
                   }
                   else
                   {
                     Prefabs.Buoy(disagree);
                   }
                break;
                case 8:
                  if (int.Parse(LastNum) == 4)
                  {
                   
                    Close();
                    ToolCallback();
                    //GetTool(agree);
                    return;
                  }
                  else
                  {
                    Prefabs.Buoy(disagree);
                  }
                break;
                case 9:
                  if (int.Parse(LastNum) == 3)
                  {
                   
                    Close();
                    ToolCallback();
                    //GetTool(agree);
                    return;
                  }
                  else
                  {
                    Prefabs.Buoy(disagree);
                  }
                break;
                case 10:
                   if (int.Parse(LastNum) == 2)
                   {                    
                     Close();
                    ToolCallback();
                  
                     return;
                   }
                   else
                   {
                    Prefabs.Buoy(disagree);
                   }
                break;
    
            }
    }


    public void Close()
    {
        Audiocontroller.Instance.PlayAudio("Back");
        Destroy(this.gameObject);
        //transform.DOMoveX(-3f, 1f);  
    }

 
    public void sure() {

        Destroy(this.gameObject.transform.parent);
    }

}
