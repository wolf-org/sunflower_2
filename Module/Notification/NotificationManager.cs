using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using VirtueSky.Core;
using VirtueSky.Inspector;

namespace VirtueSky.Notifications
{
    [EditorIcon("script_noti"), HideMonoScript]
    public class NotificationManager : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad;
        [Space, SerializeField] private bool autoSchedule = true;
        [SerializeField] private List<NotificationChannel> channels = new List<NotificationChannel>();
        private static NotificationManager ins;
        public static List<NotificationChannel> Channels => ins.channels;

        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            if (ins == null)
            {
                ins = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
#if UNITY_ANDROID
            PermissionPostNotification();
            Prepare();
#endif
            if (autoSchedule)
            {
                AutoSchedule();
            }
        }

        void AutoSchedule()
        {
            foreach (var channel in channels)
            {
                channel.Schedule();
            }
        }

#if UNITY_ANDROID

        private void Prepare()
        {
            if (Application.isMobilePlatform)
            {
                var strs = new List<string>();

                foreach (var channel in channels)
                {
                    if (!channel.bigPicture) continue;
                    if (!strs.Contains(channel.namePicture)) strs.Add(channel.namePicture);
                }

                foreach (string s in strs)
                {
                    App.StartCoroutine(PrepareImage(Application.persistentDataPath, s));
                }
            }
        }

        private IEnumerator PrepareImage(string destDir, string filename)
        {
            string path = Path.Combine(destDir, filename);
            if (File.Exists(path)) yield break;
            using var uwr =
                UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, filename));
            yield return uwr.SendWebRequest();
            File.WriteAllBytes(path, uwr.downloadHandler.data);
        }

        void PermissionPostNotification()
        {
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
            {
                UnityEngine.Android.Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
            }
        }
#endif
    }
}