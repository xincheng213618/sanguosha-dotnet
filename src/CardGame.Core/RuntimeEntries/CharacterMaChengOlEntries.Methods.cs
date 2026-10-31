#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaChengOlEntries
{
    internal bool AdvanceMaChengSubtree() => _resolve().AdvanceMaChengSubtree();
    internal void AnswerChenglie(PromptChoice choice) => _resolve().AnswerChenglie(choice);
    internal void ApplyChenglieTargets(ProgramSkillFrame f) => _resolve().ApplyChenglieTargets(f);
    internal SkillProgramStepOutcome BeginChenglie(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginChenglie(supplied, op);
    internal void BeginChenglieCleanup(ProgramSkillFrame f) => _resolve().BeginChenglieCleanup(f);
    internal PromptChoice ChenglieChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().ChenglieChoice(f, branch, label, cards, targets);
    internal IReadOnlyList<PromptChoice> ChenglieChoices(ProgramSkillFrame f) => _resolve().ChenglieChoices(f);
    internal bool ChenglieInitialEligible(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ChenglieInitialEligible(candidate, context);
    internal IReadOnlyList<int> ChenglieLegalTargets(CardUseFrame use, IReadOnlyList<int> added) => _resolve().ChenglieLegalTargets(use, added);
    internal CardUseFrame? ChenglieOutcomeUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().ChenglieOutcomeUse(c, context);
    internal bool ChenglieOwnMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().ChenglieOwnMovement(f, m);
    internal IReadOnlyList<CardSnapshot> ChengliePrivateCards(int viewer) => _resolve().ChengliePrivateCards(viewer);
    internal IReadOnlyList<CardSnapshot> ChengliePublicCards() => _resolve().ChengliePublicCards();
    internal bool ChenglieResponded(ChenglieLease r, int holder) => _resolve().ChenglieResponded(r, holder);
    internal PromptChoice ChooseChenglie(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseChenglie(p, f);
    internal void CollectIssuedChenglieCandidates(CardActionContext a, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedChenglieCandidates(a, window, result);
    internal void CompleteChenglieOutcome(ProgramSkillFrame f) => _resolve().CompleteChenglieOutcome(f);
    internal void CompleteChenglieTerminalCleanup(CardUseFrame use) => _resolve().CompleteChenglieTerminalCleanup(use);
    internal void ContinueChenglie(long id) => _resolve().ContinueChenglie(id);
    internal bool ContinuePaidChenglie(ProgramSkillFrame f) => _resolve().ContinuePaidChenglie(f);
    internal bool DrainChenglie(ProgramSkillFrame f) => _resolve().DrainChenglie(f);
    internal void FinishChenglie(ProgramSkillFrame f) => _resolve().FinishChenglie(f);
    internal void FinishChenglieMovement(long id) => _resolve().FinishChenglieMovement(id);
    internal bool HasChenglieResponseObserver(CardActionContext a, ProgramCardContinuation continuation) => _resolve().HasChenglieResponseObserver(a, continuation);
    internal bool HasChenglieTargetTail(CardUseFrame use) => _resolve().HasChenglieTargetTail(use);
    internal bool HasChenglieTargetTail2(CardUseFrame use, CardActionContext action) => _resolve().HasChenglieTargetTail(use, action);
    internal IReadOnlyList<NamedAmbushPileSnapshot>? IncludeChenglieSnapshots(IReadOnlyList<NamedAmbushPileSnapshot>? ordinary, int holder, int viewer, bool revealAll) => _resolve().IncludeChenglieSnapshots(ordinary, holder, viewer, revealAll);
    internal bool IsChenglieAppliedTargetFact(CardUseFrame use, ChenglieTargetsAppliedEvent e, IReadOnlyList<int> precedingTargets, scoped out IReadOnlyList<int> resultTargets) => _resolve().IsChenglieAppliedTargetFact(use, e, precedingTargets, out resultTargets);
    internal bool IsChengliePrivateProcessingCard(int cardId) => _resolve().IsChengliePrivateProcessingCard(cardId);
    internal bool IsIssuedChenglieCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedChenglieCandidate(c, context);
    internal bool IsMaChengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMaChengRecoveryParent(f, recovery);
    internal bool MaChengFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MaChengFirstChild(f, child);
    internal ProgramSkillFrame? MaChengObserverRoot() => _resolve().MaChengObserverRoot();
    internal void PlaceChenglieAttachment(ProgramSkillFrame f, int cardId) => _resolve().PlaceChenglieAttachment(f, cardId);
    internal void PublishChenglie(ProgramSkillFrame f) => _resolve().PublishChenglie(f);
    internal bool QualifyChenglie(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyChenglie(c, t, context);
    internal CardUseFrame RebaseChenglieSlashTargets(CardUseFrame use, IReadOnlyList<int> targets) => _resolve().RebaseChenglieSlashTargets(use, targets);
    internal void RecordChenglieMove(long id, CardMovementRecord m) => _resolve().RecordChenglieMove(id, m);
    internal void RecordChenglieRedirect(ProgramSkillFrame f, long useId, int previousSeat, int nextSeat) => _resolve().RecordChenglieRedirect(f, useId, previousSeat, nextSeat);
    internal void RecordChenglieResponse(IGameEvent fact) => _resolve().RecordChenglieResponse(fact);
    internal CardUseFrame RequireChenglieParent(ProgramSkillFrame f) => _resolve().RequireChenglieParent(f);
    internal bool ResumeChenglie(long id) => _resolve().ResumeChenglie(id);
    internal bool ReturnChenglieMovement(ProgramSkillFrame f) => _resolve().ReturnChenglieMovement(f);
    internal void SettleChenglieLease(ProgramSkillFrame f) => _resolve().SettleChenglieLease(f);
    internal bool ShouldAllowChenglie(ProgramSkillFrame f) => _resolve().ShouldAllowChenglie(f);
    internal void StartChenglieMovement(ref ProgramSkillFrame f, ChenglieStage stage, int participant) => _resolve().StartChenglieMovement(ref f, stage, participant);
    internal bool TryGetChenglieUse(ProgramSkillWindowContext c, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetChenglieUse(c, out use, out window);
    internal bool ValidChenglieAttachment(ChenglieLease r, ChenglieAttachment c) => _resolve().ValidChenglieAttachment(r, c);
    internal IReadOnlyList<Card> WithoutChengliePrivateProcessing(IReadOnlyList<Card> cards) => _resolve().WithoutChengliePrivateProcessing(cards);
    internal bool HasMaChengCapability { get => _resolve().HasMaChengCapability; }
}
