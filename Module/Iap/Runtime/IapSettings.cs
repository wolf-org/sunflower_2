using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Utils;

namespace VirtueSky.Iap
{
    [EditorIcon("icon_scriptable")]
    public class IapSettings : ScriptableSettings<IapSettings>
    {
        [SerializeField] private bool runtimeAutoInit = true;
        [SerializeField] private CoreEnum.RuntimeAutoInitType runtimeAutoInitType;
        [SerializeField] private List<IapDataProduct> iapDataProducts = new List<IapDataProduct>();
        [SerializeField] private bool isValidatePurchase = true;
#if UNITY_EDITOR
        [SerializeField, TextArea] private string googlePlayStoreKey;
        public string GooglePlayStoreKey => googlePlayStoreKey;
#endif
        public bool RuntimeAutoInit => runtimeAutoInit;
        public CoreEnum.RuntimeAutoInitType RuntimeAutoInitType => runtimeAutoInitType;
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