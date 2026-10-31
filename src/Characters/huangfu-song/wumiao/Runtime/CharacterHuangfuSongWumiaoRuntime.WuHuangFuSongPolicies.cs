#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangfuSongWumiaoRuntime
{
    internal bool WuHfMarkedFor(int owner, int id, WuHuangFuSongTurn turn) => HasWuHuangFuSongCapability && id > 0 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuHuangFuSongLianjieEntityEvent>().Any(e => e.OwnerSeat == owner && e.CardId == id && e.Turn == turn && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.DrawSequence && m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(owner) && m.Reason.Value == WuHfDraw));
    internal bool HasWuHfMaterialPrivilege(CharacterState actor, IReadOnlyList<int>? ids)
    {
        if (!HasWuHuangFuSongCapability || !actor.IsAlive || ids is not { Count: > 0 } || ids.Distinct().Count() != ids.Count || !WuHfTurnOpen(WuHfTurn()))
            return false;
        var paidUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(use => use.WuHuangFuSongUse is { LianjieMaterials: true } r && r.ActorSeat == actor.Seat && r.ProviderSeat == actor.Seat && use.Action is { Type: CardActionType.Use } a && a.ActionId == r.ActionId && a.ActorSeat == actor.Seat && a.ProviderSeat == actor.Seat && a.PhysicalCards.Count == ids.Count && a.PhysicalCards.All(c => ids.Contains(c.CardId)) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuHuangFuSongUseAdmittedEvent>().Any(e => e.Receipt == r));
        return ids.All(id => WuHfMarkedFor(actor.Seat, id, WuHfTurn()) && (_cardZonesOperations._cardZones.GetLocation(id)is { OwnerSeat: var owner, Zone: CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.WoodenOxGrain } && owner == actor.Seat || _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && (paidUse || _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(actor.Seat, id)is { ActorSeat: var seat, ProviderSeat: var provider } && seat == actor.Seat && provider == seat)));
    }

    internal bool HasWuHfPotential(int owner) => HasWuHuangFuSongCapability && _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(_coreMatchParticipantsPlayersAccess._players[owner]).Any(c => HasWuHfMaterialPrivilege(_coreMatchParticipantsPlayersAccess._players[owner], [c.Id]));
    internal bool HasWuHfOrDixianMaterialPrivilege(CharacterState actor, CardKind kind, IReadOnlyList<int>? ids, CardConversionSource? source = null) => HasWuHfMaterialPrivilege(actor, ids) || _characterLuKangWumiaoOperations.HasWuLuKangDixianMaterialPrivilege(actor, kind, ids, source) || _coreProgramTargetRulesOperations.TargetRules.HasMaterialPrivilege(new(actor.Seat, kind, Array.AsReadOnly((ids ?? []).ToArray()), source));
    internal bool HasWuHfOrWuyouMaterialPrivilege(CharacterState actor, CardKind kind, IReadOnlyList<int>? ids, long? useId = null) => (useId is { } id ? HasIssuedWuHfUse(id, actor.Seat) : HasWuHfMaterialPrivilege(actor, ids)) || _characterGuanYuWumiaoOperations.HasWuyouMaterialPrivilege(actor, kind, ids, useId);
    internal CardActionContext CaptureWuHfUseAction(CardActionContext a) => !HasWuHuangFuSongCapability || a.EffectiveRank is not null || a.ConversionChain.Any(_coreWushuangJinzhiOperations.IsWushuangJinzhiSource) ? a : new(a.ActionId, a.ParentActionId, a.Type, a.ActorSeat, a.ProviderSeat, a.RequesterSeat, a.ResponderSeat, a.OpponentSeat, a.EffectiveKind, a.TargetSeats, a.PhysicalCards, a.ConversionChain, a.DesignatedTargetSeats, a.EffectiveSuit, 0, a.EffectiveIsRed, a.FactionOrigin);
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 10)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 20)]
    internal bool HasWuHfOrDixianIssuedUse(long id, int actor) => HasIssuedWuHfUse(id, actor) || _characterLuKangWumiaoOperations.HasIssuedWuLuKangDixianUse(id, actor);
    internal bool HasWuHfOrDixianPotential(int actor) => HasWuHfPotential(actor) || _characterLuKangWumiaoOperations.HasWuLuKangDixianGrant(actor) || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Any(card => _coreProgramTargetRulesOperations.TargetRules.HasMaterialPrivilege(new(actor, CardKind.Slash, Array.AsReadOnly(new[] { card.Id }))));
    internal bool HasIssuedWuHfUse(long id, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { WuHuangFuSongUse: { LianjieMaterials: true } r, Action: { } a } && r.UseId == id && r.ActionId == a.ActionId && r.ActorSeat == actor && r.ProviderSeat == actor && a.ActorSeat == actor && a.ProviderSeat == actor && r.MaterialCount == a.PhysicalCards.Count && r.MaterialCount > 0 && WuHfTurnOpen(r.Turn) && a.PhysicalCards.All(c => c.From.OwnerSeat == actor && c.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.WoodenOxGrain && WuHfMarkedFor(actor, c.CardId, r.Turn)) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuHuangFuSongUseAdmittedEvent>().Any(e => e.Receipt == r);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 30001)]
    internal void ObserveWuHfUse(IGameEvent fact)
    {
        if (!HasWuHuangFuSongCapability)
            return;
        var id = fact switch
        {
            CardUseDeclaredEvent d => d.ResolutionId,
            ProgramCardUseActorReplacedEvent reassigned => reassigned.CardUseFrameId,
            _ => 0
        };
        if (id <= 0 || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: { Type: CardActionType.Use } a } use)
            return;
        var own = a.ActorSeat == a.ProviderSeat && a.PhysicalCards.Count > 0 && a.PhysicalCards.All(c => c.From.OwnerSeat == a.ActorSeat);
        var r = new WuHuangFuSongUseReceipt(id, a.ActionId, a.ActorSeat, a.ProviderSeat, a.EffectiveKind, a.EffectiveRank, a.PhysicalCards.Count, own && a.PhysicalCards.All(c => c.From == CardLocation.Hand(a.ActorSeat)), own && a.PhysicalCards.All(c => c.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.WoodenOxGrain && WuHfMarkedFor(a.ActorSeat, c.CardId, WuHfTurn())), WuHfTurn());
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { WuHuangFuSongUse = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuHuangFuSongUseAdmittedEvent(r));
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 10)]
    internal bool DistanceCardUseExemption10HasWuHfOrDixianIssuedUse(ProgramCardDistanceQuery q) => q.CardUseFrameId is { } dixianUse && HasWuHfOrDixianIssuedUse(dixianUse, q.Actor.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 100)]
    internal bool DistancePhysicalSlash100HasWuHfOrWuyouMaterialPrivilege(ProgramSlashDistanceQuery q) => HasWuHfOrWuyouMaterialPrivilege(q.Actor, q.Kind, q.PhysicalCardIds ?? (q.PhysicalCard!.Id > 0 ? [q.PhysicalCard!.Id] : []), q.ExistingUseFrameId);
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 10)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 10)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 20)]
    internal bool DistanceVirtualSlash10HasWuHfOrWuyouMaterialPrivilege(ProgramSlashDistanceQuery q) => HasWuHfOrWuyouMaterialPrivilege(q.Actor, q.Kind, q.PhysicalCardIds);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 20)]
    internal bool DistanceBorrowedPhysicalSlash20HasWuHfOrWuyouMaterialPrivilege(ProgramSlashDistanceQuery q) => HasWuHfOrWuyouMaterialPrivilege(q.Actor, q.Kind, [q.PhysicalCard!.Id]);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 30)]
    internal bool WuHfWuyouSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => HasWuHfOrWuyouMaterialPrivilege(query.Actor, query.Kind, query.PhysicalCardIds ?? (query.PhysicalCard is { Id: > 0 } ? [query.PhysicalCard.Id] : []));
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 70)]
    internal bool WuHfMaterialSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => HasWuHfMaterialPrivilege(query.Actor, query.PhysicalCardIds ?? (query.PhysicalCard is { Id: > 0 } ? [query.PhysicalCard.Id] : []));
}
