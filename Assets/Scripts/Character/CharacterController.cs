using Input;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private KeyboardInput keyboardInput;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float movementForce = 10f;
    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    
    private Vector3 movement = Vector3.zero;
    private bool isGrounded;

    private void Awake()
    {
        keyboardInput.Up += Jump;
        
        keyboardInput.Left += () => Move(Vector3.left);
        keyboardInput.Right += () => Move(Vector3.right);
    }

    private void Move(Vector3 dir)
    {
        if (isGrounded)
        {
            movement += dir;
        } else
        {
            movement += dir;
            movement *= 0.3f;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            movement += Vector3.up * jumpForce;
        }
    }

    void FixedUpdate()
    {
        CheckGround();

        ApplyMovementForce();
    }

    private void ApplyMovementForce()
    {
        movement *= movementForce;
        rigidbody.AddForce(movement);
        movement = Vector3.zero;
    }

    private void CheckGround()
    {
        var hit = Physics2D.CircleCast(transform.position , 0.4f, UnityEngine.Vector2.down, 0.21f);
        
        if (hit.collider != null ) 
        {
            Debug.DrawLine(transform.position, hit.point, Color.green, 10f);
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }

        _spriteRenderer.color = isGrounded ? Color.white : Color.green;
    }

    public Vector2 GetVelocity()
    {
        return rigidbody.velocity;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }
}
