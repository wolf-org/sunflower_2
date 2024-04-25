using UnityEditor;
using Wolf.Utils;

namespace Wolf.Ads.Editor
{
    public class AdWindowEditor : EditorWindow
    {
        [MenuItem("Unity-Common/Ads/AdSettings %E", false)]
        public static void MenuOpenAdSettings()
        {
            var adSetting = CreateAsset.CreateAndGetScriptableAsset<Wolf.Ads.AdSettings>("/Ads");
            Selection.activeObject = adSetting;
            EditorGUIUtility.PingObject(adSetting);
            EditorUtility.FocusProjectWindow();
        }
    }
}