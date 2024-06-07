using System;
using System.IO;
using UnityEngine;
using VirtueSky.DataStorage;
using VirtueSky.Inspector;
using VirtueSky.Misc;

namespace VirtueSky.Notifications
{
    [Serializable]
    public class NotificationChannel
    {
        [SerializeField] private string identifier;

        [SerializeField] private bool isRemoteConfigTimeSchedule;

        [ShowIf(nameof(isRemoteConfigTimeSchedule))] [SerializeField]
        private string keyRemoteConfig;

        [SerializeField] private int minute = 720;

        [SerializeField] private bool repeat;
        [SerializeField] internal bool bigPicture;

        [ShowIf(nameof(bigPicture))]
#if UNITY_EDITOR
        [HelpBox(
            "File big picture must be place in folder StreamingAsset, Name Picture must contains file extension ex .jpg")]
#endif

        [SerializeField]
        internal string namePicture;

        [SerializeField] internal bool overrideIcon;

        [SerializeField, ShowIf(nameof(overrideIcon))]
        internal string smallIcon = "icon_0";

        [SerializeField, ShowIf(nameof(overrideIcon))]
        internal string largeIcon = "icon_1";


        [SerializeField] private NotificationData[] datas;

        private int GetTimeSchedule()
        {
            return isRemoteConfigTimeSchedule ? GameData.Get(keyRemoteConfig, minute) : minute;
        }

        public void Send()
        {
            if (!Application.isMobilePlatform) return;
            var data = datas.PickRandom();
            string pathPicture = Path.Combine(Application.persistentDataPath, namePicture);
            NotificationConsole.Send(identifier,
                data.title,
                data.message,
                smallIcon: smallIcon,
                largeIcon: largeIcon,
                bigPicture: bigPicture,
                namePicture: pathPicture);
        }

        public void Schedule()
        {
            if (!Application.isMobilePlatform) return;
            var data = datas.PickRandom();

            string pathPicture = Path.Combine(Application.persistentDataPath, namePicture);

            NotificationConsole.Schedule(identifier,
                data.title,
                data.message,
                TimeSpan.FromMinutes(GetTimeSchedule()),
                smallIcon: smallIcon,
                largeIcon: largeIcon,
                bigPicture: bigPicture,
                namePicture: pathPicture,
                repeat: repeat);
        }

        public void CancelAllScheduled()
        {
            if (!Application.isMobilePlatform) return;
            NotificationConsole.CancelAllScheduled();
        }

        public void ClearBadgeCounterIOS()
        {
            if (!Application.isMobilePlatform) return;
            NotificationConsole.ClearBadgeCounterIOS();
        }
    }

    [Serializable]
    public class NotificationData
    {
        public string title;
        public string message;

        public NotificationData(string title, string message)
        {
            this.title = title;
            this.message = message;
        }
    }
}