using UnityEngine;
using UnityEditor;
using System.IO;

public static class PlayableAdCleanup
{
    [MenuItem("Tools/Cleanup Playable Ad Project")]
    public static void Cleanup()
    {
        // ── Удалить временные Editor-скрипты ─────────────────────────────────
        string[] editorToDelete = {
            "Assets/Editor/PlayableAdDebugPositions.cs",
            "Assets/Editor/PlayableAdFinalLayout.cs",
            "Assets/Editor/PlayableAdFixCamera.cs",
            "Assets/Editor/PlayableAdFixChest.cs",
            "Assets/Editor/PlayableAdFixChestRange.cs",
            "Assets/Editor/PlayableAdFixFinal2.cs",
            "Assets/Editor/PlayableAdFixFinal3.cs",
            "Assets/Editor/PlayableAdFixFinal4.cs",
            "Assets/Editor/PlayableAdFixPositions.cs",
            "Assets/Editor/PlayableAdFixScale.cs",
            "Assets/Editor/PlayableAdRemovePatch.cs",
            "Assets/Editor/PlayableAdSceneView.cs",
            "Assets/Editor/ForceRecompile.cs",
        };

        foreach (var path in editorToDelete)
        {
            if (File.Exists(path))
            {
                AssetDatabase.DeleteAsset(path);
                Debug.Log($"[Cleanup] Deleted: {path}");
            }
        }

        // ── Удалить неиспользуемые скрипты ────────────────────────────────────
        string[] scriptsToDelete = {
            "Assets/Scripts/PlayableAd/ChestTargetSelectionPatch.cs", // пустая заглушка
            "Assets/Scripts/PlayableAd/PowerLabelView.cs",            // не используется на сцене
        };

        foreach (var path in scriptsToDelete)
        {
            if (File.Exists(path))
            {
                AssetDatabase.DeleteAsset(path);
                Debug.Log($"[Cleanup] Deleted: {path}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("[Cleanup] Done!");
    }
}
