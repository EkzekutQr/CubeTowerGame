using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesPreviewer : MonoBehaviour
{
    [SerializeField] private RectTransform _parent;

    [Inject] private ICubeFactory _cubeFactory;
    [Inject] private ICubeModelsProvider _cubeModelsProvider;

    public void ShowCubes()
    {
        foreach (Cube preview in _cubeFactory
            .CreateFromModels(_parent))
            preview.Fsm.SetState<PreviewState>();
    }
}
