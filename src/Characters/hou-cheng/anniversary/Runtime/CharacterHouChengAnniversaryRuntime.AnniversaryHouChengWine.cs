#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHouChengAnniversaryRuntime
{
    internal bool ValidHouChengMovement(AnniversaryHouChengMovementEvent e)
    {
        if (e.Source.SkillId != AnniversaryHouChengComposition.Skill || e.Source.BindingId is not ("foreign-discard" or "after-damage") || e.Hash != _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash || e.Round < 1 || e.Round > _coreActualRoundProgressRoundNumberAccess._roundNumber || e.Before < 0 || e.After < e.Before || e.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || e.BatchId <= 0 || e.ActualCount is < 0 or > 2 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(e.CardOwner) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(e.Recipient) || (e.Giving ? e.CardOwner != e.Source.OwnerSeat || e.Recipient == e.Source.OwnerSeat : e.Recipient != e.Source.OwnerSeat) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryHouChengStartedEvent>().Count(s => s.FrameId == e.FrameId && s.Source == e.Source && s.Hash == e.Hash && s.Round == e.Round && s.Turn == e.Turn && s.TurnOwner == e.TurnOwner) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryHouChengMovementEvent>().Count(m => m.FrameId == e.FrameId && m.Giving == e.Giving) != 1)
            return false;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > e.Before && m.Sequence <= e.After && m.Reason.Value == (e.Giving ? HouChengGiveReason : HouChengObtainReason)).ToArray();
        return moves.Length is >= 1 and <= 2 && moves.Select(m => m.CardId).Distinct().Count() == moves.Length && moves.All(m => m.TurnNumber == e.Turn && m.From.OwnerSeat == e.CardOwner && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && (m.To == CardLocation.Hand(e.Recipient) || m.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon || _coreWushuangShopCardsOperations.IsWushuangShopDestroyedMaterial(m) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Any(d => d.CardId == m.CardId && d.MovementSequence == m.Sequence && d.ParentFrameId == e.FrameId)) || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(m, CardLocation.Hand(e.Recipient)))) && moves.Count(m => m.To == CardLocation.Hand(e.Recipient) && m.From != m.To) == e.ActualCount;
    }

    internal AnniversaryHouChengMovementEvent? HouChengWineMark(int cardId) => !HasHouChengContent || cardId <= 0 || !CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(_coreAdvancedLifecycleOperations.GetAdvancedCard(cardId).Kind, SkillProgramCardCategory.Basic) ? null : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryHouChengMovementEvent>().LastOrDefault(e => ValidHouChengMovement(e) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == cardId && m.Sequence > e.Before && m.Sequence <= e.After && m.To == CardLocation.Hand(e.Recipient) && m.From != m.To && m.Reason.Value == (e.Giving ? HouChengGiveReason : HouChengObtainReason)));
    internal bool IsAnniversaryHouChengWineCard(int cardId) => HouChengWineMark(cardId)is not null;
    internal IReadOnlyList<CardConversionSource> AnniversaryHouChengWineSources(CharacterState owner, Card card, CardKind kind, bool forResponse, bool dyingUse)
    {
        if (kind != CardKind.Alcohol || forResponse && (!dyingUse || _coreDyingCursorRuntimeOperations.Value.ActiveDying?.VictimSeat != owner.Seat) || HouChengWineMark(card.Id)is not { } mark || !_corePhysicalOwnedCardQueriesOperations.GetPlayableCards(owner).Any(c => c.Id == card.Id) || _coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, card) || _coreResponseEntityExchangeOperations.IsResponseEntityRestricted(owner.Seat, card.Id) || _coreProgramCardLegalityOperations.IsProgramEntityPhysicalRestricted(owner.Seat, card.Id) || _coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(owner.Seat, kind, [card.Id], forResponse && !dyingUse) || !_coreProgramCardLegalityOperations.IsProgramNativeResponseAllowed(owner, kind, [card.Id], dyingUse: dyingUse))
            return[];
        return[mark.Source];
    }

    internal bool HouChengUseBanned(int actor, CardKind kind, IEnumerable<int> ids, bool response, bool dying = false) => kind != CardKind.Alcohol && (!response || dying || kind == CardKind.Nullification || _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[actor], kind) || CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && (_characterGuanYuWumiaoOperations.IsJuewuTrueSlashUse(_coreMatchParticipantsPlayersAccess._players[actor]) || _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingProviders: true } request && request.CurrentCandidateSeat == actor && CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(request))) && ids.Any(IsAnniversaryHouChengWineCard);
    // IronChain conversion enumeration also supplies native Recast. Let that
    // enumeration proceed; the final action and submitted-Use guards below still
    // reject every real non-Wine Use, including converted IronChain.
    [ProgramCardLegality(ProgramCardLegalityStage.MaterialUseBan, 48700)]
    internal bool HouChengMaterialBan(ProgramCardMaterialQuery q) => q.Kind != CardKind.IronChain && HouChengUseBanned(q.ActorSeat, q.Kind, q.CardIds, q.IsResponse);
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 48700)]
    internal bool HouChengResponseBan(ProgramNativeResponseQuery q) => HouChengUseBanned(q.Provider.Seat, q.Kind, q.CardIds, true, q.DyingUse);
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 48700)]
    internal bool HouChengViewAsBan(ProgramViewAsLegalityQuery q) => q.Kind != CardKind.IronChain && HouChengUseBanned(q.Owner.Seat, q.Kind, q.CardIds, q.ForResponse && q.RequestedUseTarget is null, q.DyingUse);
    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 48700)]
    internal bool HouChengLegalUseBan(ProgramLegalActionQuery q) => q.Action.Kind is not (LegalActionKind.Recast or LegalActionKind.UseProgramSkill or LegalActionKind.EndPlay or LegalActionKind.UseEquipmentEffect or LegalActionKind.RevealGeneral) && q.Action.CardId is { } id && HouChengUseBanned(q.Actor.Seat, q.Action.PlayedCardKind ?? _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, [id], false);
    [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 48700)]
    internal void HouChengSubmittedBan(ProgramSubmittedUseQuery q)
    {
        if (HouChengUseBanned(q.ActorSeat, q.Kind, q.CardIds, false))
            throw new InvalidOperationException("Xianniang marked Basic entities may only participate in actual Alcohol uses.");
    }

    [ProgramCardAction(ProgramCardActionPhase.BeforeProgram, 48700)]
    internal void AddHouChengWineActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = null)
    {
        if (!HasHouChengContent || !actor.HasHpValue || actor.HasAlcoholEffect)
            return;
        foreach (var card in playable.Where(c => selectedPhysicalCardId is null || selectedPhysicalCardId == c.Id))
            foreach (var source in AnniversaryHouChengWineSources(actor, card, CardKind.Alcohol, false, false))
            {
                // Ordinary native Wine/conversion enumeration already supplies this
                // exact action. Only an existing whole-action identity needs a bridge.
                if (!_coreCardConversionsOperations.HasProgramCardIdentity(actor, card))
                    continue;
                if (!(_characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, card.Id) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || _coreAdvancedPoliciesOperations.Value.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.Value.HasFriendQihuiAllowance(actor) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol) || _coreRoundGainedSourceUsesOperations.HasRoundGainedBasicBonus(actor, CardKind.Alcohol, [card.Id]) || _characterHuangfuSongWumiaoOperations.HasWuHfOrDixianMaterialPrivilege(actor, CardKind.Alcohol, [card.Id], source)) || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, CardKind.Alcohol, CardActionType.Use))
                    continue;
                actions.Add(new(LegalActionKind.Alcohol, card.Id, null, $"献酿：将【{(card.DisplayName)}】当【酒】使用", CardKind.Alcohol) { ConversionSource = source });
            }
    }
}
