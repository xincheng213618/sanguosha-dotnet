#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageTargetBudgetRuntime))]
internal sealed partial class CoreGameEngineDamageTargetBudgetEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> _resolve;
    private CoreGameEngineDamageTargetBudgetEntries(Func<EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetBudgetEntries Create(Func<EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> resolve) => new(resolve);
}
