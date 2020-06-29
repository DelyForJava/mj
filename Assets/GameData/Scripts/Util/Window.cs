using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Window
{
    private GameObject gameObject;
    //引用GameObject
    public GameObject GameObject
    {
        get { return gameObject; }
        set { gameObject = value; }
    }

    private Transform transform;
    //引用Transform
    public Transform Transform
    {
        get { return transform; }
        set { transform = value; }
    }

    private string name;
    //名字
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private bool resourece;
    public bool Resource
    {
        get { return resourece; }
        set { resourece = value; }
    }

    private bool isHotFix;
    public bool IsHotFix
    {
        get { return isHotFix; }
        set { isHotFix = value; }
    }

    private string hotFixClassName;
    public string HotFixClassName
    {
        get { return hotFixClassName; }
        set { hotFixClassName = value; }
    }

    //所有的Button
    protected List<Button> m_AllButton = new List<Button>();

    public virtual void Awake(object param1 = null, object param2 = null, object param3 = null) { }

    public virtual void OnShow(object param1 = null, object param2 = null, object param3 = null) { }

    public virtual void OnDisable() { }

    public virtual void OnUpdate() { }

    public virtual void OnClose()
    {
        RemoveAllButtonListener();
        m_AllButton.Clear();
    }

    /// <summary>
    /// 同步替换图片
    /// </summary>
    /// <param name="path"></param>
    /// <param name="image"></param>
    /// <param name="setNativeSize"></param>
    /// <returns></returns>
    public bool ChangeImageSprite(string name, Image image, bool setNativeSize = false)
    {
        Debug.Log(name);

        if (image == null)
            return false;

        Sprite sp = ResourceManager.Instance.HotFixLoaderSprite(name);
        Debug.Log(sp);
        if (sp != null)
        {
            if (image.sprite != null)
                image.sprite = null;
            Debug.Log(image.sprite);
            image.sprite = sp;
            image.SetNativeSize();
            Debug.Log(image.sprite);
            if (setNativeSize)
            {
                image.SetNativeSize();
            }
            return true;
        }

        return false;
    }

    /// <summary>
    /// 移除所有的button事件
    /// </summary>
    public void RemoveAllButtonListener()
    {
        foreach (Button btn in m_AllButton)
        {
            btn.onClick.RemoveAllListeners();
        }
    }


    /// <summary>
    /// 添加button事件监听
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="action"></param>
    public void AddButtonClickListener(Button btn, UnityEngine.Events.UnityAction action)
    {
        if (btn != null)
        {
            if (!m_AllButton.Contains(btn))
            {
                m_AllButton.Add(btn);
            }
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
            btn.onClick.AddListener(BtnPlaySound);
        }
    }

   

    /// <summary>
    /// 播放button声音
    /// </summary>
    void BtnPlaySound()
    {

    }
}
