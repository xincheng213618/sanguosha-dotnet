#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOrderedPrintedSkillLossEntries
{
    internal bool CanContinueOrderedPrintedSkillLoss(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueOrderedPrintedSkillLoss(f);
    internal bool CanOfferOrderedPrintedSkillLoss(int ownerSeat, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CanOfferOrderedPrintedSkillLoss(ownerSeat, trigger, attack);
    internal bool CanOfferOrderedPrintedSkillLoss2(int ownerSeat, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferOrderedPrintedSkillLoss(ownerSeat, trigger, context);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteOrderedPrintedSkillLoss(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ExecuteOrderedPrintedSkillLoss(supplied);
    internal bool IsOrderedPrintedSkillLossMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsOrderedPrintedSkillLossMovement(f, effect, pending);
    internal int? OrderedPrintedLossActualTurnOwner() => _resolve().OrderedPrintedLossActualTurnOwner();
    internal (string SourceId, global::CardGame.Core.GeneralDefinition General) OrderedPrintedLossTemplate(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OrderedPrintedLossTemplate(f);
    internal void RestoreOrderedPrintedSkillsAfterTurnEnded(global::CardGame.Core.TurnEndedEvent ended) => _resolve().RestoreOrderedPrintedSkillsAfterTurnEnded(ended);
    internal void RestoreOrderedPrintedSkillsAtTurnEnd(int turnNumber, int turnOwnerSeat) => _resolve().RestoreOrderedPrintedSkillsAtTurnEnd(turnNumber, turnOwnerSeat);
    internal bool ResumeOrderedPrintedSkillLoss(long id) => _resolve().ResumeOrderedPrintedSkillLoss(id);
    internal bool ReturnOrderedPrintedSkillLossMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnOrderedPrintedSkillLossMovement(f);
    internal long OrderedPrintedLossMovementSequence { get => _resolve().OrderedPrintedLossMovementSequence; }
}
