#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanJunOlEntries
{
    internal void CaptureMovementBatchFactsCaptureSettledActionCards(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureSettledActionCards(batch, movements);
    internal void CaptureSettledActionCards(long batchId, int turnNumber, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureSettledActionCards(batchId, turnNumber, movements);
    internal (bool Ascending, int PhaseEventIndex)? CurrentGuanchaoPattern(int ownerSeat, string skillId) => _resolve().CurrentGuanchaoPattern(ownerSeat, skillId);
    internal int CurrentPlayPhaseEventIndex(int ownerSeat) => _resolve().CurrentPlayPhaseEventIndex(ownerSeat);
    internal global::System.Collections.Generic.IReadOnlyList<int?> GuanchaoPhaseUseRanks(int ownerSeat, int phaseEventIndex, long currentActionId, bool includeResponseUses = false) => _resolve().GuanchaoPhaseUseRanks(ownerSeat, phaseEventIndex, currentActionId, includeResponseUses);
    internal global::CardGame.Core.SkillProgramStepOutcome GuanchaoProgramChoosePattern(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().GuanchaoProgramChoosePattern(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome GuanchaoProgramRankDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().GuanchaoProgramRankDraw(frame, effect);
    internal int[] MatchingOwnSettledActionCardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MatchingOwnSettledActionCardIndexes(batch, candidate);
    internal (bool Ascending, int PhaseEventIndex)? OwnTurnGuanchaoPattern(int ownerSeat, string skillId) => _resolve().OwnTurnGuanchaoPattern(ownerSeat, skillId);
    internal void ResolveGuanchaoPatternChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveGuanchaoPatternChoice(selected);
    internal void ResolveXunxianGiftChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveXunxianGiftChoice(selected);
    internal int[]? SelectWindowIndexes_MatchingOwnSettledActionCardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingOwnSettledActionCardIndexes(batch, candidate, trigger);
    internal global::CardGame.Core.CardActionContext? SettledActionContext(int cardId) => _resolve().SettledActionContext(cardId);
    internal bool UsesOwnTurnGuanchaoLedger(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().UsesOwnTurnGuanchaoLedger(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome XunxianProgramGiftUsedCard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().XunxianProgramGiftUsedCard(frame, effect);
    internal bool TracksSettledActionCards { get => _resolve().TracksSettledActionCards; }
}
