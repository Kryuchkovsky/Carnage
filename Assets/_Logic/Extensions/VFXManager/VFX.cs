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
        private float _duration;
        private float _timer;
        private bool _isPlayed;
        
        [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }

        private void Awake()
        {
            _defaultRotation = transform.rotation.eulerAngles;
        }

        public void Initialize(Vector3 position, Quaternion rotation, float duration = -1)
        {
            _isPlayed = false;
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(Vector3.up, _defaultRotation + rotation.eulerAngles);
            _duration = duration;
            _timer = duration;
            ParticleSystem.Play();
        }

        private void Update()
        {
            if (_isPlayed || _duration <= 0) return;
            
            _timer -= Time.deltaTime;
            
            if (_timer <= 0)
            {
                OnPlayed();
            }
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