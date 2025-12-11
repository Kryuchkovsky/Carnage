using _Logic.Gameplay.Units.Stats;

namespace _Logic.Gameplay.Equipment
{
    public interface IEquipmentStatBuff : IStatBuff
    {
        float IStatBuff.Duration => -1;
        bool IStatBuff.IsPersist => true;
    }
}