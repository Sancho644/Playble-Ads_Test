namespace Gameplay
{
    public sealed class CombatResultApplySystem
    {
        private readonly ActorPower _playerPower;
        private readonly TargetComponent _playerTarget;
        private readonly CombatResultComponent _combatResult;
        private readonly VisualFeedbackEventComponent _visualFeedback;

        public CombatResultApplySystem(
            ActorPower playerPower,
            TargetComponent playerTarget,
            CombatResultComponent combatResult,
            VisualFeedbackEventComponent visualFeedback)
        {
            _playerPower = playerPower;
            _playerTarget = playerTarget;
            _combatResult = combatResult;
            _visualFeedback = visualFeedback;
        }

        public void Tick()
        {
            if (_playerPower == null || _combatResult == null || !_combatResult.HasPendingApplication)
            {
                return;
            }

            switch (_combatResult.Result)
            {
                case CombatResultState.Win:
                    ApplyWin();
                    break;
                case CombatResultState.Lose:
                    ApplyLose();
                    break;
            }

            _combatResult.MarkApplied();
            _playerTarget?.RequestClear(UnityEngine.Time.frameCount);
        }

        private void ApplyWin()
        {
            _playerPower.Add(_combatResult.PowerDelta);

            var target = _combatResult.ResolvedTarget;
            if (target != null && target.TryGetComponent<RemovalRequestComponent>(out var removalRequest))
            {
                removalRequest.Request();
            }
        }

        private void ApplyLose()
        {
            if (_visualFeedback == null)
            {
                return;
            }

            _visualFeedback.Raise(VisualFeedbackEventType.Damage, _combatResult.ResolvedTarget);
        }
    }
}
