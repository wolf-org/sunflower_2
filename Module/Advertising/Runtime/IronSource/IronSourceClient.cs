using VirtueSky.Core;
using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    public class IronSourceClient : AdClient
    {
        public bool SdkInitializationCompleted { get; private set; }

        public override void Initialize()
        {
            SdkInitializationCompleted = false;
            if (AdSettings.UseTestAppKey)
            {
                AdSettings.AndroidAppKey = "85460dcd";
                AdSettings.IosAppKey = "8545d445";
            }
#if VIRTUESKY_ADS && VIRTUESKY_IRONSOURCE
            App.AddPauseCallback(OnAppStateChange);
            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
            IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;
            AdSettings.IronSourceBannerAdUnit.Init();
            AdSettings.IronSourceInterstitialAdUnit.Init();
            AdSettings.IronSourceRewardAdUnit.Init();
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init(AdSettings.AppKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL,
                IronSourceAdUnits.BANNER);
#endif
            LoadInterstitial();
            LoadRewarded();
        }
#if VIRTUESKY_ADS && VIRTUESKY_IRONSOURCE
        private void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
        {
            if (impressionData.revenue != null)
            {
                AppTracking.TrackRevenue((double)impressionData.revenue, impressionData.adNetwork,
                    impressionData.adUnit,
                    impressionData.placement, AdNetwork.IronSource.ToString());
            }
        }

        private void OnAppStateChange(bool pauseStatus)
        {
            IronSource.Agent.onApplicationPause(pauseStatus);
        }
#endif
        void SdkInitializationCompletedEvent()
        {
            SdkInitializationCompleted = true;
        }
    }
}