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
    [SerializeField] private float m_sprintFactor = 3.0f;

    private PlayerInput m_playerInput = null;
    private CharacterController m_controller = null;

    private Vector2 m_currentMovementInput = Vector2.zero;
    private Vector3 m_currentMovement = Vector3.zero;
    private Vector3 m_currentSprintMovement = Vector3.zero;

    private bool m_isMoving = false;
    private bool m_isSprinting = false;

    private Animator m_animator = null;
    private int m_speedHash = -1;

    private void Awake()
    {
        m_playerInput = new PlayerInput();
        m_controller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();

        m_speedHash = Animator.StringToHash("Speed");
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

        if (m_isSprinting)
            m_controller.Move(m_currentSprintMovement * Time.deltaTime);
        else
            m_controller.Move(m_currentMovement * Time.deltaTime);

        HandleAnimations();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        m_currentMovementInput = context.ReadValue<Vector2>();

        m_currentMovement.x = m_currentMovementInput.x * m_moveSpeed;
        m_currentMovement.z = m_currentMovementInput.y * m_moveSpeed;

        m_currentSprintMovement.x = m_currentMovementInput.x * m_moveSpeed * m_sprintFactor;
        m_currentSprintMovement.z = m_currentMovementInput.y * m_moveSpeed * m_sprintFactor;

        m_isMoving = m_currentMovementInput.x != 0.0f || m_currentMovementInput.y != 0.0f;
    }

    private void HandleRotation()
    {
        if (!m_isMoving)
            return;

        Vector3 positionToLook = m_currentMovement;
        positionToLook.y = 0.0f;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLook);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
    }

    private void HandleGravity()
    {
        if (m_controller.isGrounded)
        {
            m_currentMovement.y = GROUND_GRAVITY;
            m_currentSprintMovement.y = GROUND_GRAVITY;
        }
        else
        {
            m_currentMovement.y = AIR_GRAVITY;
            m_currentSprintMovement.y = AIR_GRAVITY;
        }
    }

    private void HandleAnimations()
    {
        if (m_isMoving)
        {
            if (m_isSprinting)
                m_animator.SetFloat(m_speedHash, 1.0f);
            else
                m_animator.SetFloat(m_speedHash, 0.25f);
        }
        else
        {
            m_animator.SetFloat(m_speedHash, 0.0f);
        }
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        m_isSprinting = context.ReadValueAsButton();
    }
}