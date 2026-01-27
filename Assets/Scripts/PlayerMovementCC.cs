using UnityEngine;

public class PlayerMovementCC : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 2f;
    public float gravity = -9.81f;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private CharacterController cc;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundMask);

        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        cc.Move(move * speed * Time.deltaTime);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
}

