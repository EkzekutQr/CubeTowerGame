using JetBrains.Annotations;
using PrimeTween;
using System;
using UnityEngine;

public class MissState : CubeStateBase
{
    public MissState(Cube owner) : base(owner)
    {
    }

    public override void Enter()
    {
        Tween.StopAll(owner.transform);
        Tween
            .Scale(owner.transform, tweenSettingsLib.missSettings)
            .OnComplete(() =>
        UnityEngine.Object.Destroy(owner.gameObject));
    }
}

public class HoleState : CubeStateBase
{
    public HoleState(Cube owner) : base(owner)
    {
    }

    public override void Enter()
    {
        Tween.StopAll(owner.transform);

        TweenSettings<Vector3> missSettings = tweenSettingsLib.missSettings;
        missSettings.settings.duration = tweenSettingsLib.holeSettings.settings.duration;

        Sequence.Create()
            .Group(Tween.Scale(owner.transform, missSettings))
            .Group(Tween.Rotation(owner.transform, tweenSettingsLib.holeSettings))
        .OnComplete(() => UnityEngine.Object.Destroy(owner.gameObject));
    }
}

public struct FallingStatePayload
{
    public Vector2 destination;
    [CanBeNull] public float? destinationY;

    public Type nextState;
    [CanBeNull] public string loggerKey;
}

public class FallingState : CubeStateBase, IPayloadedState<FallingStatePayload>
{
    public FallingState(Cube owner) : base(owner)
    {
    }

    public void OnEnter(FallingStatePayload payload)
    {
        if (payload.destinationY != null)
            payload.destination = owner.transform.position.With(y: payload.destinationY);

        Debug.Assert(payload.nextState != null);

        Tween.PositionAtSpeed(
                owner.transform,
                payload.destination,
                tweenSettingsLib.fallSpeed,
                tweenSettingsLib.fallEase)
            .OnComplete(() =>
            {
                if (!string.IsNullOrEmpty(payload.loggerKey))
                    actionLogger.Log(payload.loggerKey);

                owner.Fsm.SetState(payload.nextState);
            });
    }
}