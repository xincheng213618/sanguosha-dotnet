#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraEffectsAndRankGuessObserversEntries
{
    internal bool ExtraEffectsAndRankGuessFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().ExtraEffectsAndRankGuessFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? ExtraEffectsAndRankGuessObserverRoot() => _resolve().ExtraEffectsAndRankGuessObserverRoot();
    internal bool TryAdvanceExtraEffectsAndRankGuessSubtree() => _resolve().TryAdvanceExtraEffectsAndRankGuessSubtree();
}
