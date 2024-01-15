using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace _Logic.Gameplay.Units.Attack.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AttackComponent : IComponent
    {
        public AttackData BacisData;
        public AttackData CurrentData;
        public float AttacksPerSecond;
        public float AttackTime;
        public float AttackTimePercentage;
        public float RemainingAttackTime;
    }
}