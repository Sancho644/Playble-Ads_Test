using System;
using View;
using UnityEngine;

namespace Gameplay
{
    public sealed class TargetSelectionRaycaster
    {
        private readonly Camera _worldCamera;
        private readonly int _targetLayerMask;

        public TargetSelectionRaycaster(Camera worldCamera, LayerMask targetLayerMask)
        {
            _worldCamera = worldCamera;
            _targetLayerMask = targetLayerMask;
        }

        public int Raycast(Vector2 screenPosition, TargetSelectionView[] results)
        {
            if (_worldCamera == null || results == null || results.Length == 0)
            {
                return 0;
            }

            var ray = _worldCamera.ScreenPointToRay(screenPosition);
            var hits = Physics.RaycastAll(ray, float.MaxValue, _targetLayerMask);
            if (hits.Length == 0)
            {
                return 0;
            }

            Array.Sort(hits, static (left, right) => left.distance.CompareTo(right.distance));

            var count = 0;
            for (var i = 0; i < hits.Length && count < results.Length; i++)
            {
                var collider = hits[i].collider;
                if (collider == null)
                {
                    continue;
                }

                var view = collider.GetComponent<TargetSelectionView>();
                if (view == null)
                {
                    view = collider.GetComponentInParent<TargetSelectionView>();
                }

                if (view == null)
                {
                    continue;
                }

                var isDuplicate = false;
                for (var j = 0; j < count; j++)
                {
                    if (results[j] == view)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if (isDuplicate)
                {
                    continue;
                }

                results[count] = view;
                count++;
            }

            return count;
        }
    }
}
