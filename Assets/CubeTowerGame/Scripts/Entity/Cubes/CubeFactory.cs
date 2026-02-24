using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFactory : ICubeFactory
{
    [Inject] private ICubeModelsProvider _modelsProvider;

    private Cube _prefab;
    private readonly RectTransform _rootLayout;
    private readonly Container _container;

    public ICubeModelsProvider ModelsProvider => _modelsProvider;

    public CubeFactory(Cube cubePrefab,
            RectTransform rootLayout,
            Container container)
    {
        this._prefab = cubePrefab;
        _container = container;
        _rootLayout = rootLayout;
    }

    public Cube Create(int index, RectTransform parent)
    {
        Cube newCube = Object.Instantiate(_prefab, parent);
        FSMDraggable fsm = InitializeStateMachine(newCube);
        Sprite sprite = ModelsProvider.AtIndex(index).Sprite;
        newCube.Construct(sprite, index, fsm);

        return newCube;
    }

    public Cube Create(int modelIndex)
    {
        return Create(modelIndex, _rootLayout);
    }

    public IEnumerable<Cube> CreateFromModels(RectTransform parent)
    {
        for (int i = 0; i < ModelsProvider.Count; i++)
            yield return Create(i, parent);
    }

    private FSMDraggable InitializeStateMachine(Cube owner)
    {
        PreviewState previewState = new(owner);
        AttributeInjector.Inject(previewState, _container);

        RaycastFallbackStrategy raycastFallbackStrategy = new(owner);
        AttributeInjector.Inject(raycastFallbackStrategy, _container);

        DraggingState floatingState = new(SelectRaycastStrategies(owner), raycastFallbackStrategy, owner);
        AttributeInjector.Inject(floatingState, _container);

        TowerState towerState = new(owner);
        AttributeInjector.Inject(towerState, _container);

        MissState missState = new(owner);
        AttributeInjector.Inject(missState, _container);

        HoleState holeState = new(owner);
        AttributeInjector.Inject(holeState, _container);

        FallingState fallingState = new(owner);
        AttributeInjector.Inject(fallingState, _container);

        return new FSMDraggable(new IState[]
        {
                previewState, floatingState, towerState, missState, holeState, fallingState
        });
    }

    private IEnumerable<IRaycastStrategy> SelectRaycastStrategies(Cube owner)
    {
        TowerRaycastStrategy towerRaycastStrategy = new(owner);
        AttributeInjector.Inject(towerRaycastStrategy, _container);
        yield return towerRaycastStrategy;

        TowerFoundationRaycastStrategy towerFoundationRaycastStrategy = new(owner);
        AttributeInjector.Inject(towerFoundationRaycastStrategy, _container);
        yield return towerFoundationRaycastStrategy;

        HoleRaycastStrategy holeRaycastStategy = new(owner);
        AttributeInjector.Inject(holeRaycastStategy, _container);
        yield return holeRaycastStategy;
    }
}

public interface ICubeFactory
{
    Cube Create(int modelIndex);
    Cube Create(int modelIndex, RectTransform parent);
    IEnumerable<Cube> CreateFromModels(RectTransform parent);
    ICubeModelsProvider ModelsProvider { get; }
}
