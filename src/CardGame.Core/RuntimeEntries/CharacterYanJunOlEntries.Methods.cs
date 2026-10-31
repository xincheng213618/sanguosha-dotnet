#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanJunOlEntries
{
    internal void CaptureMovementBatchFactsCaptureSettledActionCards(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureSettledActionCards(batch, movements);
    internal void CaptureSettledActionCards(long batchId, int turnNumber, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureSettledActionCards(batchId, turnNumber, movements);
    internal (bool Ascending, int PhaseEventIndex)? CurrentGuanchaoPattern(int ownerSeat, string skillId) => _resolve().CurrentGuanchaoPattern(ownerSeat, skillId);
    internal int CurrentPlayPhaseEventIndex(int ownerSeat) => _resolve().CurrentPlayPhaseEventIndex(ownerSeat);
    internal IReadOnlyList<int?> GuanchaoPhaseUseRanks(int ownerSeat, int phaseEventIndex, long currentActionId, bool includeResponseUses = false) => _resolve().GuanchaoPhaseUseRanks(ownerSeat, phaseEventIndex, currentActionId, includeResponseUses);
    internal SkillProgramStepOutcome GuanchaoProgramChoosePattern(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().GuanchaoProgramChoosePattern(frame, effect);
    internal SkillProgramStepOutcome GuanchaoProgramRankDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().GuanchaoProgramRankDraw(frame, effect);
    internal int[] MatchingOwnSettledActionCardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingOwnSettledActionCardIndexes(batch, candidate);
    internal (bool Ascending, int PhaseEventIndex)? OwnTurnGuanchaoPattern(int ownerSeat, string skillId) => _resolve().OwnTurnGuanchaoPattern(ownerSeat, skillId);
    internal void ResolveGuanchaoPatternChoice(PromptChoice selected) => _resolve().ResolveGuanchaoPatternChoice(selected);
    internal void ResolveXunxianGiftChoice(PromptChoice selected) => _resolve().ResolveXunxianGiftChoice(selected);
    internal int[]? SelectWindowIndexes_MatchingOwnSettledActionCardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingOwnSettledActionCardIndexes(batch, candidate, trigger);
    internal CardActionContext? SettledActionContext(int cardId) => _resolve().SettledActionContext(cardId);
    internal bool UsesOwnTurnGuanchaoLedger(ProgramSkillFrame frame) => _resolve().UsesOwnTurnGuanchaoLedger(frame);
    internal SkillProgramStepOutcome XunxianProgramGiftUsedCard(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().XunxianProgramGiftUsedCard(frame, effect);
    internal bool TracksSettledActionCards { get => _resolve().TracksSettledActionCards; }
}
