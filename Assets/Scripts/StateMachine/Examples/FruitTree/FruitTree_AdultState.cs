//Script created by Grégoire Lebas. See more at https://github.com/gregoirelebas

using UnityEngine;

public class FruitTree_AdultState : BaseSuperState<FruitTreeState>
{
    public FruitTree_AdultState(FruitTreeState key, FruitTreeState entryState) : base(key, entryState)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Adult state!");

        base.EnterState();
    }
}