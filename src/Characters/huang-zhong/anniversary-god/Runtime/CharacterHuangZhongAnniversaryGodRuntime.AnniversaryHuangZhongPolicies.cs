#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangZhongAnniversaryGodRuntime
{
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 39600)]
    internal int AnniversaryHzDijiDamage(GameEngine.IDamageAttempt a) => ActiveAnniversaryHzBody(a.TargetSeat, AnniversaryHzBody.Diji).Count();
    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 39600)]
    internal bool ObserveAnniversaryHzApplied(GameEngine.IDamageAttempt a, long damageId)
    {
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == damageId)is not DamageFrame { Amount: > 0 } damage)
            return false;
        foreach (var status in ActiveAnniversaryHzBody(a.TargetSeat, AnniversaryHzBody.Diji).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzStatusConsumedEvent(status.Id, "damage", damageId));
        if (!a.IsSourceLess && ActiveAnniversaryHzZhanjue(a.SourceSeat, 2)is { } issue)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzZhanjueSpentEvent(issue.Id, damageId, damage.Amount));
        return false;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 39600)]
    internal void ObserveAnniversaryHzExpiryAndSuit(IGameEvent payload)
    {
        if (!HasAnniversaryHuangZhongCapability)
            return;
        if (payload is CardActionAcceptedEvent { Action.Type: CardActionType.Use or CardActionType.Response, Action.EffectiveSuit: Suit.Heart } accepted && ActiveAnniversaryHzBody(accepted.Action.ActorSeat, AnniversaryHzBody.Qihai).Any())
            throw new InvalidOperationException("Qihai prohibits the actual effective Heart use or response.");
        int? target = payload switch
        {
            TurnEndedEvent e => e.ActorSeat,
            PlayerDiedEvent e => e.VictimSeat,
            _ => null
        };
        if (target is null)
            return;
        foreach (var issue in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryHzBodyIssuedEvent>().Where(e => e.Target == target && e.Body is AnniversaryHzBody.Diji or AnniversaryHzBody.Zhongshu or AnniversaryHzBody.Qihai && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryHzStatusExpiredEvent>().Any(x => x.Id == e.Id)).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzStatusExpiredEvent(issue.Id, target.Value, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 39600)]
    internal void ObserveAnniversaryHzActualUse(IGameEvent payload)
    {
        if (!HasAnniversaryHuangZhongCapability)
            return;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { } use)
        {
            if (use.Action is { } action)
                IssueAnniversaryHzActualUse(action, use.Id);
            else if (_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.OriginFrameId == use.Id && e.CardActionId is null))
                IssueAnniversaryHzLegacyUse(use);
        }
        else if (payload is CardActionAcceptedEvent accepted && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.CardActionId == accepted.Action.ActionId))
        {
            var a = accepted.Action;
            var parent = a.Type == CardActionType.Use ? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Action?.ActionId == a.ActionId)?.Id : a.EffectiveKind == CardKind.Nullification ? _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow?.Id : _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Action?.ActionId == a.ParentActionId && f.CardAttack?.TargetSeat == a.ActorSeat)?.Id;
            if (parent is { } id)
                IssueAnniversaryHzActualUse(a, id);
        }
    }

    internal void IssueAnniversaryHzActualUse(CardActionContext a, long parent)
    {
        if (a.Type == CardActionType.Use && CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) && ActiveAnniversaryHzZhanjue(a.ActorSeat, 1)is { } issue && _coreCardUseLifecycleOperations.LifecycleCardUse(parent)is { Action: { } current } use && current.ActionId == a.ActionId)
        {
            var noResponse = new IssuedCardNoResponse(a.ActionId, parent, issue.Source, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(parent, f => f with { AnniversaryHzSlashIssueId = issue.Id, IssuedNoResponse = f.IssuedNoResponse ?? noResponse });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzZhanjueSpentEvent(issue.Id, parent, 0));
            if (use.IssuedNoResponse is null)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IssuedCardNoResponseEvent(noResponse));
        }

        var statuses = ActiveAnniversaryHzBody(a.ActorSeat, AnniversaryHzBody.Zhongshu).ToArray();
        if (statuses.Length == 0)
            return;
        var proof = new AnniversaryHzInvalidation(parent, a)
        {
            StatusIds = statuses.Select(s => s.Id).ToArray()
        };
        foreach (var s in statuses)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzStatusConsumedEvent(s.Id, "use", a.ActionId));
        if (a.Type == CardActionType.Use)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(parent, f => f with { AnniversaryHzInvalidation = proof });
        else if (a.EffectiveKind == CardKind.Dodge)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(parent, f => f with { AnniversaryHzResponses = (f.AnniversaryHzResponses ?? []).Append(proof).ToArray() });
        else if (a.EffectiveKind == CardKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } counter && counter.Id == parent && counter.NamedAmbushResponse is not null)
            _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(counter with { AnniversaryHzInvalidation = proof });
        else
            throw new InvalidOperationException("Zhongshu requires its exact true-use native response parent.");
    }

    internal void IssueAnniversaryHzLegacyUse(CardUseFrame use)
    {
        var statuses = ActiveAnniversaryHzBody(use.SourceSeat, AnniversaryHzBody.Zhongshu).ToArray();
        if (statuses.Length == 0)
            return;
        var proof = new AnniversaryHzInvalidation(use.Id, null, use.SourceSeat, use.CardKind)
        {
            StatusIds = statuses.Select(s => s.Id).ToArray()
        };
        foreach (var s in statuses)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzStatusConsumedEvent(s.Id, "legacy-use", use.Id));
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { AnniversaryHzInvalidation = proof });
    }

    internal bool ValidAnniversaryHzInvalidation(AnniversaryHzInvalidation proof, CardActionContext? action, long parent)
    {
        if (proof.ParentId != parent || proof.StatusIds.Count == 0 || proof.StatusIds.Distinct().Count() != proof.StatusIds.Count)
            return false;
        var legacy = proof.Action is null;
        var actor = legacy ? proof.Actor : proof.Action!.ActorSeat;
        if (legacy ? action is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(parent)is not { Action: null } use || use.SourceSeat != actor || use.CardKind != proof.LegacyKind || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use) : action is null || !SameAnniversaryHzAction(proof.Action!, action))
            return false;
        return proof.StatusIds.All(id => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryHzBodyIssuedEvent>().Count(s => s.Id == id && s.Target == actor && s.Body == AnniversaryHzBody.Zhongshu) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryHzStatusConsumedEvent>().Count(s => s.Id == id && s.Reason == (legacy ? "legacy-use" : "use") && s.Origin == (legacy ? parent : action!.ActionId)) == 1);
    }

    internal static bool SameAnniversaryHzAction(CardActionContext a, CardActionContext b) => (a.ActionId, a.ParentActionId, a.Type, a.ActorSeat, a.ProviderSeat, a.RequesterSeat, a.ResponderSeat, a.OpponentSeat, a.EffectiveKind, a.EffectiveSuit, a.EffectiveRank) == (b.ActionId, b.ParentActionId, b.Type, b.ActorSeat, b.ProviderSeat, b.RequesterSeat, b.ResponderSeat, b.OpponentSeat, b.EffectiveKind, b.EffectiveSuit, b.EffectiveRank) && a.PhysicalCards.SequenceEqual(b.PhysicalCards) && a.ConversionChain.SequenceEqual(b.ConversionChain);
    internal bool IsAnniversaryHzUseIneffective(CardUseFrame use)
    {
        if (use.AnniversaryHzInvalidation is not { } proof)
            return false;
        if (!ValidAnniversaryHzInvalidation(proof, use.Action, use.Id))
            throw new InvalidOperationException("Zhongshu lost its exact issued whole-use proof.");
        return true;
    }

    internal bool HasAnniversaryHzIneffectiveResponse(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        if (continuation != ProgramCardContinuation.Dodge || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.AnniversaryHzResponses is not { } proofs)
            return false;
        var matches = proofs.Where(p => p.Action?.ActionId == action.ActionId).ToArray();
        if (matches.Length == 0)
            return false;
        if (action.ParentActionId != _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action?.ActionId || action.EffectiveKind != CardKind.Dodge || action.ActorSeat != attack.TargetSeat || matches.Any(p => !ValidAnniversaryHzInvalidation(p, action, attack.ResolutionId)))
            throw new InvalidOperationException("Zhongshu's ineffective Dodge changed its true response parent.");
        return true;
    }

    internal bool ContinueAnniversaryHzIneffectiveDodge(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        if (!HasAnniversaryHzIneffectiveResponse(attack, action, continuation))
            return false;
        _coreRoundOnceCardlessNativeResponsesOperations.ClearRoundOnceCardlessDodgeResponse(attack);
        _coreTieredRoundZeroResponsesOperations.ClearTieredRoundDodgeResponse(attack);
        _coreChainedStateBasicReturnsOperations.ClearChainedStateBasicDodgeResponse(attack);
        _coreDrawFundedDistinctBasicResponsesOperations.Value.ClearDrawFundedDistinctBasicDodgeResponse(attack);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        return true;
    }

    internal bool AnniversaryHzCounterspellInvalidated(NullificationWindowFrame pending, CardActionContext action)
    {
        if (pending.AnniversaryHzInvalidation is not { } proof)
            return false;
        if (pending.NamedAmbushResponse?.ActionId != action.ActionId || !ValidAnniversaryHzInvalidation(proof, action, pending.Id))
            throw new InvalidOperationException("Zhongshu's ineffective Nullification lost its exact paid chain node.");
        return true;
    }

    internal bool TryAnniversaryHzEquipmentCommitted(long id, Card equipment)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { } use || !HasAnniversaryHzPendingInvalidation(use.SourceSeat) && use.AnniversaryHzInvalidation is null)
            return false;
        var continuation = new ProgramSimpleCardContinuation(equipment.Id, SimpleCardUseEffect.EquipmentPlacement);
        if (_coreCardUseLifecycleOperations.TryBeginCommittedCardUse(id, ProgramCardContinuation.CommittedSimpleCard, simple: continuation))
            return true;
        return ContinueAnniversaryHzIneffectiveSimple(id, continuation);
    }

    internal bool ContinueAnniversaryHzIneffectiveSimple(long id, ProgramSimpleCardContinuation continuation)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { } use || !IsAnniversaryHzUseIneffective(use))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Zhongshu lost its original ineffective simple Use return.");
        foreach (var card in _coreNativeTrickUseOperations.GetCardUsePhysicalCards(id).Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Processing))
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, new("skill.anniversary-huang-zhong.zhongshu.cleanup"));
        var original = use.CardId == 0 ? new Card(0, use.CardKind, Suit.None, 0) : _coreAdvancedLifecycleOperations.GetAdvancedCard(use.CardId);
        _coreNativeCardUseCompletionOperations.FinishCardUse(id, original, use.CardKind);
        return true;
    }

    internal bool AnniversaryHzHeartForbidden(int actor, IEnumerable<int> ids)
    {
        if (!ActiveAnniversaryHzBody(actor, AnniversaryHzBody.Qihai).Any())
            return false;
        var suits = ids.Distinct().Select(id => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[actor], _coreAdvancedLifecycleOperations.GetAdvancedCard(id))).Distinct().ToArray();
        return suits is [Suit.Heart];
    }

    [ProgramCardLegality(ProgramCardLegalityStage.MaterialUseBan, 39600)]
    internal bool AnniversaryHzMaterialBan(ProgramCardMaterialQuery q) => AnniversaryHzHeartForbidden(q.ActorSeat, q.CardIds);
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 39600)]
    internal bool AnniversaryHzResponseBan(ProgramNativeResponseQuery q) => AnniversaryHzHeartForbidden(q.Provider.Seat, q.CardIds);
    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 39600)]
    internal bool AnniversaryHzProjectedBan(ProgramLegalActionQuery q) => q.Action.Kind != LegalActionKind.Recast && q.Action.CardId is { } id && AnniversaryHzHeartForbidden(q.Actor.Seat, [id]);
    [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 39600)]
    internal void AnniversaryHzSubmittedBan(ProgramSubmittedUseQuery q)
    {
        if (AnniversaryHzHeartForbidden(q.ActorSeat, q.CardIds))
            throw new InvalidOperationException("Qihai prohibits effective Heart use materials.");
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 39600)]
    internal bool AnniversaryHzUseDistance(ProgramCardDistanceQuery q) => CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && (q.CardUseFrameId is { } id ? _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.AnniversaryHzSlashIssueId is not null : ActiveAnniversaryHzZhanjue(q.Actor.Seat, 1)is not null);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 39600)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 39600)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 39600)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 39600)]
    internal bool AnniversaryHzSlashDistance(ProgramSlashDistanceQuery q) => CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && (q.ExistingUseFrameId is { } id ? _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.AnniversaryHzSlashIssueId is not null : ActiveAnniversaryHzZhanjue(q.Actor.Seat, 1)is not null);
}
