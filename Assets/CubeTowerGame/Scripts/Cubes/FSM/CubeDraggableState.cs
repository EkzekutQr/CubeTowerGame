using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDraggableState : MonoBehaviour
{

}

public class PreviewState : IState
{
    private Cube _cube;
    private FSM _fsm;

    public PreviewState(Cube cube, FSM fsm)
    {
        this._cube = cube;
        this._fsm = fsm;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }
}

public class DraggingState : IState
{
    private Cube _cube;
    private FSM _fsm;

    public DraggingState(Cube cube, FSM fsm)
    {
        this._cube = cube;
        this._fsm = fsm;
    }

    public void Enter()
    {
        //Vector2 dragDirection = (eventData.position - eventData.pressPosition).normalized;
        //_passDrag = Vector2.Dot(dragDirection, Vector2.up) < DragDotThreshold;

        //if (_passDrag)
        //    _scroll.OnBeginDrag(eventData);
        //else
        //    CreateCopy(eventData);
    }

    public void Exit()
    {
    }

    public void HandleRelease()
    {
        //if (TowerManager.Instance.TryPlace(cube))
        //{
        //    fsm.ChangeState(cube.GetComponent<CubeDraggable>().towerState);
        //    return;
        //}

        //if (HoleManager.Instance.TryDrop(cube))
        //{
        //    Object.Destroy(cube.gameObject);
        //    return;
        //}

        //GameController.Instance.ShowMessage("Miss!");

        Object.Destroy(_cube.gameObject);
    }
}

public class TowerState : IState
{
    private Cube _cube;
    private FSM _fsm;

    public TowerState(Cube cube, FSM fsm)
    {
        this._cube = cube;
        this._fsm = fsm;
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }
}