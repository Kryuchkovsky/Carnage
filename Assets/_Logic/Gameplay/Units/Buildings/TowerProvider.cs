using System;
using _Logic.Gameplay.Components;
using _Logic.Gameplay.Units.AI.Components;
using _Logic.Gameplay.Units.Attack.Components;
using _Logic.Gameplay.Units.Buildings.Components;
using _Logic.Gameplay.Units.Health.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.Units.Buildings
{
    public class TowerProvider : UnitProvider
    {
        [SerializeField] private TowerData _towerData;
        [SerializeField] private int _teamId;

        protected override void Initialize()
        {
            base.Initialize();
            Entity.AddComponent<AIComponent>();
            Entity.AddComponent<RangeAttackComponent>();
            Entity.SetComponent(new TowerComponent
            {
                Value = this
            });
            Entity.SetComponent(new AttackComponent
            {
                BacisData = _towerData.AttackData,
                CurrentData = _towerData.AttackData
            });
            Entity.SetComponent(new HealthComponent
            {
                BasicData = _towerData.HealthData,
                CurrentData = _towerData.HealthData,
                Value = _towerData.HealthData.MaxValue
            });
            Entity.SetComponent(new TeamIdComponent
            {
                Value = _teamId
            });
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, Entity.GetComponent<AttackComponent>().CurrentData.Range);
        }
    }
}