#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime))]
internal sealed partial class CoreGameEngineDamageJudgmentSuitPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> _resolve;
    private CoreGameEngineDamageJudgmentSuitPaymentsEntries(Func<EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageJudgmentSuitPaymentsEntries Create(Func<EngineOwners.CoreGameEngineDamageJudgmentSuitPaymentsRuntime> resolve) => new(resolve);
}
