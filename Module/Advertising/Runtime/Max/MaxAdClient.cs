using VirtueSky.Core;

namespace VirtueSky.Ads
{
    public class MaxAdClient : AdClient
    {
        public override void Initialize()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            MaxSdk.SetSdkKey(AdSettings.SdkKey);
            MaxSdk.InitializeSdk();
            AdSettings.MaxBannerAdUnit.Init();
            AdSettings.MaxInterstitialAdUnit.Init();
            AdSettings.MaxRewardAdUnit.Init();
            AdSettings.MaxAppOpenAdUnit.Init();
            AdSettings.MaxRewardedInterstitialAdUnit.Init();
            App.AddPauseCallback(OnAppStateChange);
            LoadInterstitial();
            LoadRewarded();
            LoadRewardedInterstitial();
            LoadAppOpen();
#endif
        }
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
        private void OnAppStateChange(bool pauseStatus)
        {
            if (!pauseStatus && AdSettings.MaxAppOpenAdUnit.autoShow && !AdStatic.isShowingAd) ShowAppOpen();
        }
#endif
    }
}