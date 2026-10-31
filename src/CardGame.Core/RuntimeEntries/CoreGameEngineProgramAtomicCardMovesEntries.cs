#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAtomicCardMovesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> _resolve;
    private CoreGameEngineProgramAtomicCardMovesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAtomicCardMovesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> resolve) => new(resolve);
}
