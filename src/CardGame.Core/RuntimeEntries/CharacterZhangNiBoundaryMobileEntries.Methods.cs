#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangNiBoundaryMobileEntries
{
    internal bool AdmitBoundaryWurongSkip(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().AdmitBoundaryWurongSkip(candidate, context);
    internal bool AdvanceBoundaryWurongObservers() => _resolve().AdvanceBoundaryWurongObservers();
    internal void AnswerBoundaryWurong(PromptChoice choice) => _resolve().AnswerBoundaryWurong(choice);
    internal SkillProgramStepOutcome BeginBoundaryWurong(ProgramSkillFrame supplied, SkillProgramEffect e) => _resolve().BeginBoundaryWurong(supplied, e);
    internal SkillProgramStepOutcome BeginBoundaryWurongGift(ProgramSkillFrame f) => _resolve().BeginBoundaryWurongGift(f);
    internal IReadOnlyList<PromptChoice> BoundaryWurongChoices(ProgramSkillFrame f) => _resolve().BoundaryWurongChoices(f);
    internal bool BoundaryWurongDyingObserver() => _resolve().BoundaryWurongDyingObserver();
    internal bool BoundaryWurongFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().BoundaryWurongFirstChild(f, child);
    internal bool BoundaryWurongGiftRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().BoundaryWurongGiftRecovery(f, recovery);
    internal ProgramSkillFrame? BoundaryWurongObserverRoot() => _resolve().BoundaryWurongObserverRoot();
    internal SkillProgramStepOutcome ConsumeBoundaryWurongSkip(ProgramSkillFrame f) => _resolve().ConsumeBoundaryWurongSkip(f);
    internal void ContinueBoundaryWurongGift(long id) => _resolve().ContinueBoundaryWurongGift(id);
    internal bool HasPendingBoundaryWurongDrawSkip(int target) => _resolve().HasPendingBoundaryWurongDrawSkip(target);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeBoundaryWurongSkip(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> original) => _resolve().IncludeBoundaryWurongSkip(owner, window, original);
    internal bool KeepBoundaryWurongPaid(ProgramSkillFrame f) => _resolve().KeepBoundaryWurongPaid(f);
    internal void PayBoundaryWurongGift(ProgramSkillFrame f) => _resolve().PayBoundaryWurongGift(f);
    internal BoundaryWurongSkipIssuedEvent[] PendingBoundaryWurongSkips(int target) => _resolve().PendingBoundaryWurongSkips(target);
    internal void PublishBoundaryWurong(ProgramSkillFrame f) => _resolve().PublishBoundaryWurong(f);
    internal bool ResumeBoundaryWurong(long id) => _resolve().ResumeBoundaryWurong(id);
    internal bool ReturnBoundaryWurongMovement(ProgramSkillFrame f) => _resolve().ReturnBoundaryWurongMovement(f);
}
