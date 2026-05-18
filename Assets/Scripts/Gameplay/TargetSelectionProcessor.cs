using View;
using UnityEngine;

namespace Gameplay
{
    public sealed class TargetSelectionProcessor
    {
        private readonly TargetSelectionView[] _hitViews = new TargetSelectionView[8];
        private readonly TargetSelectionRaycaster _raycaster;
        private readonly TargetSelectionService _selectionService;

        public TargetSelectionProcessor(Camera worldCamera, LayerMask targetLayerMask, TargetSelectionService selectionService)
        {
            _raycaster = new TargetSelectionRaycaster(worldCamera, targetLayerMask);
            _selectionService = selectionService;
        }

        public void ProcessPointerDown(Vector2 screenPosition)
        {
            if (_selectionService == null)
            {
                return;
            }

            var hitCount = _raycaster.Raycast(screenPosition, _hitViews);

            for (var i = 0; i < hitCount; i++)
            {
                var view = _hitViews[i];
                if (view == null)
                {
                    continue;
                }

                if (_selectionService.TrySelect(view.EntityIdentity))
                {
                    return;
                }

                if (!view.AllowClickThrough)
                {
                    return;
                }
            }
        }
    }
}
