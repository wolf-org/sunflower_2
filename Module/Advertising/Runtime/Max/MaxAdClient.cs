using VirtueSky.Core;

namespace VirtueSky.Ads
{
    public class MaxAdClient : AdClient
    {
        public override void Initialize()
        {
#if VIRTUESKY_ADS && VIRTUESKY_APPLOVIN
            MaxSdk.SetSdkKey(adSettings.SdkKey);
            MaxSdk.InitializeSdk();
            adSettings.MaxBannerAdUnit.Init();
            adSettings.MaxInterstitialAdUnit.Init();
            adSettings.MaxRewardAdUnit.Init();
            adSettings.MaxAppOpenAdUnit.Init();
            adSettings.MaxRewardedInterstitialAdUnit.Init();
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
            if (!pauseStatus && adSettings.MaxAppOpenAdUnit.autoShow && !AdStatic.isShowingAd) ShowAppOpen();
        }
#endif
    }
}