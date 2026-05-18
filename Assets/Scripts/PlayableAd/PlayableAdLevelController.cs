using Gameplay;
using UnityEngine;

namespace PlayableAd
{
    /// <summary>
    /// Главный контроллер уровня Playable Ad.
    /// Координирует hint-стрелку, сундук, победу.
    /// </summary>
    public sealed class PlayableAdLevelController : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private ActorPower playerPower;
        [SerializeField] private TargetComponent playerTarget;

        [Header("Enemies")]
        [SerializeField] private EntityIdentity weakEnemy;
        [SerializeField] private EntityIdentity strongEnemy;

        [Header("Chest")]
        [SerializeField] private EntityIdentity chestIdentity;
        [SerializeField] private ChestInteractionHandler chestHandler;

        [Header("Hint")]
        [SerializeField] private HintArrowView hintArrow;

        [Header("Victory")]
        [SerializeField] private LevelVictoryController victoryController;

        private bool _hintHidden;
        private bool _firstActionDone;

        private void Update()
        {
            // Скрыть hint при первом тапе
            if (!_hintHidden && playerTarget != null && playerTarget.HasTarget)
            {
                _hintHidden = true;
                hintArrow?.Hide();
            }

            // Обновить hint-цель в зависимости от состояния
            if (!_hintHidden && hintArrow != null)
            {
                UpdateHintTarget();
            }
        }

        private void UpdateHintTarget()
        {
            // Логика подсказки: сначала слабый враг, потом сундук, потом сильный
            if (weakEnemy != null && !IsEntityDead(weakEnemy))
            {
                // Если игрок может победить слабого врага
                if (playerPower != null && weakEnemy.TryGetComponent<ActorPower>(out var wp) && playerPower.Value >= wp.Value)
                    hintArrow.Show(weakEnemy.transform);
                else if (chestIdentity != null && !IsEntityDead(chestIdentity) && !chestHandler.IsCollected)
                    hintArrow.Show(chestIdentity.transform);
            }
            else if (chestIdentity != null && !chestHandler.IsCollected)
            {
                hintArrow.Show(chestIdentity.transform);
            }
            else if (strongEnemy != null && !IsEntityDead(strongEnemy))
            {
                hintArrow.Show(strongEnemy.transform);
            }
        }

        private bool IsEntityDead(EntityIdentity entity)
        {
            if (entity == null) return true;
            if (entity.TryGetComponent<VisibilityComponent>(out var vis) && vis.IsHidden) return true;
            if (entity.TryGetComponent<RemovalRequestComponent>(out var rem) && rem.IsRequested) return true;
            return false;
        }
    }
}
