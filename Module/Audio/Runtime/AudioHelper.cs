namespace VirtueSky.Audio
{
    public static class AudioHelper
    {
        #region Sfx

        public static SoundCache PlaySfx(this SoundData soundData) => AudioManager.Instance.PlaySfx(soundData);
        public static void StopSfx(this SoundCache soundCache) => AudioManager.Instance.StopSfx(soundCache);
        public static void PauseSfx(this SoundCache soundCache) => AudioManager.Instance.PauseSfx(soundCache);
        public static void ResumeSfx(this SoundCache soundCache) => AudioManager.Instance.ResumeSfx(soundCache);
        public static void FinishSfx(this SoundCache soundCache) => AudioManager.Instance.FinishSfx(soundCache);
        public static void StopAllSfx() => AudioManager.Instance.StopAllSfx();

        #endregion

        #region Music

        public static void PlayMusic(this SoundData soundData) => AudioManager.Instance.PlayMusic(soundData);
        public static void StopMusic() => AudioManager.Instance.StopMusic();
        public static void PauseMusic() => AudioManager.Instance.PauseMusic();
        public static void ResumeMusic() => AudioManager.Instance.ResumeMusic();

        #endregion
    }
}