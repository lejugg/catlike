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
    [SerializeField] private float airDrag = 0.9f;
    [SerializeField] private float airControl = 0.3f;
    [SerializeField] private float jumpForce = 0.3f;
    [SerializeField] private float characterHeight = 0.2f;
    [SerializeField] private Vector3 gravity = new Vector3(0f, -0.9f, 0f);
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector3 input = Vector3.zero;
    private Vector3 acc = Vector3.zero;
    private Vector3 vel = Vector3.zero;
    private Vector3 pos = Vector3.zero;
    private Vector3 _previousPos = Vector3.zero;

    private bool isGrounded;
    private float _distanceToGround;
    private Vector2 _closestGroundHit;

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
        
        Debug.DrawLine(_previousPos, transform.position, isGrounded ? Color.red : Color.yellow, 3f);
    }

    private void CheckIsGrounded()
    {
        var hit = Physics2D.Raycast(transform.position + Vector3.up * characterHeight, Vector2.down);
        
        if (hit.collider != null ) 
        {
             Debug.DrawLine(transform.position,hit.point, new Color(1f,1f,1f, 0.3f), 3f);
             // Debug.DrawLine(hit.point + Vector2.up * 0.01f,hit.point, Color.black, 3f);

            _distanceToGround = Vector2.Distance(hit.point, transform.position);
            _closestGroundHit = hit.point;
            
            // only factor in downwards velocity into when grounding should happen
            isGrounded = _distanceToGround < 0.01f + Mathf.Max(0f, -vel.y); 
        } else
        {
            isGrounded = false;
        }

    }

    private void ApplyMovement()
    {
        input *= movementForce;

        acc += input;
        if(!isGrounded) acc += gravity;
        if (acc.magnitude > maxAcc)
        {
            acc = acc.normalized * maxAcc;
        }

        vel += acc;
        if (vel.magnitude > maxVel)
        {
            vel = vel.normalized * maxVel;
        }
        
        pos += vel;
        if (isGrounded)
        {
            pos = new Vector3(pos.x, _closestGroundHit.y + Mathf.Max(0f, vel.y), pos.z);
        }
        transform.position = pos;

        acc *= isGrounded ? accDrag : airDrag;
        vel *= velDrag;
        input = Vector3.zero;
                    
        Debug.DrawLine(transform.position, transform.position + vel * 5f, Color.magenta);
        Debug.DrawLine(transform.position + vel * 5f, transform.position + vel * 5f  + acc * 5f, Color.yellow);
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
