//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    private const float GROUND_GRAVITY = -0.5f;

    public PlayerGroundedState(PlayerState key, PlayerStateData data) : base(key, data)
    {
    }

    public override void EnterState()
    {
        m_data.SetGravity(GROUND_GRAVITY);
    }

    public override void ExitState()
    {
    }

    public override PlayerState GetNextState()
    {
        if (m_data.IsJumpInput)
            return PlayerState.Jump;

        return PlayerState.Grounded;
    }

    public override void InitializeSubState()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerEnter(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerExit(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public override void SetSubState()
    {
        throw new System.NotImplementedException();
    }

    public override void SetSuperState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {

    }
}