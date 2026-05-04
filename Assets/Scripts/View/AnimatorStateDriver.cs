using UnityEngine;

namespace View
{
    public sealed class AnimatorStateDriver : ICharacterAnimationDriver
    {
        private readonly Animator _animator;
        private readonly string _idleStateName;
        private readonly string _moveStateName;
        private readonly int _movingBoolHash;
        private readonly bool _useMovingBool;

        private bool? _isMoving;

        public AnimatorStateDriver(
            Animator animator,
            string idleStateName,
            string moveStateName,
            string movingBoolParameter)
        {
            _animator = animator;
            _idleStateName = string.IsNullOrWhiteSpace(idleStateName) ? null : idleStateName;
            _moveStateName = string.IsNullOrWhiteSpace(moveStateName) ? null : moveStateName;
            _useMovingBool = !string.IsNullOrWhiteSpace(movingBoolParameter);
            _movingBoolHash = _useMovingBool ? Animator.StringToHash(movingBoolParameter) : 0;
        }

        public void SetMoving(bool isMoving)
        {
            if (_animator == null || _isMoving == isMoving)
            {
                return;
            }

            _isMoving = isMoving;

            if (_useMovingBool)
            {
                _animator.SetBool(_movingBoolHash, isMoving);
            }

            var stateName = isMoving ? _moveStateName : _idleStateName;
            if (!string.IsNullOrWhiteSpace(stateName))
            {
                _animator.CrossFade(stateName, 0.1f, 0, 0f);
            }
        }

        public void Update(float deltaTime)
        {
            if (_animator == null || _isMoving == null)
            {
                return;
            }

            var currentState = _animator.GetCurrentAnimatorStateInfo(0);
            if (_animator.IsInTransition(0))
            {
                return;
            }

            var expectedStateName = _isMoving.Value ? _moveStateName : _idleStateName;
            if (string.IsNullOrWhiteSpace(expectedStateName) || !IsCurrentState(currentState, expectedStateName))
            {
                return;
            }

            if (currentState.loop)
            {
                return;
            }

            if (currentState.normalizedTime < 0.999f)
            {
                return;
            }

            _animator.Play(expectedStateName, 0, 0f);
        }

        public void Dispose()
        {
        }

        private static bool IsCurrentState(AnimatorStateInfo stateInfo, string stateName)
        {
            return stateInfo.IsName(stateName) || stateInfo.IsName($"Base Layer.{stateName}");
        }
    }
}
