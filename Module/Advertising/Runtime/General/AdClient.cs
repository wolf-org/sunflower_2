namespace VirtueSky.Ads
{
    public abstract class AdClient
    {
        protected AdSettings adSettings;

        public void SetupAdSettings(AdSettings _adSettings)
        {
            this.adSettings = _adSettings;
        }

        private bool statusAppOpenFirstIgnore;


        public abstract void Initialize();

        #region Inter Ad

        public AdUnit InterstitialAdUnit()
        {
            return adSettings.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSettings.MaxInterstitialAdUnit,
                AdNetwork.Admob => adSettings.AdmobInterstitialAdUnit,
                _ => adSettings.IronSourceInterstitialAdUnit,
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
            return adSettings.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSettings.MaxRewardAdUnit,
                AdNetwork.Admob => adSettings.AdmobRewardAdUnit,
                _ => adSettings.IronSourceRewardAdUnit,
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
            return adSettings.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSettings.MaxRewardedInterstitialAdUnit,
                AdNetwork.Admob => adSettings.AdmobRewardedInterstitialAdUnit,
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
            return adSettings.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSettings.MaxAppOpenAdUnit,
                AdNetwork.Admob => adSettings.AdmobAppOpenAdUnit,
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
            return adSettings.CurrentAdNetwork switch
            {
                AdNetwork.Max => adSettings.MaxBannerAdUnit,
                AdNetwork.Admob => adSettings.AdmobBannerAdUnit,
                _ => adSettings.IronSourceBannerAdUnit
            };
        }

        #endregion
    }
}