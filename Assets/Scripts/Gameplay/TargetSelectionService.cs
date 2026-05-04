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
            {
                return false;
            }

            if (candidate == _playerIdentity || candidate.IsPlayer || !candidate.IsEnemy)
            {
                return false;
            }

            if (candidate.TryGetComponent<RemovalRequestComponent>(out var removalRequest) && removalRequest.IsRequested)
            {
                return false;
            }

            if (candidate.TryGetComponent<VisibilityComponent>(out var visibility) && visibility.IsHidden)
            {
                return false;
            }

            _playerTarget.Set(candidate);

            if (_moveTarget != null && _approachPositionService != null && _approachPositionService.TryGetApproachPosition(candidate, out var destination))
            {
                _moveTarget.SetDestination(destination);
            }

            return true;
        }
    }
}
