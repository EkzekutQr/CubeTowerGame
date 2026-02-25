using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class LocalizedLogMessage : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent localizeStringEvent;

    public void SetText(string table, string key)
    {
        LocalizedString localizedString = new();
        localizedString.SetReference(table, key);

        localizeStringEvent.StringReference = localizedString;
        localizeStringEvent.RefreshString();
    }
}
