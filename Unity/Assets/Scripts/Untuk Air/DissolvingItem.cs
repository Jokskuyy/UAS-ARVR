using UnityEngine;

/// <summary>
/// Menangani barang yang bisa rusak terkena air (Dokumen/Barang Berharga).
/// Memiliki HP yang berkurang seiring waktu saat terendam.
/// </summary>
public class DissolvingItem : MonoBehaviour
{
    public enum ItemType { Dokumen, BarangPenting }

    [Header("Item Properties")]
    public ItemType tipeBarang;
    public float maxHP = 100f;
    [Tooltip("Kerusakan per detik saat terendam air.")]
    public float damagePerSecond = 10f;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private float _currentHP;
    private bool _isDestroyed = false;
    private Renderer _renderer; // Cache renderer untuk performa

    private void Start()
    {
        _currentHP = maxHP;
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_isDestroyed || FloodController.Instance == null) return;

        // Cek apakah kena air
        if (transform.position.y < FloodController.Instance.CurrentWaterLevel)
        {
            TakeWaterDamage();
        }
    }

    private void TakeWaterDamage()
    {
        _currentHP -= damagePerSecond * Time.deltaTime;

        // Visual Feedback (Opsional: Merah saat basah)
        if (_renderer != null)
        {
            _renderer.material.color = Color.Lerp(Color.red, Color.white, _currentHP / maxHP);
        }

        // Debug Log (Hanya muncul jika showDebugLogs dicentang)
        if (showDebugLogs)
        {
            // TODO: Hapus log ini saat Final Build untuk performa
            Debug.Log($"[ITEM DAMAGE] {gameObject.name} | HP: {(int)_currentHP}/{maxHP}");
        }

        if (_currentHP <= 0)
        {
            DestroyItem();
        }
    }

    private void DestroyItem()
    {
        _isDestroyed = true;
        if (showDebugLogs) Debug.Log($"[ITEM LOST] {tipeBarang}: {gameObject.name} hancur terkena air!");

        // Disini bisa tambahkan Instantiate Particle Effect (misal: kertas hancur)
        Destroy(gameObject);
    }
}