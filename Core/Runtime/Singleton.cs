using UnityEngine;

namespace VirtueSky.Core
{
    public abstract class Singleton<T> : BaseMono where T : MonoBehaviour
    {
        [SerializeField] private bool isDontDestroyOnLoad;
        static T _instance;

        public static T Instance => _instance ??= FindObjectOfType<T>();

        protected virtual void Awake()
        {
            if (isDontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }

            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }
    }
}