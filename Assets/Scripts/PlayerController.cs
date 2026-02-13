using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.2f;
    public float rotationSpeed = 10f;


    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private bool hasKey = false;
    private Exit exit;

    [System.Obsolete]
    void Start()
    {
        controller = GetComponent<CharacterController>();
        exit = FindObjectOfType<Exit>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0f, moveZ);

        // 🔁 ROTACIÓN
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        controller.Move(move.normalized * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }


    public void CollectKey()
    {
        hasKey = true;
        Debug.Log("¡Llave recogida!");

        if (exit != null)
            exit.UnlockExit();
    }

    public bool HasKey()
    {
        return hasKey;
    }
}
