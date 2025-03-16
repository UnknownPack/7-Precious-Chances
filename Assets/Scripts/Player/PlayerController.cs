using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("General movement Settings")]
    [SerializeField] private float jogSpeed = 5f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private ViewState currentViewState = ViewState.SideScrolling;
    
    [SerializeField] private float linearDrag = 6.5f;
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip jumpingSound;
    private AudioSource _audioSource;
    
    private Rigidbody2D rb2d;
    private CapsuleCollider2D _capsule2d;
    private PlayerInput _playerInput;
    private InputAction _moveAction, _jumpAction, _sprintAction;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private float currentMoveSpeed = 0f, startingDrag;
    private bool _isDead = false;
    private Vector2 currrentMovementVector;
        
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        _capsule2d = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>(); 
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        currentMoveSpeed = jogSpeed;
        InitalizeActions();
        startingDrag = rb2d.linearDamping;
    }

    
    void Update()
    {
        ManageMovement();
        ManageSprite();
    }
    
    #region Action Defintions
    private void InitalizeActions()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
        _moveAction.Enable(); 
        
        _jumpAction = _playerInput.actions.FindAction("Jump");
        _jumpAction.Enable();
        _jumpAction.performed += JumpAction; 
        
        _sprintAction = _playerInput.actions.FindAction("Sprint");
        _sprintAction.Enable();
        _sprintAction.performed += SprintAction;  
        _sprintAction.canceled += ctx => currentMoveSpeed = jogSpeed; 
    }
    private void SprintAction(InputAction.CallbackContext context) { currentMoveSpeed = runSpeed; }

    private void JumpAction(InputAction.CallbackContext context)
    {
        if(IsGrounded())
        {
            rb2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            _animator.Play("Jump");
            _audioSource.clip = jumpingSound;
            _audioSource.loop = false;
            _audioSource.Play();
            Debug.Log("Jumping sound: " + jumpingSound);

        }
    } 
    #endregion
    
    #region Helper Functions
    
        #region Movement-Related Functions

        private void ManageMovement()
        {
            currrentMovementVector = _moveAction.ReadValue<Vector2>();  
            if (currentViewState == ViewState.SideScrolling)
            { 
                rb2d.gravityScale = 1; 
                _jumpAction.Enable();
                rb2d.linearDamping = startingDrag;
                /*
                 * Crouching
                 * if currrentMovementVector is less then 0, then that means the player is inputing down or crouch
                 * when the player is crouching, they are not allowed to move
                 */
                if(currrentMovementVector.y >= 0) {
                    rb2d.AddForce(new Vector2(currrentMovementVector.x * currentMoveSpeed, 0));
                    if (currrentMovementVector.x != 0) {
                    if (IsGrounded()) {
                        if (_audioSource.clip != walkingSound || !_audioSource.isPlaying) {
                            _audioSource.clip = walkingSound;
                            _audioSource.loop = true;
                            _audioSource.Play();
                        }
                    }
                    } else {
                        if (_audioSource.isPlaying && _audioSource == walkingSound) {
                            _audioSource.Stop();
                            _audioSource.loop = false;
                        }
                    }
                }
                

                    
            }
            else
            { 
                rb2d.gravityScale = 0; 
                _jumpAction.Disable();  
                rb2d.linearDamping = linearDrag; 
                rb2d.AddForce(currrentMovementVector * currentMoveSpeed * 0.75f);     
            }
        } 
        
        private bool IsGrounded() {return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, ~LayerMask.GetMask("Player", "Ignore Raycast")); }
        
        #endregion

        #region Sprite-Related Functions

        private void ManageSprite()
        {
            if(currrentMovementVector.x != 0)
            {
                _animator.Play("Walk"); 
                _animator.speed = Mathf.Approximately(currentMoveSpeed, jogSpeed)? 1f : 2.5f;
                _capsule2d.offset = Vector2.zero;
                _capsule2d.size = new Vector2(2f, 1f);
            }
            
            if(currentViewState == ViewState.SideScrolling && currrentMovementVector.y < 0)
            {
                _animator.Play("Crouch");
                _capsule2d.offset = new Vector2(-0.2f, -0.15f);
                _capsule2d.size = new Vector2(1.5f, 1.75f);
            }
            else if(currrentMovementVector.y == 0 && currrentMovementVector.x == 0 &&!_isDead)
            {
                _animator.Play("Default"); 
                _capsule2d.offset = Vector2.zero;
                _capsule2d.size = new Vector2(2f, 1f);
            }
            
            float oritentation = currrentMovementVector.x > 0 ? 0f : -180f;
            transform.rotation = Quaternion.Euler(0f, oritentation, 0f);
            
             
            
            if(currrentMovementVector == Vector2.zero && rb2d.linearVelocity.x > 0 && !_isDead)
                _animator.Play("Default"); 
        }

        public void PlayerDeathAnimation()
        {
            _isDead = true;
            _animator.Play("Death");
        }

        #endregion
    #endregion
    
    #region Public Methods
    void SetViewState(ViewState state) { currentViewState = state; }

    #endregion
    
}
public enum ViewState
{
    SideScrolling,
    TopDown 
}
