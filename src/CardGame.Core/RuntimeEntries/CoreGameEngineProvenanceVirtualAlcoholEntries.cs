#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProvenanceVirtualAlcoholEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime> _resolve;
    private CoreGameEngineProvenanceVirtualAlcoholEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProvenanceVirtualAlcoholEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceVirtualAlcoholRuntime> resolve) => new(resolve);
}
