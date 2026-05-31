using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Linear (horizontal) minimap.
/// Maps each world object's X position onto a horizontal bar and shows an icon for it.
/// Icons for destroyed or disabled objects are automatically hidden.
/// </summary>
public class LinearMinimapUI : MonoBehaviour
{
    [Header("Level Bounds (X axis)")]
    [Tooltip("World X that maps to the LEFT edge of the bar.")]
    public float levelStartX = -10f;
    [Tooltip("World X that maps to the RIGHT edge of the bar.")]
    public float levelEndX = 10f;
    [Tooltip("If ON, bounds are filled automatically from Start/End point transforms.")]
    public bool autoBoundsFromStartEnd = false;

    [Header("Minimap Bar")]
    [Tooltip("The RectTransform of the bar/track itself.")]
    public RectTransform minimapBar;
    [Tooltip("Optional vertical nudge for icons.")]
    public float iconVerticalOffset = 0f;

    [Header("Player (moves every frame)")]
    public Transform player;
    public RectTransform playerIcon;

    [Header("Single Points")]
    public Transform startPoint;
    public RectTransform startIcon;
    public Transform endPoint;
    public RectTransform endIcon;

    [Header("Repeating Object Icons (used as templates)")]
    public RectTransform checkpointIconTemplate;
    public RectTransform extractionIconTemplate;
    public RectTransform dropoffIconTemplate;
    public RectTransform resupplyIconTemplate;
    public RectTransform highValueIconTemplate;
    [Tooltip("Also show objects that are currently disabled in the scene.")]
    public bool includeInactiveObjects = false;

    struct Marker
    {
        public RectTransform icon;
        public float worldX;
        public Component target; // null for static points (start/end)
    }

    readonly List<Marker> markers = new List<Marker>();

    void Start()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        foreach (Marker m in markers)
            if (m.icon != null && m.icon != startIcon && m.icon != endIcon)
                Destroy(m.icon.gameObject);
        markers.Clear();

        if (autoBoundsFromStartEnd && startPoint != null && endPoint != null)
        {
            float a = GetWorldX(startPoint);
            float b = GetWorldX(endPoint);
            if (Mathf.Abs(a - b) > 0.01f)
            {
                levelStartX = a;
                levelEndX = b;
            }
            else
            {
                Debug.LogWarning("[LinearMinimapUI] Start and End points share almost " +
                                 "the same X. Keeping manual Level Start/End values.");
            }
        }

        AddMarker(startIcon, startPoint != null ? GetWorldX(startPoint) : levelStartX);
        AddMarker(endIcon, endPoint != null ? GetWorldX(endPoint) : levelEndX);

        SpawnIconsFor<CheckpointZone>(checkpointIconTemplate);
        SpawnIconsFor<ExtractionZone>(extractionIconTemplate);
        SpawnIconsFor<DropoffZone>(dropoffIconTemplate);
        SpawnIconsFor<ResupplyStation>(resupplyIconTemplate);
        SpawnIconsFor<HighValueTarget>(highValueIconTemplate);
    }

    float GetWorldX(Component obj)
    {
        Collider col = obj.GetComponentInChildren<Collider>();
        if (col != null) return col.bounds.center.x;

        Renderer rend = obj.GetComponentInChildren<Renderer>();
        if (rend != null) return rend.bounds.center.x;

        return obj.transform.position.x;
    }

    void SpawnIconsFor<T>(RectTransform template) where T : Component
    {
        if (template == null) return;

        template.gameObject.SetActive(false);

        FindObjectsInactive inactive = includeInactiveObjects
            ? FindObjectsInactive.Include
            : FindObjectsInactive.Exclude;

        T[] found = FindObjectsByType<T>(inactive, FindObjectsSortMode.None);
        foreach (T obj in found)
        {
            RectTransform copy = Instantiate(template, template.parent);
            copy.gameObject.SetActive(true);
            copy.name = template.name + "_" + obj.name;
            AddMarkerTracked(copy, GetWorldX(obj), obj);
        }
    }

    void AddMarker(RectTransform icon, float worldX)
    {
        if (icon == null) return;
        markers.Add(new Marker { icon = icon, worldX = worldX, target = null });
    }

    void AddMarkerTracked(RectTransform icon, float worldX, Component target)
    {
        if (icon == null) return;
        markers.Add(new Marker { icon = icon, worldX = worldX, target = target });
    }

    void LateUpdate()
    {
        foreach (Marker m in markers)
        {
            if (m.target == null)
            {
                // Object was fully destroyed
                if (m.icon != startIcon && m.icon != endIcon)
                    m.icon.gameObject.SetActive(false);
                else
                    PlaceIcon(m.icon, m.worldX);
            }
            else if (!m.target.gameObject.activeSelf)
            {
                // Object was disabled via SetActive(false) — hide icon
                if (m.icon != startIcon && m.icon != endIcon)
                    m.icon.gameObject.SetActive(false);
            }
            else
            {
                // Object alive and active — show icon
                m.icon.gameObject.SetActive(true);
                PlaceIcon(m.icon, m.worldX);
            }
        }

        // Player icon always updates every frame
        if (player != null && playerIcon != null)
            PlaceIcon(playerIcon, player.position.x);
    }

    void PlaceIcon(RectTransform icon, float worldX)
    {
        if (icon == null || minimapBar == null) return;

        float t = Mathf.Clamp01(Mathf.InverseLerp(levelStartX, levelEndX, worldX));
        float localX = Mathf.Lerp(minimapBar.rect.xMin, minimapBar.rect.xMax, t);
        float localY = minimapBar.rect.center.y + iconVerticalOffset;
        icon.position = minimapBar.TransformPoint(new Vector3(localX, localY, 0f));
    }
}