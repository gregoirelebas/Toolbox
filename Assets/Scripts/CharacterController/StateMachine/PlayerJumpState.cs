//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerState key, PlayerStateData data) : base(key, data)
    {
    }

    public override void EnterState()
    {
        m_data.SetGravity(m_data.JumpVelocity);

        m_data.Animator.SetBool(m_data.IsJumpingHash, true);
    }

    public override void ExitState()
    {
        m_data.Animator.SetBool(m_data.IsJumpingHash, false);
    }

    public override PlayerState GetNextState()
    {
        if (m_data.Controller.isGrounded)
            return PlayerState.Grounded;

        return PlayerState.Jump;
    }

    public override void UpdateState()
    {
        float previousVelocity = m_data.CurrentMovement.y;

        bool isFalling = m_data.CurrentMovement.y <= 0.0f;
        if (isFalling)
            m_data.SetCurrentMovementGravity(m_data.CurrentMovement.y + m_data.JumpGravity * m_data.FallFactor * Time.deltaTime);
        else
            m_data.SetCurrentMovementGravity(m_data.CurrentMovement.y + m_data.JumpGravity * Time.deltaTime);

        m_data.SetAppliedMovementGravity((previousVelocity + m_data.CurrentMovement.y) / 2.0f);
    }
}