#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePairObtainFixedRecipientReceiptsRuntime
{
    internal bool ValidPairObtain(ProgramSkillFrame f)
    {
        if (f.PairObtain is not { } d || d.InstructionIndex != 0 || f.InstructionIndex is < 1 or > 4 || f.TriggerId is not null || f.WindowContext is not null || d.Source != _corePairObtainShownGiftsOperations.PairBenefitSource(f) || d.GameplayHash != f.GameplayHash || d.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || d.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || d.ActualTurnOwnerSeat != f.OwnerSeat || d.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || d.Cursor is < 0 or > 2 || d.FirstSeat == d.SecondSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(d.FirstSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(d.SecondSeat) || !f.SelectedTargetSeats.SequenceEqual([d.FirstSeat, d.SecondSeat]) || d.Cursor == 2 && d.AwaitingMovement || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.ObtainOneFromEachSelectedTarget }, { Op: SkillProgramEffectOp.SelectOwnedCards }, { Op: SkillProgramEffectOp.RevealBoundCards }, { Op: SkillProgramEffectOp.GiveShownCardToLeastOriginalTarget }])
            return false;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PairObtainStartedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Source == d.Source && e.GameplayHash == d.GameplayHash && e.ActualTurnNumber == d.ActualTurnNumber && e.ActualTurnOwnerSeat == d.ActualTurnOwnerSeat && e.PhaseInstanceId == d.PhaseInstanceId && e.FirstSeat == d.FirstSeat && e.SecondSeat == d.SecondSeat) != 1)
            return false;
        bool Paid(ProgramPairObtainPayment? p, int cursor, int target)
        {
            if (p is null)
                return d.Cursor <= cursor && !(d.Cursor == cursor && d.AwaitingMovement);
            if (p.Cursor != cursor || p.RecipientSeat != f.OwnerSeat || p.From.OwnerSeat != target || p.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment) || p.SequenceBefore < 0 || p.SequenceAfter < p.SequenceBefore || p.SameHand != (p.From == CardLocation.Hand(f.OwnerSeat)) || p.SameHand && (p.SequenceAfter != p.SequenceBefore || p.Delivered) || !p.SameHand && p.SequenceAfter <= p.SequenceBefore)
                return false;
            var reason = _corePairObtainShownGiftsOperations.PairBenefitMoveReason(f, false);
            if (p.SameHand ? _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > p.SequenceBefore && m.Sequence <= p.SequenceAfter && m.CardId == p.CardId) : _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > p.SequenceBefore && m.Sequence <= p.SequenceAfter && m.CardId == p.CardId && m.From == p.From && (m.To == CardLocation.Hand(f.OwnerSeat) || m.To == CardLocation.OutsideGame && p.From.Zone == CardZoneKind.Equipment) && m.Reason.Value == reason) != 1)
                return false;
            if (!p.SameHand && p.Delivered != _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > p.SequenceBefore && m.Sequence <= p.SequenceAfter && m.CardId == p.CardId && m.From == p.From && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == reason))
                return false;
            return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PairObtainStepCommittedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Cursor == p.Cursor && e.CardOwnerSeat == target && e.RecipientSeat == p.RecipientSeat && e.SequenceBefore == p.SequenceBefore && e.SequenceAfter == p.SequenceAfter && e.SameHand == p.SameHand && e.Delivered == p.Delivered) == 1;
        }

        return Paid(d.FirstPayment, 0, d.FirstSeat) && Paid(d.SecondPayment, 1, d.SecondSeat) && (d.FirstPayment is null || d.SecondPayment is null || d.SecondPayment.SequenceBefore >= d.FirstPayment.SequenceAfter);
    }

    internal bool ValidShownPairGift(ProgramSkillFrame f)
    {
        if (!ValidPairObtain(f) || f.ShownPairGift is not { } r || r.InstructionIndex != 3 || f.InstructionIndex != 4 || r.FirstSeat != f.PairObtain!.FirstSeat || r.SecondSeat != f.PairObtain.SecondSeat || r.FrozenFirstHandCount < 0 || r.FrozenSecondHandCount < 0 || !Enum.IsDefined(r.Stage) || !Enum.IsDefined(r.FrozenSuit) || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || f.CardSetBindings.SingleOrDefault(b => b.Name == r.SourceBind)is not { Visibility: SkillProgramCardSetVisibility.Public } binding || !binding.CardIds.SequenceEqual([r.CardId]) || !binding.SourceLocations.SequenceEqual([CardLocation.Hand(f.OwnerSeat)]) || binding.FrozenRevealedSuit != r.FrozenSuit)
            return false;
        if (r.Stage == ProgramShownPairGiftStage.ChoosingRecipient)
            return r.RecipientSeat is null && !r.Delivered && !r.SameHand && r.SequenceAfter == r.SequenceBefore;
        if (r.RecipientSeat is not { } recipient || recipient != r.FirstSeat && recipient != r.SecondSeat || r.SameHand != (recipient == f.OwnerSeat) || (recipient == r.FirstSeat ? r.FrozenFirstHandCount : r.FrozenSecondHandCount) != Math.Min(r.FrozenFirstHandCount, r.FrozenSecondHandCount) || (r.SameHand ? r.SequenceAfter != r.SequenceBefore || r.Delivered : r.SequenceAfter <= r.SequenceBefore))
            return false;
        if (!r.SameHand && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == r.CardId && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Hand(recipient) && m.Reason.Value == _corePairObtainShownGiftsOperations.PairBenefitMoveReason(f, true)) != 1)
            return false;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShownPairGiftCommittedEvent>().Count(e => e.ProgramFrameId == f.Id && e.SourceBind == r.SourceBind && e.ShownCardId == r.CardId && e.FrozenSuit == r.FrozenSuit && e.RecipientSeat == recipient && e.FrozenFirstHandCount == r.FrozenFirstHandCount && e.FrozenSecondHandCount == r.FrozenSecondHandCount && e.SequenceBefore == r.SequenceBefore && e.SequenceAfter == r.SequenceAfter && e.SameHand == r.SameHand && e.Delivered == r.Delivered) != 1)
            return false;
        if (r.Stage == ProgramShownPairGiftStage.AwaitingGift)
            return r.DrawSequenceBefore == 0 && r.DrawSequenceAfter == 0 && r.ActualDrawCount == 0;
        var draws = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShownPairGiftRewardIssuedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (r.FrozenSuit == Suit.Spade)
            return r.Stage == ProgramShownPairGiftStage.Complete && draws.Length == 0;
        return r.DrawSequenceBefore >= r.SequenceAfter && r.DrawSequenceAfter >= r.DrawSequenceBefore && r.ActualDrawCount is >= 0 and <= 1 && draws is [var draw] && draw.OwnerSeat == f.OwnerSeat && draw.RequestedCount == 1 && draw.ActualCount == r.ActualDrawCount && draw.SequenceBefore == r.DrawSequenceBefore && draw.SequenceAfter == r.DrawSequenceAfter && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.DrawSequenceBefore && m.Sequence <= r.DrawSequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == $"{(_corePairObtainShownGiftsOperations.PairBenefitMoveReason(f, true))}.reward") == r.ActualDrawCount;
    }
}
