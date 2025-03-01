using UnityEngine;

namespace Second.Scripts.Core
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                Debug.Log("Found instance of " + typeof(T).Name);
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.Log("Instance of " + typeof(T).Name + " already exists, destroying this instance");
                Destroy(gameObject);
            }
        }
    }
}