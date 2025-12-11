using _Logic.Core;
using _Logic.Gameplay.SurvivalMode.Components;
using _Logic.Gameplay.SurvivalMode.Session;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace _Logic.Gameplay.SurvivalMode.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SurvivalModeInitializationSystem : AbstractInitializationSystem
    {
        [Inject] private SessionService _sessionService;
        
        public override void OnAwake()
        {
            var entity = World.CreateEntity();
            World.GetStash<SurvivalModeComponent>().Set(entity, new SurvivalModeComponent
            {
                SessionData = _sessionService.GetData()
            });
        }
    }
}