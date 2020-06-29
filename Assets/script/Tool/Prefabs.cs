using UnityEngine;
public class Prefabs : MonoBehaviour {

    //用于生成页面对象
    public static GameObject Load(string path)
    {
        //加载页面预制体
        GameObject prefab = ABManager.Instance.LoadAsset<GameObject>(path);
        Debug.Log("prefab===="+ prefab);
        //实例化页面
        GameObject page = GameObject.Instantiate<GameObject>(prefab);
        //名字去掉Clone
        page.name = prefab.name;

        RectTransform rtf = page.transform as RectTransform;

        //显示在Canvas下
        rtf.SetParent(GameObject.Find("/Canvas").transform);
        //Transform初始化一下
        rtf.localPosition = Vector3.zero;
        rtf.localRotation = Quaternion.identity;
        rtf.localScale = Vector3.one;
        //把当前物体放在同级最后一个
        rtf.SetAsLastSibling();

        //四锚点归零
        rtf.offsetMin = Vector2.zero;
        rtf.offsetMax = Vector2.zero;

        return page;
    }

 

    //用于生成Cell对象
    public static GameObject LoadCell(string path, Transform grid)
    {
        //加载页面预制体
        GameObject prefab = ABManager.Instance.LoadAsset<GameObject>(path);
        //实例化页面
        GameObject page = GameObject.Instantiate<GameObject>(prefab);
        //名字去掉Clone
        page.name = prefab.name;

        RectTransform rtf = page.transform as RectTransform;

        //显示在Canvas下
        rtf.SetParent(grid);
        //Transform初始化一下
        rtf.localPosition = Vector3.zero;
        rtf.localRotation = Quaternion.identity;
        rtf.localScale = Vector3.one;
        //把当前物体放在同级最后一个
        rtf.SetAsLastSibling();

        return page;
    }

    public static void PopBubble(string message)
    {
        GameObject obj =Bridge._instance.LoadAbDate(LoadAb.Main, "Bubble");
        obj.GetComponent<BubbleUIController>().InfoTxt.text = message;
    }

    public static void Buoy(string message)
    {
        GameObject obj =Bridge._instance.LoadAbDate(LoadAb.Main, "buoy");       
        obj.GetComponent<buoy>().InfoTxt.text = message;
    }

}
