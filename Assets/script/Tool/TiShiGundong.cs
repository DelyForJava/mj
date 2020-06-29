using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TiShiGundong : MonoBehaviour {

    public Image _target1;//左侧目标位置
    public Image _target2;//右侧目标位置
    bool _check = true;
    bool _check2 = true;
    public Vector2 pos;
    void Start()
    {
        pos = (transform as RectTransform).position;
        StartCoroutine(IE_MoveToLeft());
    }

    IEnumerator IE_MoveToLeft()
    {
        while (_check)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, _target1.transform.localPosition, Time.deltaTime * 50f);
            // transform.Translate(Vector3.left, Space.Self);
            if (Mathf.Abs(_target1.transform.localPosition.x - transform.localPosition.x) < 0.2f)
            {
                _check = false;
                _check2 = true;
                StartCoroutine(IE_MoveToRight());
            }
            yield return null;
           
        }
    }

    void Update()
    {
        if (_check2)
        {
            //(transform as RectTransform).position = pos;
            transform.position = _target2.transform.position;
            _check2 = false;
            _check = true;
            StartCoroutine(IE_MoveToLeft());
        }
    }

    IEnumerator IE_MoveToRight()
    {
        while (_check2)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, _target2.transform.localPosition, Time.deltaTime * 50f);
            // transform.Translate(Vector3.left, Space.Self);
            if (Mathf.Abs(_target2.transform.localPosition.x - transform.localPosition.x) < 0.2f)
            {
                _check2 = false;
                _check = true;
                StartCoroutine(IE_MoveToLeft());
            }
            yield return null;
        }
    }
}
