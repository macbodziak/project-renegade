using UnityEngine;

public abstract class PersistentSingelton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;


    public static T Instance { get => _instance; }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            InitializeOnAwake();
        }
    }

    protected abstract void InitializeOnAwake();
}
