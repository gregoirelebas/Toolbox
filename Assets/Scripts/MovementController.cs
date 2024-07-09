//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class MovementController : MonoBehaviour
{
    private const float GROUND_GRAVITY = -0.5f;
    private const float AIR_GRAVITY = -9.81f;

    [Header("Basic movement")]
    [SerializeField] private float m_moveSpeed = 5.0f;
    [SerializeField] private float m_rotationSpeed = 10.0f;

    [Header("Run")]
    [SerializeField] private float m_runFactor = 3.0f;

    [Header("Jump")]
    [SerializeField] private float m_maxJumpHeight = 2.0f;
    [SerializeField] private float m_maxJumpTime = 1.0f;

    private PlayerInput m_playerInput = null;
    private CharacterController m_controller = null;

    private Vector2 m_currentMovementInput = Vector2.zero;
    private Vector3 m_currentWalkMovement = Vector3.zero;
    private Vector3 m_currentRunMovement = Vector3.zero;

    private bool m_isMovement = false;
    private bool m_isRunning = false;

    private bool m_isJumpInput = false;
    private bool m_isJumping = false;
    private float m_initialJumpVelocity = 10.0f;
    private float m_gravity = AIR_GRAVITY;

    private Animator m_animator = null;
    private int m_isWalkingHash = -1;
    private int m_isRunningHash = -1;

    private void Awake()
    {
        m_playerInput = new PlayerInput();
        m_controller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();

        m_isWalkingHash = Animator.StringToHash("isWalking");
        m_isRunningHash = Animator.StringToHash("isRunning");
    }

    private void OnEnable()
    {
        m_playerInput.Enable();

        m_playerInput.CharacterControls.Move.performed += OnMoveInput;
        m_playerInput.CharacterControls.Move.canceled += OnMoveInput;
        m_playerInput.CharacterControls.Run.performed += OnRun;
        m_playerInput.CharacterControls.Run.canceled += OnRun;
        m_playerInput.CharacterControls.Jump.performed += OnJump;
        m_playerInput.CharacterControls.Jump.canceled += OnJump;
    }

    private void OnDisable()
    {
        m_playerInput.CharacterControls.Move.performed -= OnMoveInput;
        m_playerInput.CharacterControls.Move.canceled -= OnMoveInput;
        m_playerInput.CharacterControls.Run.performed -= OnRun;
        m_playerInput.CharacterControls.Run.canceled -= OnRun;
        m_playerInput.CharacterControls.Jump.performed -= OnJump;
        m_playerInput.CharacterControls.Jump.canceled -= OnJump;

        m_playerInput.Disable();
    }

    private void Update()
    {
        float timeToApex = m_maxJumpTime / 2.0f;
        m_gravity = -2.0f * m_maxJumpHeight / Mathf.Pow(timeToApex, 2);
        m_initialJumpVelocity = 2.0f * m_maxJumpHeight / timeToApex;

        HandleRotation();

        if (m_isRunning)
            m_controller.Move(m_currentRunMovement * Time.deltaTime);
        else
            m_controller.Move(m_currentWalkMovement * Time.deltaTime);

        HandleGravity();
        HandleJump();

        HandleAnimations();
    }

    /// <summary>
    /// Set walk and run movement using the context input read.
    /// </summary>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        m_currentMovementInput = context.ReadValue<Vector2>();

        m_currentWalkMovement.x = m_currentMovementInput.x * m_moveSpeed;
        m_currentWalkMovement.z = m_currentMovementInput.y * m_moveSpeed;

        m_currentRunMovement.x = m_currentMovementInput.x * m_moveSpeed * m_runFactor;
        m_currentRunMovement.z = m_currentMovementInput.y * m_moveSpeed * m_runFactor;

        m_isMovement = m_currentMovementInput.x != 0.0f || m_currentMovementInput.y != 0.0f;
    }

    /// <summary>
    /// Cache the run input as a boolean.
    /// </summary>
    private void OnRun(InputAction.CallbackContext context)
    {
        m_isRunning = context.ReadValueAsButton();
    }

    /// <summary>
    /// Cache the jump input as a boolean.
    /// </summary>
    private void OnJump(InputAction.CallbackContext context)
    {
        m_isJumpInput = context.ReadValueAsButton();
    }

    /// <summary>
    /// Rotate the character to face the movement direction.
    /// </summary>
    private void HandleRotation()
    {
        if (!m_isMovement)
            return;

        Vector3 positionToLook = m_currentWalkMovement;
        positionToLook.y = 0.0f;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLook);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Set gravity if grounded or increase gravity if mid air.
    /// </summary>
    private void HandleGravity()
    {
        if (m_controller.isGrounded)
        {
            m_currentWalkMovement.y = GROUND_GRAVITY;
            m_currentRunMovement.y = GROUND_GRAVITY;
        }
        else
        {
            float oldVelocity = m_currentWalkMovement.y;
            float newVelocity = m_currentWalkMovement.y + m_gravity * Time.deltaTime;
            float averageVelocity = (oldVelocity + newVelocity) / 2.0f;

            m_currentWalkMovement.y = averageVelocity;
            m_currentRunMovement.y = averageVelocity;
        }
    }

    /// <summary>
    /// Set jump velocity if character is grounded.
    /// </summary>
    private void HandleJump()
    {
        if (m_controller.isGrounded)
        {
            if (!m_isJumping && m_isJumpInput)
            {
                m_isJumping = true;

                float previousVelocity = m_currentWalkMovement.y;
                float newVelocity = m_currentWalkMovement.y + m_initialJumpVelocity;
                float averageVelocity = (previousVelocity + newVelocity) / 2.0f;

                m_currentWalkMovement.y = averageVelocity;
                m_currentRunMovement.y = averageVelocity;
            }
            else if (m_isJumping && !m_isJumpInput)
            {
                m_isJumping = false;
            }
        }
    }

    /// <summary>
    /// Set walk and run animation variables.
    /// </summary>
    private void HandleAnimations()
    {
        bool isAnimatorWalking = m_animator.GetBool(m_isWalkingHash);
        bool isAnimatorRunning = m_animator.GetBool(m_isRunningHash);

        if (m_isMovement && !isAnimatorWalking)
            m_animator.SetBool(m_isWalkingHash, true);
        else if (!m_isMovement && isAnimatorWalking)
            m_animator.SetBool(m_isWalkingHash, false);

        if (m_isRunning && !isAnimatorRunning)
            m_animator.SetBool(m_isRunningHash, true);
        else if (!m_isRunning && isAnimatorRunning)
            m_animator.SetBool(m_isRunningHash, false);
    }
}