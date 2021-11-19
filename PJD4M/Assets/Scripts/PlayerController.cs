using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float maxEnergy;

    public float energySpendRate;

    public float energyRecoverAmount;
    
    public float velocidade;

    public float maxSpeed;

    public float jumpSpeed;

    public float maxJumpTime;

    public Vector3 groundOffset;
    public Vector3 boxSize;

    public LayerMask groundLayer;
    public ContactFilter2D groundFilter;

    [SerializeField] private GameObject muzzlePrefab;
    
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform bulletOrigin;

    [SerializeField] private PlayerInput playerInput;
    
    private Rigidbody2D _rigidbody2D;

    private GameInput _gameInput;

    private Vector2 _movimento;

    private bool _doJump;

    private bool _isGrounded;

    private float _startJumpTime;

    private bool _doDoubleJump;

    private bool _isMovingRight;

    private Animator _playerAnimator;

    private Collider2D _playerCollider;

    private bool _isActive;

    private bool _isDead;

    private float _currentEnergy;

    private bool _needToFire;

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
        _playerAnimator = GetComponent<Animator>();
        _playerCollider = GetComponent<Collider2D>();
        _gameInput = new GameInput();

        _currentEnergy = maxEnergy;
        HUDObserverManager.PlayerEnergyChanged(_currentEnergy);
        _isActive = true;
    }

    private void Update()
    {
        if (_isActive)
        {
            CheckGround();
        
            AnimationUpdates(); 
            
            SpendEnergy();
            
            FireBullet();
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        _rigidbody2D.AddForce(_movimento * velocidade);
        if (Mathf.Abs(_rigidbody2D.velocity.x) > maxSpeed)
            _rigidbody2D.velocity = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x) * maxSpeed,
                _rigidbody2D.velocity.y);
        */
        if (_isActive)
        {
            _rigidbody2D.velocity = new Vector2(_movimento.x * velocidade * Time.fixedDeltaTime, _rigidbody2D.velocity.y);
        
            if(_isMovingRight && _movimento.x > 0) Flip();
            if (!_isMovingRight && _movimento.x < 0) Flip();

            Jump();
        }
        
    }

    private void AnimationUpdates()
    {
        _playerAnimator.SetFloat("Speed", Mathf.Abs(_movimento.x));
        _playerAnimator.SetBool("isGrounded", _isGrounded);
        _playerAnimator.SetFloat("VertSpeed", _rigidbody2D.velocity.y);
    }
    
    private void CheckGround()
    {
        //_isGrounded = Physics2D.Linecast(transform.position, transform.position+groundOffset, groundLayer);

        /*
        RaycastHit2D[] hit = new RaycastHit2D[] { };
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = groundLayer;
        filter.useLayerMask = true;
        int numberOfHits = Physics2D.BoxCast(transform.position + new Vector3(groundOffset.x, groundOffset.y, 0), 
            boxSize, 0, Vector2.up, filter, hit, groundOffset.z);
        Debug.Log(numberOfHits);
        
        if (numberOfHits > 0)
        {
            foreach (RaycastHit2D raycastHit2D in hit)
            {
                Debug.Log(Vector2.Angle(Vector2.up, raycastHit2D.normal));
                if (Vector2.Angle(Vector2.up, raycastHit2D.normal) < 10f)
                {
                    _isGrounded = true;
                    break;
                }
            }
        }*/

        _isGrounded = false;
        
        RaycastHit2D[] hit = Physics2D.BoxCastAll(transform.position + new Vector3(groundOffset.x * transform.localScale.x, groundOffset.y, 0), 
            boxSize, 0, Vector2.up, groundOffset.z, groundLayer);
        if (hit.Length>0)
        {
            //Debug.Log(hit.Length);
            foreach (RaycastHit2D raycastHit2D in hit)
            {
                if (Vector2.Angle(raycastHit2D.normal, Vector2.up) < 20 &&
                    raycastHit2D.point.y < transform.position.y - 1.2f)
                {
                    _isGrounded = true;
                    break;
                }
            }
        }
        
        /*
        ContactPoint2D[] hits = Array.Empty<ContactPoint2D>();
        _playerCollider.GetContacts(hits);
        if (hits.Length>0)
        {
            Debug.Log(hits.Length);
            foreach (ContactPoint2D hit in hits)
            {
                if (Vector2.Angle(hit.normal, Vector2.up) < 20 &&
                    hit.point.y < transform.position.y - 1.2f)
                {
                    _isGrounded = true;
                    break;
                }
            }
        }*/
        
        if (_isGrounded && _doDoubleJump) _doDoubleJump = false;

    }

    private void Jump()
    {
        if (_doJump)
        {
            //_rigidbody2D.AddForce(Vector2.up * jumpSpeed);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpSpeed * Time.fixedDeltaTime);
            if (Time.time - _startJumpTime > maxJumpTime) _doJump = false;
        }
    }
    
    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        if (String.Compare(obj.action.name, _gameInput.Gameplay.Move.name, StringComparison.Ordinal)==0)
        {
            _movimento = obj.ReadValue<Vector2>();
            _movimento.y = 0;
            //Debug.Log(_movimento.ToString());
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

            if (obj.performed && !_isActive)
            {
                if (_isDead)
                {
                    GameManager.instance.CheckDeath();
                    HUDObserverManager.PlayerDied(false);
                }
                else
                {
                    GameManager.instance.LoadNextLevel();
                    HUDObserverManager.PlayerVictory(false);
                }
            }
        }

        if (String.Compare(obj.action.name, _gameInput.Gameplay.Fire.name, StringComparison.Ordinal) == 0)
        {
            if(obj.performed && _isActive) StartFireAnim();
        }

    }

    private void StartFireAnim()
    {
       _playerAnimator.SetTrigger("Shoot");
        _needToFire = true;
    }
    
    private void FireBullet()
    {
        if (_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShootIdle") ||
            _playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShootWalk") ||
            _playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShootJump"))
        {
            if (_needToFire)
            {
                Instantiate(muzzlePrefab, bulletOrigin.position, bulletOrigin.rotation);
                GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation);
                newBullet.GetComponent<BulletController>().IsFlipped = _isMovingRight;
                _needToFire = false;
            }
            
        }
    }
    
    public void JumpButton()
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

    private void Flip()
    {
        _isMovingRight = !_isMovingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1,
            transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Kill"))
        {
            KillPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Victory"))
        {
            OnVictory();
        }

        if (other.CompareTag("EnergyBit"))
        {
            _currentEnergy += energyRecoverAmount;

            if (_currentEnergy > maxEnergy) _currentEnergy = maxEnergy;
            
            
            HUDObserverManager.PlayerEnergyChanged(_currentEnergy);
            
            Destroy(other.gameObject);
        }
    }

    private void KillPlayer()
    {
        _isDead = true;
        _isActive = false;
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        _playerAnimator.SetBool("Active", _isActive);
        _playerAnimator.Play("Dead");
        HUDObserverManager.PlayerDied(true);
    }

    private void OnVictory()
    {
        _isActive = false;
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        _playerAnimator.SetBool("Active", _isActive);
        _playerAnimator.Play("Victory");
        HUDObserverManager.PlayerVictory(true);
    }

    private void SpendEnergy()
    {
        _currentEnergy -= Time.deltaTime * energySpendRate;
        HUDObserverManager.PlayerEnergyChanged(_currentEnergy);
        
        if (_currentEnergy < 0)
        {
            _currentEnergy = 0;
            
            HUDObserverManager.PlayerEnergyChanged(_currentEnergy);
            KillPlayer();
        }
    }

    private void OnDrawGizmos()
    {
        //Debug.DrawLine(transform.position, transform.position+groundOffset, Color.red);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(groundOffset.x * transform.localScale.x, groundOffset.y,0), 
            boxSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(groundOffset.x * transform.localScale.x, groundOffset.y+groundOffset.z,0), 
            boxSize);
    }
}
