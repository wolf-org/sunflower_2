#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
using GoogleMobileAds.Api;
#endif
using System;
using VirtueSky.Core;

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


            MobileAds.Initialize(initStatus =>
            {
                App.RunOnMainThread(() =>
                {
                    if (!AdSettings.AdmobEnableTestMode) return;
                    var configuration = new RequestConfiguration
                        { TestDeviceIds = AdSettings.AdmobDevicesTest };
                    MobileAds.SetRequestConfiguration(configuration);
                });
            });
            AdSettings.AdmobBannerAdUnit.Init();
            AdSettings.AdmobInterstitialAdUnit.Init();
            AdSettings.AdmobRewardAdUnit.Init();
            AdSettings.AdmobRewardedInterstitialAdUnit.Init();
            AdSettings.AdmobAppOpenAdUnit.Init();
            AdSettings.AdmobNativeOverlayAdUnit.Init();
            RegisterAppStateChange();
            LoadInterstitial();
            LoadRewarded();
            LoadRewardedInterstitial();
            LoadAppOpen();
            LoadNativeOverlay();
#endif
        }

        void LoadNativeOverlay()
        {
            if (!AdSettings.AdmobNativeOverlayAdUnit.IsReady())
            {
                AdSettings.AdmobNativeOverlayAdUnit.Load();
            }
        }

#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        public void RegisterAppStateChange()
        {
            GoogleMobileAds.Api.AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }

        void OnAppStateChanged(GoogleMobileAds.Common.AppState state)
        {
            if (state == GoogleMobileAds.Common.AppState.Foreground && AdSettings.AdmobAppOpenAdUnit.autoShow &&
                !AdStatic.isShowingAd)
            {
                if (AdSettings.CurrentAdNetwork == AdNetwork.Admob) ShowAppOpen();
            }
        }
#endif
    }
}