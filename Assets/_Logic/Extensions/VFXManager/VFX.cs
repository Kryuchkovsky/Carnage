using System;
using UnityEngine;

namespace _Logic.Extensions.VFXManager
{
    /// <summary>
    ///   <para>Stop Action must be Callback, otherwise effect won't return to pool.</para>
    /// </summary>
    public class VFX : MonoBehaviour
    {
        public event Action<VFX> Played;

        private bool _isPlayed;
        
        [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }
        
        public void Initialize(float duration)
        {
            _isPlayed = false;
            ParticleSystem.Play();
        }
        
        private void OnDisable()
        {
            OnPlayed();
        }

        private void OnParticleSystemStopped()
        {
            OnPlayed();
        }

        private void OnPlayed()
        {
            if (_isPlayed) return;
            
            _isPlayed = true;
            ParticleSystem.Stop();
            Played?.Invoke(this);
        }
    }
}