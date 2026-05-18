using UnityEngine;

namespace Gameplay
{
    public sealed class CombatProximityService
    {
        private readonly Transform _playerTransform;
        private readonly Collider _playerCollider;
        private readonly float _extraReach;

        public CombatProximityService(Transform playerTransform, Collider playerCollider, float extraReach = 0.05f)
        {
            _playerTransform = playerTransform;
            _playerCollider = playerCollider;
            _extraReach = extraReach;
        }

        public bool IsInRange(EntityIdentity target)
        {
            if (_playerTransform == null || target == null)
            {
                return false;
            }

            var playerPosition = _playerTransform.position;
            var targetPosition = target.transform.position;

            if (!target.TryGetComponent<Collider>(out var targetCollider) || targetCollider == null)
            {
                return Vector3.Distance(playerPosition, targetPosition) <= _extraReach;
            }

            var targetClosestPoint = targetCollider.ClosestPoint(playerPosition);
            var playerClosestPoint = _playerCollider != null
                ? _playerCollider.ClosestPoint(targetClosestPoint)
                : playerPosition;

            playerClosestPoint.y = 0f;
            targetClosestPoint.y = 0f;

            return (targetClosestPoint - playerClosestPoint).sqrMagnitude <= _extraReach * _extraReach;
        }
    }
}
