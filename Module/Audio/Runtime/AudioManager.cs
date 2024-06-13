using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.DataStorage;
using VirtueSky.Inspector;
using VirtueSky.ObjectPooling;

namespace VirtueSky.Audio
{
    [EditorIcon("icon_sound_mixer")]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private Transform audioHolder;
        [SerializeField] private SoundComponent soundComponentPrefab;
        private SoundComponent music;

        private readonly Dictionary<SoundCache, SoundComponent> dictSfxCache =
            new Dictionary<SoundCache, SoundComponent>();

        private int key = 0;
        private const string KEY_MUSIC_VOLUME = "KEY_MUSIC_VOLUME";
        private const string KEY_SFX_VOLUME = "KEY_SFX_VOLUME";

        public float MusicVolume
        {
            get => GameData.Get(KEY_MUSIC_VOLUME, 1);
            set
            {
                GameData.Set(KEY_MUSIC_VOLUME, value);
                OnMusicVolumeChanged(value);
            }
        }

        public float SfxVolume
        {
            get => GameData.Get(KEY_SFX_VOLUME, 1);
            set
            {
                GameData.Set(KEY_SFX_VOLUME, value);
                OnSfxVolumeChanged(value);
            }
        }

        #region Sfx Method

        public SoundCache PlaySfx(SoundData soundData)
        {
            SoundComponent sfxComponent = soundComponentPrefab.Spawn(audioHolder);
            sfxComponent.PlayAudioClip(soundData.GetAudioClip(), soundData.loop, soundData.volume * SfxVolume);
            if (!soundData.loop) sfxComponent.OnCompleted += OnFinishPlayingAudio;
            SoundCache soundCache = GetSoundCache(soundData);
            dictSfxCache.Add(soundCache, sfxComponent);
            return soundCache;
        }


        public void StopSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null) return;
            StopAndCleanAudioComponent(soundComponent);
            if (dictSfxCache.ContainsKey(soundCache))
            {
                dictSfxCache.Remove(soundCache);
            }
        }


        public void PauseSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null || !soundComponent.IsPlaying) return;
            soundComponent.Pause();
        }

        public void ResumeSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null || soundComponent.IsPlaying) return;
            soundComponent.Resume();
        }


        public void FinishSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null || !soundComponent.IsPlaying) return;
            soundComponent.Finish();
            soundComponent.OnCompleted += OnFinishPlayingAudio;
        }

        public void StopAllSfx()
        {
            foreach (var cache in dictSfxCache)
            {
                StopAndCleanAudioComponent(cache.Value);
            }

            dictSfxCache.Clear();
            key = 0;
        }

        #endregion

        #region Music Method

        public void PlayMusic(SoundData soundData)
        {
            if (music == null || !music.IsPlaying)
            {
                music = soundComponentPrefab.Spawn(audioHolder);
            }

            music.FadePlayMusic(soundData.GetAudioClip(), soundData.loop, soundData.volume * MusicVolume,
                soundData.isMusicFadeVolume, soundData.fadeOutDuration, soundData.fadeInDuration);
            music.OnCompleted += StopAudioMusic;
        }


        public void StopMusic()
        {
            if (music != null && music.IsPlaying)
            {
                music.Stop();
                music.gameObject.DeSpawn();
            }
        }

        public void PauseMusic()
        {
            if (music != null && music.IsPlaying)
            {
                music.Pause();
            }
        }

        public void ResumeMusic()
        {
            if (music != null && !music.IsPlaying)
            {
                music.Resume();
            }
        }

        #endregion


        private void OnMusicVolumeChanged(float volume)
        {
            if (music != null)
            {
                music.Volume = volume;
            }
        }


        private void OnSfxVolumeChanged(float volume)
        {
            foreach (var cache in dictSfxCache)
            {
                cache.Value.Volume = volume;
            }
        }

        void OnFinishPlayingAudio(SoundComponent soundComponent)
        {
            StopAndCleanAudioComponent(soundComponent);
        }

        void StopAndCleanAudioComponent(SoundComponent soundComponent)
        {
            if (!soundComponent.IsLooping)
            {
                soundComponent.OnCompleted -= OnFinishPlayingAudio;
            }

            soundComponent.Stop();
            soundComponent.gameObject.DeSpawn();
        }

        void StopAudioMusic(SoundComponent soundComponent)
        {
            soundComponent.OnCompleted -= StopAudioMusic;
            soundComponent.gameObject.DeSpawn();
        }

        SoundComponent GetSoundComponent(SoundCache soundCache)
        {
            if (!dictSfxCache.ContainsKey(soundCache)) return null;
            foreach (var cache in dictSfxCache)
            {
                if (cache.Key == soundCache)
                {
                    return cache.Value;
                }
            }

            return null;
        }

        SoundCache GetSoundCache(SoundData soundData)
        {
            key++;
            return new SoundCache(key, soundData);
        }
    }
}