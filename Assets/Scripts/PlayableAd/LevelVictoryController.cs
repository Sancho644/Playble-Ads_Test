using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace PlayableAd
{
    /// <summary>
    /// Следит за уничтожением всех врагов и показывает экран победы.
    /// </summary>
    public sealed class LevelVictoryController : MonoBehaviour
    {
        [Header("Enemies to track")]
        [SerializeField] private VisibilityComponent[] enemyVisibilities;

        [Header("Victory Screen")]
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private TextMeshProUGUI victoryText;
        [SerializeField] private Button ctaButton;
        [SerializeField] private TextMeshProUGUI ctaButtonText;

        [Header("Player visual")]
        [SerializeField] private Transform playerVisualRoot;

        [Header("Timing")]
        [SerializeField] private float delayBeforeShow = 0.8f;

        private bool _victoryShown;
        private float _pulseTimer;

        private void Awake()
        {
            if (victoryPanel != null)
                victoryPanel.SetActive(false);

            if (ctaButton != null)
                ctaButton.onClick.AddListener(OnCtaClicked);
        }

        private void Update()
        {
            if (_victoryShown) return;
            if (!AllEnemiesDead()) return;

            _victoryShown = true;
            StartCoroutine(ShowVictoryDelayed());
        }

        private bool AllEnemiesDead()
        {
            if (enemyVisibilities == null || enemyVisibilities.Length == 0) return false;
            foreach (var vis in enemyVisibilities)
            {
                if (vis == null) continue;
                if (!vis.IsHidden) return false;
            }
            return true;
        }

        private IEnumerator ShowVictoryDelayed()
        {
            yield return new WaitForSeconds(delayBeforeShow);
            ShowVictory();
        }

        private void ShowVictory()
        {
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
                // Анимация появления
                StartCoroutine(AnimateVictoryPanel());
            }

            // Уведомить рекламную систему
            AdBridge.NotifyVictory();
        }

        private IEnumerator AnimateVictoryPanel()
        {
            if (victoryPanel == null) yield break;

            var canvasGroup = victoryPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = victoryPanel.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        private void LateUpdate()
        {
            if (!_victoryShown || ctaButton == null) return;

            // Пульсация кнопки CTA
            _pulseTimer += Time.deltaTime;
            float scale = 1f + Mathf.Sin(_pulseTimer * 3f) * 0.06f;
            ctaButton.transform.localScale = Vector3.one * scale;
        }

        private void OnCtaClicked()
        {
            AdBridge.NotifyClick();
        }
    }
}
