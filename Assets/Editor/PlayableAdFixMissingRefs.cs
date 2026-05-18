using UnityEngine;
using UnityEditor;
using Gameplay;
using PlayableAd;
using View;

public static class PlayableAdFixMissingRefs
{
    [MenuItem("Tools/Fix Missing References")]
    public static void Fix()
    {
        Debug.Log("[FixRefs] Starting...");

        var gameRoot = GameObject.Find("GameRoot");
        var player = GameObject.Find("Player");
        var weakEnemy = GameObject.Find("Enemy_Weak");
        var strongEnemy = GameObject.Find("Enemy_Strong");
        var chest = GameObject.Find("InteractableObject");

        if (gameRoot != null)
        {
            // LevelVictoryController: enemyVisibilities
            var vc = gameRoot.GetComponent<LevelVictoryController>();
            if (vc != null)
            {
                var vcSO = new SerializedObject(vc);
                var arr = vcSO.FindProperty("enemyVisibilities");
                arr.arraySize = 2;
                if (weakEnemy != null)
                    arr.GetArrayElementAtIndex(0).objectReferenceValue = weakEnemy.GetComponent<VisibilityComponent>();
                if (strongEnemy != null)
                    arr.GetArrayElementAtIndex(1).objectReferenceValue = strongEnemy.GetComponent<VisibilityComponent>();
                vcSO.ApplyModifiedProperties();
                Debug.Log("[FixRefs] LevelVictoryController.enemyVisibilities assigned");
            }

            // PlayableAdLevelController: victoryController
            var lc = gameRoot.GetComponent<PlayableAdLevelController>();
            if (lc != null)
            {
                var lcSO = new SerializedObject(lc);
                var vc2 = gameRoot.GetComponent<LevelVictoryController>();
                if (vc2 != null)
                    lcSO.FindProperty("victoryController").objectReferenceValue = vc2;
                lcSO.ApplyModifiedProperties();
                Debug.Log("[FixRefs] PlayableAdLevelController.victoryController assigned");
            }
        }

        // ChestBonusDisplay
        if (chest != null)
        {
            var cbd = chest.GetComponent<ChestBonusDisplay>();
            if (cbd == null)
            {
                cbd = chest.AddComponent<ChestBonusDisplay>();
                var cbdSO = new SerializedObject(cbd);
                cbdSO.FindProperty("bonusValue").intValue = 5;
                cbdSO.ApplyModifiedProperties();
                Debug.Log("[FixRefs] ChestBonusDisplay added");
            }

            // TargetSelectionView.entityIdentity
            var tsv = chest.GetComponent<TargetSelectionView>();
            if (tsv != null)
            {
                var tsvSO = new SerializedObject(tsv);
                tsvSO.FindProperty("entityIdentity").objectReferenceValue = chest.GetComponent<EntityIdentity>();
                tsvSO.ApplyModifiedProperties();
                Debug.Log("[FixRefs] Chest TargetSelectionView.entityIdentity fixed");
            }
        }

        // EventSystem
        if (GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var esGO = new GameObject("EventSystem");
            esGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            Debug.Log("[FixRefs] EventSystem created");
        }

        // Enemy_Strong TargetSelectionView.playerTarget
        if (strongEnemy != null && player != null)
        {
            var tsv = strongEnemy.GetComponent<TargetSelectionView>();
            if (tsv != null)
            {
                var tsvSO = new SerializedObject(tsv);
                tsvSO.FindProperty("playerTarget").objectReferenceValue = player.GetComponent<TargetComponent>();
                tsvSO.ApplyModifiedProperties();
                Debug.Log("[FixRefs] Enemy_Strong playerTarget fixed");
            }
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[FixRefs] Done!");
    }
}
