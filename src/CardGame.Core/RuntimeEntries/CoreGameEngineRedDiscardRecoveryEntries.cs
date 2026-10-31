#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRedDiscardRecoveryRuntime))]
internal sealed partial class CoreGameEngineRedDiscardRecoveryEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRedDiscardRecoveryRuntime> _resolve;
    private CoreGameEngineRedDiscardRecoveryEntries(Func<EngineOwners.CoreGameEngineRedDiscardRecoveryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRedDiscardRecoveryEntries Create(Func<EngineOwners.CoreGameEngineRedDiscardRecoveryRuntime> resolve) => new(resolve);
}
