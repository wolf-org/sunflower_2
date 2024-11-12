using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Utils;

#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif

#if VIRTUESKY_FIREBASE
using Firebase;
using Firebase.Extensions;
#endif

#if VIRTUESKY_FIREBASE_REMOTECONFIG
using Firebase.RemoteConfig;
#endif

namespace VirtueSky.RemoteConfigs
{
    [EditorIcon("icon_controller"), HideMonoScript]
    public class FirebaseRemoteConfigManager : MonoBehaviour
    {
        [SerializeField] private bool isDontDestroyOnLoad;
        [Space, SerializeField] private TypeInitRemoteConfig typeInitRemoteConfig;
#if VIRTUESKY_FIREBASE
        [Space, ReadOnly, SerializeField] private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
#endif
        [SerializeField] private bool isSetupDefaultData = true;
        [Space(10), SerializeField] private FirebaseRemoteConfigData[] listRemoteConfigData;
        private bool isFetchRemoteConfigCompleted = false;


        private static event Func<bool> OnIsFetchRemoteConfigCompletedEvent;
        private static event Func<bool> OnIsFirebaseDependencyAvailableEvent;
        private static event Func<FirebaseRemoteConfigData[]> OnGetListRemoteConfigEvent;
#if VIRTUESKY_FIREBASE
        private bool InternalIsFirebaseDependencyAvailable() => dependencyStatus == DependencyStatus.Available;
#endif
        private bool InternalIsFetchRemoteConfigCompleted() => isFetchRemoteConfigCompleted;
        private FirebaseRemoteConfigData[] InternalGetListRemoteConfig() => listRemoteConfigData;

        #region public api

        public static FirebaseRemoteConfigData[] ListRemoteConfigData => OnGetListRemoteConfigEvent?.Invoke();
        public static bool IsFetchRemoteConfigCompleted => (bool)OnIsFetchRemoteConfigCompletedEvent?.Invoke();

        public static bool FirebaseDependencyAvailable
        {
            get
            {
#if VIRTUESKY_FIREBASE
                return (bool)OnIsFirebaseDependencyAvailableEvent?.Invoke();
#else
                return false;
#endif
            }
        }

        #endregion

        private void Awake()
        {
            if (isDontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }

            OnIsFetchRemoteConfigCompletedEvent += InternalIsFetchRemoteConfigCompleted;
#if VIRTUESKY_FIREBASE
            OnIsFirebaseDependencyAvailableEvent += InternalIsFirebaseDependencyAvailable;
#endif
            OnGetListRemoteConfigEvent += InternalGetListRemoteConfig;

            if (typeInitRemoteConfig == TypeInitRemoteConfig.InitOnAwake)
            {
                Init();
            }
        }

        private void Start()
        {
            if (typeInitRemoteConfig == TypeInitRemoteConfig.InitOnStart)
            {
                Init();
            }
        }

        private void OnDestroy()
        {
            OnIsFetchRemoteConfigCompletedEvent -= InternalIsFetchRemoteConfigCompleted;
#if VIRTUESKY_FIREBASE
            OnIsFirebaseDependencyAvailableEvent -= InternalIsFirebaseDependencyAvailable;
#endif
            OnGetListRemoteConfigEvent -= InternalGetListRemoteConfig;
        }

        private void Init()
        {
#if VIRTUESKY_FIREBASE
            isFetchRemoteConfigCompleted = false;
            if (isSetupDefaultData)
            {
                foreach (var remoteConfigData in listRemoteConfigData)
                {
                    remoteConfigData.SetupDataDefault();
                }
            }

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    var app = FirebaseApp.DefaultInstance;
#if VIRTUESKY_FIREBASE_REMOTECONFIG
                    FetchDataAsync();
#endif
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}".SetColor(Color.red));
                }
            });
#endif
        }

#if VIRTUESKY_FIREBASE_REMOTECONFIG && VIRTUESKY_FIREBASE
        public Task FetchDataAsync()
        {
            Debug.Log("Fetching data...".SetColor(CustomColor.Cyan));
            Task fetchTask = FirebaseRemoteConfig.DefaultInstance
                .FetchAsync(TimeSpan.Zero);

            return fetchTask.ContinueWithOnMainThread(tast =>
            {
                var info = FirebaseRemoteConfig.DefaultInstance.Info;
                if (info.LastFetchStatus == LastFetchStatus.Success)
                {
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(
                        task =>
                        {
                            Debug.Log(String.Format(
                                "Remote data loaded and ready (last fetch time {0}).".SetColor(CustomColor.Cyan),
                                info.FetchTime));
                            foreach (var remoteConfigData in listRemoteConfigData)
                            {
                                if (string.IsNullOrEmpty(remoteConfigData.key)) continue;
                                remoteConfigData.SetupData(FirebaseRemoteConfig.DefaultInstance
                                    .GetValue(remoteConfigData.key.ToString()));
                            }

                            isFetchRemoteConfigCompleted = true;
                        });

                    Debug.Log("Firebase Remote Config Fetching completed!".SetColor(Color.green));
                }
                else
                {
                    Debug.Log("Fetching data did not completed!".SetColor(Color.red));
                }
            });
        }
#endif

#if UNITY_EDITOR
        private const string pathDefaultScript = "Assets/_Root/Scripts";
        [Button]
        private void GenerateRemoteData()
        {
            FileExtension.ValidatePath(pathDefaultScript);
            var productImplPath = $"{pathDefaultScript}/RemoteData.cs";
            var str = "namespace VirtueSky.RemoteConfigs\n{";
            str += "\n\tpublic struct RemoteData\n\t{";
            str += "\n";
            var listRmcData = listRemoteConfigData;
            foreach (var rmc in listRmcData)
            {
                var rmcKey = rmc.key;
                str += $"// {rmcKey.ToUpper()}";
                str += $"\n\t\tpublic const string KEY_{rmcKey.ToUpper()} = \"{rmcKey}\";";

                switch (rmc.typeRemoteConfigData)
                {
                    case TypeRemoteConfigData.StringData:
                        str +=
                            $"\n\t\tpublic const string DEFAULT_{rmcKey.ToUpper()} = \"{rmc.defaultValueString}\";";
                        str +=
                            $"\n\t\tpublic static string {rmcKey.ToUpper()} => VirtueSky.DataStorage.GameData.Get(KEY_{rmcKey.ToUpper()}, DEFAULT_{rmcKey.ToUpper()});";
                        break;
                    case TypeRemoteConfigData.BooleanData:
                        str +=
                            $"\n\t\tpublic const bool DEFAULT_{rmcKey.ToUpper()} = {GetBool(rmc.defaultValueBool)};";
                        str +=
                            $"\n\t\tpublic static bool {rmcKey.ToUpper()} => VirtueSky.DataStorage.GameData.Get(KEY_{rmcKey.ToUpper()}, DEFAULT_{rmcKey.ToUpper()});";
                        break;
                    case TypeRemoteConfigData.IntData:
                        str +=
                            $"\n\t\tpublic const int DEFAULT_{rmcKey.ToUpper()} = {rmc.defaultValueInt};";
                        str +=
                            $"\n\t\tpublic static int {rmcKey.ToUpper()} => VirtueSky.DataStorage.GameData.Get(KEY_{rmcKey.ToUpper()}, DEFAULT_{rmcKey.ToUpper()});";
                        break;
                }

                str += "\n";
            }

            str += "\n\t}";
            str += "\n}";

            var writer = new StreamWriter(productImplPath, false);
            writer.Write(str);
            writer.Close();
            AssetDatabase.ImportAsset(productImplPath);

            string GetBool(bool condition)
            {
                return condition ? "true" : "false";
            }
        }
#endif
    }

    enum TypeInitRemoteConfig
    {
        InitOnAwake,
        InitOnStart
    }
}