#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQianZhaoOlEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.WeifuGrantedEvent> ActiveWeifuGrants(int actor, global::CardGame.Core.CardKind kind) => _resolve().ActiveWeifuGrants(actor, kind);
    internal bool AdvanceQianZhaoSubtree() => _resolve().AdvanceQianZhaoSubtree();
    internal void AnswerQianZhao(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerQianZhao(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginKuansai(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginKuansai(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWeifu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginWeifu(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWeifuTargets(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginWeifuTargets(supplied);
    internal bool CanKuansaiRecover(int owner) => _resolve().CanKuansaiRecover(owner);
    internal global::CardGame.Core.PromptChoice ChooseQianZhaoAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseQianZhaoAi(p, f);
    internal void CollectIssuedWeifuCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedWeifuCandidates(action, window, result);
    internal void CompleteWeifuTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteWeifuTargets(f);
    internal void ConsumeWeifuActualDodge(global::CardGame.Core.IGameEvent payload) => _resolve().ConsumeWeifuActualDodge(payload);
    internal void ConsumeWeifuCounterspell(global::CardGame.Core.IGameEvent payload) => _resolve().ConsumeWeifuCounterspell(payload);
    internal bool ContinuePaidQianZhao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidQianZhao(f);
    internal bool DistanceCardUseExemption90HasWeifuUseDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption90HasWeifuUseDistance(q);
    internal bool DistancePhysicalSlash40HasWeifuUseDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash40HasWeifuUseDistance(q);
    internal bool DrainQianZhaoChildren(long id) => _resolve().DrainQianZhaoChildren(id);
    internal void ExpireWeifuActualTurnGrants() => _resolve().ExpireWeifuActualTurnGrants();
    internal void FinishQianZhao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishQianZhao(f);
    internal void FreezeWeifuActorChange(global::CardGame.Core.IGameEvent fact) => _resolve().FreezeWeifuActorChange(fact);
    internal void FreezeWeifuJudgmentResult(global::CardGame.Core.IGameEvent payload) => _resolve().FreezeWeifuJudgmentResult(payload);
    internal bool HasIssuedWeifuCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasIssuedWeifuCandidate(c, context);
    internal bool HasWeifuTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasWeifuTargetTail(use);
    internal bool HasWeifuUseDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, long? cardUseFrameId) => _resolve().HasWeifuUseDistance(actor, kind, cardUseFrameId);
    internal bool IsQianZhaoRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsQianZhaoRecoveryParent(f, recovery);
    internal bool IsWeifuJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsWeifuJudgment(j, f);
    internal void IssueWeifuUse(long id, global::CardGame.Core.CardActionContext action) => _resolve().IssueWeifuUse(id, action);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KuansaiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KuansaiChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KuansaiGiftCards(int chooser, int owner) => _resolve().KuansaiGiftCards(chooser, owner);
    internal bool KuansaiHasChoice(int target, int owner) => _resolve().KuansaiHasChoice(target, owner);
    internal bool KuansaiNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().KuansaiNormalizeImplicitDrawTwoTargetWindow(query);
    internal global::CardGame.Core.CardUseFrame? KuansaiUse(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().KuansaiUse(context);
    internal bool MatchesWeifuJudgmentInstruction(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => _resolve().MatchesWeifuJudgmentInstruction(f, target, reason, bind, visibility);
    internal void PayKuansai(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice selected) => _resolve().PayKuansai(f, selected);
    internal void PublishQianZhaoChoice(int chooser, global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string text, bool isPrivate) => _resolve().PublishQianZhaoChoice(chooser, f, choices, text, isPrivate);
    internal bool QianZhaoFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().QianZhaoFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? QianZhaoObserverRoot() => _resolve().QianZhaoObserverRoot();
    internal bool QianZhaoSilverLion(global::CardGame.Core.ProgramSkillFrame f, int target, global::CardGame.Core.CardMoveReason? reason) => _resolve().QianZhaoSilverLion(f, target, reason);
    internal bool QualifyQianZhao(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyQianZhao(c, t, context);
    internal void RecordWeifuRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int previousSeat, int nextSeat) => _resolve().RecordWeifuRedirect(redirector, useId, previousSeat, nextSeat);
    internal global::CardGame.Core.CardUseFrame RequireWeifuTargetUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireWeifuTargetUse(f);
    internal bool ResumeKuansai(long id) => _resolve().ResumeKuansai(id);
    internal bool ResumeWeifu(long id) => _resolve().ResumeWeifu(id);
    internal bool ReturnQianZhaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnQianZhaoMovement(f);
    internal bool TryGetWeifuVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, scoped out global::System.Collections.Generic.IReadOnlyList<int> primary) => _resolve().TryGetWeifuVirtualPrimaryReturn(use, parent, out primary);
    internal bool TryReturnWeifuJudgment(global::CardGame.Core.JudgmentFrame j) => _resolve().TryReturnWeifuJudgment(j);
    internal bool WeifuCurrentActorMatches(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.WeifuUseReceipt r) => _resolve().WeifuCurrentActorMatches(use, r);
    internal bool WeifuNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().WeifuNormalizeImplicitDrawTwoTargetWindow(query);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WeifuTargetChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WeifuTargetChoices(f);
    internal bool WeifuUseProof(global::CardGame.Core.CardUseFrame use) => _resolve().WeifuUseProof(use);
    internal bool HasKuansaiCapability { get => _resolve().HasKuansaiCapability; }
    internal bool HasWeifuCapability { get => _resolve().HasWeifuCapability; }
}
