using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CubeModelsData", menuName = "ScriptableObjects/CubeModelsData")]
public class CubeModelsDataSO : ScriptableObject, ICubeModelsProvider
{
    public List<Sprite> cubeModels;

    public List<Sprite> CubeModels { get => cubeModels; }
}

public interface ICubeModelsProvider
{
    List<Sprite> CubeModels { get; }
}