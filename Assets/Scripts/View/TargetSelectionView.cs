using Gameplay;
using UnityEngine;

namespace View
{
    public sealed class TargetSelectionView : MonoBehaviour
    {
        [SerializeField] private TargetComponent playerTarget;
        [SerializeField] private EntityIdentity entityIdentity;
        [SerializeField] private GameObject selectedMarker;
        [SerializeField] private bool allowClickThrough = true;

        private TargetSelectionPresenter _presenter;

        public EntityIdentity EntityIdentity => entityIdentity;
        public bool AllowClickThrough => allowClickThrough;

        private void Awake()
        {
            _presenter = new TargetSelectionPresenter(playerTarget, entityIdentity, selectedMarker);
            _presenter.Refresh();
        }

        private void LateUpdate()
        {
            _presenter?.Refresh();
        }
    }
}
