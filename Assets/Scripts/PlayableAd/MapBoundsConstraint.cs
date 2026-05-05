using Gameplay;
using UnityEngine;

namespace PlayableAd
{
    /// <summary>
    /// Ограничивает движение игрока в пределах карты.
    /// Работает как LateUpdate-корректор позиции.
    /// </summary>
    public sealed class MapBoundsConstraint : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private MoveTargetComponent moveTarget;

        [Header("Map Bounds (World Space)")]
        [SerializeField] private float minX = -9f;
        [SerializeField] private float maxX = 9f;
        [SerializeField] private float minZ = -9f;
        [SerializeField] private float maxZ = 9f;

        private void LateUpdate()
        {
            if (playerTransform == null) return;

            var pos = playerTransform.position;
            bool clamped = false;

            if (pos.x < minX) { pos.x = minX; clamped = true; }
            if (pos.x > maxX) { pos.x = maxX; clamped = true; }
            if (pos.z < minZ) { pos.z = minZ; clamped = true; }
            if (pos.z > maxZ) { pos.z = maxZ; clamped = true; }

            if (clamped)
            {
                playerTransform.position = pos;
                moveTarget?.Clear();
            }
        }

        // Проверить, находится ли точка в пределах карты
        public bool IsInBounds(Vector3 point)
        {
            return point.x >= minX && point.x <= maxX &&
                   point.z >= minZ && point.z <= maxZ;
        }

        // Зажать точку в пределах карты
        public Vector3 Clamp(Vector3 point)
        {
            point.x = Mathf.Clamp(point.x, minX, maxX);
            point.z = Mathf.Clamp(point.z, minZ, maxZ);
            return point;
        }
    }
}
