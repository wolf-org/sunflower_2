using System;
using UnityEngine;

namespace VirtueSky.GameService
{
    public abstract class ServiceAuthentication : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad;

        private static event Action OnLoginEvent;
        private static event Func<string> OnGetServerCodeEvent;
        private static event Func<string> OnGetNameEvent;
        private static event Func<StatusLogin> OnGetStatusLoginEvent;
        private static event Action<StatusLogin> OnSetStatusLoginEvent;


        protected string serverCode;
        protected string nameAuth;
        protected StatusLogin statusLogin;

        private void Awake()
        {
            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnEnable()
        {
            OnLoginEvent += InternalLogin;
            OnGetServerCodeEvent += InternalGetServerCode;
            OnGetNameEvent += InternalGetNameAuth;
            OnGetStatusLoginEvent += InternalGetStatusLogin;
            OnSetStatusLoginEvent += InternalSetStatusLogin;
        }

        protected virtual void OnDisable()
        {
            OnLoginEvent -= InternalLogin;
            OnGetServerCodeEvent -= InternalGetServerCode;
            OnGetNameEvent -= InternalGetNameAuth;
            OnGetStatusLoginEvent -= InternalGetStatusLogin;
            OnSetStatusLoginEvent -= InternalSetStatusLogin;
        }

        private void Start()
        {
            InternalInit();
        }

        private string InternalGetNameAuth() => nameAuth;
        private string InternalGetServerCode() => serverCode;
        private StatusLogin InternalGetStatusLogin() => statusLogin;
        private void InternalSetStatusLogin(StatusLogin status) => statusLogin = status;
        protected abstract void InternalInit();
        protected abstract void InternalLogin();

        #region Api

        public static void Login() => OnLoginEvent?.Invoke();
        public static string GetServerCode() => OnGetServerCodeEvent?.Invoke();
        public static string GetNameAuthentication() => OnGetNameEvent?.Invoke();
        public static StatusLogin GetStatusLogin() => (StatusLogin)OnGetStatusLoginEvent?.Invoke();
        public static void SetStatusLogin(StatusLogin statusLogin) => OnSetStatusLoginEvent?.Invoke(statusLogin);

        #endregion
    }
}