using System;
using UnityEngine;

namespace _Logic.Extensions.VFXManager
{
    /// <summary>
    ///   <para>Stop Action must be Callback, otherwise effect won't return to pool.</para>
    /// </summary>
    public class VFX : MonoBehaviour
    {
        public event Action Played;

        [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }
        
        private void OnParticleSystemStopped()
        {
            Played?.Invoke();
        }
    }
}