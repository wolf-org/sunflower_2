using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    [CreateAssetMenu(menuName = "Sunflower2/Tracking Event/AppsFlyer/Tracking No Param",
        fileName = "tracking_appsflyer_no_param")]
    [EditorIcon("scriptable_af")]
    public class ScriptableTrackingAppsFlyerNoParam : ScriptableObject
    {
        [Space, HeaderLine("Event Name"), SerializeField]
        private string eventName;

        public void TrackEvent()
        {
#if VIRTUESKY_APPSFLYER
            AppsFlyerSDK.AppsFlyer.sendEvent(eventName, null);
#endif
        }
    }
}