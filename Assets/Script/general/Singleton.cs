using UnityEngine;

public abstract class Singleton<T>:MonoBehaviour where T : MonoBehaviour
{
    public static T instance{get;private set;}
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance=this as T;//instance是T类型的，而this是singleton类型的，所以要强制类型转换。
    }
    protected virtual void OnApplicationQuit()
    {
        instance=null;
        Destroy(gameObject);
    }
}

public abstract class SingletonPersistent<T>: Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    } 
}