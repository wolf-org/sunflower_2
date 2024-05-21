#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
using GoogleMobileAds.Api;
#endif
using System;
using VirtueSky.Core;
using VirtueSky.TrackingRevenue;

namespace VirtueSky.Ads
{
    public class AdmobClient : AdClient
    {
        public override void Initialize()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            // On Android, Unity is paused when displaying interstitial or rewarded video.
            // This setting makes iOS behave consistently with Android.
            MobileAds.SetiOSAppPauseOnBackground(true);

            // When true all events raised by GoogleMobileAds will be raised
            // on the Unity main thread. The default value is false.
            // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
            MobileAds.RaiseAdEventsOnUnityMainThread = true;

            adSettings.AdmobBannerAdUnit.Init();
            adSettings.AdmobInterstitialAdUnit.Init();
            adSettings.AdmobRewardAdUnit.Init();
            adSettings.AdmobRewardedInterstitialAdUnit.Init();
            adSettings.AdmobAppOpenAdUnit.Init();

            MobileAds.Initialize(initStatus =>
            {
                App.RunOnMainThread(() =>
                {
                    if (!adSettings.AdmobEnableTestMode) return;
                    var configuration = new RequestConfiguration
                        { TestDeviceIds = adSettings.AdmobDevicesTest };
                    MobileAds.SetRequestConfiguration(configuration);
                });
            });
            adSettings.AdmobBannerAdUnit.paidedCallback = AppTracking.TrackRevenue;
            adSettings.AdmobInterstitialAdUnit.paidedCallback = AppTracking.TrackRevenue;
            adSettings.AdmobRewardAdUnit.paidedCallback = AppTracking.TrackRevenue;
            adSettings.AdmobRewardedInterstitialAdUnit.paidedCallback = AppTracking.TrackRevenue;
            adSettings.AdmobAppOpenAdUnit.paidedCallback = AppTracking.TrackRevenue;
            RegisterAppStateChange();
            LoadInterstitial();
            LoadRewarded();
            LoadRewardedInterstitial();
            LoadAppOpen();
#endif
        }

        public override void LoadInterstitial()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (!IsInterstitialReady()) adSettings.AdmobInterstitialAdUnit.Load();
#endif
        }

        public override bool IsInterstitialReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return adSettings.AdmobInterstitialAdUnit.IsReady();
#else
            return false;
#endif
        }

        public override AdUnit ShowInterstitial()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return adSettings.AdmobInterstitialAdUnit.Show();
#else
            return null;
#endif
        }

        public override void LoadRewarded()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (!IsRewardedReady()) adSettings.AdmobRewardAdUnit.Load();
#endif
        }

        public override bool IsRewardedReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return adSettings.AdmobRewardAdUnit.IsReady();
#else
            return false;
#endif
        }

        public override AdUnit ShowReward()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return adSettings.AdmobRewardAdUnit.Show();
#else
            return null;
#endif
        }

        public override void LoadRewardedInterstitial()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (!IsRewardedInterstitialReady()) adSettings.AdmobRewardedInterstitialAdUnit.Load();
#endif
        }

        public override bool IsRewardedInterstitialReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return adSettings.AdmobRewardedInterstitialAdUnit.IsReady();
#else
            return false;
#endif
        }

        public override AdUnit ShowRewardedInterstitial()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return adSettings.AdmobRewardedInterstitialAdUnit.Show();
#else
            return null;
#endif
        }

        public override void LoadAppOpen()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (!IsAppOpenReady()) adSettings.AdmobAppOpenAdUnit.Load();
#endif
        }

        public override bool IsAppOpenReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return adSettings.AdmobAppOpenAdUnit.IsReady();
#else
            return false;
#endif
        }

        public override void ShowAppOpen()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (statusAppOpenFirstIgnore) adSettings.AdmobAppOpenAdUnit.Show();
            statusAppOpenFirstIgnore = true;
#endif
        }

        public override void ShowBanner()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            adSettings.AdmobBannerAdUnit.Show();
#endif
        }

        public override void HideBanner()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            adSettings.AdmobBannerAdUnit.Hide();
#endif
        }

        public override void DestroyBanner()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            adSettings.AdmobBannerAdUnit.Destroy();
#endif
        }

#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        public void RegisterAppStateChange()
        {
            GoogleMobileAds.Api.AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }

        void OnAppStateChanged(GoogleMobileAds.Common.AppState state)
        {
            if (state == GoogleMobileAds.Common.AppState.Foreground && adSettings.AdmobAppOpenAdUnit.autoShow)
            {
                if (adSettings.CurrentAdNetwork == AdNetwork.Admob) ShowAppOpen();
            }
        }
#endif
    }
}