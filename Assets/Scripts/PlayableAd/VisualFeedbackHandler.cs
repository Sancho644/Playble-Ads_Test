using Gameplay;
using UnityEngine;

namespace PlayableAd
{
    /// <summary>
    /// Слушает VisualFeedbackEventComponent и запускает визуальные эффекты:
    /// - Damage: floating "!"
    /// - Win: floating "+N"
    /// </summary>
    public sealed class VisualFeedbackHandler : MonoBehaviour
    {
        [SerializeField] private VisualFeedbackEventComponent visualFeedback;
        [SerializeField] private ActorPower playerPower;
        [SerializeField] private CombatResultComponent combatResult;

        private int _lastEventVersion = -1;
        private int _lastResultVersion = -1;

        private void Update()
        {
            HandleDamageFeedback();
            HandleWinFeedback();
        }

        private void HandleDamageFeedback()
        {
            if (visualFeedback == null) return;
            if (visualFeedback.EventVersion == _lastEventVersion) return;
            if (visualFeedback.EventType != VisualFeedbackEventType.Damage) return;

            _lastEventVersion = visualFeedback.EventVersion;

            if (FloatingTextSpawner.Instance != null)
            {
                var pos = visualFeedback.Source != null
                    ? visualFeedback.Source.transform.position
                    : transform.position;
                FloatingTextSpawner.Instance.SpawnDamage(pos);
            }
        }

        private void HandleWinFeedback()
        {
            if (combatResult == null) return;
            if (combatResult.ResultVersion == _lastResultVersion) return;
            if (combatResult.Result != CombatResultState.Win) return;

            _lastResultVersion = combatResult.ResultVersion;

            if (FloatingTextSpawner.Instance != null && combatResult.PowerDelta > 0)
            {
                var pos = combatResult.ResolvedTarget != null
                    ? combatResult.ResolvedTarget.transform.position
                    : transform.position;
                FloatingTextSpawner.Instance.SpawnPowerGain(pos, combatResult.PowerDelta);
            }
        }
    }
}
