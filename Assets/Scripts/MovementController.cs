//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class MovementController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 5.0f;
    [SerializeField] private float m_rotationSpeed = 10.0f;

    private PlayerInput m_playerInput = null;
    private CharacterController m_controller = null;
    private Animator m_animator = null;

    private Vector2 m_currentMovementInput = Vector2.zero;
    private Vector3 m_currentMovement = Vector3.zero;
    private bool m_isMoving = false;

    private void Awake()
    {
        m_playerInput = new PlayerInput();
        m_controller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        m_playerInput.Enable();

        m_playerInput.CharacterControls.Move.performed += OnMoveInput;
        m_playerInput.CharacterControls.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        m_playerInput.Disable();
    }

    private void Update()
    {
        HandleRotation();
        m_controller.Move(m_currentMovement * m_moveSpeed * Time.deltaTime);
        HandleAnimations();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        m_currentMovementInput = context.ReadValue<Vector2>();

        m_currentMovement.x = m_currentMovementInput.x;
        m_currentMovement.z = m_currentMovementInput.y;

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

    private void HandleAnimations()
    {
        if (m_isMoving)
            m_animator.SetFloat("Speed", 1.0f);
        else if (!m_isMoving)
            m_animator.SetFloat("Speed", 0.0f);
    }
}