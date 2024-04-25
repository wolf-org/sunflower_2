using VirtueSky.TrackingRevenue;

namespace VirtueSky.Ads
{
    public class MaxAdClient : AdClient
    {
        public override void Initialize()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            MaxSdk.SetSdkKey(AdSettings.SdkKey);
            MaxSdk.InitializeSdk();
            MaxSdk.SetIsAgeRestrictedUser(AdSettings.ApplovinEnableAgeRestrictedUser);
            AdSettings.MaxBannerAdUnit.Init();
            AdSettings.MaxInterstitialAdUnit.Init();
            AdSettings.MaxRewardAdUnit.Init();
            AdSettings.MaxAppOpenAdUnit.Init();
            AdSettings.MaxRewardedInterstitialAdUnit.Init();

            AdSettings.MaxBannerAdUnit.paidedCallback = AppTracking.TrackRevenue;
            AdSettings.MaxInterstitialAdUnit.paidedCallback = AppTracking.TrackRevenue;
            AdSettings.MaxRewardAdUnit.paidedCallback = AppTracking.TrackRevenue;
            AdSettings.MaxRewardedInterstitialAdUnit.paidedCallback = AppTracking.TrackRevenue;
            AdSettings.MaxAppOpenAdUnit.paidedCallback = AppTracking.TrackRevenue;

            LoadInterstitial();
            LoadRewarded();
            LoadRewardedInterstitial();
            LoadAppOpen();
#endif
        }

        public override void LoadInterstitial()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (!IsInterstitialReady()) AdSettings.MaxInterstitialAdUnit.Load();
#endif
        }

        public override bool IsInterstitialReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return AdSettings.MaxInterstitialAdUnit.IsReady();
#else
            return false;
#endif
        }

        public override void LoadRewarded()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (!IsRewardedReady()) AdSettings.MaxRewardAdUnit.Load();
#endif
        }

        public override bool IsRewardedReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return AdSettings.MaxRewardAdUnit.IsReady();
#else
            return false;
#endif
        }

        public override void LoadRewardedInterstitial()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (!IsRewardedInterstitialReady()) AdSettings.MaxRewardedInterstitialAdUnit.Load();
#endif
        }

        public override bool IsRewardedInterstitialReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return AdSettings.MaxRewardedInterstitialAdUnit.IsReady();
#else
            return false;
#endif
        }

        public override void LoadAppOpen()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (!IsAppOpenReady()) AdSettings.MaxAppOpenAdUnit.Load();
#endif
        }

        public override bool IsAppOpenReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return AdSettings.MaxAppOpenAdUnit.IsReady();
#else
            return false;
#endif
        }

        internal void ShowAppOpen()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (statusAppOpenFirstIgnore) AdSettings.MaxAppOpenAdUnit.Show();
            statusAppOpenFirstIgnore = true;
#endif
        }
    }
}