using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragMajiang : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Canvas canvas;
    public Camera camera1;
    public Vector2 CPos;
    public Vector2 mousePos;

	public Transform Pai;
	public Transform parent;
    public Transform Obj;
	public Game_Controller gm;
    public int Count;
	public bool isOwn;
    public bool StartOwn;
	void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        camera1 = Camera.main;
		gm=GameObject.Find("Gold_Game").GetComponent<Game_Controller>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        Vector3 p=Camera.main.WorldToScreenPoint(Input.mousePosition);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
          (transform.parent as RectTransform, p, camera1, out pos))
            {
            (transform.parent as RectTransform).anchoredPosition += new Vector2(0, pos.y);
             }
        Vector2 Cc = (transform.parent as RectTransform).anchoredPosition;
        float ve2 = Vector2.Distance(CPos, Cc);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Vector2.Distance((transform.parent as RectTransform).anchoredPosition,CPos)>120)
        {
            StartOwn = true;
        }
        Debug.Log("StartOwn" + StartOwn);
        MaJiang.Instance.Majiang_ = null;
        if ((transform.parent as RectTransform).anchoredPosition.x!=CPos.x)
        {
            CPos.x = (transform.parent as RectTransform).anchoredPosition.x;
        }
        (transform.parent as RectTransform).anchoredPosition = CPos;
        if (gm.isOwn)
        {
            if (StartOwn)
            {
               // Debug.LogError("Start");
                StartOwn = false;
                Count = 0;
                Obj.gameObject.SetActive(false);
                Image evet = Obj.GetChild(0).GetComponent<Image>();
                gm.DragMajiang(Obj, evet);
                return;
            }
           
        }
        Vector2 ObjPos = (Obj as RectTransform).anchoredPosition;
        if (ObjPos.y != 21)
        {
            (Obj as RectTransform).anchoredPosition -= new Vector2(0, ((Obj as RectTransform).anchoredPosition.y - 21));
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Count==0)
        {
            Count++;
            CPos = (transform.parent as RectTransform).anchoredPosition;
        }
        Obj = eventData.pointerPress.transform.parent;
        Debug.Log(eventData.pointerPress.transform.gameObject.name);
    }
}
