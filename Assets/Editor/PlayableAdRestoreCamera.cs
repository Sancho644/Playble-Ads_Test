using UnityEngine;
using UnityEditor;
using Gameplay;

public static class PlayableAdRestoreCamera
{
    [MenuItem("Tools/Restore Original Camera + Fix Colliders")]
    public static void Restore()
    {
        // ── Вернуть оригинальную позицию камеры ───────────────────────────────
        var cam = Camera.main;
        if (cam != null)
        {
            cam.transform.position = new Vector3(15f, 13f, 14f);
            cam.transform.rotation = Quaternion.Euler(30f, 240f, 0f);
            cam.orthographic = true;
            cam.orthographicSize = 10f;
            Debug.Log("[Restore] Camera restored to original: pos=(15,13,14) rot=(30,240,0) orthoSize=10");
        }

        // ── Исправить BoxCollider на врагах — поднять центр ───────────────────
        FixCollider("Enemy_Weak");
        FixCollider("Enemy_Strong");
        FixCollider("Player");

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[Restore] Done!");
    }

    static void FixCollider(string name)
    {
        var go = GameObject.Find(name);
        if (go == null) return;

        var col = go.GetComponent<BoxCollider>();
        if (col == null) return;

        // Центрировать коллайдер по высоте модели (~2 units высота)
        col.center = new Vector3(0f, 1f, 0f);
        col.size = new Vector3(1f, 2f, 1f);
        Debug.Log($"[Restore] BoxCollider fixed on {name}: center=(0,1,0) size=(1,2,1)");
    }
}
