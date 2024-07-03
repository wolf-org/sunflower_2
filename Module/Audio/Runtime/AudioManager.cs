using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.DataStorage;
using VirtueSky.Inspector;
using VirtueSky.ObjectPooling;

namespace VirtueSky.Audio
{
    [EditorIcon("icon_sound_mixer"), HideMonoScript]
    public class AudioManager : Singleton<AudioManager>
    {
        [Space, SerializeField] private Transform audioHolder;
        [SerializeField] private SoundComponent soundComponentPrefab;
        private SoundComponent music;

        private readonly Dictionary<SoundCache, SoundComponent> dictSfxCache =
            new Dictionary<SoundCache, SoundComponent>();

        private int key = 0;
        private const string KEY_MUSIC_VOLUME = "KEY_MUSIC_VOLUME";
        private const string KEY_SFX_VOLUME = "KEY_SFX_VOLUME";

        private float InternalMusicVolume
        {
            get => GameData.Get(KEY_MUSIC_VOLUME, 1);
            set
            {
                GameData.Set(KEY_MUSIC_VOLUME, value);
                OnMusicVolumeChanged(value);
            }
        }

        private float InternalSfxVolume
        {
            get => GameData.Get(KEY_SFX_VOLUME, 1);
            set
            {
                GameData.Set(KEY_SFX_VOLUME, value);
                OnSfxVolumeChanged(value);
            }
        }

        #region Sfx Method

        private SoundCache InternalPlaySfx(SoundData soundData)
        {
            SoundComponent sfxComponent = soundComponentPrefab.Spawn(audioHolder);
            sfxComponent.PlayAudioClip(soundData.GetAudioClip(), soundData.loop, soundData.volume * InternalSfxVolume);
            if (!soundData.loop) sfxComponent.OnCompleted += OnFinishPlayingAudio;
            SoundCache soundCache = GetSoundCache(soundData);
            dictSfxCache.Add(soundCache, sfxComponent);
            return soundCache;
        }


        private void InternalStopSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null) return;
            StopAndCleanAudioComponent(soundComponent);
            if (dictSfxCache.ContainsKey(soundCache))
            {
                dictSfxCache.Remove(soundCache);
            }
        }


        private void InternalPauseSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null || !soundComponent.IsPlaying) return;
            soundComponent.Pause();
        }

        private void InternalResumeSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null || soundComponent.IsPlaying) return;
            soundComponent.Resume();
        }


        private void InternalFinishSfx(SoundCache soundCache)
        {
            var soundComponent = GetSoundComponent(soundCache);
            if (soundComponent == null || !soundComponent.IsPlaying) return;
            soundComponent.Finish();
            soundComponent.OnCompleted += OnFinishPlayingAudio;
        }

        private void InternalStopAllSfx()
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

        private void InternalPlayMusic(SoundData soundData)
        {
            if (music == null || !music.IsPlaying)
            {
                music = soundComponentPrefab.Spawn(audioHolder);
            }

            music.FadePlayMusic(soundData.GetAudioClip(), soundData.loop, soundData.volume * InternalMusicVolume,
                soundData.isMusicFadeVolume, soundData.fadeOutDuration, soundData.fadeInDuration);
            music.OnCompleted += StopAudioMusic;
        }


        private void InternalStopMusic()
        {
            if (music != null && music.IsPlaying)
            {
                music.Stop();
                music.gameObject.DeSpawn();
            }
        }

        private void InternalPauseMusic()
        {
            if (music != null && music.IsPlaying)
            {
                music.Pause();
            }
        }

        private void InternalResumeMusic()
        {
            if (music != null && !music.IsPlaying)
            {
                music.Resume();
            }
        }

        #endregion

        #region Handle

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

        #endregion

        #region Public APi

        public static SoundCache PlaySfx(SoundData soundData) => Instance.InternalPlaySfx(soundData);
        public static void StopSfx(SoundCache soundCache) => Instance.InternalStopSfx(soundCache);
        public static void PauseSfx(SoundCache soundCache) => Instance.InternalPauseSfx(soundCache);
        public static void ResumeSfx(SoundCache soundCache) => Instance.InternalResumeSfx(soundCache);
        public static void FinishSfx(SoundCache soundCache) => Instance.InternalFinishSfx(soundCache);
        public static void StopAllSfx() => Instance.InternalStopAllSfx();
        public static void PlayMusic(SoundData soundData) => Instance.InternalPlayMusic(soundData);
        public static void StopMusic() => Instance.InternalStopMusic();
        public static void PauseMusic() => Instance.InternalPauseMusic();
        public static void ResumeMusic() => Instance.InternalResumeMusic();

        public static float SfxVolume
        {
            get => Instance.InternalSfxVolume;
            set => Instance.InternalSfxVolume = value;
        }

        public static float MusicVolume
        {
            get => Instance.InternalMusicVolume;
            set => Instance.InternalMusicVolume = value;
        }

        #endregion
    }
}