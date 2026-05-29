using UnityEngine;

/// <summary>Explicit states for the landing zone state machine.</summary>
public enum LandingZoneState { Idle, InProgress, Completed }

/// <summary>
/// Abstract base implementing IInteractable. Manages interaction state
/// explicitly (State Pattern) and delegates completion behavior to subclasses
/// via OnLandingComplete (Template Method Pattern).
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class LandingZone : MonoBehaviour, IInteractable
{
    [Header("Landing Zone Settings")]
    [SerializeField] protected float landingDuration = 5f;
    [SerializeField] protected string interactionLabel = "INTERACTING...";
    [SerializeField] protected string playerTag = "Player";
    [SerializeField] protected bool oneTimeUse = false;

    protected float currentTime = 0f;
    protected Transform currentInteractor;

    // State Pattern: one enum replaces two separate booleans
    protected LandingZoneState State = LandingZoneState.Idle;

    //IInteractable 
    public float InteractionDuration => landingDuration;
    public bool IsAvailable => !(oneTimeUse && State == LandingZoneState.Completed);

    public virtual void OnInteractionStart(GameObject interactor)
    {
        State = LandingZoneState.InProgress;
        currentInteractor = interactor.transform;
        currentTime = 0f;
        GameEvents.RaiseInteractionStarted(interactionLabel, currentInteractor.position);
        GameEvents.RaiseInteractionProgress(0f);
    }

    public virtual void OnInteractionComplete(GameObject interactor)
    {
        GameEvents.RaiseInteractionCompleted();
        OnLandingComplete();   // Template Method hook
        State = oneTimeUse ? LandingZoneState.Completed : LandingZoneState.Idle;
        currentTime = 0f;
    }

    public virtual void OnInteractionCancelled(GameObject interactor)
    {
        if (State == LandingZoneState.InProgress)
            GameEvents.RaiseInteractionCancelled();

        State = LandingZoneState.Idle;
        currentTime = 0f;
        currentInteractor = null;
    }

    //Template Method hook
    protected abstract void OnLandingComplete();

    // Unity trigger detection drives state transitions
    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsAvailable) return;
        if (!other.CompareTag(playerTag)) return;
        if (State == LandingZoneState.InProgress) return;
        OnInteractionStart(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (State != LandingZoneState.InProgress) return;
        OnInteractionCancelled(other.gameObject);
    }

    void Update()
    {
        if (State != LandingZoneState.InProgress) return;

        currentTime += Time.deltaTime;
        GameEvents.RaiseInteractionProgress(Mathf.Clamp01(currentTime / landingDuration));

        if (currentTime >= landingDuration && currentInteractor != null)
            OnInteractionComplete(currentInteractor.gameObject);
    }
}