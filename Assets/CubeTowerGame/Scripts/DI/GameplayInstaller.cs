using Reflex.Core;
using Reflex.Enums;
using Reflex.Injectors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private CubeModelsDataSO _cubeModelsDataSO;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterFactory(CubeFactory(), Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
        containerBuilder.RegisterValue(_cubeModelsDataSO, new Type[] { typeof(ICubeModelsProvider) });
    }
    private Func<Container, ICubeFactory> CubeFactory() =>
            container =>
            {
                CubeFactory cubeFactory = new(_cubePrefab);
                AttributeInjector.Inject(cubeFactory, container);
                return cubeFactory;
            };
}
