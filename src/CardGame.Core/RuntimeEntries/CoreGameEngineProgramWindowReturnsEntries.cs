#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramWindowReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramWindowReturnsRuntime> _resolve;
    private CoreGameEngineProgramWindowReturnsEntries(Func<EngineOwners.CoreGameEngineProgramWindowReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramWindowReturnsEntries Create(Func<EngineOwners.CoreGameEngineProgramWindowReturnsRuntime> resolve) => new(resolve);
}
