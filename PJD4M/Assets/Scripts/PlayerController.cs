using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float velocidade;

    public float jumpSpeed;

    public float maxJumpTime;

    public Vector3 groundOffset;

    public LayerMask groundLayer;

    [SerializeField] private PlayerInput playerInput;
    
    private Rigidbody2D _rigidbody2D;

    private GameInput _gameInput;

    private Vector2 _movimento;

    private bool _doJump;

    private bool _isGrounded;

    private float _startJumpTime;

    private bool _doDoubleJump;

    private bool _isMovingRight;

    private void OnEnable()
    {
        playerInput.onActionTriggered += OnActionTriggered;
    }

    private void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gameInput = new GameInput();
    }

    private void Update()
    {
        CheckGround();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody2D.AddForce(_movimento * velocidade);
        if(_isMovingRight && _movimento.x > 0) Flip();
        if (!_isMovingRight && _movimento.x < 0) Flip();
        
        Jump();
    }

    private void CheckGround()
    {
        _isGrounded = Physics2D.Linecast(transform.position, transform.position+groundOffset, groundLayer);
        if (_isGrounded && _doDoubleJump) _doDoubleJump = false;

    }

    private void Jump()
    {
        if (_doJump)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpSpeed);
            if (Time.time - _startJumpTime > maxJumpTime) _doJump = false;
        }
    }
    
    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        if (String.Compare(obj.action.name, _gameInput.Gameplay.Move.name, StringComparison.Ordinal)==0)
        {
            _movimento = obj.ReadValue<Vector2>();
            _movimento.y = 0;
            Debug.Log(_movimento.ToString());
        }

        if (String.Compare(obj.action.name, _gameInput.Gameplay.Jump.name, StringComparison.Ordinal) == 0)
        {

            if (obj.started)
            {
                if (_isGrounded)
                {
                    _doJump = true;
                    _startJumpTime = Time.time;
                }
                else
                {
                    if (!_doDoubleJump)
                    {
                        _doDoubleJump = true;
                        _doJump = true;
                        _startJumpTime = Time.time;
                    }
                }
            }

            if (obj.canceled) _doJump = false;
        }
    }

    private void Flip()
    {
        _isMovingRight = !_isMovingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1,
            transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position+groundOffset, Color.red);
    }
}
