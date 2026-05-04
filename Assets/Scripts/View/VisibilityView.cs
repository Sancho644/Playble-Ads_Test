using Gameplay;
using UnityEngine;

namespace View
{
    public sealed class VisibilityView : MonoBehaviour
    {
        [SerializeField] private VisibilityComponent visibility;
        [SerializeField] private GameObject contentRoot;

        private VisibilityPresenter _presenter;

        private void Awake()
        {
            if (contentRoot == null)
            {
                contentRoot = gameObject;
            }

            _presenter = new VisibilityPresenter(visibility, contentRoot);
            _presenter.Refresh();
        }

        private void LateUpdate()
        {
            _presenter?.Refresh();
        }
    }
}
