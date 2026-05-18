using Core;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerCombat combat;
        [SerializeField] private PlayerPower power;
        [SerializeField] private Animator animator;

        public static PlayerController Instance;

        public int Power => power.Power;

        private ITargetable _currentTarget;

        private void Awake()
        {
            Instance = this;
        }

        public void MoveToTarget()
        {
            
        }

        public void Attack()
        {
            
        }

        public void SetTarget(ITargetable target)
        {
            _currentTarget = target;

            GameManager.Instance.ChangeState(GameState.MovingToTarget);

            var position = target.GetTransform().position;
            var stopDistance = target.GetInteractionDistance();
            
            movement.SetStopDistance(stopDistance);
            movement.MoveTo(position, OnTargetReached);
        }

        private void OnTargetReached()
        {
            GameManager.Instance.ChangeState(GameState.Interacting);

            combat.Attack(_currentTarget);
        }

        private void ReachTarget()
        {
            GameManager.Instance.ChangeState(GameState.Interacting);

            combat.Attack(_currentTarget);
        }

        public void PlayFailFeedback()
        {
            
        }

        public void AddPower(int value)
        {
            throw new System.NotImplementedException();
        }
    }
}