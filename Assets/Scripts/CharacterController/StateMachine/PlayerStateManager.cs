//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public enum PlayerState
{
    Grounded,
    Idle,
    Walk,
    Run,
    Jump
}

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerStateManager : StateManager<PlayerState>
{
    [SerializeField] private PlayerStateData m_data = new PlayerStateData();

    private PlayerInput m_playerInput = null;
    private CharacterController m_controller = null;
    private Animator m_animator = null;

    private void Awake()
    {
        m_playerInput = new PlayerInput();
        m_controller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();

        m_data.SetCharacterController(m_controller);
        m_data.SetAnimator(m_animator);
        m_data.ComputeJumpGravityAndVelocity();

        CreateStates();

        m_currentState = m_states[PlayerState.Grounded];
    }

    private void OnEnable()
    {
        m_playerInput.Enable();

        m_playerInput.CharacterControls.Move.performed += m_data.OnMoveInput;
        m_playerInput.CharacterControls.Move.canceled += m_data.OnMoveInput;
        m_playerInput.CharacterControls.Run.performed += m_data.OnRun;
        m_playerInput.CharacterControls.Run.canceled += m_data.OnRun;
        m_playerInput.CharacterControls.Jump.performed += m_data.OnJump;
        m_playerInput.CharacterControls.Jump.canceled += m_data.OnJump;
    }

    private void OnDisable()
    {
        m_playerInput.CharacterControls.Move.performed -= m_data.OnMoveInput;
        m_playerInput.CharacterControls.Move.canceled -= m_data.OnMoveInput;
        m_playerInput.CharacterControls.Run.performed -= m_data.OnRun;
        m_playerInput.CharacterControls.Run.canceled -= m_data.OnRun;
        m_playerInput.CharacterControls.Jump.performed -= m_data.OnJump;
        m_playerInput.CharacterControls.Jump.canceled -= m_data.OnJump;

        m_playerInput.Disable();
    }

    protected override void Update()
    {
        HandleRotation();
        HandleMovement();

        base.Update();
    }

    /// <summary>
    /// Create an instance of each state and populate the dictionary.
    /// </summary>
    private void CreateStates()
    {
        m_states.Add(PlayerState.Grounded, new PlayerGroundedState(PlayerState.Grounded, m_data));
        m_states.Add(PlayerState.Idle, new PlayerIdleState(PlayerState.Idle, m_data));
        m_states.Add(PlayerState.Walk, new PlayerWalkState(PlayerState.Walk, m_data));
        m_states.Add(PlayerState.Run, new PlayerRunState(PlayerState.Run, m_data));
        m_states.Add(PlayerState.Jump, new PlayerJumpState(PlayerState.Jump, m_data));
    }

    /// <summary>
    /// Rotate the character to face the movement direction.
    /// </summary>
    private void HandleRotation()
    {
        if (!m_data.IsMovement)
            return;

        Vector3 positionToLook = m_data.CurrentMovement;
        positionToLook.y = 0.0f;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLook);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_data.RotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Move the character controller using current movement.
    /// </summary>
    private void HandleMovement()
    {
        m_controller.Move(m_data.AppliedMovement * Time.deltaTime);
    }
}