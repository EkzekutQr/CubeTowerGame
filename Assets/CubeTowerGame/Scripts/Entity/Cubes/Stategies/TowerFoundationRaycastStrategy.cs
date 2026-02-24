using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerFoundationRaycastStrategy : CubeRaycastStrategyBase
{
    public TowerFoundationRaycastStrategy(Cube owner) : base(owner)
    {
    }

    public override bool TryApply(RaycastResult result)
    {
        if (result.gameObject.TryGetComponent(out TowerFoundation towerFoundation) &&
            owner.transform.position.y > towerFoundation.transform.position.y &&
            towerFoundation.Next == null)
        {
            towerFoundation.Next = owner;
            owner.Prev = towerFoundation;

            owner.Fsm.SetState<FallingState, FallingStatePayload>(new FallingStatePayload
            {
                destinationY = towerFoundation.transform.position.y,
                nextState = typeof(TowerState),
                loggerKey = LocalizationKeys.TowerPlacementFirstKey,
            });
            return true;
        }
        return false;
    }
}
