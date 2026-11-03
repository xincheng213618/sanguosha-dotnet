#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangGuanOlRuntime
{
    internal bool HasMiuyanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMiuyanCompletedUse);

    internal void IssueMiuyanUse(long useId, CardActionContext? action)
    {
        if (!HasMiuyanCapability || action is not { Type: CardActionType.Use, EffectiveKind: CardKind.FireAttack } || action.ConversionChain.SingleOrDefault(s => s.SkillId == "ol:miuyan" && s.BindingId == "black-fire-attack")is not { } source)
            return;
        var program = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!;
        WangGuanComposition.ValidateProgram(source.SkillId, program);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { } use || use.MiuyanUse is not null || use.Action?.ActionId != action.ActionId || use.SourceSeat != action.ActorSeat || source.OwnerSeat != action.ActorSeat || action.ProviderSeat != action.ActorSeat || action.PhysicalCards is not [var material] || material.From.OwnerSeat != source.OwnerSeat || material.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.WoodenOxGrain) || _cardZonesOperations._cardZones.GetLocation(material.CardId) != material.From || _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId))is not (Suit.Spade or Suit.Club) || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], source.SkillId, source.SkillInstanceId) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MiuyanUseIssuedEvent>().Any(e => e.Invoice.UseId == useId))
            throw new InvalidOperationException("Miuyan lost its unique real accepted black Fire Attack material or actual phase.");
        var previous = _skillRuntimeStateOperations._skillRuntimeState.GetConversionState(source.OwnerSeat, source.SkillId);
        var current = _skillRuntimeStateOperations._skillRuntimeState.ToggleConversionState(source.OwnerSeat, source.SkillId);
        var invoice = new MiuyanUseInvoice(useId, action.ActionId, source, program.GameplayHash, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber, phase, previous, current, action.ActorSeat, action.ProviderSeat, material, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { MiuyanUse = invoice });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MiuyanUseIssuedEvent(invoice));
    }

    internal bool MiuyanUseProof(CardUseFrame use)
    {
        if (use.MiuyanUse is not { } r || use.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.FireAttack } action || r.UseId != use.Id || r.ActionId != action.ActionId || use.CardKind != CardKind.FireAttack || use.CardId != r.Material.CardId || use.PhysicalCardIds is not [var id] || id != r.Material.CardId || action.PhysicalCards is not [var cost] || cost != r.Material || action.ProviderSeat != r.ProviderSeat || !action.ConversionChain.Contains(r.Source) || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, r.OriginalActor, r.ProviderSeat) || r.Source.OwnerSeat != r.OriginalActor || r.Source.SkillId != "ol:miuyan" || r.Source.BindingId != "black-fire-attack" || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash != r.GameplayHash || r.ActualTurn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.RoundNumber != _coreActualRoundProgressRoundNumberAccess._roundNumber || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) || r.Phase.TurnNumber != r.ActualTurn || r.Phase.ActualTurnOwnerSeat != r.TurnOwner || !Enum.IsDefined(r.PreviousState) || r.CurrentState == (r.PreviousState) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MiuyanUseIssuedEvent>().Count(e => e.Invoice == r) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MiuyanUseIssuedEvent>().Count(e => e.Invoice.UseId == use.Id) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.SourceSeat == r.OriginalActor && e.CardId == r.Material.CardId && e.CardKind == CardKind.FireAttack) != 1)
            return false;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.BeforePayment && m.CardId == r.Material.CardId && m.CardKind == r.Material.CardKind && m.From == r.Material.From && (m.To == CardLocation.Processing || m.To == CardLocation.OutsideGame && _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon) && m.Reason == CardMoveReasons.Use) == 1;
    }

    internal CardUseFrame? MiuyanCandidateUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsIssuedMiuyanCandidate(c) || context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.OwnerSeat != c.OwnerSeat || context.CardUse is not { } card || _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is not { Step: ResolutionFrameStep.Completed, MiuyanUse: { } r, Action: { } action } use || !MiuyanUseProof(use) || card.CardActionId != r.ActionId || r.Source.OwnerSeat != c.OwnerSeat || r.Source.SkillId != c.SkillId || r.Source.SkillInstanceId != c.SkillInstanceId || r.GameplayHash != c.GameplayHash || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != use.Id || window.Action.ActionId != action.ActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return null;
        var current = window.Candidates[window.CandidateIndex];
        return current.OwnerSeat == c.OwnerSeat && current.SkillId == c.SkillId && current.TriggerId == c.BindingId && current.SkillInstanceId == c.SkillInstanceId && current.GameplayHash == c.GameplayHash && _coreCardActionsOperations.CreateCardActionProgramContext2(window, current) == context && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) == 1 ? use : null;
    }

    internal bool IsIssuedMiuyanCandidate(ProgramTriggerCandidate c) => HasMiuyanCapability && c.SkillId == "ol:miuyan" && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.Triggers.Any(t => t.Id == c.BindingId && t.Effects is [{ Op: SkillProgramEffectOp.ResolveMiuyanCompletedUse }]) == true;
    internal bool CanRunIssuedMiuyanCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => MiuyanCandidateUse(c, context)is { } use && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WangGuanStartedEvent>().Any(e => e.Op == SkillProgramEffectOp.ResolveMiuyanCompletedUse && e.NativeId == use.Id);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 14)]
    internal void CollectIssuedMiuyanCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { MiuyanUse: { } r } use || action.ActionId != r.ActionId || !MiuyanUseProof(use))
            return;
        var t = _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program!.Triggers.Single();
        if (result.Any(c => c.OwnerSeat == r.Source.OwnerSeat && c.SkillId == r.Source.SkillId && c.SkillInstanceId == r.Source.SkillInstanceId && c.TriggerId == t.Id))
            return;
        result.Add(new(r.Source.OwnerSeat, r.Source.OwnerSeat, r.Source.SkillId, t.Id, r.GameplayHash, r.Source.SkillInstanceId, t.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, r.Source.OwnerSeat, r.Source.OwnerSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat], action))));
    }

    internal IReadOnlyList<WangGuanHandEntity> MiuyanClaimEntities(MiuyanUseInvoice use, int owner) => Array.AsReadOnly(
        (ProgramPublicRevealCards.Read(_publicHandRevealHistory.Read(), "actualPhase", "explicit",
            use.ActualTurn, use.TurnOwner, use.Phase) ?? [])
        .Select(card => card.CardId).Order().Select(id => (Id: id, From: _cardZonesOperations._cardZones.GetLocation(id)))
        .Where(x => x.From is { Zone: CardZoneKind.Hand, OwnerSeat: { } holder } && holder != owner && _coreMatchParticipantsPlayersAccess._players[holder].IsAlive)
        .Select(x => new WangGuanHandEntity(x.Id, _coreAdvancedLifecycleOperations.GetAdvancedCard(x.Id).Kind, x.From.OwnerSeat!.Value,
            _cardZonesOperations._cardZones.CardsAt(x.From).ToList().FindIndex(c => c.Id == x.Id),
            _currentHandEntries.CurrentHandEntry(x.From.OwnerSeat!.Value, x.Id)!.Value)).ToArray());
    [ProgramIssuedTrigger(630, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger630(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsIssuedMiuyanCandidate(candidate) ? CanRunIssuedMiuyanCandidate(candidate, context) : null;
}
