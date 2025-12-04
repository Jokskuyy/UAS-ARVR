using UnityEngine;

/// <summary>
/// Menangani benda berbahaya (Elektronik/Kimia).
/// Melaporkan status bahaya global ke sistem jika benda ini terendam air.
/// </summary>
public class HazardItem : MonoBehaviour
{
    public enum HazardType { Elektronik, ZatBerbahaya }

    [Header("Item Settings")]
    public HazardType hazardType;
    [Tooltip("Centang jika ingin melihat log saat benda ini kena air.")]
    public bool showDebugLogs = true;

    // --- GLOBAL STATE (API untuk Tim NPC & Player) ---
    // Statis: Nilai dibagi ke seluruh game.
    public static int ActiveElectricHazards = 0;
    public static int ActiveToxicHazards = 0;
    // -------------------------------------------------

    private bool _isSubmerged = false;

    private void Update()
    {
        if (FloodController.Instance == null) return;

        float waterHeight = FloodController.Instance.CurrentWaterLevel;

        // Cek Logika Tenggelam
        if (transform.position.y < waterHeight)
        {
            if (!_isSubmerged)
            {
                _isSubmerged = true;
                UpdateGlobalHazardState(true); // Lapor bahaya AKTIF
            }
        }
        else
        {
            if (_isSubmerged)
            {
                _isSubmerged = false;
                UpdateGlobalHazardState(false); // Lapor bahaya NON-AKTIF (Surut/Diangkat)
            }
        }
    }

    private void UpdateGlobalHazardState(bool isAdding)
    {
        int value = isAdding ? 1 : -1;

        if (hazardType == HazardType.Elektronik)
        {
            ActiveElectricHazards += value;
            if (showDebugLogs)
                Debug.Log($"[HAZARD] {gameObject.name} {(isAdding ? "TERENDAM" : "AMAN")}. Total Sumber Listrik Aktif: {ActiveElectricHazards}");
        }
        else
        {
            ActiveToxicHazards += value;
            if (showDebugLogs)
                Debug.Log($"[HAZARD] {gameObject.name} {(isAdding ? "BOCOR" : "AMAN")}. Total Sumber Racun Aktif: {ActiveToxicHazards}");
        }
    }

    // Safety: Jika benda ini didestroy/disable, pastikan bahaya dikurangi
    private void OnDisable()
    {
        if (_isSubmerged)
        {
            UpdateGlobalHazardState(false);
            _isSubmerged = false;
        }
    }
}