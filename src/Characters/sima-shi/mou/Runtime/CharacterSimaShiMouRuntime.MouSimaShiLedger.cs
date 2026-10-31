#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaShiMouRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.CardEntitySkillLabel, 36400)]
    internal string? MouSimaShiCardLabel(int cardId) => HasMouSimaShi && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiMarkedEvent>().Any(e => e.CardId == cardId) ? "死士" : null;
    internal CardConversionSource? MouSimaShiNoResponseSource(CharacterState actor, IReadOnlyList<int> ids) => HasMouSimaShi && actor.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkill(actor, MouSimaShiRuntime.Sanshi) && ids.Count > 0 && MouSimaShiRules.HasMarked(actor.Seat, ids) ? new(MouSimaShiRuntime.Sanshi, "dead-soldier-no-response", actor.Seat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(actor, MouSimaShiRuntime.Sanshi)) : null;
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 36400)]
    internal void ObserveMouSimaShiAction(IGameEvent payload)
    {
        if (!HasMouSimaShi)
            return;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { Action: { Type: CardActionType.Use } a } use && MouSimaShiNoResponseSource(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], a.PhysicalCards.Select(c => c.CardId).ToArray())is { } source && use.IssuedNoResponse is null)
        {
            var issued = new IssuedCardNoResponse(a.ActionId, use.Id, source, _coreActualTurnProgressTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { IssuedNoResponse = issued });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IssuedCardNoResponseEvent(issued));
        }

        if (payload is NativeTrueUseProcessingEntryEvent { Kind: NativeTrueUseDiscardKind.Use } native && _coreCardUseLifecycleOperations.LifecycleCardUse(native.ParentFrameId)is { Action: { Type: CardActionType.Use } nativeAction } && nativeAction.ActionId == native.ParentActionId && nativeAction.ProviderSeat == native.ProviderSeat && nativeAction.PhysicalCards.Any(c => c.CardId == native.CardId && c.CardKind == native.CardKind && c.From == native.From) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiMaterialEvent>().Any(e => e.ActionId == nativeAction.ActionId && e.CardId == native.CardId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSimaShiMaterialEvent(nativeAction.ActionId, nativeAction.ActorSeat, native.ParentFrameId, native.CardId, native.CardKind, native.From, native.EntrySequence, 0));
        if (payload is not CardActionAcceptedEvent { Action: { } action })
            return;
        // An accepted native action supplies the physical invoice, including a
        // true Played response with no CardUseFrame (Mengmou/Pingliao).
        var parent = action.Type == CardActionType.Use ? _coreCardUseLifecycleOperations.LifecycleCardUse(_resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.Action?.ActionId == action.ActionId)?.Id ?? 0)?.Id ?? 0 : _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id ?? 0;
        foreach (var cost in action.PhysicalCards)
        {
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiMaterialEvent>().Any(e => e.ActionId == action.ActionId && e.CardId == cost.CardId))
                continue;
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cost.CardId);
            if (entry is null || entry.CardKind != cost.CardKind || entry.From != cost.From)
                continue;
            var processing = entry.To == CardLocation.Processing && (action.Type == CardActionType.Use || entry.Reason == CardMoveReasons.Respond || entry.Reason == CardMoveReasons.Nullification || entry.Reason.Value == "conversion.declaration.pay");
            var direct = entry.To == CardLocation.DiscardPile && _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, cost.CardId);
            if (parent > 0 && (processing || direct))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSimaShiMaterialEvent(action.ActionId, action.ActorSeat, parent, cost.CardId, cost.CardKind, cost.From, processing ? entry.Sequence : 0, direct ? entry.Sequence : 0));
        }
    }

    internal void CaptureMouSimaShiDiscardBatch(CardMovementBatchContext batch)
    {
        if (!HasMouSimaShi)
            return;
        foreach (var m in batch.Movements.Where(m => m.To == CardLocation.DiscardPile && m.From != m.To && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiMarkedEvent>().Any(e => e.CardId == m.CardId)))
        {
            var previous = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(x => x.CardId == m.CardId && x.Sequence < m.Sequence);
            var finish = m.From == CardLocation.Processing && (CoreGameEngineNativeUsedDiscardOriginsRuntime.IsNativeUseFinishReason(m.Reason) || m.Reason == CardMoveReasons.ResponseFinished || m.Reason == CardMoveReasons.NullificationFinished);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSimaShiDiscardEvent(batch.TurnNumber, m.CardId, m.CardKind, m.Sequence, batch.ParentFrameId ?? 0, previous?.To == CardLocation.Processing ? previous.Sequence : 0, finish));
        }
    }

    internal bool ValidMouSimaShiDebt(MouSimaShiClaimIssuedEvent d) => d.Source.SkillId == MouSimaShiRuntime.Chenlve && d.Source.BindingId == "claim-dead-soldiers" && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(d.Phase) && d.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && d.Phase.ActorSeat == d.Source.OwnerSeat && _contentRegistryOperations._contentRegistry.GetSkill(d.Source.SkillId).Program?.GameplayHash == d.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiStartedEvent>().Count(e => e.FrameId == d.FrameId && e.Source == d.Source && e.GameplayHash == d.GameplayHash && e.Operation == SkillProgramEffectOp.ResolveMouSimaShiChenlve) == 1;
    internal IReadOnlyList<ActualPhaseEndItem> CollectMouSimaShiPhaseEndCandidates(ActualDiscardRecoveryPhaseKey phase) => !HasMouSimaShi ? [] : Array.AsReadOnly(_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiClaimIssuedEvent>().Where(d => d.Phase == phase && ValidMouSimaShiDebt(d) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiPhaseSettledEvent>().Any(e => e.DebtId == d.FrameId)).Select(d =>
    {
        var t = _contentRegistryOperations._contentRegistry.GetSkill(d.Source.SkillId).Program!.Triggers.Single(t => t.Id == "remove-issued-entities");
        return new ActualPhaseEndItem(new(d.Source.OwnerSeat, d.Source.SkillId, t.Id, d.Source.SkillInstanceId, d.GameplayHash, t.Priority), d.FrameId);
    }).ToArray());
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeMouSimaShiDeathCandidates(CharacterState owner, IReadOnlyList<ProgramTriggerCandidate> existing)
    {
        if (!HasMouSimaShi)
            return existing;
        var extra = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiClaimIssuedEvent>().Where(d => d.Source.OwnerSeat == owner.Seat && ValidMouSimaShiDebt(d) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiDeathSettledEvent>().Any(e => e.DebtId == d.FrameId)).Select(d =>
        {
            var t = _contentRegistryOperations._contentRegistry.GetSkill(d.Source.SkillId).Program!.Triggers.Single(t => t.Id == "death-return-shuffle");
            return new ProgramTriggerCandidate(owner.Seat, d.Source.SkillId, t.Id, d.Source.SkillInstanceId, d.GameplayHash, t.Priority);
        });
        return Array.AsReadOnly(existing.Where(c => c.SkillId != MouSimaShiRuntime.Chenlve || c.BindingId != "death-return-shuffle").Concat(extra).Distinct().ToArray());
    }

    internal MouSimaShiClaimIssuedEvent? MouSimaShiDue(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (!HasMouSimaShi || c.SkillId != MouSimaShiRuntime.Chenlve || w.OwnerSeat != c.OwnerSeat)
            return null;
        var debt = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiClaimIssuedEvent>().SingleOrDefault(d => d.Source.OwnerSeat == c.OwnerSeat && d.Source.SkillInstanceId == c.SkillInstanceId && d.GameplayHash == c.GameplayHash && ValidMouSimaShiDebt(d));
        if (debt is null)
            return null;
        var paid = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.MouSimaShi?.DebtId == debt.FrameId && f.WindowContext == w && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c));
        if (c.BindingId == "remove-issued-entities" && w.Window == SkillProgramTriggerWindow.ActualPhaseEnded && w.ActualPhaseEnd is { } due && due.EntitlementId == debt.FrameId && due.Phase == debt.Phase && _coreActualPhaseEndsOperations.IsExactActualPhaseEndCandidate(c, w) && (paid || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiPhaseSettledEvent>().Any(e => e.DebtId == debt.FrameId)))
            return debt;
        if (c.BindingId != "death-return-shuffle" || w.Window != SkillProgramTriggerWindow.OwnerDied || _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramDeathTriggerWindowFrame p || p.OwnerSeat != c.OwnerSeat || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || p.Candidates[p.CandidateIndex] != c || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == p.DeathFrameId)is not DeathFrame death || death.VictimSeat != c.OwnerSeat)
            return null;
        return paid || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSimaShiDeathSettledEvent>().Any(e => e.DebtId == debt.FrameId) ? debt : null;
    }

    [ProgramIssuedTrigger(440)]
    internal bool IsIssuedMouSimaShiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => MouSimaShiDue(c, w)is not null;
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 4500)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouSimaShiDiscardBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureMouSimaShiDiscardBatch(completed);
        return completed;
    }
}
