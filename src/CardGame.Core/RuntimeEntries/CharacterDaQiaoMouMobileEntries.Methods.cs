#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDaQiaoMouMobileEntries
{
    internal bool AdvanceMouDaQiaoChildren() => _resolve().AdvanceMouDaQiaoChildren();
    internal global::CardGame.Core.PromptChoice AiMouDaQiaoGuose(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouDaQiaoGuose(p, f);
    internal global::CardGame.Core.PromptChoice AiMouDaQiaoHeart(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouDaQiaoHeart(p, f);
    internal void AnswerMouDaQiaoGuose(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouDaQiaoGuose(choice);
    internal void AnswerMouDaQiaoHeart(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouDaQiaoHeart(choice);
    internal void AssertMouDaQiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMouDaQiao(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouDaQiaoGuose(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMouDaQiaoGuose(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouDaQiaoLiuliPhase(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginMouDaQiaoLiuliPhase(f);
    internal bool CanMouDaQiaoGuose(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanMouDaQiaoGuose(c);
    internal void ContinueMouDaQiaoGuose(long id) => _resolve().ContinueMouDaQiaoGuose(id);
    internal bool ContinuePaidMouDaQiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidMouDaQiao(f);
    internal bool DrainMouDaQiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouDaQiao(f);
    internal void FinishMouDaQiaoLiuliPhase(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouDaQiaoLiuliPhase(f);
    internal void GrantMouDaQiaoMark(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.MouDaQiaoLiuliReceipt r, int target) => _resolve().GrantMouDaQiaoMark(f, r, target);
    internal bool HasUnissuedMouDaQiaoPhase(int owner) => _resolve().HasUnissuedMouDaQiaoPhase(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeMouDaQiaoPhaseCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> ordinary) => _resolve().IncludeMouDaQiaoPhaseCandidates(owner, window, ordinary);
    internal bool IsIssuedMouDaQiaoPhaseCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().IsIssuedMouDaQiaoPhaseCandidate(candidate, c);
    internal bool IsReturningMouDaQiaoLiuliPhase(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsReturningMouDaQiaoLiuliPhase(f);
    internal global::CardGame.Core.MouDaQiaoLiuliMarkGrantedEvent? LiveMouDaQiaoMark() => _resolve().LiveMouDaQiaoMark();
    internal bool MouDaQiaoDamageObserver(long id) => _resolve().MouDaQiaoDamageObserver(id);
    internal bool MouDaQiaoDying() => _resolve().MouDaQiaoDying();
    internal bool MouDaQiaoFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouDaQiaoFirstChild(f, child);
    internal bool MouDaQiaoGuoseTarget(int owner, int? card, int target, string branch) => _resolve().MouDaQiaoGuoseTarget(owner, card, target, branch);
    internal int MouDaQiaoGuoseUses(int owner, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().MouDaQiaoGuoseUses(owner, phase);
    internal bool MouDaQiaoHeartAvailable(int owner, int actualTurn) => _resolve().MouDaQiaoHeartAvailable(owner, actualTurn);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouDaQiaoHeartChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDaQiaoHeartChoices(f);
    internal bool MouDaQiaoMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation m) => _resolve().MouDaQiaoMovementProof(f, e, m);
    internal global::CardGame.Core.ProgramSkillFrame? MouDaQiaoObserverRoot() => _resolve().MouDaQiaoObserverRoot();
    internal bool MouDaQiaoObserverSuffix(int index) => _resolve().MouDaQiaoObserverSuffix(index);
    internal int MouDaQiaoProjectedPhaseUses(int owner, string skill, int fallback) => _resolve().MouDaQiaoProjectedPhaseUses(owner, skill, fallback);
    internal bool MouDaQiaoRecovery(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouDaQiaoRecovery(f, recovery);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> MouDaQiaoRemovable(int target) => _resolve().MouDaQiaoRemovable(target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouDaQiaoRemovalChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDaQiaoRemovalChoices(f);
    internal bool PaidMouDaQiaoLiuli(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PaidMouDaQiaoLiuli(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMouDaQiaoHeart(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveMouDaQiaoHeart(supplied);
    internal bool ResumeMouDaQiao(long id) => _resolve().ResumeMouDaQiao(id);
    internal void ReturnMouDaQiaoGuoseDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnMouDaQiaoGuoseDraw(f, o);
    internal void ReturnMouDaQiaoGuoseUse(long useId) => _resolve().ReturnMouDaQiaoGuoseUse(useId);
    internal bool ReturnMouDaQiaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouDaQiaoMovement(f);
    internal void SetMouDaQiaoMarker(long frameId, int target, int count, int issuer) => _resolve().SetMouDaQiaoMarker(frameId, target, count, issuer);
    internal bool ValidMouDaQiaoGuose(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouDaQiaoGuose(f);
    internal bool ValidMouDaQiaoPhase(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouDaQiaoPhase(f);
    internal bool HasMouDaQiao { get => _resolve().HasMouDaQiao; }
    internal bool HasMouDaQiaoLiuli { get => _resolve().HasMouDaQiaoLiuli; }
    internal int MouDaQiaoGuoseLimit { get => _resolve().MouDaQiaoGuoseLimit; }
}
