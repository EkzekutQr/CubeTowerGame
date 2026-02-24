using Reflex.Core;
using Reflex.Enums;
using Reflex.Injectors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Resolution = Reflex.Enums.Resolution;

public class GameplayInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private CubeModelsDataSO _cubeModelsDataSO;
    [SerializeField] private ScrollRect previewsScrollRect;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private string localizationTable;
    [SerializeField] private RectTransform logLayout;
    [SerializeField] private LocalizedLogMessage logMessagePrefab;
    [SerializeField] private RectTransform rootLayout;

    public void InstallBindings(ContainerBuilder containerBuilder) =>
            containerBuilder
                .RegisterValue(_cubeModelsDataSO, typeof(ICubeModelsProvider).ToArray())
                .RegisterValue(previewsScrollRect)
                .RegisterValue(canvas)
                .RegisterValue(raycaster)
                .RegisterFactory(LogFactory(), Lifetime.Singleton, Resolution.Lazy)
                .RegisterFactory(CubeFactory(), Lifetime.Singleton, Resolution.Lazy);
    private Func<Container, ICubeFactory> CubeFactory() =>
            container =>
            {
                CubeFactory cubeFactory = new(_cubePrefab, rootLayout, container);
                AttributeInjector.Inject(cubeFactory, container);
                return cubeFactory;
            };
    private Func<Container, IActionLogger> LogFactory() =>
            container =>
            {
                LocalizedLogFactory localizedLogFactory = new(localizationTable, logMessagePrefab, logLayout);
                AttributeInjector.Inject(localizedLogFactory, container);
                return localizedLogFactory;
            };
}

public static class Extensions
{
    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }

    public static Type[] ToArray(this Type type) =>
        new[] { type };
}
