#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDaQiaoMouMobileEntries
{
    internal bool AdvanceMouDaQiaoChildren() => _resolve().AdvanceMouDaQiaoChildren();
    internal PromptChoice AiMouDaQiaoGuose(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouDaQiaoGuose(p, f);
    internal PromptChoice AiMouDaQiaoHeart(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouDaQiaoHeart(p, f);
    internal void AnswerMouDaQiaoGuose(PromptChoice choice) => _resolve().AnswerMouDaQiaoGuose(choice);
    internal void AnswerMouDaQiaoHeart(PromptChoice choice) => _resolve().AnswerMouDaQiaoHeart(choice);
    internal SkillProgramStepOutcome BeginMouDaQiaoGuose(ProgramSkillFrame supplied) => _resolve().BeginMouDaQiaoGuose(supplied);
    internal SkillProgramStepOutcome BeginMouDaQiaoLiuliPhase(ProgramSkillFrame f) => _resolve().BeginMouDaQiaoLiuliPhase(f);
    internal bool CanMouDaQiaoGuose(ProgramActivationLegalityContext c) => _resolve().CanMouDaQiaoGuose(c);
    internal void ContinueMouDaQiaoGuose(long id) => _resolve().ContinueMouDaQiaoGuose(id);
    internal bool ContinuePaidMouDaQiao(ProgramSkillFrame f) => _resolve().ContinuePaidMouDaQiao(f);
    internal bool DrainMouDaQiao(ProgramSkillFrame f) => _resolve().DrainMouDaQiao(f);
    internal void FinishMouDaQiaoLiuliPhase(ProgramSkillFrame f) => _resolve().FinishMouDaQiaoLiuliPhase(f);
    internal void GrantMouDaQiaoMark(ProgramSkillFrame f, MouDaQiaoLiuliReceipt r, int target) => _resolve().GrantMouDaQiaoMark(f, r, target);
    internal bool HasUnissuedMouDaQiaoPhase(int owner) => _resolve().HasUnissuedMouDaQiaoPhase(owner);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeMouDaQiaoPhaseCandidates(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _resolve().IncludeMouDaQiaoPhaseCandidates(owner, window, ordinary);
    internal bool IsIssuedMouDaQiaoPhaseCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().IsIssuedMouDaQiaoPhaseCandidate(candidate, c);
    internal bool IsReturningMouDaQiaoLiuliPhase(ProgramSkillFrame f) => _resolve().IsReturningMouDaQiaoLiuliPhase(f);
    internal MouDaQiaoLiuliMarkGrantedEvent? LiveMouDaQiaoMark() => _resolve().LiveMouDaQiaoMark();
    internal bool MouDaQiaoDying() => _resolve().MouDaQiaoDying();
    internal bool MouDaQiaoFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouDaQiaoFirstChild(f, child);
    internal bool MouDaQiaoGuoseTarget(int owner, int? card, int target, string branch) => _resolve().MouDaQiaoGuoseTarget(owner, card, target, branch);
    internal int MouDaQiaoGuoseUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _resolve().MouDaQiaoGuoseUses(owner, phase);
    internal bool MouDaQiaoHeartAvailable(int owner, int actualTurn) => _resolve().MouDaQiaoHeartAvailable(owner, actualTurn);
    internal IReadOnlyList<PromptChoice> MouDaQiaoHeartChoices(ProgramSkillFrame f) => _resolve().MouDaQiaoHeartChoices(f);
    internal ProgramSkillFrame? MouDaQiaoObserverRoot() => _resolve().MouDaQiaoObserverRoot();
    internal bool MouDaQiaoObserverSuffix(int index) => _resolve().MouDaQiaoObserverSuffix(index);
    internal int MouDaQiaoProjectedPhaseUses(int owner, string skill, int fallback) => _resolve().MouDaQiaoProjectedPhaseUses(owner, skill, fallback);
    internal bool MouDaQiaoRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouDaQiaoRecovery(f, recovery);
    internal IEnumerable<Card> MouDaQiaoRemovable(int target) => _resolve().MouDaQiaoRemovable(target);
    internal IReadOnlyList<PromptChoice> MouDaQiaoRemovalChoices(ProgramSkillFrame f) => _resolve().MouDaQiaoRemovalChoices(f);
    internal bool PaidMouDaQiaoLiuli(ProgramSkillFrame f) => _resolve().PaidMouDaQiaoLiuli(f);
    internal SkillProgramStepOutcome ResolveMouDaQiaoHeart(ProgramSkillFrame supplied) => _resolve().ResolveMouDaQiaoHeart(supplied);
    internal bool ResumeMouDaQiao(long id) => _resolve().ResumeMouDaQiao(id);
    internal void ReturnMouDaQiaoGuoseDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnMouDaQiaoGuoseDraw(f, o);
    internal void ReturnMouDaQiaoGuoseUse(long useId) => _resolve().ReturnMouDaQiaoGuoseUse(useId);
    internal bool ReturnMouDaQiaoMovement(ProgramSkillFrame f) => _resolve().ReturnMouDaQiaoMovement(f);
    internal void SetMouDaQiaoMarker(long frameId, int target, int count, int issuer) => _resolve().SetMouDaQiaoMarker(frameId, target, count, issuer);
    internal bool ValidMouDaQiaoGuose(ProgramSkillFrame f) => _resolve().ValidMouDaQiaoGuose(f);
    internal bool ValidMouDaQiaoPhase(ProgramSkillFrame f) => _resolve().ValidMouDaQiaoPhase(f);
    internal bool HasMouDaQiao { get => _resolve().HasMouDaQiao; }
    internal bool HasMouDaQiaoLiuli { get => _resolve().HasMouDaQiaoLiuli; }
    internal int MouDaQiaoGuoseLimit { get => _resolve().MouDaQiaoGuoseLimit; }
}
