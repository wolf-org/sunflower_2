using UnityEditor;
using UnityEngine;
using VirtueSky.UIButton;
using VirtueSky.UtilsEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ButtonCustom), true)]
[CanEditMultipleObjects]
public class ButtomCustomEditor : UnityEditor.UI.ButtonEditor
{
    private ButtonCustom _buttonCustom;
    private SerializedProperty _isHandleEventClickButton;
    private SerializedProperty _isMotion;
    private SerializedProperty _ease;
    private SerializedProperty _scale;
    private SerializedProperty _easingTypes;
    private SerializedProperty _isShrugOver;
    private SerializedProperty _timeShrug;
    private SerializedProperty _strength;

    protected override void OnEnable()
    {
        base.OnEnable();
        _buttonCustom = target as ButtonCustom;
        _isHandleEventClickButton = serializedObject.FindProperty("isHandleEventClickButton");
        _isMotion = serializedObject.FindProperty("isMotion");
        _easingTypes = serializedObject.FindProperty("easingTypes");
        _scale = serializedObject.FindProperty("scale");
        _isShrugOver = serializedObject.FindProperty("isShrugOver");
        _timeShrug = serializedObject.FindProperty("timeShrug");
        _strength = serializedObject.FindProperty("strength");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        GUILayout.Space(10);
        Uniform.DrawGroupFoldout("button_custom_setting", "Setting", () => DrawSetting(), false);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    void DrawSetting()
    {
        EditorGUILayout.PropertyField(_isHandleEventClickButton);
        EditorGUILayout.PropertyField(_isMotion);
        if (_isMotion.boolValue)
        {
            EditorGUILayout.PropertyField(_easingTypes);
            EditorGUILayout.PropertyField(_scale);
            EditorGUILayout.PropertyField(_isShrugOver);
            if (_isShrugOver.boolValue)
            {
                EditorGUILayout.PropertyField(_timeShrug);
                EditorGUILayout.PropertyField(_strength);
            }
        }
    }
}
#endif