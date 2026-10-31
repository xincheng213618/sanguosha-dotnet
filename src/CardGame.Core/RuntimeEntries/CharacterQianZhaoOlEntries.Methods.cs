#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQianZhaoOlEntries
{
    internal IReadOnlyList<WeifuGrantedEvent> ActiveWeifuGrants(int actor, CardKind kind) => _resolve().ActiveWeifuGrants(actor, kind);
    internal bool AdvanceQianZhaoSubtree() => _resolve().AdvanceQianZhaoSubtree();
    internal void AnswerQianZhao(PromptChoice selected) => _resolve().AnswerQianZhao(selected);
    internal SkillProgramStepOutcome BeginKuansai(ProgramSkillFrame supplied) => _resolve().BeginKuansai(supplied);
    internal SkillProgramStepOutcome BeginWeifu(ProgramSkillFrame supplied) => _resolve().BeginWeifu(supplied);
    internal SkillProgramStepOutcome BeginWeifuTargets(ProgramSkillFrame supplied) => _resolve().BeginWeifuTargets(supplied);
    internal bool CanKuansaiRecover(int owner) => _resolve().CanKuansaiRecover(owner);
    internal PromptChoice ChooseQianZhaoAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseQianZhaoAi(p, f);
    internal void CollectIssuedWeifuCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedWeifuCandidates(action, window, result);
    internal void CompleteWeifuTargets(ProgramSkillFrame f) => _resolve().CompleteWeifuTargets(f);
    internal void ConsumeWeifuActualDodge(IGameEvent payload) => _resolve().ConsumeWeifuActualDodge(payload);
    internal void ConsumeWeifuCounterspell(IGameEvent payload) => _resolve().ConsumeWeifuCounterspell(payload);
    internal bool ContinuePaidQianZhao(ProgramSkillFrame f) => _resolve().ContinuePaidQianZhao(f);
    internal bool DistanceCardUseExemption90HasWeifuUseDistance(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption90HasWeifuUseDistance(q);
    internal bool DistancePhysicalSlash40HasWeifuUseDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash40HasWeifuUseDistance(q);
    internal bool DrainQianZhaoChildren(long id) => _resolve().DrainQianZhaoChildren(id);
    internal void ExpireWeifuActualTurnGrants() => _resolve().ExpireWeifuActualTurnGrants();
    internal void FinishQianZhao(ProgramSkillFrame f) => _resolve().FinishQianZhao(f);
    internal void FreezeWeifuActorChange(IGameEvent fact) => _resolve().FreezeWeifuActorChange(fact);
    internal void FreezeWeifuJudgmentResult(IGameEvent payload) => _resolve().FreezeWeifuJudgmentResult(payload);
    internal bool HasIssuedWeifuCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().HasIssuedWeifuCandidate(c, context);
    internal bool HasWeifuTargetTail(CardUseFrame use) => _resolve().HasWeifuTargetTail(use);
    internal bool HasWeifuUseDistance(CharacterState actor, CardKind kind, long? cardUseFrameId) => _resolve().HasWeifuUseDistance(actor, kind, cardUseFrameId);
    internal bool IsQianZhaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsQianZhaoRecoveryParent(f, recovery);
    internal bool IsWeifuJudgment(JudgmentFrame j, ProgramSkillFrame f) => _resolve().IsWeifuJudgment(j, f);
    internal void IssueWeifuUse(long id, CardActionContext action) => _resolve().IssueWeifuUse(id, action);
    internal IReadOnlyList<PromptChoice> KuansaiChoices(ProgramSkillFrame f) => _resolve().KuansaiChoices(f);
    internal IReadOnlyList<Card> KuansaiGiftCards(int chooser, int owner) => _resolve().KuansaiGiftCards(chooser, owner);
    internal bool KuansaiHasChoice(int target, int owner) => _resolve().KuansaiHasChoice(target, owner);
    internal bool KuansaiNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().KuansaiNormalizeImplicitDrawTwoTargetWindow(query);
    internal CardUseFrame? KuansaiUse(ProgramSkillWindowContext context) => _resolve().KuansaiUse(context);
    internal bool MatchesWeifuJudgmentInstruction(ProgramSkillFrame f, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _resolve().MatchesWeifuJudgmentInstruction(f, target, reason, bind, visibility);
    internal void PayKuansai(ProgramSkillFrame f, PromptChoice selected) => _resolve().PayKuansai(f, selected);
    internal void PublishQianZhaoChoice(int chooser, ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices, string text, bool isPrivate) => _resolve().PublishQianZhaoChoice(chooser, f, choices, text, isPrivate);
    internal bool QianZhaoFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().QianZhaoFirstChild(f, child);
    internal ProgramSkillFrame? QianZhaoObserverRoot() => _resolve().QianZhaoObserverRoot();
    internal bool QianZhaoSilverLion(ProgramSkillFrame f, int target, CardMoveReason? reason) => _resolve().QianZhaoSilverLion(f, target, reason);
    internal bool QualifyQianZhao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyQianZhao(c, t, context);
    internal void RecordWeifuRedirect(ProgramSkillFrame redirector, long useId, int previousSeat, int nextSeat) => _resolve().RecordWeifuRedirect(redirector, useId, previousSeat, nextSeat);
    internal CardUseFrame RequireWeifuTargetUse(ProgramSkillFrame f) => _resolve().RequireWeifuTargetUse(f);
    internal bool ResumeKuansai(long id) => _resolve().ResumeKuansai(id);
    internal bool ResumeWeifu(long id) => _resolve().ResumeWeifu(id);
    internal bool ReturnQianZhaoMovement(ProgramSkillFrame f) => _resolve().ReturnQianZhaoMovement(f);
    internal bool TryGetWeifuVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, scoped out IReadOnlyList<int> primary) => _resolve().TryGetWeifuVirtualPrimaryReturn(use, parent, out primary);
    internal bool TryReturnWeifuJudgment(JudgmentFrame j) => _resolve().TryReturnWeifuJudgment(j);
    internal bool WeifuCurrentActorMatches(CardUseFrame use, WeifuUseReceipt r) => _resolve().WeifuCurrentActorMatches(use, r);
    internal bool WeifuNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().WeifuNormalizeImplicitDrawTwoTargetWindow(query);
    internal IReadOnlyList<PromptChoice> WeifuTargetChoices(ProgramSkillFrame f) => _resolve().WeifuTargetChoices(f);
    internal bool WeifuUseProof(CardUseFrame use) => _resolve().WeifuUseProof(use);
    internal bool HasKuansaiCapability { get => _resolve().HasKuansaiCapability; }
    internal bool HasWeifuCapability { get => _resolve().HasWeifuCapability; }
}
