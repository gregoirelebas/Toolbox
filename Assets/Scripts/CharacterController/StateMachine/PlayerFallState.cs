//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerState key, PlayerStateData data) : base(key, data)
    {
    }

    public override PlayerState GetNextState()
    {
        if (m_data.CharacterController.isGrounded)
            return PlayerState.Grounded;

        return PlayerState.Fall;
    }

    public override void UpdateState()
    {
        m_data.SetVerticalCurrentMovement(m_data.CurrentMovement.y + m_data.JumpGravity * m_data.FallFactor * Time.deltaTime);

        float previousVelocity = m_data.CurrentMovement.y;
        m_data.SetVerticalAppliedMovement((previousVelocity + m_data.CurrentMovement.y) / 2.0f);

        m_data.SetHorizontalAppliedMovement(m_data.IsRunning ? m_data.RunFactor : 1.0f);
    }
}