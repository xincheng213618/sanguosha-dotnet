#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessObserversRuntime))]
internal sealed partial class CoreGameEngineExtraEffectsAndRankGuessObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessObserversRuntime> _resolve;
    private CoreGameEngineExtraEffectsAndRankGuessObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraEffectsAndRankGuessObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessObserversRuntime> resolve) => new(resolve);
}
