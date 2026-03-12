using QCore.Tools.GameObjectsPool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Hub.ClickerPanel
{
    public class ClickerParticlesThrower : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private ResourceParticle _resourceParticle;
        [SerializeField] private int _poolPersistentSize = 10;

        private GameObjectPool<ResourceParticle> _pool;
        
        private void Awake()
        {
            _pool = new GameObjectPool<ResourceParticle>(_resourceParticle, _poolPersistentSize, InstantiateParticle);
            _pool.InitRoot(transform);
            _pool.WarmUp();
        }

        public void Throw(int amount)
        {
            Vector3 position = GetRandomWorldPointInRectTransform(_rect);
            ResourceParticle particle = _pool.Get(position, Quaternion.identity, transform);
            particle.SetAmount(amount);
            particle.Play(() => _pool.Release(particle));
        }
        
        private Vector3 GetRandomWorldPointInRectTransform(RectTransform rectTransform)
        {
            Rect rect = rectTransform.rect;
    
            float randomX = Random.Range(rect.xMin, rect.xMax);
            float randomY = Random.Range(rect.yMin, rect.yMax);
            Vector3 localPoint = new Vector3(randomX, randomY, 0);
    
            return rectTransform.TransformPoint(localPoint);
        }
        
        private ResourceParticle InstantiateParticle(ResourceParticle prefab, Transform root)
        {
            return Instantiate(prefab, root);
        }

        public void Clear()
        {
            _pool.ReleaseAllElements();
        }
    }
}