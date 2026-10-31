#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime))]
internal sealed partial class CoreGameEngineProvenanceVirtualAlcoholEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime> _resolve;
    private CoreGameEngineProvenanceVirtualAlcoholEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProvenanceVirtualAlcoholEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime> resolve) => new(resolve);
}
