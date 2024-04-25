using System;
using Wolf.Misc;

namespace Wolf.Ads
{
    [Serializable]
    public class MaxBannerAdUnit : AdUnit
    {
        public BannerSize size;
        public BannerPosition position;

        public MaxBannerAdUnit(string _androidId, string _iOSId) : base(_androidId, _iOSId)
        {
            size = BannerSize.Banner;
            position = BannerPosition.Bottom;
        }

        private bool isBannerDestroyed = true;
        private bool _registerCallback = false;

        public override void Init()
        {
            _registerCallback = false;
        }

        public override void Load()
        {
#if WOLF_ADS && ADS_MAX
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            if (!_registerCallback)
            {
                MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnAdLoaded;
                MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnAdExpanded;
                MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnAdLoadFailed;
                MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnAdCollapsed;
                MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaid;
                if (size != BannerSize.Adaptive)
                    MaxSdk.SetBannerExtraParameter(Id, "adaptive_banner", "false");
                _registerCallback = true;
            }

            if (isBannerDestroyed)
            {
                MaxSdk.CreateBanner(Id, ConvertPosition());
            }
#endif
        }

        public override bool IsReady()
        {
            return !string.IsNullOrEmpty(Id);
        }

        protected override void ShowImpl()
        {
#if WOLF_ADS && ADS_MAX
            Load();
            MaxSdk.ShowBanner(Id);
#endif
        }

        public override void Destroy()
        {
#if WOLF_ADS && ADS_MAX
            if (string.IsNullOrEmpty(Id)) return;
            isBannerDestroyed = true;
            MaxSdk.DestroyBanner(Id);
#endif
        }

        public void Hide()
        {
#if WOLF_ADS && ADS_MAX
            if (string.IsNullOrEmpty(Id)) return;
            MaxSdk.HideBanner(Id);
#endif
        }

        #region Fun Callback

#if WOLF_ADS && ADS_MAX
        public MaxSdkBase.BannerPosition ConvertPosition()
        {
            switch (position)
            {
                case BannerPosition.Top: return MaxSdkBase.BannerPosition.TopCenter;
                case BannerPosition.Bottom: return MaxSdkBase.BannerPosition.BottomCenter;
                case BannerPosition.TopLeft: return MaxSdkBase.BannerPosition.TopLeft;
                case BannerPosition.TopRight: return MaxSdkBase.BannerPosition.TopRight;
                case BannerPosition.BottomLeft: return MaxSdkBase.BannerPosition.BottomLeft;
                case BannerPosition.BottomRight: return MaxSdkBase.BannerPosition.BottomRight;
                default:
                    return MaxSdkBase.BannerPosition.BottomCenter;
            }
        }

        private void OnAdRevenuePaid(string unit, MaxSdkBase.AdInfo info)
        {
            paidedCallback?.Invoke(info.Revenue,
                info.NetworkName,
                unit,
                info.AdFormat, AdNetwork.Max.ToString());
        }

        private void OnAdLoaded(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref loadedCallback);
        }

        private void OnAdExpanded(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref displayedCallback);
        }

        private void OnAdLoadFailed(string unit, MaxSdkBase.ErrorInfo info)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
        }

        private void OnAdCollapsed(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref closedCallback);
        }
#endif

        #endregion
    }
}