using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using _Logic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Extensions.VFXManager
{
    public class EffectCreator : SingletonBehavior<EffectCreator>
    {
        private Dictionary<string, ObjectPool<Effect>> _effectsPools;

        [SerializeField] private EffectsCatalog _effectsCatalog;
        [SerializeField, Range(0, 1024)] private int _initialPoolsCapacity;
        [SerializeField] private bool _autoFillingIsEnabled;

        protected override void Init()
        {
            base.Init();
            
            _effectsPools = new Dictionary<string, ObjectPool<Effect>>();
            
            foreach (var effect in _effectsCatalog.Effects)
            {
                _effectsPools.Add(
                    effect.Id, 
                    new ObjectPool<Effect>(effect._effect, _initialPoolsCapacity, _autoFillingIsEnabled, transform,
                        e =>
                        {
                            e.Played += () => _effectsPools[effect.Id].Return(e);
                        },
                        e =>
                        {
                            e.transform.parent = transform;
                            e.ParticleSystem.Play();
                        },
                        e =>
                        {
                            e.ParticleSystem.Stop();
                        }));
            }
        }

        public void CreateEffect(string id, Vector3 position, Quaternion rotation = new())
        {
            var effect = _effectsPools[id].Take();
            effect.transform.position = position;
            effect.transform.rotation = rotation;
            effect.ParticleSystem.Play();
        }
    }
}