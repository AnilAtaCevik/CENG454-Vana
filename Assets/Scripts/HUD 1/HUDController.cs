using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    // ========================
    // HEALTH
    // ========================
    [Header("Health")]
    [SerializeField] private Image healthFill;
    [SerializeField] private TMP_Text healthText;

    // ========================
    // FUEL
    // ========================
    [Header("Fuel")]
    [SerializeField] private Image fuelFill;
    [SerializeField] private TMP_Text fuelText;

    // ========================
    // WEAPON STATUS
    // ========================
    [Header("Weapon Status")]
    [SerializeField] private TMP_Text missileAmmoText;
    [SerializeField] private TMP_Text minigunStatusText;
    [SerializeField] private TMP_Text flareText;

    // ========================
    // DAMAGE FLASH
    // ========================
    [Header("Damage Flash")]
    [SerializeField] private Image damageFlashImage;
    [SerializeField] private float flashDuration = 0.3f;
    [SerializeField] private float flashMaxAlpha = 0.4f;

    // ========================
    // FEEDBACK LABEL
    // ========================
    [Header("Feedback Label")]
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private float feedbackDuration = 3f;
    [SerializeField] private Color infoColor = Color.white;
    [SerializeField] private Color warningColor = new Color(1f, 0.7f, 0f);
    [SerializeField] private Color criticalColor = Color.red;

    // ========================
    // INTERACTION PROGRESS
    // ========================
    [Header("Interaction Progress")]
    [SerializeField] private GameObject interactionProgressRoot;
    [SerializeField] private Image interactionProgressFill;
    [SerializeField] private TMP_Text interactionProgressText;
    [SerializeField] private TMP_Text interactionProgressLabel;

    // ========================
    // MISSION TASKS
    // ========================
    [Header("Mission Tasks")]
    [SerializeField] private TMP_Text missionTitleText;
    [SerializeField] private Transform objectivesContainer;
    [SerializeField] private GameObject objectivePrefab;

    private Coroutine flashCoroutine;
    private Coroutine feedbackCoroutine;

    // ========================
    // SUBSCRIBE / UNSUBSCRIBE
    // ========================

    void OnEnable()
    {
        GameEvents.OnHealthChanged += HandleHealthChanged;
        GameEvents.OnDamageTaken += HandleDamageTaken;
        GameEvents.OnFuelChanged += HandleFuelChanged;
        GameEvents.OnFuelEmpty += HandleFuelEmpty;
        GameEvents.OnFeedbackRaised += HandleFeedback;
        GameEvents.OnInteractionStarted += HandleInteractionStarted;
        GameEvents.OnInteractionProgress += HandleInteractionProgress;
        GameEvents.OnInteractionCompleted += HandleInteractionCompleted;
        GameEvents.OnInteractionCancelled += HandleInteractionCancelled;
        GameEvents.OnMissionStarted += HandleMissionStarted;
        GameEvents.OnObjectiveAdded += HandleObjectiveAdded;
        GameEvents.OnObjectiveCompleted += HandleObjectiveCompleted;

        WeaponEvents.OnMissileAmmoChanged += HandleMissileAmmo;
        WeaponEvents.OnMinigunOverheated += HandleMinigunOverheated;
        WeaponEvents.OnMinigunCooldownFinished += HandleMinigunCooled;
        // TODO: uncomment when flare events added to WeaponEvents
        // WeaponEvents.OnFlareAmmoChanged += HandleFlareAmmo;

    }

    void OnDisable()
    {
        GameEvents.OnHealthChanged -= HandleHealthChanged;
        GameEvents.OnDamageTaken -= HandleDamageTaken;
        GameEvents.OnFuelChanged -= HandleFuelChanged;
        GameEvents.OnFuelEmpty -= HandleFuelEmpty;
        GameEvents.OnFeedbackRaised -= HandleFeedback;
        GameEvents.OnInteractionStarted -= HandleInteractionStarted;
        GameEvents.OnInteractionProgress -= HandleInteractionProgress;
        GameEvents.OnInteractionCompleted -= HandleInteractionCompleted;
        GameEvents.OnInteractionCancelled -= HandleInteractionCancelled;
        GameEvents.OnMissionStarted -= HandleMissionStarted;
        GameEvents.OnObjectiveAdded -= HandleObjectiveAdded;
        GameEvents.OnObjectiveCompleted -= HandleObjectiveCompleted;

        WeaponEvents.OnMissileAmmoChanged -= HandleMissileAmmo;
        WeaponEvents.OnMinigunOverheated -= HandleMinigunOverheated;
        WeaponEvents.OnMinigunCooldownFinished -= HandleMinigunCooled;
        // WeaponEvents.OnFlareAmmoChanged -= HandleFlareAmmo;
    }

    void Start()
    {
        if (damageFlashImage != null)
            damageFlashImage.gameObject.SetActive(false);    // CHANGED

        if (interactionProgressRoot != null)
            interactionProgressRoot.SetActive(false);

        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);

        if (feedbackText != null)
            feedbackText.text = string.Empty;

        if (flareText != null)
            flareText.text = "FLARES: --";

        if (minigunStatusText != null)
            minigunStatusText.text = "MINIGUN: READY";

        if (missileAmmoText != null)
            missileAmmoText.text = "MISSILES: --";
    }

    // ========================
    // HEALTH
    // ========================

    private void HandleHealthChanged(float current, float max)
    {
        if (healthFill != null)
            healthFill.fillAmount = current / max;

        if (healthText != null)
            healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    private void HandleDamageTaken(float amount)
    {
        if (damageFlashImage == null) return;
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        damageFlashImage.gameObject.SetActive(true);

        float half = flashDuration * 0.5f;
        float elapsed = 0f;

        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            SetFlashAlpha(Mathf.Lerp(0f, flashMaxAlpha, elapsed / half));
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            SetFlashAlpha(Mathf.Lerp(flashMaxAlpha, 0f, elapsed / half));
            yield return null;
        }

        SetFlashAlpha(0f);
        damageFlashImage.gameObject.SetActive(false);
    }

    private void SetFlashAlpha(float alpha)
    {
        Color c = damageFlashImage.color;
        c.a = alpha;
        damageFlashImage.color = c;
    }

    // ========================
    // FUEL
    // ========================

    private void HandleFuelChanged(float current, float max)
    {
        if (fuelFill != null)
            fuelFill.fillAmount = current / max;

        if (fuelText != null)
            fuelText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    private void HandleFuelEmpty()
    {
        if (fuelFill != null)
            fuelFill.color = criticalColor;
    }

    // ========================
    // WEAPONS
    // ========================

    private void HandleMissileAmmo(int current, int max)
    {
        if (missileAmmoText != null)
            missileAmmoText.text = $"MISSILES: {current} / {max}";
    }

    private void HandleMinigunOverheated()
    {
        if (minigunStatusText != null)
        {
            minigunStatusText.text = "MINIGUN: OVERHEATED";
            minigunStatusText.color = criticalColor;
        }
        GameEvents.RaiseFeedback("Minigun overheated!", FeedbackSeverity.Warning);
    }

    private void HandleMinigunCooled()
    {
        if (minigunStatusText != null)
        {
            minigunStatusText.text = "MINIGUN: READY";
            minigunStatusText.color = infoColor;
        }
    }

    private void HandleFlareAmmo(int current, int max)
    {
        if (flareText != null)
            flareText.text = $"FLARES: {current} / {max}";
    }

    // ========================
    // FEEDBACK LABEL
    // ========================

    private void HandleFeedback(string message, FeedbackSeverity severity)
    {
        if (feedbackText == null) return;
        if (feedbackCoroutine != null) StopCoroutine(feedbackCoroutine);
        feedbackCoroutine = StartCoroutine(ShowFeedback(message, severity));
    }

    private IEnumerator ShowFeedback(string message, FeedbackSeverity severity)
    {
        if (feedbackPanel != null)
            feedbackPanel.SetActive(true);

        feedbackText.color = severity switch
        {
            FeedbackSeverity.Warning => warningColor,
            FeedbackSeverity.Critical => criticalColor,
            _ => infoColor
        };

        feedbackText.text = message;
        yield return new WaitForSeconds(feedbackDuration);
        feedbackText.text = string.Empty;

        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
    }

    // ========================
    // INTERACTION PROGRESS
    // ========================

    private void HandleInteractionStarted(string label, Vector3 worldPos)
    {
        if (interactionProgressRoot != null)
            interactionProgressRoot.SetActive(true);

        if (interactionProgressFill != null)
            interactionProgressFill.fillAmount = 0f;

        if (interactionProgressLabel != null)
            interactionProgressLabel.text = label;

        PositionAtWorldPoint(worldPos);
    }

    private void PositionAtWorldPoint(Vector3 worldPos)
    {
        if (interactionProgressRoot == null || Camera.main == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        screenPos.x += -30f;
        screenPos.y += 100f;
        interactionProgressRoot.GetComponent<RectTransform>().position = screenPos;
    }

    private void HandleInteractionProgress(float progress)
    {
        if (interactionProgressFill != null)
            interactionProgressFill.fillAmount = progress;

        if (interactionProgressText != null)
            interactionProgressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
    }

    private void HandleInteractionCompleted()
    {
        if (interactionProgressRoot != null)
            interactionProgressRoot.SetActive(false);
    }

    private void HandleInteractionCancelled()
    {
        if (interactionProgressRoot != null)
            interactionProgressRoot.SetActive(false);
    }

    // ========================
    // MISSION TASKS
    // ========================

    private void HandleMissionStarted(string missionName)
    {
        if (missionTitleText != null)
            missionTitleText.text = missionName;

        if (objectivesContainer != null)
            foreach (Transform child in objectivesContainer)
                Destroy(child.gameObject);
    }

    private void HandleObjectiveAdded(string objective)
    {
        if (objectivesContainer == null || objectivePrefab == null) return;

        GameObject obj = Instantiate(objectivePrefab, objectivesContainer);
        TMP_Text label = obj.GetComponentInChildren<TMP_Text>();
        if (label != null) label.text = "• " + objective;
    }

    private void HandleObjectiveCompleted(string objective)
    {
        if (objectivesContainer == null) return;

        foreach (Transform child in objectivesContainer)
        {
            TMP_Text label = child.GetComponentInChildren<TMP_Text>();
            if (label != null && label.text == "• " + objective)
            {
                label.fontStyle = FontStyles.Strikethrough;
                label.color = Color.gray;
                break;
            }
        }
    }
}