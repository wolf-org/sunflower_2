using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Utils;

namespace VirtueSky.Iap
{
    public class IapSettings : ScriptableSettings<IapSettings>
    {
        [SerializeField] private bool runtimeAutoInit = true;
        [SerializeField] private List<IapDataProduct> iapDataProducts = new List<IapDataProduct>();
        [SerializeField] private bool isValidatePurchase = true;
#if UNITY_EDITOR
        [SerializeField, TextArea] private string googlePlayStoreKey;
        public string GooglePlayStoreKey => googlePlayStoreKey;
#endif
        public bool RuntimeAutoInit => runtimeAutoInit;
        public List<IapDataProduct> IapDataProducts => iapDataProducts;
        public bool IsValidatePurchase => isValidatePurchase;
    }

    [Serializable]
    public class IapDataProduct
    {
#if UNITY_ANDROID
        public string androidId;
#elif UNITY_IOS
        public string iOSId;
#endif
        public IapProductType iapProductType;

        public string Id
        {
            get
            {
#if UNITY_ANDROID
                return androidId;
#elif UNITY_IOS
                return iOSId;
#else
                return string.Empty;
#endif
            }
        }
    }

    public enum IapProductType
    {
        /// <summary>
        /// Consumables may be purchased more than once.
        ///
        /// Purchase history is not typically retained by store
        /// systems once consumed.
        /// </summary>
        Consumable,

        /// <summary>
        /// Non consumables cannot be repurchased and are owned indefinitely.
        /// </summary>
        NonConsumable,

        /// <summary>
        /// Subscriptions have a finite window of validity.
        /// </summary>
        Subscription
    }
}