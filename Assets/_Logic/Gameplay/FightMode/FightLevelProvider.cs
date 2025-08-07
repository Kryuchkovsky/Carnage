using System.Collections.Generic;
using _Logic.Gameplay.FightMode.Components;
using _Logic.Gameplay.Levels;
using Scellecs.Morpeh;
using UnityEngine;

namespace _Logic.Gameplay.FightMode
{
    public class FightLevelProvider : LevelProvider
    {
        [SerializeField] private List<Transform> _teamSpawnPoints = new();

        protected override void Initialize()
        {
            base.Initialize();
            Entity.SetComponent(new FightLevelComponent
            {
                Value = this
            });
        }

        public bool TryGetSpawnPosition(int index, out Vector3 position)
        {
            position = transform.position;

            if (index >= 0 && index < _teamSpawnPoints.Count)
            {
                position = _teamSpawnPoints[index].position;
                return true;
            }

            return false;
        }
    }
}