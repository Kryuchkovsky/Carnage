using _GameLogic.Extensions.Patterns;
using _Logic.Extensions.Patterns;
using _Logic.Extensions.Popup;
using UnityEngine;

namespace _Logic.Extensions.HealthBar
{
    public class HealthBarCreationService : SingletonBehavior<HealthBarCreationService>
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private HealthBarView _healthBar;
        [SerializeField] private HealthBarSettings _settings;
        
        private ObjectPool<HealthBarView> _healthBarsPool;

        protected override void Init()
        {
            base.Init();
            _camera ??= Camera.main;
            _healthBarsPool = new(_healthBar, 16, true, 
                returnAction: v => v.Reset());
        }

        public HealthBarView CreateHealthBar(Transform target, float offsetY, bool isAlly)
        {
            var healthBar = _healthBarsPool.Take();
            healthBar.Initiate(_camera, isAlly ? _settings.AlliedHealthBarColorData : _settings.EnemyTeamHealthBarColorData, target, offsetY);
            return healthBar;
        }

        public void RemoveHealthBar(HealthBarView healthBarView)
        {
            _healthBarsPool.Return(healthBarView);
        }
    }
}