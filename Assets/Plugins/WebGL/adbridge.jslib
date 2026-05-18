mergeInto(LibraryManager.library, {

    AdBridge_NotifyReady: function () {
        try {
            if (typeof mraid !== 'undefined') {
                // MRAID ready
                console.log('[AdBridge] MRAID ready');
            } else {
                console.log('[AdBridge] NotifyReady (no MRAID)');
            }
        } catch(e) {
            console.warn('[AdBridge] NotifyReady error:', e);
        }
    },

    AdBridge_NotifyClick: function () {
        try {
            if (typeof mraid !== 'undefined' && typeof mraid.open === 'function') {
                mraid.open('https://play.google.com/store');
            } else if (typeof window !== 'undefined') {
                // Fallback: открыть ссылку напрямую
                window.open('https://play.google.com/store', '_blank');
            }
            console.log('[AdBridge] NotifyClick');
        } catch(e) {
            console.warn('[AdBridge] NotifyClick error:', e);
        }
    },

    AdBridge_NotifyVictory: function () {
        try {
            console.log('[AdBridge] NotifyVictory');
            if (typeof window !== 'undefined' && typeof window.onAdVictory === 'function') {
                window.onAdVictory();
            }
        } catch(e) {
            console.warn('[AdBridge] NotifyVictory error:', e);
        }
    },

    AdBridge_Pause: function () {
        try {
            console.log('[AdBridge] Pause');
            if (typeof window !== 'undefined' && typeof window.onAdPause === 'function') {
                window.onAdPause();
            }
        } catch(e) {
            console.warn('[AdBridge] Pause error:', e);
        }
    },

    AdBridge_Resume: function () {
        try {
            console.log('[AdBridge] Resume');
            if (typeof window !== 'undefined' && typeof window.onAdResume === 'function') {
                window.onAdResume();
            }
        } catch(e) {
            console.warn('[AdBridge] Resume error:', e);
        }
    },

    AdBridge_Mute: function (muted) {
        try {
            console.log('[AdBridge] Mute:', muted);
            if (typeof window !== 'undefined' && typeof window.onAdMute === 'function') {
                window.onAdMute(muted);
            }
        } catch(e) {
            console.warn('[AdBridge] Mute error:', e);
        }
    }

});
