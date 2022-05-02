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
    [SerializeField] private float airControl = 0.3f;
    [SerializeField] private float jumpForce = 0.3f;
    [SerializeField] private Vector3 gravity = new Vector3(0f, -0.9f, 0f);

    private UnityEngine.Vector3 input = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 acc = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 vel = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 pos = UnityEngine.Vector3.zero;
    private UnityEngine.Vector3 _previousPos = UnityEngine.Vector3.zero;

    private bool isGrounded;
    private float _distanceToGround;
    private UnityEngine.Vector2 _closestGroundHit;

    private void Awake()
    {
        keyboardInput.Up += Jump;
        keyboardInput.Down += () => { };
        
        keyboardInput.Left += () => Move(Vector3.left);
        keyboardInput.Right += () => Move(Vector3.right);
    }

    private void Move(Vector3 dir)
    {
        if (isGrounded)
        {
            input += dir;
        } else
        {
            input += dir * airControl;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            input += Vector3.up * jumpForce;
        }
    }

    void FixedUpdate()
    {
        _previousPos = transform.position;

        CheckIsGrounded();
        ApplyMovement();
        
        Debug.DrawLine(_previousPos, transform.position, new Color(0.2f, 1f,1f), 3f);
    }

    private void CheckIsGrounded()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down);
        
        if (hit.collider != null ) 
        {
            Debug.DrawLine(transform.position - Vector3.up,hit.point, Color.blue, 5f);

            _distanceToGround = Vector2.Distance(hit.point, transform.position);
            _closestGroundHit = hit.point;
            isGrounded = _distanceToGround < 0.1f + Mathf.Abs(vel.y);
        } else
        {
            isGrounded = false;
        }
    }

    private void ApplyMovement()
    {
        input *= movementForce;

        acc += input;
        Vector3.ClampMagnitude(acc, maxAcc);
        if(!isGrounded) acc += gravity;
        
        vel += acc;
        Vector3.ClampMagnitude(vel, maxVel);
        
        pos += vel;
        if (isGrounded)
        {
            pos = new Vector3(pos.x, Mathf.Max(_closestGroundHit.y, pos.y), pos.z);
        }
        transform.position = pos;

        if (input.magnitude < maxAcc)
        {
            input = Vector3.zero;
        }
        else
        {
            input -= input.normalized * maxAcc;
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

    public UnityEngine.Vector2 GetVelocity()
    {
        return UnityEngine.Vector2.zero;
    }
}
