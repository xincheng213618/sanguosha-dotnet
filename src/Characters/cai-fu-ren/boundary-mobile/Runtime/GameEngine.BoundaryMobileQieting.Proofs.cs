namespace CardGame.Core;

public sealed partial class GameEngine
{
    private void AssertBoundaryMobileQieting(ProgramSkillFrame frame)
    {
        var receipt = frame.OrdinaryQieting!;
        var issued = ProgramEventHistory<OrdinaryQietingOptionIssuedEvent>().Where(entry => entry.FrameId == frame.Id).ToArray();
        if (!receipt.Paid)
        {
            if (receipt.Stage != OrdinaryQietingStage.Choice || receipt.Option is not null || receipt.CardId is not null || receipt.PrintedKind is not null ||
                receipt.MobileSelection is not null || receipt.DrawOutcome is not null || receipt.GrainIds.Count != 0 || issued.Length != 0 || receipt.Before != 0 || receipt.After != 0)
                throw new InvalidOperationException("Unpaid mobile Qieting cannot hold a benefit or private view.");
            return;
        }
        if (issued is not [var option] || option.Option != receipt.Option || option.Before < 0 || option.Before > _movementSequence || receipt.Stage == OrdinaryQietingStage.Choice)
            throw new InvalidOperationException("Mobile Qieting owns exactly one committed option.");
        if (receipt.Option == "draw")
        {
            if (receipt.Stage is not (OrdinaryQietingStage.Draw or OrdinaryQietingStage.DrawChildren) || receipt.CardId is not null || receipt.PrintedKind is not null ||
                receipt.MobileSelection is not null || receipt.GrainIds.Count != 0 || option.CardId is not null || option.PrintedKind is not null || option.Before > receipt.Before)
                throw new InvalidOperationException("Mobile Qieting cannot combine its draw with another branch.");
            if (receipt.Stage == OrdinaryQietingStage.DrawChildren && (receipt.DrawOutcome is not { } draw || draw.RecipientSeat != frame.OwnerSeat || draw.RequestedCount != 1 ||
                draw.SequenceBefore != receipt.Before || draw.SequenceAfter != receipt.After || ProgramEventHistory<OrdinaryQietingDrawReturnedEvent>().Where(entry => entry.FrameId == frame.Id).ToArray() is not [var returned] ||
                returned != new OrdinaryQietingDrawReturnedEvent(frame.Id, draw.ActualDrawIds.Count, draw.ReplacementGainedIds.Count, draw.SequenceBefore, draw.SequenceAfter, draw.AttemptFrameId)))
                throw new InvalidOperationException("Mobile Qieting draw must retain its single typed native return.");
        }
        else if (receipt.Option == "hand")
        {
            if (receipt.MobileSelection is not { } selection || receipt.DrawOutcome is not null || receipt.GrainIds.Count != 0 || option.CardId is not null || option.PrintedKind is not null ||
                receipt.Stage is not (OrdinaryQietingStage.Viewing or OrdinaryQietingStage.MovementChildren) || selection.ReplacedCardId is not null || selection.ReplacedKind is not null ||
                selection.Hand is not System.Collections.IList { IsReadOnly: true } || selection.Viewed is not System.Collections.IList { IsReadOnly: true } || selection.Hand.Count == 0 ||
                selection.Hand.Distinct().Count() != selection.Hand.Count || selection.Viewed.Count != Math.Min(2, selection.Hand.Count) || selection.Viewed.Distinct().Count() != selection.Viewed.Count)
                throw new InvalidOperationException("Mobile Qieting must retain its original frozen at-most-two private hand sample.");
            var random = new DeterministicRandom(unchecked((int)selection.RandomBefore)); var remaining = selection.Hand.ToList(); var expected = new List<int>();
            while (expected.Count < 2 && remaining.Count > 0) { var index = random.Next(remaining.Count); expected.Add(remaining[index]); remaining.RemoveAt(index); }
            if (random.State != selection.RandomAfter || !expected.SequenceEqual(selection.Viewed) || ProgramEventHistory<BoundaryMobileQietingViewedEvent>().Count(entry =>
                entry == new BoundaryMobileQietingViewedEvent(frame.Id, frame.OwnerSeat, receipt.Qualification.TurnOwnerSeat, selection.Viewed.Count)) != 1)
                throw new InvalidOperationException("Mobile Qieting cannot replace or redraw its private sample after a cold return.");
            if (receipt.Stage == OrdinaryQietingStage.Viewing)
            {
                if (receipt.CardId is not null || receipt.PrintedKind is not null || receipt.Before != 0 || receipt.After != 0 || frame.PendingMovementContinuation is not null ||
                    !selection.Hand.SequenceEqual(GetHand(_players[receipt.Qualification.TurnOwnerSeat]).Select(card => card.Id)))
                    throw new InvalidOperationException("Mobile Qieting viewing must retain the unchanged actual source hand.");
            }
            else if (receipt.CardId is not { } taken || !selection.Viewed.Contains(taken) || receipt.PrintedKind != GetAdvancedCard(taken).Kind || option.Before > receipt.Before ||
                _cardMovements.Count(move => move.Sequence > receipt.Before && move.Sequence <= receipt.After && IsBoundaryMobileQietingPaidMovement(frame, move)) != 1)
                throw new InvalidOperationException("Mobile Qieting must obtain exactly one of its originally viewed real entities.");
        }
        else if (receipt.Option == "equipment")
        {
            if (receipt.Stage != OrdinaryQietingStage.MovementChildren || receipt.CardId is not { } id || receipt.PrintedKind is not { } kind || !EquipmentRules.IsEquipment(kind) ||
                receipt.DrawOutcome is not null || receipt.MobileSelection is not { } selection || selection.Hand.Count != 0 || selection.Viewed.Count != 0 || selection.RandomBefore != 0 || selection.RandomAfter != 0 ||
                (selection.ReplacedCardId is null) != (selection.ReplacedKind is null) || option.CardId != id || option.PrintedKind != kind || option.Before != receipt.Before ||
                ProgramEventHistory<OrdinaryQietingEquipmentMovedEvent>().Where(entry => entry.FrameId == frame.Id).ToArray() is not [var moved] || moved.CardId != id || moved.PrintedKind != kind ||
                moved.From != CardLocation.Equipment(receipt.Qualification.TurnOwnerSeat) || moved.To != CardLocation.Equipment(frame.OwnerSeat) ||
                !_cardMovements.Any(move => move.Sequence == moved.MovementSequence && IsBoundaryMobileQietingPaidMovement(frame, move)) ||
                selection.ReplacedCardId is { } replacement && _cardMovements.Count(move => move.Sequence > receipt.Before && move.Sequence <= receipt.After && move.CardId == replacement &&
                    move.CardKind == selection.ReplacedKind && move.From == CardLocation.Equipment(frame.OwnerSeat) && move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.EquipmentReplace) != 1)
                throw new InvalidOperationException("Mobile Qieting lost the exact incoming equipment or its native slot replacement.");
        }
        else throw new InvalidOperationException("Mobile Qieting has only its three printed benefits.");
        if (frame.PendingMovementContinuation is { } movement && !IsOrdinaryCaiFuRenMovement(frame, GetPausedPrivateOfferEffect(frame), movement))
            throw new InvalidOperationException("Mobile Qieting cannot attach an unrelated payment return.");
    }
    private bool IsBoundaryMobileQietingPaidMovement(ProgramSkillFrame frame, CardMovementRecord move)
    {
        var receipt = frame.OrdinaryQieting!;
        if (receipt.Option == "hand") return move.CardId == receipt.CardId && move.CardKind == receipt.PrintedKind && move.From == CardLocation.Hand(receipt.Qualification.TurnOwnerSeat) && move.To == CardLocation.Hand(frame.OwnerSeat) && move.Reason.Value == BoundaryMobileQietingHandReason;
        if (receipt.Option != "equipment") return false;
        return move.CardId == receipt.CardId && move.CardKind == receipt.PrintedKind && move.From == CardLocation.Equipment(receipt.Qualification.TurnOwnerSeat) && move.To == CardLocation.Equipment(frame.OwnerSeat) && move.Reason.Value == OrdinaryQietingMoveReason ||
            receipt.MobileSelection is { ReplacedCardId: { } replaced, ReplacedKind: { } replacedKind } && replaced == move.CardId && replacedKind == move.CardKind && move.From == CardLocation.Equipment(frame.OwnerSeat) && move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.EquipmentReplace ||
            receipt.PrintedKind == CardKind.WoodenOx && receipt.GrainIds.Contains(move.CardId) && move.From == CardLocation.WoodenOxGrain(receipt.Qualification.TurnOwnerSeat) && move.To == CardLocation.WoodenOxGrain(frame.OwnerSeat) && move.Reason == CardMoveReasons.WoodenOxTransfer ||
            receipt.MobileSelection?.ReplacedKind == CardKind.WoodenOx && move.From == CardLocation.WoodenOxGrain(frame.OwnerSeat) && move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.WoodenOxGrainDiscard;
    }
    private bool BoundaryMobileQietingRecoveryMaterial(ProgramSkillFrame frame, int source, int target, int amount, int? cardId, CardMoveReason? reason)
    {
        var receipt = frame.OrdinaryQieting!;
        return receipt is { Stage: OrdinaryQietingStage.MovementChildren, Option: "equipment" } && source == target && amount == 1 &&
            (source == receipt.Qualification.TurnOwnerSeat && receipt.PrintedKind == CardKind.SilverLion && cardId == receipt.CardId && reason?.Value == OrdinaryQietingMoveReason ||
             source == frame.OwnerSeat && receipt.MobileSelection is { ReplacedKind: CardKind.SilverLion, ReplacedCardId: { } replaced } && cardId == replaced && reason == CardMoveReasons.EquipmentReplace);
    }
    private bool IsBoundaryMobileQietingRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) =>
        RecoveryReplacementFrameRidesOn(recovery, frame) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion &&
        BoundaryMobileQietingRecoveryMaterial(frame, recovery.Attempt.SourceSeat, recovery.Attempt.TargetSeat, recovery.Attempt.Amount, recovery.Attempt.Completion.CardId, recovery.Attempt.Completion.MoveReason);
    private bool IsBoundaryMobileQietingHpChild(ProgramSkillFrame frame, HpChangedTriggerWindowFrame hp) => hp.ResumeFrameId == frame.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement &&
        hp.Change.ParentFrameId == frame.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == hp.Change.TargetSeat && frame.OrdinaryQieting is { Option: "equipment", Stage: OrdinaryQietingStage.MovementChildren } receipt &&
        (hp.Change.TargetSeat == receipt.Qualification.TurnOwnerSeat && receipt.PrintedKind == CardKind.SilverLion || hp.Change.TargetSeat == frame.OwnerSeat && receipt.MobileSelection?.ReplacedKind == CardKind.SilverLion);
}
