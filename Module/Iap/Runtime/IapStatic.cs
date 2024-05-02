using System;

#if VIRTUESKY_IAP
using UnityEngine.Purchasing;
#endif


namespace VirtueSky.Iap
{
    public static class IapStatic
    {
        public static Action<string> OnPurchaseFailed;
        public static Action<string> OnPurchaseSuccess;
#if VIRTUESKY_IAP
        public static Action<Product> OnIapTrackingRevenue;
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