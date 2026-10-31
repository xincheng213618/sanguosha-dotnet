#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangNiBoundaryMobileEntries
{
    internal bool AdmitBoundaryWurongSkip(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AdmitBoundaryWurongSkip(candidate, context);
    internal bool AdvanceBoundaryWurongObservers() => _resolve().AdvanceBoundaryWurongObservers();
    internal void AnswerBoundaryWurong(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryWurong(choice);
    internal void AssertBoundaryWurong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertBoundaryWurong(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryWurong(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect e) => _resolve().BeginBoundaryWurong(supplied, e);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryWurongGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginBoundaryWurongGift(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryWurongChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryWurongChoices(f);
    internal bool BoundaryWurongDamageObserver(long id) => _resolve().BoundaryWurongDamageObserver(id);
    internal bool BoundaryWurongDyingObserver() => _resolve().BoundaryWurongDyingObserver();
    internal bool BoundaryWurongFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().BoundaryWurongFirstChild(f, child);
    internal bool BoundaryWurongGiftMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().BoundaryWurongGiftMovement(f, e, p);
    internal bool BoundaryWurongGiftRecovery(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().BoundaryWurongGiftRecovery(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? BoundaryWurongObserverRoot() => _resolve().BoundaryWurongObserverRoot();
    internal global::CardGame.Core.SkillProgramStepOutcome ConsumeBoundaryWurongSkip(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ConsumeBoundaryWurongSkip(f);
    internal void ContinueBoundaryWurongGift(long id) => _resolve().ContinueBoundaryWurongGift(id);
    internal bool HasPendingBoundaryWurongDrawSkip(int target) => _resolve().HasPendingBoundaryWurongDrawSkip(target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeBoundaryWurongSkip(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> original) => _resolve().IncludeBoundaryWurongSkip(owner, window, original);
    internal bool IsBoundaryWurongChoiceResult(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramChoiceResultBinding b) => _resolve().IsBoundaryWurongChoiceResult(f, b);
    internal bool KeepBoundaryWurongPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryWurongPaid(f);
    internal void PayBoundaryWurongGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayBoundaryWurongGift(f);
    internal global::CardGame.Core.BoundaryWurongSkipIssuedEvent[] PendingBoundaryWurongSkips(int target) => _resolve().PendingBoundaryWurongSkips(target);
    internal void PublishBoundaryWurong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBoundaryWurong(f);
    internal bool ResumeBoundaryWurong(long id) => _resolve().ResumeBoundaryWurong(id);
    internal bool ReturnBoundaryWurongMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryWurongMovement(f);
}
