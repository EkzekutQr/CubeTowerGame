using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFactory : ICubeFactory
{
    private Cube _prefab;

    public CubeFactory(Cube cubePrefab)
    {
        this._prefab = cubePrefab;
    }

    public Cube CreateCube(Sprite sprite, Transform parent)
    {
        Debug.Log(sprite);
        Cube cube = GameObject.Instantiate(_prefab, parent);
        cube.Construct(sprite);

        return cube;
    }
}

public interface ICubeFactory
{
    Cube CreateCube(Sprite sprite, Transform parent);
}
