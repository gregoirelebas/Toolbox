//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public class FruitTree_SeedState : FruitTree_BaseState
{
    public FruitTree_SeedState(FruitTreeState key) : base(key)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        Debug.Log("Seed state!");
    }

    public override FruitTreeState GetNextState()
    {
        if (ElapsedTime >= LifeTime)
            return FruitTreeState.Sapling;

        return Key;
    }
}