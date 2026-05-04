using UnityEngine;

namespace Gameplay
{
    public sealed class ApproachPositionService
    {
        private readonly Transform _playerTransform;
        private readonly Collider _playerCollider;
        private readonly float _extraOffset;

        public ApproachPositionService(Transform playerTransform, Collider playerCollider, float extraOffset = 0.05f)
        {
            _playerTransform = playerTransform;
            _playerCollider = playerCollider;
            _extraOffset = extraOffset;
        }

        public bool TryGetApproachPosition(EntityIdentity target, out Vector3 destination)
        {
            if (_playerTransform == null || target == null || !target.TryGetComponent<Collider>(out var targetCollider))
            {
                destination = default;
                return false;
            }

            var playerPosition = _playerTransform.position;
            var closestPoint = targetCollider.ClosestPoint(playerPosition);
            var direction = closestPoint - playerPosition;
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.0001f)
            {
                direction = target.transform.position - playerPosition;
                direction.y = 0f;
            }

            if (direction.sqrMagnitude <= 0.0001f)
            {
                direction = Vector3.forward;
            }

            direction.Normalize();

            var playerRadius = GetPlayerRadius();
            destination = closestPoint - direction * (playerRadius + _extraOffset);
            destination.y = _playerTransform.position.y;
            return true;
        }

        private float GetPlayerRadius()
        {
            if (_playerCollider == null)
            {
                return 0f;
            }

            var extents = _playerCollider.bounds.extents;
            return Mathf.Max(extents.x, extents.z);
        }
    }
}
