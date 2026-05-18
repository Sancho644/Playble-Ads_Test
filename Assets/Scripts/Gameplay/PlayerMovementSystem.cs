using UnityEngine;

namespace Gameplay
{
    public sealed class PlayerMovementSystem
    {
        private readonly Transform _playerTransform;
        private readonly MoveTargetComponent _moveTarget;
        private readonly MoveSpeedComponent _moveSpeed;
        private readonly MovementVisualStateComponent _movementVisualState;
        private readonly float _stopDistanceSqr;

        public PlayerMovementSystem(
            Transform playerTransform,
            MoveTargetComponent moveTarget,
            MoveSpeedComponent moveSpeed,
            MovementVisualStateComponent movementVisualState,
            float stopDistance = 0.05f)
        {
            _playerTransform = playerTransform;
            _moveTarget = moveTarget;
            _moveSpeed = moveSpeed;
            _movementVisualState = movementVisualState;
            _stopDistanceSqr = stopDistance * stopDistance;
        }

        public void Tick(float deltaTime)
        {
            if (_playerTransform == null || _moveTarget == null || _moveSpeed == null || !_moveTarget.HasDestination)
            {
                _movementVisualState?.Stop();
                return;
            }

            var currentPosition = _playerTransform.position;
            var destination = _moveTarget.Destination;
            var currentPositionOnPlane = new Vector3(currentPosition.x, destination.y, currentPosition.z);
            var toDestination = destination - currentPositionOnPlane;
            var toDestinationOnPlane = new Vector2(toDestination.x, toDestination.z);

            if (toDestinationOnPlane.sqrMagnitude <= _stopDistanceSqr)
            {
                _playerTransform.position = new Vector3(destination.x, currentPosition.y, destination.z);
                _moveTarget.Clear();
                _movementVisualState?.Stop();
                return;
            }

            _movementVisualState?.SetMoving(new Vector3(toDestination.x, 0f, toDestination.z));

            var step = _moveSpeed.UnitsPerSecond * deltaTime;
            var nextPosition = Vector3.MoveTowards(currentPositionOnPlane, destination, step);
            _playerTransform.position = new Vector3(nextPosition.x, currentPosition.y, nextPosition.z);
        }
    }
}
