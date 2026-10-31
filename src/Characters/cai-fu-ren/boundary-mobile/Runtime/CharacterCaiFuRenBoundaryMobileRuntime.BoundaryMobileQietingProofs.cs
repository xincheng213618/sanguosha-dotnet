#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiFuRenBoundaryMobileRuntime
{

    internal bool IsBoundaryMobileQietingPaidMovement(ProgramSkillFrame frame, CardMovementRecord move)
    {
        var receipt = frame.OrdinaryQieting!;
        if (receipt.Option == "hand")
            return move.CardId == receipt.CardId && move.CardKind == receipt.PrintedKind && move.From == CardLocation.Hand(receipt.Qualification.TurnOwnerSeat) && move.To == CardLocation.Hand(frame.OwnerSeat) && move.Reason.Value == BoundaryMobileQietingHandReason;
        if (receipt.Option != "equipment")
            return false;
        return move.CardId == receipt.CardId && move.CardKind == receipt.PrintedKind && move.From == CardLocation.Equipment(receipt.Qualification.TurnOwnerSeat) && move.To == CardLocation.Equipment(frame.OwnerSeat) && move.Reason.Value == "skill-program.cai-fu-ren.qieting.move" || receipt.MobileSelection is { ReplacedCardId: { } replaced, ReplacedKind: { } replacedKind } && replaced == move.CardId && replacedKind == move.CardKind && move.From == CardLocation.Equipment(frame.OwnerSeat) && move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.EquipmentReplace || receipt.PrintedKind == CardKind.WoodenOx && receipt.GrainIds.Contains(move.CardId) && move.From == CardLocation.WoodenOxGrain(receipt.Qualification.TurnOwnerSeat) && move.To == CardLocation.WoodenOxGrain(frame.OwnerSeat) && move.Reason == CardMoveReasons.WoodenOxTransfer || receipt.MobileSelection?.ReplacedKind == CardKind.WoodenOx && move.From == CardLocation.WoodenOxGrain(frame.OwnerSeat) && move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.WoodenOxGrainDiscard;
    }

    internal bool BoundaryMobileQietingRecoveryMaterial(ProgramSkillFrame frame, int source, int target, int amount, int? cardId, CardMoveReason? reason)
    {
        var receipt = frame.OrdinaryQieting!;
        return receipt is { Stage: OrdinaryQietingStage.MovementChildren, Option: "equipment" } && source == target && amount == 1 && (source == receipt.Qualification.TurnOwnerSeat && receipt.PrintedKind == CardKind.SilverLion && cardId == receipt.CardId && reason?.Value == "skill-program.cai-fu-ren.qieting.move" || source == frame.OwnerSeat && receipt.MobileSelection is { ReplacedKind: CardKind.SilverLion, ReplacedCardId: { } replaced } && cardId == replaced && reason == CardMoveReasons.EquipmentReplace);
    }

    internal bool IsBoundaryMobileQietingRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, frame) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && BoundaryMobileQietingRecoveryMaterial(frame, recovery.Attempt.SourceSeat, recovery.Attempt.TargetSeat, recovery.Attempt.Amount, recovery.Attempt.Completion.CardId, recovery.Attempt.Completion.MoveReason);
    internal bool IsBoundaryMobileQietingHpChild(ProgramSkillFrame frame, HpChangedTriggerWindowFrame hp) => hp.ResumeFrameId == frame.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.ParentFrameId == frame.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == hp.Change.TargetSeat && frame.OrdinaryQieting is { Option: "equipment", Stage: OrdinaryQietingStage.MovementChildren } receipt && (hp.Change.TargetSeat == receipt.Qualification.TurnOwnerSeat && receipt.PrintedKind == CardKind.SilverLion || hp.Change.TargetSeat == frame.OwnerSeat && receipt.MobileSelection?.ReplacedKind == CardKind.SilverLion);
}
