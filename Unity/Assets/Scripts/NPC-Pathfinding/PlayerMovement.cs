using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 100f;

    private CharacterController controller;
    private Vector3 velocity;
    private Transform cameraTransform; // Referensi kamera untuk rotasi vertikal
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Mengunci kursor agar tidak keluar layar saat main
        Cursor.lockState = CursorLockMode.Locked;

        // Mencari kamera utama secara otomatis (pastikan ada kamera dengan tag MainCamera)
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        if (controller == null) return;

        // 1. ROTASI KAMERA (MOUSE)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotasi badan karakter (Kiri/Kanan)
        transform.Rotate(Vector3.up * mouseX);

        // Rotasi kamera (Atas/Bawah) - Opsional, tambahkan jika kamera belum bisa nunduk
        if (cameraTransform != null)
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // 2. GRAVITASI
        // Reset velocity y jika sudah menyentuh tanah agar tidak menumpuk
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 3. PERGERAKAN (WASD)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Perbaikan: HAPUS pembalikan nilai (-x, -z) agar W = Maju
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // 4. APLIKASI GRAVITASI
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}