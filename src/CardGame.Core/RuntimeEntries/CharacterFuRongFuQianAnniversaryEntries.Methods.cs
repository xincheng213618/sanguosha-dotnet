#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFuRongFuQianAnniversaryEntries
{
    internal IEnumerable<AnniversaryXueweiGrant> ActiveAnniversaryXueweiGrants() => _resolve().ActiveAnniversaryXueweiGrants();
    internal bool AdmitAnniversaryXuewei(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().AdmitAnniversaryXuewei(c, context);
    internal bool AdvanceAnniversaryFuPairObserver() => _resolve().AdvanceAnniversaryFuPairObserver();
    internal PromptChoice AiAnniversaryXuewei(PendingDecision p, ProgramSkillFrame f) => _resolve().AiAnniversaryXuewei(p, f);
    internal PromptChoice AiAnniversaryYuguan(PendingDecision p, ProgramSkillFrame f) => _resolve().AiAnniversaryYuguan(p, f);
    internal Dictionary<string, string> AnniversaryFuPairArgs(ProgramSkillFrame f, string branch) => _resolve().AnniversaryFuPairArgs(f, branch);
    internal IReadOnlyList<PromptChoice> AnniversaryFuPairChoices(ProgramSkillFrame f) => _resolve().AnniversaryFuPairChoices(f);
    internal bool AnniversaryFuPairFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().AnniversaryFuPairFirstChild(f, child);
    internal ProgramSkillFrame? AnniversaryFuPairObserverRoot() => _resolve().AnniversaryFuPairObserverRoot();
    internal AnniversaryXueweiGrant? AnniversaryXueweiWitness(ProgramTriggerCandidate c, ProgramSkillWindowContext context, bool allowPrevented = false) => _resolve().AnniversaryXueweiWitness(c, context, allowPrevented);
    internal void AnswerAnniversaryFuPair(PromptChoice choice) => _resolve().AnswerAnniversaryFuPair(choice);
    internal SkillProgramStepOutcome BeginAnniversaryFuPair(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginAnniversaryFuPair(supplied, op);
    internal bool CanArmAnniversaryXuewei(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanArmAnniversaryXuewei(c, context);
    internal bool CanOfferAnniversaryFuPair(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferAnniversaryFuPair(c, trigger, context);
    internal bool CanOfferAnniversaryYuguan(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanOfferAnniversaryYuguan(c, context);
    internal IEnumerable<BeforeDamageProgramCandidate> CollectAnniversaryFuRongFuQianXueweiCandidates(int sourceSeat, int targetSeat, bool rangeChainOnly) => _resolve().CollectAnniversaryFuRongFuQianXueweiCandidates(sourceSeat, targetSeat, rangeChainOnly);
    internal void ContinueAnniversaryFuPair(long id) => _resolve().ContinueAnniversaryFuPair(id);
    internal bool DrainAnniversaryFuPair(long id) => _resolve().DrainAnniversaryFuPair(id);
    internal void ExpireAnniversaryXuewei(TurnStartedEvent fact) => _resolve().ExpireAnniversaryXuewei(fact);
    internal void FinishAnniversaryFuPair(ProgramSkillFrame f) => _resolve().FinishAnniversaryFuPair(f);
    internal bool HasAnniversaryFuPairDying() => _resolve().HasAnniversaryFuPairDying();
    internal bool KeepPaidAnniversaryFuPair(ProgramSkillFrame f) => _resolve().KeepPaidAnniversaryFuPair(f);
    internal void PublishAnniversaryFuPair(ProgramSkillFrame f) => _resolve().PublishAnniversaryFuPair(f);
    internal bool ResumeAnniversaryFuPair(long id) => _resolve().ResumeAnniversaryFuPair(id);
    internal void ReturnAnniversaryFuPairDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw) => _resolve().ReturnAnniversaryFuPairDraw(supplied, draw);
    internal bool ReturnAnniversaryFuPairMovement(ProgramSkillFrame f) => _resolve().ReturnAnniversaryFuPairMovement(f);
    internal bool HasAnniversaryFuPair { get => _resolve().HasAnniversaryFuPair; }
}
