using System.Collections.Generic;
using System.Linq;
using _Logic.Core;
using _Logic.Gameplay.Items.Components;
using _Logic.Gameplay.Units;
using _Logic.Gameplay.Units.Components;
using _Logic.Gameplay.Units.Stats;
using _Logic.Gameplay.Units.Stats.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using VContainer;

namespace _Logic.Gameplay.Items.Equipment.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class EquipmentSetRequestProcessingSystem : AbstractUpdateSystem
    {
        private Stash<UnitComponent> _unitStash;
        private Stash<EquipmentsComponent> _equipmentsStash;
        private Stash<StatsComponent> _statsStash;
        private Request<EquipmentSetRequest> _equipmentSetRequest;

        [Inject] private EquipmentCatalog _equipmentCatalog;
        
        public override void OnAwake()
        {
            _equipmentSetRequest = World.GetRequest<EquipmentSetRequest>();
            _unitStash = World.GetStash<UnitComponent>();
            _equipmentsStash = World.GetStash<EquipmentsComponent>();
            _statsStash = World.GetStash<StatsComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var request in _equipmentSetRequest.Consume())
            {
                if (!_unitStash.Has(request.Entity) || !_statsStash.Has(request.Entity))
                    return;

                ref var unitComponent = ref _unitStash.Get(request.Entity);
                ref var statsComponent = ref _statsStash.Get(request.Entity);
                var stats = statsComponent.Value;
                
                var dressingEquipment = _equipmentCatalog.GetById(request.EquipmentId);
                ref var equipmentsComponent = ref _equipmentsStash.Get(request.Entity, out var hasEquipmentsComponent);
                
                var dressedEquipment = hasEquipmentsComponent 
                    ? equipmentsComponent.Dictionary 
                    : new Dictionary<SlotType, EquipmentData>();

                if (!hasEquipmentsComponent)
                {
                    _equipmentsStash.Set(request.Entity, new EquipmentsComponent
                    {
                        Dictionary = dressedEquipment
                    });
                }
                
                DressEquipment(dressedEquipment, dressingEquipment, unitComponent.Value, stats);
            }
        }

        private void DressEquipment(Dictionary<SlotType, EquipmentData> dressedEquipment, EquipmentData dressingEquipment, UnitProvider unit, StatStorage stats)
        {
            if (dressingEquipment.SetSlotsPreset && dressingEquipment.SlotsPreset is not null && dressingEquipment.SlotsPreset.PossibleSlots.Count > 0)
            {
                var slotType = SlotType.None;
                
                foreach (var possibleSlot in dressingEquipment.SlotsPreset.PossibleSlots)
                {
                    if (possibleSlot != SlotType.None && !dressedEquipment.ContainsKey(possibleSlot))
                    {
                        slotType = possibleSlot;
                        break;
                    }
                }

                if (slotType == SlotType.None)
                    slotType = dressingEquipment.SlotsPreset.PossibleSlots.First(s => s != SlotType.None);

                FreeSlot(dressedEquipment, slotType, unit, stats);
                FreeSlots(dressedEquipment, dressingEquipment.SlotsPreset.BlockingSlots, unit, stats);
                DressEquipment(dressedEquipment, dressingEquipment, slotType, unit, stats);
            }
            else
            {
                FreeSlot(dressedEquipment, dressingEquipment.SlotType, unit, stats);
                DressEquipment(dressedEquipment, dressingEquipment, dressingEquipment.SlotType, unit, stats);
            }
        }
        
        private void DressEquipment(Dictionary<SlotType, EquipmentData> dressedEquipment, EquipmentData dressingEquipment, SlotType slotType, UnitProvider unit, StatStorage stats)
        {
            if (slotType != SlotType.None && dressedEquipment.TryAdd(slotType, dressingEquipment))
            {
                stats.AddBuff(dressingEquipment);
                SetEquipmentToView(dressingEquipment, slotType, unit);
            }
        }

        private void FreeSlots(Dictionary<SlotType, EquipmentData> dressedEquipment, List<SlotType> freeSlots, UnitProvider unit, StatStorage stats)
        {
            foreach (var slot in freeSlots)
                FreeSlot(dressedEquipment, slot, unit, stats);
        }
        
        private void FreeSlot(Dictionary<SlotType, EquipmentData> dressedEquipment, SlotType freeSlot, UnitProvider unit, StatStorage stats)
        {
            if (dressedEquipment.TryGetValue(freeSlot, out var equipment))
            {
                stats.RemoveBuff(equipment);
                dressedEquipment.Remove(freeSlot);
            }
        }

        private void SetEquipmentToView(EquipmentData equipmentData, SlotType slotType, UnitProvider unit)
        {
            EquipmentProvider equipmentProvider = null;

            if (unit.Model.GetEquipment(slotType, out equipmentProvider))
                Object.Destroy(equipmentProvider.gameObject);

            equipmentProvider = Object.Instantiate(equipmentData.Prefab);
            unit.Model.SetEquipment(slotType, equipmentProvider);
        }
    }
}