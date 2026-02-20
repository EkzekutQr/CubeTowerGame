using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private CubesPreviewer _cubesPreviewer;

    private void Start()
    {
        _cubesPreviewer.ShowCubes();
    }
}
