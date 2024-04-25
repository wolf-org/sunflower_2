namespace Wolf.Ads
{
    public abstract class AdClient
    {
        private AdSettings adSetting;
        protected bool statusAppOpenFirstIgnore;

        public abstract void Initialize();
        public abstract void LoadInterstitial();
        public abstract bool IsInterstitialReady();
        public abstract void LoadRewarded();
        public abstract bool IsRewardedReady();
        public abstract void LoadRewardedInterstitial();
        public abstract bool IsRewardedInterstitialReady();
        public abstract void LoadAppOpen();
        public abstract bool IsAppOpenReady();
    }
}