using UnityEngine;

namespace PlayableAd
{
    /// <summary>
    /// Анимированная стрелка-подсказка, указывающая на первую правильную цель.
    /// Исчезает после первого тапа игрока или по таймеру.
    /// </summary>
    public sealed class HintArrowView : MonoBehaviour
    {
        [SerializeField] private GameObject arrowRoot;
        [SerializeField] private Transform target;
        [SerializeField] private float showDuration = 4f;
        [SerializeField] private float bobAmplitude = 0.15f;
        [SerializeField] private float bobFrequency = 2f;
        [SerializeField] private float offsetY = 2.5f;

        private float _elapsed;
        private bool _hidden;
        private Vector3 _baseOffset;

        private void Awake()
        {
            _baseOffset = new Vector3(0f, offsetY, 0f);
        }

        private void Update()
        {
            if (_hidden || arrowRoot == null || target == null) return;

            _elapsed += Time.deltaTime;

            // Позиция над целью с боббингом
            float bob = Mathf.Sin(_elapsed * bobFrequency * Mathf.PI * 2f) * bobAmplitude;
            arrowRoot.transform.position = target.position + _baseOffset + Vector3.up * bob;

            // Скрыть по таймеру
            if (_elapsed >= showDuration)
                Hide();
        }

        public void Hide()
        {
            _hidden = true;
            if (arrowRoot != null)
                arrowRoot.SetActive(false);
        }

        public void Show(Transform newTarget = null)
        {
            _hidden = false;
            _elapsed = 0f;
            if (newTarget != null) target = newTarget;
            if (arrowRoot != null)
                arrowRoot.SetActive(true);
        }
    }
}
