using System.Collections.Generic;
using System.Linq;
using _Logic.Core;
using _Logic.Gameplay.SurvivalMode;
using _Logic.Gameplay.Units.Health.Events;
using _Logic.Gameplay.Units.Spawn;
using _Logic.Gameplay.Units.Team.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.FightMode.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class UnitsCountHandlingSystem : AbstractUpdateSystem
    {
        private Stash<TeamComponent> _teamStash;
        private Event<UnitSpawnEvent> _unitSpawnEvent;
        private Event<UnitDeathEvent> _unitDeathEvent;

        [Inject] private GameplayUIContainer _gameplayUIContainer;

        private Dictionary<int, int> _unitsCountInTeams = new();
        private bool _hasChanged;

        public override void OnAwake()
        {
            _teamStash = World.GetStash<TeamComponent>();
            _unitSpawnEvent = World.GetEvent<UnitSpawnEvent>();
            _unitDeathEvent = World.GetEvent<UnitDeathEvent>();
            _gameplayUIContainer.BattleStateView.SetData(0, 0);
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var e in _unitSpawnEvent.publishedChanges)
            {
                var teamComponent = _teamStash.Get(e.Entity, out var exist);

                if (exist)
                {
                    if (!_unitsCountInTeams.TryAdd(teamComponent.Id, 1))
                        _unitsCountInTeams[teamComponent.Id] += 1;

                    _hasChanged = true;
                }
            }
            
            foreach (var e in _unitDeathEvent.publishedChanges)
            {
                var teamComponent = _teamStash.Get(e.CorpseEntity, out var exist);

                if (exist && _unitsCountInTeams.ContainsKey(teamComponent.Id))
                {
                    var count = Mathf.Max(0, _unitsCountInTeams[teamComponent.Id] - 1);
                    _unitsCountInTeams[teamComponent.Id] = count;
                    _hasChanged = true;
                }
            }

            UpdateView();
        }

        private void UpdateView()
        {
            if (_hasChanged is false)
                return;

            var values = _unitsCountInTeams.Values;
            _gameplayUIContainer.BattleStateView.SetData(values.ElementAtOrDefault(0), values.ElementAtOrDefault(1));
            _hasChanged = false;
        }
    }
}