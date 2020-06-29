using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
        // 设置为切换场景不被销毁的属性
        GameObject.DontDestroyOnLoad(gameObject);

    }
}

