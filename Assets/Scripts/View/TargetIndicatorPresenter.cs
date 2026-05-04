using Gameplay;
using UnityEngine;

namespace View
{
    public sealed class TargetIndicatorPresenter
    {
        private readonly MoveTargetComponent _moveTarget;
        private readonly Transform _sourceTransform;
        private readonly LineRenderer _lineRenderer;
        private readonly GameObject _targetIndicator;

        private bool _isVisible;
        private bool _isInitialized;

        public TargetIndicatorPresenter(
            MoveTargetComponent moveTarget,
            Transform sourceTransform,
            LineRenderer lineRenderer,
            GameObject targetIndicator)
        {
            _moveTarget = moveTarget;
            _sourceTransform = sourceTransform;
            _lineRenderer = lineRenderer;
            _targetIndicator = targetIndicator;
        }

        public void Refresh()
        {
            if (_moveTarget == null || _sourceTransform == null)
            {
                SetVisible(false);
                return;
            }

            if (!_moveTarget.HasDestination)
            {
                SetVisible(false);
                return;
            }

            var sourcePosition = _sourceTransform.position;
            var targetPosition = _moveTarget.Destination;

            if (_lineRenderer != null)
            {
                _lineRenderer.positionCount = 2;
                _lineRenderer.SetPosition(0, sourcePosition);
                _lineRenderer.SetPosition(1, targetPosition);
            }

            if (_targetIndicator != null)
            {
                _targetIndicator.transform.position = targetPosition;
            }

            SetVisible(true);
        }

        private void SetVisible(bool isVisible)
        {
            if (_isInitialized && _isVisible == isVisible)
            {
                return;
            }

            _isInitialized = true;
            _isVisible = isVisible;

            if (_lineRenderer != null)
            {
                _lineRenderer.enabled = isVisible;
            }

            if (_targetIndicator != null)
            {
                _targetIndicator.SetActive(isVisible);
            }
        }
    }
}
