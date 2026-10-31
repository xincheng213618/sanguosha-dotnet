#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime))]
internal sealed partial class CoreGameEngineProvenanceVirtualAlcoholEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime> _resolve;
    private CoreGameEngineProvenanceVirtualAlcoholEntries(Func<EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProvenanceVirtualAlcoholEntries Create(Func<EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime> resolve) => new(resolve);
}
