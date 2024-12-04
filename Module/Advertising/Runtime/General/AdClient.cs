namespace VirtueSky.Ads
{
    public abstract class AdClient
    {
        private bool statusAppOpenFirstIgnore;


        public abstract void Initialize();

        #region Inter Ad

        public AdUnit InterstitialAdUnit()
        {
            return AdSettings.CurrentAdNetwork switch
            {
                AdNetwork.Max => AdSettings.MaxInterstitialAdUnit,
                AdNetwork.Admob => AdSettings.AdmobInterstitialAdUnit,
                _ => AdSettings.IronSourceInterstitialAdUnit,
            };
        }

        protected virtual bool IsInterstitialReady()
        {
            return InterstitialAdUnit().IsReady();
        }

        public virtual void LoadInterstitial()
        {
            if (InterstitialAdUnit() == null) return;
            if (!IsInterstitialReady()) InterstitialAdUnit().Load();
        }

        #endregion

        #region Reward Ad

        public AdUnit RewardAdUnit()
        {
            return AdSettings.CurrentAdNetwork switch
            {
                AdNetwork.Max => AdSettings.MaxRewardAdUnit,
                AdNetwork.Admob => AdSettings.AdmobRewardAdUnit,
                _ => AdSettings.IronSourceRewardAdUnit,
            };
        }

        protected virtual bool IsRewardedReady()
        {
            return RewardAdUnit().IsReady();
        }

        public virtual void LoadRewarded()
        {
            if (RewardAdUnit() == null) return;
            if (!IsRewardedReady()) RewardAdUnit().Load();
        }

        #endregion

        #region Reward Inter Ad

        public AdUnit RewardedInterstitialAdUnit()
        {
            return AdSettings.CurrentAdNetwork switch
            {
                AdNetwork.Max => AdSettings.MaxRewardedInterstitialAdUnit,
                AdNetwork.Admob => AdSettings.AdmobRewardedInterstitialAdUnit,
                _ => null
            };
        }

        protected virtual bool IsRewardedInterstitialReady()
        {
            return RewardedInterstitialAdUnit().IsReady();
        }

        public virtual void LoadRewardedInterstitial()
        {
            if (RewardedInterstitialAdUnit() == null) return;
            if (!IsRewardedInterstitialReady()) RewardedInterstitialAdUnit().Load();
        }

        #endregion

        #region AppOpen Ad

        public AdUnit AppOpenAdUnit()
        {
            return AdSettings.CurrentAdNetwork switch
            {
                AdNetwork.Max => AdSettings.MaxAppOpenAdUnit,
                AdNetwork.Admob => AdSettings.AdmobAppOpenAdUnit,
                _ => null
            };
        }

        protected virtual bool IsAppOpenReady()
        {
            return AppOpenAdUnit().IsReady();
        }

        public virtual void LoadAppOpen()
        {
            if (AppOpenAdUnit() == null) return;
            if (!IsAppOpenReady()) AppOpenAdUnit().Load();
        }

        public virtual void ShowAppOpen()
        {
            if (statusAppOpenFirstIgnore) AppOpenAdUnit().Show();
            statusAppOpenFirstIgnore = true;
        }

        #endregion

        #region Banner Ad

        public AdUnit BannerAdUnit()
        {
            return AdSettings.CurrentAdNetwork switch
            {
                AdNetwork.Max => AdSettings.MaxBannerAdUnit,
                AdNetwork.Admob => AdSettings.AdmobBannerAdUnit,
                _ => AdSettings.IronSourceBannerAdUnit
            };
        }

        #endregion

        #region Native Overlay Ad

        // Native overlay only for admob
        public AdUnit NativeOverlayAdUnit()
        {
            return AdSettings.AdmobNativeOverlayAdUnit;
        }

        protected virtual bool IsNativeOverlayAdReady()
        {
            return NativeOverlayAdUnit().IsReady();
        }

        public virtual void LoadNativeOverlayAd()
        {
            if (NativeOverlayAdUnit() == null) return;
            if (!IsNativeOverlayAdReady()) NativeOverlayAdUnit().Load();
        }

        #endregion
    }
}