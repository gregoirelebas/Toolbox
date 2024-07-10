//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerState key, PlayerStateData data) : base(key, data)
    {
    }

    public override void EnterState()
    {
        m_data.SetVerticalCurrentMovement(m_data.JumpVelocity);
        m_data.SetVerticalAppliedMovement(m_data.JumpVelocity);

        m_data.Animator.SetBool(m_data.IsJumpingHash, true);
    }

    public override void ExitState()
    {
        m_data.Animator.SetBool(m_data.IsJumpingHash, false);
    }

    public override PlayerState GetNextState()
    {
        if (m_data.IsCharacterGrounded)
            return PlayerState.Grounded;
        else if (m_data.CurrentMovement.y <= 0.0f)
            return PlayerState.Fall;

        return PlayerState.Jump;
    }

    public override void UpdateState()
    {
        m_data.SetVerticalCurrentMovement(m_data.CurrentMovement.y + m_data.JumpGravity * Time.deltaTime);

        float previousVelocity = m_data.CurrentMovement.y;
        m_data.SetVerticalAppliedMovement((previousVelocity + m_data.CurrentMovement.y) / 2.0f);

        m_data.SetHorizontalAppliedMovement(m_data.IsRunning ? m_data.RunFactor : 1.0f);
    }
}