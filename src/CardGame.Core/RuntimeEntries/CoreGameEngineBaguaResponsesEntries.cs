#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineBaguaResponsesRuntime))]
internal sealed partial class CoreGameEngineBaguaResponsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineBaguaResponsesRuntime> _resolve;
    private CoreGameEngineBaguaResponsesEntries(Func<EngineOwners.CoreGameEngineBaguaResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBaguaResponsesEntries Create(Func<EngineOwners.CoreGameEngineBaguaResponsesRuntime> resolve) => new(resolve);
}
