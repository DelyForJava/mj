using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MaJiang : MonoBehaviour {

	 public static MaJiang Instance;
	
	//服务器获取的数字
	 public int PicCount;
	 public  Sprite Impic;
	 public  GameObject Majiang_;
	 public List<Sprite> NowPai = new List<Sprite>();
	 
	void Awake()
	{
		Instance = gameObject.GetComponent<MaJiang>();
	}
	private MaJiang() { }

	public GameObject SelectGameObject(GameObject go)
	{
		Majiang_ = go;
		Impic = Majiang_.transform.GetChild(0).GetComponent<Image>().sprite;
		return Majiang_;		
	}
	
}
