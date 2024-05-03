using UnityEditor;
using UnityEngine;
using VirtueSky.Audio;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPAudioDrawer
    {
        public static void OnDrawAudio(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("AUDIO", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create Sound Data"))
            {
                AudioWindowEditor.CreateSoundData();
            }

            GUILayout.EndVertical();
        }
    }
}