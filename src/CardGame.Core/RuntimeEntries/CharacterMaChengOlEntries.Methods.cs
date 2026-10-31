#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaChengOlEntries
{
    internal bool AdvanceMaChengSubtree() => _resolve().AdvanceMaChengSubtree();
    internal void AnswerChenglie(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerChenglie(choice);
    internal void ApplyChenglieTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplyChenglieTargets(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginChenglie(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginChenglie(supplied, op);
    internal void BeginChenglieCleanup(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginChenglieCleanup(f);
    internal global::CardGame.Core.PromptChoice ChenglieChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().ChenglieChoice(f, branch, label, cards, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChenglieChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChenglieChoices(f);
    internal bool ChenglieInitialEligible(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ChenglieInitialEligible(candidate, context);
    internal global::System.Collections.Generic.IReadOnlyList<int> ChenglieLegalTargets(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> added) => _resolve().ChenglieLegalTargets(use, added);
    internal global::CardGame.Core.CardUseFrame? ChenglieOutcomeUse(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ChenglieOutcomeUse(c, context);
    internal bool ChenglieOwnMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().ChenglieOwnMovement(f, m);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> ChengliePrivateCards(int viewer) => _resolve().ChengliePrivateCards(viewer);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> ChengliePublicCards() => _resolve().ChengliePublicCards();
    internal bool ChenglieResponded(global::CardGame.Core.ChenglieLease r, int holder) => _resolve().ChenglieResponded(r, holder);
    internal global::CardGame.Core.PromptChoice ChooseChenglie(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseChenglie(p, f);
    internal void CollectIssuedChenglieCandidates(global::CardGame.Core.CardActionContext a, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedChenglieCandidates(a, window, result);
    internal void CompleteChenglieOutcome(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteChenglieOutcome(f);
    internal void CompleteChenglieTerminalCleanup(global::CardGame.Core.CardUseFrame use) => _resolve().CompleteChenglieTerminalCleanup(use);
    internal void ContinueChenglie(long id) => _resolve().ContinueChenglie(id);
    internal bool ContinuePaidChenglie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidChenglie(f);
    internal bool DrainChenglie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainChenglie(f);
    internal void FinishChenglie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishChenglie(f);
    internal void FinishChenglieMovement(long id) => _resolve().FinishChenglieMovement(id);
    internal bool HasChenglieResponseObserver(global::CardGame.Core.CardActionContext a, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().HasChenglieResponseObserver(a, continuation);
    internal bool HasChenglieTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasChenglieTargetTail(use);
    internal bool HasChenglieTargetTail2(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action) => _resolve().HasChenglieTargetTail(use, action);
    internal bool HasMaChengBeforeDamageObserver(long id) => _resolve().HasMaChengBeforeDamageObserver(id);
    internal bool HasMaChengDamageObserver(long id) => _resolve().HasMaChengDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NamedAmbushPileSnapshot>? IncludeChenglieSnapshots(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NamedAmbushPileSnapshot>? ordinary, int holder, int viewer, bool revealAll) => _resolve().IncludeChenglieSnapshots(ordinary, holder, viewer, revealAll);
    internal bool IsChenglieAppliedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ChenglieTargetsAppliedEvent e, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, scoped out global::System.Collections.Generic.IReadOnlyList<int> resultTargets) => _resolve().IsChenglieAppliedTargetFact(use, e, precedingTargets, out resultTargets);
    internal bool IsChengliePrivateProcessingCard(int cardId) => _resolve().IsChengliePrivateProcessingCard(cardId);
    internal bool IsIssuedChenglieCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedChenglieCandidate(c, context);
    internal bool IsMaChengMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsMaChengMovement(f, e, pending);
    internal bool IsMaChengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMaChengRecoveryParent(f, recovery);
    internal bool MaChengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MaChengFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MaChengObserverRoot() => _resolve().MaChengObserverRoot();
    internal void PlaceChenglieAttachment(global::CardGame.Core.ProgramSkillFrame f, int cardId) => _resolve().PlaceChenglieAttachment(f, cardId);
    internal void PublishChenglie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishChenglie(f);
    internal bool QualifyChenglie(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyChenglie(c, t, context);
    internal global::CardGame.Core.CardUseFrame RebaseChenglieSlashTargets(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().RebaseChenglieSlashTargets(use, targets);
    internal void RecordChenglieMove(long id, global::CardGame.Core.CardMovementRecord m) => _resolve().RecordChenglieMove(id, m);
    internal void RecordChenglieRedirect(global::CardGame.Core.ProgramSkillFrame f, long useId, int previousSeat, int nextSeat) => _resolve().RecordChenglieRedirect(f, useId, previousSeat, nextSeat);
    internal void RecordChenglieResponse(global::CardGame.Core.IGameEvent fact) => _resolve().RecordChenglieResponse(fact);
    internal global::CardGame.Core.CardUseFrame RequireChenglieParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireChenglieParent(f);
    internal bool ResumeChenglie(long id) => _resolve().ResumeChenglie(id);
    internal bool ReturnChenglieMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnChenglieMovement(f);
    internal void SettleChenglieLease(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleChenglieLease(f);
    internal bool ShouldAllowChenglie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShouldAllowChenglie(f);
    internal void StartChenglieMovement(ref global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ChenglieStage stage, int participant) => _resolve().StartChenglieMovement(ref f, stage, participant);
    internal bool TryGetChenglieUse(global::CardGame.Core.ProgramSkillWindowContext c, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetChenglieUse(c, out use, out window);
    internal bool ValidChenglieAttachment(global::CardGame.Core.ChenglieLease r, global::CardGame.Core.ChenglieAttachment c) => _resolve().ValidChenglieAttachment(r, c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> WithoutChengliePrivateProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().WithoutChengliePrivateProcessing(cards);
    internal bool HasMaChengCapability { get => _resolve().HasMaChengCapability; }
}
