using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class ILRuntimeManager : Singleton<ILRuntimeManager> 
{
    private MonoBehaviour m_Mono;

    ILRuntime.Runtime.Enviorment.AppDomain appdomain;

    System.IO.MemoryStream fs;
    System.IO.MemoryStream p;

    public ILRuntime.Runtime.Enviorment.AppDomain ILRunAppDomain
    {
        get { return appdomain; }
    }
    public void Init(MonoBehaviour mono)
    {
        m_Mono = mono;
        m_Mono.StartCoroutine(LoadModelFromLocal());
        TestInfo.Instance.ShowTxt("ILRuntime初始化");
    }
    public IEnumerator LoadModelFromLocal()
    {
        string xmlUrl=string.Empty;
       #if UNITY_EDITOR_WIN || UNITY_EDITOR || UNITY_EDITOR_OSX
        xmlUrl =URL.EDITORORWIN_HOTFIX;
#elif UNITY_ANDROID
        xmlUrl = URL.ANDROID_HOTFIX;
#elif UNITY_IOS
        xmlUrl = URL.IOS_HOTFIX;
#endif
        UnityWebRequest webRequest =UnityWebRequest.Get(xmlUrl);

#if UNITY_5_6
        yield return webRequest.Send();
#else
        yield return webRequest.SendWebRequest();
#endif
        byte[] dll=webRequest.downloadHandler.data;
       TestInfo.Instance.ShowTxt(dll.Length.ToString());
        //fs = new MemoryStream(dll);
        appdomain=new ILRuntime.Runtime.Enviorment.AppDomain();
        appdomain.LoadAssembly(new MemoryStream(dll), null, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        InitializeILRuntime();
        OnHotFixLoaded();
    }
    
    void InitializeILRuntime()
    {
        appdomain.DelegateManager.RegisterMethodDelegate<System.Object>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.Object,System.Object > ();
        appdomain.DelegateManager.RegisterMethodDelegate<System.Object, System.Object,System.Object>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.Object,UnityEngine.Object, System.Object>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.String, UnityEngine.Object, System.Object, System.Object>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.String>();

        //注册button点击事件

        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((action) =>
        {
            return new UnityEngine.Events.UnityAction(() =>
            {
                ((System.Action)action)();
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Action<System.String>>((act) =>
        {
            return new System.Action<System.String>((obj) =>
            {
                ((Action<System.String>)act)(obj);
            });
        });
        //注册适配器
        appdomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
        appdomain.RegisterCrossBindingAdaptor(new WindowAdapter());
        //appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
        //CLR重定向Get，add方法
        SetupCLRAddCompontent();
        SetUpCLRGetCompontent();
        
        //绑定
        //ILRuntime.Runtime.Generated.CLRBindings.Initialize(appdomain);
    }

    void OnHotFixLoaded()
    {
        TestInfo.Instance.ShowTxt("注册面板测试------------------------");
        appdomain.Invoke("HotFix_Project.OriginClass", "Init", null, null);
    }
    public void DoCoroutine(IEnumerator coroutine)
    {
       m_Mono.StartCoroutine(coroutine);
    }
    //CLR重定向
    private unsafe void SetUpCLRGetCompontent()
    {
        var arr = typeof(GameObject).GetMethods();
        foreach (var i in arr)
        {
            if (i.Name == "GetCompontent" && i.GetGenericArguments().Length == 1)
            {
                appdomain.RegisterCLRMethodRedirection(i, GetCompontent);
            }
        }
    }
    //CLR重定向
    private unsafe StackObject* GetCompontent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

        var ptr = __esp - 1;
        GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
        if (instance == null)
            throw new System.NullReferenceException();

        __intp.Free(ptr);

        var genericArgument = __method.GenericArguments;
        if (genericArgument != null && genericArgument.Length == 1)
        {
            var type = genericArgument[0];
            object res = null;
            if (type is CLRType)
            {
                res = instance.GetComponent(type.TypeForCLR);
            }
            else
            {
                var clrInstances = instance.GetComponents<MonoBehaviourAdapter.Adaptor>();
                foreach (var clrInstance in clrInstances)
                {
                    if (clrInstance.ILInstance != null)
                    {
                        if (clrInstance.ILInstance.Type == type)
                        {
                            res = clrInstance.ILInstance;
                            break;
                        }
                    }
                }
            }

            return ILIntepreter.PushObject(ptr, __mStack, res);
        }

        return __esp;
    }
    //CLR重定向
    unsafe void SetupCLRAddCompontent()
    {
        var arr = typeof(GameObject).GetMethods();
        foreach (var i in arr)
        {
            if (i.Name == "AddComponent" && i.GetGenericArguments().Length == 1)
            {
                appdomain.RegisterCLRMethodRedirection(i, AddCompontent);
            }
        }
    }
    //CLR重定向
    private unsafe StackObject* AddCompontent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

        var ptr = __esp - 1;
        GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
        if (instance == null)
        {
            throw new System.NullReferenceException();
        }
        __intp.Free(ptr);

        var genericArgument = __method.GenericArguments;
        if (genericArgument != null && genericArgument.Length == 1)
        {
            var type = genericArgument[0];
            object res;
            if (type is CLRType)//CLRType表示这个类型是Unity工程里的类型   //ILType表示是热更dll里面的类型
            {
                //Unity主工程的类，不需要做处理
                res = instance.AddComponent(type.TypeForCLR);
            }
            else
            {
                //创建出来MonoTest
                var ilInstance = new ILTypeInstance(type as ILType, false);
                var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adaptor>();
                clrInstance.ILInstance = ilInstance;
                clrInstance.AppDomain = __domain;
                //这个实例默认创建的CLRInstance不是通过AddCompontent出来的有效实例，所以要替换
                ilInstance.CLRInstance = clrInstance;

                res = clrInstance.ILInstance;

                //补掉Awake
                clrInstance.Awake();
            }
            return ILIntepreter.PushObject(ptr, __mStack, res);
        }

        return __esp;
    }
}

	
