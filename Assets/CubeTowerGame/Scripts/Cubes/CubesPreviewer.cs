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
        foreach (Sprite cubeModel in _cubeModelsProvider.CubeModels)
        {
            Cube cube = _cubeFactory.CreateCube(cubeModel, _parent);
        }
    }
}
