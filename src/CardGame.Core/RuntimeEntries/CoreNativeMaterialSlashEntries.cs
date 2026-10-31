#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeMaterialSlashEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMaterialSlashRuntime> _resolve;
    private CoreNativeMaterialSlashEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMaterialSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeMaterialSlashEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMaterialSlashRuntime> resolve) => new(resolve);
}
