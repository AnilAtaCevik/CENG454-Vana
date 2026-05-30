using UnityEngine;
using UnityEngine.UI;

public class HitMarkerUI : MonoBehaviour
{
    public static HitMarkerUI Instance;

    [SerializeField] private Image hitMarkerImage;
    [SerializeField] private float displayTime = 0.1f;

    private float timer;

    void Awake()
    {
        Instance = this;
        hitMarkerImage.enabled = false;
    }

    void Update()
    {
        if (hitMarkerImage.enabled)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                hitMarkerImage.enabled = false;
            }
        }
    }

    public void ShowHitMarker()
    {
        hitMarkerImage.enabled = true;
        timer = displayTime;
    }
}