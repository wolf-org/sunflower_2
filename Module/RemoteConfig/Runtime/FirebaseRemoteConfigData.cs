using System;

#if VIRTUESKY_FIREBASE_REMOTECONFIG
using Firebase.RemoteConfig;
#endif

using UnityEngine;
using VirtueSky.DataStorage;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.RemoteConfigs
{
    [Serializable]
    public class FirebaseRemoteConfigData
    {
        public string key;
        public TypeRemoteConfigData typeRemoteConfigData;

        [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.StringData)]
        public string defaultValueString;

        [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.StringData)] [SerializeField, ReadOnly]
        private string resultValueString;

        [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.BooleanData)]
        public bool defaultValueBool;

        [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.BooleanData)] [SerializeField, ReadOnly]
        private bool resultValueBool;

        [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.IntData)]
        public int defaultValueInt;

        [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.IntData)] [SerializeField, ReadOnly]
        private int resultValueInt;


        public void SetupDataDefault()
        {
            switch (typeRemoteConfigData)
            {
                case TypeRemoteConfigData.StringData:
                    GameData.Set(key, defaultValueString);
                    break;
                case TypeRemoteConfigData.BooleanData:
                    GameData.Set(key, defaultValueBool);
                    break;
                case TypeRemoteConfigData.IntData:
                    GameData.Set(key, defaultValueInt);
                    break;
            }
        }
#if VIRTUESKY_FIREBASE_REMOTECONFIG
        public void SetupData(ConfigValue result)
        {
            switch (typeRemoteConfigData)
            {
                case TypeRemoteConfigData.StringData:
                    if (result.Source == ValueSource.RemoteValue)
                    {
                        GameData.Set(key, result.StringValue);
                    }

                    resultValueString = GameData.Get<string>(key);
                    Debug.Log($"{key}: {resultValueString}".SetColor(Color.green));
                    break;
                case TypeRemoteConfigData.BooleanData:
                    if (result.Source == ValueSource.RemoteValue)
                    {
                        GameData.Set(key, result.BooleanValue);
                    }

                    resultValueBool = GameData.Get<bool>(key);
                    Debug.Log($"{key}: {resultValueBool}".SetColor(Color.green));
                    break;
                case TypeRemoteConfigData.IntData:
                    if (result.Source == ValueSource.RemoteValue)
                    {
                        GameData.Set(key, int.Parse(result.StringValue));
                    }

                    resultValueInt = GameData.Get<int>(key);
                    Debug.Log($"{key}: {resultValueInt}".SetColor(Color.green));
                    break;
            }
        }
#endif
    }

    public enum TypeRemoteConfigData
    {
        StringData,
        BooleanData,
        IntData
    }
}