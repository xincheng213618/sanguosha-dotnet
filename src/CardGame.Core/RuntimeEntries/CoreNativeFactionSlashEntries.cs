#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeFactionSlashRuntime))]
internal sealed partial class CoreNativeFactionSlashEntries
{
    private readonly Func<EngineOwners.CoreNativeFactionSlashRuntime> _resolve;
    private CoreNativeFactionSlashEntries(Func<EngineOwners.CoreNativeFactionSlashRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeFactionSlashEntries Create(Func<EngineOwners.CoreNativeFactionSlashRuntime> resolve) => new(resolve);
}
