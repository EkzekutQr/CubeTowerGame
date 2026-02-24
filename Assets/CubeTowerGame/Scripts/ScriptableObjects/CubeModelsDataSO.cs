using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CubeModelsData", menuName = "ScriptableObjects/CubeModelsData")]
public class CubeModelsDataSO : ScriptableObject, ICubeModelsProvider
{
    public List<CubeModel> cubeModels;

    public List<Sprite> sprites;

    public List<CubeModel> CubeModels { get => cubeModels; }

    public int Count => cubeModels.Count;

    public CubeModel AtIndex(int index) => cubeModels[index];

    [ContextMenu("Create From Sprites")]
    private void CreateFromSprites()
    {
        cubeModels = new List<CubeModel>();
        cubeModels = sprites.ToArray().Select(s => new CubeModel(s)).ToList();
    }
}

public interface ICubeModelsProvider
{
    int Count { get; }
    CubeModel AtIndex(int index);
    List<CubeModel> CubeModels { get; }
}

[Serializable]
public class CubeModel
{
    [SerializeField] private Sprite sprite;

    public Sprite Sprite => sprite;

    public CubeModel(Sprite sp)
    {
        sprite = sp;
    }
}