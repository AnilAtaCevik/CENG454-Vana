using System.Collections;
using UnityEngine;

/// <summary>
/// Place this in the scene at each checkpoint location.
/// Configure enemy groups, hint messages, and index in Inspector.
/// One-shot trigger — once activated it won't fire again.
/// </summary>
[RequireComponent(typeof(Collider))]
public class CheckpointZone : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    [SerializeField] private int checkpointIndex = 0;

    [Header("Enemy Groups To Clear")]
    [Tooltip("Drag parent GameObjects of enemy groups here.")]
    [SerializeField] private GameObject[] enemyGroupsToClear;

    [Header("Hint Messages")]
    [Tooltip("Messages shown on HUD feedback panel one by one after checkpoint.")]
    [SerializeField] private string[] hintMessages;
    [SerializeField] private float messageDuration = 3f;
    [SerializeField] private FeedbackSeverity messageSeverity = FeedbackSeverity.Info;

    [Header("Visuals")]
    [SerializeField] private GameObject activeVisual;
    [SerializeField] private GameObject activatedVisual;

    private bool _activated = false;

    void Start()
    {
        if (CheckpointManager.ActivatedCheckpoints.Contains(checkpointIndex))
        {
            _activated = true;
            ClearEnemyGroups();
            SwapVisual();
            if (hintMessages != null && hintMessages.Length > 0)
                StartCoroutine(ShowMessages());
            Debug.Log($"[CheckpointZone] {checkpointIndex} auto-restored on scene load.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"TRIGGER HIT BY: {other.gameObject.name} tag: {other.tag}");
        if (_activated) return;
        if (!other.CompareTag("Player")) return;

        _activated = true;

        // No health/fuel reading needed — CheckpointRestorer lets
        // them reset naturally from their own Start() methods
        CheckpointManager.SaveCheckpoint(
            checkpointIndex,
            other.transform.position,
            100f, 100f,  // placeholders, not used by restorer
            100f, 100f,  // placeholders, not used by restorer
            PassengerState.IsCarryingPassengers
        );

        ClearEnemyGroups();
        SwapVisual();

        GameEvents.RaiseFeedback("Checkpoint reached!", FeedbackSeverity.Info);

        if (hintMessages != null && hintMessages.Length > 0)
            StartCoroutine(ShowMessages());

        Debug.Log($"[CheckpointZone] Checkpoint {checkpointIndex} activated.");
    }

    private void ClearEnemyGroups()
    {
        if (enemyGroupsToClear == null) return;
        foreach (var group in enemyGroupsToClear)
            if (group != null) group.SetActive(false);
    }

    private void SwapVisual()
    {
        if (activeVisual != null) activeVisual.SetActive(false);
        if (activatedVisual != null) activatedVisual.SetActive(true);
    }

    private IEnumerator ShowMessages()
    {
        yield return new WaitForSeconds(messageDuration + 0.5f);

        foreach (string msg in hintMessages)
        {
            if (string.IsNullOrEmpty(msg)) continue;
            GameEvents.RaiseFeedback(msg, messageSeverity);
            yield return new WaitForSeconds(messageDuration);
        }
    }
}