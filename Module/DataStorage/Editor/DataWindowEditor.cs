using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace VirtueSky.DataStorage
{
#if UNITY_EDITOR
    public class DataWindowEditor : EditorWindow
    {
        [MenuItem("Unity-Common/Game Data/Clear All Data")]
        public static void ClearAllData()
        {
            GameData.DeleteAll();
            GameData.DeleteFileData();
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=Green>Clear all data succeed</color>");
        }

        [MenuItem("Unity-Common/Game Data/Save Data")]
        public static void SaveData()
        {
            GameData.Save();
            Debug.Log($"<color=Green>Save data succeed</color>");
        }

        [MenuItem("Unity-Common/Game Data/Clear Path Data")]
        public static void ClearSunDataPath()
        {
            GameData.DeleteAll();
            GameData.DeleteFileData();
            Debug.Log($"<color=Green>Clear path data succeed</color>");
        }

        [MenuItem("Unity-Common/Game Data/Clear PlayerPrefs Data")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=Green>Clear data PlayerPrefs succeed</color>");
        }

        [MenuItem("Unity-Common/Game Data/Open Path Data")]
        public static void OpenSunPathData()
        {
            string path = GameData.GetPersistentDataPath();
            switch (SystemInfo.operatingSystemFamily)
            {
                case OperatingSystemFamily.Windows:
                    if (Directory.Exists(path))
                    {
                        Process.Start(path);
                    }
                    else
                    {
                        Debug.LogError("The directory does not exist: " + path);
                    }

                    break;
                case OperatingSystemFamily.MacOSX:
                    if (Directory.Exists(path))
                    {
                        Process.Start("open", path);
                    }
                    else
                    {
                        Debug.LogError("The directory does not exist: " + path);
                    }

                    break;
            }
        }
    }
#endif
}