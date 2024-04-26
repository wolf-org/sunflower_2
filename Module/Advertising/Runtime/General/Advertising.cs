using System;
using System.Collections;
using UnityEngine;
using VirtueSky.Core;

namespace VirtueSky.Ads
{
    public class Advertising : Singleton<Advertising>
    {
        private IEnumerator autoLoadAdCoroutine;
        private float _lastTimeLoadInterstitialAdTimestamp = DEFAULT_TIMESTAMP;
        private float _lastTimeLoadRewardedTimestamp = DEFAULT_TIMESTAMP;
        private float _lastTimeLoadRewardedInterstitialTimestamp = DEFAULT_TIMESTAMP;
        private float _lastTimeLoadAppOpenTimestamp = DEFAULT_TIMESTAMP;
        private const float DEFAULT_TIMESTAMP = -1000;

        private AdClient currentAdClient;
        private AdSettings adSettings;

        private void Start()
        {
            adSettings = AdSettings.Instance;
            switch (adSettings.CurrentAdNetwork)
            {
                case AdNetwork.Max:
                    currentAdClient = new MaxAdClient();
                    currentAdClient.SetupAdSettings(adSettings);
                    break;
                case AdNetwork.Admob:
                    break;
            }

            InitAutoLoadAds();
        }

        public void InitAutoLoadAds()
        {
            currentAdClient.Initialize();
            if (autoLoadAdCoroutine != null) StopCoroutine(autoLoadAdCoroutine);
            autoLoadAdCoroutine = IeAutoLoadAll();
            StartCoroutine(autoLoadAdCoroutine);
            Debug.Log("currentAdClient: " + currentAdClient);
        }

        IEnumerator IeAutoLoadAll(float delay = 0)
        {
            if (delay > 0) yield return new WaitForSeconds(delay);
            while (true)
            {
                AutoLoadInterAds();
                AutoLoadRewardAds();
                AutoLoadRewardInterAds();
                AutoLoadAppOpenAds();
                yield return new WaitForSeconds(adSettings.AdCheckingInterval);
            }
        }

        public void OnChangePreventDisplayOpenAd(bool state)
        {
            AdStatic.isShowingAd = state;
        }

        #region Fun Show Ads

        public AdUnit ShowInterstitial()
        {
            return currentAdClient.ShowInterstitial();
        }

        public AdUnit ShowReward()
        {
            return currentAdClient.ShowReward();
        }

        public AdUnit ShowRewardedInterstitial()
        {
            return currentAdClient.ShowRewardedInterstitial();
        }

        public void ShowAppOpen()
        {
            currentAdClient.ShowAppOpen();
        }

        public void ShowBanner()
        {
            currentAdClient.ShowBanner();
        }

        public void HideBanner()
        {
            currentAdClient.HideBanner();
        }

        public void DestroyBanner()
        {
            currentAdClient.DestroyBanner();
        }

        #endregion

        #region Func Load Ads

        void AutoLoadInterAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadInterstitialAdTimestamp <
                adSettings.AdLoadingInterval) return;
            currentAdClient.LoadInterstitial();
            _lastTimeLoadInterstitialAdTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadRewardAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadRewardedTimestamp <
                adSettings.AdLoadingInterval) return;
            currentAdClient.LoadRewarded();
            _lastTimeLoadRewardedTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadRewardInterAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadRewardedInterstitialTimestamp <
                adSettings.AdLoadingInterval) return;
            currentAdClient.LoadRewardedInterstitial();
            _lastTimeLoadRewardedInterstitialTimestamp = Time.realtimeSinceStartup;
        }

        void AutoLoadAppOpenAds()
        {
            if (Time.realtimeSinceStartup - _lastTimeLoadAppOpenTimestamp <
                adSettings.AdLoadingInterval) return;
            currentAdClient.LoadAppOpen();
            _lastTimeLoadAppOpenTimestamp = Time.realtimeSinceStartup;
        }

        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInitialize()
        {
            if (AdSettings.Instance.RuntimeAutoInit)
            {
                var ads = new GameObject("Advertising");
                ads.AddComponent<Advertising>();
                DontDestroyOnLoad(ads);
            }
        }
    }
}