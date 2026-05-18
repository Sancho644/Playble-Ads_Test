using Gameplay;
using UnityEngine;

namespace View
{
    public sealed class PlayerMovementVisualPresenter
    {
        private readonly MovementVisualStateComponent _movementVisualState;
        private readonly Transform _visualRoot;
        private readonly ICharacterAnimationDriver _animationDriver;
        private readonly float _rotationSpeed;
        private readonly float _yawOffset;

        public PlayerMovementVisualPresenter(
            MovementVisualStateComponent movementVisualState,
            Transform visualRoot,
            ICharacterAnimationDriver animationDriver,
            float rotationSpeed,
            float yawOffset)
        {
            _movementVisualState = movementVisualState;
            _visualRoot = visualRoot;
            _animationDriver = animationDriver;
            _rotationSpeed = rotationSpeed;
            _yawOffset = yawOffset;
        }

        public void Refresh(float deltaTime)
        {
            if (_movementVisualState == null || _visualRoot == null)
            {
                return;
            }

            _animationDriver?.SetMoving(_movementVisualState.IsMoving);
            _animationDriver?.Update(deltaTime);

            var facingDirection = _movementVisualState.FacingDirection;
            if (facingDirection.sqrMagnitude <= 0.0001f)
            {
                return;
            }

            var targetRotation = Quaternion.LookRotation(facingDirection, Vector3.up) * Quaternion.Euler(0f, _yawOffset, 0f);
            _visualRoot.rotation = Quaternion.RotateTowards(_visualRoot.rotation, targetRotation, _rotationSpeed * deltaTime);
        }
    }
}
