//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Grounded,
    Idle,
    Walk,
    Run,
    Jump
}

public class PlayerStateManager : StateManager<PlayerState>
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

        CreateStates();

        ComputeJumpGravityAndVelocity();

        m_currentState = m_states[PlayerState.Grounded];
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

    protected override void Update()
    {
        base.Update();

        HandleRotation();
        HandleMovement();
    }

    /// <summary>
    /// Create an instance of each state and populate the dictionary.
    /// </summary>
    private void CreateStates()
    {
        m_states.Add(PlayerState.Grounded, new PlayerGroundedState(PlayerState.Grounded));
        m_states.Add(PlayerState.Idle, new PlayerIdleState(PlayerState.Idle));
        m_states.Add(PlayerState.Walk, new PlayerWalkState(PlayerState.Walk));
        m_states.Add(PlayerState.Run, new PlayerRunState(PlayerState.Run));
        m_states.Add(PlayerState.Jump, new PlayerJumpState(PlayerState.Jump));
    }

    /// <summary>
    /// Compute the jump gravity and initial velocity based on max jump time and max jump height.
    /// </summary>
    private void ComputeJumpGravityAndVelocity()
    {
        float timeToApex = m_maxJumpTime / 2.0f;
        m_gravity = -2.0f * m_maxJumpHeight / Mathf.Pow(timeToApex, 2);
        m_initialJumpVelocity = 2.0f * m_maxJumpHeight / timeToApex;
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
        m_controller.Move(m_appliedMovement * Time.deltaTime);
    }
}