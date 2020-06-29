using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class buoy : MonoBehaviour
{
    public Transform Info;
    public Text InfoTxt;

    void Start()
    {    
        Info.DOLocalMoveY(0f, 0.1f).OnComplete(() =>
        {
            Info.DOLocalMoveY(100, 2.0f).OnComplete(() =>
            {
                Destroy(gameObject);            
            });
        });
    }
}
