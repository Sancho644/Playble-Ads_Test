using System.Runtime.InteropServices;
using UnityEngine;

namespace PlayableAd
{
    /// <summary>
    /// Мост между Unity и рекламной платформой (MRAID / заглушки).
    /// Все методы безопасны для вызова в любой среде.
    /// </summary>
    public static class AdBridge
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void AdBridge_NotifyReady();

        [DllImport("__Internal")]
        private static extern void AdBridge_NotifyClick();

        [DllImport("__Internal")]
        private static extern void AdBridge_NotifyVictory();

        [DllImport("__Internal")]
        private static extern void AdBridge_Pause();

        [DllImport("__Internal")]
        private static extern void AdBridge_Resume();

        [DllImport("__Internal")]
        private static extern void AdBridge_Mute(bool muted);
#endif

        /// <summary>Реклама загружена и готова к показу.</summary>
        public static void NotifyReady()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            AdBridge_NotifyReady();
#else
            Debug.Log("[AdBridge] NotifyReady");
#endif
        }

        /// <summary>Пользователь нажал CTA — переход в магазин.</summary>
        public static void NotifyClick()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            AdBridge_NotifyClick();
#else
            Debug.Log("[AdBridge] NotifyClick — CTA pressed");
#endif
        }

        /// <summary>Игрок победил — уровень пройден.</summary>
        public static void NotifyVictory()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            AdBridge_NotifyVictory();
#else
            Debug.Log("[AdBridge] NotifyVictory");
#endif
        }

        /// <summary>Пауза (например, реклама свёрнута).</summary>
        public static void Pause()
        {
            //Time.timeScale = 0f;
            AudioListener.pause = true;
#if UNITY_WEBGL && !UNITY_EDITOR
            AdBridge_Pause();
#else
            Debug.Log("[AdBridge] Pause");
#endif
        }

        /// <summary>Возобновление после паузы.</summary>
        public static void Resume()
        {
            //Time.timeScale = 1f;
            AudioListener.pause = false;
#if UNITY_WEBGL && !UNITY_EDITOR
            AdBridge_Resume();
#else
            Debug.Log("[AdBridge] Resume");
#endif
        }

        /// <summary>Управление звуком.</summary>
        public static void Mute(bool muted)
        {
            AudioListener.volume = muted ? 0f : 1f;
#if UNITY_WEBGL && !UNITY_EDITOR
            AdBridge_Mute(muted);
#else
            Debug.Log($"[AdBridge] Mute: {muted}");
#endif
        }
    }
}
