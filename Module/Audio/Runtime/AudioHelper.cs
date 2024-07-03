namespace VirtueSky.Audio
{
    public static class AudioHelper
    {
        public static SoundCache PlaySfx(this SoundData soundData) => AudioManager.PlaySfx(soundData);
        public static void StopSfx(this SoundCache soundCache) => AudioManager.StopSfx(soundCache);
        public static void PauseSfx(this SoundCache soundCache) => AudioManager.PauseSfx(soundCache);
        public static void ResumeSfx(this SoundCache soundCache) => AudioManager.ResumeSfx(soundCache);
        public static void FinishSfx(this SoundCache soundCache) => AudioManager.FinishSfx(soundCache);
        public static void PlayMusic(this SoundData soundData) => AudioManager.PlayMusic(soundData);
    }
}