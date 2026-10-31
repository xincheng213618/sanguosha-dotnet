#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouDunBoundaryMobileEntries
{
    internal bool AdvanceBoundaryXiahouDunChildren() => _resolve().AdvanceBoundaryXiahouDunChildren();
    internal global::CardGame.Core.PromptChoice AiBoundaryXiahouDun(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiBoundaryXiahouDun(prompt, f);
    internal void AnswerBoundaryXiahouDun(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerBoundaryXiahouDun(selected);
    internal void AppendBoundaryXiahouDunEnding(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendBoundaryXiahouDunEnding(items);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryXiahouDun(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginBoundaryXiahouDun(supplied, op);
    internal bool BoundaryXiahouDunBeforeDamageObserver(long id) => _resolve().BoundaryXiahouDunBeforeDamageObserver(id);
    internal global::CardGame.Core.PromptChoice BoundaryXiahouDunChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, int value, string text, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().BoundaryXiahouDunChoice(f, branch, value, text, cards, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryXiahouDunChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryXiahouDunChoices(f);
    internal bool BoundaryXiahouDunDamageObserver(long id) => _resolve().BoundaryXiahouDunDamageObserver(id);
    internal global::CardGame.Core.BoundaryXiahouDunStoredEvent BoundaryXiahouDunDeposit(long id) => _resolve().BoundaryXiahouDunDeposit(id);
    internal global::CardGame.Core.BoundaryXiahouDunStoredEvent[] BoundaryXiahouDunDeposits(int owner) => _resolve().BoundaryXiahouDunDeposits(owner);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BoundaryXiahouDunStoredEvent> BoundaryXiahouDunDueDeposits(int owner, long endingId) => _resolve().BoundaryXiahouDunDueDeposits(owner, endingId);
    internal bool BoundaryXiahouDunDyingObserver() => _resolve().BoundaryXiahouDunDyingObserver();
    internal bool BoundaryXiahouDunFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().BoundaryXiahouDunFirstChild(f, child);
    internal global::CardGame.Core.Card BoundaryXiahouDunHandCard(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice c) => _resolve().BoundaryXiahouDunHandCard(f, c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryXiahouDunHandChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryXiahouDunHandChoices(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> BoundaryXiahouDunHeld(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryXiahouDunHeld(f);
    internal bool BoundaryXiahouDunMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().BoundaryXiahouDunMovementProof(f, e, pending);
    internal global::CardGame.Core.ProgramSkillFrame? BoundaryXiahouDunObserverRoot() => _resolve().BoundaryXiahouDunObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PrivateWhirlPileSnapshot>? BoundaryXiahouDunPileViews(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PrivateWhirlPileSnapshot>? existing, int owner, int viewer, bool revealAll) => _resolve().BoundaryXiahouDunPileViews(existing, owner, viewer, revealAll);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> BoundaryXiahouDunPrivateCards(int viewer) => _resolve().BoundaryXiahouDunPrivateCards(viewer);
    internal bool BoundaryXiahouDunRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().BoundaryXiahouDunRecoveryParent(f, child);
    internal bool BoundaryXiahouDunSuffix(int index) => _resolve().BoundaryXiahouDunSuffix(index);
    internal void ContinueBoundaryXiahouDun(long id) => _resolve().ContinueBoundaryXiahouDun(id);
    internal void DiscardBoundaryXiahouDunDeathPiles() => _resolve().DiscardBoundaryXiahouDunDeathPiles();
    internal bool DrainBoundaryXiahouDun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainBoundaryXiahouDun(f);
    internal void FinishBoundaryXiahouDun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishBoundaryXiahouDun(f);
    internal bool IssuedBoundaryXiahouDun(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IssuedBoundaryXiahouDun(c, context);
    internal bool KeepBoundaryXiahouDun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryXiahouDun(f);
    internal bool OfferBoundaryXiahouDun(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().OfferBoundaryXiahouDun(gate);
    internal void PublishBoundaryXiahouDun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBoundaryXiahouDun(f);
    internal bool ResumeBoundaryXiahouDun(long id) => _resolve().ResumeBoundaryXiahouDun(id);
    internal void ReturnBoundaryXiahouDunDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnBoundaryXiahouDunDraw(f, outcome);
    internal bool ReturnBoundaryXiahouDunMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryXiahouDunMovement(f);
    internal void StoreBoundaryXiahouDun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().StoreBoundaryXiahouDun(f);
    internal bool ValidBoundaryXiahouDun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBoundaryXiahouDun(f);
    internal bool ValidBoundaryXiahouDunDeposit(global::CardGame.Core.BoundaryXiahouDunStoredEvent d) => _resolve().ValidBoundaryXiahouDunDeposit(d);
}
