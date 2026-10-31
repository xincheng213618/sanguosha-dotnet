#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangXunAnniversaryEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ZhangXunDueLease> ActiveZhangXunLeases() => _resolve().ActiveZhangXunLeases();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> AnniversaryZhangXunSlashContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().AnniversaryZhangXunSlashContributions(player, query);
    internal void AnswerZhangXunFollowup(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerZhangXunFollowup(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome ArmAnniversaryZhangXun(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ArmAnniversaryZhangXun(supplied);
    internal bool CanOfferZhangXunFollowup(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferZhangXunFollowup(c, trigger, context);
    internal global::CardGame.Core.SkillProgramStepOutcome ChooseAnniversaryZhangXunSlash(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ChooseAnniversaryZhangXunSlash(supplied);
    internal void CloseZhangXunPhase(global::CardGame.Core.ProgramBindingResolvedEvent fact) => _resolve().CloseZhangXunPhase(fact);
    internal global::CardGame.Core.GameEngine.ZhangXunDueLease? ExactZhangXunCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().ExactZhangXunCandidate(c, w);
    internal bool HasZhangXunSlashBenefit(int seat) => _resolve().HasZhangXunSlashBenefit(seat);
    internal bool IsIssuedAnniversaryZhangXunCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedAnniversaryZhangXunCandidate(c, w);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedAnniversaryZhangXunPlayEndingCandidates(global::CardGame.Core.CharacterState phaseActor) => _resolve().IssuedAnniversaryZhangXunPlayEndingCandidates(phaseActor);
    internal bool KeepIssuedZhangXunFollowup(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepIssuedZhangXunFollowup(f);
    internal bool KeepZhangXunAlreadyUsedSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepZhangXunAlreadyUsedSlash(f);
    internal bool LegalZhangXunFollowupTarget(int owner, int target) => _resolve().LegalZhangXunFollowupTarget(owner, target);
    internal void ObserveAnniversaryZhangXunLease(global::CardGame.Core.ZhangXunSuizhengLeaseEvent fact) => _resolve().ObserveAnniversaryZhangXunLease(fact);
    internal void PublishZhangXunFollowup(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhangXunFollowup(f);
    internal bool ValidZhangXunFollowup(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidZhangXunFollowup(f);
    internal bool ValidZhangXunLease(global::CardGame.Core.ZhangXunSuizhengLeaseEvent lease, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history) => _resolve().ValidZhangXunLease(lease, history);
    internal bool ZhangXunCardDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().ZhangXunCardDistance(q);
    internal global::CardGame.Core.PromptChoice ZhangXunFollowupAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangXunFollowupAi(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhangXunFollowupChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangXunFollowupChoices(f);
    internal global::CardGame.Core.CardConversionSource ZhangXunFrameSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangXunFrameSource(f);
    internal bool ZhangXunPhaseUsed(long leaseId, int turn, int phase) => _resolve().ZhangXunPhaseUsed(leaseId, turn, phase);
    internal bool ZhangXunSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().ZhangXunSlashDistance(q);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ZhangXunSuizhengDamageWitness> ZhangXunVictims(global::CardGame.Core.GameEngine.ZhangXunDueLease due, int cutoff) => _resolve().ZhangXunVictims(due, cutoff);
    internal bool HasAnniversaryZhangXunCapability { get => _resolve().HasAnniversaryZhangXunCapability; }
}
