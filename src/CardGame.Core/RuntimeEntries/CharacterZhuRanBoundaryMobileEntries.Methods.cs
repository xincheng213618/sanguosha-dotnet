#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRanBoundaryMobileEntries
{
    internal bool AdvanceBoundaryDanshouObservers() => _resolve().AdvanceBoundaryDanshouObservers();
    internal void AnswerBoundaryDanshou(PromptChoice choice) => _resolve().AnswerBoundaryDanshou(choice);
    internal IReadOnlyList<PromptChoice> BoundaryDanshouChoices(ProgramSkillFrame f) => _resolve().BoundaryDanshouChoices(f);
    internal int BoundaryDanshouCount(int owner, int actor, int turn, int turnOwner, long cutoff) => _resolve().BoundaryDanshouCount(owner, actor, turn, turnOwner, cutoff);
    internal bool BoundaryDanshouDyingObserver() => _resolve().BoundaryDanshouDyingObserver();
    internal bool BoundaryDanshouFinalizedTargetWindows(ProgramTargetCardWindowQuery query) => _resolve().BoundaryDanshouFinalizedTargetWindows(query);
    internal bool BoundaryDanshouFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().BoundaryDanshouFirstChild(f, child);
    internal Card[] BoundaryDanshouMaterials(int owner) => _resolve().BoundaryDanshouMaterials(owner);
    internal TurnEndingBoundaryFrame? BoundaryDanshouMother(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().BoundaryDanshouMother(c, context);
    internal ProgramSkillFrame? BoundaryDanshouObserverRoot() => _resolve().BoundaryDanshouObserverRoot();
    internal bool BoundaryDanshouRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().BoundaryDanshouRecovery(f, recovery);
    internal void CaptureBoundaryDanshouUse(CardUseFrame use) => _resolve().CaptureBoundaryDanshouUse(use);
    internal void ContinueBoundaryDanshou(long id) => _resolve().ContinueBoundaryDanshou(id);
    internal bool KeepBoundaryDanshouPaid(ProgramSkillFrame f) => _resolve().KeepBoundaryDanshouPaid(f);
    internal void ObserveBoundaryDanshouHistory(IGameEvent payload) => _resolve().ObserveBoundaryDanshouHistory(payload);
    internal void PayBoundaryDanshou(ProgramSkillFrame f) => _resolve().PayBoundaryDanshou(f);
    internal SkillProgramStepOutcome PrepareBoundaryDanshou(ProgramSkillFrame supplied) => _resolve().PrepareBoundaryDanshou(supplied);
    internal void PublishBoundaryDanshou(ProgramSkillFrame f) => _resolve().PublishBoundaryDanshou(f);
    internal bool QualifyBoundaryDanshou(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().QualifyBoundaryDanshou(c, context);
    internal bool ResumeBoundaryDanshou(long id) => _resolve().ResumeBoundaryDanshou(id);
    internal bool ReturnBoundaryDanshouMovement(ProgramSkillFrame f) => _resolve().ReturnBoundaryDanshouMovement(f);
    internal bool SealBoundaryDanshouCompletedUse(CardUseFrame use) => _resolve().SealBoundaryDanshouCompletedUse(use);
    internal bool HasBoundaryDanshou { get => _resolve().HasBoundaryDanshou; }
}
