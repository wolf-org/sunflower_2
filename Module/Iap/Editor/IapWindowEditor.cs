using UnityEditor;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Iap
{
    public class IapWindowEditor : EditorWindow
    {
        [MenuItem("Unity-Common/Iap/IapSettings %W", false)]
        public static void MenuOpenIapSettings()
        {
            var settings = CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Iap.IapSettings>();
            Selection.activeObject = settings;
            EditorGUIUtility.PingObject(settings);
            EditorUtility.FocusProjectWindow();
        }
    }
}