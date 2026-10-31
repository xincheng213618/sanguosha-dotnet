#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime))]
internal sealed partial class CoreGameEngineProgramOwnCardDiscardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime> _resolve;
    private CoreGameEngineProgramOwnCardDiscardsEntries(Func<EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOwnCardDiscardsEntries Create(Func<EngineOwners.CoreGameEngineProgramOwnCardDiscardsRuntime> resolve) => new(resolve);
}
