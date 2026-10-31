#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime))]
internal sealed partial class CoreGameEngineDamageTargetBudgetEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> _resolve;
    private CoreGameEngineDamageTargetBudgetEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetBudgetEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetBudgetRuntime> resolve) => new(resolve);
}
