#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKeBiNengOlRuntime
{
    internal bool ValidKeBiNeng(ProgramSkillFrame f)
    {
        if (f.KeBiNengPingduan is not { } r || f.SkillId != "ol:pingduan" || f.ActivationId != "ordered-three-items" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || r.InstructionIndex != 1 || r.Source != KeBiNengSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var actor] || actor != r.ActorSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.CompletedItems is < 0 or > 3 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolvePingduanOrderedBasicRecastEquipment || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<KeBiNengPingduanStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.ActualTurn == r.ActualTurn && e.PhaseInstance == r.PhaseInstance && e.ActorSeat == actor) != 1)
            return false;
        if (r.NativeUse is { } origin && (origin.ProgramFrameId != f.Id || origin.InstructionIndex != f.InstructionIndex || origin.Source != r.Source || origin.GameplayHash != r.GameplayHash || origin.ActualTurn != r.ActualTurn || origin.PhaseInstance != r.PhaseInstance || origin.ActorSeat != actor || origin.CardUseFrameId <= f.Id || origin.ActionId <= 0 || CardCatalog.Get(origin.CardKind).CategoryName != "基本牌" || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<KeBiNengPingduanNativeIssuedEvent>().Count(e => e.Origin == origin) != 1))
            return false;
        if (r.SelectedBasicAction is { } selected && !r.BasicActions.Any(a => SameKeBiNengAction(a, selected)))
            return false;
        if (r.SelectedBasicAction is { ConversionSource: { } conversion } acceptedKenshang && _contentRegistryOperations._contentRegistry.GetSkill(conversion.SkillId).Program?.ViewAs.Any(v => v.Id == conversion.BindingId && v.KenshangUse == true) == true && !_characterMaXiuMaTieOlOperations.HasAcceptedKenshangKeAction(f, acceptedKenshang))
            return false;
        if (r.SelectedBasicAction is { ConversionSource.SkillId: "ol:zonghu" } acceptedZonghu && !_characterLiuZhangOlOperations.HasAcceptedZonghuKeAction(f, acceptedZonghu))
            return false;
        if (r.SelectedBasicAction is { ConversionSource.SkillId: "ol:huanpei" } acceptedHuanpei && !_characterHuanShuJunOlOperations.HasAcceptedHuanpeiKeAction(f, acceptedHuanpei))
            return false;
        if (r.SelectedBasicAction is { ConversionSource.SkillId: BoundaryLiuFengComposition.Qinling } acceptedQinling && !_characterLiuFengBoundaryOperations.HasAcceptedBoundaryLiuFengKeAction(f, acceptedQinling))
            return false;
        var paid = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<KeBiNengPingduanItemPaidEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Item).ToArray();
        if (paid.Length != r.CompletedItems || !paid.Select(e => e.Item).SequenceEqual(Enumerable.Range(1, r.CompletedItems)) || paid.Any(e => e.ActorSeat != actor))
            return false;
        if (r.CompletedItems > 0 && (r.NativeUse is null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<KeBiNengPingduanNativeReturnedEvent>().Count(e => e.Origin == r.NativeUse) != 1))
            return false;
        if (KeBiNengCardPaymentStage(r))
        {
            if (r.SelectedCard is not { } c || r.PaidBatchId <= 0 || paid.LastOrDefault()is not { } cardPaid || cardPaid.CardId != c.CardId || cardPaid.From != c.From || cardPaid.Before != r.Before || cardPaid.After > r.After || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == c.CardId && m.From == c.From && m.Reason == KeBiNengPaymentReason(r)) != 1)
                return false;
        }

        if (r.Stage is KeBiNengPingduanStage.BasicRewardChildren or KeBiNengPingduanStage.RecastDrawChildren or KeBiNengPingduanStage.RecastRewardChildren or KeBiNengPingduanStage.EquipmentRewardChildren)
        {
            var recast = r.Stage == KeBiNengPingduanStage.RecastDrawChildren;
            if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<KeBiNengPingduanDrawPaidEvent>().Count(e => e.FrameId == f.Id && e.Item == r.CompletedItems && e.RecastDraw == recast && e.Before == r.Before && e.After == r.After && e.DrawCount == r.DrawCount) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(actor) && m.Reason == (recast ? CardMoveReasons.RecastDraw : new CardMoveReason(PingduanRewardReason))) != r.DrawCount)
                return false;
        }

        return r.Stage switch
        {
            KeBiNengPingduanStage.BasicChoice => r.SelectedBasicAction is null && r.NativeUse is null && r.CompletedItems == 0,
            KeBiNengPingduanStage.BasicIssuing => r.SelectedBasicAction is not null && r.NativeUse is null && r.CompletedItems == 0,
            KeBiNengPingduanStage.BasicNative => r.NativeUse is not null && r.CompletedItems == 0,
            KeBiNengPingduanStage.BasicReturned or KeBiNengPingduanStage.BasicRewardChildren or KeBiNengPingduanStage.RecastChoice => r.NativeUse is not null && r.CompletedItems == 1,
            KeBiNengPingduanStage.RecastChildren or KeBiNengPingduanStage.RecastDrawChildren or KeBiNengPingduanStage.RecastRewardChildren or KeBiNengPingduanStage.EquipmentChoice => r.SelectedCard is not null && r.CompletedItems == 2,
            KeBiNengPingduanStage.EquipmentChildren or KeBiNengPingduanStage.EquipmentRewardChildren => r.SelectedCard is not null && r.CompletedItems == 3,
            _ => false
        };
    }
}
