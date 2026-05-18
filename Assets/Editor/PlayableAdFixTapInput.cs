using UnityEngine;
using UnityEditor;
using Gameplay;

public static class PlayableAdFixTapInput
{
    [MenuItem("Tools/Fix TapMoveInput GroundLayer")]
    public static void Fix()
    {
        var gameRoot = GameObject.Find("GameRoot");
        if (gameRoot == null) return;

        var tap = gameRoot.GetComponent<TapMoveInput>();
        if (tap == null) return;

        var so = new SerializedObject(tap);
        // Default layer = 0, LayerMask value = 1 << 0 = 1
        so.FindProperty("groundLayerMask").intValue = 1; // Default layer
        so.ApplyModifiedProperties();

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[FixTapInput] groundLayerMask set to Default layer");
    }
}
