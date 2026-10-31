#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPanShuOlEntries
{
    internal void ApplyProgramSkillRecovery(long frameId, int sourceSeat, int targetSeat) => _resolve().ApplyProgramSkillRecovery(frameId, sourceSeat, targetSeat);
    internal IReadOnlySet<SkillProgramCardCategory> CardIdsCategorySet(IReadOnlyList<int> cardIds) => _resolve().CardIdsCategorySet(cardIds);
    internal IEnumerable<ProgramTriggerCandidate> CollectOtherLastHandLossCandidates(CardMovementBatchContext batch) => _resolve().CollectOtherLastHandLossCandidates(batch);
    internal void CollectWindow_CollectOtherLastHandLossCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectOtherLastHandLossCandidates(batch, result);
    internal void ContinueZhirenFieldAfterEquipment(ProgramSkillFrame frame) => _resolve().ContinueZhirenFieldAfterEquipment(frame);
    internal SkillProgramStepOutcome ContinueZhirenTiers(ProgramSkillFrame active, int length, bool skippedDiscards) => _resolve().ContinueZhirenTiers(active, length, skippedDiscards);
    internal int CountNonConvertedActualUsesThisTurn(int ownerSeat) => _resolve().CountNonConvertedActualUsesThisTurn(ownerSeat);
    internal bool ExcludeOrdinaryMovement_IsOtherLastHandLossTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsOtherLastHandLossTrigger(candidate, trigger);
    internal void PresentZhirenFieldPrompt(ProgramSkillFrame frame, bool delayedTrick) => _resolve().PresentZhirenFieldPrompt(frame, delayedTrick);
    internal bool? ProveOtherLastHandMovement(SkillProgramTrigger trigger, ProgramSkillWindowContext context, CardMovementBatchContext batch, CharacterState owner) => _resolve().ProveOtherLastHandMovement(trigger, context, batch, owner);
    internal void ResolveZhirenFieldChoice(PromptChoice selected) => _resolve().ResolveZhirenFieldChoice(selected);
    internal void SetForeignProgramBooleanState(ProgramSkillFrame frame, string targetSkillId, string stateId, bool value) => _resolve().SetForeignProgramBooleanState(frame, targetSkillId, stateId, value);
    internal SkillProgramStepOutcome YanerProgramResolvePairBenefit(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().YanerProgramResolvePairBenefit(frame, effect);
    internal IReadOnlyList<Card> ZhirenFieldDelayedCandidates() => _resolve().ZhirenFieldDelayedCandidates();
    internal IReadOnlyList<Card> ZhirenFieldEquipmentCandidates() => _resolve().ZhirenFieldEquipmentCandidates();
    internal int ZhirenNameLength(ProgramSkillFrame frame) => _resolve().ZhirenNameLength(frame);
    internal SkillProgramStepOutcome ZhirenProgramReorderTopByLength(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ZhirenProgramReorderTopByLength(frame, effect);
    internal SkillProgramStepOutcome ZhirenProgramResolveFieldTiers(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ZhirenProgramResolveFieldTiers(frame, effect);
}
