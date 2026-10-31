#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOrderedPrintedSkillLossEntries
{
    internal bool CanContinueOrderedPrintedSkillLoss(ProgramSkillFrame f) => _resolve().CanContinueOrderedPrintedSkillLoss(f);
    internal bool CanOfferOrderedPrintedSkillLoss(int ownerSeat, SkillProgramTrigger trigger, GameEngine.IDamageAttempt attack) => _resolve().CanOfferOrderedPrintedSkillLoss(ownerSeat, trigger, attack);
    internal bool CanOfferOrderedPrintedSkillLoss2(int ownerSeat, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferOrderedPrintedSkillLoss(ownerSeat, trigger, context);
    internal SkillProgramStepOutcome ExecuteOrderedPrintedSkillLoss(ProgramSkillFrame supplied) => _resolve().ExecuteOrderedPrintedSkillLoss(supplied);
    internal int? OrderedPrintedLossActualTurnOwner() => _resolve().OrderedPrintedLossActualTurnOwner();
    internal (string SourceId, GeneralDefinition General) OrderedPrintedLossTemplate(ProgramSkillFrame f) => _resolve().OrderedPrintedLossTemplate(f);
    internal void RestoreOrderedPrintedSkillsAfterTurnEnded(TurnEndedEvent ended) => _resolve().RestoreOrderedPrintedSkillsAfterTurnEnded(ended);
    internal void RestoreOrderedPrintedSkillsAtTurnEnd(int turnNumber, int turnOwnerSeat) => _resolve().RestoreOrderedPrintedSkillsAtTurnEnd(turnNumber, turnOwnerSeat);
    internal bool ResumeOrderedPrintedSkillLoss(long id) => _resolve().ResumeOrderedPrintedSkillLoss(id);
    internal bool ReturnOrderedPrintedSkillLossMovement(ProgramSkillFrame f) => _resolve().ReturnOrderedPrintedSkillLossMovement(f);
    internal long OrderedPrintedLossMovementSequence { get => _resolve().OrderedPrintedLossMovementSequence; }
}
