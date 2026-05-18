using TMPro;
using UnityEngine;

namespace PlayableAd
{
    /// <summary>
    /// Спавнит всплывающий текст "+N" при получении силы.
    /// </summary>
    public sealed class FloatingTextSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject floatingTextPrefab;
        [SerializeField] private float riseSpeed = 2f;
        [SerializeField] private float lifetime = 0.9f;

        private static FloatingTextSpawner _instance;

        public static FloatingTextSpawner Instance => _instance;

        private void Awake()
        {
            _instance = this;
        }

        public void Spawn(Vector3 worldPosition, string text, Color color)
        {
            if (floatingTextPrefab == null) return;
            var go = Instantiate(floatingTextPrefab, worldPosition + Vector3.up * 1.5f, Quaternion.identity);
            var ft = go.GetComponent<FloatingTextItem>();
            if (ft != null)
                ft.Init(text, color, riseSpeed, lifetime);
        }

        public void SpawnPowerGain(Vector3 worldPosition, int amount)
        {
            Spawn(worldPosition, $"+{amount}", new Color(0.2f, 0.9f, 0.3f));
        }

        public void SpawnDamage(Vector3 worldPosition)
        {
            Spawn(worldPosition, "!", new Color(1f, 0.2f, 0.2f));
        }
    }
}
