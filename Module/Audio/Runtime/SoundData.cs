using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;
using Random = UnityEngine.Random;

namespace VirtueSky.Audio
{
    [CreateAssetMenu(menuName = "Sunflower2/Audio/Sound Data", fileName = "sound_data")]
    [EditorIcon("scriptable_audioclip")]
    public class SoundData : ScriptableObject
    {
        [Space] public bool loop = false;
        [Range(0f, 1f)] public float volume = 1;

        public SoundType soundType;

        [Space, Header("Fade Volume - Only Music"), Tooltip("Only Music Background")] [ShowIf(nameof(soundType), SoundType.Music)]
        public bool isMusicFadeVolume = false;

        [ShowIf(nameof(ConditionFadeMusic), true)]
        public float fadeInDuration = .5f;

        [ShowIf(nameof(ConditionFadeMusic), true)]
        public float fadeOutDuration = .5f;

        [Space, SerializeField] private List<AudioClip> audioClips;
        public bool ConditionFadeMusic => isMusicFadeVolume && soundType == SoundType.Music;
        public int NumberOfAudioClips => audioClips.Count;

        public AudioClip GetAudioClip()
        {
            if (audioClips.Count > 0)
            {
                return audioClips[Random.Range(0, audioClips.Count)];
            }

            return null;
        }
    }

    public enum SoundType
    {
        Sfx,
        Music
    }
}