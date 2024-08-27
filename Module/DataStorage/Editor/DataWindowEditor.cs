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
        [MenuItem("Sunflower2/Clear All Data", priority = 501)]
        public static void ClearAllData()
        {
            GameData.DeleteAll();
            GameData.DeleteFileData();
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=Green>Clear all data succeed</color>");
        }

        [MenuItem("Sunflower2/Save Data", priority = 504)]
        public static void SaveData()
        {
            GameData.Save();
            Debug.Log($"<color=Green>Save data succeed</color>");
        }

        [MenuItem("Sunflower2/Clear Path Data", priority = 502)]
        public static void ClearDataPath()
        {
            GameData.DeleteAll();
            GameData.DeleteFileData();
            Debug.Log($"<color=Green>Clear path data succeed</color>");
        }


        [MenuItem("Sunflower2/Open Path Data", priority = 503)]
        public static void OpenPathData()
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