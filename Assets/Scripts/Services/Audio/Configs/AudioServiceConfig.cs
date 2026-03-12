using UnityEngine;

namespace Services.Audio.Configs
{
    [CreateAssetMenu(menuName = "Configs/Sounds/" + nameof(AudioServiceConfig), fileName = nameof(AudioServiceConfig))]
    public class AudioServiceConfig : ScriptableObject
    {
        public AudioSourceWrapper AudioSourceWrapperPrefab => _audioSourceWrapperPrefab;
        public int PersistentPoolSize => _persistentPoolSize;
        
        [SerializeField] private AudioSourceWrapper _audioSourceWrapperPrefab;
        [SerializeField] private int _persistentPoolSize;
    }
}