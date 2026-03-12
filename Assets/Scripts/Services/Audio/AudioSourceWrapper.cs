using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Services.Audio.Configs;
using UnityEngine;

namespace Services.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceWrapper : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private CancellationTokenSource _cts;

        public void Init(AudioClipConfig audioClipConfig)
        {
            _audioSource.clip = audioClipConfig.Clip;
            _audioSource.volume = audioClipConfig.Volume;
            _audioSource.pitch = audioClipConfig.Pitch;
        }

        public async void Play(Action finished = null)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
        
            _audioSource.Stop();
            _audioSource.Play();

            try
            {
                await UniTask.WaitWhile(() => _audioSource.isPlaying,
                    cancellationToken: _cts.Token);
            }
            finally
            {
                finished?.Invoke();
            }
        }
    
        public void Stop()
        {
            _cts?.Cancel();
            _audioSource.Stop();
        }
    
        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    
#if UNITY_EDITOR
        private void OnValidate()
        {
            _audioSource ??= GetComponent<AudioSource>();
        }
#endif
    }
}