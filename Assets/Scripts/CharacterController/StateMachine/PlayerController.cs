//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public enum PlayerState
{
    Grounded,
    Idle,
    Walk,
    Run,
    Jump,
    Fall
}

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStateData m_data = new PlayerStateData();

    private StateManager<PlayerState> m_stateManager = new StateManager<PlayerState>();

    private PlayerInput m_playerInput = null;
    private CharacterController m_controller = null;
    private Animator m_animator = null;

    private Vector3 m_cameraMovement = Vector3.zero;

    private void Awake()
    {
        m_playerInput = new PlayerInput();
        m_controller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();

        m_data.SetCharacterController(m_controller);
        m_data.SetAnimator(m_animator);
        m_data.ComputeJumpGravityAndVelocity();

        CreateStates();
    }

    private void OnEnable()
    {
        m_stateManager.SetEntryState(PlayerState.Grounded);

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

    private void Update()
    {
        HandleRotation();
        HandleMovement();

        m_stateManager.Update();
    }

    /// <summary>
    /// Create an instance of each state and populate the dictionary.
    /// </summary>
    private void CreateStates()
    {
        BaseSuperState<PlayerState> groundedState = new PlayerGroundedState(PlayerState.Grounded, PlayerState.Idle, m_data);
        m_stateManager.AddState(groundedState);

        groundedState.AddSubState(new PlayerIdleState(PlayerState.Idle, m_data));
        groundedState.AddSubState(new PlayerWalkState(PlayerState.Walk, m_data));
        groundedState.AddSubState(new PlayerRunState(PlayerState.Run, m_data));

        m_stateManager.AddState(new PlayerJumpState(PlayerState.Jump, m_data));
        m_stateManager.AddState(new PlayerFallState(PlayerState.Fall, m_data));
    }

    /// <summary>
    /// Rotate the character to face the movement direction.
    /// </summary>
    private void HandleRotation()
    {
        if (!m_data.IsMovement)
            return;

        Vector3 positionToLook = m_cameraMovement;
        positionToLook.y = 0.0f;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLook);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_data.RotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Move the character controller using current movement.
    /// </summary>
    private void HandleMovement()
    {
        m_cameraMovement = ConverToCameraSpace(m_data.AppliedMovement);
        m_cameraMovement.y = m_data.AppliedMovement.y;
        m_controller.Move(m_cameraMovement * Time.deltaTime);
    }

    private Vector3 ConverToCameraSpace(Vector3 vector)
    {
        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 cameraForward = Camera.main.transform.forward;

        cameraRight.y = 0.0f;
        cameraForward.y = 0.0f;

        cameraRight = cameraRight.normalized;
        cameraForward = cameraForward.normalized;

        Vector3 xProduct = cameraRight * vector.x;
        Vector3 zProduct = cameraForward * vector.z;

        return xProduct + zProduct;
    }
}