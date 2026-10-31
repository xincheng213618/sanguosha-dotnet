#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangZhaoZhangHongBoundaryMobileEntries
{
    internal void AnswerBoundaryZhangZhaoPhase(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryZhangZhaoPhase(choice);
    internal void AssertBoundaryZhangZhaoPhase(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertBoundaryZhangZhaoPhase(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryZhangZhaoPhase(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginBoundaryZhangZhaoPhase(f);
    internal global::CardGame.Core.PromptChoice BoundaryZhangZhaoPhaseAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryZhangZhaoPhaseAi(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryZhangZhaoPhaseChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryZhangZhaoPhaseChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DiscardRecoveryEntity> BoundaryZhangZhaoPhaseEntities(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().BoundaryZhangZhaoPhaseEntities(phase);
    internal bool BoundaryZhangZhaoPhaseMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? op, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().BoundaryZhangZhaoPhaseMovement(f, op, p);
    internal bool BoundaryZhangZhaoPlacementPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryZhangZhaoPlacementPaid(f);
    internal bool KeepBoundaryZhangZhaoPaidReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryZhangZhaoPaidReturn(f);
    internal bool KeepBoundaryZhangZhaoPlacement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryZhangZhaoPlacement(f);
    internal void ObserveBoundaryZhangZhaoPhaseDiscard(global::CardGame.Core.CardMovedEvent e) => _resolve().ObserveBoundaryZhangZhaoPhaseDiscard(e);
    internal void ObserveBoundaryZhangZhaoPlacement(global::CardGame.Core.CardMovedEvent e) => _resolve().ObserveBoundaryZhangZhaoPlacement(e);
    internal void PublishBoundaryZhangZhaoPhase(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBoundaryZhangZhaoPhase(f);
    internal bool QualifyBoundaryZhangZhaoMobile(global::CardGame.Core.ProgramTriggerCandidate b, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().QualifyBoundaryZhangZhaoMobile(b, c);
    internal bool ResumeBoundaryZhangZhaoPhase(long id) => _resolve().ResumeBoundaryZhangZhaoPhase(id);
    internal bool ReturnBoundaryZhangZhaoPhase(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryZhangZhaoPhase(f);
    internal bool ReturnBoundaryZhangZhaoPlacement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryZhangZhaoPlacement(f);
}
