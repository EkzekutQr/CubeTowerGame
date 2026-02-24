using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFoundation : MonoBehaviour, INode, ISaveDataReader, ISaveDataWriter
{
    private ICubeFactory _cubeFactory;
    public INode Next { get; set; }
    public INode Prev { get; set; }

    [Inject]
    private void Construct(
        ISaveLoadService saveLoadService,
        ICubeFactory cubeFactory)
    {
        saveLoadService.RegisterReader(this);
        saveLoadService.RegisterWriter(this);

        _cubeFactory = cubeFactory;
    }

    public void Read(SaveData saveData)
    {
        INode node = this;

        foreach (var nodeData in saveData.towerNodeDataSet)
        {
            Cube cube = _cubeFactory.Create(nodeData.modelIndex);

            cube.transform.position = nodeData.position;
            cube.Fsm.SetState<TowerState>();

            cube.Prev = node;
            node.Next = cube;

            node = cube;
        }
    }

    public void Write(SaveData saveData)
    {
        List<TowerNodeData> towerNodeDataSet = new();
        INode node = Next;

        while (node != null)
        {
            if (node is Cube cube)
            {
                TowerNodeData nodeData = new()
                {
                    position = cube.transform.position,
                    modelIndex = cube.SpriteIndex
                };
                towerNodeDataSet.Add(nodeData);
            }
            node = node.Next;
        }
        saveData.towerNodeDataSet = towerNodeDataSet;
    }
}
