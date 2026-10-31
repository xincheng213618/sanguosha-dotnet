#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime))]
internal sealed partial class CoreGameEngineOriginalHandEntitiesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> _resolve;
    private CoreGameEngineOriginalHandEntitiesEntries(Func<EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalHandEntitiesEntries Create(Func<EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> resolve) => new(resolve);
}
