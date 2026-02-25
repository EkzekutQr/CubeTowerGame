using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CubeRaycastStrategyBase : IRaycastStrategy
{
    [Inject] protected IActionLogger actionLogger;

    protected readonly Cube owner;

    protected CubeRaycastStrategyBase(Cube owner)
    {
        this.owner = owner;
    }

    public abstract bool TryApply(RaycastResult result);
}