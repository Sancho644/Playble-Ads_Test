using Gameplay;
using UnityEngine;

namespace PlayableAd
{
    /// <summary>
    /// Обрабатывает взаимодействие с сундуком.
    /// Когда игрок выбрал сундук как цель и подошёл достаточно близко —
    /// добавляет бонус к силе и скрывает сундук.
    /// </summary>
    public sealed class ChestInteractionHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EntityIdentity playerIdentity;
        [SerializeField] private ActorPower playerPower;
        [SerializeField] private TargetComponent playerTarget;
        [SerializeField] private MoveTargetComponent playerMoveTarget;

        [Header("Chest")]
        [SerializeField] private EntityIdentity chestIdentity;
        [SerializeField] private int chestBonus = 5;
        [SerializeField] private VisibilityComponent chestVisibility;
        [SerializeField] private RemovalRequestComponent chestRemoval;
        [SerializeField] private Animator chestAnimator;
        [SerializeField] private string chestOpenStateName = "Open";

        [Header("Proximity")]
        [SerializeField] private float interactRange = 3f;

        private bool _collected;
        private bool _opening;
        private float _openTimer;
        private const float OpenDuration = 0.6f;

        public bool IsCollected => _collected;

        private void Update()
        {
            if (_collected || playerIdentity == null || chestIdentity == null) return;

            // Ждём завершения анимации открытия
            if (_opening)
            {
                _openTimer += Time.deltaTime;
                if (_openTimer >= OpenDuration)
                    FinishOpen();
                return;
            }

            // Игрок должен выбрать сундук как цель
            if (playerTarget == null || !playerTarget.HasTarget) return;
            if (playerTarget.CurrentTarget != chestIdentity) return;

            // Проверяем дистанцию
            float dist = Vector3.Distance(
                playerIdentity.transform.position,
                chestIdentity.transform.position);
            if (dist > interactRange) return;

            StartOpen();
        }

        private void StartOpen()
        {
            _opening = true;
            _openTimer = 0f;

            // Остановить движение
            playerMoveTarget?.Clear();

            // Анимация открытия
            if (chestAnimator != null && !string.IsNullOrEmpty(chestOpenStateName))
                chestAnimator.CrossFade(chestOpenStateName, 0.05f, 0, 0f);
        }

        private void FinishOpen()
        {
            _collected = true;
            _opening = false;

            // Добавить силу игроку
            playerPower?.Add(chestBonus);

            // Floating text
            if (FloatingTextSpawner.Instance != null)
                FloatingTextSpawner.Instance.SpawnPowerGain(
                    chestIdentity.transform.position, chestBonus);

            // Скрыть сундук
            chestRemoval?.Request();

            // Очистить цель — игрок свободен
            playerTarget?.Clear();
        }
    }
}
