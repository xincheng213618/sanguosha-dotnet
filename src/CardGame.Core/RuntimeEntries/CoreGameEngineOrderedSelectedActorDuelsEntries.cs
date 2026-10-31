#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime))]
internal sealed partial class CoreGameEngineOrderedSelectedActorDuelsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime> _resolve;
    private CoreGameEngineOrderedSelectedActorDuelsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOrderedSelectedActorDuelsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedSelectedActorDuelsRuntime> resolve) => new(resolve);
}
