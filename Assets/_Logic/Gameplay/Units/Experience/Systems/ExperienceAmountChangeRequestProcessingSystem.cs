using _Logic.Core;
using _Logic.Core.Components;
using _Logic.Extensions.Popup;
using _Logic.Gameplay.Units.Experience.Components;
using _Logic.Gameplay.Units.Experience.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Units.Experience.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ExperienceAmountChangeRequestProcessingSystem : AbstractUpdateSystem
    {
        private Request<ExperienceAmountChangeRequest> _experienceAmountChangeRequest;
        private Request<LevelChangeRequest> _levelChangeRequest;
        
        [Inject] private ExperienceSettings _experienceSettings;
        [Inject] private PopupsService _popupsService;

        private readonly string _popupFormat = "+ {0:0} exp.";
        private Color _popupColor = new(0.63f, 0.42f, 0.18f);
        private int _popupIndex;
        
        public override void OnAwake()
        {
            _experienceAmountChangeRequest = World.GetRequest<ExperienceAmountChangeRequest>();
            _levelChangeRequest = World.GetRequest<LevelChangeRequest>();
            _popupIndex = _popupsService.RegisterPopupAndGetId(_popupFormat);
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _experienceAmountChangeRequest.Consume())
            {
                if (request.ReceivingEntity.IsNullOrDisposed() || !request.ReceivingEntity.Has<ExperienceComponent>()) continue;
                
                ref var expComponent = ref request.ReceivingEntity.GetComponent<ExperienceComponent>();
                expComponent.TotalExperienceAmount += request.Change;
                expComponent.Progress = (expComponent.TotalExperienceAmount - expComponent.CurrentLevelCost) / expComponent.LevelUpCost;
                var newLevelCost = expComponent.NextLevelCost;
                var levelChange = 0;
                
                ref var transformComponent = ref request.ReceivingEntity.GetComponent<TransformComponent>(out var hasTransformComponent);

                if (hasTransformComponent)
                {
                    _popupsService.CreateTextPopup(_popupIndex, transformComponent.Value, request.Change, _popupColor);
                }

                while (expComponent.TotalExperienceAmount > newLevelCost)
                {
                    levelChange += 1;
                    newLevelCost += _experienceSettings.CalculateLevelCost(expComponent.Level + levelChange);
                }

                if (levelChange != 0)
                {
                    _levelChangeRequest.Publish(new LevelChangeRequest
                    {
                        Entity = request.ReceivingEntity,
                        Change = levelChange
                    });
                }
            }
        }
    }
}