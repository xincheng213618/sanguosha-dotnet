#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime))]
internal sealed partial class CoreGameEngineOwnedDamagePointJudgmentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> _resolve;
    private CoreGameEngineOwnedDamagePointJudgmentsEntries(Func<EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedDamagePointJudgmentsEntries Create(Func<EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> resolve) => new(resolve);
}
