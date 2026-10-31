#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunYuanOlRuntime
{
    internal const string DongxinDiscardReason = "skill-program.sun-yuan.dongxin.discard";
    internal const string DongxinReclaimReason = "skill-program.sun-yuan.dongxin.reclaim";
    internal bool DongxinDirectionAvailable(int owner, string direction) => !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<SunYuanPaidEvent>().Any(e => e.OwnerSeat == owner && e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Direction == direction);
    internal bool DongxinContextLegal(int owner, string direction, ProgramSkillWindowContext c) => c.Window == SkillProgramTriggerWindow.AfterDamageApplied && c.Amount > 0 && c.TargetSeat is { } victim && _coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) && c.SourceSeat is { } source && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && (direction == "dealt" ? source == owner : direction == "received" && victim == owner) && DongxinDirectionAvailable(owner, direction) && DongxinCanPay(owner, victim);
    internal bool DongxinCanPay(int owner, int victim)
    {
        var total = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[victim]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment2(victim).Count;
        var required = Math.Min(Math.Max(0, _coreMatchParticipantsPlayersAccess._players[victim].MaxHp - _coreMatchParticipantsPlayersAccess._players[victim].Hp) + 1, total);
        return SunYuanEligibleCards(owner, victim, false).Count >= required;
    }

    // Registration only; the query still belongs to legacy GameEngine.Dongxin.Queries.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 80)]
    internal void ProjectDongxinCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasSunYuanCapability && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DongxinEntityAcquiredEvent>().Any(grant => grant.CardId == cardId && _cardZonesOperations._cardZones.Contains(CardLocation.Hand(grant.RecipientSeat), cardId)) ? "恫心" : null);
    }

    internal bool HasDongxinEntitySlashQuota(CharacterState actor, CardKind kind, IReadOnlyList<int> ids) => HasSunYuanCapability && actor.IsAlive && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && ids is [var id] && id > 0 && _cardZonesOperations._cardZones.Contains(CardLocation.Hand(actor.Seat), id) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DongxinEntityAcquiredEvent>().Any(e => e.RecipientSeat == actor.Seat && e.CardId == id && CoreNativeCardClassificationsRuntime.IsSlashCard(e.EffectiveKind));
    internal void BindDongxinEntityUse(CardUseFrame supplied)
    {
        if (!HasSunYuanCapability || supplied.DongxinUse is not null || supplied.Action is not { Type: CardActionType.Use } a || a.ActorSeat != a.ProviderSeat || !HasDongxinEntitySlashQuota(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], a.EffectiveKind, a.PhysicalCards.Select(c => c.CardId).ToArray()))
            return;
        var id = a.PhysicalCards.Single().CardId;
        var grant = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DongxinEntityAcquiredEvent>().Last(e => e.RecipientSeat == a.ActorSeat && e.CardId == id);
        var receipt = new DongxinUseReceipt(grant.FrameId, id, grant.AcquiredSequence, a.ActorSeat, a.ProviderSeat, supplied.Id, a.ActionId);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(supplied.Id, u => u with { DongxinUse = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DongxinEntityUseBoundEvent(receipt));
    }

    internal bool HasIssuedDongxinEntitySlashQuota(long? useId, int actor) => useId is { } id && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { DongxinUse: { } r, Action: { } a } u && r.UseId == u.Id && r.ActionId == a.ActionId && u.SourceSeat == actor && a.ActorSeat == actor && a.ProviderSeat == r.Provider && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(u, r.OriginalActor, r.Provider) && a.PhysicalCards is [var cost] && cost.CardId == r.CardId && CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DongxinEntityUseBoundEvent>().Count(e => e.Receipt == r) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DongxinEntityAcquiredEvent>().Count(e => e.FrameId == r.GrantFrameId && e.CardId == r.CardId && e.RecipientSeat == r.OriginalActor && e.AcquiredSequence == r.AcquiredSequence) == 1;
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27491)]
    internal void RecordDongxinEntityDeparture(CardMovedEvent e)
    {
        if (!HasSunYuanCapability || e.From.Zone != CardZoneKind.Hand || e.From.OwnerSeat is not { } owner || e.To == e.From || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault()is not { } m || m.CardId != e.CardId || m.From != e.From || m.To != e.To || m.Reason != e.Reason)
            return;
        foreach (var grant in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DongxinEntityAcquiredEvent>().Where(g => g.RecipientSeat == owner && g.CardId == m.CardId).DistinctBy(g => g.FrameId).ToArray())
            if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<DongxinEntityDepartedEvent>().Any(d => d.GrantFrameId == grant.FrameId && d.CardId == m.CardId && d.Sequence == m.Sequence))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DongxinEntityDepartedEvent(grant.FrameId, owner, m.CardId, m.Sequence));
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 30)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 70)]
    internal bool HasIssuedDongxinSlashDebitExemption(long useId, int actorSeat) => HasIssuedDongxinEntitySlashQuota(useId, actorSeat);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 50)]
    internal bool DongxinSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => HasDongxinEntitySlashQuota(query.Actor, query.Kind, query.PhysicalCardIds ?? (query.PhysicalCard is { Id: > 0 } ? [query.PhysicalCard.Id] : []));
}
