﻿using System.Collections.Generic;
using _Logic.Extensions.Patterns;
using UnityEngine;

namespace _Logic.Extensions.HealthBar
{
    public class HealthBarsService : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Canvas _canvas;
        
        private ObjectPool<HealthBarView> _healthBarsPool;
        private List<HealthBarView> _healthBarViews;
        private HealthBarSettings _settings;

        public void Initialize(HealthBarSettings settings)
        {
            _camera ??= Camera.main;
            _settings = settings;
            _healthBarsPool = new(_settings.HealthBar, 16, true, _canvas.transform,
                takeAction: v => _healthBarViews.Add(v),
                returnAction: v =>
                {
                    v.Reset();
                    _healthBarViews.Remove(v);
                });
            _healthBarViews = new List<HealthBarView>(1024);
        }

        private void LateUpdate()
        {
            for (int i = 0; i < _healthBarViews.Count;)
            {
                _healthBarViews[i].Update();
                
                if (_healthBarViews[i].IsInitiated)
                {
                    i++;
                }
                else
                {
                    RemoveHealthBar(_healthBarViews[i]);
                }
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