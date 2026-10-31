#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepPoisonOutcomesRuntime))]
internal sealed partial class CoreGameEnginePrepPoisonOutcomesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepPoisonOutcomesRuntime> _resolve;
    private CoreGameEnginePrepPoisonOutcomesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepPoisonOutcomesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepPoisonOutcomesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepPoisonOutcomesRuntime> resolve) => new(resolve);
}
