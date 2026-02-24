using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private CubesPreviewer _cubesPreviewer;

    [Inject] private IActionLogger _actionLogger;
    [Inject] private ISaveLoadService _saveLoadService;

    private void Start()
    {
        _saveLoadService.Load();
        _cubesPreviewer.ShowCubes();
        _actionLogger.IsEnabled = true;
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            _saveLoadService.Save();
    }
#if UNITY_EDITOR
    private void OnApplicationQuit() =>
        _saveLoadService.Save();
#endif
}
