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

        public IapDataProduct GetIapProduct(string id)
        {
            foreach (var product in IapDataProducts)
            {
                if (product.Id == id) return product;
            }

            return null;
        }
    }
}