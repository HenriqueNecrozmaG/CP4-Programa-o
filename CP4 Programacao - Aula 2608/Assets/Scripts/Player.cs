using UnityEditor.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpStrenght;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask layerGround;

    private float horizontal;
    private bool jumpPressed;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }
        Pause();
    }

    void FixedUpdate()
    {
        Movement(horizontal);
        Jump();
    }

    void Movement(float x)
    {
        Vector3 v = rb.linearVelocity;
        v.x = x * speed;
        v.z = 0f;
        rb.linearVelocity = v;
    }

    void Jump()
    {
        if (jumpPressed && IsGrounded())
        {
            Vector3 v = rb.linearVelocity;
            v.y = 0f;
            rb.linearVelocity = v;
            rb.AddForce(Vector3.up * jumpStrenght, ForceMode.Impulse);
            jumpPressed = false;
        }
        else
        {
            jumpPressed = false;
        }
    }

    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block") && !IsGrounded())
        {
            Destroy(collision.gameObject);
            GameManager.Instance.UpdateScore(50);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!IsGrounded())
            {
                Destroy(collision.gameObject);
                GameManager.Instance.UpdateScore(200);
            }
            else
            {
                GameManager.Instance.UpdateLives(-1);
                GameManager.Instance.Death();
            }
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!IsGrounded())
            {
                Destroy(collision.gameObject);
                GameManager.Instance.UpdateScore(200);
            }
            else
            {
                GameManager.Instance.UpdateLives(-1);
                GameManager.Instance.Death();
            }
        }
        if (collision.gameObject.CompareTag("CoinBlock") && !IsGrounded())
        {
            GameManager.Instance.UpdateCoins(1);
            GameManager.Instance.UpdateScore(100);
        }
        if (collision.gameObject.CompareTag("InvisibleBlock") && !IsGrounded())
        {
            GameManager.Instance.UpdateLives(1);
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, layerGround);
    }
}
