using UnityEngine;
#if VIRTUESKY_FIREBASE_ANALYTIC
using Firebase.Analytics;
#endif


namespace VirtueSky.Tracking
{
    public struct AppTracking
    {
        public static void TrackRevenue(double value, string network, string unitId, string format,
            string adNetwork)
        {
            AdjustTrackingRevenue.AdjustTrackRevenue(value, network, unitId, format, adNetwork);
            FirebaseAnalyticTrackingRevenue.FirebaseAnalyticTrackRevenue(value, network, unitId,
                format, adNetwork);
            AppsFlyerTrackingRevenue.AppsFlyerTrackRevenueAd(value, network, unitId, format, adNetwork);
        }

        public static void FirebaseAnalyticTrackATTResult(int status)
        {
#if VIRTUESKY_FIREBASE_ANALYTIC
            FirebaseAnalytics.LogEvent("app_tracking_transparency", "status", status);
#endif
        }

        #region Firebase Track (Log Event)

        public static void FirebaseAnalyticTrack(string eventName)
        {
            if (!Application.isMobilePlatform) return;
#if VIRTUESKY_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
#endif
        }

        public static void FirebaseAnalyticTrack(string eventName, string parameterName, string parameterValue)
        {
            if (!Application.isMobilePlatform) return;
#if VIRTUESKY_FIREBASE_ANALYTIC
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
#endif
        }
#if VIRTUESKY_FIREBASE_ANALYTIC
        public static void FirebaseAnalyticTrack(string eventName, Parameter[] parameters)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parameters);
        }
#endif

        #endregion
    }
}