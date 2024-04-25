using UnityEngine;
using Wolf.DataStorage;

namespace Wolf.Core
{
    public class RuntimeInitialize
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoInitialize()
        {
            var app = new GameObject("MonoGlobal");
            App.InitMonoGlobalComponent(app.AddComponent<MonoGlobal>());
            GameData.Init();
            Object.DontDestroyOnLoad(app);
        }
    }
}