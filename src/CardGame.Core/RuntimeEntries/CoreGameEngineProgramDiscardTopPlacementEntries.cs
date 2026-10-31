#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramDiscardTopPlacementRuntime))]
internal sealed partial class CoreGameEngineProgramDiscardTopPlacementEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramDiscardTopPlacementRuntime> _resolve;
    private CoreGameEngineProgramDiscardTopPlacementEntries(Func<EngineOwners.CoreGameEngineProgramDiscardTopPlacementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDiscardTopPlacementEntries Create(Func<EngineOwners.CoreGameEngineProgramDiscardTopPlacementRuntime> resolve) => new(resolve);
}
