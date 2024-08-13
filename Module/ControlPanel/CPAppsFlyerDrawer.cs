using UnityEditor;
using UnityEngine;
using VirtueSky.Tracking;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPAppsFlyerDrawer
    {
        private static AppsFlyerSetting _setting;
        private static UnityEditor.Editor _editor;

        public static void OnEnable()
        {
            Init();
        }

        private static void Init()
        {
            if (_editor != null) _editor = null;
            _setting = CreateAsset.GetScriptableAsset<AppsFlyerSetting>();
            _editor = UnityEditor.Editor.CreateEditor(_setting);
        }

        public static void OnDrawAppsFlyer()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("APPSFLYER", EditorStyles.boldLabel);
            GUILayout.Space(10);
            CPUtility.DrawButtonInstallPackage("Install AppsFlyer", "Remove AppsFlyer",
                ConstantPackage.PackageNameAppFlyer, ConstantPackage.MaxVersionAppFlyer);
            CPUtility.DrawButtonInstallPackage("Install AppsFlyer Revenue Generic", "Remove AppsFlyer Revenue Generic",
                ConstantPackage.PackageNameAppFlyerRevenueGeneric, ConstantPackage.MaxVersionAppFlyerRevenueGeneric);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
#if !VIRTUESKY_APPSFLYER
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols: {ConstantDefineSymbols.VIRTUESKY_APPSFLYER} for AppsFlyer to use",
                MessageType.Info);
#endif
            GUILayout.Space(10);
            GUILayout.Label("ADD DEFINE SYMBOLS", EditorStyles.boldLabel);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_APPSFLYER);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Label("APPSFLYER SETTINGS", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (_setting == null)
            {
                if (GUILayout.Button("Create AppsFlyerSettings"))
                {
                    _setting =
                        CreateAsset.CreateAndGetScriptableAsset<AppsFlyerSetting>(isPingAsset: false);
                    Init();
                }
            }
            else
            {
                if (_editor == null)
                {
                    EditorGUILayout.HelpBox("Couldn't create the settings editor.",
                        MessageType.Error);
                    return;
                }
                else
                {
                    EditorGUILayout.HelpBox(
                        "Set your devKey and appID to init the AppsFlyer SDK and start tracking. You must modify these fields and provide:\ndevKey - Your application devKey provided by AppsFlyer.\nappId - For iOS only. Your iTunes Application ID.\nUWP app id - For UWP only. Your application app id \nMac OS app id - For MacOS app only.",
                        MessageType.Info);
                    _editor.OnInspectorGUI();
                }
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();
        }
    }
}