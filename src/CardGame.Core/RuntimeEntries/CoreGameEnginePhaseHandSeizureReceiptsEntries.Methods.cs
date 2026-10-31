#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseHandSeizureReceiptsEntries
{
    internal void AssertPhaseHandSeizure(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertPhaseHandSeizure(frame);
    internal bool CanContinuePhaseHandSeizure(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinuePhaseHandSeizure(frame);
    internal bool IsPhaseHandDebtChoiceLegal(global::CardGame.Core.PromptChoice choice) => _resolve().IsPhaseHandDebtChoiceLegal(choice);
    internal bool IsPhaseHandSeizureMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsPhaseHandSeizureMovement(frame, effect, pending);
    internal bool ValidPhaseHandDebtReturnReceipt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidPhaseHandDebtReturnReceipt(frame);
    internal bool ValidPhaseHandSeizureReceipt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidPhaseHandSeizureReceipt(frame);
}
