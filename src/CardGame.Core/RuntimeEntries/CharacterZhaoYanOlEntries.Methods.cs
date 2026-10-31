#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoYanOlEntries
{
    internal global::CardGame.Core.ProgramTongxieArmedEvent? ActiveTongxieArm(int ownerSeat) => _resolve().ActiveTongxieArm(ownerSeat);
    internal bool CanRunTongxieArm(int ownerSeat) => _resolve().CanRunTongxieArm(ownerSeat);
    internal bool CanRunTongxieFollowUp(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunTongxieFollowUp(ownerSeat, context);
    internal bool CanRunTongxieGuard(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunTongxieGuard(ownerSeat, context);
    internal void CaptureTongxieSettledSlashUse(long batchId, global::CardGame.Core.CardMovementRecord movement) => _resolve().CaptureTongxieSettledSlashUse(batchId, movement);
    internal global::CardGame.Core.SkillProgramStepOutcome ContinueTongxieFollowUp(global::CardGame.Core.ProgramSkillFrame active, global::CardGame.Core.TongxieFollowUpState pending) => _resolve().ContinueTongxieFollowUp(active, pending);
    internal global::CardGame.Core.SkillProgramStepOutcome ContinueTongxieGuard(global::CardGame.Core.ProgramSkillFrame active, global::CardGame.Core.TongxieGuardState pending) => _resolve().ContinueTongxieGuard(active, pending);
    internal string DescribeTongxieActors(string verb, global::System.Collections.Generic.IReadOnlyList<int> seats) => _resolve().DescribeTongxieActors(verb, seats);
    internal string DescribeTongxieMembers(global::System.Collections.Generic.IReadOnlyList<int> members) => _resolve().DescribeTongxieMembers(members);
    internal void DrawForUniqueLeastTongxieHand(global::CardGame.Core.ProgramSkillFrame active, global::System.Collections.Generic.IReadOnlyList<int> members) => _resolve().DrawForUniqueLeastTongxieHand(active, members);
    internal global::CardGame.Core.SkillProgramStepOutcome FinalizeTongxieGuard(global::CardGame.Core.ProgramSkillFrame active, global::CardGame.Core.TongxieGuardState pending) => _resolve().FinalizeTongxieGuard(active, pending);
    internal global::CardGame.Core.TongxieChainUse? FindTongxieChainUse(global::CardGame.Core.CardMovementBatchContext batch, int movementIndex) => _resolve().FindTongxieChainUse(batch, movementIndex);
    internal bool IsTongxieProducedUse(long actionId) => _resolve().IsTongxieProducedUse(actionId);
    internal int[] MatchingTongxieSettledSlashIndexes(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().MatchingTongxieSettledSlashIndexes(batch);
    internal global::CardGame.Core.SkillProgramStepOutcome PresentTongxieFollowUpPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentTongxieFollowUpPrompt(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome PresentTongxieGuardPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentTongxieGuardPrompt(frame);
    internal void ResolveTongxieArmChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveTongxieArmChoice(selected);
    internal void ResolveTongxieFollowUpChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveTongxieFollowUpChoice(selected);
    internal void ResolveTongxieGuardChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveTongxieGuardChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiTongxieArmChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiTongxieArmChoice(decision);
    internal global::CardGame.Core.PromptChoice SelectAiTongxieFollowUpChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiTongxieFollowUpChoice(decision);
    internal global::CardGame.Core.PromptChoice SelectAiTongxieGuardChoice(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiTongxieGuardChoice(decision);
    internal int[]? SelectWindowIndexes_MatchingTongxieSettledSlashIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingTongxieSettledSlashIndexes(batch, candidate, trigger);
    internal double TongxieArmDrawBonus(global::CardGame.Core.GameSnapshot view, int ownerSeat, int[] chosen) => _resolve().TongxieArmDrawBonus(view, ownerSeat, chosen);
    internal global::System.Collections.Generic.IEnumerable<int> TongxieEligiblePreventers(global::System.Collections.Generic.IReadOnlyList<int> members, int protectedSeat) => _resolve().TongxieEligiblePreventers(members, protectedSeat);
    internal global::System.Collections.Generic.IEnumerable<int> TongxieFollowUpResponders(global::System.Collections.Generic.IReadOnlyList<int> members, int actorSeat, int targetSeat) => _resolve().TongxieFollowUpResponders(members, actorSeat, targetSeat);
    internal bool TongxieLostHpThisTurn(int seat) => _resolve().TongxieLostHpThisTurn(seat);
    internal global::CardGame.Core.SkillProgramStepOutcome TongxieProgramArm(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().TongxieProgramArm(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome TongxieProgramFollowUp(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().TongxieProgramFollowUp(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome TongxieProgramGuard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().TongxieProgramGuard(frame, effect);
}
