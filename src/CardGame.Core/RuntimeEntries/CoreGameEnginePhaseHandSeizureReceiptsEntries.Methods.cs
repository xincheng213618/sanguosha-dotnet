#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseHandSeizureReceiptsEntries
{
    internal void AssertPhaseHandSeizure(ProgramSkillFrame frame) => _resolve().AssertPhaseHandSeizure(frame);
    internal bool CanContinuePhaseHandSeizure(ProgramSkillFrame frame) => _resolve().CanContinuePhaseHandSeizure(frame);
    internal bool IsPhaseHandDebtChoiceLegal(PromptChoice choice) => _resolve().IsPhaseHandDebtChoiceLegal(choice);
    internal bool IsPhaseHandSeizureMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsPhaseHandSeizureMovement(frame, effect, pending);
    internal bool ValidPhaseHandDebtReturnReceipt(ProgramSkillFrame frame) => _resolve().ValidPhaseHandDebtReturnReceipt(frame);
    internal bool ValidPhaseHandSeizureReceipt(ProgramSkillFrame frame) => _resolve().ValidPhaseHandSeizureReceipt(frame);
}
