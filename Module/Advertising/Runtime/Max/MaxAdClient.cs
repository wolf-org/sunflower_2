using VirtueSky.TrackingRevenue;

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

        public override void LoadInterstitial()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (!IsInterstitialReady()) adSettings.MaxInterstitialAdUnit.Load();
#endif
        }

        public override bool IsInterstitialReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return adSettings.MaxInterstitialAdUnit.IsReady();
#else
            return false;
#endif
        }

        public override AdUnit ShowInterstitial()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return adSettings.MaxInterstitialAdUnit.Show();
#endif
        }

        public override void LoadRewarded()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (!IsRewardedReady()) adSettings.MaxRewardAdUnit.Load();
#endif
        }

        public override bool IsRewardedReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return adSettings.MaxRewardAdUnit.IsReady();
#else
            return false;
#endif
        }

        public override AdUnit ShowReward()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return adSettings.MaxRewardAdUnit.Show();
#endif
        }

        public override void LoadRewardedInterstitial()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (!IsRewardedInterstitialReady()) adSettings.MaxRewardedInterstitialAdUnit.Load();
#endif
        }

        public override bool IsRewardedInterstitialReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return adSettings.MaxRewardedInterstitialAdUnit.IsReady();
#else
            return false;
#endif
        }

        public override AdUnit ShowRewardedInterstitial()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return adSettings.MaxRewardedInterstitialAdUnit.Show();
#endif
        }

        public override void LoadAppOpen()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (!IsAppOpenReady()) adSettings.MaxAppOpenAdUnit.Load();
#endif
        }

        public override bool IsAppOpenReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return adSettings.MaxAppOpenAdUnit.IsReady();
#else
            return false;
#endif
        }

        public override void ShowAppOpen()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (statusAppOpenFirstIgnore) adSettings.MaxAppOpenAdUnit.Show();
            statusAppOpenFirstIgnore = true;
#endif
        }

        public override void ShowBanner()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            adSettings.MaxBannerAdUnit.Show();
#endif
        }

        public override void HideBanner()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            adSettings.MaxBannerAdUnit.Hide();
#endif
        }

        public override void DestroyBanner()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            adSettings.MaxBannerAdUnit.Destroy();
#endif
        }
    }
}