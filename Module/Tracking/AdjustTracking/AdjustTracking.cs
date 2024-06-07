namespace VirtueSky.Tracking
{
    public struct AdjustTracking
    {
        public static void TrackEvent(string eventToken)
        {
#if VIRTUESKY_ADJUST
            com.adjust.sdk.Adjust.trackEvent(new com.adjust.sdk.AdjustEvent(eventToken));
#endif
        }
    }
}