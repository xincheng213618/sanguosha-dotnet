#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPanShuOlEntries
{
    internal void ApplyProgramSkillRecovery(long frameId, int sourceSeat, int targetSeat) => _resolve().ApplyProgramSkillRecovery(frameId, sourceSeat, targetSeat);
    internal global::System.Collections.Generic.IReadOnlySet<global::CardGame.Core.SkillProgramCardCategory> CardIdsCategorySet(global::System.Collections.Generic.IReadOnlyList<int> cardIds) => _resolve().CardIdsCategorySet(cardIds);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectOtherLastHandLossCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectOtherLastHandLossCandidates(batch);
    internal void CollectWindow_CollectOtherLastHandLossCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectOtherLastHandLossCandidates(batch, result);
    internal void ContinueZhirenFieldAfterEquipment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueZhirenFieldAfterEquipment(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ContinueZhirenTiers(global::CardGame.Core.ProgramSkillFrame active, int length, bool skippedDiscards) => _resolve().ContinueZhirenTiers(active, length, skippedDiscards);
    internal int CountNonConvertedActualUsesThisTurn(int ownerSeat) => _resolve().CountNonConvertedActualUsesThisTurn(ownerSeat);
    internal bool ExcludeOrdinaryMovement_IsOtherLastHandLossTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsOtherLastHandLossTrigger(candidate, trigger);
    internal void PresentZhirenFieldPrompt(global::CardGame.Core.ProgramSkillFrame frame, bool delayedTrick) => _resolve().PresentZhirenFieldPrompt(frame, delayedTrick);
    internal bool? ProveOtherLastHandMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.CharacterState owner) => _resolve().ProveOtherLastHandMovement(trigger, context, batch, owner);
    internal void ResolveZhirenFieldChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveZhirenFieldChoice(selected);
    internal void SetForeignProgramBooleanState(global::CardGame.Core.ProgramSkillFrame frame, string targetSkillId, string stateId, bool value) => _resolve().SetForeignProgramBooleanState(frame, targetSkillId, stateId, value);
    internal global::CardGame.Core.SkillProgramStepOutcome YanerProgramResolvePairBenefit(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().YanerProgramResolvePairBenefit(frame, effect);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> ZhirenFieldDelayedCandidates() => _resolve().ZhirenFieldDelayedCandidates();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> ZhirenFieldEquipmentCandidates() => _resolve().ZhirenFieldEquipmentCandidates();
    internal int ZhirenNameLength(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ZhirenNameLength(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ZhirenProgramReorderTopByLength(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ZhirenProgramReorderTopByLength(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome ZhirenProgramResolveFieldTiers(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ZhirenProgramResolveFieldTiers(frame, effect);
}
