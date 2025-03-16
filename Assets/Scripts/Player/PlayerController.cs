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
    
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsule2d;
    private PlayerInput _playerInput;
    private InputAction _moveAction, _jumpAction, _sprintAction;
    private SpriteRenderer spriteRenderer;
    private float currentMoveSpeed = 0f, startingDrag;
    private Vector2 currrentMovementVector;
        
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capsule2d = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
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
    private void JumpAction(InputAction.CallbackContext context) { if(IsGrounded())rb2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); } 
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
                if(currrentMovementVector.y >= 0)
                    rb2d.AddForce(new Vector2(currrentMovementVector.x * currentMoveSpeed, 0));
            }
            else
            { 
                rb2d.gravityScale = 0; 
                _jumpAction.Disable();  
                rb2d.linearDamping = linearDrag; 
                rb2d.AddForce(currrentMovementVector * currentMoveSpeed * 0.75f); 
            }
        } 
        
        private bool IsGrounded() {return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, ~LayerMask.GetMask("Player")); }
        
        #endregion

        #region Sprite-Related Functions

        private void ManageSprite()
        {
            /*PLACE HOLDER*/
            /* TODO: Implement proper logic/ aniamtion change for when the player is crocuhing */
            if(currentViewState == ViewState.SideScrolling)
            {
                if (currrentMovementVector.y < 0)
                    transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                else
                    transform.localScale = Vector3.one;
            }
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
