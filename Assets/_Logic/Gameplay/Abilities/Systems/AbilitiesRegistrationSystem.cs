using System;
using _Logic.Core;
using _Logic.Gameplay.Abilities.Components;
using _Logic.Gameplay.Effects;
using _Logic.Gameplay.SelectionPanel;
using _Logic.Gameplay.Units.Attack.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;
using VContainer;

namespace _Logic.Gameplay.Abilities.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AbilitiesRegistrationSystem : AbstractInitializationSystem
    {
        [Inject] private ImpactCatalog _impactCatalog;
        
        public override void OnAwake()
        {
            var entity = World.CreateEntity();
            var abilitySelections = new FastList<SelectionData>();
            abilitySelections.Add(new SelectionData("Add fragmentation attack", e =>
            {
                if (e.Has<FragmentationAttackComponent>())
                {
                    ref var component = ref e.GetComponent<FragmentationAttackComponent>();
                    component.Fragments += 1;
                }
                else
                {
                    e.SetComponent(new FragmentationAttackComponent
                    {
                        Fragments = 1
                    });
                }
            }));
            abilitySelections.Add(new SelectionData("Add rebound attack", e =>
            {
                if (e.Has<ReboundAttackComponent>())
                {
                    ref var component = ref e.GetComponent<ReboundAttackComponent>();
                    component.Rebounds += 1;
                }
                else
                {
                    e.SetComponent(new ReboundAttackComponent
                    {
                        Rebounds = 1
                    });
                }
            }));
            abilitySelections.Add(new SelectionData("Add split attack", e =>
            {
                if (e.Has<SplitAttackComponent>())
                {
                    ref var component = ref e.GetComponent<SplitAttackComponent>();
                    component.AdditionalTargets += 1;
                }
                else
                {
                    e.SetComponent(new SplitAttackComponent
                    {
                        AdditionalTargets = 1
                    });
                }
            }));

            foreach (int typeIndex in (ImpactType[]) Enum.GetValues(typeof(ImpactType)))
            {
                if (typeIndex == 0) continue;
                
                var data = _impactCatalog.GetData(typeIndex);
                abilitySelections.Add(new SelectionData(data.Description, e =>
                {
                    if (e.Has<AttackComponent>())
                    {
                        ref var component = ref e.GetComponent<AttackComponent>();
                        component.ImpactTypes.Add(data.Type);
                    }
                }));
            }
            
            entity.SetComponent(new AbilitySelectionsComponent
            {
                Value = abilitySelections
            });
        }
    }
}