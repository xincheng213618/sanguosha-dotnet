#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraEffectsAndRankGuessObserversEntries
{
    internal bool ExtraEffectsAndRankGuessFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().ExtraEffectsAndRankGuessFirstChild(root, child);
    internal ProgramSkillFrame? ExtraEffectsAndRankGuessObserverRoot() => _resolve().ExtraEffectsAndRankGuessObserverRoot();
    internal bool TryAdvanceExtraEffectsAndRankGuessSubtree() => _resolve().TryAdvanceExtraEffectsAndRankGuessSubtree();
}
