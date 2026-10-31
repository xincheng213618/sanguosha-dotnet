#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeMaterialSlashRuntime))]
internal sealed partial class CoreNativeMaterialSlashEntries
{
    private readonly Func<EngineOwners.CoreNativeMaterialSlashRuntime> _resolve;
    private CoreNativeMaterialSlashEntries(Func<EngineOwners.CoreNativeMaterialSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeMaterialSlashEntries Create(Func<EngineOwners.CoreNativeMaterialSlashRuntime> resolve) => new(resolve);
}
