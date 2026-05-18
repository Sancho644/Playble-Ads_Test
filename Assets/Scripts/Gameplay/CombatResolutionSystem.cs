namespace Gameplay
{
    public sealed class CombatResolutionSystem
    {
        private readonly EntityIdentity _playerIdentity;
        private readonly ActorPower _playerPower;
        private readonly TargetComponent _playerTarget;
        private readonly CombatResultComponent _combatResult;
        private readonly CombatProximityService _combatProximityService;

        public CombatResolutionSystem(
            EntityIdentity playerIdentity,
            ActorPower playerPower,
            TargetComponent playerTarget,
            CombatResultComponent combatResult,
            CombatProximityService combatProximityService)
        {
            _playerIdentity = playerIdentity;
            _playerPower = playerPower;
            _playerTarget = playerTarget;
            _combatResult = combatResult;
            _combatProximityService = combatProximityService;
        }

        public void Tick()
        {
            if (_playerIdentity == null || _playerPower == null || _playerTarget == null || _combatResult == null)
            {
                return;
            }

            if (!_playerTarget.HasTarget)
            {
                return;
            }

            if (_combatResult.ProcessedSelectionVersion == _playerTarget.SelectionVersion)
            {
                return;
            }

            var target = _playerTarget.CurrentTarget;
            if (target == null || !target.TryGetComponent<ActorPower>(out var targetPower))
            {
                _combatResult.SetResult(
                    CombatResultState.None,
                    target,
                    0,
                    _playerTarget.SelectionVersion);
                _playerTarget.Clear();
                return;
            }

            if (_combatProximityService != null && !_combatProximityService.IsInRange(target))
            {
                return;
            }

            if (_playerPower.Value > targetPower.Value)
            {
                ResolveWin(target, targetPower.Value);
                return;
            }

            ResolveLose(target);
        }

        private void ResolveWin(EntityIdentity target, int powerGain)
        {
            _combatResult.SetResult(
                CombatResultState.Win,
                target,
                powerGain,
                _playerTarget.SelectionVersion);
        }

        private void ResolveLose(EntityIdentity target)
        {
            _combatResult.SetResult(
                CombatResultState.Lose,
                target,
                0,
                _playerTarget.SelectionVersion);
        }
    }
}
