#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePublicPersistentPilesRuntime))]
internal sealed partial class CoreGameEnginePublicPersistentPilesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicPersistentPilesRuntime> _resolve;
    private CoreGameEnginePublicPersistentPilesEntries(Func<EngineOwners.CoreGameEnginePublicPersistentPilesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPersistentPilesEntries Create(Func<EngineOwners.CoreGameEnginePublicPersistentPilesRuntime> resolve) => new(resolve);
}
