using Gameplay;
using UnityEngine;

namespace View
{
    public sealed class TargetIndicatorView : MonoBehaviour
    {
        [SerializeField] private MoveTargetComponent moveTarget;
        [SerializeField] private Transform sourceTransform;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private GameObject targetIndicator;

        private TargetIndicatorPresenter _presenter;

        private void Awake()
        {
            if (sourceTransform == null)
            {
                sourceTransform = transform;
            }

            _presenter = new TargetIndicatorPresenter(
                moveTarget,
                sourceTransform,
                lineRenderer,
                targetIndicator);

            _presenter.Refresh();
        }

        private void LateUpdate()
        {
            _presenter?.Refresh();
        }
    }
}
