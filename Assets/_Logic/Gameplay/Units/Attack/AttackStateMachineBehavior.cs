using System;
using UnityEngine;

namespace _Logic.Gameplay.Units.Attack
{
    public class AttackStateMachineBehavior : StateMachineBehaviour
    {
        public event Action AttackCompleted;
        
        [SerializeField, Range(0, 1)] private float _attackEventTiming = 0.5f;

        private bool _attackEventIsSent;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            _attackEventIsSent = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (!_attackEventIsSent && stateInfo.normalizedTime >= _attackEventTiming)
            {
                AttackCompleted?.Invoke();
                _attackEventIsSent = true;
            }
        }
    }
}