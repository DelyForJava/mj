using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class btn_child : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    float x, y, z;


    public void OnPointerDown(PointerEventData eventData)
    {
        transform.parent.DOScale(new Vector3(x * 0.75f, y * 0.75f, z), 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Tweener t1 = transform.parent.DOScale(new Vector3(x * 1.5f, y * 1.5f, z), 0.005f);
        Tweener t2 = transform.parent.DOScale(new Vector3(x, y, z), 0.007f);

        Sequence seq = DOTween.Sequence();
        seq.Append(t1);
        seq.Append(t2);
    }

    void Awake()
    {
        x = transform.parent.localScale.x;
        y = transform.parent.localScale.y;
        z = transform.parent.localScale.z;
    }

}
