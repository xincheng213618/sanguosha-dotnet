#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouDunBoundaryMobileEntries
{
    internal bool AdvanceBoundaryXiahouDunChildren() => _resolve().AdvanceBoundaryXiahouDunChildren();
    internal PromptChoice AiBoundaryXiahouDun(PendingDecision prompt, ProgramSkillFrame f) => _resolve().AiBoundaryXiahouDun(prompt, f);
    internal void AnswerBoundaryXiahouDun(PromptChoice selected) => _resolve().AnswerBoundaryXiahouDun(selected);
    internal void AppendBoundaryXiahouDunEnding(List<TurnEndingBoundaryItem> items) => _resolve().AppendBoundaryXiahouDunEnding(items);
    internal SkillProgramStepOutcome BeginBoundaryXiahouDun(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginBoundaryXiahouDun(supplied, op);
    internal PromptChoice BoundaryXiahouDunChoice(ProgramSkillFrame f, string branch, int value, string text, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().BoundaryXiahouDunChoice(f, branch, value, text, cards, targets);
    internal IReadOnlyList<PromptChoice> BoundaryXiahouDunChoices(ProgramSkillFrame f) => _resolve().BoundaryXiahouDunChoices(f);
    internal BoundaryXiahouDunStoredEvent BoundaryXiahouDunDeposit(long id) => _resolve().BoundaryXiahouDunDeposit(id);
    internal BoundaryXiahouDunStoredEvent[] BoundaryXiahouDunDeposits(int owner) => _resolve().BoundaryXiahouDunDeposits(owner);
    internal IEnumerable<BoundaryXiahouDunStoredEvent> BoundaryXiahouDunDueDeposits(int owner, long endingId) => _resolve().BoundaryXiahouDunDueDeposits(owner, endingId);
    internal bool BoundaryXiahouDunDyingObserver() => _resolve().BoundaryXiahouDunDyingObserver();
    internal bool BoundaryXiahouDunFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().BoundaryXiahouDunFirstChild(f, child);
    internal Card BoundaryXiahouDunHandCard(ProgramSkillFrame f, PromptChoice c) => _resolve().BoundaryXiahouDunHandCard(f, c);
    internal IReadOnlyList<PromptChoice> BoundaryXiahouDunHandChoices(ProgramSkillFrame f) => _resolve().BoundaryXiahouDunHandChoices(f);
    internal IEnumerable<Card> BoundaryXiahouDunHeld(ProgramSkillFrame f) => _resolve().BoundaryXiahouDunHeld(f);
    internal ProgramSkillFrame? BoundaryXiahouDunObserverRoot() => _resolve().BoundaryXiahouDunObserverRoot();
    internal IReadOnlyList<PrivateWhirlPileSnapshot>? BoundaryXiahouDunPileViews(IReadOnlyList<PrivateWhirlPileSnapshot>? existing, int owner, int viewer, bool revealAll) => _resolve().BoundaryXiahouDunPileViews(existing, owner, viewer, revealAll);
    internal IEnumerable<CardSnapshot> BoundaryXiahouDunPrivateCards(int viewer) => _resolve().BoundaryXiahouDunPrivateCards(viewer);
    internal bool BoundaryXiahouDunRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child) => _resolve().BoundaryXiahouDunRecoveryParent(f, child);
    internal bool BoundaryXiahouDunSuffix(int index) => _resolve().BoundaryXiahouDunSuffix(index);
    internal void ContinueBoundaryXiahouDun(long id) => _resolve().ContinueBoundaryXiahouDun(id);
    internal void DiscardBoundaryXiahouDunDeathPiles() => _resolve().DiscardBoundaryXiahouDunDeathPiles();
    internal bool DrainBoundaryXiahouDun(ProgramSkillFrame f) => _resolve().DrainBoundaryXiahouDun(f);
    internal void FinishBoundaryXiahouDun(ProgramSkillFrame f) => _resolve().FinishBoundaryXiahouDun(f);
    internal bool IssuedBoundaryXiahouDun(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IssuedBoundaryXiahouDun(c, context);
    internal bool KeepBoundaryXiahouDun(ProgramSkillFrame f) => _resolve().KeepBoundaryXiahouDun(f);
    internal bool OfferBoundaryXiahouDun(ProgramTriggerLegalityContext gate) => _resolve().OfferBoundaryXiahouDun(gate);
    internal void PublishBoundaryXiahouDun(ProgramSkillFrame f) => _resolve().PublishBoundaryXiahouDun(f);
    internal bool ResumeBoundaryXiahouDun(long id) => _resolve().ResumeBoundaryXiahouDun(id);
    internal void ReturnBoundaryXiahouDunDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnBoundaryXiahouDunDraw(f, outcome);
    internal bool ReturnBoundaryXiahouDunMovement(ProgramSkillFrame f) => _resolve().ReturnBoundaryXiahouDunMovement(f);
    internal void StoreBoundaryXiahouDun(ProgramSkillFrame f) => _resolve().StoreBoundaryXiahouDun(f);
    internal bool ValidBoundaryXiahouDun(ProgramSkillFrame f) => _resolve().ValidBoundaryXiahouDun(f);
    internal bool ValidBoundaryXiahouDunDeposit(BoundaryXiahouDunStoredEvent d) => _resolve().ValidBoundaryXiahouDunDeposit(d);
}
