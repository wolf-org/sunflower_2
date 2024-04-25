using UnityEngine;
using Wolf.DataStorage;

namespace Wolf.Ads
{
    public static class AdStatic
    {
        public static bool IsRemoveAd
        {
            get => GameData.Get($"{Application.identifier}_removeads", false);
            set => GameData.Set($"{Application.identifier}_removeads", value);
        }
    }
}