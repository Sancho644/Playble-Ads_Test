using Gameplay;
using UnityEngine;

namespace View
{
    public sealed class VisibilityPresenter
    {
        private readonly VisibilityComponent _visibility;
        private readonly GameObject _contentRoot;

        private bool _isVisible = true;

        public VisibilityPresenter(VisibilityComponent visibility, GameObject contentRoot)
        {
            _visibility = visibility;
            _contentRoot = contentRoot;
        }

        public void Refresh()
        {
            if (_visibility == null || _contentRoot == null)
            {
                return;
            }

            var shouldBeVisible = !_visibility.IsHidden;
            if (_isVisible == shouldBeVisible)
            {
                return;
            }

            _isVisible = shouldBeVisible;
            _contentRoot.SetActive(_isVisible);
        }
    }
}
