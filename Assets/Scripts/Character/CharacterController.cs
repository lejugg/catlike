using Input;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private KeyboardInput keyboardInput;
    
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float maxAcc = 1f;
    [SerializeField] private float maxVel = 1f;
    [SerializeField] private float accDrag = 0.5f;
    [SerializeField] private float velDrag = 0.5f;
    [SerializeField] private Vector3 gravity = new Vector3(0f, -0.9f, 0f);

    private Vector3 input = Vector3.zero;
    private Vector3 acc = Vector3.zero;
    private Vector3 vel = Vector3.zero;
    private Vector3 pos = Vector3.zero;
    private Vector3 _previousPos = Vector3.zero;

    private bool isGrounded;

    private void Awake()
    {
        keyboardInput.Up += Jump;
        keyboardInput.Down += () => { };
        
        keyboardInput.Left += () => Move(Vector3.left);
        keyboardInput.Right += () => Move(Vector3.right);
    }

    private void Move(Vector3 dir)
    {
        input += dir;
    }

    private void Jump()
    {
        input += Vector3.up;
    }

    void FixedUpdate()
    {
        _previousPos = transform.position;

        CheckIsGrounded();
        ApplyMovement();
        
        Debug.DrawLine(_previousPos, transform.position, Color.green, 5f);
    }

    private void CheckIsGrounded()
    {
        isGrounded = transform.position.y < 0;
    }

    private void ApplyMovement()
    {
        input.Normalize();
        input *= movementForce;

        acc += input;
        Vector3.ClampMagnitude(acc, maxAcc);
        if(!isGrounded) acc += gravity;

        vel += acc;
        Vector3.ClampMagnitude(vel, maxVel);

        pos += vel;
        transform.position = pos;

        if (input.magnitude < maxAcc)
        {
            input = Vector3.zero;
        }
        else
        {
            input *= (input.magnitude - maxAcc);
        }

        acc *= accDrag;
        vel *= velDrag;
    }

    // private void ApplyMovementForce()
    // {
    //     input *= movementForce;
    //     rigidbody.AddForce(input);
    //     input = Vector3.zero;
    // }
    //
    // private void CheckGround()
    // {
    //     var hit = Physics2D.CircleCast(transform.position , 0.4f, UnityEngine.Vector2.down, 0.21f);
    //     
    //     if (hit.collider != null ) 
    //     {
    //         Debug.DrawLine(transform.position, hit.point, Color.green, 10f);
    //         isGrounded = true;
    //     } else
    //     {
    //         isGrounded = false;
    //     }
    //
    //     _spriteRenderer.color = isGrounded ? Color.white : Color.green;
    // }


    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public Vector2 GetVelocity()
    {
        return Vector2.zero;
    }
}
