using UnityEngine;

namespace Gameplay
{
    public sealed class TargetSelectionService
    {
        private readonly EntityIdentity _playerIdentity;
        private readonly TargetComponent _playerTarget;
        private readonly MoveTargetComponent _moveTarget;
        private readonly ApproachPositionService _approachPositionService;

        public TargetSelectionService(
            EntityIdentity playerIdentity,
            TargetComponent playerTarget,
            MoveTargetComponent moveTarget,
            ApproachPositionService approachPositionService)
        {
            _playerIdentity = playerIdentity;
            _playerTarget = playerTarget;
            _moveTarget = moveTarget;
            _approachPositionService = approachPositionService;
        }

        public bool TrySelect(EntityIdentity candidate)
        {
            if (_playerIdentity == null || _playerTarget == null || candidate == null)
                return false;

            // Нельзя выбрать самого себя или другого игрока
            if (candidate == _playerIdentity || candidate.IsPlayer)
                return false;

            // Принимаем врагов и интерактивные объекты (сундуки)
            if (!candidate.IsEnemy && !candidate.IsInteractiveObject)
                return false;

            if (candidate.TryGetComponent<RemovalRequestComponent>(out var removalRequest) && removalRequest.IsRequested)
                return false;

            if (candidate.TryGetComponent<VisibilityComponent>(out var visibility) && visibility.IsHidden)
                return false;

            _playerTarget.Set(candidate);

            // Двигаться к цели
            if (_moveTarget != null && _approachPositionService != null)
            {
                if (_approachPositionService.TryGetApproachPosition(candidate, out var destination))
                    _moveTarget.SetDestination(destination);
                else
                    _moveTarget.SetDestination(candidate.transform.position);
            }

            return true;
        }
    }
}
