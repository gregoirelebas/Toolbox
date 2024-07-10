//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public class FruitTree_BlossomingState : FruitTree_BaseState
{
    public FruitTree_BlossomingState(FruitTreeState key) : base(key)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Blossoming state!");
    }

    public override FruitTreeState GetNextState()
    {
        if (ElapsedTime >= LifeTime)
            return FruitTreeState.Fruity;

        return Key;
    }
}