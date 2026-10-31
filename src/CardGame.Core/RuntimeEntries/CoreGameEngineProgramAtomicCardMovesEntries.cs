#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime))]
internal sealed partial class CoreGameEngineProgramAtomicCardMovesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> _resolve;
    private CoreGameEngineProgramAtomicCardMovesEntries(Func<EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAtomicCardMovesEntries Create(Func<EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> resolve) => new(resolve);
}
