using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Kecepatan gerak
    public float gravity = -9.81f; // Nilai gravitasi
    
    private CharacterController controller; 
    private Vector3 velocity; 

    void Start()
    {
        controller = GetComponent<CharacterController>(); 
        if (controller == null)
        {
            Debug.LogError("PlayerMovement membutuhkan komponen CharacterController!");
        }
    }

    void Update()
    {
        // Debugging Input: Cek apakah input WASD diterima
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Debug.Log("Input WASD Diterima!");
        }

        if (controller == null) return; 

        // 1. Logika Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // Hitung Input (W, A, S, D)
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical"); 
        
        // >>> PERBAIKAN INVERSI <<<
        // Balikkan arah input
        x = -x; 
        z = -z; 
        // >>> END PERBAIKAN <<<
        
        // Hitung Arah Gerak Relatif terhadap rotasi Player
        Vector3 move = transform.right * x + transform.forward * z;
        
        // Gerakkan Player
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Terapkan Gravitasi 
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}