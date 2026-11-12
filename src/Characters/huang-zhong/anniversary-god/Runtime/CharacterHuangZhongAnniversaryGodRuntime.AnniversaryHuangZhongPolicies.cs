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
            else if (_nativeCardUseAdmission.IsLegacyProducerUse(use) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.OriginFrameId == use.Id && e.CardActionId is null))
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
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(parent, f => f with { AnniversaryHzSlashIssueId = issue.Id });

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzZhanjueSpentEvent(issue.Id, parent, 0));
            _coreCardUseLifecycleOperations.IssueNoResponsePolicy(noResponse);
        }

        var statuses = ActiveAnniversaryHzBody(a.ActorSeat, AnniversaryHzBody.Zhongshu).ToArray();
        if (statuses.Length == 0)
            return;
        foreach (var s in statuses)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzStatusConsumedEvent(s.Id, "use", a.ActionId));
        if (a.Type == CardActionType.Use)
            _nativeCardActionInvalidations.IssueUse(parent, a, new(SimpleCancellation: new(
                NativeSimpleCardCancellationBoundary.BeforeTargetEffects, Enum.GetValues<SimpleCardUseEffect>(),
                new("skill.anniversary-huang-zhong.zhongshu.cleanup"), Priority: 30)));
        else if (a.EffectiveKind == CardKind.Dodge)
            _nativeCardActionInvalidations.IssueDodge(parent, a);
        else if (a.EffectiveKind == CardKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } counter && counter.Id == parent && counter.NamedAmbushResponse is not null)
            _nativeCardActionInvalidations.IssueCounterspell(counter.Id, a);
        else
            throw new InvalidOperationException("Zhongshu requires its exact true-use native response parent.");
    }

    internal void IssueAnniversaryHzLegacyUse(CardUseFrame use)
    {
        var statuses = ActiveAnniversaryHzBody(use.SourceSeat, AnniversaryHzBody.Zhongshu).ToArray();
        if (statuses.Length == 0)
            return;
        foreach (var s in statuses)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHzStatusConsumedEvent(s.Id, "legacy-use", use.Id));
        _nativeCardActionInvalidations.IssueUse(use.Id, null, new(SimpleCancellation: new(
            NativeSimpleCardCancellationBoundary.BeforeTargetEffects, Enum.GetValues<SimpleCardUseEffect>(),
            new("skill.anniversary-huang-zhong.zhongshu.cleanup"), Priority: 30)));
    }

    internal bool TryAnniversaryHzEquipmentCommitted(long id, Card equipment)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { } use || !HasAnniversaryHzPendingInvalidation(use.SourceSeat) && !_nativeCardActionInvalidations.IsUseIneffective(use))
            return false;
        var continuation = new ProgramSimpleCardContinuation(equipment.Id, SimpleCardUseEffect.EquipmentPlacement);
        if (_coreCardUseLifecycleOperations.TryBeginCommittedCardUse(id, ProgramCardContinuation.CommittedSimpleCard, simple: continuation))
            return true;
        return _nativeCardActionInvalidationExecution.TryCompleteSimple(id, continuation, NativeSimpleCardCancellationBoundary.BeforeTargetEffects);
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
