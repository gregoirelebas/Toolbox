//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public class FruitTree_SaplingState : FruitTree_BaseState
{
    public FruitTree_SaplingState(FruitTreeState key) : base(key)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Sapling state!");
    }

    public override FruitTreeState GetNextState()
    {
        if (ElapsedTime >= LifeTime)
            return FruitTreeState.Adult;

        return Key;
    }
}