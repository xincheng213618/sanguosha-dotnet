#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePairedHandRecastRuntime))]
internal sealed partial class CoreGameEnginePairedHandRecastEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePairedHandRecastRuntime> _resolve;
    private CoreGameEnginePairedHandRecastEntries(Func<EngineOwners.CoreGameEnginePairedHandRecastRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairedHandRecastEntries Create(Func<EngineOwners.CoreGameEnginePairedHandRecastRuntime> resolve) => new(resolve);
}
