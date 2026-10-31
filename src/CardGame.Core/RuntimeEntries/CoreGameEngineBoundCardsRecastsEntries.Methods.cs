#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundCardsRecastsEntries
{
    internal SkillProgramStepOutcome BeginBoundCardsRecast(ProgramSkillFrame supplied, string sourceBind) => _resolve().BeginBoundCardsRecast(supplied, sourceBind);
    internal bool CanKeepPaidBoundCardsRecast(ProgramSkillFrame frame) => _resolve().CanKeepPaidBoundCardsRecast(frame);
    internal void CompleteBoundCardsRecastDraw(ProgramSkillFrame frame, int actual, long before, long after) => _resolve().CompleteBoundCardsRecastDraw(frame, actual, before, after);
    internal bool DrainBoundCardsRecastChildren(ProgramSkillFrame frame) => _resolve().DrainBoundCardsRecastChildren(frame);
    internal bool IsBoundCardsRecastPayment(CardMovementRecord move, NativeRecastMaterial material) => _resolve().IsBoundCardsRecastPayment(move, material);
    internal bool ResumeBoundCardsRecast(long frameId) => _resolve().ResumeBoundCardsRecast(frameId);
    internal void ReturnBoundCardsRecastDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome) => _resolve().ReturnBoundCardsRecastDraw(frame, outcome);
    internal bool ReturnBoundCardsRecastMovement(ProgramSkillFrame frame) => _resolve().ReturnBoundCardsRecastMovement(frame);
    internal bool ValidBoundCardsRecast(ProgramSkillFrame frame) => _resolve().ValidBoundCardsRecast(frame);
}
