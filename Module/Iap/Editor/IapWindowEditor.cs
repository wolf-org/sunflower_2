using UnityEditor;
using VirtueSky.Utils;

namespace VirtueSky.Iap
{
    public class IapWindowEditor : EditorWindow
    {
        [MenuItem("Unity-Common/Iap/IapSettings %W", false)]
        public static void MenuOpenIapSettings()
        {
            var settings = FileExtension.CreateAndGetScriptableAsset<VirtueSky.Iap.IapSettings>();
            Selection.activeObject = settings;
            EditorGUIUtility.PingObject(settings);
            EditorUtility.FocusProjectWindow();
        }
    }
}