using System;
using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPAboutDrawer
    {
        public static void OnDrawAbout(Rect position, Action drawSetting = null)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.About, "About");
            GUILayout.Space(10);
            GUILayout.TextArea("Name: Sunflower2", EditorStyles.boldLabel);
            GUILayout.TextArea(
                "Core singleton for building Unity games",
                EditorStyles.boldLabel);
            GUILayout.TextArea($"Version: {ConstantPackage.VersionUnityCommon}",
                EditorStyles.boldLabel);
            GUILayout.TextArea("Author: VirtueSky", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Open GitHub Repository"))
            {
                Application.OpenURL("https://github.com/wolf-org/sunflower_2");
            }

            if (GUILayout.Button("Document"))
            {
                Application.OpenURL("https://github.com/wolf-org/sunflower_2/wiki");
            }

            GUILayout.Space(10);

            GUILayout.EndVertical();
            GUILayout.Box(EditorResources.IconVirtueSky, GUIStyle.none,
                GUILayout.Width(180), GUILayout.Height(180));
            GUILayout.EndHorizontal();


            GUILayout.Space(10);
            CPUtility.DrawLineLastRectY(3, ConstantControlPanel.POSITION_X_START_CONTENT, position.width);
            GUILayout.EndVertical();
        }
    }
}