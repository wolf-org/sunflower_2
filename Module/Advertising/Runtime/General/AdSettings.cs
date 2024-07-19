using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Utils;

namespace VirtueSky.Ads
{
    [EditorIcon("icon_scriptable")]
    public class AdSettings : ScriptableSettings<AdSettings>
    {
        [SerializeField] private bool runtimeAutoInit = true;
        [Range(5, 100), SerializeField] private float adCheckingInterval = 8f;
        [Range(5, 100), SerializeField] private float adLoadingInterval = 15f;
        [SerializeField] private AdNetwork adNetwork = AdNetwork.Max;

        [Tooltip("Install google-mobile-ads sdk to use GDPR"), SerializeField]
        private bool enableGDPR;

        [SerializeField] private bool enableGDPRTestMode;

        #region Max

        [TextArea, SerializeField] private string sdkKey;
        [SerializeField] private bool applovinEnableAgeRestrictedUser;
        [SerializeField] private MaxBannerAdUnit maxBannerAdUnit;
        [SerializeField] private MaxInterstitialAdUnit maxInterstitialAdUnit;
        [SerializeField] private MaxRewardAdUnit maxRewardAdUnit;
        [SerializeField] private MaxRewardedInterstitialAdUnit maxRewardedInterstitialAdUnit;
        [SerializeField] private MaxAppOpenAdUnit maxAppOpenAdUnit;

        public string SdkKey => Instance.sdkKey;
        public bool ApplovinEnableAgeRestrictedUser => Instance.applovinEnableAgeRestrictedUser;
        public MaxBannerAdUnit MaxBannerAdUnit => Instance.maxBannerAdUnit;
        public MaxInterstitialAdUnit MaxInterstitialAdUnit => Instance.maxInterstitialAdUnit;
        public MaxRewardAdUnit MaxRewardAdUnit => Instance.maxRewardAdUnit;

        public MaxRewardedInterstitialAdUnit MaxRewardedInterstitialAdUnit =>
            Instance.maxRewardedInterstitialAdUnit;

        public MaxAppOpenAdUnit MaxAppOpenAdUnit => Instance.maxAppOpenAdUnit;

        #endregion

        #region Admob

        [SerializeField] private AdmobBannerAdUnit admobBannerAdUnit;
        [SerializeField] private AdmobInterstitialAdUnit admobInterstitialAdUnit;
        [SerializeField] private AdmobRewardAdUnit admobRewardAdUnit;
        [SerializeField] private AdmobRewardedInterstitialAdUnit admobRewardedInterstitialAdUnit;
        [SerializeField] private AdmobAppOpenAdUnit admobAppOpenAdUnit;
        [SerializeField] private bool admobEnableTestMode;
        [SerializeField] private List<string> admobDevicesTest;


        public AdmobBannerAdUnit AdmobBannerAdUnit => admobBannerAdUnit;
        public AdmobInterstitialAdUnit AdmobInterstitialAdUnit => admobInterstitialAdUnit;
        public AdmobRewardAdUnit AdmobRewardAdUnit => admobRewardAdUnit;
        public AdmobRewardedInterstitialAdUnit AdmobRewardedInterstitialAdUnit => admobRewardedInterstitialAdUnit;
        public AdmobAppOpenAdUnit AdmobAppOpenAdUnit => admobAppOpenAdUnit;
        public bool AdmobEnableTestMode => admobEnableTestMode;
        public List<string> AdmobDevicesTest => admobDevicesTest;

        #endregion

        #region IronSource

        [SerializeField] private string androidAppKey;
        [SerializeField] private string iOSAppKey;
        [SerializeField] private bool useTestAppKey;
        [SerializeField] private IronSourceBannerAdUnit ironSourceBannerAdUnit;
        [SerializeField] private IronSourceInterstitialAdUnit ironSourceInterstitialAdUnit;
        [SerializeField] private IronSourceRewardAdUnit ironSourceRewardAdUnit;

        public string AndroidAppKey
        {
            get => androidAppKey;
            set => androidAppKey = value;
        }

        public string IosAppKey
        {
            get => iOSAppKey;
            set => iOSAppKey = value;
        }

        public string AppKey
        {
            get
            {
#if UNITY_ANDROID
                return androidAppKey;
#elif UNITY_IOS
                return iOSAppKey;
#else
                return string.Empty;
#endif
            }
            set
            {
#if UNITY_ANDROID
                androidAppKey = value;
#elif UNITY_IOS
                iOSAppKey = value;
#endif
            }
        }

        public bool UseTestAppKey => useTestAppKey;
        public IronSourceBannerAdUnit IronSourceBannerAdUnit => ironSourceBannerAdUnit;
        public IronSourceInterstitialAdUnit IronSourceInterstitialAdUnit => ironSourceInterstitialAdUnit;
        public IronSourceRewardAdUnit IronSourceRewardAdUnit => ironSourceRewardAdUnit;

        #endregion

        public bool RuntimeAutoInit => runtimeAutoInit;
        public float AdCheckingInterval => adCheckingInterval;
        public float AdLoadingInterval => adLoadingInterval;

        public AdNetwork CurrentAdNetwork
        {
            get => adNetwork;
            set => adNetwork = value;
        }

        public bool EnableGDPR => enableGDPR;
        public bool EnableGDPRTestMode => enableGDPRTestMode;
    }

    public enum AdNetwork
    {
        Max,
        Admob,
        IronSource
    }

    public enum BannerPosition
    {
        Top = 1,
        Bottom = 0,
        TopLeft = 2,
        TopRight = 3,
        BottomLeft = 4,
        BottomRight = 5,
    }

    public enum BannerSize
    {
        Banner = 0, // 320x50
        Adaptive = 5, // full width
        MediumRectangle = 1, // 300x250
        IABBanner = 2, // 468x60
        Leaderboard = 3, // 728x90
        //    SmartBanner = 4,
    }
}