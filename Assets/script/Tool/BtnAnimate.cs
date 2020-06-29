using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BtnAnimate : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerClickHandler
{
    float x,y, z;

    public void OnPointerClick(PointerEventData eventData)
    {

        transform.DOScale(new Vector3(x, y, z), 0.02f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(x*0.75f, y * 0.75f, z), 0.05f);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Tweener t1 = transform.DOScale(new Vector3(x * 1.5f,y * 1.5f, z), 0.005f);
        Tweener t2 = transform.DOScale(new Vector3(x, y, z), 0.005f);

        Sequence seq = DOTween.Sequence();
        seq.Append(t1);
        seq.Append(t2);
    }

    void Awake()
    {
        x = transform.localScale.x;
        y = transform.localScale.y;
        z = transform.localScale.z;
    }
}
