#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoYanOlEntries
{
    internal ProgramTongxieArmedEvent? ActiveTongxieArm(int ownerSeat) => _resolve().ActiveTongxieArm(ownerSeat);
    internal bool CanRunTongxieArm(int ownerSeat) => _resolve().CanRunTongxieArm(ownerSeat);
    internal bool CanRunTongxieFollowUp(int ownerSeat, ProgramSkillWindowContext context) => _resolve().CanRunTongxieFollowUp(ownerSeat, context);
    internal bool CanRunTongxieGuard(int ownerSeat, ProgramSkillWindowContext context) => _resolve().CanRunTongxieGuard(ownerSeat, context);
    internal void CaptureTongxieSettledSlashUse(long batchId, CardMovementRecord movement) => _resolve().CaptureTongxieSettledSlashUse(batchId, movement);
    internal SkillProgramStepOutcome ContinueTongxieFollowUp(ProgramSkillFrame active, TongxieFollowUpState pending) => _resolve().ContinueTongxieFollowUp(active, pending);
    internal SkillProgramStepOutcome ContinueTongxieGuard(ProgramSkillFrame active, TongxieGuardState pending) => _resolve().ContinueTongxieGuard(active, pending);
    internal string DescribeTongxieActors(string verb, IReadOnlyList<int> seats) => _resolve().DescribeTongxieActors(verb, seats);
    internal string DescribeTongxieMembers(IReadOnlyList<int> members) => _resolve().DescribeTongxieMembers(members);
    internal void DrawForUniqueLeastTongxieHand(ProgramSkillFrame active, IReadOnlyList<int> members) => _resolve().DrawForUniqueLeastTongxieHand(active, members);
    internal SkillProgramStepOutcome FinalizeTongxieGuard(ProgramSkillFrame active, TongxieGuardState pending) => _resolve().FinalizeTongxieGuard(active, pending);
    internal TongxieChainUse? FindTongxieChainUse(CardMovementBatchContext batch, int movementIndex) => _resolve().FindTongxieChainUse(batch, movementIndex);
    internal bool IsTongxieProducedUse(long actionId) => _resolve().IsTongxieProducedUse(actionId);
    internal int[] MatchingTongxieSettledSlashIndexes(CardMovementBatchContext batch) => _resolve().MatchingTongxieSettledSlashIndexes(batch);
    internal SkillProgramStepOutcome PresentTongxieFollowUpPrompt(ProgramSkillFrame frame) => _resolve().PresentTongxieFollowUpPrompt(frame);
    internal SkillProgramStepOutcome PresentTongxieGuardPrompt(ProgramSkillFrame frame) => _resolve().PresentTongxieGuardPrompt(frame);
    internal void ResolveTongxieArmChoice(PromptChoice selected) => _resolve().ResolveTongxieArmChoice(selected);
    internal void ResolveTongxieFollowUpChoice(PromptChoice selected) => _resolve().ResolveTongxieFollowUpChoice(selected);
    internal void ResolveTongxieGuardChoice(PromptChoice selected) => _resolve().ResolveTongxieGuardChoice(selected);
    internal PromptChoice SelectAiTongxieArmChoice(PendingDecision decision) => _resolve().SelectAiTongxieArmChoice(decision);
    internal PromptChoice SelectAiTongxieFollowUpChoice(PendingDecision decision) => _resolve().SelectAiTongxieFollowUpChoice(decision);
    internal PromptChoice SelectAiTongxieGuardChoice(PendingDecision decision) => _resolve().SelectAiTongxieGuardChoice(decision);
    internal int[]? SelectWindowIndexes_MatchingTongxieSettledSlashIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingTongxieSettledSlashIndexes(batch, candidate, trigger);
    internal double TongxieArmDrawBonus(GameSnapshot view, int ownerSeat, int[] chosen) => _resolve().TongxieArmDrawBonus(view, ownerSeat, chosen);
    internal IEnumerable<int> TongxieEligiblePreventers(IReadOnlyList<int> members, int protectedSeat) => _resolve().TongxieEligiblePreventers(members, protectedSeat);
    internal IEnumerable<int> TongxieFollowUpResponders(IReadOnlyList<int> members, int actorSeat, int targetSeat) => _resolve().TongxieFollowUpResponders(members, actorSeat, targetSeat);
    internal bool TongxieLostHpThisTurn(int seat) => _resolve().TongxieLostHpThisTurn(seat);
    internal SkillProgramStepOutcome TongxieProgramArm(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().TongxieProgramArm(frame, effect);
    internal SkillProgramStepOutcome TongxieProgramFollowUp(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().TongxieProgramFollowUp(frame, effect);
    internal SkillProgramStepOutcome TongxieProgramGuard(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().TongxieProgramGuard(frame, effect);
}
