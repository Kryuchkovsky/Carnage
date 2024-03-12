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
            _settings = ConfigManager.GetConfig<HealthBarSettings>();
            _healthBarsPool = new(_settings.HealthBar, 16, true, _canvas.transform,
                takeAction: v => _healthBarViews.Add(v),
                returnAction: v =>
                {
                    v.Reset();
                    _healthBarViews.Remove(v);
                });
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
            return healthBar;
        }

        public void RemoveHealthBar(HealthBarView healthBarView) => _healthBarsPool.Return(healthBarView);
    }
}