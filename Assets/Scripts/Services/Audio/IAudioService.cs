using Services.Audio.Configs;

namespace Services.Audio
{
    public interface IAudioService
    {
        public void Init();
        public void Play(AudioClipConfig audioClipConfig);
        public void Dispose();
    }
}