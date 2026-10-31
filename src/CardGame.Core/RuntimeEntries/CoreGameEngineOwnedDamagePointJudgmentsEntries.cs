#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime))]
internal sealed partial class CoreGameEngineOwnedDamagePointJudgmentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> _resolve;
    private CoreGameEngineOwnedDamagePointJudgmentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedDamagePointJudgmentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> resolve) => new(resolve);
}
