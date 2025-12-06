using UnityEngine;

/// <summary>
/// Simulasi mengapung sederhana. 
/// Otomatis mematikan gravitasi saat objek menyentuh permukaan air.
/// </summary>
[RequireComponent(typeof(Rigidbody))] // Wajib punya Rigidbody
public class SimpleBuoyancy : MonoBehaviour
{
    [Header("Buoyancy Settings")]
    [Tooltip("Offset ketinggian dari permukaan air.")]
    public float floatOffset = 0.0f;
    public float wobbleSpeed = 2.0f;
    public float wobbleAmount = 0.1f;

    private float _floorLevel;
    private Rigidbody _rb;

    private void Start()
    {
        _floorLevel = transform.position.y;
        _rb = GetComponent<Rigidbody>();

        // Pastikan RB tidak kinematic agar gravitasi bekerja normal di awal
        _rb.isKinematic = false;
    }

    private void Update()
    {
        if (FloodController.Instance == null) return;

        float waterLevel = FloodController.Instance.CurrentWaterLevel;

        // Target posisi Y jika mengapung
        float targetY = waterLevel + floatOffset + (Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount);

        // Logic Switch: Mode Darat vs Mode Air
        if (targetY > _floorLevel)
        {
            // MODE MENGAPUNG
            _rb.useGravity = false; // Matikan gravitasi Unity

            // Set posisi manual (Hanya Y yang berubah, X dan Z tetap bebas/bisa didorong)
            transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        }
        else
        {
            // MODE DARAT (Belum kena air)
            _rb.useGravity = true; // Kembalikan ke fisika normal
        }
    }
}