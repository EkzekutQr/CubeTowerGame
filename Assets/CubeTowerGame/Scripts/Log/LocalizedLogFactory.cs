using PrimeTween;
using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IActionLogger
{
    bool IsEnabled { get; set; }
    void Log(string text);
}

public class LocalizedLogFactory : IActionLogger
{
    [Inject] private TweenSettingsLibrary tweenSettingsLibrary;

    public bool IsEnabled { get; set; }

    private readonly string _table;
    private readonly LocalizedLogMessage _localizedLogMessagePrefab;
    private readonly RectTransform _layout;

    public LocalizedLogFactory(string table, LocalizedLogMessage localizedLogMessagePrefab, RectTransform layout)
    {
        _table = table;
        _localizedLogMessagePrefab = localizedLogMessagePrefab;
        _layout = layout;
    }

    public void Log(string key)
    {
        Debug.Log($"Logging action with key: {key}");
        if (!IsEnabled)
            return;
        Debug.Log($"Logging action with key: {key}");

        LocalizedLogMessage localizedLogMessage = Object.Instantiate(_localizedLogMessagePrefab, _layout);

        localizedLogMessage.SetText(_table, key);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_layout);

        Sequence.Create()
            .Chain(Tween.Scale(localizedLogMessage.transform, tweenSettingsLibrary.logInSettings))
            .ChainDelay(tweenSettingsLibrary.logLifetime)
            .Chain(Tween.Scale(localizedLogMessage.transform, tweenSettingsLibrary.logOutSettings))
            .OnComplete(() => Object.Destroy(localizedLogMessage.gameObject));
    }
}
