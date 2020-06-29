using ILRuntime.Runtime;
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GangActionController : MonoBehaviour {
    public Game_Controller Game_;
    public void GangAction(string edate)
    {
        GangInfo d = JsonMapper.ToObject<GangInfo>(edate);
        if (d.seatNum == Game_.seatNum)
            Game_.activePTrusteeship = true;
        else
            Game_.activePTrusteeship = false;
        //杠摸牌    
        GangShowCard(d);
        Game_.StopCountDown();
        //抢杠的监测
        RobGang(d);
    }
    public void GangAnimator(GangInfo Gang)
    {
        if (Gang.seatNum == Game_.youplaycount)
            StartCoroutine(Game_.SkillTShow("You", "gang"));
        else if (Gang.seatNum == Game_.zuoplaycount)
            StartCoroutine(Game_.SkillTShow("Zuo", "gang"));
        else if (Gang.seatNum == Game_.shangplaycount)
            StartCoroutine(Game_.SkillTShow("Shang", "gang"));
    }
    //不同情况下的Gang摸牌
    public void GangShowCard(GangInfo d)
    {
        if (d.seatNum == Game_.seatNum)
        {
            Game_.PlayerNum = Game_.seatNum;
            if (Game_.GangT3)
            { 
                //碰后杠
                PengAfterGang(d);
            }
            else
            {
                if (Game_.isMingGang)
                {
                    Debug.Log("暗gang");
                    //暗杠处理把之前补到的牌放好等待下张补牌
                    (Game_.DaoChuObject.transform as RectTransform).anchoredPosition = (Game_.Pai.GetChild(Game_.Pai.childCount - 2) as RectTransform).anchoredPosition;
                    (Game_.DaoChuObject.transform as RectTransform).anchoredPosition += new Vector2(108, 0);
                     //Game_.isMingGang = false;
                }
                else
                {
                    Debug.Log("明杠");
                    //别人打出牌杠的要先
                    Destroy(Game_.DaoChuObject);
                    // 新生成对象
                    CreatAndClick();
                }               
                //刷新玩家手牌
                Game_.ShowSkillCard(d.handCards, d.skillCards, Game_.Pai, true);
                Game_.RefreshSkill(d.skillCards, Game_.Pai);
                Game_.isMyGang = true;
            }
        }
        if (d.seatNum == Game_.youplaycount)
        {
            if (Game_.YouGangT3)
            {
                Debug.Log("YouT3");
                OtherPengAfterGang(d, Game_.You, Game_.YouGangT3);
            }
            else
            {   //明杠将摸到的牌放好
                if (Game_.isYouMingGang)
                {
                    (Game_.DaoChuObject.transform as RectTransform).anchoredPosition = (Game_.You.GetChild(Game_.You.childCount - 2) as RectTransform).anchoredPosition;
                    (Game_.DaoChuObject.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);
                    //Game_.isYouMingGang = false;
                }
                else//暗杠将别人打出的牌消除
                {
                    Destroy(Game_.DaoChuObject);
                    GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "YouRes", Game_.You);
                    (go.transform as RectTransform).anchoredPosition = (Game_.You.GetChild(Game_.You.childCount - 2) as RectTransform).anchoredPosition;
                    (go.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);
                }
                GangShow(d, Game_.You);
            }
        }
        if (d.seatNum == Game_.shangplaycount)
        {
            if (Game_.ShangGangT3)
            {   //上杠不仅要放在指定位置还要提高y轴
                OtherPengAfterGang(d, Game_.Shang, Game_.ShangGangT3);
            }
            else
            {   
                if (Game_.isShangMingGang)
                {
                    (Game_.DaoChuObject.transform as RectTransform).anchoredPosition = (Game_.Shang.GetChild(Game_.Shang.childCount - 2) as RectTransform).anchoredPosition;
                    (Game_.DaoChuObject.transform as RectTransform).anchoredPosition += new Vector2(68, 0);
                    //Game_.isShangMingGang = false;
                }
                else
                {
                    Destroy(Game_.DaoChuObject);
                    GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "ShangRes", Game_.Shang);
                    (go.transform as RectTransform).anchoredPosition = (Game_.Shang.GetChild(Game_.Shang.childCount - 2) as RectTransform).anchoredPosition;
                    (go.transform as RectTransform).anchoredPosition += new Vector2(68, 0);
                }
                GangShow(d, Game_.Shang);
            }
        }
        if (d.seatNum == Game_.zuoplaycount)
        {
            if (Game_.ZuoGangT3)
            {
                //左碰后杠只要放在指定位置等待摸牌
                OtherPengAfterGang(d,Game_.Zuo,Game_.ZuoGangT3);
            }
            else
            {
                if (Game_.isZuoMingGang)
                {
                    (Game_.DaoChuObject.transform as RectTransform).anchoredPosition = (Game_.Zuo.GetChild(Game_.Zuo.childCount - 2) as RectTransform).anchoredPosition;
                    (Game_.DaoChuObject.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);
                    // Game_.isZuoMingGang = false;
                }
                else
                {
                    Destroy(Game_.DaoChuObject);
                    GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "ZuoRes", Game_.Zuo);
                    (go.transform as RectTransform).anchoredPosition = (Game_.Zuo.GetChild(Game_.Zuo.childCount - 2) as RectTransform).anchoredPosition;
                    (go.transform as RectTransform).anchoredPosition -= new Vector2(0, 50);
                }
                GangShow(d, Game_.Zuo);
            }
        }
    }
    //碰后杠
    public void PengAfterGang(GangInfo d)
    {
        Debug.Log("碰后杠");
        Transform tar = Game_.Pai.GetChild(Game_.Pai.childCount - 1);
        tar.GetComponent<Image>().enabled = false;
        tar.GetChild(0).gameObject.SetActive(false);
        //tar.GetChild(1).gameObject.SetActive(true);
        //tar.GetChild(1).GetChild(0).GetComponent<Image>().sprite = tar.GetChild(0).GetComponent<Image>().sprite;
        DarkCardShow(tar,"prefabs/DarkGang",Game_.Pai);
        int SkillGangPoint = 0;
        for (int i = 0; i < d.skillCards.Count; i++)
        {
            if (d.skillCards[i] == Game_.DrawCPai)
            {
                SkillGangPoint = i;
                break;
            }
        }
        int NG = SkillGangPoint;
        Debug.Log(NG);
        ////找到要磊的目标
        RectTransform rect = (Game_.Pai.GetChild(NG + 1) as RectTransform);
        Debug.Log(NG + 1);
        //生成了一个直接放在碰牌上的;
        tar.transform.SetSiblingIndex(NG + 2);
        (tar as RectTransform).anchoredPosition = rect.anchoredPosition;
        (tar as RectTransform).anchoredPosition += new Vector2(0, 29);
         Game_.RefashCard(d.handCards, d.skillCards,Game_.reascalcount);
         Game_.GangT3 = false;
         Game_.isMyGang = false;
    }
    //明杠，暗杠生成牌后加入点击事件，后等待服务器补牌
    public void CreatAndClick()
    {
        GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "PaiRes_My", Game_.Pai);
        go.transform.GetChild(0).gameObject.AddComponent<EventTrigger>();
        go.transform.GetChild(0).gameObject.AddComponent<DragMajiang>();
        go.name = "XP";
        Transform childT = go.transform.GetChild(0);
        childT.GetComponent<Image>().sprite = Game_.list[Game_.DrawCPai];
        //添加点击事件
        EventTrigger trigger = go.transform.GetChild(0).GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(Game_.OnClick);
        trigger.triggers.Add(entry);
        //位置的放置
        RectTransform pos = go.transform as RectTransform;
        (pos.transform as RectTransform).anchoredPosition = (Game_.Pai.GetChild(Game_.Pai.childCount - 2) as RectTransform).anchoredPosition;
        (pos.transform as RectTransform).anchoredPosition += new Vector2(108, 0);
    }
    //其他玩家的吃碰杠
    public void OtherPengAfterGang(GangInfo d,Transform ObjGame,bool ObjGang_)
    {
        GameObject go = ObjGame.GetChild(ObjGame.childCount - 1).gameObject;
        go.transform.GetChild(0).gameObject.SetActive(false);
        go.transform.GetChild(1).gameObject.SetActive(true);
        go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[Game_.DrawCPai];
        int SkillGangPoint = 0;
        for (int i = 0; i < d.skillCards.Count; i++)
        {
            if (d.skillCards[i] == Game_.DrawCPai)
            {
                SkillGangPoint = i;
                break;
            }
        }
        int NG = SkillGangPoint;
        //找到要磊的目标
        RectTransform rect = (ObjGame.GetChild(NG + 1) as RectTransform);
        if (ObjGame == Game_.Zuo) { DarkCardShow(rect, "prefabs/DarkZYCard", Game_.Zuo); }
        else if (ObjGame == Game_.You) { DarkCardShow(rect, "prefabs/DarkZYCard", Game_.You); }
        else if (ObjGame == Game_.Shang) { DarkCardShow(go.transform,"prefabs/DarkGang", Game_.Shang); }
        (go.transform as RectTransform).anchoredPosition = rect.anchoredPosition;
        if (ObjGame == Game_.Shang)
        {
            (go.transform as RectTransform).anchoredPosition += new Vector2(0, 18);
        }
        else
        {
            Debug.Log("Gang+++++++++++++++++++");
            rect.anchoredPosition += new Vector2(0, 20);
        }
        if(ObjGame == Game_.Shang) { go.transform.SetSiblingIndex(NG + 2); } 
        else { go.transform.SetSiblingIndex(NG + 1); }
        ObjGang_ = false;
        Debug.Log(ObjGang_);
    }
    //"prefabs/DarkGang"
    public void DarkCardShow(Transform ObjGame,string path,Transform Aim)
    {
        GameObject go = Resources.Load<GameObject>(path);
        Transform t = GameObject.Instantiate(go).transform;
        t.SetParent(ObjGame);
        RectTransform rect = t as RectTransform;
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
        if (Aim == Game_.Pai){ rect.anchoredPosition += new Vector2(-5.3f, -14.8f);}
        else if (Aim == Game_.You) { rect.anchoredPosition += new Vector2(-9.8f, 18);}
        else if (Aim == Game_.Shang)
        {   (rect.GetChild(0) as RectTransform).sizeDelta = new Vector2(70.7f, 91.5f);
            (rect.GetChild(0) as RectTransform).anchoredPosition = new Vector2(19.9f, -16.9f);
        }
    }
    //显示杠后的牌
    public void GangShow(GangInfo d,Transform ObjGame)
    {
        for (int i = 0; i < d.skillCards.Count; i++)
        {
            Transform tf = ObjGame.GetChild(i);
            tf.GetChild(0).gameObject.SetActive(false);
            tf.GetChild(1).gameObject.SetActive(true);
            tf.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[d.skillCards[i]];
        }
        GangPosit(d, ObjGame);
       
    }
    //杠位置调整
    public void GangPosit(GangInfo d, Transform ObjGame)
    {
        Debug.Log("GangAction"); 
        if (ObjGame == Game_.Shang)
        {
            RectTransform rect = (ObjGame.GetChild(d.skillCards.Count - 1) as RectTransform);
            if (rect.anchoredPosition != (ObjGame.GetChild(d.skillCards.Count - 3) as RectTransform).anchoredPosition)
            {
                rect.anchoredPosition = (ObjGame.GetChild(d.skillCards.Count - 3) as RectTransform).anchoredPosition;
                rect.anchoredPosition += new Vector2(0, 20);
                for (int i = d.skillCards.Count; i < ObjGame.childCount; i++)
                {
                    (ObjGame.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(60, 0);
                }
            }
            rect.SetSiblingIndex(d.skillCards.Count - 2);
            if (Game_.isShangMingGang) {
                Game_.isShangMingGang = false;
                DarkCardShow(ObjGame.GetChild(d.skillCards.Count - 2) as RectTransform, "prefabs/DarkGang", Game_.Shang); }
        }
        else
        {
            RectTransform rect = (ObjGame.GetChild(d.skillCards.Count - 1) as RectTransform);
            if (rect.anchoredPosition != (ObjGame.GetChild(d.skillCards.Count - 3) as RectTransform).anchoredPosition)
            {
                rect.anchoredPosition = (ObjGame.GetChild(d.skillCards.Count - 3) as RectTransform).anchoredPosition;
                rect.anchoredPosition += new Vector2(0, 20);
                for (int i = d.skillCards.Count; i < ObjGame.childCount; i++)
                {
                    (ObjGame.GetChild(i) as RectTransform).anchoredPosition += new Vector2(0, 50);
                }
            }
            rect.SetSiblingIndex(d.skillCards.Count - 2);
            if (ObjGame == Game_.Zuo&& Game_.isZuoMingGang) 
            { 
                    Game_.isZuoMingGang = false;
                    DarkCardShow(ObjGame.GetChild(d.skillCards.Count - 2) as RectTransform, "prefabs/DarkZYCard", Game_.Zuo);
            }
            else if (ObjGame == Game_.You && Game_.isYouMingGang) {
                Game_.isYouMingGang = false;
                DarkCardShow(rect, "prefabs/DarkZYCard", Game_.You); }
        }
    }
    //抢杠
    public void RobGang(GangInfo d)
    {
        Game_.StopCountDown();
        Game_.StartCountDown();
        if (Game_.GrabCards == 0) { Game_.GrabCards = Game_.DrawCPai;}
        if (d.rob==Game_.seatNum)
        {
            Game_.skillMap.Find("skillone/guo").gameObject.SetActive(true);
            GameObject QG = Game_.skillMap.Find("qianggang").gameObject;
            QG.SetActive(true);
            QG.GetComponent<Button>().onClick.RemoveAllListeners();
            QG.GetComponent<Button>().onClick.AddListener(() =>
            {
                Game_.jie.HuCard = Game_.GrabCards;
                QG.SetActive(false);
                GameObject go = Bridge._instance.LoadAbDate(LoadAb.Game, "PaiRes_My", Game_.Pai);
                (go.transform as RectTransform).anchoredPosition = (Game_.Pai.GetChild(Game_.Pai.childCount - 2) as RectTransform).anchoredPosition;
                (go.transform as RectTransform).anchoredPosition += new Vector2(184, 0);
                Transform childT = go.transform.GetChild(0);
                go.transform.GetChild(0).gameObject.SetActive(false);
                go.transform.GetChild(1).gameObject.SetActive(true);
                go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Game_.list[Game_.GrabCards];
                go.GetComponent<Image>().enabled = false;
                childT.GetComponent<Image>().sprite = Game_.list[Game_.GrabCards];
                Game_.HuPaiCard = Game_.GrabCards;
                ActionInfo hu = new ActionInfo();
                hu.actionCode = "HuAction";
                hu.Params = new ActionInfo.Data();
                string hus = JsonMapper.ToJson(hu);
                WebSoketCall.One().SendToWeb(hus);
                QianGangJuge(d.seatNum, d.skillCards);
            });
        }
        else
        {
            if (Game_.isOwn == true)
            {
                Game_.isOwn = false;
            }
        }
    }
    //除抢杠玩家的判断
    public void QianGangJuge(int seatNum_, List<int> skillCards)
    {
        if (seatNum_ == Game_.seatNum)
        {
            if (!Game_.G3bool)
            {
                Game_.Pai.GetChild(skillCards.Count - 1).gameObject.SetActive(false);
            }
            else
            {
                Game_.G3bool = false;
                Game_.Pai.GetChild(Game_.G3Position).gameObject.SetActive(false);
            }
            for (int i = skillCards.Count - 3; i < skillCards.Count; i++)
            {
                if (i == skillCards.Count - 1)
                {
                    break;
                }
                else if (i == skillCards.Count - 3)
                {
                    (Game_.Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(14, 0);
                }
                else
                {
                    (Game_.Pai.GetChild(i) as RectTransform).anchoredPosition -= new Vector2(28, 0);
                }
            }
        }
        else if (seatNum_ == Game_.youplaycount)
        {
            if (!Game_.G3bool)
            {
                Game_.You.GetChild(skillCards.Count - 1).gameObject.SetActive(false);
            }
            else
            {
                Game_.G3bool = false;
                Game_.You.GetChild(Game_.G3Position).gameObject.SetActive(false);
            }
        }
        else if (seatNum_ == Game_.shangplaycount)
        {
            if (!Game_.G3bool)
            {
                Game_.Shang.GetChild(skillCards.Count - 1).gameObject.SetActive(false);
            }
            else
            {
                Game_.G3bool = false;
                Game_.Shang.GetChild(Game_.G3Position).gameObject.SetActive(false);
            }
        }
        else if (seatNum_ == Game_.zuoplaycount)
        {
            if (!Game_.G3bool)
            {
                Game_.Zuo.GetChild(skillCards.Count - 1).gameObject.SetActive(false);
            }
            else
            {
                Game_.G3bool = false;
                Game_.Zuo.GetChild(Game_.G3Position).gameObject.SetActive(false);
            }
        }
    }

    public int LeipaiTarget(List<int> SkillCards,int Card)
    {
        int ObjTim=0;
        for (int i = 0; i < SkillCards.Count; i++)
        {
            if (SkillCards[i] == Card)
            {
                ObjTim = i;
            }
        }
        return ObjTim;
    }
}
