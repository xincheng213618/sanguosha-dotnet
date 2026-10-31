#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrepPoisonOutcomesRuntime))]
internal sealed partial class CoreGameEnginePrepPoisonOutcomesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrepPoisonOutcomesRuntime> _resolve;
    private CoreGameEnginePrepPoisonOutcomesEntries(Func<EngineOwners.CoreGameEnginePrepPoisonOutcomesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepPoisonOutcomesEntries Create(Func<EngineOwners.CoreGameEnginePrepPoisonOutcomesRuntime> resolve) => new(resolve);
}
