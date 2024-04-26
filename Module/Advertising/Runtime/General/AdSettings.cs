using UnityEngine;
using VirtueSky.Utils;

namespace VirtueSky.Ads
{
    public class AdSettings : ScriptableSettings<AdSettings>
    {
        [SerializeField] private bool runtimeAutoInit;
        [Range(5, 100), SerializeField] private float adCheckingInterval = 8f;
        [Range(5, 100), SerializeField] private float adLoadingInterval = 15f;
        [SerializeField] private AdNetwork adNetwork = AdNetwork.Max;


        [Header("Applovin")] [Space, TextArea, SerializeField]
        private string sdkKey;

        [SerializeField] private bool applovinEnableAgeRestrictedUser;
        [SerializeField] private MaxBannerAdUnit maxBannerAdUnit;
        [SerializeField] private MaxInterstitialAdUnit maxInterstitialAdUnit;
        [SerializeField] private MaxRewardAdUnit maxRewardAdUnit;
        [SerializeField] private MaxRewardedInterstitialAdUnit maxRewardedInterstitialAdUnit;
        [SerializeField] private MaxAppOpenAdUnit maxAppOpenAdUnit;

        internal string SdkKey => Instance.sdkKey;
        internal bool ApplovinEnableAgeRestrictedUser => Instance.applovinEnableAgeRestrictedUser;
        internal MaxBannerAdUnit MaxBannerAdUnit => Instance.maxBannerAdUnit;
        internal MaxInterstitialAdUnit MaxInterstitialAdUnit => Instance.maxInterstitialAdUnit;
        internal MaxRewardAdUnit MaxRewardAdUnit => Instance.maxRewardAdUnit;

        internal MaxRewardedInterstitialAdUnit MaxRewardedInterstitialAdUnit =>
            Instance.maxRewardedInterstitialAdUnit;

        internal MaxAppOpenAdUnit MaxAppOpenAdUnit => Instance.maxAppOpenAdUnit;

        internal bool RuntimeAutoInit => runtimeAutoInit;
        internal float AdCheckingInterval => adCheckingInterval;
        internal float AdLoadingInterval => adLoadingInterval;

        internal AdNetwork CurrentAdNetwork
        {
            get => adNetwork;
            set => adNetwork = value;
        }
    }

    public enum AdNetwork
    {
        Max,
        Admob
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