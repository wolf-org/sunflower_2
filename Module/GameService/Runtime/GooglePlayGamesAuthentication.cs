#if UNITY_ANDROID && VIRTUESKY_GPGS
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using System;
using System.Threading.Tasks;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.GameService
{
    [EditorIcon("icon_authentication")]
    public class GooglePlayGamesAuthentication : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad;
        public static event Action<string> ServerCodeEvent;
        public static event Action<string> NameEvent;
        public static event Action<StatusLogin> StatusLoginEvent;
        private static GooglePlayGamesAuthentication ins;

        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            if (ins == null)
            {
                ins = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
#if UNITY_ANDROID && VIRTUESKY_GPGS
            PlayGamesPlatform.Activate();
#endif
        }

        private async void InternalGetNewServerCode()
        {
#if UNITY_ANDROID && VIRTUESKY_GPGS
            if (!PlayGamesPlatform.Instance.IsAuthenticated()) return;
            (string serverCode, StatusLogin statusLogin) = await Excute();
            ServerCodeEvent?.Invoke(serverCode);
            StatusLoginEvent?.Invoke(statusLogin);

            Task<(string, StatusLogin)> Excute()
            {
                var taskSource = new TaskCompletionSource<(string, StatusLogin)>();
                PlayGamesPlatform.Instance.RequestServerSideAccess(true,
                    code => taskSource.SetResult((code, StatusLogin.Successful)));
                return taskSource.Task;
            }
#endif
        }


        private void InternalLogin()
        {
#if UNITY_ANDROID && VIRTUESKY_GPGS
            PlayGamesPlatform.Instance.Authenticate((success) =>
            {
                if (success == SignInStatus.Success)
                {
                    Debug.Log("Login with Google Play games successful.");
                    PlayGamesPlatform.Instance.RequestServerSideAccess(true,
                        code =>
                        {
                            Debug.Log("Authorization code: " + code);
                            ServerCodeEvent?.Invoke(code);
                            NameEvent?.Invoke(PlayGamesPlatform.Instance.GetUserDisplayName());
                            StatusLoginEvent?.Invoke(StatusLogin.Successful);
                        });
                }
                else
                {
                    PlayGamesPlatform.Instance.ManuallyAuthenticate(success =>
                    {
                        if (success == SignInStatus.Success)
                        {
                            PlayGamesPlatform.Instance.RequestServerSideAccess(true,
                                code =>
                                {
                                    Debug.Log("Authorization code: " + code);
                                    ServerCodeEvent?.Invoke(code);
                                    NameEvent?.Invoke(PlayGamesPlatform.Instance.GetUserDisplayName());
                                    StatusLoginEvent?.Invoke(StatusLogin.Successful);
                                });
                        }
                        else
                        {
                            Debug.Log("Login Failed");
                            ServerCodeEvent?.Invoke("");
                            StatusLoginEvent?.Invoke(StatusLogin.Failed);
                        }
                    });
                }
            });
#endif
        }

        #region Api

        public static void Login() => ins.InternalLogin();

        public static void GetNewServerCode() => ins.InternalGetNewServerCode();

        public static bool IsSignIn()
        {
#if UNITY_ANDROID && VIRTUESKY_GPGS
            return PlayGamesPlatform.Instance.IsAuthenticated();

#else
            return false;
#endif
        }

        #endregion
    }
}