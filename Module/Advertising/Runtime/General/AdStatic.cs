using System;
using UnityEngine;
using VirtueSky.DataStorage;

namespace VirtueSky.Ads
{
    public static class AdStatic
    {
        public static bool IsRemoveAd
        {
            get => GameData.Get($"{Application.identifier}_removeads", false);
            set => GameData.Set($"{Application.identifier}_removeads", value);
        }

        internal static bool isShowingAd;

        public static AdUnit OnDisplayed(this AdUnit unit, Action onDisplayed)
        {
            unit.displayedCallback = onDisplayed;
            return unit;
        }

        public static AdUnit OnClosed(this AdUnit unit, Action onClosed)
        {
            unit.closedCallback = onClosed;
            return unit;
        }

        public static AdUnit OnLoaded(this AdUnit unit, Action onLoaded)
        {
            unit.loadedCallback = onLoaded;
            return unit;
        }

        public static AdUnit OnFailedToLoad(this AdUnit unit, Action onFailedToLoad)
        {
            unit.failedToLoadCallback = onFailedToLoad;
            return unit;
        }

        public static AdUnit OnFailedToDisplay(this AdUnit unit, Action onFailedToDisplay)
        {
            unit.failedToDisplayCallback = onFailedToDisplay;
            return unit;
        }

        public static AdUnit OnCompleted(this AdUnit unit, Action onCompleted)
        {
            if (!Application.isMobilePlatform)
            {
                onCompleted?.Invoke();
            }

            switch (unit)
            {
                // case AdmobInterVariable admobInter:
                //     admobInter.completedCallback = onCompleted;
                //     return unit;
                // case AdmobRewardVariable admobReward:
                //     admobReward.completedCallback = onCompleted;
                //     return unit;
                // case AdmobRewardInterVariable admobRewardInter:
                //     admobRewardInter.completedCallback = onCompleted;
                //     return unit;
                case MaxInterstitialAdUnit maxInter:
                    maxInter.completedCallback = onCompleted;
                    return unit;
                case MaxRewardAdUnit maxReward:
                    maxReward.completedCallback = onCompleted;
                    return unit;
                case MaxRewardedInterstitialAdUnit maxRewardInter:
                    maxRewardInter.completedCallback = onCompleted;
                    return unit;
            }

            return unit;
        }

        public static AdUnit OnSkipped(this AdUnit unit, Action onSkipped)
        {
            switch (unit)
            {
                // case AdmobRewardVariable admobReward:
                //     admobReward.skippedCallback = onSkipped;
                //     return unit;
                // case AdmobRewardInterVariable admobRewardInter:
                //     admobRewardInter.skippedCallback = onSkipped;
                //     return unit;
                case MaxRewardAdUnit maxReward:
                    maxReward.skippedCallback = onSkipped;
                    return unit;
                case MaxRewardedInterstitialAdUnit maxRewardInter:
                    maxRewardInter.skippedCallback = onSkipped;
                    return unit;
            }

            return unit;
        }
    }
}