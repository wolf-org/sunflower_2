using UnityEditor;
using VirtueSky.Utils;

namespace VirtueSky.Ads
{
    public class AdWindowEditor : EditorWindow
    {
        [MenuItem("Unity-Common/Ads/AdSettings %E", false)]
        public static void MenuOpenAdSettings()
        {
            var adSetting = FileExtension.CreateAndGetScriptableAsset<VirtueSky.Ads.AdSettings>();
            Selection.activeObject = adSetting;
            EditorGUIUtility.PingObject(adSetting);
            EditorUtility.FocusProjectWindow();
        }
    }
}