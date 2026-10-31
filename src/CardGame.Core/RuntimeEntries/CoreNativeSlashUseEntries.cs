#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed partial class CoreNativeSlashUseEntries
{
    private readonly Func<EngineOwners.CoreNativeSlashUseRuntime> _resolve;
    private CoreNativeSlashUseEntries(Func<EngineOwners.CoreNativeSlashUseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeSlashUseEntries Create(Func<EngineOwners.CoreNativeSlashUseRuntime> resolve) => new(resolve);
}
