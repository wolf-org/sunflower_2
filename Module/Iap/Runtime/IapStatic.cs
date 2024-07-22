using System;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Threading.Tasks;

#if VIRTUESKY_IAP
using UnityEngine.Purchasing;
#endif


namespace VirtueSky.Iap
{
    public static class IapStatic
    {
#if VIRTUESKY_IAP
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RuntimeBeforeSceneLoad()
        {
            AutoInitialize(CoreEnum.RuntimeAutoInitType.BeforeSceneLoad);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RuntimeAfterSceneLoad()
        {
            AutoInitialize(CoreEnum.RuntimeAutoInitType.AfterSceneLoad);
        }

        private static void AutoInitialize(CoreEnum.RuntimeAutoInitType iapRuntimeAutoInitType)
        {
            if (IapSettings.Instance == null) return;
            if (!IapSettings.Instance.RuntimeAutoInit) return;
            if (IapSettings.Instance.RuntimeAutoInitType != iapRuntimeAutoInitType) return;
            var iapManager = new GameObject("IapManager");
            iapManager.AddComponent<IapManager>();
            UnityEngine.Object.DontDestroyOnLoad(iapManager);
        }
#endif


        public static IapDataProduct OnCompleted(this IapDataProduct product, Action onComplete)
        {
            product.purchaseSuccessCallback = onComplete;
            return product;
        }

        public static IapDataProduct OnFailed(this IapDataProduct product, Action onFailed)
        {
            product.purchaseFailedCallback = onFailed;
            return product;
        }
    }
}