#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardSupplyCompletionRuntime
{
    internal bool TryBeginProvidedCardSupplyCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? request)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerWindow(SkillProgramTriggerWindow.CardSupplyCompleted))
            return false;
        if (request is null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Id != attack.ResolutionId || use.CompletedCardSupplyFrameId is not null || use.Action is not { Type: CardActionType.Use } action || action.ActorSeat == action.ProviderSeat)
            return false;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || action.RequesterSeat != action.ActorSeat || action.PhysicalCards.Count == 0 && !_characterGuoTuOlOperations.IsWeijieZeroSupply(use, request) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || request.ActiveAttack?.ResolutionId != use.Id || request.OwnerSeat != action.ActorSeat || request.CurrentCandidateSeat != action.ProviderSeat || request.AwaitingProviders || request.Purpose is not (FactionCardRequestPurpose.ProgramSkillUse or FactionCardRequestPurpose.AssistedProgramUse or FactionCardRequestPurpose.BorrowedSwordUse or FactionCardRequestPurpose.QinglongCrescentBladeUse) || action.PhysicalCards.Any(c => c.From.OwnerSeat != action.ProviderSeat || c.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) && !_coreOwnPublicPileConversionsOperations.IsOwnPublicPileActionCost(action, c)))
            throw new InvalidOperationException("An active supplied Slash lost its exact native provider/requester payment.");
        var costs = action.PhysicalCards.Select(c =>
        {
            var entry = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == c.CardId);
            return entry is not null && IsCardSupplyCompletionEntry(action, use.Id, request.OwnerFrameId, c, entry) ? entry : throw new InvalidOperationException("An active supplied Slash lost its exact native entry cost.");
        }).OrderBy(m => m.Sequence).ToArray();
        var sequences = costs.Select(m => m.Sequence).ToHashSet();
        var r = new ProgramCardSupplyCompletionReturn
        {
            ActionId = action.ActionId,
            ParentFrameId = use.Id,
            RequestOwnerFrameId = request.OwnerFrameId,
            Purpose = request.Purpose,
            RequesterSeat = action.ActorSeat,
            ProviderSeat = action.ProviderSeat,
            EffectiveKind = action.EffectiveKind,
            ActualTurnNumber = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            ActualTurnOwnerSeat = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat,
            NativeCosts = costs,
            CostBatches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == use.Id && b.AwaitingProgramFrameId is null && b.Movements.Any(m => sequences.Contains(m.Sequence))).OrderBy(b => b.Id).ToArray(),
            CostRecoveries = (use.PendingRecoveryAttempts ?? []).Where(a => global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime.IsResponseCompletionSilverLionAttempt(costs, a)).ToArray(),
            CostHealthChanges = _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Where(h => h.ParentFrameId == use.Id && global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime.IsResponseCompletionSilverLionHealth(costs, h)).ToArray()
        };
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardSupplyCompletionStartedEvent>().Any(e => e.ActionId == r.ActionId))
            throw new InvalidOperationException("An actual supplied play cannot complete twice.");
        var window = new ProgramCardTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, use.Id, action, ProgramCardContinuation.CommittedSlash, [], AttackOwnerFrameId: use.Id)
        {
            CardSupplyCompletion = r
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardSupplyCompletionStartedEvent(window.Id, r.ActionId, r.ParentFrameId, r.RequestOwnerFrameId, r.Purpose, r.RequesterSeat, r.ProviderSeat, r.EffectiveKind, r.ActualTurnNumber, r.ActualTurnOwnerSeat));
        _coreRuntimeOperations.PushRuntimeFrame(window);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramCardTriggerWindowFrame>();
        return true;
    }

    internal static CardActionContext CardSupplyCompletionActorAction(ProgramCardTriggerWindowFrame window) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CardSupplyCompletion.CardSupplyCompletionActorAction(window);
    internal static CardSupplyCompletedEvent CardSupplyCompletionFact(ProgramCardTriggerWindowFrame window) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CardSupplyCompletion.CardSupplyCompletionFact(window);
    internal bool IsCardSupplyCompletionCostPaid(ProgramCardTriggerWindowFrame window, CardActionCost cost) => window.CardSupplyCompletion is { } r && window.Action.PhysicalCards.Contains(cost) && r.NativeCosts.Where(m => m.CardId == cost.CardId).ToArray()is [var moved] && IsCardSupplyCompletionEntry(window.Action, r.ParentFrameId, r.RequestOwnerFrameId, cost, moved);
    internal bool IsCardSupplyCompletionEntry(CardActionContext action, long useId, long requestOwnerId, CardActionCost cost, CardMovementRecord moved)
    {
        if (!_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(moved) || moved.CardId != cost.CardId || moved.CardKind != cost.CardKind || moved.From != cost.From || moved.To != CardLocation.Processing)
            return false;
        if (cost.From.Zone == CardZoneKind.PublicPersistentPile && !_coreOwnPublicPileConversionsOperations.IsOwnPublicPileActionCost(action, cost))
            return false;
        if (moved.Reason == CardMoveReasons.Use)
            return true;
        // A successful declaration has already drained its own payment children.
        // The native Use claims that same entity instead of moving it a second
        // time. Its original request frame remains the typed payment owner.
        if (moved.Reason.Value != "conversion.declaration.pay" || moved.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || action.PhysicalCards is not [var only] || only != cost || cost.From != CardLocation.Hand(action.ProviderSeat) || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == requestOwnerId)?.AcceptedDeclarationPayment is not { Claimed: true } paid || paid.OwnerFrameId != requestOwnerId || paid.DeclarationId <= 0 || paid.ProviderSeat != action.ProviderSeat || paid.ActorSeat != action.ActorSeat || paid.Cost != cost || paid.Source.OwnerSeat != action.ProviderSeat || !action.ConversionChain.Contains(paid.Source) || _corePlayCardHistoryOperations.ViewAsRule(paid.Source)is not { DeclarationValidation: not null } rule || rule.OutputKind != paid.DeclaredKind)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<CardDeclarationCommittedEvent>().Where(e => e.DeclarationId == paid.DeclarationId).ToArray()is not [var declared] || declared.OwnerSeat != action.ProviderSeat || declared.ActorSeat != action.ActorSeat || declared.DeclaredKind != paid.DeclaredKind || !declared.TargetSeats.SequenceEqual(action.EffectiveDesignatedTargetSeats) || history.OfType<CardDeclarationRevealedEvent>().Any(e => e.DeclarationId == paid.DeclarationId && !e.Succeeded))
            return false;
        return paid.DeclaredKind == action.EffectiveKind || paid.DeclaredKind == CardKind.Slash && action.EffectiveKind == CardKind.FireSlash && history.OfType<ZhuqueFanConvertedEvent>().Any(e => e.ResolutionId == useId && e.SourceSeat == action.ActorSeat && e.PhysicalCardIds.SequenceEqual([cost.CardId]) && e.TargetSeats.SequenceEqual(action.EffectiveDesignatedTargetSeats));
    }

    internal bool ValidCardSupplyCompletionWindow(ProgramCardTriggerWindowFrame window)
    {
        if (window.CardSupplyCompletion is not { } r || window.ResponseCompletion is not null || window.CompletedResponseReturn is not null || window.Continuation != ProgramCardContinuation.CommittedSlash || window.ParentFrameId != r.ParentFrameId || window.AttackOwnerFrameId != r.ParentFrameId || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || r.RequesterSeat == r.ProviderSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ProviderSeat) || r.Purpose is not (FactionCardRequestPurpose.ProgramSkillUse or FactionCardRequestPurpose.AssistedProgramUse or FactionCardRequestPurpose.BorrowedSwordUse or FactionCardRequestPurpose.QinglongCrescentBladeUse) || _coreCardUseLifecycleOperations.LifecycleCardUse(r.ParentFrameId)is not { Action: { } native, CardAttack: not null } use || use.CompletedCardSupplyFrameId is not null || window.Action is not { Type: CardActionType.Use } a || a.ActionId != r.ActionId || a.ActorSeat != r.RequesterSeat || a.ProviderSeat != r.ProviderSeat || a.RequesterSeat != r.RequesterSeat || a.EffectiveKind != r.EffectiveKind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(r.EffectiveKind) || native.ActionId != a.ActionId || native.ActorSeat != a.ActorSeat || native.ProviderSeat != a.ProviderSeat || native.RequesterSeat != a.RequesterSeat || native.EffectiveKind != a.EffectiveKind || !native.PhysicalCards.SequenceEqual(a.PhysicalCards) || !native.ConversionChain.SequenceEqual(a.ConversionChain) || !native.TargetSeats.SequenceEqual(a.TargetSeats) || a.PhysicalCards.Count == 0 && !(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } weijieRequest && _characterGuoTuOlOperations.IsWeijieZeroSupply(use, weijieRequest)) || a.PhysicalCards.Any(c => c.From.OwnerSeat != r.ProviderSeat || c.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) && !_coreOwnPublicPileConversionsOperations.IsOwnPublicPileActionCost(a, c)) || r.NativeCosts.Count != a.PhysicalCards.Count || r.NativeCosts.Select(m => m.Sequence).Distinct().Count() != r.NativeCosts.Count || !a.PhysicalCards.All(c => IsCardSupplyCompletionCostPaid(window, c)))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<CardUseDeclaredEvent>().Where(e => e.ResolutionId == use.Id).ToArray()is not [var declared] || declared.SourceSeat != r.RequesterSeat || declared.CardKind != r.EffectiveKind || declared.CardId != use.CardId || history.OfType<CardSupplyCompletionStartedEvent>().Where(e => e.ActionId == r.ActionId).ToArray()is not [var started] || started != new CardSupplyCompletionStartedEvent(window.Id, r.ActionId, r.ParentFrameId, r.RequestOwnerFrameId, r.Purpose, r.RequesterSeat, r.ProviderSeat, r.EffectiveKind, r.ActualTurnNumber, r.ActualTurnOwnerSeat) || r.CostBatchCursor < 0 || r.CostBatchCursor > r.CostBatches.Count || r.CostRecoveryCursor < 0 || r.CostRecoveryCursor > r.CostRecoveries.Count || r.CostHealthCursor < 0 || r.CostHealthCursor > r.CostHealthChanges.Count || r.CostBatches.Select(b => b.Id).Distinct().Count() != r.CostBatches.Count || r.CostBatches.Any(b => b.ParentFrameId != r.ParentFrameId || b.AwaitingProgramFrameId is not null || b.Movements.Count == 0 || !b.Movements.Any(m => r.NativeCosts.Contains(m)) || b.Movements.Any(m => !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m))) || r.CostRecoveries.Any(a2 => !global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime.IsResponseCompletionSilverLionAttempt(r.NativeCosts, a2)) || r.CostHealthChanges.Any(h => h.ParentFrameId != r.ParentFrameId || !global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime.IsResponseCompletionSilverLionHealth(r.NativeCosts, h)))
            return false;
        var completed = history.OfType<CardSupplyCompletedEvent>().Where(e => e.ActionId == r.ActionId).ToArray();
        if (r.CostsDrained)
            return r.CostBatchCursor == r.CostBatches.Count && r.CostRecoveryCursor == r.CostRecoveries.Count && r.CostHealthCursor == r.CostHealthChanges.Count && r.ActiveHealthChildFrameId is null && r.ActiveCostChildFrameId is null && completed is [var fact] && fact == CardSupplyCompletionFact(window);
        if (window.Candidates.Count != 0 || window.CandidateIndex != 0 || completed.Length != 0)
            return false;
        if (r.ActiveHealthChildFrameId is { } health)
            return r.ActiveCostChildFrameId is null && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == health)is { } child && IsCardSupplyCompletionHealthChild(window, child);
        return r.ActiveCostChildFrameId is not { } movement || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == movement)is { } moved && CardSupplyCompletionFirstChild(window, moved);
    }

    internal bool TryDrainCardSupplyCompletionCosts(ProgramCardTriggerWindowFrame supplied)
    {
        if (supplied.CardSupplyCompletion is not { CostsDrained: false })
            return false;
        var window = supplied;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != window.Id || !ValidCardSupplyCompletionWindow(window))
            throw new InvalidOperationException("A supplied play lost its exact paid cost prelude.");
        var r = window.CardSupplyCompletion!;
        if (r.ActiveCostChildFrameId is not null || r.ActiveHealthChildFrameId is not null)
            throw new InvalidOperationException("A supplied play cannot skip its actual native cost child.");
        if (TryDrainCardSupplyCompletionHealth(window))
            return true;
        window = (ProgramCardTriggerWindowFrame)_resolutionStackOperations._resolutionStack[^1];
        r = window.CardSupplyCompletion!;
        while (r.CostBatchCursor < r.CostBatches.Count)
        {
            var batch = r.CostBatches[r.CostBatchCursor];
            var queued = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.SingleOrDefault(b => b.Id == batch.Id);
            if (queued is null || !queued.Movements.SequenceEqual(batch.Movements))
                throw new InvalidOperationException("A supplied play lost its real queued Use movement batch.");
            _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Remove(queued);
            var child = _coreDeferredTurnEndPreludesOperations.CreateCardsMovedProgramWindow(queued, null);
            if (child is null)
            {
                r = r with
                {
                    CostBatchCursor = r.CostBatchCursor + 1
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(window = window with { CardSupplyCompletion = r });
                continue;
            }

            child = child with
            {
                ResumeCardSupplyCompletionFrameId = window.Id
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(window with { CardSupplyCompletion = r with { ActiveCostChildFrameId = child.Id } });
            _coreRuntimeOperations.PushRuntimeFrame(child);
            _coreRuntimeOperations.AdvanceRuntimeTop<CardsMovedTriggerWindowFrame>();
            return true;
        }

        var candidates = _coreCardActionsOperations.CollectSharedCardActionCandidates(CardSupplyCompletionActorAction(window), SkillProgramTriggerWindow.CardSupplyCompleted, [], null).OrderByDescending(c => c.Priority).ThenBy(c => _coreTableOrderOperations.RelativeTableOrder(0, c.OwnerSeat)).ThenBy(c => c.SkillId, StringComparer.Ordinal).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).ThenBy(c => c.TriggerId, StringComparer.Ordinal).Select(c => c.FrozenContext is { CardUse: { } use } context ? c with { FrozenContext = context with { ParentFrameId = window.Id, CardUse = use with { ParentCardUseFrameId = window.ParentFrameId } } } : throw new InvalidOperationException("A supplied play candidate lost its frozen provider context.")).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(window = window with { CardSupplyCompletion = r with { CostsDrained = true }, Candidates = Array.AsReadOnly(candidates) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(CardSupplyCompletionFact(window));
        return false;
    }

    internal bool TryDrainCardSupplyCompletionHealth(ProgramCardTriggerWindowFrame supplied)
    {
        var window = supplied;
        var r = window.CardSupplyCompletion!;
        while (r.CostRecoveryCursor < r.CostRecoveries.Count)
        {
            var request = r.CostRecoveries[r.CostRecoveryCursor];
            var native = _resolutionStackOperations._resolutionStack.Single(f => f.Id == r.ParentFrameId);
            var actual = native.PendingRecoveryAttempts?.SingleOrDefault(a => a.Id == request.Id);
            if (actual is null || !global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime.SameResponseCompletionRecovery(actual, request))
                throw new InvalidOperationException("A supplied play lost its queued Silver Lion recovery.");
            var remaining = native.PendingRecoveryAttempts!.Where(a => a.Id != request.Id).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeFrame(native.Id, native with { PendingRecoveryAttempts = remaining.Length == 0 ? null : Array.AsReadOnly(remaining) });
            _coreRuntimeOperations.ReplaceRuntimeTop(window with { CardSupplyCompletion = r with { ActiveHealthChildFrameId = request.Id } });
            _coreRuntimeOperations.PushRuntimeFrame(new RecoveryReplacementFrame(request.Id, window.Id, request, new(PostEventContinuation.CardSupplyCompletion, window.Id)));
            _coreRuntimeOperations.AdvanceRuntimeFrame(request.Id);
            return true;
        }

        while (r.CostHealthCursor < r.CostHealthChanges.Count)
        {
            var change = r.CostHealthChanges[r.CostHealthCursor];
            if (!_coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Remove(change))
                throw new InvalidOperationException("A supplied play lost its actual cost HP invoice.");
            var owner = _coreMatchParticipantsPlayersAccess._players[change.TargetSeat];
            var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner)with
            {
                HpChangeAmount = change.Amount,
                HpBeforeChange = change.HpBefore,
                HpAfterChange = change.HpAfter,
                HealthChangeKind = _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerCondition(SkillProgramTriggerConditionKind.HealthChangeKindIs) ? change.Kind : null
            };
            var candidates = owner.IsAlive ? new[]
            {
                SkillProgramTriggerWindow.AfterHpRecovered,
                SkillProgramTriggerWindow.AfterHealthChanged
            }.SelectMany(w => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, w)).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Condition.Evaluate(facts, c.SkillId, c.SkillInstanceId)).SelectMany(c => Enumerable.Range(0, _coreProgramLifecycleOperations.GetProgramTrigger2(c).HpChangeOccurrence == SkillProgramHpChangeOccurrence.PerPoint ? change.Amount : 1).Select(i => c with { OccurrenceIndex = i })).ToArray() : [];
            if (candidates.Length == 0)
            {
                r = r with
                {
                    CostHealthCursor = r.CostHealthCursor + 1
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(window = window with { CardSupplyCompletion = r });
                continue;
            }

            var contexts = candidates.Select(c => new ProgramSkillWindowContext(_coreProgramLifecycleOperations.GetProgramTrigger2(c).Window, change.Id, owner.Seat, SourceSeat: change.SourceSeat, TargetSeat: owner.Seat, Amount: change.Amount, OccurrenceIndex: c.OccurrenceIndex, Facts: facts, HpChange: change)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(window with { CardSupplyCompletion = r with { ActiveHealthChildFrameId = change.Id } });
            _coreRuntimeOperations.PushRuntimeFrame(new HpChangedTriggerWindowFrame(change.Id, change, Array.AsReadOnly(candidates), Array.AsReadOnly(contexts), PostEventContinuation.CardSupplyCompletion, window.Id));
            _coreRuntimeOperations.AdvanceRuntimeTop<HpChangedTriggerWindowFrame>();
            return true;
        }

        return false;
    }

    internal bool IsCardSupplyCompletionHealthChild(ProgramCardTriggerWindowFrame window, ResolutionFrame child)
    {
        if (window.CardSupplyCompletion is not { CostsDrained: false, ActiveHealthChildFrameId: { } id } r || child.Id != id || r.ActiveCostChildFrameId is not null)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return r.CostRecoveryCursor >= 0 && r.CostRecoveryCursor < r.CostRecoveries.Count && recovery.ParentFrameId == window.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.CardSupplyCompletion, window.Id) && global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime.SameResponseCompletionRecovery(recovery.Attempt, r.CostRecoveries[r.CostRecoveryCursor]) && global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime.IsResponseCompletionSilverLionAttempt(r.NativeCosts, recovery.Attempt);
        return child is HpChangedTriggerWindowFrame hp && r.CostRecoveryCursor == r.CostRecoveries.Count && r.CostHealthCursor >= 0 && r.CostHealthCursor < r.CostHealthChanges.Count && hp.Continuation == PostEventContinuation.CardSupplyCompletion && hp.ResumeFrameId == window.Id && hp.CardId is null && hp.CardKind is null && hp.Change == r.CostHealthChanges[r.CostHealthCursor] && hp.Change.ParentFrameId == r.ParentFrameId && global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionHealthRuntime.IsResponseCompletionSilverLionHealth(r.NativeCosts, hp.Change);
    }

    internal bool CardSupplyCompletionFirstChild(ProgramCardTriggerWindowFrame window, ResolutionFrame child) => IsCardSupplyCompletionHealthChild(window, child) || window.CardSupplyCompletion is { CostsDrained: false, ActiveCostChildFrameId: { } id } r && r.CostBatchCursor >= 0 && r.CostBatchCursor < r.CostBatches.Count && child is CardsMovedTriggerWindowFrame moved && moved.Id == id && moved.ResumeCardSupplyCompletionFrameId == window.Id && moved.ResumeResponseCompletionFrameId is null && moved.ResumeProgramFrameId is null && moved.Batch.Id == r.CostBatches[r.CostBatchCursor].Id && moved.Batch.ParentFrameId == r.ParentFrameId && moved.Batch.AwaitingProgramFrameId is null && moved.Batch.Movements.SequenceEqual(r.CostBatches[r.CostBatchCursor].Movements);
    internal bool ReturnCardSupplyCompletionMovement(CardsMovedTriggerWindowFrame child)
    {
        if (child.ResumeCardSupplyCompletionFrameId is not { } id)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramCardTriggerWindowFrame window || window.Id != id || !CardSupplyCompletionFirstChild(window, child))
            throw new InvalidOperationException("A supplied play movement lost its exact typed parent.");
        var r = window.CardSupplyCompletion!;
        _coreRuntimeOperations.ReplaceRuntimeTop(window with { CardSupplyCompletion = r with { CostBatchCursor = r.CostBatchCursor + 1, ActiveCostChildFrameId = null } });
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramCardTriggerWindowFrame>();
        return true;
    }

    internal bool ReturnCardSupplyCompletionRecovery(RecoveryReplacementFrame child)
    {
        if (child.Return.Continuation != PostEventContinuation.CardSupplyCompletion)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramCardTriggerWindowFrame window || !IsCardSupplyCompletionHealthChild(window, child))
            throw new InvalidOperationException("A supplied play recovery lost its exact typed parent.");
        var r = window.CardSupplyCompletion!;
        _coreRuntimeOperations.ReplaceRuntimeTop(window with { CardSupplyCompletion = r with { CostRecoveryCursor = r.CostRecoveryCursor + 1, ActiveHealthChildFrameId = null } });
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramCardTriggerWindowFrame>();
        return true;
    }

    internal bool ReturnCardSupplyCompletionHpChange(HpChangedTriggerWindowFrame child)
    {
        if (child.Continuation != PostEventContinuation.CardSupplyCompletion)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramCardTriggerWindowFrame window || !IsCardSupplyCompletionHealthChild(window, child))
            throw new InvalidOperationException("A supplied play HP observer lost its exact typed parent.");
        var r = window.CardSupplyCompletion!;
        _coreRuntimeOperations.ReplaceRuntimeTop(window with { CardSupplyCompletion = r with { CostHealthCursor = r.CostHealthCursor + 1, ActiveHealthChildFrameId = null } });
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramCardTriggerWindowFrame>();
        return true;
    }

    internal void ContinueCardSupplyAfterCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack, ProgramCardTriggerWindowFrame window)
    {
        if (window.CardSupplyCompletion is not { CostsDrained: true } || !ValidCardSupplyCompletionWindow(window) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Id != window.ParentFrameId || attack.ResolutionId != use.Id)
            throw new InvalidOperationException("A supplied play lost its once-paid native Slash return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { CompletedCardSupplyFrameId = window.Id });
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        var action = use.Action;
        if (_coreRecoveryReplacementOperations.TryPauseRecoveryPaidCardUse(use.Id, new(RecoveryPaidCardUseKind.CommittedSlash, attack.SourceSeat)))
            return;
        if (action is not null && _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(use.Id) && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            return;
        _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool CardSupplyCompletionStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => parent is ProgramCardTriggerWindowFrame { CardSupplyCompletion: not null } w && CardSupplyCompletionFirstChild(w, child) || child is ProgramCardTriggerWindowFrame { CardSupplyCompletion: not null } window && window.ParentFrameId == parent.Id && ValidCardSupplyCompletionWindow(window);
    internal ProgramCardTriggerWindowFrame? CardSupplyCompletionObserverRoot()
    {
        for (var index = 0; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
            if (_resolutionStackOperations._resolutionStack[index] is ProgramCardTriggerWindowFrame { CardSupplyCompletion: not null } root && ValidCardSupplyCompletionWindow(root) && CardSupplyCompletionFirstChild(root, _resolutionStackOperations._resolutionStack[index + 1]) && _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index))
                return root;
        return null;
    }
}
