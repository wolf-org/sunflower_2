using System.Collections.Generic;
using VirtueSky.Misc;

namespace VirtueSky.Tracking
{
    public class AppsFlyerTracking
    {
        public static void TrackEvent(string eventName)
        {
#if VIRTUESKY_APPSFLYER
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, null);
#endif
        }

        public static void TrackEvent(string eventName, string parameterName, string parameterValue)
        {
#if VIRTUESKY_APPSFLYER
            Dictionary<string, string> eventValues = new Dictionary<string, string>();
            eventValues.Add(parameterName, parameterValue);
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, eventValues);
#endif
        }

        public static void TrackEvent(string eventName, Dictionary<string, string> eventValues)
        {
#if VIRTUESKY_APPSFLYER
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, eventValues);
#endif
        }

        public void TrackEvent(string eventName, List<string> paramNames, List<string> paramValues)
        {
#if VIRTUESKY_APPSFLYER
            IDictionary<string, string> eventValues = paramNames.MakeDictionary(paramValues);
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, (Dictionary<string, string>)eventValues);
#endif
        }
    }
}