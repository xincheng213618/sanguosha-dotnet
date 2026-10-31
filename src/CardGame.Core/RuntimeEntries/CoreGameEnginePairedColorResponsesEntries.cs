#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePairedColorResponsesRuntime))]
internal sealed partial class CoreGameEnginePairedColorResponsesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePairedColorResponsesRuntime> _resolve;
    private CoreGameEnginePairedColorResponsesEntries(Func<EngineOwners.CoreGameEnginePairedColorResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairedColorResponsesEntries Create(Func<EngineOwners.CoreGameEnginePairedColorResponsesRuntime> resolve) => new(resolve);
}
