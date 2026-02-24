using PrimeTween;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class RaycastFallbackStrategy: IRaycastFallbackStrategy
{
    protected readonly Cube owner;
    [Inject] protected IActionLogger actionLogger;

    public RaycastFallbackStrategy(Cube owner)
    {
        this.owner = owner;
    }

    public void Apply()
    {
        owner.Fsm.SetState<MissState>();
        actionLogger.Log(LocalizationKeys.MissKey);
    }
}



