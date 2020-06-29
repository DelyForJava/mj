using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BubbleUIController : MonoBehaviour {

    public Transform Info;
    public Image InfoImg;
    public Text InfoTxt;

    void Start()
    {
        Info.DOLocalMoveY(0f, 0.1f).OnComplete(() =>
        {
            InfoImg.DOFade(0f, 0.3f).SetDelay(1f);
            InfoTxt.DOFade(0f, 0.3f).SetDelay(1f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }
}
