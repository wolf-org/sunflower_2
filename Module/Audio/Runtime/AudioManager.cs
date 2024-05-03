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
        private List<SoundData> listAudioDatas = new List<SoundData>();
        private List<SoundComponent> listSoundComponents = new List<SoundComponent>();
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

        /// <summary>
        /// Play Sound FX
        /// </summary>
        /// <param name="soundData"></param>
        public void PlaySfx(SoundData soundData)
        {
            SoundComponent sfxComponent = PoolManager.Instance.Spawn(soundComponentPrefab, audioHolder);
            sfxComponent.PlayAudioClip(soundData.GetAudioClip(), soundData.loop, soundData.volume * SfxVolume);
            if (!soundData.loop) sfxComponent.OnCompleted += OnFinishPlayingAudio;
            listAudioDatas.Add(soundData);
            listSoundComponents.Add(sfxComponent);
        }

        /// <summary>
        /// Stop Sound FX
        /// </summary>
        /// <param name="soundData"></param>
        public void StopSfx(SoundData soundData)
        {
            var soundComponent = GetSoundComponent(soundData);
            if (soundComponent == null) return;
            StopAndCleanAudioComponent(soundComponent);
            if (listAudioDatas.Count > 0)
            {
                listSoundComponents.Remove(GetSoundComponent(soundData));
                listAudioDatas.Remove(soundData);
            }
        }

        /// <summary>
        /// Pause Sound FX
        /// </summary>
        /// <param name="soundData"></param>
        public void PauseSfx(SoundData soundData)
        {
            var soundComponent = GetSoundComponent(soundData);
            if (soundComponent == null || !soundComponent.IsPlaying) return;
            soundComponent.Pause();
        }

        /// <summary>
        /// Resume Sound FX
        /// </summary>
        /// <param name="soundData"></param>
        public void ResumeSfx(SoundData soundData)
        {
            var soundComponent = GetSoundComponent(soundData);
            if (soundComponent == null || soundComponent.IsPlaying) return;
            soundComponent.Resume();
        }

        /// <summary>
        /// Finish Sound FX
        /// </summary>
        /// <param name="soundData"></param>
        public void FinishSfx(SoundData soundData)
        {
            var soundComponent = GetSoundComponent(soundData);
            if (soundComponent == null || !soundComponent.IsPlaying) return;
            soundComponent.Finish();
            soundComponent.OnCompleted += OnFinishPlayingAudio;
        }

        /// <summary>
        /// Stop All Sound FX
        /// </summary>
        public void StopAllSfx()
        {
            foreach (var soundComponent in listSoundComponents)
            {
                StopAndCleanAudioComponent(soundComponent);
            }

            listSoundComponents.Clear();
            listAudioDatas.Clear();
        }

        #endregion

        #region Music Method

        /// <summary>
        /// Play Music
        /// </summary>
        /// <param name="soundData"></param>
        public void PlayMusic(SoundData soundData)
        {
            if (music == null || !music.IsPlaying)
            {
                music = PoolManager.Instance.Spawn(soundComponentPrefab, audioHolder);
            }

            music.FadePlayMusic(soundData.GetAudioClip(), soundData.loop, soundData.volume * MusicVolume,
                soundData.isMusicFadeVolume, soundData.fadeOutDuration, soundData.fadeInDuration);
            music.OnCompleted += StopAudioMusic;
        }

        /// <summary>
        /// Stop Music
        /// </summary>
        public void StopMusic()
        {
            if (music != null && music.IsPlaying)
            {
                music.Stop();
                PoolManager.Instance.DeSpawn(music.gameObject);
            }
        }

        /// <summary>
        /// Pause Music
        /// </summary>
        public void PauseMusic()
        {
            if (music != null && music.IsPlaying)
            {
                music.Pause();
            }
        }

        /// <summary>
        /// ResumeMusic
        /// </summary>
        public void ResumeMusic()
        {
            if (music != null && !music.IsPlaying)
            {
                music.Resume();
            }
        }

        #endregion

        /// <summary>
        /// Change volume for music
        /// </summary>
        /// <param name="volume"></param>
        private void OnMusicVolumeChanged(float volume)
        {
            if (music != null)
            {
                music.Volume = volume;
            }
        }

        /// <summary>
        /// Change volume for all sound fx
        /// </summary>
        /// <param name="volume"></param>
        private void OnSfxVolumeChanged(float volume)
        {
            foreach (var audio in listSoundComponents)
            {
                audio.Volume = volume;
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
            PoolManager.Instance.DeSpawn(soundComponent.gameObject);
        }

        void StopAudioMusic(SoundComponent soundComponent)
        {
            soundComponent.OnCompleted -= StopAudioMusic;
            PoolManager.Instance.DeSpawn(soundComponent.gameObject);
        }

        SoundComponent GetSoundComponent(SoundData soundData)
        {
            int index = listAudioDatas.FindIndex(x => x == soundData);
            if (index < 0)
            {
                return null;
            }

            return listSoundComponents[index];
        }
    }
}