using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuActionController : MonoBehaviour {

    public Game_Controller Game_;
    public void HuAction(string edata)
    {
        Debug.Log("HuAction" + edata);
        HuclassInfo huclass = JsonMapper.ToObject<HuclassInfo>(edata);
        Debug.Log("huclass" + huclass.seatNum);
        //赋值
        Assignment(huclass);
        //刷新手牌
        ReFashHandCard(huclass);
        if (huclass.seatNum == 0)
        {
            FlowBureau();
            Game_.jie.isFlowBurea = true;
        }
        SettleMent();
    }
    //huAnimator
    public void HuLevel1Animator(HuclassInfo huclass)
    {
        string HuDes = "";
        if (huclass.seatNum != 0) {
        if (huclass.players[huclass.seatNum - 1].hu == 1) { HuDes = "平胡"; }
        else if (huclass.players[huclass.seatNum - 1].hu == 2) { HuDes = "宝吊"; }
        else if (huclass.players[huclass.seatNum - 1].hu == 3) { HuDes = "十三浪"; }
        else if (huclass.players[huclass.seatNum - 1].hu == 7) { HuDes = "七对"; }
        else if (huclass.players[huclass.seatNum - 1].hu == 9) { HuDes = "九幺"; }
        else if (huclass.players[huclass.seatNum - 1].hu == 33) { HuDes = "碰碰胡"; }
        else if (huclass.players[huclass.seatNum - 1].hu == 2033) { HuDes ="宝吊碰碰胡";}
        else if (huclass.players[huclass.seatNum - 1].hu == 16) { HuDes = "七对九幺"; }
        }else
        {
            HuDes = "流局";
        }
        if (huclass.seatNum == Game_.youplaycount)
            StartCoroutine(SkillTShow("You", "HuDes",HuDes));
        else if (huclass.seatNum == Game_.zuoplaycount)
            StartCoroutine(SkillTShow("Zuo", "HuDes",HuDes));
        else if (huclass.seatNum == Game_.shangplaycount)
            StartCoroutine(SkillTShow("Shang", "HuDes",HuDes));
        else if (huclass.seatNum == Game_.seatNum)
            StartCoroutine(SkillTShow("My", "HuDes", HuDes));
        else if (huclass.seatNum ==0) { StartCoroutine(
            SkillTShow("You", "HuDes", HuDes));
            StartCoroutine(SkillTShow("Zuo", "HuDes", HuDes));
            StartCoroutine(SkillTShow("Shang", "HuDes", HuDes));
            StartCoroutine(SkillTShow("My", "HuDes", HuDes));
        }
    }
    public void ShowHuHandCard(HuclassInfo huclass)
    {
        if (huclass.seatNum == Game_.seatNum)
        {
            List<int> MyCard = new List<int>();
            //刷新当前手牌
            MyCard = Game_.RefashCard(huclass.players[Game_.seatNum-1].handCards, huclass.players[Game_.seatNum - 1].skillCards, Game_.reascalcount);
            //刷新状态
            ReFreshHandCardStatue_(MyCard);
            FashPostive(huclass, Game_.seatNum - 1);
        }
        else if (huclass.seatNum == Game_.youplaycount) {
            ReFreshOtherCard(huclass, Game_.one, Game_.youplaycount-1, Game_.You);
        }
        else if (huclass.seatNum == Game_.shangplaycount) {
            ReFreshOtherCard(huclass, Game_.twotwo, Game_.shangplaycount-1, Game_.Shang);
        }
        else if (huclass.seatNum == Game_.zuoplaycount) { 
            ReFreshOtherCard(huclass, Game_.three, Game_.zuoplaycount - 1, Game_.Zuo); }
    }
    public IEnumerator SkillTShow(string choseN, string skillname,string HuDes)
    {
        Transform go = null;
        switch (choseN)
        {
            case "Shang":
                go = GameObject.Find("ShangSkillCards").transform.Find(skillname);
                go.GetComponent<Text>().text = HuDes;
                break;
            case "Zuo":
                go = GameObject.Find("ZuoSkillCards").transform.Find(skillname);
                go.GetComponent<Text>().text = HuDes;
                break;
            case "You":
                go = GameObject.Find("YouSkillCards").transform.Find(skillname);
                go.GetComponent<Text>().text = HuDes;
                break;
            case "My":
                go = GameObject.Find("MySkillCards").transform.Find(skillname);
                go.GetComponent<Text>().text = HuDes;
                break;
        }
        go.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        go.gameObject.SetActive(false);

    }
    //赋值
    public void Assignment(HuclassInfo huclass)
    {
        Game_.jie.Initle();
        Game_.jie.listCard = Game_.list;
        Game_.jie.HuTheme = huclass.seatNum;
        Game_.jie.mySeatNum =Game_.seatNum;
        Game_.jie.rascalCard = Game_.rascalCard;
        Game_.jie.youSeatNum=Game_.youplaycount;
        Game_.jie.zuoSeatNum = Game_.zuoplaycount;
        Game_.jie.shangSeatNum = Game_.shangplaycount;
        Game_.jie.HuDescrite = huclass.description;
        Game_.jie.gangPointsList = huclass.gangPointsList;
        Game_.jie.goldChangeList = huclass.goldChangeList;
    }
    //刷新手牌
    public void ReFashHandCard(HuclassInfo huclass)
    {
        List<int> MyCard = new List<int>();
		for (int i = 0; i < huclass.players.Count; i++)
		{
			if (huclass.players[i].seatNum == Game_.seatNum)
			{
				Game_.jie.MyRasacal = huclass.players[i].rascalCount;
                //刷新当前手牌
                MyCard= Game_.RefashCard(huclass.players[i].handCards,huclass.players[i].skillCards,Game_.reascalcount);
                //刷新状态
                ReFreshHandCardStatue_(MyCard);
                 //改变位置 之前以及添加位置就不再换位置
                 //FashPostive(huclass, i); 
            }
            if (huclass.players[i].seatNum == Game_.youplaycount)
            {
                Game_.jie.YouRascal = huclass.players[i].rascalCount;
                ReFreshOtherCard(huclass,Game_.one,i,Game_.You);
            }
            if (huclass.players[i].seatNum == Game_.zuoplaycount)
            {
                Game_.jie.ZuoRasCal = huclass.players[i].rascalCount;
                ReFreshOtherCard(huclass,Game_.three,i,Game_.Zuo);
            }
            if(huclass.players[i].seatNum == Game_.shangplaycount)
            {
                Game_.jie.ShangRasCal = huclass.players[i].rascalCount;
                ReFreshOtherCard(huclass, Game_.twotwo, i, Game_.Shang);
            }
		}
        //结算数据获取自己手牌
        Game_.jie.MyPaiCard = MyCard;
        //获取右边手牌
        Game_.jie.YouPaiCard = Game_.one;
        //获取上边手牌
        Game_.jie.ShangPaiCard = Game_.twotwo;
        //获取左边手牌
        Game_.jie.ZuoPaiCard = Game_.three;
    }
    //刷新手牌状态
    public void ReFreshHandCardStatue_(List<int> MyCard)
    {
        for (int j = 0; j < MyCard.Count; j++)
        {
            Image pic = Game_.Pai.GetChild(j).GetComponent<Image>();
            pic.enabled = false;
            Game_.Pai.GetChild(j).GetChild(0).gameObject.SetActive(false);
            Transform t1 = Game_.Pai.GetChild(j).Find("skillpai");
            t1.gameObject.SetActive(true);
            //防报错
            if (MyCard[j] == 0) { return; }
            Game_.Pai.GetChild(j).GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[MyCard[j]];
            Game_.Pai.GetChild(Game_.Pai.childCount - 1 - j).GetChild(2).gameObject.SetActive(false);
        }
    }
    //状态改变后改变位置
    public void FashPostive(HuclassInfo huclass,int i)
    {
        int count = 0;
        for (int j = huclass.players[i].skillCards.Count; j < Game_.Pai.childCount; j++)
        {
            (Game_.Pai.GetChild(j) as RectTransform).anchoredPosition -= new Vector2(count * 13, 0);
            count++;
        }
    }
    //刷新其他玩家手牌
    public void ReFreshOtherCard(HuclassInfo huclass,List<int> ObjList,int i,Transform ObjGame)
    {
        List<int> youList = new List<int>();
        youList =Game_.OtherResh(huclass.players[i].handCards, huclass.players[i].skillCards, huclass.players[i].rascalCount);
        Debug.Log(ObjList.Count);
        Debug.Log(ObjGame.childCount);
        
        Game_.ContrastPai(ObjGame.childCount, ObjList.Count, ObjGame);
        Debug.Log(ObjGame.childCount);
        Debug.Log(youList.Count);
        for (int j = huclass.players[i].skillCards.Count; j < ObjList.Count; j++)
         {
             if (huclass.players[i].skillCards.Count>0)
                {
                    Transform tf = ObjGame.GetChild(j);
                    tf.GetChild(0).gameObject.SetActive(false);
                    tf.GetChild(1).gameObject.SetActive(true);
                    tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[youList[j]];
                }
          }
        ObjList = youList;
    }
    //流局
    public void FlowBureau()
    {
        GameObject go = GameObject.Find("TrusteeshipStatus");
        Game_.isOwn = false;
        Game_.skillMap.Find("TuoGuang").gameObject.SetActive(false);
        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
   //结算的显示
    public void SettleMent()
    {   
        //对数据进行处理多余的数据在结算后清空
        if (Game_.PlayerActionDataOfLink != null) { Game_.PlayerActionDataOfLink.Clear(); }
        if (Game_.DrawActionData != null) { Game_.DrawActionData.Clear(); }
        //托管状态还原
        Transform tg = Game_.skillMap.Find("TuoGuang");
        tg.gameObject.SetActive(false);
        Transform TTss = GameObject.Find("TrusteeshipStatus").transform;
        for (int i = 0; i < TTss.childCount; i++)
        {
            TTss.GetChild(i).gameObject.SetActive(false);
        }
        //
        Game_.HuDataInfo = null;
        //通过JieCreateRoom这个标识确认是否是创建房间
        //在通过Createround来确定当前局数
        if (UserId.JieCreateRoom)
        {
            Game_.Createround -= 1;
            if (Game_.Createround > 0)
            {
                Game_.CurrentRound.text = Game_.Createround.ToString();
                Game_.JieTrabsform.gameObject.SetActive(true);
                Game_.jie.StartJieSuang();
            }
            else
            {
                UserId.JieCreateRoom = false;
                return;
            }
        }
        else { Game_.jie.StartJieSuang(); Game_.JieTrabsform.gameObject.SetActive(true); }
    }
}
