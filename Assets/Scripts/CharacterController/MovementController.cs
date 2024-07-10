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
    [SerializeField, Range(1.0f, 10.0f)] private float m_moveSpeed = 5.0f;
    [SerializeField, Range(1.0f, 50.0f)] private float m_rotationSpeed = 10.0f;

    [Header("Run")]
    [SerializeField, Range(1.0f, 5.0f)] private float m_runFactor = 3.0f;

    [Header("Jump")]
    [SerializeField, Range(1.0f, 5.0f)] private float m_maxJumpHeight = 2.0f;
    [SerializeField, Range(0.1f, 3.0f)] private float m_maxJumpTime = 1.0f;
    [SerializeField, Range(0.1f, 5.0f)] private float m_fallFactor = 2.0f;

    private PlayerInput m_playerInput = null;
    private CharacterController m_controller = null;

    private Vector2 m_moveInput = Vector2.zero;
    private Vector3 m_currentMovement = Vector3.zero;
    private Vector3 m_appliedMovement = Vector3.zero;

    private bool m_isMovement = false;
    private bool m_isRunning = false;

    private bool m_isJumpInput = false;
    private bool m_isJumping = false;

    private float m_gravity = 0.0f;
    private float m_initialJumpVelocity = 0.0f;

    private Animator m_animator = null;
    private int m_isWalkingHash = -1;
    private int m_isRunningHash = -1;
    private int m_isJumpingHash = -1;

    private void Awake()
    {
        m_playerInput = new PlayerInput();
        m_controller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();

        m_isWalkingHash = Animator.StringToHash("isWalking");
        m_isRunningHash = Animator.StringToHash("isRunning");
        m_isJumpingHash = Animator.StringToHash("isJumping");

        float timeToApex = m_maxJumpTime / 2.0f;
        m_gravity = -2.0f * m_maxJumpHeight / Mathf.Pow(timeToApex, 2);
        m_initialJumpVelocity = 2.0f * m_maxJumpHeight / timeToApex;
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
        HandleRotation();
        HandleMovement();

        HandleGravity();
        HandleJump();

        HandleAnimations();
    }

    /// <summary>
    /// Set current movement using the context input.
    /// </summary>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        m_moveInput = context.ReadValue<Vector2>();

        m_currentMovement.x = m_moveInput.x * m_moveSpeed;
        m_currentMovement.z = m_moveInput.y * m_moveSpeed;

        m_isMovement = m_moveInput.x != 0.0f || m_moveInput.y != 0.0f;
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

        Vector3 positionToLook = m_currentMovement;
        positionToLook.y = 0.0f;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLook);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Move the character controller using current movement.
    /// </summary>
    private void HandleMovement()
    {
        if (m_isRunning)
        {
            m_appliedMovement.x = m_currentMovement.x * m_runFactor;
            m_appliedMovement.z = m_currentMovement.z * m_runFactor;
        }
        else
        {
            m_appliedMovement.x = m_currentMovement.x;
            m_appliedMovement.z = m_currentMovement.z;
        }

        m_controller.Move(m_appliedMovement * Time.deltaTime);
    }

    /// <summary>
    /// Set gravity velocity to current movement.
    /// </summary>
    private void HandleGravity()
    {
        bool isFalling = m_currentMovement.y <= 0.0f;

        if (m_controller.isGrounded)
        {
            m_currentMovement.y = GROUND_GRAVITY;
            m_appliedMovement.y = GROUND_GRAVITY;
        }
        else
        {
            float previousVelocity = m_currentMovement.y;

            if (isFalling)
                m_currentMovement.y += m_gravity * m_fallFactor * Time.deltaTime;
            else
                m_currentMovement.y += m_gravity * Time.deltaTime;

            m_appliedMovement.y = (previousVelocity + m_currentMovement.y) / 2.0f;
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

                m_currentMovement.y = m_initialJumpVelocity;
                m_appliedMovement.y = m_initialJumpVelocity;

                m_animator.SetBool(m_isJumpingHash, true);
            }
            else if (m_isJumping && !m_isJumpInput)
            {
                m_isJumping = false;

                m_animator.SetBool(m_isJumpingHash, false);
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