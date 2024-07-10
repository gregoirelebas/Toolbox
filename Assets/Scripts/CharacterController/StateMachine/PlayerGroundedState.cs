//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

public class PlayerGroundedState : PlayerSuperState
{
    public PlayerGroundedState(PlayerState key, PlayerState entryKey, PlayerStateData data) : base(key, entryKey, data)
    {
    }

    /// <summary>
    /// Get the default entry subState based on current context.
    /// </summary>
    /// <returns></returns>
    private PlayerState GetEntrySubState()
    {
        if (!m_data.IsMovement)
            return PlayerState.Idle;
        else if (m_data.IsRunning)
            return PlayerState.Run;
        else
            return PlayerState.Walk;
    }

    public override void EnterState()
    {
        m_entryState = GetEntrySubState();
        m_manager.SetEntryState(m_entryState);

        m_data.SetVerticalCurrentMovement(m_data.JumpGravity);
        m_data.SetVerticalAppliedMovement(m_data.JumpGravity);
    }

    public override PlayerState GetNextState()
    {
        if (m_data.IsJumpInput)
            return PlayerState.Jump;

        if (!m_data.CharacterController.isGrounded)
            return PlayerState.Fall;

        return PlayerState.Grounded;
    }
}