using System;
using VirtueSky.Misc;

namespace VirtueSky.Ads
{
    [Serializable]
    public class MaxInterstitialAdUnit : AdUnit
    {
        [NonSerialized] internal Action completedCallback;
        private bool _registerCallback = false;

        // public MaxInterstitialAdUnit(string _androidId, string _iOSId) : base(_androidId, _iOSId)
        // {
        // }

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
                MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnAdLoaded;
                MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnAdLoadFailed;
                MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaid;
                MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnAdDisplayed;
                MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnAdHidden;
                MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnAdDisplayFailed;
                MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnAdClicked;
                _registerCallback = true;
            }

            MaxSdk.LoadInterstitial(Id);
#endif
        }


        public override bool IsReady()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            return !string.IsNullOrEmpty(Id) && MaxSdk.IsInterstitialReady(Id);
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if VIRTUESKY_ADS && VIRTUESKY_MAX
            MaxSdk.ShowInterstitial(Id);
#endif
        }

        public override void Destroy()
        {
        }

        protected override void ResetChainCallback()
        {
            base.ResetChainCallback();
            completedCallback = null;
        }

        #region Func Callback

#if VIRTUESKY_ADS && VIRTUESKY_MAX
        private void OnAdDisplayFailed(string unit, MaxSdkBase.ErrorInfo error,
            MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref failedToDisplayCallback);
            OnAdFailedToDisplayEvent?.Invoke(error.Message);
        }

        private void OnAdHidden(string unit, MaxSdkBase.AdInfo info)
        {
            AdStatic.isShowingAd = false;
            Common.CallActionAndClean(ref completedCallback);
            if (!string.IsNullOrEmpty(Id)) MaxSdk.LoadInterstitial(Id);
        }

        private void OnAdDisplayed(string unit, MaxSdkBase.AdInfo info)
        {
            AdStatic.isShowingAd = true;
            Common.CallActionAndClean(ref displayedCallback);
            OnAdDisplayedEvent?.Invoke();
        }

        private void OnAdClicked(string arg1, MaxSdkBase.AdInfo arg2)
        {
            Common.CallActionAndClean(ref clickedCallback);
            OnAdClickedEvent?.Invoke();
        }

        private void OnAdRevenuePaid(string unit, MaxSdkBase.AdInfo info)
        {
            paidedCallback?.Invoke(info.Revenue,
                info.NetworkName,
                unit,
                info.AdFormat, AdNetwork.Max.ToString());
        }

        private void OnAdLoadFailed(string unit, MaxSdkBase.ErrorInfo info)
        {
            Common.CallActionAndClean(ref failedToLoadCallback);
            OnAdFailedToLoadEvent?.Invoke(info.Message);
        }

        private void OnAdLoaded(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref loadedCallback);
            OnAdLoadEvent?.Invoke();
        }
#endif

        #endregion
    }
}