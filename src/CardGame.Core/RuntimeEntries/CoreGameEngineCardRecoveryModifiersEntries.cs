#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardRecoveryModifiersRuntime))]
internal sealed partial class CoreGameEngineCardRecoveryModifiersEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardRecoveryModifiersRuntime> _resolve;
    private CoreGameEngineCardRecoveryModifiersEntries(Func<EngineOwners.CoreGameEngineCardRecoveryModifiersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardRecoveryModifiersEntries Create(Func<EngineOwners.CoreGameEngineCardRecoveryModifiersRuntime> resolve) => new(resolve);
}
