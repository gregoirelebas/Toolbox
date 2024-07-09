//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class MovementController : MonoBehaviour
{
    private const float GROUND_GRAVITY = -0.5f;
    private const float AIR_GRAVITY = -9.81f;

    [SerializeField] private float m_moveSpeed = 5.0f;
    [SerializeField] private float m_rotationSpeed = 10.0f;
    [SerializeField] private float m_runFactor = 3.0f;

    private PlayerInput m_playerInput = null;
    private CharacterController m_controller = null;

    private Vector2 m_currentMovementInput = Vector2.zero;
    private Vector3 m_currentWalkMovement = Vector3.zero;
    private Vector3 m_currentRunMovement = Vector3.zero;

    private bool m_isMovement = false;
    private bool m_isRunning = false;

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
        m_playerInput.CharacterControls.Sprint.performed += OnSprint;
        m_playerInput.CharacterControls.Sprint.canceled += OnSprint;
    }

    private void OnDisable()
    {
        m_playerInput.CharacterControls.Move.performed -= OnMoveInput;
        m_playerInput.CharacterControls.Move.canceled -= OnMoveInput;
        m_playerInput.CharacterControls.Sprint.performed -= OnSprint;
        m_playerInput.CharacterControls.Sprint.canceled -= OnSprint;

        m_playerInput.Disable();
    }

    private void Update()
    {
        HandleRotation();

        HandleGravity();

        if (m_isRunning)
            m_controller.Move(m_currentRunMovement * Time.deltaTime);
        else
            m_controller.Move(m_currentWalkMovement * Time.deltaTime);

        HandleAnimations();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        m_currentMovementInput = context.ReadValue<Vector2>();

        m_currentWalkMovement.x = m_currentMovementInput.x * m_moveSpeed;
        m_currentWalkMovement.z = m_currentMovementInput.y * m_moveSpeed;

        m_currentRunMovement.x = m_currentMovementInput.x * m_moveSpeed * m_runFactor;
        m_currentRunMovement.z = m_currentMovementInput.y * m_moveSpeed * m_runFactor;

        m_isMovement = m_currentMovementInput.x != 0.0f || m_currentMovementInput.y != 0.0f;
    }

    private void HandleRotation()
    {
        if (!m_isMovement)
            return;

        Vector3 positionToLook = m_currentWalkMovement;
        positionToLook.y = 0.0f;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLook);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
    }

    private void HandleGravity()
    {
        if (m_controller.isGrounded)
        {
            m_currentWalkMovement.y = GROUND_GRAVITY;
            m_currentRunMovement.y = GROUND_GRAVITY;
        }
        else
        {
            m_currentWalkMovement.y = AIR_GRAVITY;
            m_currentRunMovement.y = AIR_GRAVITY;
        }
    }

    private void HandleAnimations()
    {
        bool isAnimatorWalking = m_animator.GetBool(m_isWalkingHash);
        bool isAnimatorRunning = m_animator.GetBool(m_isRunningHash);

        if (m_isMovement && !isAnimatorWalking)
            m_animator.SetBool(m_isWalkingHash, true);
        else if (!m_isMovement && isAnimatorWalking)
            m_animator.SetBool(m_isWalkingHash, false);

        if (m_isMovement && m_isRunning && !isAnimatorRunning)
            m_animator.SetBool(m_isRunningHash, true);
        else if (m_isMovement && !m_isRunning && isAnimatorRunning)
            m_animator.SetBool(m_isRunningHash, false);
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        m_isRunning = context.ReadValueAsButton();
    }
}