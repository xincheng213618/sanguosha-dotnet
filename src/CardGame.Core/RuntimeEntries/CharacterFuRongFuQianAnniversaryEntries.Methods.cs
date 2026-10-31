#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFuRongFuQianAnniversaryEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.AnniversaryXueweiGrant> ActiveAnniversaryXueweiGrants() => _resolve().ActiveAnniversaryXueweiGrants();
    internal bool AdmitAnniversaryXuewei(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AdmitAnniversaryXuewei(c, context);
    internal bool AdvanceAnniversaryFuPairObserver() => _resolve().AdvanceAnniversaryFuPairObserver();
    internal global::CardGame.Core.PromptChoice AiAnniversaryXuewei(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiAnniversaryXuewei(p, f);
    internal global::CardGame.Core.PromptChoice AiAnniversaryYuguan(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiAnniversaryYuguan(p, f);
    internal global::System.Collections.Generic.Dictionary<string, string> AnniversaryFuPairArgs(global::CardGame.Core.ProgramSkillFrame f, string branch) => _resolve().AnniversaryFuPairArgs(f, branch);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AnniversaryFuPairChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnniversaryFuPairChoices(f);
    internal bool AnniversaryFuPairFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().AnniversaryFuPairFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? AnniversaryFuPairObserverRoot() => _resolve().AnniversaryFuPairObserverRoot();
    internal global::CardGame.Core.AnniversaryXueweiGrant? AnniversaryXueweiWitness(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context, bool allowPrevented = false) => _resolve().AnniversaryXueweiWitness(c, context, allowPrevented);
    internal void AnswerAnniversaryFuPair(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerAnniversaryFuPair(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryFuPair(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginAnniversaryFuPair(supplied, op);
    internal bool CanArmAnniversaryXuewei(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanArmAnniversaryXuewei(c, context);
    internal bool CanOfferAnniversaryFuPair(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferAnniversaryFuPair(c, trigger, context);
    internal bool CanOfferAnniversaryYuguan(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferAnniversaryYuguan(c, context);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectAnniversaryFuRongFuQianXueweiCandidates(int sourceSeat, int targetSeat, bool rangeChainOnly) => _resolve().CollectAnniversaryFuRongFuQianXueweiCandidates(sourceSeat, targetSeat, rangeChainOnly);
    internal void ContinueAnniversaryFuPair(long id) => _resolve().ContinueAnniversaryFuPair(id);
    internal bool DrainAnniversaryFuPair(long id) => _resolve().DrainAnniversaryFuPair(id);
    internal void ExpireAnniversaryXuewei(global::CardGame.Core.TurnStartedEvent fact) => _resolve().ExpireAnniversaryXuewei(fact);
    internal void FinishAnniversaryFuPair(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishAnniversaryFuPair(f);
    internal bool HasAnniversaryFuPairBeforeDamageObserver(long id) => _resolve().HasAnniversaryFuPairBeforeDamageObserver(id);
    internal bool HasAnniversaryFuPairDying() => _resolve().HasAnniversaryFuPairDying();
    internal bool HasAnniversaryFuRongFuQianPrompt() => _resolve().HasAnniversaryFuRongFuQianPrompt();
    internal bool KeepPaidAnniversaryFuPair(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidAnniversaryFuPair(f);
    internal void PublishAnniversaryFuPair(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishAnniversaryFuPair(f);
    internal void RequireAnniversaryFuPair(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireAnniversaryFuPair(f);
    internal bool ResumeAnniversaryFuPair(long id) => _resolve().ResumeAnniversaryFuPair(id);
    internal void ReturnAnniversaryFuPairDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome draw) => _resolve().ReturnAnniversaryFuPairDraw(supplied, draw);
    internal bool ReturnAnniversaryFuPairMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnniversaryFuPairMovement(f);
    internal bool HasAnniversaryFuPair { get => _resolve().HasAnniversaryFuPair; }
}
