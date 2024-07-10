//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerStateData
{
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

    private CharacterController m_controller = null;

    private Vector2 m_moveInput = Vector2.zero;
    private Vector3 m_currentMovement = Vector3.zero;
    private Vector3 m_appliedMovement = Vector3.zero;

    private bool m_isMovement = false;
    private bool m_isRunning = false;

    private bool m_isJumpInput = false;
    private bool m_isJumping = false;

    private float m_jumpGravity = 0.0f;
    private float m_initialJumpVelocity = 0.0f;

    private Animator m_animator = null;
    private int m_isWalkingHash = -1;
    private int m_isRunningHash = -1;
    private int m_isJumpingHash = -1;

    public CharacterController Controller => m_controller;

    public float RotationSpeed => m_rotationSpeed;

    public Vector3 CurrentMovement => m_currentMovement;
    public Vector3 AppliedMovement => m_appliedMovement;

    public float JumpGravity => m_jumpGravity;
    public float FallFactor => m_fallFactor;
    public float JumpVelocity => m_initialJumpVelocity;

    public bool IsMovement => m_isMovement;
    public bool IsRunning => m_isRunning;
    public bool IsJumpInput => m_isJumpInput;
    public bool IsJumping => m_isJumping;

    /// <summary>
    /// Set the character controller for states to use.
    /// </summary>
    public void SetCharacterController(CharacterController controller)
    {
        m_controller = controller;
    }

    /// <summary>
    /// Compute the jump gravity and initial velocity based on max jump time and max jump height.
    /// </summary>
    public void ComputeJumpGravityAndVelocity()
    {
        float timeToApex = m_maxJumpTime / 2.0f;
        m_jumpGravity = -2.0f * m_maxJumpHeight / Mathf.Pow(timeToApex, 2);
        m_initialJumpVelocity = 2.0f * m_maxJumpHeight / timeToApex;
    }

    /// <summary>
    /// Set current movement using the context input.
    /// </summary>
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        m_moveInput = context.ReadValue<Vector2>();

        m_currentMovement.x = m_moveInput.x * m_moveSpeed;
        m_currentMovement.z = m_moveInput.y * m_moveSpeed;

        m_isMovement = m_moveInput.x != 0.0f || m_moveInput.y != 0.0f;
    }

    /// <summary>
    /// Cache the run input as a boolean.
    /// </summary>
    public void OnRun(InputAction.CallbackContext context)
    {
        m_isRunning = context.ReadValueAsButton();
    }

    /// <summary>
    /// Cache the jump input as a boolean.
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        m_isJumpInput = context.ReadValueAsButton();
    }

    /// <summary>
    /// Set the Animator reference for states to use.
    /// </summary>
    public void SetAnimator(Animator animator)
    {
        m_animator = animator;

        m_isWalkingHash = Animator.StringToHash("isWalking");
        m_isRunningHash = Animator.StringToHash("isRunning");
        m_isJumpingHash = Animator.StringToHash("isJumping");
    }

    /// <summary>
    /// Set both current and applied movement y vector value.
    /// </summary>
    public void SetGravity(float gravity)
    {
        m_currentMovement.y = gravity;
        m_appliedMovement.y = gravity;
    }

    /// <summary>
    /// Set current movement y vector value.
    /// </summary>
    public void SetCurrentMovementGravity(float gravity)
    {
        m_currentMovement.y = gravity;
    }

    /// <summary>
    /// Set applied movement y vector value.
    /// </summary>
    public void SetAppliedMovementGravity(float gravity)
    {
        m_appliedMovement.y = gravity;
    }
}