#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBaguaResponsesRuntime))]
internal sealed partial class CoreGameEngineBaguaResponsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBaguaResponsesRuntime> _resolve;
    private CoreGameEngineBaguaResponsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBaguaResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBaguaResponsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBaguaResponsesRuntime> resolve) => new(resolve);
}
