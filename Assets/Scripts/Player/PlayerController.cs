using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jogSpeed = 5f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 1f;
    [SerializeField] private ViewState currentViewState = ViewState.SideScrolling;
    
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsule2d;
    private PlayerInput _playerInput;
    private InputAction _moveAction, _jumpAction, _sprintAction, _crouchAction;
    private SpriteRenderer spriteRenderer;
    private float currentMoveSpeed = 0f;
    private Vector2 currrentMovementVector;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capsule2d = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        currentMoveSpeed = jogSpeed;
        InitalizeActions();
         
    }

    
    void Update()
    {
        ManageMovement();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
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
        
        _crouchAction = _playerInput.actions.FindAction("Crouch");
        _crouchAction.Enable();
        _crouchAction.performed += CrouchAction;
    }
    private void SprintAction(InputAction.CallbackContext context) { currentMoveSpeed = runSpeed; }
    private void JumpAction(InputAction.CallbackContext context) { if(IsGrounded())rb2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); }
    private void CrouchAction(InputAction.CallbackContext context) { /* TODO: Implement proper logic for crouching */ }
    #endregion
    
    #region Helper Functions
    
        #region Movement-Related Functions

        private void ManageMovement()
        {
            currrentMovementVector = _moveAction.ReadValue<Vector2>();  
            if (currentViewState == ViewState.SideScrolling)
            {
                rb2d.AddForce(new Vector2(currrentMovementVector.x * currentMoveSpeed, 0));
                rb2d.gravityScale = 1; 
                _jumpAction.Enable();
            }
            else
            {
                rb2d.AddForce(currrentMovementVector * currentMoveSpeed * 0.75f);
                rb2d.gravityScale = 0; 
                _jumpAction.Disable(); 
            }
        } 
        private bool IsGrounded() {return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, ~LayerMask.GetMask("Player")); }
        
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
