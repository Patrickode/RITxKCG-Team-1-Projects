using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMode
{
    None,
    Build,
    Destroy
}

public class ModeManager : MonoBehaviour
{
    [Tooltip("The prefab to use when building a snowman.\n\n" +
        "雪だるまを作るときに使用するプレハブ。")]
    [SerializeField] private Snowman buildSnowmanPrefab = null;

    public static PlayerMode CurrentMode { get; private set; } = PlayerMode.None;
    public static Action<Transform> HealSnowman;
    public static Action<Transform> RefundSnowman;

    private void Awake() { TargettingManager.TryCreateOrDestroy += OnTryCreateOrDestroy; }
    private void OnDestroy() { TargettingManager.TryCreateOrDestroy -= OnTryCreateOrDestroy; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { CurrentMode = PlayerMode.Build; }
        else if (Input.GetKeyDown(KeyCode.E)) { CurrentMode = PlayerMode.Destroy; }
        else if (Input.GetMouseButton(1)) { CurrentMode = PlayerMode.None; }
    }

    private void OnTryCreateOrDestroy(RaycastHit targetInfo)
    {
        switch (CurrentMode)
        {
            case PlayerMode.Build:
                TryBuild(targetInfo);
                break;
            case PlayerMode.Destroy:
                TryDestroy(targetInfo);
                break;
            default:
                break;
        }

        CurrentMode = PlayerMode.None;
    }

    private void TryBuild(RaycastHit targetInfo)
    {
        if (targetInfo.transform.CompareTag("Snowman"))
        {
            HealSnowman?.Invoke(targetInfo.transform);
        }
        else if (!targetInfo.transform.CompareTag("NoBuild") && Vector3.Dot(Vector3.up, targetInfo.normal) > 0.4f)
        {
            if (SnowManager.TrySpendSnow(buildSnowmanPrefab.CostToBuild))
            {
                GameObject newSnowman = Instantiate(
                    buildSnowmanPrefab.gameObject,
                    targetInfo.point,
                    Quaternion.identity
                );
                newSnowman.transform.position += targetInfo.normal * buildSnowmanPrefab.transform.localScale.y / 2;
                newSnowman.transform.up = targetInfo.normal;
            }
        }
    }

    private void TryDestroy(RaycastHit targetInfo)
    {
        if (targetInfo.transform.CompareTag("Snowman"))
        {
            Destroy(targetInfo.transform.gameObject);
            RefundSnowman?.Invoke(targetInfo.transform);
        }
    }
}
