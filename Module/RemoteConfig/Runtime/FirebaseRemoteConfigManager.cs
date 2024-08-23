using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;

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
        [Space(10), SerializeField] private List<FirebaseRemoteConfigData> listRemoteConfigData;
        private bool isFetchRemoteConfigCompleted = false;
        private static FirebaseRemoteConfigManager ins;
        public static List<FirebaseRemoteConfigData> ListRemoteConfigData => ins.listRemoteConfigData;
        public static bool IsFetchRemoteConfigCompleted => ins.isFetchRemoteConfigCompleted;

        public static bool FirebaseDependencyAvailable
        {
            get
            {
#if VIRTUESKY_FIREBASE
                 return ins.dependencyStatus == DependencyStatus.Available;
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            if (isDontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }

            if (ins == null)
            {
                ins = this;
            }
            else
            {
                Destroy(gameObject);
            }

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
                    Debug.LogError("Could not resolve all Firebase dependencies: " +
                                   dependencyStatus);
                }
            });
#endif
        }

#if VIRTUESKY_FIREBASE_REMOTECONFIG && VIRTUESKY_FIREBASE
        public Task FetchDataAsync()
        {
            Debug.Log("Fetching data...");
            Task fetchTask = FirebaseRemoteConfig.DefaultInstance
                .FetchAsync(TimeSpan.Zero);
            if (fetchTask.IsCanceled)
            {
                Debug.Log("Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                Debug.Log("Fetch encountered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("Fetch completed successfully!");
            }

            return fetchTask.ContinueWithOnMainThread(tast =>
            {
                var info = FirebaseRemoteConfig.DefaultInstance.Info;
                if (info.LastFetchStatus == LastFetchStatus.Success)
                {
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(
                        task =>
                        {
                            Debug.Log(String.Format(
                                "Remote data loaded and ready (last fetch time {0}).",
                                info.FetchTime));
                            foreach (var remoteConfigData in listRemoteConfigData)
                            {
                                if (string.IsNullOrEmpty(remoteConfigData.key)) continue;
                                remoteConfigData.SetupData(FirebaseRemoteConfig.DefaultInstance
                                    .GetValue(remoteConfigData.key.ToString()));
                            }

                            isFetchRemoteConfigCompleted = true;
                        });

                    Debug.Log("<color=Green>Firebase Remote Config Fetching completed!</color>");
                }
                else
                {
                    Debug.Log("Fetching data did not completed!");
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

            var listRmcData = listRemoteConfigData;
            for (int i = 0; i < listRmcData.Count; i++)
            {
                var rmcKey = listRmcData[i].key;

                str += $"\n\t\tpublic const string KEY_{rmcKey.ToUpper()} = \"{rmcKey}\";";

                switch (listRmcData[i].typeRemoteConfigData)
                {
                    case TypeRemoteConfigData.StringData:
                        str +=
                            $"\n\t\tpublic const string DEFAULT_{rmcKey.ToUpper()} = \"{listRmcData[i].defaultValueString}\";";
                        str +=
                            $"\n\t\tpublic static string {rmcKey.ToUpper()} => VirtueSky.DataStorage.GameData.Get(KEY_{rmcKey.ToUpper()}, DEFAULT_{rmcKey.ToUpper()});";
                        break;
                    case TypeRemoteConfigData.BooleanData:
                        str +=
                            $"\n\t\tpublic const bool DEFAULT_{rmcKey.ToUpper()} = {GetBool(listRmcData[i].defaultValueBool)};";
                        str +=
                            $"\n\t\tpublic static bool {rmcKey.ToUpper()} => VirtueSky.DataStorage.GameData.Get(KEY_{rmcKey.ToUpper()}, DEFAULT_{rmcKey.ToUpper()});";
                        break;
                    case TypeRemoteConfigData.IntData:
                        str +=
                            $"\n\t\tpublic const int DEFAULT_{rmcKey.ToUpper()} = {listRmcData[i].defaultValueInt};";
                        str +=
                            $"\n\t\tpublic static int {rmcKey.ToUpper()} => VirtueSky.DataStorage.GameData.Get(KEY_{rmcKey.ToUpper()}, DEFAULT_{rmcKey.ToUpper()});";
                        break;
                }
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