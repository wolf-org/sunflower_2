using UnityEditor;
using UnityEngine;
using VirtueSky.Notifications;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPNotificationDrawer
    {
        public static void DrawNotification()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.Notification, "Notifications");
            GUILayout.Space(10);
            CPUtility.DrawButtonInstallPackage("Install Notifications", "Remove Notifications",
                ConstantPackage.PackageNameMobileNotification, ConstantPackage.MaxVersionMobileNotification);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
#if !VIRTUESKY_NOTIFICATION
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols: {ConstantDefineSymbols.VIRTUESKY_NOTIFICATION} for Notification to use",
                MessageType.Info);
#endif
            CPUtility.DrawHeader("Define symbols");
            GUILayout.Space(10);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_NOTIFICATION);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            if (GUILayout.Button("Create notification chanel"))
            {
                CreateAsset.CreateScriptableAssetsOnlyName<NotificationChannel>(FileExtension.DefaultRootPath + "/Notifications_Chanel", "notification_channel_data");
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();
        }
    }
}