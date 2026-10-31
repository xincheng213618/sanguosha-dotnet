#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanTanYuanShangOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 50)]
    internal bool NeifaFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => HasNeifaCapability;
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 60)]
    internal bool NeifaNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasNeifaCapability;
    internal bool HasNeifaCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GainThenDiscardAndArmNeifaActualTurn);

    internal NeifaTurnState? CurrentNeifa(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreMatchParticipantsPlayersAccess._players[seat].NeifaTurnState is { ActualTurn: var turn } state && turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber ? state : null;
    internal int NeifaX(int seat, bool basicBranch) => Math.Min(5, _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(seat)).Count(c => basicBranch ? CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.Kind) != SkillProgramCardCategory.Basic : CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.Kind) == SkillProgramCardCategory.Basic));
    internal int NeifaSlashLimitBonus(int seat) => CurrentNeifa(seat)is { BasicBranch: true } ? NeifaX(seat, true) : 0;
    internal int NeifaSlashTargetBonus(int seat, CardKind kind) => CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && CurrentNeifa(seat)is { BasicBranch: true } ? 1 : 0;
    internal bool IsNeifaUseForbidden(int actor, CardKind kind, CardActionType type)
    {
        if (CurrentNeifa(actor)is not { } s)
            return false;
        var use = type == CardActionType.Use || type == CardActionType.Response && (kind == CardKind.Nullification || kind == CardKind.Dodge && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[actor], kind));
        if (!use)
            return false;
        var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind);
        return s.BasicBranch && category is SkillProgramCardCategory.Trick or SkillProgramCardCategory.Equipment || s.NonBasicBranch && category == SkillProgramCardCategory.Basic;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 530)]
    internal void ObserveNeifaTurnBoundary(IGameEvent payload)
    {
        if (payload is TurnEndedEvent)
            ExpireNeifaTurn();
    }

    internal void ExpireNeifaTurn()
    {
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            if (p.NeifaTurnState is { } s && s.ActualTurn <= _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
                p.NeifaTurnState = null;
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.CaptureMovement, 30)]
    internal void CaptureNeifaPaymentBatch(CardMovementBatchContext batch)
    {
        if (!HasNeifaCapability || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { Action: { Type: CardActionType.Use } a } use || CurrentNeifa(a.ActorSeat)is not { NonBasicBranch: true } || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(a.EffectiveKind) != SkillProgramCardCategory.Equipment || batch.ParentFrameId != use.Id || batch.ParentBatchId is not null || a.PhysicalCards.Count == 0 || batch.Movements.Count != a.PhysicalCards.Count)
            return;
        for (var i = 0; i < a.PhysicalCards.Count; i++)
        {
            var c = a.PhysicalCards[i];
            var m = batch.Movements[i];
            if (m.CardId != c.CardId || m.From != c.From || m.Reason != CardMoveReasons.EquipmentUse || m.To != (_coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(a, c.CardId) ? CardLocation.DrawPile : CardLocation.Processing) || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
                return;
        }

        if (use.NeifaMaterialPayment is not null)
            throw new InvalidOperationException("Neifa equipment cannot pay its materials twice.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { NeifaMaterialPayment = new(use.Id, a.ActionId, batch.Movements[0].Sequence - 1, batch.Movements[^1].Sequence) { Batch = batch } });
    }

    internal void CaptureNeifaPaidEquipmentUse(long useId, CardActionContext action)
    {
        if (!HasNeifaCapability || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind) != SkillProgramCardCategory.Equipment || CurrentNeifa(action.ActorSeat)is not { NonBasicBranch: true } state || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { } actual } use)
            return;
        if (action.Type != CardActionType.Use || !CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(actual, action) || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action))
            throw new InvalidOperationException("Neifa equipment requires actual accepted native use.");
        if (use.NeifaEquipment is not null)
            return;
        if (action.PhysicalCards.Count > 0)
        {
            if (use.NeifaMaterialPayment is not { } p || p.OwnerFrameId != use.Id || p.ActionId != action.ActionId || p.Batch.ParentFrameId != use.Id || p.Batch.Movements.Count != action.PhysicalCards.Count || p.SequenceAfter <= p.SequenceBefore)
                throw new InvalidOperationException("Neifa equipment has no actual material payment.");
            for (var i = 0; i < action.PhysicalCards.Count; i++)
            {
                var c = action.PhysicalCards[i];
                var m = p.Batch.Movements[i];
                if (m.CardId != c.CardId || m.From != c.From || m.Reason != CardMoveReasons.EquipmentUse || m.Sequence <= p.SequenceBefore || m.Sequence > p.SequenceAfter || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.To != (_coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, c.CardId) ? CardLocation.DrawPile : CardLocation.Processing))
                    throw new InvalidOperationException("Neifa equipment payment changed its native materials.");
            }
        }
        else if (use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 })
            throw new InvalidOperationException("Neifa zero-material equipment requires its actual issued native zero-entity use.");
        var ordinal = checked(state.EquipmentUses + 1);
        _coreMatchParticipantsPlayersAccess._players[action.ActorSeat].NeifaTurnState = state with
        {
            EquipmentUses = ordinal
        };
        var q = new NeifaEquipmentUseProof(use.Id, action, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, ordinal, ordinal <= 2 ? NeifaX(action.ActorSeat, false) : 0, state.Source, state.GameplayHash);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { NeifaEquipment = q });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NeifaEquipmentUseCountedEvent(use.Id, action.ActionId, action.ActorSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, ordinal, q.Source, q.GameplayHash));
        if (ordinal > 2)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NeifaEquipmentUseQualifiedEvent(use.Id, action.ActionId, action.ActorSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, ordinal, q.RequestedDraw, q.Source, q.GameplayHash));
    }

    internal bool ValidNeifaEquipment(NeifaEquipmentUseProof q, CardUseFrame use) => q.UseId == use.Id && q.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && q.Ordinal is 1 or 2 && q.RequestedDraw is >= 0 and <= 5 && use.Action is { } a && CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(a, q.Action) && CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(a.EffectiveKind) == SkillProgramCardCategory.Equipment && _coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, a) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NeifaEquipmentUseQualifiedEvent>().Count(e => e == new NeifaEquipmentUseQualifiedEvent(use.Id, a.ActionId, a.ActorSeat, q.ActualTurn, q.Ordinal, q.RequestedDraw, q.Source, q.GameplayHash)) == 1;
}
