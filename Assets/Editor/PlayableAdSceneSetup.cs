using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using Gameplay;
using PlayableAd;
using View;

/// <summary>
/// Editor-скрипт для настройки сцены Playable Ad.
/// Запускается один раз через меню Tools > Setup Playable Ad Scene.
/// </summary>
public static class PlayableAdSceneSetup
{
    [MenuItem("Tools/Setup Playable Ad Scene")]
    public static void SetupScene()
    {
        Debug.Log("[PlayableAdSetup] Starting scene setup...");

        // 1. Настроить позиции и силу существующих объектов
        SetupPlayer();
        SetupExistingEnemy();
        SetupChest();

        // 2. Создать второго врага (дублировать первого)
        CreateSecondEnemy();

        // 3. Настроить камеру
        SetupCamera();

        // 4. Создать UI Canvas с числами силы
        CreatePowerLabelsCanvas();

        // 5. Создать Floating Text Spawner
        CreateFloatingTextSpawner();

        // 6. Создать Hint Arrow
        CreateHintArrow();

        // 7. Создать экран победы
        CreateVictoryScreen();

        // 8. Настроить GameRoot компоненты
        SetupGameRoot();

        // 9. Добавить AdLifecycleController
        SetupAdLifecycle();

        // 10. Настроить MapBoundsConstraint
        SetupMapBounds();

        // Сохранить сцену
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[PlayableAdSetup] Scene setup complete!");
    }

    // ─── Player ───────────────────────────────────────────────────────────────

    static void SetupPlayer()
    {
        var player = GameObject.Find("Player");
        if (player == null) { Debug.LogError("Player not found!"); return; }

        // Позиция: левый нижний угол
        player.transform.position = new Vector3(-3f, 0f, -3f);

        // Сила игрока = 3
        var power = player.GetComponent<ActorPower>();
        if (power != null) power.SetValue(3);

        Debug.Log("[PlayableAdSetup] Player configured: pos=-3,0,-3 power=3");
    }

    // ─── Existing Enemy (слабый, сила=2) ─────────────────────────────────────

    static void SetupExistingEnemy()
    {
        var enemy = GameObject.Find("Enemy");
        if (enemy == null) { Debug.LogError("Enemy not found!"); return; }

        // Позиция: правый нижний угол
        enemy.transform.position = new Vector3(2f, 0f, 1f);
        enemy.name = "Enemy_Weak";

        // Сила = 2
        var power = enemy.GetComponent<ActorPower>();
        if (power == null) power = enemy.AddComponent<ActorPower>();
        power.SetValue(2);

        // Убедиться что есть RemovalRequestComponent
        if (enemy.GetComponent<RemovalRequestComponent>() == null)
            enemy.AddComponent<RemovalRequestComponent>();

        Debug.Log("[PlayableAdSetup] Enemy_Weak configured: pos=2,0,1 power=2");
    }

    // ─── Chest (бонус +5) ─────────────────────────────────────────────────────

    static void SetupChest()
    {
        var chest = GameObject.Find("InteractableObject");
        if (chest == null) { Debug.LogError("InteractableObject not found!"); return; }

        // Позиция: центр карты
        chest.transform.position = new Vector3(0f, 0f, 3f);

        // Добавить коллайдер если нет
        if (chest.GetComponent<Collider>() == null)
        {
            var col = chest.AddComponent<BoxCollider>();
            col.size = new Vector3(1f, 1f, 1f);
        }

        // Добавить RemovalRequestComponent если нет
        if (chest.GetComponent<RemovalRequestComponent>() == null)
            chest.AddComponent<RemovalRequestComponent>();

        Debug.Log("[PlayableAdSetup] Chest configured: pos=0,0,3");
    }

    // ─── Second Enemy (сильный, сила=7) ──────────────────────────────────────

    static void CreateSecondEnemy()
    {
        // Проверить, не создан ли уже
        if (GameObject.Find("Enemy_Strong") != null)
        {
            Debug.Log("[PlayableAdSetup] Enemy_Strong already exists, skipping.");
            return;
        }

        var weakEnemy = GameObject.Find("Enemy_Weak");
        if (weakEnemy == null) weakEnemy = GameObject.Find("Enemy");
        if (weakEnemy == null) { Debug.LogError("Cannot find enemy to duplicate!"); return; }

        // Дублировать слабого врага
        var strongEnemy = Object.Instantiate(weakEnemy);
        strongEnemy.name = "Enemy_Strong";
        strongEnemy.transform.position = new Vector3(-1f, 0f, 6f);

        // Сила = 7
        var power = strongEnemy.GetComponent<ActorPower>();
        if (power == null) power = strongEnemy.AddComponent<ActorPower>();
        power.SetValue(7);

        // Сбросить состояние компонентов
        var visibility = strongEnemy.GetComponent<VisibilityComponent>();
        if (visibility != null) visibility.Show();

        var removal = strongEnemy.GetComponent<RemovalRequestComponent>();
        if (removal != null) removal.Clear();

        Debug.Log("[PlayableAdSetup] Enemy_Strong created: pos=-1,0,6 power=7");
    }

    // ─── Camera ───────────────────────────────────────────────────────────────

    static void SetupCamera()
    {
        var cam = Camera.main;
        if (cam == null) return;

        // Изометрическая позиция для portrait-вида
        cam.transform.position = new Vector3(0f, 14f, -8f);
        cam.transform.rotation = Quaternion.Euler(55f, 0f, 0f);
        cam.orthographic = true;
        cam.orthographicSize = 8f;

        Debug.Log("[PlayableAdSetup] Camera configured for portrait isometric view");
    }

    // ─── Power Labels Canvas ──────────────────────────────────────────────────

    static void CreatePowerLabelsCanvas()
    {
        if (GameObject.Find("PowerLabelsCanvas") != null)
        {
            Debug.Log("[PlayableAdSetup] PowerLabelsCanvas already exists, skipping.");
            return;
        }

        // Создать Canvas (Screen Space Overlay для UI)
        var canvasGO = new GameObject("PowerLabelsCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Создать лейблы для каждого юнита
        CreatePowerLabelUI(canvasGO.transform, "Player", new Color(0.2f, 0.6f, 1f));
        CreatePowerLabelUI(canvasGO.transform, "Enemy_Weak", new Color(1f, 0.25f, 0.25f));
        CreatePowerLabelUI(canvasGO.transform, "Enemy_Strong", new Color(1f, 0.25f, 0.25f));
        CreatePowerLabelUI(canvasGO.transform, "InteractableObject", new Color(0.2f, 0.9f, 0.3f));

        Debug.Log("[PlayableAdSetup] PowerLabelsCanvas created");
    }

    static void CreatePowerLabelUI(Transform parent, string targetName, Color color)
    {
        var labelGO = new GameObject($"PowerLabel_{targetName}");
        labelGO.transform.SetParent(parent, false);

        var tracker = labelGO.AddComponent<WorldSpacePowerLabel>();
        tracker.TargetName = targetName;
        tracker.LabelColor = color;

        Debug.Log($"[PlayableAdSetup] PowerLabel created for {targetName}");
    }

    // ─── Floating Text Spawner ────────────────────────────────────────────────

    static void CreateFloatingTextSpawner()
    {
        if (GameObject.Find("FloatingTextSpawner") != null) return;

        var go = new GameObject("FloatingTextSpawner");
        var spawner = go.AddComponent<FloatingTextSpawner>();

        // Создать prefab для floating text
        var prefabGO = new GameObject("FloatingTextPrefab");
        prefabGO.transform.SetParent(go.transform);

        var tmp = prefabGO.AddComponent<TextMeshPro>();
        tmp.fontSize = 5f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.text = "+0";

        prefabGO.AddComponent<FloatingTextItem>();

        // Назначить prefab
        var spawnerSO = new SerializedObject(spawner);
        spawnerSO.FindProperty("floatingTextPrefab").objectReferenceValue = prefabGO;
        spawnerSO.ApplyModifiedProperties();

        // Скрыть prefab
        prefabGO.SetActive(false);

        Debug.Log("[PlayableAdSetup] FloatingTextSpawner created");
    }

    // ─── Hint Arrow ───────────────────────────────────────────────────────────

    static void CreateHintArrow()
    {
        if (GameObject.Find("HintArrow") != null) return;

        var go = new GameObject("HintArrow");

        // Создать визуал стрелки из примитивов
        var arrowRoot = new GameObject("ArrowRoot");
        arrowRoot.transform.SetParent(go.transform);

        // Тело стрелки (цилиндр)
        var body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        body.name = "ArrowBody";
        body.transform.SetParent(arrowRoot.transform);
        body.transform.localPosition = new Vector3(0f, 0f, 0f);
        body.transform.localScale = new Vector3(0.1f, 0.3f, 0.1f);
        Object.DestroyImmediate(body.GetComponent<Collider>());

        // Наконечник (конус из сферы)
        var tip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        tip.name = "ArrowTip";
        tip.transform.SetParent(arrowRoot.transform);
        tip.transform.localPosition = new Vector3(0f, -0.4f, 0f);
        tip.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        Object.DestroyImmediate(tip.GetComponent<Collider>());

        // Материал — жёлтый
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = new Color(1f, 0.9f, 0f);
        body.GetComponent<Renderer>().material = mat;
        tip.GetComponent<Renderer>().material = mat;

        var hint = go.AddComponent<HintArrowView>();
        var hintSO = new SerializedObject(hint);
        hintSO.FindProperty("arrowRoot").objectReferenceValue = arrowRoot;
        hintSO.FindProperty("showDuration").floatValue = 5f;
        hintSO.FindProperty("offsetY").floatValue = 2.5f;
        hintSO.ApplyModifiedProperties();

        Debug.Log("[PlayableAdSetup] HintArrow created");
    }

    // ─── Victory Screen ───────────────────────────────────────────────────────

    static void CreateVictoryScreen()
    {
        if (GameObject.Find("VictoryCanvas") != null) return;

        var canvasGO = new GameObject("VictoryCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(390f, 844f);
        scaler.matchWidthOrHeight = 0.5f;

        canvasGO.AddComponent<GraphicRaycaster>();

        // Панель победы
        var panelGO = new GameObject("VictoryPanel");
        panelGO.transform.SetParent(canvasGO.transform, false);

        var panelRect = panelGO.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        var panelImage = panelGO.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.75f);

        // Текст "YOU WIN!"
        var titleGO = new GameObject("TitleText");
        titleGO.transform.SetParent(panelGO.transform, false);
        var titleRect = titleGO.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.1f, 0.55f);
        titleRect.anchorMax = new Vector2(0.9f, 0.75f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        var titleTMP = titleGO.AddComponent<TextMeshProUGUI>();
        titleTMP.text = "YOU WIN!";
        titleTMP.fontSize = 72f;
        titleTMP.fontStyle = FontStyles.Bold;
        titleTMP.alignment = TextAlignmentOptions.Center;
        titleTMP.color = new Color(1f, 0.85f, 0.1f);

        // Кнопка CTA
        var btnGO = new GameObject("CTAButton");
        btnGO.transform.SetParent(panelGO.transform, false);
        var btnRect = btnGO.AddComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.15f, 0.3f);
        btnRect.anchorMax = new Vector2(0.85f, 0.48f);
        btnRect.offsetMin = Vector2.zero;
        btnRect.offsetMax = Vector2.zero;

        var btnImage = btnGO.AddComponent<Image>();
        btnImage.color = new Color(0.1f, 0.8f, 0.2f);

        var btn = btnGO.AddComponent<Button>();
        var btnColors = btn.colors;
        btnColors.highlightedColor = new Color(0.15f, 1f, 0.3f);
        btnColors.pressedColor = new Color(0.05f, 0.6f, 0.15f);
        btn.colors = btnColors;

        var btnTextGO = new GameObject("ButtonText");
        btnTextGO.transform.SetParent(btnGO.transform, false);
        var btnTextRect = btnTextGO.AddComponent<RectTransform>();
        btnTextRect.anchorMin = Vector2.zero;
        btnTextRect.anchorMax = Vector2.one;
        btnTextRect.offsetMin = Vector2.zero;
        btnTextRect.offsetMax = Vector2.zero;

        var btnTMP = btnTextGO.AddComponent<TextMeshProUGUI>();
        btnTMP.text = "PLAY NOW";
        btnTMP.fontSize = 42f;
        btnTMP.fontStyle = FontStyles.Bold;
        btnTMP.alignment = TextAlignmentOptions.Center;
        btnTMP.color = Color.white;

        // Скрыть панель изначально
        panelGO.SetActive(false);

        // Добавить LevelVictoryController на GameRoot
        var gameRoot = GameObject.Find("GameRoot");
        if (gameRoot != null)
        {
            var existing = gameRoot.GetComponent<LevelVictoryController>();
            if (existing == null)
            {
                var vc = gameRoot.AddComponent<LevelVictoryController>();
                var vcSO = new SerializedObject(vc);

                // Назначить ссылки
                vcSO.FindProperty("victoryPanel").objectReferenceValue = panelGO;
                vcSO.FindProperty("victoryText").objectReferenceValue = titleTMP;
                vcSO.FindProperty("ctaButton").objectReferenceValue = btn;
                vcSO.FindProperty("ctaButtonText").objectReferenceValue = btnTMP;

                // Враги
                var weakEnemy = GameObject.Find("Enemy_Weak");
                var strongEnemy = GameObject.Find("Enemy_Strong");
                var enemyVisArr = vcSO.FindProperty("enemyVisibilities");
                int count = 0;
                if (weakEnemy != null) count++;
                if (strongEnemy != null) count++;
                enemyVisArr.arraySize = count;
                int idx = 0;
                if (weakEnemy != null)
                {
                    var vis = weakEnemy.GetComponent<VisibilityComponent>();
                    if (vis != null) enemyVisArr.GetArrayElementAtIndex(idx++).objectReferenceValue = vis;
                }
                if (strongEnemy != null)
                {
                    var vis = strongEnemy.GetComponent<VisibilityComponent>();
                    if (vis != null) enemyVisArr.GetArrayElementAtIndex(idx++).objectReferenceValue = vis;
                }

                vcSO.ApplyModifiedProperties();
            }
        }

        Debug.Log("[PlayableAdSetup] VictoryCanvas created");
    }

    // ─── GameRoot ─────────────────────────────────────────────────────────────

    static void SetupGameRoot()
    {
        var gameRoot = GameObject.Find("GameRoot");
        if (gameRoot == null) return;

        var player = GameObject.Find("Player");
        var weakEnemy = GameObject.Find("Enemy_Weak");
        var strongEnemy = GameObject.Find("Enemy_Strong");
        var chest = GameObject.Find("InteractableObject");

        // VisualFeedbackHandler
        if (gameRoot.GetComponent<VisualFeedbackHandler>() == null)
        {
            var vfh = gameRoot.AddComponent<VisualFeedbackHandler>();
            var vfhSO = new SerializedObject(vfh);
            if (player != null)
            {
                vfhSO.FindProperty("visualFeedback").objectReferenceValue =
                    player.GetComponent<VisualFeedbackEventComponent>();
                vfhSO.FindProperty("playerPower").objectReferenceValue =
                    player.GetComponent<ActorPower>();
                vfhSO.FindProperty("combatResult").objectReferenceValue =
                    player.GetComponent<CombatResultComponent>();
            }
            vfhSO.ApplyModifiedProperties();
        }

        // ChestInteractionHandler
        if (gameRoot.GetComponent<ChestInteractionHandler>() == null && chest != null && player != null)
        {
            var cih = gameRoot.AddComponent<ChestInteractionHandler>();
            var cihSO = new SerializedObject(cih);
            cihSO.FindProperty("playerIdentity").objectReferenceValue = player.GetComponent<EntityIdentity>();
            cihSO.FindProperty("playerPower").objectReferenceValue = player.GetComponent<ActorPower>();
            cihSO.FindProperty("playerTarget").objectReferenceValue = player.GetComponent<TargetComponent>();
            cihSO.FindProperty("playerMoveTarget").objectReferenceValue = player.GetComponent<MoveTargetComponent>();
            cihSO.FindProperty("chestIdentity").objectReferenceValue = chest.GetComponent<EntityIdentity>();
            cihSO.FindProperty("chestBonus").intValue = 5;
            cihSO.FindProperty("chestVisibility").objectReferenceValue = chest.GetComponent<VisibilityComponent>();
            cihSO.FindProperty("chestRemoval").objectReferenceValue = chest.GetComponent<RemovalRequestComponent>();
            cihSO.FindProperty("interactRange").floatValue = 1.5f;
            cihSO.ApplyModifiedProperties();
        }

        // PlayableAdLevelController
        if (gameRoot.GetComponent<PlayableAdLevelController>() == null)
        {
            var lc = gameRoot.AddComponent<PlayableAdLevelController>();
            var lcSO = new SerializedObject(lc);
            if (player != null)
            {
                lcSO.FindProperty("playerPower").objectReferenceValue = player.GetComponent<ActorPower>();
                lcSO.FindProperty("playerTarget").objectReferenceValue = player.GetComponent<TargetComponent>();
            }
            if (weakEnemy != null)
                lcSO.FindProperty("weakEnemy").objectReferenceValue = weakEnemy.GetComponent<EntityIdentity>();
            if (strongEnemy != null)
                lcSO.FindProperty("strongEnemy").objectReferenceValue = strongEnemy.GetComponent<EntityIdentity>();
            if (chest != null)
                lcSO.FindProperty("chestIdentity").objectReferenceValue = chest.GetComponent<EntityIdentity>();

            var hintArrow = GameObject.Find("HintArrow");
            if (hintArrow != null)
                lcSO.FindProperty("hintArrow").objectReferenceValue = hintArrow.GetComponent<HintArrowView>();

            lcSO.ApplyModifiedProperties();
        }

        Debug.Log("[PlayableAdSetup] GameRoot components configured");
    }

    // ─── AdLifecycle ──────────────────────────────────────────────────────────

    static void SetupAdLifecycle()
    {
        var gameRoot = GameObject.Find("GameRoot");
        if (gameRoot == null) return;

        if (gameRoot.GetComponent<AdLifecycleController>() == null)
            gameRoot.AddComponent<AdLifecycleController>();

        Debug.Log("[PlayableAdSetup] AdLifecycleController added");
    }

    // ─── MapBounds ────────────────────────────────────────────────────────────

    static void SetupMapBounds()
    {
        var gameRoot = GameObject.Find("GameRoot");
        if (gameRoot == null) return;

        if (gameRoot.GetComponent<MapBoundsConstraint>() == null)
        {
            var mbc = gameRoot.AddComponent<MapBoundsConstraint>();
            var mbcSO = new SerializedObject(mbc);

            var player = GameObject.Find("Player");
            if (player != null)
            {
                mbcSO.FindProperty("playerTransform").objectReferenceValue = player.transform;
                mbcSO.FindProperty("moveTarget").objectReferenceValue = player.GetComponent<MoveTargetComponent>();
            }

            mbcSO.FindProperty("minX").floatValue = -8f;
            mbcSO.FindProperty("maxX").floatValue = 8f;
            mbcSO.FindProperty("minZ").floatValue = -8f;
            mbcSO.FindProperty("maxZ").floatValue = 8f;
            mbcSO.ApplyModifiedProperties();
        }

        Debug.Log("[PlayableAdSetup] MapBoundsConstraint configured");
    }
}
