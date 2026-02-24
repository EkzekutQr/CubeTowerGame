using PrimeTween;
using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class CubeDraggableState : CubeStateBase, IDraggableState
{
    protected CubeDraggableState(Cube owner) : base(owner)
    {
    }

    public virtual void OnBeginDrag(PointerEventData eventData) { }

    public virtual void OnDrag(PointerEventData eventData) { }

    public virtual void OnEndDrag(PointerEventData eventData) { }
}

public class PreviewState : CubeDraggableState
{
    private const float DragDotThreshold = 0.35f;

    [Inject] private ScrollRect _scroll;
    [Inject] private ICubeFactory _factory;

    private bool _passDrag;
    private Cube copy;

    public PreviewState(Cube owner) : base(owner)
    {
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 dragDirection = (eventData.position - eventData.pressPosition).normalized;
        _passDrag = Vector2.Dot(dragDirection, Vector2.up) < DragDotThreshold;

        if (_passDrag)
            _scroll.OnBeginDrag(eventData);
        else
            CreateCopy(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (_passDrag)
            _scroll.OnDrag(eventData);
        else
            copy.Fsm.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (_passDrag)
            _scroll.OnEndDrag(eventData);
        else
            copy.Fsm.OnEndDrag(eventData);
    }

    private void CreateCopy(PointerEventData eventData)
    {
        copy = _factory.Create(owner.SpriteIndex);
        copy.Fsm.SetState<DraggingState>();
        copy.Fsm.OnBeginDrag(eventData);
    }
}

public class DraggingState : CubeDraggableState
{
    private const float Step = 20f;

    [Inject] private Canvas _canvas;
    [Inject] private GraphicRaycaster _raycaster;

    private readonly IEnumerable<IRaycastStrategy> _raycastStrategies;
    private readonly IRaycastFallbackStrategy _raycastFallbackStrategy;

    public DraggingState(
        IEnumerable<IRaycastStrategy> raycastStrategies,
        IRaycastFallbackStrategy raycastFallbackStrategy,
        Cube owner) : base(owner)
    {
        _raycastFallbackStrategy = raycastFallbackStrategy;
        _raycastStrategies = raycastStrategies;
    }

    public override void Enter()
    {
        owner.transform.SetAsLastSibling();

        Tween.Scale(owner.transform, tweenSettingsLib.scaleInSettings);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        owner.RectTransform.anchoredPosition = eventData.position / _canvas.scaleFactor;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Tween.Scale(owner.transform, tweenSettingsLib.scaleOutSettings);

        List<RaycastResult> results = Raycast(eventData.position, Vector2.down, eventData.position.y, Screen.height / Step);
        ApplyStrategies(results);
    }

    private List<RaycastResult> Raycast(Vector2 position, Vector2 direction, float distance, float step)
    {
        var results = new List<RaycastResult>();

        while (distance > 0)
        {
            var data = new PointerEventData(EventSystem.current);
            data.position = position;

            _raycaster.Raycast(data, results);

            distance -= step;
            position += direction * step;
        }
        return results;
    }

    private void ApplyStrategies(List<RaycastResult> results)
    {
        foreach (RaycastResult result in results)
        {
            foreach (IRaycastStrategy strategy in _raycastStrategies)
            {
                if (strategy.TryApply(result))
                    return;
            }
        }
        _raycastFallbackStrategy.Apply();
    }
}

public interface IRaycastStrategy
{
    bool TryApply(RaycastResult result);
}
public interface IRaycastFallbackStrategy
{
    void Apply();
}

public class TowerState : CubeDraggableState
{
    public TowerState(Cube owner) : base(owner)
    {
    }

    public override void Enter()
    {
        if (owner.Prev is not Cube prevCube)
            return;

        if (prevCube.IntersectsX(owner))
            return;

        DiscardUp(owner);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        PokeTree();

        owner.Fsm.SetState<DraggingState>();
    }

    private void PokeTree()
    {
        int i = 0;
        const int max = 50;

        INode node = owner;
        INode nextNode = owner.Next;

        if (node.Prev != null)
            node.Prev.Next = nextNode;
        if (nextNode != null)
            nextNode.Prev = node.Prev;

        while (nextNode != null && i < max)
        {
            if (node is Cube cube && nextNode is Cube nextCube)
                nextCube.Fsm.SetState<FallingState, FallingStatePayload>(new FallingStatePayload
                {
                    destinationY = cube.transform.position.y,
                    nextState = typeof(TowerState)
                });

            node = nextNode;
            nextNode = node.Next;

            i++;
            if (i > max - 1)
                Debug.LogError("Overflow");
        }
        owner.Next = null;
        owner.Prev = null;
    }

    private void DiscardUp(INode from)
    {
        actionLogger.Log(LocalizationKeys.TowerFallenKey);

        INode node = from;

        while (node != null)
        {
            if (node is Cube cube)
                cube.Fsm.SetState<MissState>();

            node.Prev.Next = null;
            node = node.Next;
        }
    }
}

public interface INode
{
    INode Next { get; set; }
    INode Prev { get; set; }
}

public abstract class CubeStateBase : MonoStateBase<Cube>
{
    [Inject] protected TweenSettingsLibrary tweenSettingsLib;
    [Inject] protected IActionLogger actionLogger;

    protected CubeStateBase(Cube owner) : base(owner)
    {
    }
}