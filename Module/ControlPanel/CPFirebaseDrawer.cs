using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPFirebaseDrawer
    {
        private static bool isShowInstallRemoteConfig = true;
        private static bool isShowInstallAnalytic = true;
        private static Vector2 scroll = Vector2.zero;
        private static bool isCustomPackageName;
        private static string packageName;

        public static void OnDrawFirebase(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.Firebase, "Firebase");
            scroll = EditorGUILayout.BeginScrollView(scroll);
            DrawInstall(position);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Define Symbols");
            GUILayout.Space(10);
#if !VIRTUESKY_FIREBASE || !VIRTUESKY_FIREBASE_REMOTECONFIG
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols: \n {ConstantDefineSymbols.VIRTUESKY_FIREBASE} for Firebase App,\n {ConstantDefineSymbols.VIRTUESKY_FIREBASE_REMOTECONFIG} for Firebase Remote Config to use",
                MessageType.Info);
#endif

            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE_REMOTECONFIG);
#if !VIRTUESKY_FIREBASE_ANALYTIC
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols: {ConstantDefineSymbols.VIRTUESKY_FIREBASE_ANALYTIC} for Firebase Analytic to use",
                MessageType.Info);
#endif
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE_ANALYTIC);
            GUILayout.Space(10);
#if UNITY_ANDROID
            CPUtility.GuiLine(2);
            DrawDebugView();
            GUILayout.Space(10);
#endif

            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        static void DrawInstall(Rect position)
        {
            GUILayout.Space(10);
            isShowInstallRemoteConfig =
                GUILayout.Toggle(isShowInstallRemoteConfig, "Install Firebase Remote Config And Dependency");
            GUILayout.Space(10);
            if (isShowInstallRemoteConfig)
            {
                CPUtility.DrawButtonInstallPackage("Install Firebase Remote Config", "Remove Firebase Remote Config",
                    ConstantPackage.PackageNameFirebaseRemoteConfig, ConstantPackage.MaxVersionFirebaseRemoteConfig);
                CPUtility.DrawButtonInstallPackage("Install Firebase App", "Remove Firebase App",
                    ConstantPackage.PackageNameFirebaseApp, ConstantPackage.MaxVersionFirebaseApp);
                CPUtility.DrawButtonInstallPackage("Install Google External Dependency Manager",
                    "Remove Google External Dependency Manager",
                    ConstantPackage.PackageNameGGExternalDependencyManager,
                    ConstantPackage.MaxVersionGGExternalDependencyManager);
            }

            GUILayout.Space(10);

            isShowInstallAnalytic = GUILayout.Toggle(isShowInstallAnalytic, "Install Firebase Analytic And Dependency");
            GUILayout.Space(10);
            if (isShowInstallAnalytic)
            {
                CPUtility.DrawButtonInstallPackage("Install Firebase Analytics", "Remove Firebase Analytics",
                    ConstantPackage.PackageNameFirebaseAnalytics, ConstantPackage.MaxVersionFirebaseAnalytics);
                CPUtility.DrawButtonInstallPackage("Install Firebase App", "Remove Firebase App",
                    ConstantPackage.PackageNameFirebaseApp, ConstantPackage.MaxVersionFirebaseApp);
                CPUtility.DrawButtonInstallPackage("Install Google External Dependency Manager",
                    "Remove Google External Dependency Manager",
                    ConstantPackage.PackageNameGGExternalDependencyManager,
                    ConstantPackage.MaxVersionGGExternalDependencyManager);
                GUILayout.Space(10);
            }
        }

        static void DrawDebugView()
        {
            GUILayout.Space(10);
            CPUtility.DrawHeader("Debug View");
            isCustomPackageName = EditorGUILayout.Toggle("Custom Package Name: ", isCustomPackageName);
            if (isCustomPackageName)
            {
                GUI.enabled = true;
            }
            else
            {
                packageName = Application.identifier;
                GUI.enabled = false;
            }

            packageName = EditorGUILayout.TextField("Package Name: ", packageName);
            GUI.enabled = true;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Run Debug View", GUILayout.Width(400)))
            {
                SetDebugView(packageName);
            }

            if (GUILayout.Button("Set None Debug View"))
            {
                SetDebugView(".none.");
            }

            GUILayout.EndHorizontal();
        }

        static void SetDebugView(string package)
        {
            var fileName = $"{AndroidExternalToolsSettings.sdkRootPath}/platform-tools/adb";
            var arguments = $"shell setprop debug.firebase.analytics.app {package}";
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                Arguments = arguments,
            };

            var process = Process.Start(startInfo);
            process!.WaitForExit();
            UnityEngine.Debug.Log($"{fileName} {arguments}");
        }
    }
}