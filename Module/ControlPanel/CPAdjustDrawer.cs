using UnityEditor;
using UnityEngine;
using VirtueSky.Tracking;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPAdjustDrawer
    {
        private static AdjustConfig config;
        private static UnityEditor.Editor _editor;
        private static Vector2 scroll = Vector2.zero;

        public static void OnEnable()
        {
            Init();
        }

        private static void Init()
        {
            if (_editor != null) _editor = null;
            config = CreateAsset.GetScriptableAsset<AdjustConfig>();
            _editor = UnityEditor.Editor.CreateEditor(config);
        }

        public static void OnDrawAdjust()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.Adjust, "Adjust");
            GUILayout.Space(10);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            CPUtility.DrawButtonInstallPackage("Install Adjust", "Remove Adjust",
                ConstantPackage.PackageNameAdjust, ConstantPackage.MaxVersionAdjust);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
#if !VIRTUESKY_ADJUST
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols: {ConstantDefineSymbols.VIRTUESKY_ADJUST} for Adjust to use",
                MessageType.Info);
               GUILayout.Space(10);
#endif
            CPUtility.DrawHeader("Define symbols");
            GUILayout.Space(10);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADJUST);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Adjust Config");
            GUILayout.Space(10);
            if (config == null)
            {
                if (GUILayout.Button("Create AdjustConfig"))
                {
                    config =
                        CreateAsset.CreateAndGetScriptableAsset<AdjustConfig>(isPingAsset: false);
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
                    _editor.OnInspectorGUI();
                }
            }

            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Adjust Tracking");
            GUILayout.Space(10);

            if (GUILayout.Button("Create Scriptable Tracking Adjust"))
            {
                CreateAsset.CreateScriptableAssetsOnlyName<ScriptableTrackingAdjust>($"{FileExtension.DefaultRootPath}/Tracking_Adjust", "tracking_adjust");
            }

            GUILayout.Space(10);
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}