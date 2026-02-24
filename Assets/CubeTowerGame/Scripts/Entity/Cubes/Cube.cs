using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cube : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, INode
{
    [SerializeField] private Image _image;

    public Bounds WorldBounds => CalcUnscaledWorldBounds();
    public RectTransform RectTransform => (RectTransform)transform;

    private FSMDraggable _fsm;
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;



    public int SpriteIndex { get; private set; }
    public FSMDraggable Fsm { get => _fsm; private set => _fsm = value; }
    public INode Next { get; set; }
    public INode Prev { get; set; }

    public void Construct(Sprite sprite, int spriteIndex, FSMDraggable fSMDraggable)
    {
        _image.sprite = sprite;
        SpriteIndex = spriteIndex;
        _fsm = fSMDraggable;

        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData){
        Debug.Log(_fsm);
        _fsm.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData) =>
        _fsm.OnDrag(eventData);

    public void OnEndDrag(PointerEventData eventData) =>
        _fsm.OnEndDrag(eventData);

    public bool IntersectsX(Cube other, float sizeMult = .5f)
    {
        Bounds bounds = WorldBounds;
        bounds.center = bounds.center.With(y: other.WorldBounds.center.y);
        bounds.size *= sizeMult;

        return other.WorldBounds.Intersects(bounds);
    }

    private Bounds CalcUnscaledWorldBounds()
    {
        Vector3[] worldCorners = new Vector3[4];
        RectTransform.GetWorldCorners(worldCorners);

        Vector3 min = worldCorners[0];
        Vector3 max = worldCorners[2];

        Vector3 size = max - min;
        size.x /= transform.localScale.x;
        size.y /= transform.localScale.y;

        return new Bounds((min + max) / 2, size);
    }
}
