#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedDamagePointJudgmentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> _resolve;
    private CoreGameEngineOwnedDamagePointJudgmentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedDamagePointJudgmentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDamagePointJudgmentsRuntime> resolve) => new(resolve);
}
