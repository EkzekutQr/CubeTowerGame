using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cube : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _image;

    private RectTransform _rectTransform;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    private FSM _fsm;

    private PreviewState previewState;
    private DraggingState draggingState;
    private TowerState towerState;

    public void Construct(Sprite sprite)
    {
        _image.sprite = sprite;
    }
    private void Awake()
    {
        _fsm = new FSM();

        previewState = new PreviewState(this, _fsm);
        draggingState = new DraggingState(this, _fsm);
        towerState = new TowerState(this, _fsm);

        _fsm.ChangeState(previewState);

        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_fsm.CurrentState is PreviewState)
            _fsm.ChangeState(draggingState);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_fsm.CurrentState is DraggingState)
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_fsm.CurrentState is DraggingState dragging)
            dragging.HandleRelease();
    }

    public void SetToTower()
    {
        _fsm.ChangeState(towerState);
    }

    public void ResetToPreview()
    {
        _fsm.ChangeState(previewState);
    }
}
