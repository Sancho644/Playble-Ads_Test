using TMPro;
using UnityEngine;

namespace PlayableAd
{
    /// <summary>
    /// Всплывающий текст: поднимается вверх и исчезает.
    /// </summary>
    public sealed class FloatingTextItem : MonoBehaviour
    {
        [SerializeField] private TextMeshPro label;

        private float _riseSpeed;
        private float _lifetime;
        private float _elapsed;
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
            if (label == null)
                label = GetComponentInChildren<TextMeshPro>();
        }

        public void Init(string text, Color color, float riseSpeed, float lifetime)
        {
            if (label != null)
            {
                label.text = text;
                label.color = color;
            }
            _riseSpeed = riseSpeed;
            _lifetime = lifetime;
            _elapsed = 0f;
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            transform.position += Vector3.up * _riseSpeed * Time.deltaTime;

            // Billboard
            if (_cam != null)
                transform.rotation = _cam.transform.rotation;

            // Fade out
            float t = _elapsed / _lifetime;
            if (label != null)
            {
                var c = label.color;
                c.a = Mathf.Lerp(1f, 0f, t);
                label.color = c;
            }

            if (_elapsed >= _lifetime)
                Destroy(gameObject);
        }
    }
}
