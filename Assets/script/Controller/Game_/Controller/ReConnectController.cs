using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReConnectController : MonoBehaviour {

    public Game_Controller Game_;
    public void ReConnectAction(string edate)
    {
        ReConnectData RC = JsonMapper.ToObject<ReConnectData>(edate);
        //将断线后需要复原的参数重新赋值
        ParamentSetting(RC);
        //重新赋值位置
        SetNumGet(RC);
        //玩家数据获取
        PlayerData(RC);
        //手牌管理技能牌管理
        HandCard(RC);
        //显示手牌
        HandCardShow(RC);
        //自己状态变化
        MyStatusRecover(RC);
    }
    //参数复原
    public void ParamentSetting(ReConnectData RC)
    {
        UserId.GameState = true;
        //
        Game_.reascalcount = RC.player.rascalCount;
        //机器人数
        Game_.robCount = RC.robotSize;
        //是否托管
        Game_.IsAutoPut = RC.IsAutoPut;
        //游戏类型
        UserId.GameType = RC.gameType;
        //桌号
        Game_.tableNum = RC.tableNum;
        Game_.PlayerNum = RC.turnNum;
        //当前轮数显示
        Game_.CurrentRound.text = RC.round.ToString();
        //缺少当前是第几局
        //用于结算的轮数
        Game_.Createround = RC.round;
        //桌号显示
        Game_.TableNumShow.text = Game_.tableNum.ToString();
        //癞子牌显示
        Game_.rascalCard = RC.rascalCard;
        Game_.isGameStart = false;
        Game_.DarwPai = RC.handCards;
        if (RC.handCards.Count > 0) {Game_.DrawCPai = RC.handCards[RC.handCards.Count - 1]; }
        else { Game_.DrawCPai = Game_.rascalCard; }
        Game_.OtherPaiShow.text = RC.darkCardsSize.ToString();
        Game_.OtherPaiShow.gameObject.SetActive(true);
        Game_.reascalPic.sprite = Game_.list[RC.rascalCard];
        Game_.reascalPic.gameObject.SetActive(true);
        Game_.NoGameStart = false;
        Game_.GameGetLiang = false;
        Game_.YQHY.gameObject.SetActive(false);
        if (Game_.tableNum > 100000)
        {
            UserId.JieCreateRoom = true;
            Game_.CurrentRound.text = RC.round.ToString();
            Game_.Createround = RC.round;
            Game_.CurrentRound.gameObject.SetActive(true);
            Game_.TableNumShow.gameObject.SetActive(true);
        }
    }
    //重新赋值位置
    public void SetNumGet(ReConnectData RC)
    {
        for (int i = 0; i < RC.idList.Count; i++)
        {
            if (!Game_.dic.ContainsKey(RC.idList[i]))
            {
                Game_.dic.Add(RC.idList[i], i + 1);
            }
            if (!Game_.playerId.Contains(RC.idList[i]))
            {
                Game_.playerId.Add(RC.idList[i]);
            }
            if (RC.idList[i] == UserId.memberId)
            {
                Game_.seatNum = i + 1;
                if (Game_.seatNum == 4)
                {
                    Game_.youplaycount = 1;
                    Game_.zuoplaycount = 3;
                    Game_.shangplaycount = 2;
                }
                else if (Game_.seatNum == 1)
                {
                    Game_.youplaycount = 2;
                    Game_.shangplaycount = 3;
                    Game_.zuoplaycount = 4;
                }
                else if (Game_.seatNum == 3)
                {
                    Game_.youplaycount = 4;
                    Game_.zuoplaycount = 2;
                    Game_.shangplaycount = 1;
                }
                else if (Game_.seatNum == 2)
                {
                    Game_.youplaycount = 3;
                    Game_.zuoplaycount = 1;
                    Game_.shangplaycount = 4;
                }
            }
        }
    }
    //玩家数据获取
    public void PlayerData(ReConnectData RC)
    {
        for (int i = 0; i < RC.idList.Count; i++) { 
         Member member = new Member();
        member.memberId = RC.idList[i].ToString();
        string json = JsonMapper.ToJson(member);
        JX.HttpCallSever.One().PostCallServer("http://" + Bridge.GetHostAndPort() + "/api/member/getinfo", json, Game_.PlayerInfo);
        }
    }
    //手牌管理
    public void HandCard(ReConnectData RC)
    {
        //吃碰杠暂存
        List<int> Chi = new List<int>();
        List<int> Peng = new List<int>();
        List<int> Gang = new List<int>();
        for (int i = 0; i < 4; i++) 
        { 
            int num = 0;
            if (i == Game_.seatNum - 1)
            {
            int rascalCount = RC.rascalCountSizeList[i];
            for (int j = 0; j < rascalCount; j++)
            {
                RC.handCards.Add(RC.rascalCard);
            }
            }
            num = RC.handCardsSizeList[i] + RC.skillCardsList[i].Count + RC.rascalCountSizeList[i];
            if (num > 13)
            {
                //根据数据与手牌进行对比管理手牌
                if (i == Game_.seatNum - 1)
                {
                    Game_.TurnNum(Game_.seatNum);
                    int Num1 = Game_.Pai.childCount;
                    for (int j = 0; j < num - Num1; j++)
                    {
                        GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "PaiRes_My", Game_.Pai);
                        go.transform.GetChild(0).gameObject.AddComponent<EventTrigger>();
                        go.transform.GetChild(0).gameObject.AddComponent<DragMajiang>();
                        go.name = "XP";
                        //添加点击事件
                        Transform childT = go.transform.GetChild(0);
                        childT.GetComponent<Image>().sprite = Game_.list[RC.handCards[RC.handCards.Count - 1]];
                        //添加点击事件
                        EventTrigger trigger = go.transform.GetChild(0).GetComponent<EventTrigger>();
                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerClick;
                        entry.callback = new EventTrigger.TriggerEvent();
                        entry.callback.AddListener(Game_.OnClick);
                        trigger.triggers.Add(entry);
                        RectTransform pos = go.transform as RectTransform;
                        pos.anchoredPosition = (Game_.Pai.GetChild(Game_.Pai.childCount - 2) as RectTransform).anchoredPosition;
                        pos.anchoredPosition += new Vector2(108, 0);
                    }
                }
                else if (i == Game_.youplaycount - 1)
                {  
                    OtherHandCardBuCard(Game_.youplaycount, Game_.You, num, "YouRes");
                }
                else if (i == Game_.zuoplaycount - 1)
                {
                    OtherHandCardBuCard(Game_.zuoplaycount, Game_.Zuo, num, "ZuoRes");
                }
                else if (i == Game_.shangplaycount - 1)
                {
                    OtherHandCardBuCard(Game_.shangplaycount, Game_.Shang, num, "ShangRes");
                }
            }
            else
            {   //刷新的只有自己的牌多了就去掉
                if (i == Game_.seatNum - 1)
                {
                    int a = Game_.Pai.childCount - num;
                    Debug.Log("A+++++++++" + a);
                    for (int j = 0; j < a; j++)
                    {
                        Destroy(Game_.Pai.GetChild(Game_.Pai.childCount - 1 - j).gameObject);
                    }
                }
            }
            if (Game_.WLBD == false)
            {
                //技能牌显示
                SkillShow(RC, i);
                //技能牌管理分类
                HandleSkill(RC, i, Chi, Peng, Gang);
            }
        }
        //分类好的技能显示
        ShowSkill(Chi, Peng, Gang);
    }
    //其他玩家管理手牌
    public void OtherHandCardBuCard(int ObjSeatNum,Transform ObjGame,int num,string Res)
    {
        Game_.TurnNum(ObjSeatNum);
        int Num2 = ObjGame.childCount;
        for (int j = 0; j < num - Num2; j++)
        {
            GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, Res, ObjGame);
            (go.transform as RectTransform).anchoredPosition = (ObjGame.GetChild(ObjGame.childCount - 2) as RectTransform).anchoredPosition;
            if (ObjGame != Game_.Shang) {
            (go.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);
            }
            else { (go.transform as RectTransform).anchoredPosition += new Vector2(68, 0); }
            go.transform.SetAsLastSibling();
        }
        Game_.CurrentDuixiang = ObjGame.GetChild(ObjGame.childCount - 1);
    }
    //技能牌显示
    public void SkillShow(ReConnectData RC,int i)
    {
        for (int j = 0; j < RC.skillCardsList[i].Count; j++) 
        { 
            //技能牌自己显示
            if (i == Game_.seatNum - 1)
            {
                Image pic = Game_.Pai.GetChild(j).GetComponent<Image>();
                pic.enabled = false;
                Game_.Pai.GetChild(j).GetChild(0).gameObject.SetActive(false);
                Transform t1 = Game_.Pai.GetChild(j).Find("skillpai");
                t1.gameObject.SetActive(true);
                t1.GetChild(0).GetComponent<Image>().sprite = Game_.list[RC.skillCardsList[i][j]];
            }
            else if (i == Game_.youplaycount - 1)
            {
                ShowOtherSkill(RC, i, Game_.You, j);
            }
            else if(i== Game_.shangplaycount - 1)
            {
                ShowOtherSkill(RC, i, Game_.Shang, j);
            }
            else if (i == Game_.zuoplaycount - 1)
            {
                ShowOtherSkill(RC, i, Game_.Zuo, j);
            }
        }
    }
    //其他玩家技能牌显示
    public void ShowOtherSkill(ReConnectData RC, int i,Transform ObjGame,int j)
    {
        Transform tf = ObjGame.GetChild(j);
        tf.GetChild(0).gameObject.SetActive(false);
        tf.GetChild(1).gameObject.SetActive(true);
        tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[RC.skillCardsList[i][j]];
    }
    //自己技能分下类
    public void HandleSkill(ReConnectData RC,int i,List<int> Chi, List<int> Peng, List<int> Gang)
    {
        if (i == Game_.seatNum - 1)
        {
            if (RC.skillCardsList[i].Count > 0)
            {
                int CurrentPoint = RC.skillCardsList[i][0];
                int CurrentIndex = 0;
                int SameCount = 0;
                int OtherCount = 0;
                for (int k = 0; k < RC.skillCardsList[i].Count; k++)
                {
                    if (CurrentPoint != RC.skillCardsList[i][k])
                    {
                        switch (SameCount)
                        {
                            case 2:
                                Peng.Add(CurrentIndex - 2);
                                SameCount = 0;
                                break;
                            case 3:
                                Gang.Add(CurrentIndex - 3);
                                SameCount = 0;
                                break;
                            default:
                                SameCount = 0;
                                break;
                        }
                        if ((CurrentPoint + 1) == RC.skillCardsList[i][k])
                        {
                            OtherCount++;
                        }
                        else
                        {
                            if (OtherCount == 2)
                            {
                                Chi.Add(CurrentIndex - 2);
                                OtherCount = 0;
                            }
                            else if (OtherCount == 3)
                            {
                                Chi.Add(CurrentIndex - 3);
                                OtherCount = 0;
                            }
                            else
                            {
                                OtherCount = 0;
                            }
                        }
                        CurrentPoint = RC.skillCardsList[i][k];
                        CurrentIndex = k;
                    }
                    else
                    {
                        CurrentIndex = k;
                        SameCount++;
                        if (k == 0) { SameCount -= 1; }
                    }
                    if (k == RC.skillCardsList[i].Count - 1)
                    {
                        if (SameCount == 2)
                        {
                            Peng.Add(CurrentIndex - 2);
                            SameCount = 0;
                        }
                        else if (SameCount == 3)
                        {
                            Gang.Add(CurrentIndex - 3);
                            SameCount = 0;
                        }
                        else if (OtherCount == 2)
                        {
                            Chi.Add(CurrentIndex - 2);
                            OtherCount = 0;
                        }
                        else if (OtherCount == 3)
                        {
                            Chi.Add(CurrentIndex - 3);
                            OtherCount = 0;
                        }
                    }
                }
            }
        }
    }
    //将排好的技能牌显示与位置变化
    public void ShowSkill(List<int> Chi, List<int> Peng, List<int> Gang)
    {
        for (int j = 0; j < Chi.Count; j++)
        {
            Debug.Log("ReCHi" + Chi[j]);
            for (int k = Chi[j] + 1; k < Chi[j] + 3; k++)
            {
                //防止重复更换位置
                if (Game_.Pai.childCount > k + 1)
                {
                    float a = Vector2.Distance((Game_.Pai.GetChild(k) as RectTransform).anchoredPosition, (Game_.Pai.GetChild(k + 1) as RectTransform).anchoredPosition);
                    if (a < 100)
                    {
                        Debug.Log(a);
                        break;
                    }
                }
                if (k == Chi[j] + 1) { (Game_.Pai.GetChild(k) as RectTransform).anchoredPosition -= new Vector2(14, 0); }
                else { (Game_.Pai.GetChild(k) as RectTransform).anchoredPosition -= new Vector2(28, 0); }
            }
        }
        for (int j = 0; j < Peng.Count; j++)
        {
            Debug.Log("RePeng" + Peng[j]);
            for (int k = Peng[j] + 1; k < Peng[j] + 3; k++)
            {
                float a = Vector2.Distance((Game_.Pai.GetChild(k) as RectTransform).anchoredPosition, (Game_.Pai.GetChild(k + 1) as RectTransform).anchoredPosition);
                if (a < 100)
                {
                    Debug.Log(a);
                    break;
                }
                if (k == Peng[j] + 1) { (Game_.Pai.GetChild(k) as RectTransform).anchoredPosition -= new Vector2(14, 0); }
                else { (Game_.Pai.GetChild(k) as RectTransform).anchoredPosition -= new Vector2(28, 0); }
            }
        }
        for (int j = 0; j < Gang.Count; j++)
        {
            Debug.Log("Gang" + Gang[j]);
            for (int k = Gang[j] + 1; k < Gang[j] + 4; k++)
            {
                float a = Vector2.Distance((Game_.Pai.GetChild(k) as RectTransform).anchoredPosition, (Game_.Pai.GetChild(k + 1) as RectTransform).anchoredPosition);
                if (a < 100)
                {
                    Debug.Log(a);
                    break;
                }
                if (k == Gang[j] + 1) { (Game_.Pai.GetChild(k) as RectTransform).anchoredPosition -= new Vector2(14, 0); }
                else if (k == Gang[j] + 2) { (Game_.Pai.GetChild(k) as RectTransform).anchoredPosition -= new Vector2(28, 0); }
                else
                {
                    RectTransform rect = (Game_.Pai.GetChild(k) as RectTransform);
                    rect.anchoredPosition = (Game_.Pai.GetChild(Gang[j] + 1) as RectTransform).anchoredPosition;
                    rect.anchoredPosition = (Game_.Pai.GetChild(k) as RectTransform).anchoredPosition;
                    rect.anchoredPosition += new Vector2(0, 29);
                }
            }
            for (int k = Gang[j] + 4; k < Game_.Pai.childCount; k++)
            {
                Debug.Log(Gang[j] + 4);
                (Game_.Pai.GetChild(k) as RectTransform).anchoredPosition -= new Vector2(100, 0);
            }
        }
    }
    //手牌显示打出牌清空以及重新显示打出牌
    public void HandCardShow(ReConnectData RC)
    {
        for (int i = 0; i < RC.handCards.Count; i++)
        {
            int Current = i + RC.skillCardsList[Game_.seatNum - 1].Count;
            Game_.Pai.GetChild(Current).GetChild(0).GetComponent<Image>().sprite = Game_.list[RC.handCards[i]];
        }
        if (Game_.WLBD == false)
        { 
            Game_.CleanData(Game_.YP);
            Game_.CleanData(Game_.SP);
            Game_.CleanData(Game_.ZP);
            Game_.CleanData(Game_.parent);
            //显示打出的牌
            for (int i = 0; i < RC.riverCards.Count; i++)
            {
                int AddNum = i % 4;
                ChoseAddNum(AddNum, RC.riverCards[i]);
            }
        }
    }
    //将断线重连的牌进行分类
    public void ChoseAddNum(int num, int picNum)
    {
        if (Game_.seatNum - 1 == num)
        {    
            GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "MajiangRes", Game_.parent);
            go.transform.GetChild(0).GetComponent<Image>().sprite = Game_.list[picNum];
        }
        else if (Game_.youplaycount - 1 == num)
        {
            //GameObject go = Prefabs.LoadCell("majiang#You_MaJiang", YP);
            GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "You_MaJiang", Game_.YP);
            go.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 270));
            go.transform.GetChild(0).rotation *= Quaternion.Euler(new Vector3(0, 0, 180));
            go.transform.GetChild(0).GetComponent<Image>().sprite = Game_.list[picNum];
        }
        else if (Game_.shangplaycount - 1 == num)
        {
            //GameObject go2 = Prefabs.LoadCell("majiang#ShangMajiangRes", SP);
            GameObject go2 = Bridge._instance.LoadAbDate(LoadAb.Game, "ShangMajiangRes", Game_.SP);
            go2.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 0));
            go2.transform.GetChild(0).rotation *= Quaternion.Euler(new Vector3(0, 0, 0));
            go2.transform.GetChild(0).GetComponent<Image>().sprite = Game_.list[picNum];
        }
        else if (Game_.zuoplaycount - 1 == num)
        {
            //GameObject go1 = Prefabs.LoadCell("majiang#Zuo_Majiang", ZP);
            GameObject go1 = Bridge._instance.LoadAbDate(LoadAb.Game, "Zuo_Majiang", Game_.ZP);
            Game_.DaoChuObject = go1;
            go1.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 90));
            go1.transform.GetChild(0).rotation *= Quaternion.Euler(new Vector3(0, 0, -90));
            go1.transform.GetChild(0).GetComponent<Image>().sprite = Game_.list[picNum];
        }
    }
    //自己状态转变
    public void MyStatusRecover(ReConnectData RC)
    {
        Transform tg = Game_.skillMap.Find("TuoGuang");
        Game_.StopCountDown();
        Game_.TurnNum(RC.turnNum);
        Game_.GameGetLiang = false;
        if (RC.turnNum == Game_.seatNum)
        {
            Game_.StartTG = true;
            Game_.activePTrusteeship = true;
            Game_.isOwn = true;
        }
        else
        {
            Game_.isOwn = false;
            Game_.StartTG = false;
        }
        foreach (var Key in RC.player.skillMap.Keys)
        {
            if (RC.player.skillMap[Key] == 1)
            {
                Debug.Log(Key);
                Game_.GuoPai();
                break;
            }
        }
        Game_.ShowSkillCard(RC.player.handCards, RC.player.skillCards, Game_.Pai, false);
        if (Game_.robCount >= 3)
        {
            Debug.Log("wwwwww");
            if(RC.player.playerStatus == 0)
            {
                tg.gameObject.SetActive(true);
                Game_.StartTG = true;
            }
            else
            {
                tg.gameObject.SetActive(false); 
                Game_.StartTG = false;
            }
        }
        else
        {
            if (RC.turnNum == Game_.seatNum) { StartCoroutine(Game_.ie()); }
            if (RC.player.playerStatus == 0)
            {
                Debug.Log("wwwwww1");
                Game_.StartTG = true;
                tg.gameObject.SetActive(true);
            }
            else if (RC.player.playerStatus == 1)
            {
                Debug.Log("wwwwww2");
                Game_.StartTG = false;
                tg.gameObject.SetActive(false);
            }
        }
    }
}
