using System;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    public class MaxBannerAdUnit : AdUnit
    {
        public BannerSize size = BannerSize.Banner;
        public BannerPosition position = BannerPosition.Bottom;

        // public MaxBannerAdUnit(string _androidId, string _iOSId) : base(_androidId, _iOSId)
        // {
        //     size = BannerSize.Banner;
        //     position = BannerPosition.Bottom;
        // }

        private bool isBannerDestroyed = true;
        private bool _registerCallback = false;

        public override void Init()
        {
            _registerCallback = false;
        }

        public override void Load()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            if (!_registerCallback)
            {
                MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnAdLoaded;
                MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnAdExpanded;
                MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnAdLoadFailed;
                MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnAdCollapsed;
                MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaid;
                MaxSdkCallbacks.Banner.OnAdClickedEvent += OnAdClicked;
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
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            Load();
            MaxSdk.ShowBanner(Id);
#endif
        }

        public override void Destroy()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (string.IsNullOrEmpty(Id)) return;
            isBannerDestroyed = true;
            MaxSdk.DestroyBanner(Id);
#endif
        }

        public void Hide()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            if (string.IsNullOrEmpty(Id)) return;
            MaxSdk.HideBanner(Id);
#endif
        }

        #region Fun Callback

#if VIRTUESKY_ADS && VIRTUESKY_MAX
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
            OnAdLoadEvent?.Invoke();
        }

        private void OnAdClicked(string arg1, MaxSdkBase.AdInfo arg2)
        {
            Common.CallActionAndClean(ref clickedCallback);
            OnAdClickedEvent?.Invoke();
        }

        private void OnAdExpanded(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref displayedCallback);
            OnAdDisplayedEvent?.Invoke();
        }

        private void OnAdLoadFailed(string unit, MaxSdkBase.ErrorInfo info)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            OnAdFailedToLoadEvent?.Invoke(info.Message);
        }

        private void OnAdCollapsed(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref closedCallback);
            OnAdClosedEvent?.Invoke();
        }
#endif

        #endregion
    }
}