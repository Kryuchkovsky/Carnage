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

        private Vector3 _defaultRotation;
        private bool _isPlayed;
        
        [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }

        private void Awake()
        {
            _defaultRotation = transform.rotation.eulerAngles;
        }

        public void Initialize(Vector3 position, Quaternion rotation)
        {
            _isPlayed = false;
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(Vector3.up, _defaultRotation + rotation.eulerAngles);
            ParticleSystem.Play();
        }
        
        private void OnDisable() => OnPlayed();
        private void OnParticleSystemStopped() => OnPlayed();

        private void OnPlayed()
        {
            if (_isPlayed) return;
            
            _isPlayed = true;
            ParticleSystem.Stop();
            Played?.Invoke(this);
        }
    }
}