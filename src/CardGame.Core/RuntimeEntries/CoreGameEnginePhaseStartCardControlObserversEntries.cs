#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartCardControlObserversRuntime))]
internal sealed partial class CoreGameEnginePhaseStartCardControlObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartCardControlObserversRuntime> _resolve;
    private CoreGameEnginePhaseStartCardControlObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartCardControlObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseStartCardControlObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartCardControlObserversRuntime> resolve) => new(resolve);
}
