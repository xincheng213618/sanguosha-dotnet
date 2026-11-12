#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDaQiaoLeEntries
{
    internal bool AdvanceZixiEffectsChildren() => _resolve().AdvanceZixiEffectsChildren();
    internal bool AdvanceZixiPlacementChildren() => _resolve().AdvanceZixiPlacementChildren();
    internal void AnswerZixiPlacement(PromptChoice choice) => _resolve().AnswerZixiPlacement(choice);
    internal SkillProgramStepOutcome BeginZixiEffects(ProgramSkillFrame supplied) => _resolve().BeginZixiEffects(supplied);
    internal SkillProgramStepOutcome BeginZixiPlacement(ProgramSkillFrame supplied) => _resolve().BeginZixiPlacement(supplied);
    internal bool CanOfferZixiEffects(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferZixiEffects(candidate, trigger, context);
    internal bool CanPlaceZixiQin(int owner, Card card) => _resolve().CanPlaceZixiQin(owner, card);
    internal void CaptureZixiEffectsObtain(long id, long batch, IReadOnlyList<CardMovementRecord> moves) => _resolve().CaptureZixiEffectsObtain(id, batch, moves);
    internal PromptChoice ChooseAiZixiPlacement(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAiZixiPlacement(p, f);
    internal void CompleteZixiPlacement(ProgramSkillFrame f, bool completed) => _resolve().CompleteZixiPlacement(f, completed);
    internal bool ContinueIssuedZixiEffects(ProgramSkillFrame f) => _resolve().ContinueIssuedZixiEffects(f);
    internal void ContinueZixiEffects(long id) => _resolve().ContinueZixiEffects(id);
    internal void ContinueZixiPlacement(long id) => _resolve().ContinueZixiPlacement(id);
    internal bool DrainZixiEffects(ProgramSkillFrame f) => _resolve().DrainZixiEffects(f);
    internal void FinishZixiEffects(ProgramSkillFrame f) => _resolve().FinishZixiEffects(f);
    internal bool HasPaidZixiPlacement(ProgramSkillFrame f) => _resolve().HasPaidZixiPlacement(f);
    internal bool IsZixiEffectsRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsZixiEffectsRecoveryParent(f, recovery);
    internal bool IsZixiPlacementRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsZixiPlacementRecoveryParent(f, recovery);
    internal void IssueZixiExtraEffect(ProgramSkillFrame f, CardUseFrame use, ProgramCardTriggerWindowFrame window) => _resolve().IssueZixiExtraEffect(f, use, window);
    internal bool PermitPaidZixiPlacement(ProgramSkillFrame f) => _resolve().PermitPaidZixiPlacement(f);
    internal void PublishZixiPlacement(ProgramSkillFrame f) => _resolve().PublishZixiPlacement(f);
    internal bool ResumeZixiEffects(long id) => _resolve().ResumeZixiEffects(id);
    internal bool ResumeZixiPlacement(long id) => _resolve().ResumeZixiPlacement(id);
    internal void ReturnZixiEffectsDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnZixiEffectsDraw(supplied, outcome);
    internal bool ReturnZixiEffectsMovement(ProgramSkillFrame f) => _resolve().ReturnZixiEffectsMovement(f);
    internal bool ReturnZixiPlacementMovement(ProgramSkillFrame f) => _resolve().ReturnZixiPlacementMovement(f);
    internal bool TryGetZixiEffectsUse(int actor, ProgramSkillWindowContext context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window, scoped out int target) => _resolve().TryGetZixiEffectsUse(actor, context, out use, out window, out target);
    internal bool ValidZixiEffectsStarted(ProgramSkillFrame f) => _resolve().ValidZixiEffectsStarted(f);
    internal bool ZixiEffectsFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZixiEffectsFirstChild(f, child);
    internal bool ZixiEffectsLifecycleEdge(int index) => _resolve().ZixiEffectsLifecycleEdge(index);
    internal ProgramSkillFrame? ZixiEffectsObserverRoot() => _resolve().ZixiEffectsObserverRoot();
    internal bool ZixiEffectsOxSpill(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().ZixiEffectsOxSpill(f, batch);
    internal bool ZixiEffectsPrimaryMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().ZixiEffectsPrimaryMovement(f, m);
    internal bool ZixiFinalizedBeforeActivityTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().ZixiFinalizedBeforeActivityTargetWindow(query);
    internal IReadOnlyList<Card> ZixiJudgmentAreaCards(int target) => _resolve().ZixiJudgmentAreaCards(target);
    internal bool ZixiNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().ZixiNormalizeImplicitDrawTwoTargetWindow(query);
    internal bool ZixiPlacementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().ZixiPlacementBatch(f, batch);
    internal IReadOnlyList<PromptChoice> ZixiPlacementChoices(ProgramSkillFrame f) => _resolve().ZixiPlacementChoices(f);
    internal bool ZixiPlacementFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZixiPlacementFirstChild(f, child);
    internal bool ZixiPlacementLionRemoval(ProgramSkillFrame f, int seat) => _resolve().ZixiPlacementLionRemoval(f, seat);
    internal bool ZixiPlacementMother(ProgramSkillFrame f) => _resolve().ZixiPlacementMother(f);
    internal ProgramSkillFrame? ZixiPlacementObserverRoot() => _resolve().ZixiPlacementObserverRoot();
    internal bool ZixiPlacementObserverSuffix(int index) => _resolve().ZixiPlacementObserverSuffix(index);
    internal bool ZixiPlacementRoot(ProgramSkillFrame f) => _resolve().ZixiPlacementRoot(f);
    internal bool HasZixiEffectsCapability { get => _resolve().HasZixiEffectsCapability; }
}
