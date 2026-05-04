using Gameplay;
using UnityEngine;

namespace View
{
    public sealed class TargetSelectionPresenter
    {
        private readonly TargetComponent _playerTarget;
        private readonly EntityIdentity _entityIdentity;
        private readonly GameObject _selectedMarker;

        private bool _isVisible;

        public TargetSelectionPresenter(TargetComponent playerTarget, EntityIdentity entityIdentity, GameObject selectedMarker)
        {
            _playerTarget = playerTarget;
            _entityIdentity = entityIdentity;
            _selectedMarker = selectedMarker;
        }

        public void Refresh()
        {
            if (_playerTarget == null || _entityIdentity == null || _selectedMarker == null)
            {
                return;
            }

            var shouldBeVisible = _playerTarget.IsSelected(_entityIdentity);
            if (_isVisible == shouldBeVisible)
            {
                return;
            }

            _isVisible = shouldBeVisible;
            _selectedMarker.SetActive(_isVisible);
        }
    }
}
