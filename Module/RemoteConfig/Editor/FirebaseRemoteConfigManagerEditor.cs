using System.IO;
using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.RemoteConfigs.Editor
{
    // [CustomEditor(typeof(FirebaseRemoteConfigManager), false)]
    public class FirebaseRemoteConfigManagerEditor : UnityEditor.Editor
    {
        private FirebaseRemoteConfigManager _firebaseRemoteConfigManager;
        private SerializedProperty _dependencyStatus;
        private SerializedProperty _isSetupDefaultData;
        private SerializedProperty _listRemoteConfigData;

        private void OnEnable()
        {
            _firebaseRemoteConfigManager = target as FirebaseRemoteConfigManager;
            _dependencyStatus = serializedObject.FindProperty("dependencyStatus");
            _isSetupDefaultData = serializedObject.FindProperty("isSetupDefaultData");
            _listRemoteConfigData = serializedObject.FindProperty("listRemoteConfigData");
        }

        // public override void OnInspectorGUI()
        // {
        //     base.OnInspectorGUI();
        //     serializedObject.Update();
        //     GUILayout.Space(10);
        //     if (GUILayout.Button("Generate Remote Data"))
        //     {
        //         GenerateRemoteData();
        //     }
        //
        //     serializedObject.ApplyModifiedProperties();
        // }

        private const string pathDefaultScript = "Assets/_Root/Scripts";

        void GenerateRemoteData()
        {
            FileExtension.ValidatePath(pathDefaultScript);
            var productImplPath = $"{pathDefaultScript}/RemoteData.cs";
            var str = "namespace VirtueSky.RemoteConfigs\n{";
            str += "\n\tpublic struct RemoteData\n\t{";

            var listRmcData = _firebaseRemoteConfigManager.ListRemoteConfigData;
            for (int i = 0; i < listRmcData.Count; i++)
            {
                var rmcKey = listRmcData[i].key;

                str += $"\n\t\tpublic const string KEY_{rmcKey.ToUpper()} = \"{rmcKey}\";";

                switch (listRmcData[i].typeRemoteConfigData)
                {
                    case TypeRemoteConfigData.StringData:
                        GenMethod(ref str, "string", rmcKey);
                        break;
                    case TypeRemoteConfigData.BooleanData:
                        GenMethod(ref str, "bool", rmcKey);
                        break;
                    case TypeRemoteConfigData.IntData:
                        GenMethod(ref str, "int", rmcKey);
                        break;
                }
            }

            str += "\n\t}";
            str += "\n}";

            var writer = new StreamWriter(productImplPath, false);
            writer.Write(str);
            writer.Close();
            AssetDatabase.ImportAsset(productImplPath);
        }

        void GenMethod(ref string str, string typeMethod, string key)
        {
            str +=
                $"\n\t\tpublic static {typeMethod} {key.ToUpper()} => VirtueSky.DataStorage.GameData.Get<{typeMethod}>(KEY_{key.ToUpper()});";
        }
    }
}