#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangXunAnniversaryEntries
{
    internal IReadOnlyList<GameEngine.ZhangXunDueLease> ActiveZhangXunLeases() => _resolve().ActiveZhangXunLeases();
    internal IEnumerable<RuleQueryContribution> AnniversaryZhangXunSlashContributions(CharacterState player, SkillRuleQuery query) => _resolve().AnniversaryZhangXunSlashContributions(player, query);
    internal void AnswerZhangXunFollowup(PromptChoice choice) => _resolve().AnswerZhangXunFollowup(choice);
    internal SkillProgramStepOutcome ArmAnniversaryZhangXun(ProgramSkillFrame supplied) => _resolve().ArmAnniversaryZhangXun(supplied);
    internal bool CanOfferZhangXunFollowup(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferZhangXunFollowup(c, trigger, context);
    internal SkillProgramStepOutcome ChooseAnniversaryZhangXunSlash(ProgramSkillFrame supplied) => _resolve().ChooseAnniversaryZhangXunSlash(supplied);
    internal void CloseZhangXunPhase(ProgramBindingResolvedEvent fact) => _resolve().CloseZhangXunPhase(fact);
    internal GameEngine.ZhangXunDueLease? ExactZhangXunCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().ExactZhangXunCandidate(c, w);
    internal bool HasZhangXunSlashBenefit(int seat) => _resolve().HasZhangXunSlashBenefit(seat);
    internal bool IsIssuedAnniversaryZhangXunCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedAnniversaryZhangXunCandidate(c, w);
    internal IEnumerable<ProgramTriggerCandidate> IssuedAnniversaryZhangXunPlayEndingCandidates(CharacterState phaseActor) => _resolve().IssuedAnniversaryZhangXunPlayEndingCandidates(phaseActor);
    internal bool KeepIssuedZhangXunFollowup(ProgramSkillFrame f) => _resolve().KeepIssuedZhangXunFollowup(f);
    internal bool KeepZhangXunAlreadyUsedSlash(ProgramSkillFrame f) => _resolve().KeepZhangXunAlreadyUsedSlash(f);
    internal bool LegalZhangXunFollowupTarget(int owner, int target) => _resolve().LegalZhangXunFollowupTarget(owner, target);
    internal void ObserveAnniversaryZhangXunLease(ZhangXunSuizhengLeaseEvent fact) => _resolve().ObserveAnniversaryZhangXunLease(fact);
    internal void PublishZhangXunFollowup(ProgramSkillFrame f) => _resolve().PublishZhangXunFollowup(f);
    internal bool ValidZhangXunFollowup(ProgramSkillFrame f) => _resolve().ValidZhangXunFollowup(f);
    internal bool ValidZhangXunLease(ZhangXunSuizhengLeaseEvent lease, IReadOnlyList<IGameEvent> history) => _resolve().ValidZhangXunLease(lease, history);
    internal bool ZhangXunCardDistance(ProgramCardDistanceQuery q) => _resolve().ZhangXunCardDistance(q);
    internal PromptChoice ZhangXunFollowupAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ZhangXunFollowupAi(p, f);
    internal IReadOnlyList<PromptChoice> ZhangXunFollowupChoices(ProgramSkillFrame f) => _resolve().ZhangXunFollowupChoices(f);
    internal CardConversionSource ZhangXunFrameSource(ProgramSkillFrame f) => _resolve().ZhangXunFrameSource(f);
    internal bool ZhangXunPhaseUsed(long leaseId, int turn, int phase) => _resolve().ZhangXunPhaseUsed(leaseId, turn, phase);
    internal bool ZhangXunSlashDistance(ProgramSlashDistanceQuery q) => _resolve().ZhangXunSlashDistance(q);
    internal IReadOnlyList<ZhangXunSuizhengDamageWitness> ZhangXunVictims(GameEngine.ZhangXunDueLease due, int cutoff) => _resolve().ZhangXunVictims(due, cutoff);
    internal bool HasAnniversaryZhangXunCapability { get => _resolve().HasAnniversaryZhangXunCapability; }
}
