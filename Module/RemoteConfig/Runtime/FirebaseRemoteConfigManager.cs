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
    public class FirebaseRemoteConfigManager : Singleton<FirebaseRemoteConfigManager>
    {
#if VIRTUESKY_FIREBASE
        [ReadOnly, SerializeField] private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
#endif
        [SerializeField] private bool isSetupDefaultData = true;
        [Space(10), SerializeField] private List<FirebaseRemoteConfigData> listRemoteConfigData;

        public List<FirebaseRemoteConfigData> ListRemoteConfigData => listRemoteConfigData;


        private bool isFetchRemoteConfigCompleted = false;
        public bool IsFetchRemoteConfigCompleted => isFetchRemoteConfigCompleted;

#if VIRTUESKY_FIREBASE
        private void Start()
        {
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
        }
#endif

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
                                if (!string.IsNullOrEmpty(remoteConfigData.key.ToString()))
                                {
                                    remoteConfigData.SetupData(FirebaseRemoteConfig.DefaultInstance
                                        .GetValue(remoteConfigData.key.ToString()));
                                }
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

            var listRmcData = ListRemoteConfigData;
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
}