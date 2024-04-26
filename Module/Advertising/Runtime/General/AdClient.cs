using System;

namespace VirtueSky.Ads
{
    public abstract class AdClient
    {
        protected AdSettings adSettings;

        public void SetupAdSettings(AdSettings _adSettings)
        {
            this.adSettings = _adSettings;
        }

        protected bool statusAppOpenFirstIgnore;

        public abstract void Initialize();
        public abstract void LoadInterstitial();
        public abstract bool IsInterstitialReady();
        public abstract AdUnit ShowInterstitial();
        public abstract void LoadRewarded();
        public abstract bool IsRewardedReady();
        public abstract AdUnit ShowReward();
        public abstract void LoadRewardedInterstitial();
        public abstract bool IsRewardedInterstitialReady();
        public abstract AdUnit ShowRewardedInterstitial();
        public abstract void LoadAppOpen();
        public abstract bool IsAppOpenReady();
        public abstract void ShowAppOpen();
        public abstract void ShowBanner();
        public abstract void HideBanner();
        public abstract void DestroyBanner();
    }
}