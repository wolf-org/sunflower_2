using System.Collections.Generic;
using UnityEngine;
#if VIRTUESKY_FIREBASE_ANALYTIC
using Firebase.Analytics;
#endif


namespace VirtueSky.Tracking
{
    public struct FirebaseTracking
    {
        public static void TrackEvent(string eventName)
        {
            if (!Application.isMobilePlatform) return;
#if VIRTUESKY_FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(eventName);
#endif
        }

        public static void TrackEvent(string eventName, string parameterName, string parameterValue)
        {
            if (!Application.isMobilePlatform) return;
#if VIRTUESKY_FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
#endif
        }

        public static void TrackEvent(string eventName, Dictionary<string, string> dictParameters)
        {
            if (!Application.isMobilePlatform) return;
#if VIRTUESKY_FIREBASE_ANALYTIC
            List<Parameter> list = new List<Parameter>();
            foreach (var param in dictParameters)
            {
                list.Add(new Parameter(param.Key, param.Value));
            }

            FirebaseAnalytics.LogEvent(eventName, list.ToArray());
#endif
        }
#if VIRTUESKY_FIREBASE_ANALYTIC
        public static void TrackEvent(string eventName, Parameter[] parameters)
        {
            if (!Application.isMobilePlatform) return;
            FirebaseAnalytics.LogEvent(eventName, parameters);
        }
#endif
    }
}