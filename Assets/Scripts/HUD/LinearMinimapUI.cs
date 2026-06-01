using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Linear (horizontal) minimap.
/// Maps each world object's X position onto a horizontal bar and shows an icon.
/// - Repeating objects (checkpoints, extraction, dropoff, resupply, high value)
///   are auto-discovered by component type.
/// - Start/End icons auto-snap to LevelStartZone / LevelCompleteZone in the scene
///   if you don't assign a Transform manually.
/// - Icons for destroyed or disabled objects are hidden automatically.
/// </summary>
public class LinearMinimapUI : MonoBehaviour
{
    [Header("Level Bounds (X axis)")]
    [Tooltip("World X that maps to the LEFT edge of the bar.")]
    public float levelStartX = -10f;
    [Tooltip("World X that maps to the RIGHT edge of the bar.")]
    public float levelEndX = 10f;
    [Tooltip("If ON, the bounds above are filled in from the resolved Start/End points.")]
    public bool autoBoundsFromStartEnd = false;

    [Header("Minimap Bar")]
    [Tooltip("The RectTransform of the bar/track itself.")]
    public RectTransform minimapBar;
    [Tooltip("Optional vertical nudge for icons.")]
    public float iconVerticalOffset = 0f;

    [Header("Player (moves every frame)")]
    public Transform player;
    public RectTransform playerIcon;

    [Header("Single Points (optional — leave empty to auto-find LevelStartZone / LevelCompleteZone)")]
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
        public Component target; // null for static markers (start/end)
        public bool isStatic;    // true for start/end — never hide them
    }

    readonly List<Marker> markers = new List<Marker>();

    void Start()
    {
        Rebuild();
    }

    /// <summary>Clears and rebuilds every icon. Safe to call again if the scene changes.</summary>
    public void Rebuild()
    {
        // Throw away copies from the previous build (but never the start/end icons themselves).
        foreach (Marker m in markers)
            if (m.icon != null && m.icon != startIcon && m.icon != endIcon)
                Destroy(m.icon.gameObject);
        markers.Clear();

        // Resolve start/end: prefer the manually-assigned transforms; otherwise look for
        // a LevelStartZone / LevelCompleteZone in the scene. This lets the minimap figure
        // out where the level begins and ends without you wiring anything by hand.
        Transform effStart = startPoint != null ? startPoint : FindZoneTransform<LevelStartZone>();
        Transform effEnd = endPoint != null ? endPoint : FindZoneTransform<LevelCompleteZone>();

        // Optionally take the level bounds straight from those points.
        if (autoBoundsFromStartEnd && effStart != null && effEnd != null)
        {
            float a = GetWorldX(effStart);
            float b = GetWorldX(effEnd);
            if (Mathf.Abs(a - b) > 0.01f)
            {
                levelStartX = a;
                levelEndX = b;
            }
            else
            {
                Debug.LogWarning("[LinearMinimapUI] Start and End share (almost) the same X. " +
                                 "Keeping the manual Level Start/End values. " +
                                 "Make sure both points are actually placed in the world.");
            }
        }

        // Place start/end icons.
        AddStaticMarker(startIcon, effStart != null ? GetWorldX(effStart) : levelStartX);
        AddStaticMarker(endIcon, effEnd != null ? GetWorldX(effEnd) : levelEndX);

        // Repeating objects: one icon per component instance in the scene.
        SpawnIconsFor<CheckpointZone>(checkpointIconTemplate);
        SpawnIconsFor<ExtractionZone>(extractionIconTemplate);
        SpawnIconsFor<DropoffZone>(dropoffIconTemplate);
        SpawnIconsFor<ResupplyStation>(resupplyIconTemplate);
        SpawnIconsFor<HighValueTarget>(highValueIconTemplate);
    }

    Transform FindZoneTransform<T>() where T : Component
    {
        T zone = FindFirstObjectByType<T>(FindObjectsInactive.Include);
        return zone != null ? zone.transform : null;
    }

    /// <summary>
    /// Returns the world X of an object the way you'd actually see it.
    /// Prefers the collider center, then a renderer, then the raw transform — this
    /// is offset on a child.
    /// </summary>
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

        // The template is never shown — only its copies.
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
            AddTrackedMarker(copy, GetWorldX(obj), obj);
        }
    }

    void AddStaticMarker(RectTransform icon, float worldX)
    {
        if (icon == null) return;
        markers.Add(new Marker { icon = icon, worldX = worldX, target = null, isStatic = true });
    }

    void AddTrackedMarker(RectTransform icon, float worldX, Component target)
    {
        if (icon == null) return;
        markers.Add(new Marker { icon = icon, worldX = worldX, target = target, isStatic = false });
    }

    void LateUpdate()
    {
        foreach (Marker m in markers)
        {
            if (m.icon == null) continue;

            if (m.isStatic)
            {
                // Start / End — always shown, always placed.
                PlaceIcon(m.icon, m.worldX);
            }
            else
            {
                // Tracked object: hide its icon if the object was destroyed.
                bool alive = m.target != null && m.target.gameObject.activeInHierarchy;
                m.icon.gameObject.SetActive(alive);
                if (alive) PlaceIcon(m.icon, m.worldX);
            }
        }

        // Player icon: re-placed every frame since the player moves.
        if (player != null && playerIcon != null)
            PlaceIcon(playerIcon, player.position.x);
    }

    /// <summary>Puts an icon at the correct spot along the bar for a given world X.</summary>
    void PlaceIcon(RectTransform icon, float worldX)
    {
        if (icon == null || minimapBar == null) return;

        float t = Mathf.Clamp01(Mathf.InverseLerp(levelStartX, levelEndX, worldX));
        float localX = Mathf.Lerp(minimapBar.rect.xMin, minimapBar.rect.xMax, t);
        float localY = minimapBar.rect.center.y + iconVerticalOffset;
        icon.position = minimapBar.TransformPoint(new Vector3(localX, localY, 0f));
    }
}