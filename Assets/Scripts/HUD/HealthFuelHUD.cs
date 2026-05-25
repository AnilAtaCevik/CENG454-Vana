using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthFuelHUD : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Fuel UI")]
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Image fuelFill;
    [SerializeField] private TextMeshProUGUI fuelText;

    [Header("Colors")]
    [SerializeField] private Color colorFull = new Color(0.2f, 0.85f, 0.3f);
    [SerializeField] private Color colorMid  = new Color(0.95f, 0.75f, 0.1f);
    [SerializeField] private Color colorLow  = new Color(0.9f, 0.25f, 0.2f);

    [Header("Thresholds")]
    [SerializeField] private float midThreshold = 0.5f;
    [SerializeField] private float lowThreshold = 0.25f;

    private void Onable()
    {
        GameEventSystem.OnHealthChanged += UpdateHealth;
        GameEventSystem.OnFuelChanged += UpdateFuel;
    }
    private void OnDisable()
    {
        GameEventSystem.OnHealthChanged -= UpdateHealth;
        GameEventSystem.OnFuelChanged -= UpdateFuel;
    }
    private void Start()
    {
        UpdateHealth(1f);
        UpdateFuel(1f);
    }
    private void UpdateHealth(float value)
    {
        if(healthSlider != null)
            healthSlider.value = value;

        if (healthText != null)
            healthText.text = Mathf.RoundToInt(value * 100f) + " / 100";

        if (healthFill != null)
            healthFill.color = GetBarColor(value);
    }
    private void UpdateFuel(float value)
    {
        if (fuelSlider != null)
            fuelSlider.value = value;

        if (fuelText != null)
            fuelText.text = Mathf.RoundToInt(value * 100f) + " / 100";

        if (fuelFill != null)
            fuelFill.color = GetBarColor(value);
    }
    private Color GetBarColor(float value)
    {
        if (value > midThreshold)
        {
            float t = (value - midThreshold) / (1f - midThreshold);
            return Color.Lerp(colorMid, colorFull, t);
        }
        else if (value > lowThreshold)
        {
            float t = (value - lowThreshold) / (midThreshold - lowThreshold);
            return Color.Lerp(colorLow, colorMid, t);
        }
        else
        {
            return colorLow;
        }
    }
}
