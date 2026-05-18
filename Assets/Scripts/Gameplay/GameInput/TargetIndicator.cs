using UnityEngine;

namespace Gameplay.GameInput
{
    public class TargetIndicator : MonoBehaviour
    {
        public void Show(Transform target)
        {
            gameObject.SetActive(true);

            transform.position =
                target.position + Vector3.up * 0.05f;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}