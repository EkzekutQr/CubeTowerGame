using Reflex.Core;
using Reflex.Enums;
using UnityEngine;

public class RootInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private TweenSettingsLibrary tweenSettingsLib;
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.RegisterValue(tweenSettingsLib)
                .RegisterType(typeof(SaveLoadService), typeof(ISaveLoadService).ToArray(), Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
    }
}
