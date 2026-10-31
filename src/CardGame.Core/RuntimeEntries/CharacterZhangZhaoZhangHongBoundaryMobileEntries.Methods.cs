#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangZhaoZhangHongBoundaryMobileEntries
{
    internal void AnswerBoundaryZhangZhaoPhase(PromptChoice choice) => _resolve().AnswerBoundaryZhangZhaoPhase(choice);
    internal SkillProgramStepOutcome BeginBoundaryZhangZhaoPhase(ProgramSkillFrame f) => _resolve().BeginBoundaryZhangZhaoPhase(f);
    internal PromptChoice BoundaryZhangZhaoPhaseAi(PendingDecision p, ProgramSkillFrame f) => _resolve().BoundaryZhangZhaoPhaseAi(p, f);
    internal IReadOnlyList<PromptChoice> BoundaryZhangZhaoPhaseChoices(ProgramSkillFrame f) => _resolve().BoundaryZhangZhaoPhaseChoices(f);
    internal IReadOnlyList<DiscardRecoveryEntity> BoundaryZhangZhaoPhaseEntities(ActualDiscardRecoveryPhaseKey phase) => _resolve().BoundaryZhangZhaoPhaseEntities(phase);
    internal bool BoundaryZhangZhaoPlacementPaid(ProgramSkillFrame f) => _resolve().BoundaryZhangZhaoPlacementPaid(f);
    internal bool KeepBoundaryZhangZhaoPaidReturn(ProgramSkillFrame f) => _resolve().KeepBoundaryZhangZhaoPaidReturn(f);
    internal bool KeepBoundaryZhangZhaoPlacement(ProgramSkillFrame f) => _resolve().KeepBoundaryZhangZhaoPlacement(f);
    internal void ObserveBoundaryZhangZhaoPhaseDiscard(CardMovedEvent e) => _resolve().ObserveBoundaryZhangZhaoPhaseDiscard(e);
    internal void ObserveBoundaryZhangZhaoPlacement(CardMovedEvent e) => _resolve().ObserveBoundaryZhangZhaoPlacement(e);
    internal void PublishBoundaryZhangZhaoPhase(ProgramSkillFrame f) => _resolve().PublishBoundaryZhangZhaoPhase(f);
    internal bool QualifyBoundaryZhangZhaoMobile(ProgramTriggerCandidate b, ProgramSkillWindowContext c) => _resolve().QualifyBoundaryZhangZhaoMobile(b, c);
    internal bool ResumeBoundaryZhangZhaoPhase(long id) => _resolve().ResumeBoundaryZhangZhaoPhase(id);
    internal bool ReturnBoundaryZhangZhaoPhase(ProgramSkillFrame f) => _resolve().ReturnBoundaryZhangZhaoPhase(f);
    internal bool ReturnBoundaryZhangZhaoPlacement(ProgramSkillFrame f) => _resolve().ReturnBoundaryZhangZhaoPlacement(f);
}
