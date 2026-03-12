using QCore.Tools.Extensions;
using QCore.Tools.GameObjectsPool;
using QCore.Tools.Providers;
using Services.Audio.Configs;
using UnityEngine;
using Zenject;

namespace Services.Audio
{
    public class AudioService : IAudioService
    {
        private AudioServiceConfig Config => _configProvider.Provide();
        
        [Inject] private IDataProvider<AudioServiceConfig> _configProvider;

        private Transform _root;
        private GameObjectPool<AudioSourceWrapper> _pool;
        
        public void Init()
        {
            GameObject root = new GameObject("Audio");
            Object.DontDestroyOnLoad(root);
            _pool = new GameObjectPool<AudioSourceWrapper>(Config.AudioSourceWrapperPrefab, Config.PersistentPoolSize, InstantiateAudioSourceWrapper);
            _pool.InitRoot(root.transform);
            _pool.WarmUp();
        }
        
        public void Play(AudioClipConfig audioClipConfig)
        {
            AudioSourceWrapper audioSourceWrapper = _pool.Get(_root);
            audioSourceWrapper.Init(audioClipConfig);
            audioSourceWrapper.Play(() => _pool.Release(audioSourceWrapper));
        }

        private AudioSourceWrapper InstantiateAudioSourceWrapper(AudioSourceWrapper prefab, Transform root)
        {
            return Object.Instantiate(prefab, root);
        }
        
        public void Dispose()
        {
            _pool.ReleaseAllElements();

            if (_root.IsNullOrDestroyed() == false)
            {
                Object.Destroy(_root.gameObject);
            }
        }
    }
}