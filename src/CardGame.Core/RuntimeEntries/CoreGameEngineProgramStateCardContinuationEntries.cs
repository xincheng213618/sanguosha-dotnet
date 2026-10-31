#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateCardContinuationRuntime))]
internal sealed partial class CoreGameEngineProgramStateCardContinuationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateCardContinuationRuntime> _resolve;
    private CoreGameEngineProgramStateCardContinuationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateCardContinuationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramStateCardContinuationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramStateCardContinuationRuntime> resolve) => new(resolve);
}
