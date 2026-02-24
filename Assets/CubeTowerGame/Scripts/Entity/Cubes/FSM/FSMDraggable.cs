using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FSMDraggable : FSM<IState>
{
    public FSMDraggable(IEnumerable<IState> states) : base(states)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CurrentState is IDraggableState draggableState)
            draggableState.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CurrentState is IDraggableState draggableState)
            draggableState.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (CurrentState is IDraggableState draggableState)
            draggableState.OnEndDrag(eventData);
    }
}

public interface IDraggableState : IState
{
    void OnBeginDrag(PointerEventData eventData);
    void OnDrag(PointerEventData eventData);
    void OnEndDrag(PointerEventData eventData);
}
