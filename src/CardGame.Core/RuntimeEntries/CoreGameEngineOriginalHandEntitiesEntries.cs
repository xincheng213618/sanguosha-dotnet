#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime))]
internal sealed partial class CoreGameEngineOriginalHandEntitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> _resolve;
    private CoreGameEngineOriginalHandEntitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalHandEntitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> resolve) => new(resolve);
}
