#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineExtraEffectsAndRankGuessObserversRuntime))]
internal sealed partial class CoreGameEngineExtraEffectsAndRankGuessObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineExtraEffectsAndRankGuessObserversRuntime> _resolve;
    private CoreGameEngineExtraEffectsAndRankGuessObserversEntries(Func<EngineOwners.CoreGameEngineExtraEffectsAndRankGuessObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraEffectsAndRankGuessObserversEntries Create(Func<EngineOwners.CoreGameEngineExtraEffectsAndRankGuessObserversRuntime> resolve) => new(resolve);
}
