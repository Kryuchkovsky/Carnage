using UnityEngine;

namespace _Logic.Gameplay.Units.Attack
{
    public class RandomAttackStateMachineBehavior : StateMachineBehaviour
    {
        private readonly string _attackVarName = "AttackVar";

        [SerializeField, Min(1)] private int _attackCount = 1;
        
        private int _hash;
        
        private void Awake()
        {
            _hash = Animator.StringToHash(_attackVarName);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            var attackVar = Random.Range(0, _attackCount);
            animator.SetInteger(_hash, attackVar);
        }
    }
}