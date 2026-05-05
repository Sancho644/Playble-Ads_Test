using UnityEngine;

namespace PlayableAd
{
    /// <summary>
    /// Управляет жизненным циклом рекламы: NotifyReady при старте,
    /// обработка pause/resume через Application.focusChanged.
    /// </summary>
    public sealed class AdLifecycleController : MonoBehaviour
    {
        private void Start()
        {
            AdBridge.NotifyReady();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
                AdBridge.Resume();
            else
                AdBridge.Pause();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                AdBridge.Pause();
            else
                AdBridge.Resume();
        }
    }
}
