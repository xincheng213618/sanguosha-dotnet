#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundCardsRecastsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundCardsRecast(global::CardGame.Core.ProgramSkillFrame supplied, string sourceBind) => _resolve().BeginBoundCardsRecast(supplied, sourceBind);
    internal bool CanKeepPaidBoundCardsRecast(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanKeepPaidBoundCardsRecast(frame);
    internal void CompleteBoundCardsRecastDraw(global::CardGame.Core.ProgramSkillFrame frame, int actual, long before, long after) => _resolve().CompleteBoundCardsRecastDraw(frame, actual, before, after);
    internal bool DrainBoundCardsRecastChildren(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrainBoundCardsRecastChildren(frame);
    internal bool IsBoundCardsRecastPayment(global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.NativeRecastMaterial material) => _resolve().IsBoundCardsRecastPayment(move, material);
    internal bool ResumeBoundCardsRecast(long frameId) => _resolve().ResumeBoundCardsRecast(frameId);
    internal void ReturnBoundCardsRecastDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnBoundCardsRecastDraw(frame, outcome);
    internal bool ReturnBoundCardsRecastMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnBoundCardsRecastMovement(frame);
    internal bool ValidBoundCardsRecast(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidBoundCardsRecast(frame);
}
