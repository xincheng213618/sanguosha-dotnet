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

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27210)]
    internal void ObserveMiuyanPhaseHandReveal(IGameEvent fact)
    {
        if (!HasMiuyanCapability || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase || IsMiuyanShandaoMovementReveal(fact) || IsMiuyanCategoryGiftMovementReveal(fact))
            return;
        var(frame, ids) = fact switch
        {
            ProgramCardsRevealedEvent e => (e.FrameId, e.Cards.Select(c => c.Id)),
            CardsRevealedEvent e => (e.ResolutionId, e.Cards.Select(c => c.Id)),
            FireAttackCardRevealedEvent e => (e.ResolutionId, new[] { e.CardId }.AsEnumerable()),
            ProgramHandComparisonSourceRevealedEvent e => (e.FrameId, new[] { e.CardId }.AsEnumerable()),
            ProgramHandComparisonCardRevealedEvent e => (e.FrameId, new[] { e.CardId }.AsEnumerable()),
            ProgramHandDraftRevealedEvent e => (e.FrameId, MiuyanDraftHandIds(e)),
            ProgramQingliangRevealEvent e => (e.FrameId, MiuyanQingliangHandIds(e)),
            ProgramJuanxiaTrickUsedEvent { TrickKind: "FireAttack", CardId: > 0 } e => (e.FrameId, MiuyanJuanxiaHandIds(e)),
            _ => (0L, Enumerable.Empty<int>())};
        foreach (var id in ids.Distinct().ToArray())
            if (_cardZonesOperations._cardZones.GetLocation(id)is { Zone: CardZoneKind.Hand, OwnerSeat: { } holder } && _characterLuoXianOlOperations.CurrentHandEntry(holder, id)is { } entry && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MiuyanPhaseHandRevealedEvent>().Any(e => e.Phase == phase && e.CardId == id))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MiuyanPhaseHandRevealedEvent(phase, frame, id, holder, entry, fact.GetType().Name));
    }

    internal bool IsMiuyanShandaoMovementReveal(IGameEvent fact)
    {
        if (fact is not CardsRevealedEvent { Cards: [var card] } e || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == e.ResolutionId)is not { SkillId: "ol:shandao", ActivationId: "top-and-two-tricks", TriggerId: null, InstructionIndex: 1, TianChouShandao: { Stage: TianChouShandaoStage.TopChildren, TargetsFrozen: true } r } f || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveShandaoTopAndGlobalTricks || r.Source != CharacterTianChouOlRuntime.TianChouSource(f) || r.Cursor < 0 || r.Cursor >= r.Targets.Count)
            return false;
        return _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(r.Targets[r.Cursor]);
    }

    internal bool IsMiuyanCategoryGiftMovementReveal(IGameEvent fact)
    {
        if (fact is not ProgramCardsRevealedEvent { SkillId: "ol:zhaosong", BindingId: "give-hand-category-mark", Bind: "category-mark-gift", Cards: [var card] } e || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == e.FrameId)is not { SkillId: "ol:zhaosong", TriggerId: "give-hand-category-mark", InstructionIndex: 1, RecipientCategoryMark: { Operation: SkillProgramEffectOp.GiveHandAndGrantCategoryMark, Stage: RecipientCategoryMarkStage.ChoosingGift } r } f || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Operation || r.Source != CoreGameEngineRecipientCategoryMarksRuntime.RecipientMarkSource(f) || r.HolderSeat != e.OwnerSeat)
            return false;
        return r.Materials.Any(m => m.CardId == card.Id && m.From == CardLocation.Hand(r.HolderSeat)) && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(r.HolderSeat);
    }

    internal IEnumerable<int> MiuyanQingliangHandIds(ProgramQingliangRevealEvent e)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == e.FrameId)is not { } f || f.SkillId != e.SkillId || CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) != e.BindingId || f.OwnerSeat != e.OwnerSeat || f.CardSetBindings.SingleOrDefault(b => b.Name == "qingliang-reveal")is not { Visibility: SkillProgramCardSetVisibility.Public } set || set.CardIds.Count != e.CardCount)
            throw new InvalidOperationException("Miuyan lost Qingliang's exact original public Hand binding.");
        return set.CardIds;
    }

    internal IEnumerable<int> MiuyanDraftHandIds(ProgramHandDraftRevealedEvent e)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == e.FrameId)is not { PublicHandDraft: { Stage: 2 } draft } f || f.SkillId != e.SkillId || f.OwnerSeat != e.OwnerSeat || !draft.CardIds.SequenceEqual(e.CardIds) || !draft.RecipientSeats.SequenceEqual(e.RecipientSeats))
            throw new InvalidOperationException("Miuyan lost the native draft's exact original public Hand entities.");
        return e.CardIds;
    }

    internal IEnumerable<int> MiuyanJuanxiaHandIds(ProgramJuanxiaTrickUsedEvent e)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == e.FrameId)is not { JuanxiaLaunch: { Stage: JuanxiaStage.Fire, PendingKind: "FireAttack" } state } f || f.OwnerSeat != e.OwnerSeat || f.SkillId != e.SkillId || CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) != e.BindingId || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.JuanxiaDeclareTricks || state.TargetSeat != e.TargetSeat || state.RevealedCardId != e.CardId || !f.SelectedTargetSeats.SequenceEqual([state.TargetSeat]))
            throw new InvalidOperationException("Miuyan lost Juanxia's actual original public fire-reveal entity.");
        return[e.CardId];
    }

    internal IReadOnlyList<WangGuanHandEntity> MiuyanClaimEntities(MiuyanUseInvoice use, int owner) => Array.AsReadOnly(_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MiuyanPhaseHandRevealedEvent>().Where(e => e.Phase == use.Phase).Select(e => e.CardId).Distinct().Order().Select(id => (Id: id, From: _cardZonesOperations._cardZones.GetLocation(id))).Where(x => x.From is { Zone: CardZoneKind.Hand, OwnerSeat: { } holder } && holder != owner && _coreMatchParticipantsPlayersAccess._players[holder].IsAlive).Select(x => new WangGuanHandEntity(x.Id, _coreAdvancedLifecycleOperations.GetAdvancedCard(x.Id).Kind, x.From.OwnerSeat!.Value, _cardZonesOperations._cardZones.CardsAt(x.From).ToList().FindIndex(c => c.Id == x.Id), _characterLuoXianOlOperations.CurrentHandEntry(x.From.OwnerSeat!.Value, x.Id)!.Value)).ToArray());
    [ProgramIssuedTrigger(630, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger630(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsIssuedMiuyanCandidate(candidate) ? CanRunIssuedMiuyanCandidate(candidate, context) : null;
}
