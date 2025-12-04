using UnityEngine;

/// <summary>
/// MANAGER PUSAT AIR (Singleton).
/// Mengatur ketinggian air dan menyediakan data ketinggian air untuk skrip lain.
/// </summary>
public class FloodController : MonoBehaviour
{
    // Singleton Instance: Agar mudah diakses oleh Programmer NPC & Player
    public static FloodController Instance { get; private set; }

    [Header("Flood Configuration")]
    [Tooltip("Kecepatan kenaikan air (meter per detik).")]
    [SerializeField] private float riseSpeed = 0.1f;

    [Tooltip("Batas maksimal ketinggian air.")]
    [SerializeField] private float maxHeight = 3.0f;

    [Header("Debug Settings")]
    [Tooltip("Centang ini untuk melihat log status air di Console.")]
    public bool showDebugLogs = true;

    // Variabel publik untuk dibaca skrip lain (Read-Only di Inspector)
    [field: Header("Live Status (Do Not Edit)")]
    [field: SerializeField] public float CurrentWaterLevel { get; private set; }
    [field: SerializeField] public bool IsFlooding { get; private set; } = false;

    private void Awake()
    {
        // Setup Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (IsFlooding && transform.position.y < maxHeight)
        {
            // Logika menaikkan air
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        }

        // Update nilai global
        CurrentWaterLevel = transform.position.y;
    }

    /// <summary>
    /// Panggil fungsi ini untuk memulai bencana banjir.
    /// </summary>
    public void StartFlooding()
    {
        IsFlooding = true;
        if (showDebugLogs) Debug.LogWarning("[FLOOD SYSTEM] Peringatan: Air mulai naik!");
    }

    /// <summary>
    /// Panggil fungsi ini untuk menghentikan air (misal: Game Over / Pause).
    /// </summary>
    public void StopFlooding()
    {
        IsFlooding = false;
        if (showDebugLogs) Debug.Log("[FLOOD SYSTEM] Air berhenti naik.");
    }
}