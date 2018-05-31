using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    //预设一个实例的引用，用于保持对本类唯一对象的引用
    private static T m_Instance = null;
    //创建对象的接口 --全局创建点, 在项目中的任何一个地方需要该类型的对象时调用SingleTon.Instance
    public static T instance
    {
        get
        {
            if (m_Instance == null)
            {
                //在场景中查找中是否已经有该类型对象
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (m_Instance == null)
                {
                    //创建一个空物体,并挂载对象,然后再从物体上取得该对象
                    m_Instance = new GameObject("Singleton of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    m_Instance.Init();
                    DontDestroyOnLoad(m_Instance);
                }
            }
            return m_Instance;
        }

    }
    //如果手动已经挂在某物体上了，此时对象已经创建，所以将本对象交由m_Instance引用来管理
    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
        }
    }
    //如果本对象在使用之前需要做一个初始化操作，请在子类中重写
    public virtual void Init() { }
    //应用程序结束时，将本对象设为null，以便垃圾回收来释放
    private void OnApplicationQuit()
    {
        m_Instance = null;
    }
}
