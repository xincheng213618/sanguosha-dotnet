#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuHuanBoundaryMobileEntries
{
    internal void AnswerBoundaryPingkou(PromptChoice answer) => _resolve().AnswerBoundaryPingkou(answer);
    internal SkillProgramStepOutcome BeginBoundaryPingkou(ProgramSkillFrame supplied, int maximum) => _resolve().BeginBoundaryPingkou(supplied, maximum);
    internal PromptChoice BoundaryPingkouAi(PendingDecision pending, ProgramSkillFrame frame) => _resolve().BoundaryPingkouAi(pending, frame);
    internal IReadOnlyList<PromptChoice> BoundaryPingkouChoices(ProgramSkillFrame frame) => _resolve().BoundaryPingkouChoices(frame);
    internal bool CanOfferBoundaryZhuHuan(ProgramTriggerLegalityContext gate) => _resolve().CanOfferBoundaryZhuHuan(gate);
    internal void CompleteBoundaryPingkou(ProgramSkillFrame frame) => _resolve().CompleteBoundaryPingkou(frame);
    internal bool ContinueBoundaryPingkou(long id) => _resolve().ContinueBoundaryPingkou(id);
    internal void IssueBoundaryPingkouDamage(ProgramSkillFrame frame) => _resolve().IssueBoundaryPingkouDamage(frame);
    internal bool KeepIssuedBoundaryPingkou(ProgramSkillFrame frame) => _resolve().KeepIssuedBoundaryPingkou(frame);
    internal void ObtainBoundaryPingkouEquipment(ProgramSkillFrame frame) => _resolve().ObtainBoundaryPingkouEquipment(frame);
    internal void PublishBoundaryPingkou(ProgramSkillFrame frame) => _resolve().PublishBoundaryPingkou(frame);
    internal bool ReturnBoundaryPingkouMovement(ProgramSkillFrame frame) => _resolve().ReturnBoundaryPingkouMovement(frame);
    internal void SkipBoundaryZhuHuanDraw(ProgramSkillFrame frame) => _resolve().SkipBoundaryZhuHuanDraw(frame);
    internal bool ValidBoundaryPingkou(ProgramSkillFrame frame) => _resolve().ValidBoundaryPingkou(frame);
}
