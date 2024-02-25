using System.Collections.Generic;
using _GameLogic.Extensions.Patterns;
using _Logic.Extensions.Configs;
using _Logic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Extensions.HealthBar
{
    public class HealthBarsService : SingletonBehavior<HealthBarsService>
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Canvas _canvas;
        
        private ObjectPool<HealthBarView> _healthBarsPool;
        private HashSet<HealthBarView> _healthBarViews;
        private HealthBarSettings _settings;

        protected override void Init()
        {
            base.Init();
            _camera ??= Camera.main;
            _settings = ConfigsManager.GetConfig<HealthBarSettings>();
            _healthBarsPool = new(_settings.HealthBar, 16, true, _canvas.transform,
                returnAction: v => v.Reset());
            _healthBarViews = new HashSet<HealthBarView>(1024);
        }

        private void LateUpdate()
        {
            foreach (var view in _healthBarViews)
            {
                view.Update();
            }
        }

        public HealthBarView CreateHealthBar(Transform target, float offsetY, bool isAlly)
        {
            var healthBar = _healthBarsPool.Take();
            var data = isAlly ? _settings.AlliedHealthBarColorData : _settings.EnemyTeamHealthBarColorData;
            healthBar.Initiate(_camera, data, target, offsetY, _settings.HidingDelay);
            _healthBarViews.Add(healthBar);
            return healthBar;
        }

        public void RemoveHealthBar(HealthBarView healthBarView)
        {
            _healthBarViews.Remove(healthBarView);
            _healthBarsPool.Return(healthBarView);
        }
    }
}