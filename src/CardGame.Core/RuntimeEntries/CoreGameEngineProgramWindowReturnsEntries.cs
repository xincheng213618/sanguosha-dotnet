#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramWindowReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramWindowReturnsRuntime> _resolve;
    private CoreGameEngineProgramWindowReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramWindowReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramWindowReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramWindowReturnsRuntime> resolve) => new(resolve);
}
