#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaTieAnniversaryRuntime
{
    internal ProgramSkillFrame? MaTieQueryFrame(int actor)
    {
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        var parent = top switch
        {
            DrawFundedDistinctBasicFrame paid when IsAnniversaryMaTiePaymentFrame(paid) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == paid.Payment.RequestAcceptance!.Identity.FrameId),
            ChainedStateBasicFrame paid when IsAnniversaryMaTiePaymentFrame(paid) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == paid.Payment.RequestAcceptance!.Identity.FrameId),
            ProgramSkillFrame { EquipmentSlotNamedUse: { } slot }when AnniversaryMaTieSlotParent(slot)is { } original => original,
            _ => top
        };
        return parent is ProgramSkillFrame { AnniversaryMaTie: { Stage: AnniversaryMaTieStage.SlashChoice or AnniversaryMaTieStage.Issuing } } f &&
            _slashRequests.GetIdentity(f) is { } identity && identity.ActorSeat == actor &&
            (_slashRequests.GetOffer(f, actor) is not null || f.SlashRequest?.Acceptance is { } accepted &&
                _slashRequests.CanExecuteAcceptedUse(f, accepted)) ? f : null;
    }

    internal bool IsAnniversaryMaTieSlashQuery(CharacterState actor) => MaTieQueryFrame(actor.Seat)is not null;
    internal bool AnniversaryMaTieRequestActor(ProgramSkillFrame f, int actor) => f.AnniversaryMaTie is not null &&
        _slashRequests.GetIdentity(f) is { } identity && identity.ActorSeat == actor;
    internal int[] AnniversaryMaTieRequestTargets(ProgramSkillFrame f, int actor) => AnniversaryMaTieRequestActor(f, actor)
        ? _slashRequests.GetTargets(f, actor).ToArray() : [];
    internal IReadOnlyList<GameEngine.MaTieSlashPlan> MaTieSlashPlans(ProgramSkillFrame f)
    {
        var r = f.AnniversaryMaTie!;
        if (r.Stage != AnniversaryMaTieStage.SlashChoice || MaTieQueryFrame(r.Actors[r.Cursor])?.Id != f.Id)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actors[r.Cursor]];
        var plans = new List<GameEngine.MaTieSlashPlan>();
        foreach (var action in _coreNativeLegalActionsOperations.BuildLegalActions(actor, false).Where(a => a.Kind == LegalActionKind.Slash && a.MinCardCount == 0 && a.TargetSeats.Contains(r.Target)))
            plans.Add(new(AnniversaryMaTieComposition.Freeze(action), action.CardId is> 0 ? [action.CardId.Value] : []));
        var view = f with
        {
            AssistedSlashRequest = new(actor.Seat, r.Target)
        };
        foreach (var choice in _coreAssistedPhysicalCardsOperations.AssistedPhysicalSlashChoices(view).Where(c => c.Parameters.GetValueOrDefault("request-option") == "use" && c.Cards.Count > 1))
        {
            var(_, kind, source) = _coreAssistedSlashPaymentOperations.ReadAssistedSlashPayment(actor, choice);
            plans.Add(new(new(LegalActionKind.Slash, choice.Cards[0], r.Target, choice.Description, kind) { ConversionSource = source }, Array.AsReadOnly(choice.Cards.ToArray()), choice));
        }

        return Array.AsReadOnly(plans.ToArray());
    }

    internal void IssueMaTieSlash(ProgramSkillFrame f, int index, PendingDecision? originalDecision = null)
    {
        var r = f.AnniversaryMaTie!;
        originalDecision ??= r.OriginalDecision ?? _corePendingDecisionPolicyPipelineOperations._pendingDecision;
        if (originalDecision is null)
            throw new InvalidOperationException("A requested Slash must retain its true original response decision.");
        var plan = MaTieSlashPlans(f)[index];
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actors[r.Cursor]];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaTie = r with { Stage = AnniversaryMaTieStage.Issuing, SelectedAction = plan.Action, Materials = plan.Materials, Provider = actor.Seat, UseId = 0, ActionId = 0 } });
        if (plan.Multi is { } multi)
        {
            var(cards, kind, source) = _coreAssistedSlashPaymentOperations.ReadAssistedSlashPayment(actor, multi);
            var acceptance = _slashRequests.Accept(f, actor.Seat, r.Target);
            _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[r.Target], cards[0], kind, actor.Seat, physicalCards: cards, countsTowardSlashLimit: false, conversionSource: source, programSkillCardUseFrameId: f.Id, programSlashRequest: acceptance);
            return;
        }

        var identity = _slashRequests.GetIdentity(f) ?? throw new InvalidOperationException("Quxian lost its issued action request.");
        _coreNativeActionDispatchOperations.ExecuteAction(actor, plan.Action, programSlashRequestIdentity: identity, originalDecision: originalDecision);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 46700)]
    internal void BindAnniversaryMaTieSlash(CardUseDeclaredEvent fact)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId) is not
            { Action: { Type: CardActionType.Use } action, ProgramSlashRequestExecution: { } execution } use ||
            !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) ||
            _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == execution.ProgramFrameId) is not
                ProgramSkillFrame { AnniversaryMaTie: { } r } f || f.SkillId != AnniversaryMaTieComposition.Quxian)
            return;
        if (use.AnniversaryMaTieOrigin is not null)
        {
            if (!MaTieNativeMatches(use, f))
                throw new InvalidOperationException("Quxian native use changed its bound original request.");
            return;
        }
        var accepted = execution.Acceptance;
        if (r.Stage is not (AnniversaryMaTieStage.SlashChoice or AnniversaryMaTieStage.Issuing) ||
            r.Cursor < 0 || r.Cursor >= r.Actors.Count || r.UseId != 0 || execution.UseFrameId != use.Id ||
            execution.CardActionId != action.ActionId || accepted.Identity.IssuanceSlot != r.Cursor ||
            !_slashRequests.OwnsAcceptance(f, accepted, r.Actors[r.Cursor], r.Target))
            throw new InvalidOperationException("Quxian native use lost its actual original request occurrence.");
        var materials = action.PhysicalCards.Select(card => card.CardId).ToArray();
        var selected = new LegalAction(LegalActionKind.Slash, materials.FirstOrDefault(), r.Target, "驱险：使用杀", action.EffectiveKind)
        {
            TargetSeats = action.TargetSeats,
            ConversionSource = action.ConversionChain.FirstOrDefault(),
            AdditionalConversionSources = action.ConversionChain.Skip(1).ToArray()
        };
        var origin = new AnniversaryMaTieNativeOrigin(f.Id, r.Source, r.Hash, r.Cursor, use.Id, action.ActionId,
            accepted.Identity.ActorSeat, action.ProviderSeat, r.Target);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryMaTie = r with
        {
            Stage = AnniversaryMaTieStage.NativeUse, SelectedAction = selected, Materials = materials,
            Provider = action.ProviderSeat, UseId = use.Id, ActionId = action.ActionId
        } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { AnniversaryMaTieOrigin = origin, UnlimitedUse = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieSlashIssuedEvent(origin));
    }

    internal bool MaTieNativeMatches(CardUseFrame use, ProgramSkillFrame f) =>
        use.AnniversaryMaTieOrigin is { } origin && use.ProgramSlashRequestExecution is { } execution &&
        f.AnniversaryMaTie is { Stage: AnniversaryMaTieStage.NativeUse } receipt &&
        origin.FrameId == f.Id && execution.ProgramFrameId == f.Id && execution.UseFrameId == use.Id &&
        origin.UseId == use.Id && receipt.UseId == use.Id && origin.ActionId == receipt.ActionId &&
        use.Action is { Type: CardActionType.Use } action && action.ActionId == origin.ActionId &&
        execution.CardActionId == origin.ActionId && origin.Index == receipt.Cursor &&
        origin.Source == receipt.Source && origin.Source == execution.Acceptance.Identity.Source &&
        origin.Hash == receipt.Hash && origin.Hash == execution.Acceptance.Identity.GameplayHash &&
        origin.Target == receipt.Target && origin.Provider == receipt.Provider &&
        execution.Acceptance.Identity.IssuanceSlot == receipt.Cursor &&
        _slashRequests.OwnsAcceptance(f, execution.Acceptance, origin.Actor, origin.Target);
    [NativeUseValidation(46700)]
    internal void ValidateMaTieNative(CardUseFrame use)
    {
        if (use.AnniversaryMaTieOrigin is { } n && (use.Step != ResolutionFrameStep.Completed || !use.ProgramUseCommitted || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } f || !MaTieNativeMatches(use, f)))
            throw new InvalidOperationException("Quxian requires its exact wholly completed committed native use.");
    }

    [NativeUseReturn(46700)]
    internal void ReturnMaTieNative(CardUseFrame use)
    {
        if (use.AnniversaryMaTieOrigin is not { } n)
            return;
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == n.FrameId);
        var r = f.AnniversaryMaTie!;
        if (!MaTieNativeMatches(use, f) || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null)
            throw new InvalidOperationException("Quxian cannot repeat or substitute a native return.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryMaTie = r with { Stage = AnniversaryMaTieStage.NativeReturned, Used = r.Used.Contains(n.Actor) ? r.Used : r.Used.Append(n.Actor).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieSlashReturnedEvent(f.Id, n.Index, n.UseId, n.ActionId));
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && use.CardAttack is not { Active: true })
            ContinueMaTie(f.Id);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 46700)]
    internal bool ReturnMaTieAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryMaTie.Stage: AnniversaryMaTieStage.NativeReturned } f)
            return false;
        ContinueMaTie(f.Id);
        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 46701)]
    internal void ObserveMaTieDamage(DamageAppliedEvent e)
    {
        if (e.Amount <= 0 || e.SourceCardUseResolutionId is not { } useId || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { AnniversaryMaTieOrigin: { } n } use || e.TargetSeat != n.Target || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } parent || !MaTieNativeMatches(use, parent))
            return;
        var damage = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().LastOrDefault(d => d.TargetSeat == e.TargetSeat && d.Amount == e.Amount);
        if (damage is null)
            throw new InvalidOperationException("Quxian applied damage lost its actual owning damage frame.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { AnniversaryMaTie = parent.AnniversaryMaTie!with { TargetDamaged = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaTieTargetDamagedEvent(parent.Id, useId, damage.Id, n.Target, e.Amount));
    }
}
