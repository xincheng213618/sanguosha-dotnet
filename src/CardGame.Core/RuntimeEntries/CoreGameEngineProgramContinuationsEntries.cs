#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramContinuationsRuntime))]
internal sealed partial class CoreGameEngineProgramContinuationsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramContinuationsRuntime> _resolve;
    private CoreGameEngineProgramContinuationsEntries(Func<EngineOwners.CoreGameEngineProgramContinuationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramContinuationsEntries Create(Func<EngineOwners.CoreGameEngineProgramContinuationsRuntime> resolve) => new(resolve);
}
