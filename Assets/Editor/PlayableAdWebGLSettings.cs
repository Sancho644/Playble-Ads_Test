using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

/// <summary>
/// Настраивает WebGL Player Settings для Playable Ad:
/// - IL2CPP scripting backend
/// - Brotli compression
/// - Strip engine code
/// - High managed stripping
/// - Portrait orientation
/// - Минимальный размер билда
/// </summary>
public static class PlayableAdWebGLSettings
{
    [MenuItem("Tools/Configure WebGL Settings for Playable Ad")]
    public static void Configure()
    {
        Debug.Log("[WebGLSettings] Configuring WebGL Player Settings...");

        // ── Scripting Backend: IL2CPP ─────────────────────────────────────────
        PlayerSettings.SetScriptingBackend(
            NamedBuildTarget.WebGL,
            ScriptingImplementation.IL2CPP);
        Debug.Log("[WebGLSettings] Scripting backend: IL2CPP");

        // ── Strip Engine Code ─────────────────────────────────────────────────
        PlayerSettings.stripEngineCode = true;
        Debug.Log("[WebGLSettings] Strip engine code: enabled");

        // ── Managed Stripping Level: High ─────────────────────────────────────
        PlayerSettings.SetManagedStrippingLevel(
            NamedBuildTarget.WebGL,
            ManagedStrippingLevel.High);
        Debug.Log("[WebGLSettings] Managed stripping level: High");

        // ── WebGL Compression: Brotli ─────────────────────────────────────────
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
        Debug.Log("[WebGLSettings] Compression: Brotli");

        // ── WebGL: отключить исключения для меньшего размера ──────────────────
        PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.None;
        Debug.Log("[WebGLSettings] Exception support: None (smaller build)");

        // ── WebGL: Data Caching ───────────────────────────────────────────────
        PlayerSettings.WebGL.dataCaching = true;
        Debug.Log("[WebGLSettings] Data caching: enabled");

        // ── Orientation: Portrait ─────────────────────────────────────────────
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.allowedAutorotateToPortrait = true;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = false;
        PlayerSettings.allowedAutorotateToLandscapeRight = false;
        Debug.Log("[WebGLSettings] Orientation: Portrait");

        // ── Company / Product Name ────────────────────────────────────────────
        PlayerSettings.companyName = "PlayableAd";
        PlayerSettings.productName = "RPG Combat Demo";

        // ── WebGL Template: минимальный ───────────────────────────────────────
        PlayerSettings.WebGL.template = "APPLICATION:Default";

        // ── Разрешение для portrait ───────────────────────────────────────────
        PlayerSettings.defaultScreenWidth = 390;
        PlayerSettings.defaultScreenHeight = 844;

        // ── Color Space: Gamma (меньше размер) ───────────────────────────────
        PlayerSettings.colorSpace = ColorSpace.Gamma;
        Debug.Log("[WebGLSettings] Color space: Gamma");

        // ── Graphics API: только WebGL2 ───────────────────────────────────────
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL, false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.WebGL,
            new[] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 });
        Debug.Log("[WebGLSettings] Graphics API: OpenGLES3 (WebGL2)");

        // ── Сохранить ─────────────────────────────────────────────────────────
        AssetDatabase.SaveAssets();

        Debug.Log("[WebGLSettings] ✓ WebGL settings configured successfully!");
        Debug.Log("[WebGLSettings] Expected build size: < 5 MB with Brotli compression");
    }

    [MenuItem("Tools/Show WebGL Build Size Estimate")]
    public static void ShowBuildSizeEstimate()
    {
        Debug.Log("[WebGLSettings] Build size estimate:");
        Debug.Log("  - Unity runtime (stripped): ~1.5-2 MB");
        Debug.Log("  - Game scripts (IL2CPP): ~0.3-0.5 MB");
        Debug.Log("  - 3D models (FBX): ~0.5-1 MB");
        Debug.Log("  - Textures (compressed): ~0.5-1 MB");
        Debug.Log("  - Audio (3-4 clips, mono 22kHz): ~0.3-0.5 MB");
        Debug.Log("  - Total estimate: ~3-5 MB (Brotli compressed)");
        Debug.Log("[WebGLSettings] To reduce further:");
        Debug.Log("  1. Use only 3-4 audio clips (remove unused)");
        Debug.Log("  2. Compress textures to DXT/ASTC");
        Debug.Log("  3. Remove unused packages from manifest.json");
    }
}
