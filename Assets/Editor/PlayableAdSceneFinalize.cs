using UnityEngine;
using UnityEditor;
using Gameplay;
using PlayableAd;
using View;

public static class PlayableAdSceneFinalize
{
    [MenuItem("Tools/Finalize Playable Ad Scene")]
    public static void Finalize()
    {
        Debug.Log("[PlayableAdFinalize] Starting finalization...");

        FixChestActorPower();
        FixEnemyStrongTargetView();
        FixTargetIndicatorLineRenderer();
        FixMovementVisualState();
        FixEnemyStrongDeathView();
        SetupLevelControllerHintTarget();

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[PlayableAdFinalize] Finalization complete!");
    }

    static void FixChestActorPower()
    {
        var chest = GameObject.Find("InteractableObject");
        if (chest == null) return;

        if (chest.GetComponent<ChestBonusDisplay>() == null)
        {
            var cbd = chest.AddComponent<ChestBonusDisplay>();
            var cbdSO = new SerializedObject(cbd);
            cbdSO.FindProperty("bonusValue").intValue = 5;
            cbdSO.ApplyModifiedProperties();
        }

        Debug.Log("[PlayableAdFinalize] Chest ActorPower fixed");
    }

    static void FixEnemyStrongTargetView()
    {
        var strongEnemy = GameObject.Find("Enemy_Strong");
        if (strongEnemy == null) return;

        var player = GameObject.Find("Player");
        if (player == null) return;

        var playerTarget = player.GetComponent<TargetComponent>();

        var tsv = strongEnemy.GetComponent<TargetSelectionView>();
        if (tsv != null)
        {
            var tsvSO = new SerializedObject(tsv);
            tsvSO.FindProperty("playerTarget").objectReferenceValue = playerTarget;
            tsvSO.FindProperty("entityIdentity").objectReferenceValue = strongEnemy.GetComponent<EntityIdentity>();
            var marker = strongEnemy.transform.Find("SelectionMarker");
            if (marker != null)
                tsvSO.FindProperty("selectedMarker").objectReferenceValue = marker.gameObject;
            tsvSO.ApplyModifiedProperties();
        }

        var deathView = strongEnemy.GetComponent<EnemyDeathAnimationView>();
        if (deathView != null)
        {
            var dvSO = new SerializedObject(deathView);
            dvSO.FindProperty("removalRequest").objectReferenceValue = strongEnemy.GetComponent<RemovalRequestComponent>();
            dvSO.FindProperty("visibility").objectReferenceValue = strongEnemy.GetComponent<VisibilityComponent>();
            dvSO.ApplyModifiedProperties();
        }

        var visView = strongEnemy.GetComponent<VisibilityView>();
        if (visView != null)
        {
            var vvSO = new SerializedObject(visView);
            vvSO.FindProperty("visibility").objectReferenceValue = strongEnemy.GetComponent<VisibilityComponent>();
            var marker = strongEnemy.transform.Find("SelectionMarker");
            if (marker != null)
                vvSO.FindProperty("contentRoot").objectReferenceValue = marker.gameObject;
            vvSO.ApplyModifiedProperties();
        }

        Debug.Log("[PlayableAdFinalize] Enemy_Strong TargetSelectionView fixed");
    }

    static void FixTargetIndicatorLineRenderer()
    {
        var ti = GameObject.Find("TargetIndicator");
        if (ti == null) return;

        var lr = ti.GetComponent<LineRenderer>();
        if (lr == null) return;

        lr.startWidth = 0.08f;
        lr.endWidth = 0.04f;
        lr.startColor = new Color(1f, 0.9f, 0.2f, 0.8f);
        lr.endColor = new Color(1f, 0.9f, 0.2f, 0.2f);
        lr.useWorldSpace = true;
        lr.positionCount = 2;

        var mat = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
        if (mat.shader == null || mat.shader.name == "Hidden/InternalErrorShader")
            mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = new Color(1f, 0.9f, 0.2f, 0.8f);
        lr.material = mat;

        Debug.Log("[PlayableAdFinalize] TargetIndicator LineRenderer configured");
    }

    static void FixMovementVisualState()
    {
        var gameRoot = GameObject.Find("GameRoot");
        if (gameRoot == null) return;

        var player = GameObject.Find("Player");
        if (player == null) return;

        var runner = gameRoot.GetComponent<PlayerMovementRunner>();
        if (runner == null) return;

        var mvs = player.GetComponent<MovementVisualStateComponent>();
        if (mvs == null) return;

        var runnerSO = new SerializedObject(runner);
        runnerSO.FindProperty("movementVisualState").objectReferenceValue = mvs;
        runnerSO.ApplyModifiedProperties();

        Debug.Log("[PlayableAdFinalize] MovementVisualState linked to PlayerMovementRunner");
    }

    static void FixEnemyStrongDeathView()
    {
        var strongEnemy = GameObject.Find("Enemy_Strong");
        if (strongEnemy == null) return;

        var deathView = strongEnemy.GetComponent<EnemyDeathAnimationView>();
        if (deathView == null) return;

        var animator = strongEnemy.GetComponentInChildren<Animator>(true);
        if (animator == null) return;

        var dvSO = new SerializedObject(deathView);
        dvSO.FindProperty("animator").objectReferenceValue = animator;
        dvSO.ApplyModifiedProperties();

        Debug.Log("[PlayableAdFinalize] Enemy_Strong EnemyDeathAnimationView animator linked");
    }

    static void SetupLevelControllerHintTarget()
    {
        var gameRoot = GameObject.Find("GameRoot");
        if (gameRoot == null) return;

        var lc = gameRoot.GetComponent<PlayableAdLevelController>();
        if (lc == null) return;

        var hintArrow = GameObject.Find("HintArrow");
        if (hintArrow == null) return;

        var chestHandler = gameRoot.GetComponent<ChestInteractionHandler>();

        var lcSO = new SerializedObject(lc);
        lcSO.FindProperty("hintArrow").objectReferenceValue = hintArrow.GetComponent<HintArrowView>();
        if (chestHandler != null)
            lcSO.FindProperty("chestHandler").objectReferenceValue = chestHandler;
        lcSO.ApplyModifiedProperties();

        var weakEnemy = GameObject.Find("Enemy_Weak");
        if (weakEnemy != null)
        {
            var hintView = hintArrow.GetComponent<HintArrowView>();
            if (hintView != null)
            {
                var hvSO = new SerializedObject(hintView);
                hvSO.FindProperty("target").objectReferenceValue = weakEnemy.transform;
                hvSO.ApplyModifiedProperties();
            }
        }

        Debug.Log("[PlayableAdFinalize] LevelController hint target configured");
    }
}
