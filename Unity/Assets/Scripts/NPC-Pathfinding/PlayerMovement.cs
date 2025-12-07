using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float gravity = -9.81f; 
    public float mouseSensitivity = 100f;
    
    private CharacterController controller; 
    private Vector3 velocity; 

    void Start()
    {
        controller = GetComponent<CharacterController>(); 
        if (controller == null)
        {
            Debug.LogError("PlayerMovement membutuhkan komponen CharacterController!");
        }
        
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Debug.Log("Input WASD Diterima!");
        }

        if (controller == null) return; 

        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX); 
        
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical"); 
        
       
        x = -x; 
        z = -z; 
    
        Vector3 move = transform.right * x + transform.forward * z;
    
        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}