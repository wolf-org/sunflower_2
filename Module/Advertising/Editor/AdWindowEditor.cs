using UnityEditor;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Ads
{
    public class AdWindowEditor : EditorWindow
    {
        [MenuItem("Unity-Common/Ads/AdSettings %E", false)]
        public static void MenuOpenAdSettings()
        {
            var adSetting = CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Ads.AdSettings>();
            Selection.activeObject = adSetting;
            EditorGUIUtility.PingObject(adSetting);
            EditorUtility.FocusProjectWindow();
        }
    }
}