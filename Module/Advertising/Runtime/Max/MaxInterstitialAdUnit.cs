using System;
using Wolf.Misc;

namespace Wolf.Ads
{
    [Serializable]
    public class MaxInterstitialAdUnit : AdUnit
    {
        [NonSerialized] internal Action completedCallback;
        private bool _registerCallback = false;

        public MaxInterstitialAdUnit(string _androidId, string _iOSId) : base(_androidId, _iOSId)
        {
        }

        public override void Init()
        {
            _registerCallback = false;
        }

        public override void Load()
        {
#if WOLF_ADS && WOLF_MAX
            if (AdStatic.IsRemoveAd || string.IsNullOrEmpty(Id)) return;
            if (!_registerCallback)
            {
                MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnAdLoaded;
                MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnAdLoadFailed;
                MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaid;
                MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnAdDisplayed;
                MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnAdHidden;
                MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnAdDisplayFailed;
                _registerCallback = true;
            }

            MaxSdk.LoadInterstitial(Id);
#endif
        }

        public override bool IsReady()
        {
#if WOLF_ADS && WOLF_MAX
            return !string.IsNullOrEmpty(Id) && MaxSdk.IsInterstitialReady(Id);
#else
            return false;
#endif
        }

        protected override void ShowImpl()
        {
#if WOLF_ADS && WOLF_MAX
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

#if WOLF_ADS && WOLF_MAX
        private void OnAdDisplayFailed(string unit, MaxSdkBase.ErrorInfo error,
            MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref failedToDisplayCallback);
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
        }

        private void OnAdLoaded(string unit, MaxSdkBase.AdInfo info)
        {
            Common.CallActionAndClean(ref loadedCallback);
        }
#endif

        #endregion
    }
}