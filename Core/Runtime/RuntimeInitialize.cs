using UnityEngine;
using VirtueSky.DataStorage;

namespace VirtueSky.Core
{
    public struct RuntimeInitialize
    {
        public static bool IsInitializedMonoGlobal { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoInitialize()
        {
            IsInitializedMonoGlobal = false;
            var app = new GameObject("MonoGlobal");
            App.InitMonoGlobalComponent(app.AddComponent<MonoGlobal>());
            IsInitializedMonoGlobal = true;
            Object.DontDestroyOnLoad(app);
            GameData.Init();
        }
    }
}