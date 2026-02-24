using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoleRaycastStrategy : CubeRaycastStrategyBase
{
    public HoleRaycastStrategy(Cube owner) : base(owner)
    {
    }

    public override bool TryApply(RaycastResult result)
    {
        if (result.gameObject.TryGetComponent(out Hole hole))
        {
            owner.Fsm.SetState<FallingState, FallingStatePayload>(new FallingStatePayload
            {
                destinationY = hole.transform.position.y,
                nextState = typeof(HoleState),
                loggerKey = LocalizationKeys.HoleDropKey,
            });
            return true;
        }
        return false;
    }
}
