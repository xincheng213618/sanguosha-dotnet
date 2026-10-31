#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCuiLieAnniversaryEntries
{
    internal IReadOnlyList<int> AnniversaryCuiLieDelayedPlacementTargets(NativeOwners.JizhiResolution pending, IReadOnlyList<int> original) => _resolve().AnniversaryCuiLieDelayedPlacementTargets(pending, original);
    internal IEnumerable<ProgramTriggerCandidate> AnniversaryCuiLieIssuedPindianCandidates(IReadOnlyList<PindianRevealedCard> cards) => _resolve().AnniversaryCuiLieIssuedPindianCandidates(cards);
    internal int AnniversaryCuiLiePindianBonus(int owner) => _resolve().AnniversaryCuiLiePindianBonus(owner);
    internal void AnswerCuiLie(PromptChoice answer) => _resolve().AnswerCuiLie(answer);
    internal void ApplyCuiLieMutation(ProgramSkillFrame f, IReadOnlyList<int> added) => _resolve().ApplyCuiLieMutation(f, added);
    internal SkillProgramStepOutcome BeginCuiLie(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginCuiLie(supplied, op);
    internal bool CanOfferCuiLie(ProgramTriggerLegalityContext c) => _resolve().CanOfferCuiLie(c);
    internal bool ContinueAnniversaryCuiLieDelayedPlacement(CardUseFrame use) => _resolve().ContinueAnniversaryCuiLieDelayedPlacement(use);
    internal void ContinueCuiLie(long id) => _resolve().ContinueCuiLie(id);
    internal bool CuiLieAdditionalEligible(CardUseFrame u, int seat) => _resolve().CuiLieAdditionalEligible(u, seat);
    internal bool CuiLieAdvanceChildren() => _resolve().CuiLieAdvanceChildren();
    internal PromptChoice CuiLieAi(PendingDecision p) => _resolve().CuiLieAi(p);
    internal ProgramSkillFrame CuiLieChildren(ProgramSkillFrame f, AnniversaryCuiLieStage stage) => _resolve().CuiLieChildren(f, stage);
    internal PromptChoice CuiLieChoice(ProgramSkillFrame f, string verb, string value, string text, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().CuiLieChoice(f, verb, value, text, cards, targets);
    internal IReadOnlyList<PromptChoice> CuiLieChoices(ProgramSkillFrame f) => _resolve().CuiLieChoices(f);
    internal int CuiLieChooser(ProgramSkillFrame f) => _resolve().CuiLieChooser(f);
    internal bool CuiLieContestEligible(int owner, CardUseFrame use) => _resolve().CuiLieContestEligible(owner, use);
    internal bool CuiLieDelayedTargetLegal(CardUseFrame use, int target) => _resolve().CuiLieDelayedTargetLegal(use, target);
    internal bool CuiLieDying() => _resolve().CuiLieDying();
    internal bool CuiLieFinalizedWindow(ProgramTargetCardWindowQuery q) => _resolve().CuiLieFinalizedWindow(q);
    internal bool CuiLieFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().CuiLieFirstChild(f, child);
    internal IEnumerable<RuleQueryContribution> CuiLieIncoming(int target) => _resolve().CuiLieIncoming(target);
    internal AnniversaryCuiLieLeaseEvent[] CuiLieLeases(int owner) => _resolve().CuiLieLeases(owner);
    internal bool CuiLieMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().CuiLieMovement(f, effect, p);
    internal bool CuiLieObserverEdge(int i) => _resolve().CuiLieObserverEdge(i);
    internal ProgramSkillFrame? CuiLieObserverRoot() => _resolve().CuiLieObserverRoot();
    internal bool CuiLieObserverSuffix(int i) => _resolve().CuiLieObserverSuffix(i);
    internal bool CuiLieOriginal(ProgramSkillFrame f, SkillProgramEffectOp op) => _resolve().CuiLieOriginal(f, op);
    internal bool CuiLieRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().CuiLieRecoveryParent(f, recovery);
    internal Card[] CuiLieTransferable(ProgramSkillFrame f) => _resolve().CuiLieTransferable(f);
    internal CardUseFrame? CuiLieUse(ProgramSkillWindowContext? w) => _resolve().CuiLieUse(w);
    internal void FinishCuiLie(ProgramSkillFrame f) => _resolve().FinishCuiLie(f);
    internal void GiveCuiLie(ProgramSkillFrame f) => _resolve().GiveCuiLie(f);
    internal bool HasAnniversaryCuiLieCancellation(CardUseFrame use) => _resolve().HasAnniversaryCuiLieCancellation(use);
    internal bool HasAnniversaryCuiLieDelayedTail(CardUseFrame use) => _resolve().HasAnniversaryCuiLieDelayedTail(use);
    internal bool IsAnniversaryCuiLieClaimedUseMaterial(long useId, int cardId) => _resolve().IsAnniversaryCuiLieClaimedUseMaterial(useId, cardId);
    internal bool IsAnniversaryCuiLieDelayedMaterialPlaced(CardUseFrame use) => _resolve().IsAnniversaryCuiLieDelayedMaterialPlaced(use);
    internal bool IsAnniversaryCuiLiePindianSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().IsAnniversaryCuiLiePindianSettlementBatch(f, batch);
    internal bool IsAnniversaryCuiLieTargetMutationFact(CardUseFrame use, AnniversaryCuiLieUseChangedEvent e, IReadOnlyList<int> original, int? currentProofActor = default) => _resolve().IsAnniversaryCuiLieTargetMutationFact(use, e, original, currentProofActor);
    internal bool IsIssuedCuiLieDraw(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedCuiLieDraw(c, w);
    internal void IssueCuiLieDraw(ProgramSkillFrame f) => _resolve().IssueCuiLieDraw(f);
    internal bool KeepCuiLie(ProgramSkillFrame f) => _resolve().KeepCuiLie(f);
    internal void PublishCuiLie(ProgramSkillFrame f) => _resolve().PublishCuiLie(f);
    internal bool ResumeCuiLie(long id) => _resolve().ResumeCuiLie(id);
    internal void ReturnCuiLieDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw) => _resolve().ReturnCuiLieDraw(supplied, draw);
    internal bool ReturnCuiLieMovement(ProgramSkillFrame f) => _resolve().ReturnCuiLieMovement(f);
    internal void SettleCuiLieContest(ProgramSkillFrame f) => _resolve().SettleCuiLieContest(f);
    internal void SkipCuiLieDelayedTarget(CardUseFrame use, int target, CardEffectSkipReason reason) => _resolve().SkipCuiLieDelayedTarget(use, target, reason);
    internal bool TryBeginAnniversaryCuiLieDelayedPlacement(ProgramCardTriggerWindowFrame window) => _resolve().TryBeginAnniversaryCuiLieDelayedPlacement(window);
    internal bool TrySkipAnniversaryCuiLieDelayedTarget(NullificationWindowFrame window) => _resolve().TrySkipAnniversaryCuiLieDelayedTarget(window);
    internal bool ValidCuiLie(ProgramSkillFrame f) => _resolve().ValidCuiLie(f);
    internal bool WindowSettlement_IsAnniversaryCuiLiePindianSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsAnniversaryCuiLiePindianSettlementBatch(frame, batch);
    internal bool HasAnniversaryCuiLieCapability { get => _resolve().HasAnniversaryCuiLieCapability; }
}
