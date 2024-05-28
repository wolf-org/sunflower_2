using System;
using UnityEngine;
using VirtueSky.Core;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
using GoogleMobileAds.Api;
#endif
using System.Collections;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    public class AdmobBannerAdUnit : AdUnit
    {
        public BannerSize size = BannerSize.Adaptive;
        public BannerPosition position = BannerPosition.Bottom;
        public bool useCollapsible;
        public bool useTestId;
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        private BannerView _bannerView;
#endif
        private readonly WaitForSeconds _waitBannerReload = new WaitForSeconds(5f);
        private IEnumerator _reload;

        public override void Init()
        {
            if (useTestId)
            {
                GetUnitTest();
            }
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            Destroy();
            _bannerView = new BannerView(Id, ConvertSize(), ConvertPosition());
            _bannerView.OnAdFullScreenContentClosed += OnAdClosed;
            _bannerView.OnBannerAdLoadFailed += OnAdFailedToLoad;
            _bannerView.OnBannerAdLoaded += OnAdLoaded;
            _bannerView.OnAdFullScreenContentOpened += OnAdOpening;
            _bannerView.OnAdPaid += OnAdPaided;
            _bannerView.OnAdClicked += OnAdClicked;
            var adRequest = new AdRequest();
            if (useCollapsible)
            {
                adRequest.Extras.Add("collapsible", ConvertPlacementCollapsible());
            }

            _bannerView.LoadAd(adRequest);

#endif
        }


        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            return _bannerView != null;
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            Load();
            _bannerView.Show();
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (_bannerView == null) return;
            _bannerView.Destroy();
            _bannerView = null;
#endif
        }

        public void Hide()
        {
#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
            if (_bannerView == null) return;
            _bannerView.Hide();
#endif
        }

        #region Fun Callback

#if VIRTUESKY_ADS && VIRTUESKY_ADMOB
        public AdSize ConvertSize()
        {
            switch (size)
            {
                case BannerSize.Adaptive:
                    return AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(
                        AdSize.FullWidth);
                case BannerSize.MediumRectangle: return AdSize.MediumRectangle;
                case BannerSize.Leaderboard: return AdSize.Leaderboard;
                case BannerSize.IABBanner: return AdSize.IABBanner;
                //case BannerSize.SmartBanner: return AdSize.SmartBanner;
                default: return AdSize.Banner;
            }
        }

        private void OnAdClicked()
        {
            Common.CallActionAndClean(ref clickedCallback);
            OnAdClickedEvent?.Invoke();
        }

        public AdPosition ConvertPosition()
        {
            switch (position)
            {
                case BannerPosition.Top: return AdPosition.Top;
                case BannerPosition.Bottom: return AdPosition.Bottom;
                case BannerPosition.TopLeft: return AdPosition.TopLeft;
                case BannerPosition.TopRight: return AdPosition.TopRight;
                case BannerPosition.BottomLeft: return AdPosition.BottomLeft;
                case BannerPosition.BottomRight: return AdPosition.BottomRight;
                default: return AdPosition.Bottom;
            }
        }

        public string ConvertPlacementCollapsible()
        {
            if (position == BannerPosition.Top)
            {
                return "top";
            }
            else if (position == BannerPosition.Bottom)
            {
                return "bottom";
            }

            return "bottom";
        }

        private void OnAdPaided(AdValue value)
        {
            paidedCallback?.Invoke(value.Value / 1000000f,
                "Admob",
                Id,
                "BannerAd", AdNetwork.Admob.ToString());
        }

        private void OnAdOpening()
        {
            Common.CallActionAndClean(ref displayedCallback);
            OnAdDisplayedEvent?.Invoke();
        }

        private void OnAdLoaded()
        {
            Common.CallActionAndClean(ref loadedCallback);
            OnAdLoadEvent?.Invoke();
        }

        private void OnAdFailedToLoad(LoadAdError error)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            OnAdFailedToLoadEvent?.Invoke(error.GetMessage());
            if (_reload != null) App.StopCoroutine(_reload);
            _reload = DelayBannerReload();
            App.StartCoroutine(_reload);
        }

        private void OnAdClosed()
        {
            Common.CallActionAndClean(ref closedCallback);
            OnAdClosedEvent?.Invoke();
        }

        private IEnumerator DelayBannerReload()
        {
            yield return _waitBannerReload;
            Load();
        }
#endif

        #endregion

        void GetUnitTest()
        {
#if UNITY_ANDROID
            androidId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IOS
            iOSId = "ca-app-pub-3940256099942544/2934735716";
#endif
        }
    }
}