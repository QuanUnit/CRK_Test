using QCore.Tools.Providers;
using Services.Audio;
using Services.Audio.Configs;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class AudioInstaller : MonoInstaller
    {
        [SerializeField] private AudioServiceConfig _audioServiceConfig;
        
        public override void InstallBindings()
        {
            DefaultDataProvider<AudioServiceConfig> audioServiceConfigProvider = new DefaultDataProvider<AudioServiceConfig>(_audioServiceConfig);
            
            Container.Bind<IDataProvider<AudioServiceConfig>>().FromInstance(audioServiceConfigProvider).AsSingle();
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
        }
    }
}