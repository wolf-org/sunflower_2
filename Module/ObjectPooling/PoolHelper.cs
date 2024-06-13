using UnityEngine;

namespace VirtueSky.ObjectPooling
{
    public static class PoolHelper
    {
        private static Pool _pool;

        public static void InitPool()
        {
            if (_pool == null)
            {
                _pool = new Pool();
                _pool.Initialize();
            }
        }

        public static Pool GetPool() => _pool;

        public static GameObject Spawn(this GameObject prefab, Transform parent = null, bool worldPositionStays = true,
            bool initialize = true)
        {
            return _pool.Spawn(prefab, parent, worldPositionStays, initialize);
        }

        public static T Spawn<T>(this T type, Transform parent = null, bool worldPositionStays = true,
            bool initialize = true) where T : Component
        {
            return _pool.Spawn(type, parent, worldPositionStays, initialize).GetComponent<T>();
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation,
            Transform parent = null,
            bool worldPositionStays = true,
            bool initialize = true)
        {
            return _pool.Spawn(prefab, position, rotation, parent, worldPositionStays, initialize);
        }

        public static T Spawn<T>(this T type, Vector3 position, Quaternion rotation, Transform parent = null,
            bool worldPositionStays = true, bool initialize = true)
            where T : Component
        {
            return _pool.Spawn(type, position, rotation, parent, worldPositionStays, initialize)
                .GetComponent<T>();
        }

        public static void DeSpawn(this GameObject gameObject, bool destroy = false, bool worldPositionStays = true)
        {
            _pool.DeSpawn(gameObject, destroy, worldPositionStays);
        }

        public static void DeSpawn<T>(this T type, bool destroy = false, bool worldPositionStays = true)
            where T : Component
        {
            _pool.DeSpawn(type, destroy, worldPositionStays);
        }
    }
}