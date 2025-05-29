using UnityEngine;


public class Skeleton1Controller : MonoBehaviour
{
    public KeyCode jumpKey = KeyCode.Z;
    public float jumpForce = 5.2f;
    public float groundCheckDistance = 0.1f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(jumpKey) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f);
    }

}