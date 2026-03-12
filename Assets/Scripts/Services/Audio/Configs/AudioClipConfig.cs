using UnityEngine;

namespace Services.Audio.Configs
{
    [CreateAssetMenu(menuName = "Configs/Sounds/" + nameof(AudioClipConfig), fileName = nameof(AudioClipConfig))]
    public class AudioClipConfig : ScriptableObject
    {
        public AudioClip Clip => _clip;
        public float Volume => _volume;
        public float Pitch => _pitch;
        
        [SerializeField] private AudioClip _clip;
        [SerializeField][Range(0f, 1f)] private float _volume = 1;
        [SerializeField][Range(-3f, 3f)] private float _pitch = 1;
    }
}