using ExternalDependencies.Zenject.Source.Util;
using Services.Audio.Configs;
using UnityEngine;

namespace Services.Audio.Components
{
    public abstract class AudioPlayerMonoComponent : MonoBehaviour
    {
        [SerializeField] private AudioClipConfig _audioClipConfig;

        private IAudioService _audioService;

        protected virtual void Awake()
        {
            _audioService = DiContextScope.ContextContainer.Resolve<IAudioService>();
        }
        
        public void Play()
        {
            _audioService.Play(_audioClipConfig);
        }
    }
}