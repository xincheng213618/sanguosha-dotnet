#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDingYuanOlRuntime
{
    internal const string AdoptedChildGrantSource = "marker:adopted-child";
    internal bool TracksDingYuan => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PrepareAdoptedChildAppointment) == true;
    internal int? AdoptedChildHolder => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Markers.GetValueOrDefault(PlayerMarkerKind.AdoptedChild) > 0).Select(player => (int? )player.Seat).SingleOrDefault();

    internal static string AdoptedChildGrantId(int seat) => RuntimeSkills.RuntimeRules_ding_yuan_ol_ProgramDingYuan.AdoptedChildGrantId(seat);
    void IDingYuanProgramHost.PrepareAdoptedChild(ProgramSkillFrame frame, bool transfer) => PrepareAdoptedChild(frame, transfer);
    void IDingYuanProgramHost.AssignAdoptedChild(ProgramSkillFrame frame) => AssignAdoptedChild(frame);
    void IDingYuanProgramHost.QualifyFirstRoundDamage(ProgramSkillFrame frame) => QualifyFirstRoundDamage(frame);
    IReadOnlyList<AdoptedChildParticipant> IAdoptedChildSelectionQuery.Participants => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => new AdoptedChildParticipant(player.Seat, player.IsAlive, player.Markers.GetValueOrDefault(PlayerMarkerKind.AdoptedChild) > 0, _coreNationalSkillsOperations.HasRuntimeSkill(player, "ol:cixiao"))).ToArray());

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26020)]
    internal bool CanOfferDingYuan(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(effect => effect.Op is SkillProgramEffectOp.PrepareAdoptedChildAppointment or SkillProgramEffectOp.PrepareAdoptedChildTransfer))
        {
            var transfer = trigger.Effects[0].Op == SkillProgramEffectOp.PrepareAdoptedChildTransfer;
            return context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && transfer == (AdoptedChildHolder is not null) && _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && (transfer || player.Seat != candidate.OwnerSeat) && player.Seat != AdoptedChildHolder) && (!transfer || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0);
        }

        if (trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.QualifyFirstRoundDamage))
            return FirstRoundDamageFor(context)is not null && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstRoundDamageQualifiedEvent>().Any(fact => fact.DamageFrameId == context.DamageFrameId && fact.OwnerSeat == candidate.OwnerSeat && fact.SkillInstanceId == candidate.SkillInstanceId && fact.BindingId == candidate.BindingId);
        if (candidate.SkillId == "ol:panshi" && trigger.Id == "preparation-tribute")
            return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0 && _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Seat != candidate.OwnerSeat && _coreNationalSkillsOperations.HasRuntimeSkill(player, "ol:cixiao"));
        return true;
    }

    internal void PrepareAdoptedChild(ProgramSkillFrame supplied, bool transfer)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.InstructionIndex != 1 || frame.AdoptedChildRelationship is not null || frame.WindowContext is not { } context || frame.SkillId != "ol:cixiao" || !CanOfferDingYuan(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(frame), context) || transfer != (AdoptedChildHolder is not null))
            throw new InvalidOperationException("An adopted-child relationship requires its exact own preparation and initial relation.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { AdoptedChildRelationship = new(frame.Id, new(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, context.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, AdoptedChildHolder, transfer) });
    }

    internal void AssignAdoptedChild(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.AdoptedChildRelationship is not { } receipt || receipt.FrameId != frame.Id || receipt.Source.OwnerSeat != frame.OwnerSeat || receipt.Source.SkillInstanceId != frame.SkillInstanceId || receipt.GameplayHash != frame.GameplayHash || receipt.ParentFrameId != frame.WindowContext?.ParentFrameId || receipt.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || frame.InstructionIndex != (receipt.Transfer ? 4 : 3) || frame.SelectedTargetSeats is not [var successor])
            throw new InvalidOperationException("An adopted-child assignment lost its owning preparation receipt.");
        // The ordinary movement instruction owns and remembers a paid transfer.
        // A nested relationship change can invalidate it, never charge it again.
        if (receipt.PreviousHolder != AdoptedChildHolder || !_coreMatchParticipantsPlayersAccess._players[successor].IsAlive || !receipt.Transfer && successor == frame.OwnerSeat || successor == receipt.PreviousHolder)
            return;
        if (receipt.PreviousHolder is { } old)
            _coreStrategicProgramsOperations.SetStrategicMarker(old, PlayerMarkerKind.AdoptedChild, frame.OwnerSeat, 0, frame);
        _coreStrategicProgramsOperations.SetStrategicMarker(successor, PlayerMarkerKind.AdoptedChild, frame.OwnerSeat, 1, frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AdoptedChildAssignedEvent(frame.Id, receipt.Source, receipt.PreviousHolder, successor, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26020)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 26020)]
    internal void SynchronizeAdoptedChildSkillGrants()
    {
        if (!TracksDingYuan || _characterDingYuanOlSynchronizingAdoptedChildGrantsAccess._synchronizingAdoptedChildGrants)
            return;
        _characterDingYuanOlSynchronizingAdoptedChildGrantsAccess._synchronizingAdoptedChildGrants = true;
        try
        {
            foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            {
                var id = AdoptedChildGrantId(player.Seat);
                var existing = player.SkillGrants.Grants.SingleOrDefault(grant => grant.GrantId == id);
                var marked = player.IsAlive && player.Markers.GetValueOrDefault(PlayerMarkerKind.AdoptedChild) > 0;
                if (!marked && existing is not null)
                {
                    player.SkillGrants.RemoveGrant(id);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AdoptedChildSkillGrantChangedEvent(player.Seat, id, existing.SkillInstanceId, false));
                }
                else if (marked && existing is null)
                {
                    var instance = _coreNationalSkillsOperations.GetSkillBindingShard(player).ActiveGrants.Where(grant => grant.SkillId == "ol:panshi").OrderBy(grant => grant.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault()?.SkillInstanceId ?? id;
                    player.SkillGrants.Grant(new(id, "ol:panshi", instance, AdoptedChildGrantSource));
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AdoptedChildSkillGrantChangedEvent(player.Seat, id, instance, true));
                }
            }
        }
        finally
        {
            _characterDingYuanOlSynchronizingAdoptedChildGrantsAccess._synchronizingAdoptedChildGrants = false;
        }
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26021)]
    internal void ObserveDingYuanNativeFacts(IGameEvent payload)
    {
        if (TracksDingYuan && payload is CardUseDeclaredEvent declared && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && declared.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(declared.CardKind) && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { } use && use.SourceSeat == declared.SourceSeat && use.CardKind == declared.CardKind && use.AdoptedChildSlashUse is null)
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { AdoptedChildSlashUse = new(use.Id, use.SourceSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) });
        if (payload is DamageRequestedEvent requested && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } requestedAttack && _resolutionStackOperations._resolutionStack.LastOrDefault()is DamageFrame requestedFrame && requestedFrame.Id == requested.ResolutionId && requestedFrame.ParentFrameId == requestedAttack.ResolutionId && requestedFrame.AdoptedChildDamage is null && requestedFrame.TargetSeat == requested.TargetSeat && requestedFrame.SourceSeat == requested.SourceSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(requestedAttack.ResolutionId)?.AdoptedChildDamageIssue is { } issue && issue.CardUseFrameId == requestedAttack.ResolutionId && issue.ActorSeat == requested.SourceSeat && issue.TargetSeat == requested.TargetSeat && issue.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && issue.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId)
            _coreRuntimeOperations.ReplaceRuntimeFrame(requestedFrame.Id, requestedFrame with { AdoptedChildDamage = new(requestedFrame.Id, issue.CardUseFrameId, requested.SourceSeat, requested.TargetSeat, issue.ActualTurn, issue.PhaseInstance, issue.SkillInstanceId) });
        if (payload is not DamageAppliedEvent { Amount: > 0 } applied || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || damage.ParentFrameId != attack.ResolutionId || damage.SourceSeat != applied.SourceSeat || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount || damage.Nature != applied.Nature || attack.IsSourceLess != applied.SourceLess || !attack.DamageWasApplied)
            return;
        if (TracksRoundFirstDamage && _coreActualRoundProgressRoundNumberAccess._roundNumber > 0 && !applied.SourceLess && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundFirstSourcedDamageEvent>().Any(fact => fact.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundFirstSourcedDamageEvent(_coreActualRoundProgressRoundNumberAccess._roundNumber, damage.Id, applied.SourceSeat, applied.TargetSeat, applied.Amount));
        if (damage.AdoptedChildDamage is { } debt && debt.DamageFrameId == damage.Id && debt.CardUseFrameId == attack.ResolutionId && debt.ActorSeat == applied.SourceSeat && debt.TargetSeat == applied.TargetSeat && debt.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && debt.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && debt.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AdoptedChildPlayEndRequestedEvent>().Any(fact => fact.DamageFrameId == damage.Id))
        {
            _coreGameEngineStrategicProgramsStrategicEndPlayTurnAccess._strategicEndPlayTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AdoptedChildPlayEndRequestedEvent(damage.Id, debt.CardUseFrameId, debt.ActorSeat, debt.TargetSeat, debt.ActualTurn, debt.PhaseInstance));
        }
    }

    internal void CaptureAdoptedChildSlashDamageIssue(GameEngine.IDamageAttempt attack)
    {
        if (!TracksDingYuan || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use)
            return;
        AdoptedChildDamageIssue? issue = IsAdoptedChildSlashDamage(attack, out var origin) ? new(use.Id, attack.SourceSeat, attack.TargetSeat, origin.ActualTurn, origin.PhaseInstance, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], "ol:panshi")) : null;
        if (use.AdoptedChildDamageIssue != issue)
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { AdoptedChildDamageIssue = issue });
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 90)]
    internal int AdoptedChildSlashDamage(GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.AdoptedChildDamageIssue is { } issue && issue.CardUseFrameId == attack.ResolutionId && issue.ActorSeat == attack.SourceSeat && issue.TargetSeat == attack.TargetSeat && issue.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && issue.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId ? 1 : 0;
    internal bool IsAdoptedChildSlashDamage(GameEngine.IDamageAttempt attack, out AdoptedChildSlashUseReceipt origin)
    {
        origin = null !;
        // OL's operator FAQ requires a direct Slash target. Elemental damage
        // still propagates its finalized amount, but a chained recipient cannot
        // acquire a second Panshi bonus or an independent end-Play consequence.
        if (!TracksDingYuan || attack.IsSourceLess || attack.IsChainPropagation || attack.CardUserSeat != attack.SourceSeat || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) || attack.SourceSeat == attack.TargetSeat || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], "ol:panshi") || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat], "ol:cixiao") || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { AdoptedChildSlashUse: { } stamp } use || use.SourceSeat != attack.SourceSeat || use.CardKind != kind || stamp.CardUseFrameId != use.Id || stamp.ActorSeat != attack.SourceSeat || stamp.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || stamp.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || stamp.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play)
            return false;
        origin = stamp;
        return true;
    }
}
