using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayableAd
{
    /// <summary>
    /// Screen-space UI лейбл, который следует за 3D-объектом в мире.
    /// Отображает ActorPower с анимацией при изменении.
    /// </summary>
    public sealed class WorldSpacePowerLabel : MonoBehaviour
    {
        [HideInInspector] public string TargetName;
        [HideInInspector] public Color LabelColor = Color.white;

        private Transform _target;
        private ActorPower _actorPower;
        private ChestBonusDisplay _chestBonus;
        private EntityIdentity _entityIdentity;
        private VisibilityComponent _visibility;
        private RemovalRequestComponent _removal;

        private TextMeshProUGUI _label;
        private RectTransform _rectTransform;
        private Camera _mainCamera;

        private int _lastPower = int.MinValue;
        private float _punchTimer;
        private Vector3 _baseScale;
        private const float PunchDuration = 0.25f;
        private const float PunchScale = 1.6f;

        // Смещение над головой юнита (в мировых единицах)
        private const float WorldOffsetY = 2.2f;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _rectTransform = GetComponent<RectTransform>();
            if (_rectTransform == null)
                _rectTransform = gameObject.AddComponent<RectTransform>();

            // Создать фон
            var bg = new GameObject("Background");
            bg.transform.SetParent(transform, false);
            var bgRect = bg.AddComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(80f, 50f);
            var bgImage = bg.AddComponent<Image>();
            bgImage.color = new Color(0f, 0f, 0f, 0.55f);

            // Создать текст
            var textGO = new GameObject("Text");
            textGO.transform.SetParent(bg.transform, false);
            var textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            _label = textGO.AddComponent<TextMeshProUGUI>();
            _label.fontSize = 28f;
            _label.fontStyle = FontStyles.Bold;
            _label.alignment = TextAlignmentOptions.Center;
            _label.color = LabelColor;

            _baseScale = bg.transform.localScale;
        }

        private void Start()
        {
            // Найти целевой объект по имени
            var targetGO = GameObject.Find(TargetName);
            if (targetGO == null)
            {
                gameObject.SetActive(false);
                return;
            }

            _target = targetGO.transform;
            _actorPower = targetGO.GetComponent<ActorPower>();
            _chestBonus = targetGO.GetComponent<ChestBonusDisplay>();
            _entityIdentity = targetGO.GetComponent<EntityIdentity>();
            _visibility = targetGO.GetComponent<VisibilityComponent>();
            _removal = targetGO.GetComponent<RemovalRequestComponent>();

            // Установить начальный цвет по типу
            if (_entityIdentity != null && _label != null)
            {
                if (_entityIdentity.IsPlayer)
                    _label.color = new Color(0.3f, 0.7f, 1f);
                else if (_entityIdentity.IsEnemy)
                    _label.color = new Color(1f, 0.3f, 0.3f);
                else
                    _label.color = new Color(0.3f, 1f, 0.4f);
            }
        }

        private void LateUpdate()
        {
            if (_target == null || _mainCamera == null) return;

            // Скрыть если объект мёртв/скрыт
            bool shouldHide = (_visibility != null && _visibility.IsHidden) ||
                              (_removal != null && _removal.IsRequested);
            if (shouldHide)
            {
                gameObject.SetActive(false);
                return;
            }

            // Позиция в Screen Space
            var worldPos = _target.position + Vector3.up * WorldOffsetY;
            var screenPos = _mainCamera.WorldToScreenPoint(worldPos);

            // Если за камерой — скрыть
            if (screenPos.z < 0f)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            _rectTransform.position = screenPos;

            // Обновить текст — для сундука читаем ChestBonusDisplay
            int currentPower = _chestBonus != null
                ? _chestBonus.BonusValue
                : (_actorPower != null ? _actorPower.Value : 0);

            if (currentPower != _lastPower)
            {
                _lastPower = currentPower;
                bool isBonus = _entityIdentity != null && _entityIdentity.IsInteractiveObject;
                if (_label != null)
                    _label.text = isBonus ? $"+{currentPower}" : currentPower.ToString();
                _punchTimer = PunchDuration;
            }

            // Punch scale анимация
            var bgTransform = transform.childCount > 0 ? transform.GetChild(0) : null;
            if (bgTransform != null && _punchTimer > 0f)
            {
                _punchTimer -= Time.deltaTime;
                float t = _punchTimer / PunchDuration;
                float scale = Mathf.Lerp(1f, PunchScale, t);
                bgTransform.localScale = _baseScale * scale;
            }
        }
    }
}
