#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiMouMobileRuntime
{
    internal ProgramSkillFrame? MouJwRequestFrame(int actor) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.MouJiangWei is { Op: SkillProgramEffectOp.ResolveMouJiangWeiTiaoxin, Accepted: true, Stage: MouJiangWeiStage.Responding or MouJiangWeiStage.Issuing or MouJiangWeiStage.FactionRequest } r && r.Cursor < r.Targets.Count && r.Targets[r.Cursor] == actor && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && ValidMouJw(f));
    internal bool IsMouJwSlashQuery(CharacterState actor) => MouJwRequestFrame(actor.Seat)is not null;
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 33200)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 33200)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 33200)]
    internal bool MouJwRequestedDistance(ProgramSlashDistanceQuery q) => MouJwRequestFrame(q.Actor.Seat)is { } f && q.Target.Seat == f.OwnerSeat;
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 33200)]
    internal bool MouJwRequestedCardDistance(ProgramCardDistanceQuery q) => CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && MouJwRequestFrame(q.Actor.Seat)is { } f && q.Target.Seat == f.OwnerSeat;
    // Native requested Slash is a true Use, with the same ordinary-quota treatment as classic Tiaoxin.
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 33200)]
    internal bool MouJwRequestedQuota(ProgramSlashQuotaQuery q) => MouJwRequestFrame(q.Actor.Seat)is { } f && q.Target.Seat == f.OwnerSeat;
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

    internal void IssueMouJwSlash(ProgramSkillFrame f, int index)
    {
        var r = f.MouJiangWei!;
        var plan = MouJwSlashPlans(f)[index];
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]];
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouJiangWei = r with { Stage = MouJiangWeiStage.Issuing, NativeAction = plan.Action, Materials = plan.Materials, UseId = 0, ActionId = 0, ProviderSeat = actor.Seat } });
        if (plan.Multi is { } multi)
        {
            var(cards, kind, source) = _coreAssistedSlashPaymentOperations.ReadAssistedSlashPayment(actor, multi);
            _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], cards[0], kind, actor.Seat, physicalCards: cards, countsTowardSlashLimit: false, conversionSource: source, programSkillCardUseFrameId: f.Id);
        }
        else
            _coreNativeActionDispatchOperations.ExecuteAction(actor, plan.Action);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 33201)]
    internal void BindMouJwSlash(CardUseDeclaredEvent fact)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.MouJiangWei is { Stage: MouJiangWeiStage.Issuing })is not { MouJiangWei: { } r } parent)
            return;
        if (!ValidMouJw(parent) || r.NativeAction is not { } plan || r.Cursor >= r.Targets.Count || a.ActorSeat != r.Targets[r.Cursor] || a.ProviderSeat != r.ProviderSeat || a.RequesterSeat != (r.ProviderSeat == a.ActorSeat ? (int? )null : a.ActorSeat) || !a.TargetSeats.SequenceEqual(plan.TargetSeats) || !a.PhysicalCards.Select(c => c.CardId).SequenceEqual(r.Materials) || a.EffectiveKind != (plan.PlayedCardKind ?? (plan.CardId is> 0 ? _corePlayCardHistoryOperations.ApplyProgramUseAppearance(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(plan.CardId.Value), plan.ConversionSource).Kind : CardKind.Slash)))
            throw new InvalidOperationException("Tiaoxin changed its requested actual actor, source materials or accepted Slash plan.");
        var origin = new MouJiangWeiNativeOrigin(parent.Id, r.Source, r.GameplayHash, r.Cursor, use.Id, a.ActionId, a.ActorSeat, parent.OwnerSeat)
        {
            ProviderSeat = a.ProviderSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MouJiangWei = r with { Stage = MouJiangWeiStage.NativeUse, UseId = use.Id, ActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouJiangWeiRequest = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiSlashIssuedEvent(origin));
    }

    internal bool ValidMouJwUse(CardUseFrame use, ProgramSkillFrame f) => use.MouJiangWeiRequest is { } n && f.MouJiangWei is { Stage: MouJiangWeiStage.NativeUse } r && n.FrameId == f.Id && n.Source == r.Source && n.GameplayHash == r.GameplayHash && n.Index == r.Cursor && n.UseId == use.Id && n.UseId == r.UseId && n.ActionId == r.ActionId && use.Action is { Type: CardActionType.Use } a && a.ActionId == n.ActionId && a.ProviderSeat == n.ProviderSeat && n.ProviderSeat == r.ProviderSeat && a.RequesterSeat == (n.ProviderSeat == n.ActorSeat ? (int? )null : n.ActorSeat) && n.ActorSeat == r.Targets[r.Cursor] && n.TargetSeat == f.OwnerSeat && a.PhysicalCards.Select(c => c.CardId).SequenceEqual(r.Materials) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouJiangWeiSlashIssuedEvent>().Count(e => e.Origin == n) == 1;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 33200)]
    internal bool MouJwRequestedNoDebit(long id, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouJiangWeiRequest: { } n } u && n.ActorSeat == actor && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is { } p && ValidMouJwUse(u, p);
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
        if (!ValidMouJwUse(use, f) || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouJiangWeiSlashReturnedEvent>().Any(e => e.UseId == use.Id))
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
