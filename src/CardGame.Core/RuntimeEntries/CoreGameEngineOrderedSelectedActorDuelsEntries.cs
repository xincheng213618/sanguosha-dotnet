#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime))]
internal sealed partial class CoreGameEngineOrderedSelectedActorDuelsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime> _resolve;
    private CoreGameEngineOrderedSelectedActorDuelsEntries(Func<EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOrderedSelectedActorDuelsEntries Create(Func<EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime> resolve) => new(resolve);
}
