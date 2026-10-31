#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDiscardTopPlacementEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardTopPlacementRuntime> _resolve;
    private CoreGameEngineProgramDiscardTopPlacementEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardTopPlacementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDiscardTopPlacementEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardTopPlacementRuntime> resolve) => new(resolve);
}
