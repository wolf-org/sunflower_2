using VirtueSky.Tracking;

namespace VirtueSky.Ads
{
    public class MaxAdClient : AdClient
    {
        public override void Initialize()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            MaxSdk.SetSdkKey(adSettings.SdkKey);
            MaxSdk.InitializeSdk();
            MaxSdk.SetIsAgeRestrictedUser(adSettings.ApplovinEnableAgeRestrictedUser);
            adSettings.MaxBannerAdUnit.Init();
            adSettings.MaxInterstitialAdUnit.Init();
            adSettings.MaxRewardAdUnit.Init();
            adSettings.MaxAppOpenAdUnit.Init();
            adSettings.MaxRewardedInterstitialAdUnit.Init();

            adSettings.MaxBannerAdUnit.paidedCallback = AppTracking.TrackRevenue;
            adSettings.MaxInterstitialAdUnit.paidedCallback = AppTracking.TrackRevenue;
            adSettings.MaxRewardAdUnit.paidedCallback = AppTracking.TrackRevenue;
            adSettings.MaxRewardedInterstitialAdUnit.paidedCallback = AppTracking.TrackRevenue;
            adSettings.MaxAppOpenAdUnit.paidedCallback = AppTracking.TrackRevenue;

            LoadInterstitial();
            LoadRewarded();
            LoadRewardedInterstitial();
            LoadAppOpen();
#endif
        }
    }
}