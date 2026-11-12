#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiMouMobileRuntime
{
    internal IReadOnlyList<GameEngine.MouJwSlashPlan> MouJwSlashPlans(ProgramSkillFrame f)
    {
        var r = f.MouJiangWei!;
        if (r.Stage != MouJiangWeiStage.Responding || r.Cursor >= r.Targets.Count || !_coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]];
        var result = new List<GameEngine.MouJwSlashPlan>();
        foreach (var a in _coreNativeLegalActionsOperations.BuildLegalActions(actor, false).Where(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.Count > 0 && a.TargetSeats[0] == f.OwnerSeat && a.MinCardCount == 0))
        {
            var frozen = a with
            {
                TargetSeats = Array.AsReadOnly(a.TargetSeats.ToArray()),
                AdditionalConversionSources = Array.AsReadOnly((a.AdditionalConversionSources ?? []).ToArray()),
                SelectableCardIds = Array.AsReadOnly(a.SelectableCardIds.ToArray()),
                SelectableTargetSeats = Array.AsReadOnly(a.SelectableTargetSeats.ToArray())
            };
            result.Add(new(frozen, a.CardId is> 0 ? [a.CardId.Value] : []));
        }

        // Reuse the native conversion inventory and exact payment decoder, without installing
        // an assisted-request draft on a differently owned instruction.
        var view = f with
        {
            AssistedSlashRequest = new(actor.Seat, f.OwnerSeat)
        };
        foreach (var c in _coreAssistedPhysicalCardsOperations.AssistedPhysicalSlashChoices(view).Where(c => c.Parameters.GetValueOrDefault("request-option") == "use" && c.Cards.Count > 1))
        {
            var(_, kind, source) = _coreAssistedSlashPaymentOperations.ReadAssistedSlashPayment(actor, c);
            result.Add(new(new(LegalActionKind.Slash, c.Cards[0], f.OwnerSeat, c.Description, kind) { ConversionSource = source }, Array.AsReadOnly(c.Cards.ToArray()), c));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void IssueMouJwSlash(ProgramSkillFrame f, int index, PendingDecision? originalDecision = null)
    {
        var r = f.MouJiangWei!;
        originalDecision ??= _corePendingDecisionPolicyPipelineOperations._pendingDecision;
        if (originalDecision is null)
            throw new InvalidOperationException("A requested Slash must retain its true original response decision.");
        var plan = MouJwSlashPlans(f)[index];
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]];
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouJiangWei = r with { Stage = MouJiangWeiStage.Issuing, NativeAction = plan.Action, Materials = plan.Materials, UseId = 0, ActionId = 0, ProviderSeat = actor.Seat } });
        if (plan.Multi is { } multi)
        {
            var(cards, kind, source) = _coreAssistedSlashPaymentOperations.ReadAssistedSlashPayment(actor, multi);
            _slashRequests.Accept(f, actor.Seat, f.OwnerSeat);
            _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], cards[0], kind, actor.Seat, physicalCards: cards, countsTowardSlashLimit: false, conversionSource: source, programSkillCardUseFrameId: f.Id);
        }
        else
            _coreNativeActionDispatchOperations.ExecuteAction(actor, plan.Action, programSlashRequestIdentity: _slashRequests.GetIdentity(f), originalDecision: originalDecision);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 33201)]
    internal void BindMouJwSlash(CardUseDeclaredEvent fact)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId) is not { Action: { Type: CardActionType.Use } a, ProgramSlashRequestExecution: { } execution } use ||
            !CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) ||
            _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == execution.ProgramFrameId) is not { MouJiangWei: { Accepted: true, Stage: MouJiangWeiStage.Responding or MouJiangWeiStage.Issuing or MouJiangWeiStage.FactionRequest } r } parent)
            return;
        var acceptance = execution.Acceptance;
        if (r.Cursor >= r.Targets.Count || !_slashRequests.OwnsAcceptance(parent, acceptance, r.Targets[r.Cursor], parent.OwnerSeat))
            throw new InvalidOperationException("Tiaoxin lost its original accepted responder and request occurrence.");
        var origin = new MouJiangWeiNativeOrigin(parent.Id, r.Source, r.GameplayHash, r.Cursor, use.Id, a.ActionId,
            acceptance.Identity.ActorSeat, acceptance.TargetSeat) { ProviderSeat = a.ProviderSeat };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MouJiangWei = r with
        {
            Stage = MouJiangWeiStage.NativeUse, UseId = use.Id, ActionId = a.ActionId,
            Materials = a.PhysicalCards.Select(cost => cost.CardId).ToArray(), ProviderSeat = a.ProviderSeat
        } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouJiangWeiRequest = origin });
        if (r.FactionPolicy is not null && a.ProviderSeat != acceptance.Identity.ActorSeat)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiFactionProvidedEvent(parent.Id, r.Cursor,
                acceptance.Identity.ActorSeat, a.ProviderSeat, a.EffectiveKind, a.PhysicalCards.Count));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiSlashIssuedEvent(origin));
    }

    internal bool ValidMouJwUse(CardUseFrame use, ProgramSkillFrame f) =>
        use.MouJiangWeiRequest is { } n && f.MouJiangWei is { Stage: MouJiangWeiStage.NativeUse } r &&
        n.FrameId == f.Id && n.Source == r.Source && n.GameplayHash == r.GameplayHash && n.Index == r.Cursor &&
        n.UseId == use.Id && n.UseId == r.UseId && n.ActionId == r.ActionId && use.Action is { Type: CardActionType.Use } a &&
        a.ActionId == n.ActionId && a.ProviderSeat == n.ProviderSeat && n.ProviderSeat == r.ProviderSeat &&
        n.ActorSeat == r.Targets[r.Cursor] && n.TargetSeat == f.OwnerSeat &&
        a.PhysicalCards.Select(c => c.CardId).SequenceEqual(r.Materials) && use.ProgramSlashRequestExecution is { } execution &&
        execution.ProgramFrameId == f.Id && _slashRequests.OwnsAcceptance(f, execution.Acceptance, n.ActorSeat, n.TargetSeat);
    [NativeUseValidation(33200)]
    internal void ValidateMouJwReturn(CardUseFrame use)
    {
        if (use.MouJiangWeiRequest is { } n && (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } f || !ValidMouJwUse(use, f)))
            throw new InvalidOperationException("Tiaoxin must return its exact completed native Slash.");
    }

    [NativeUseReturn(33200)]
    internal void ReturnMouJwSlash(CardUseFrame use)
    {
        if (use.MouJiangWeiRequest is not { } n)
            return;
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == n.FrameId);
        if (!ValidMouJwUse(use, f) || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null)
            throw new InvalidOperationException("Tiaoxin cannot repeat a native return or pay again.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouJiangWei = f.MouJiangWei!with { Stage = MouJiangWeiStage.NativeReturned } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiSlashReturnedEvent(f.Id, n.Index, n.UseId, n.ActionId));
        if (use.CardAttack is not { Active: true })
            ContinueMouJw(f.Id);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 33200)]
    internal bool ResumeMouJwAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouJiangWei: { Stage: MouJiangWeiStage.NativeReturned } } f)
            return false;
        ContinueMouJw(f.Id);
        return true;
    }
}
