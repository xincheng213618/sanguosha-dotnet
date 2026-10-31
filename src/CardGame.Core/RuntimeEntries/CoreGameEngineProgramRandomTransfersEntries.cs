#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramRandomTransfersRuntime))]
internal sealed partial class CoreGameEngineProgramRandomTransfersEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramRandomTransfersRuntime> _resolve;
    private CoreGameEngineProgramRandomTransfersEntries(Func<EngineOwners.CoreGameEngineProgramRandomTransfersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramRandomTransfersEntries Create(Func<EngineOwners.CoreGameEngineProgramRandomTransfersRuntime> resolve) => new(resolve);
}
