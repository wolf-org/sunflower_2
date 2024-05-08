using UnityEngine;

namespace VirtueSky.ObjectPooling
{
    public static class PoolStatic
    {
        public static GameObject Spawn(this GameObject prefab, Transform parent = null, bool worldPositionStays = true,
            bool initialize = true)
        {
            return PoolManager.Instance.Spawn(prefab, parent, worldPositionStays, initialize);
        }

        public static T Spawn<T>(this T type, Transform parent = null, bool worldPositionStays = true,
            bool initialize = true) where T : Component
        {
            return PoolManager.Instance.Spawn(type, parent, worldPositionStays, initialize).GetComponent<T>();
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation,
            Transform parent = null,
            bool worldPositionStays = true,
            bool initialize = true)
        {
            return PoolManager.Instance.Spawn(prefab, position, rotation, parent, worldPositionStays, initialize);
        }

        public static T Spawn<T>(this T type, Vector3 position, Quaternion rotation, Transform parent = null,
            bool worldPositionStays = true, bool initialize = true)
            where T : Component
        {
            return PoolManager.Instance.Spawn(type, position, rotation, parent, worldPositionStays, initialize)
                .GetComponent<T>();
        }

        public static void DeSpawn(this GameObject gameObject, bool destroy = false, bool worldPositionStays = true)
        {
            PoolManager.Instance.DeSpawn(gameObject, destroy, worldPositionStays);
        }
    }
}