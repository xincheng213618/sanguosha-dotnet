#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoZhaoAnniversaryRuntime
{
    internal IEnumerable<ProgramTriggerCandidate> CollectIssuedAnniversaryGuoZhaoCandidates(CardMovementBatchContext batch)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawAnniversaryGuoZhaoLostColor))
            yield break;
        foreach (var source in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryGuoZhaoLeaseEvent>().Select(e => (e.Source.OwnerSeat, e.Source.SkillInstanceId, e.Hash)).Distinct())
        {
            if (!_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat].IsAlive || GuoZhaoLostBenefits(source.OwnerSeat, source.SkillInstanceId, source.Hash, batch).Count == 0)
                continue;
            var p = _contentRegistryOperations._contentRegistry.GetSkill(AnniversaryGuoZhaoComposition.Pianchong).Program!;
            if (p.GameplayHash != source.Hash)
                throw new InvalidOperationException("Pianchong's issued duration cannot cross a changed program hash.");
            var t = p.Triggers.Single(t => t.Id == "lost-color-benefit");
            yield return new(source.OwnerSeat, p.Id, t.Id, source.SkillInstanceId, source.Hash, t.Priority);
        }
    }

    internal bool IsIssuedAnniversaryGuoZhaoCandidate(ProgramTriggerCandidate c) => c.SkillId == AnniversaryGuoZhaoComposition.Pianchong && c.BindingId == "lost-color-benefit" && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryGuoZhaoLeaseEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId && e.Hash == c.GameplayHash);
    internal bool CanRunIssuedAnniversaryGuoZhaoCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsIssuedAnniversaryGuoZhaoCandidate(c) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || c.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash || context is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } || context.OwnerSeat != c.OwnerSeat || context.SourceSeat != c.OwnerSeat || context.TargetSeat != c.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } w || w.Batch.Id != batch.Id || !w.Batch.Movements.SequenceEqual(batch.Movements) || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex] != c)
            return false;
        return GuoZhaoLostBenefits(c.OwnerSeat, c.SkillInstanceId, c.GameplayHash, batch).Count > 0;
    }

    internal Card[] GuoZhaoEquipmentCandidates(ProgramSkillFrame f)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        return !owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None ? [] : _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && !c.IsGeneralWeapon && owner.EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot) > 0 && !_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, c.Kind, CardActionType.Use) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(owner, c.Kind, [owner.Seat]) && !_coreResponseEntityExchangeOperations.IsResponseEntityRestricted(owner.Seat, c.Id) && !_coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(owner, c)).OrderBy(c => c.Id).ToArray();
    }

    internal void BeginGuoZhaoEquipment(ProgramSkillFrame f)
    {
        var r = f.AnniversaryGuoZhao!;
        if (!ValidGuoZhao(f) || !r.Paid || r.Option != "equipment" || r.Stage is not (AnniversaryGuoZhaoStage.EquipmentNext or AnniversaryGuoZhaoStage.EquipmentChildren))
            throw new InvalidOperationException("Zunwei equipment needs its original paid option and returned whole equipment Use.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetEquipment2(f.OwnerSeat).Count >= _corePhysicalOwnedCardQueriesOperations.GetEquipment2(r.Target).Count || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            FinishGuoZhao(f);
            return;
        }

        var cards = GuoZhaoEquipmentCandidates(f);
        if (cards.Length == 0)
        {
            FinishGuoZhao(f);
            return;
        }

        var before = _randomOperations._random.State;
        var card = cards[_randomOperations._random.Next(cards.Length)];
        var after = _randomOperations._random.State;
        var receipt = new AnniversaryGuoZhaoEquipment(r.Cursor, card.Id, card.Kind, 0, 0, before, after)
        {
            Candidates = cards.Select(c => c.Id).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryGuoZhao = r with { Stage = AnniversaryGuoZhaoStage.NativeEquipment, Equipment = receipt, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [], conversionSource: r.Source);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id) ?? throw new InvalidOperationException("Zunwei needs its actual physical equipment Use.");
        var actionId = use.Action!.ActionId;
        f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == f.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { AnniversaryGuoZhao = f.AnniversaryGuoZhao!with { Equipment = receipt with { UseId = id, ActionId = actionId } } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryGuoZhaoEquipmentIssuedEvent(f.Id, r.Cursor, card.Id, card.Kind, id, actionId));
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, CardMoveReasons.EquipmentUse);
        if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card, id))
            _coreNativeEquipmentUseOperations.CompleteEquipmentUse(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card, id);
    }

    internal bool IsAnniversaryGuoZhaoNativeEquipment(ResolutionFrame child, ResolutionFrame parent)
    {
        if (child is not CardUseFrame use || parent is not ProgramSkillFrame { AnniversaryGuoZhao: { Paid: true, Option: "equipment", Stage: AnniversaryGuoZhaoStage.NativeEquipment, Equipment: { } equipment } r } f || !ValidGuoZhao(f) || use.Id != equipment.UseId || use.CardId != equipment.CardId || use.CardKind != equipment.Kind || !(use.PhysicalCardIds ?? [use.CardId]).SequenceEqual([equipment.CardId]) || use.Action is not { Type: CardActionType.Use, PhysicalCards: [var cost] } action || action.ActionId != equipment.ActionId || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || action.EffectiveKind != equipment.Kind || cost.CardId != equipment.CardId || cost.From != CardLocation.DrawPile || !action.ConversionChain.Contains(r.Source))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryGuoZhaoEquipmentIssuedEvent>().Count(e => e == new AnniversaryGuoZhaoEquipmentIssuedEvent(f.Id, equipment.Ordinal, equipment.CardId, equipment.Kind, equipment.UseId, equipment.ActionId)) == 1;
    }

    [NativeUseValidation(44300)]
    internal void ValidateGuoZhaoEquipment(CardUseFrame use)
    {
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.AnniversaryGuoZhao?.Equipment?.UseId == use.Id);
        if (parent is not null && (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || !IsAnniversaryGuoZhaoNativeEquipment(use, parent)))
            throw new InvalidOperationException("Zunwei cannot complete a different or unfinished equipment Use.");
    }

    [NativeUseReturn(44300)]
    internal bool ReturnGuoZhaoEquipment(CardUseFrame use)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryGuoZhao: { Stage: AnniversaryGuoZhaoStage.NativeEquipment, Equipment: { } paid } r } f || paid.UseId != use.Id)
            return false;
        if (_resolutionStackOperations._resolutionStack.Any(x => x.Id == use.Id) || !IsAnniversaryGuoZhaoNativeEquipment(use, f) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == paid.CardId) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryGuoZhaoEquipmentReturnedEvent>().Any(e => e.FrameId == f.Id && e.Ordinal == paid.Ordinal))
            throw new InvalidOperationException("Zunwei must return one exact whole physical equipment Use before its next random choice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryGuoZhaoEquipmentReturnedEvent(f.Id, paid.Ordinal, use.Id, paid.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryGuoZhao = r with { Stage = AnniversaryGuoZhaoStage.EquipmentChildren, Cursor = r.Cursor + 1, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueGuoZhao(f.Id);
        return true;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 3400)]
    internal void CollectWindow_CollectIssuedAnniversaryGuoZhaoCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectIssuedAnniversaryGuoZhaoCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 700)]
    internal bool ExcludeOrdinaryMovement_IssuedGuoZhaoColor(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => candidate.SkillId == AnniversaryGuoZhaoComposition.Pianchong && candidate.BindingId == "lost-color-benefit";
    [ProgramIssuedTrigger(570, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger570(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsIssuedAnniversaryGuoZhaoCandidate(candidate) ? CanRunIssuedAnniversaryGuoZhaoCandidate(candidate, context) : null;
}
