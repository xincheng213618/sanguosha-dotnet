#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuHuanBoundaryMobileEntries
{
    internal void AnswerBoundaryPingkou(global::CardGame.Core.PromptChoice answer) => _resolve().AnswerBoundaryPingkou(answer);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryPingkou(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginBoundaryPingkou(supplied);
    internal global::CardGame.Core.PromptChoice BoundaryPingkouAi(global::CardGame.Core.PendingDecision pending, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryPingkouAi(pending, frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryPingkouChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryPingkouChoices(frame);
    internal int BoundaryPingkouMaximum(int owner) => _resolve().BoundaryPingkouMaximum(owner);
    internal bool CanOfferBoundaryZhuHuan(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferBoundaryZhuHuan(gate);
    internal void CompleteBoundaryPingkou(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompleteBoundaryPingkou(frame);
    internal bool ContinueBoundaryPingkou(long id) => _resolve().ContinueBoundaryPingkou(id);
    internal void IssueBoundaryPingkouDamage(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IssueBoundaryPingkouDamage(frame);
    internal bool KeepIssuedBoundaryPingkou(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepIssuedBoundaryPingkou(frame);
    internal void ObserveBoundaryZhuHuanSkippedDraw(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveBoundaryZhuHuanSkippedDraw(payload);
    internal bool ObserveBoundaryZhuHuanSkippedPhase(global::CardGame.Core.ProgramActualPhaseSkipContext context) => _resolve().ObserveBoundaryZhuHuanSkippedPhase(context);
    internal void ObtainBoundaryPingkouEquipment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ObtainBoundaryPingkouEquipment(frame);
    internal void PublishBoundaryPingkou(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishBoundaryPingkou(frame);
    internal void RecordBoundaryZhuHuanSkippedPhase(int owner, global::CardGame.Core.SkillProgramTurnPhase phase, long extraDrawFrameId = 0L) => _resolve().RecordBoundaryZhuHuanSkippedPhase(owner, phase, extraDrawFrameId);
    internal bool ReturnBoundaryPingkouMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnBoundaryPingkouMovement(frame);
    internal void SkipBoundaryZhuHuanDraw(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SkipBoundaryZhuHuanDraw(frame);
    internal bool ValidBoundaryPingkou(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidBoundaryPingkou(frame);
    internal bool TracksBoundaryZhuHuanPhaseSkips { get => _resolve().TracksBoundaryZhuHuanPhaseSkips; }
}
