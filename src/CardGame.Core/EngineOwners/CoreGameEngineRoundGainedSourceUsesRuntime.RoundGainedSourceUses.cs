#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundGainedSourceUsesRuntime
{
    internal bool TracksRoundGainedSourceUses => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AdjustOneRoundGainedOrdinaryTrickTarget);
    internal long RoundGainedMovementSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 340)]
    internal void ObserveRoundGainedRoundBoundary(IGameEvent payload)
    {
        if (!TracksRoundGainedSourceUses || payload is not RoundStartedEvent round)
            return;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundGainedRoundBoundaryEvent>().Any(e => e.RoundNumber == round.RoundNumber))
            throw new InvalidOperationException("A round-gained source ledger cannot issue its round boundary twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundGainedRoundBoundaryEvent(round.RoundNumber, round.ActorSeat, RoundGainedMovementSequence));
    }

    internal RoundGainedRoundBoundaryEvent? RoundGainedCurrentBoundary() => _coreActualRoundProgressRoundNumberAccess._roundNumber > 0 ? _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundGainedRoundBoundaryEvent>().SingleOrDefault(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber) : null;
    internal IReadOnlyList<RoundGainedMaterialAcquisition>? RoundGainedMaterials(CharacterState owner, string skillId, string policyId, IReadOnlyList<int> ids, RoundGainedRoundBoundaryEvent boundary)
    {
        if (ids.Count == 0 || ids.Distinct().Count() != ids.Count)
            return null;
        var excluded = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundGainedEquipmentDrawIssuedEvent>().Where(e => e.Qualification.ActorSeat == owner.Seat && e.Qualification.Source.SkillId == skillId && e.Qualification.PolicyId == policyId && e.DrawRoundNumber == boundary.RoundNumber).SelectMany(e => e.DrawnCardIds).ToHashSet();
        var result = new List<RoundGainedMaterialAcquisition>();
        foreach (var id in ids)
        {
            if (excluded.Contains(id))
                return null;
            var gain = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == id && m.Sequence > boundary.MovementSequence && m.To == CardLocation.Hand(owner.Seat) && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(m) != CardLocation.Hand(owner.Seat));
            if (gain is null)
                return null;
            result.Add(new(id, gain.Sequence, _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(gain)));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal bool InRoundGainedOwnPlay(CharacterState owner) => owner.IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat == owner.Seat && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play;
    internal bool HasRoundGainedBasicBonus(CharacterState owner, CardKind kind, IReadOnlyList<int> ids)
    {
        if (!TracksRoundGainedSourceUses || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) != SkillProgramCardCategory.Basic || ids is null || ids.Count == 0 || !InRoundGainedOwnPlay(owner))
            return false;
        var policies = _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.RoundGainedOtherSourceUse).ToArray();
        if (policies.Length == 0 || RoundGainedCurrentBoundary()is not { } boundary || ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat != owner.Seat || _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(owner, id)is null))
            return false;
        return policies.Any(item => RoundGainedMaterials(owner, item.Source.SkillId, item.Policy.Id, ids, boundary)is not null);
    }

    internal bool HasPotentialRoundGainedBasicMaterials(CharacterState owner, CardKind kind)
    {
        if (!TracksRoundGainedSourceUses || !InRoundGainedOwnPlay(owner) || !_coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.RoundGainedOtherSourceUse))
            return false;
        return _coreActionQueriesOperations.GetSlashUseCards(owner).Any(card => HasRoundGainedBasicBonus(owner, kind, [card.Id])) || _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(owner).Any(cards => HasRoundGainedBasicBonus(owner, kind, cards.Select(c => c.Id).ToArray())) || _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(owner, kind, forResponse: false).Any(selection => HasRoundGainedBasicBonus(owner, selection.OutputKind, selection.Cards.Select(c => c.Id).ToArray()));
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.AfterSlashDebitIdentity, 20)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 30)]
    internal bool HasIssuedRoundGainedBasicBonus(long useId, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { } action } use && action.ActorSeat == actor && CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.CardKind) == SkillProgramCardCategory.Basic && use.RoundGainedUseQualifications is { Count: > 0 } qualifications && qualifications.Any(q => q.ActorSeat == actor && ValidRoundGainedQualification(use, q));
    internal void IssueRoundGainedUseQualifications(long useId, CardActionContext? action)
    {
        if (!TracksRoundGainedSourceUses || action is not { Type: CardActionType.Use } || action.ResponderSeat is not null || action.OpponentSeat is not null || action.PhysicalCards.Count == 0 || action.ActorSeat != action.ProviderSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat) || !InRoundGainedOwnPlay(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]) || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { PhysicalCardIds: { } physicalIds } use || use.Action?.ActionId != action.ActionId || use.SourceSeat != action.ActorSeat)
            return;
        var policies = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], SkillProgramCardPolicyKind.RoundGainedOtherSourceUse).ToArray();
        if (policies.Length == 0 || RoundGainedCurrentBoundary()is not { } boundary)
            return;
        if (use.RoundGainedUseQualifications is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundGainedUseQualifiedEvent>().Any(e => e.Qualification.CardUseFrameId == useId))
            throw new InvalidOperationException("A real Use cannot replace its frozen round-gained qualifications.");
        var owner = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        var qualifications = new List<RoundGainedUseQualification>();
        foreach (var group in policies.GroupBy(item => item.Source.SkillId))
        {
            var entry = group.OrderBy(item => item.Source.SkillInstanceId, StringComparer.Ordinal).First();
            var gains = RoundGainedMaterials(owner, entry.Source.SkillId, entry.Policy.Id, physicalIds, boundary);
            if (gains is null)
                continue;
            qualifications.Add(new(new(entry.Source.SkillId, entry.Policy.Id, owner.Seat, entry.Source.SkillInstanceId), entry.Source.Program.GameplayHash, entry.Policy.Id, boundary.RoundNumber, boundary.MovementSequence, useId, action.ActionId, owner.Seat, action.EffectiveKind) { MaterialGains = gains });
        }

        if (qualifications.Count == 0)
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, frame => frame with { RoundGainedUseQualifications = qualifications });
        foreach (var qualification in qualifications)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundGainedUseQualifiedEvent(qualification));
    }

    internal bool SameRoundGainedQualification(RoundGainedUseQualification a, RoundGainedUseQualification b) => a.Source == b.Source && a.GameplayHash == b.GameplayHash && a.PolicyId == b.PolicyId && a.RoundNumber == b.RoundNumber && a.RoundStartMovementSequence == b.RoundStartMovementSequence && a.CardUseFrameId == b.CardUseFrameId && a.ActionId == b.ActionId && a.ActorSeat == b.ActorSeat && a.EffectiveKind == b.EffectiveKind && a.MaterialGains.SequenceEqual(b.MaterialGains);
    internal bool ValidRoundGainedQualification(CardUseFrame use, RoundGainedUseQualification q)
    {
        if (use.Action is not { Type: CardActionType.Use } action || use.PhysicalCardIds is not { } physicalIds || use.Id != q.CardUseFrameId || action.ActionId != q.ActionId || use.CardKind != q.EffectiveKind || action.EffectiveKind != q.EffectiveKind || action.ProviderSeat != q.ActorSeat || q.Source.OwnerSeat != q.ActorSeat || q.Source.BindingId != q.PolicyId || q.RoundNumber <= 0 || q.MaterialGains.Count == 0 || !q.MaterialGains.Select(m => m.CardId).SequenceEqual(physicalIds) || !q.MaterialGains.Select(m => m.CardId).SequenceEqual(action.PhysicalCards.Select(c => c.CardId)) || q.MaterialGains.Select(m => m.CardId).Distinct().Count() != q.MaterialGains.Count || _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program is not { } program || program.GameplayHash != q.GameplayHash || program.CardPolicies.Count(p => p.Kind == SkillProgramCardPolicyKind.RoundGainedOtherSourceUse && p.Id == q.PolicyId) != 1)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<RoundGainedRoundBoundaryEvent>().Where(e => e.RoundNumber == q.RoundNumber).ToArray()is not [var boundary] || boundary.MovementSequence != q.RoundStartMovementSequence || history.OfType<RoundGainedUseQualifiedEvent>().Count(e => SameRoundGainedQualification(e.Qualification, q)) != 1)
            return false;
        foreach (var gain in q.MaterialGains)
            if (gain.MovementSequence <= boundary.MovementSequence || _coreCardMovementJournalCardMovementsAccess._cardMovements.SingleOrDefault(m => m.Sequence == gain.MovementSequence)is not { } movement || movement.CardId != gain.CardId || movement.To != CardLocation.Hand(q.ActorSeat) || _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(movement) != gain.OriginalSource || gain.OriginalSource == CardLocation.Hand(q.ActorSeat))
                return false;
        return true;
    }

    internal void AssertRoundGainedUseQualifications(CardUseFrame use)
    {
        if (use.RoundGainedUseQualifications is null && !TracksRoundGainedSourceUses)
            return;
        var issued = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundGainedUseQualifiedEvent>().Where(e => e.Qualification.CardUseFrameId == use.Id).ToArray();
        if (use.RoundGainedUseQualifications is not { } qualifications)
        {
            if (issued.Length != 0)
                throw new InvalidOperationException("A qualified Use lost its owning round-source receipt.");
            return;
        }

        if (qualifications.Count == 0 || qualifications.Count != issued.Length || qualifications.Select(q => (q.Source.OwnerSeat, q.Source.SkillId)).Distinct().Count() != qualifications.Count || qualifications.Any(q => !ValidRoundGainedQualification(use, q)))
            throw new InvalidOperationException("A round-source qualification lost its exact acquired materials, policy or native Use.");
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 4)]
    internal void CollectIssuedRoundGainedUseCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window is not (SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardUseCommitted) || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { RoundGainedUseQualifications: { } qualifications } use || use.Action?.ActionId != action.ActionId)
            return;
        AssertRoundGainedUseQualifications(use);
        foreach (var q in qualifications)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[q.ActorSeat].IsAlive || action.ActorSeat != q.ActorSeat)
                continue;
            var op = window == SkillProgramTriggerWindow.CardUseTargetsFinalized ? SkillProgramEffectOp.AdjustOneRoundGainedOrdinaryTrickTarget : SkillProgramEffectOp.DrawForRoundGainedEquipmentUse;
            var program = _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program!;
            foreach (var trigger in program.Triggers.Where(t => t.Window == window && t.Effects is [{ } e] && e.Op == op))
            {
                if (window == SkillProgramTriggerWindow.CardUseTargetsFinalized ? !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind) : CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.CardKind) != SkillProgramCardCategory.Equipment)
                    continue;
                result.RemoveAll(c => c.OwnerSeat == q.ActorSeat && c.SkillId == q.Source.SkillId && c.TriggerId == trigger.Id && c.SkillInstanceId != q.Source.SkillInstanceId);
                if (result.Any(c => c.OwnerSeat == q.ActorSeat && c.SkillId == q.Source.SkillId && c.TriggerId == trigger.Id && c.SkillInstanceId == q.Source.SkillInstanceId))
                    continue;
                var target = action.TargetSeats.Count == 1 ? action.TargetSeats[0] : -1;
                result.Add(new(q.ActorSeat, target, q.Source.SkillId, trigger.Id, q.GameplayHash, q.Source.SkillInstanceId, trigger.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, q.ActorSeat, target, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[q.ActorSeat], action))));
            }
        }
    }

    internal RoundGainedUseQualification? ExactIssuedRoundGainedUseCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, bool paid = false)
    {
        if (context.CardUse is not { } card || context.Window is not (SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardUseCommitted) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) || !paid && (!_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None) || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != card.ParentCardUseFrameId || window.Action.ActionId != card.CardActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { RoundGainedUseQualifications: { } qualifications, Action: { Type: CardActionType.Use } action } use || action.ActionId != card.CardActionId || action.ActorSeat != candidate.OwnerSeat || context.OwnerSeat != candidate.OwnerSeat || context.SourceSeat != action.ActorSeat || !window.Action.TargetSeats.SequenceEqual(action.TargetSeats) || !window.Action.PhysicalCards.SequenceEqual(action.PhysicalCards) || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return null;
        var q = qualifications.SingleOrDefault(q => q.ActorSeat == candidate.OwnerSeat && q.Source.SkillId == candidate.SkillId && q.Source.SkillInstanceId == candidate.SkillInstanceId && q.GameplayHash == candidate.GameplayHash);
        var current = window.Candidates[window.CandidateIndex];
        var program = _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program;
        var trigger = program?.Triggers.SingleOrDefault(t => t.Id == candidate.BindingId);
        var equipment = context.Window == SkillProgramTriggerWindow.CardUseCommitted;
        var op = equipment ? SkillProgramEffectOp.DrawForRoundGainedEquipmentUse : SkillProgramEffectOp.AdjustOneRoundGainedOrdinaryTrickTarget;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == window.Id);
        if (q is null || current.OwnerSeat != candidate.OwnerSeat || current.SkillId != candidate.SkillId || current.TriggerId != candidate.BindingId || current.SkillInstanceId != candidate.SkillInstanceId || current.GameplayHash != candidate.GameplayHash || program?.GameplayHash != candidate.GameplayHash || trigger?.Window != context.Window || trigger.Effects is not [{ } effect] || effect.Op != op || _coreCardActionsOperations.CreateCardActionProgramContext2(window, current) != context || window.Continuation != (equipment ? ProgramCardContinuation.CommittedSimpleCard : ProgramCardContinuation.FinalizedTrick) || (equipment ? CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.CardKind) != SkillProgramCardCategory.Equipment : !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind)) || index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != use.Id || !ValidRoundGainedQualification(use, q) || (equipment ? !HasExactRoundGainedCommittedEquipmentUse(use, q) : !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action)))
            return null;
        return q;
    }

    internal bool HasExactRoundGainedCommittedEquipmentUse(CardUseFrame use, RoundGainedUseQualification q) => use.ProgramUseCommitted && use.Action is { Type: CardActionType.Use } action && action.ActorSeat == q.ActorSeat && action.ProviderSeat == q.ActorSeat && action.ResponderSeat is null && action.OpponentSeat is null && CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.CardKind) == SkillProgramCardCategory.Equipment && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == use.CardId && e.CardKind == use.CardKind && e.SourceSeat == q.ActorSeat) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetsConfirmedEvent>().Count(e => e.ResolutionId == use.Id && e.TargetSeats.SequenceEqual(action.TargetSeats)) == 1;
    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 60)]
    internal bool HasIssuedRoundGainedUseCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => ExactIssuedRoundGainedUseCandidate(candidate, context)is not null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 490)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 750)]
    internal bool CanContinueIssuedRoundGainedUse(ProgramSkillFrame frame) => frame.RoundGainedEquipmentDrawReceipt is not null && _coreRoundGainedEquipmentDrawsOperations.Value.ValidRoundGainedEquipmentDraw(frame) || frame.TriggerId is { } trigger && frame.WindowContext is { } context && ExactIssuedRoundGainedUseCandidate(new(frame.OwnerSeat, frame.SkillId, trigger, frame.SkillInstanceId, frame.GameplayHash, 0), context)is not null;
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 90)]
    internal bool DistancePhysicalSlash90HasIssuedRoundGainedBasicBonus(ProgramSlashDistanceQuery q) => q.ExistingUseFrameId is { } roundUseId && HasIssuedRoundGainedBasicBonus(roundUseId, q.Actor.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 110)]
    internal bool DistancePhysicalSlash110HasRoundGainedBasicBonus(ProgramSlashDistanceQuery q) => q.ExistingUseFrameId is null && HasRoundGainedBasicBonus(q.Actor, q.Kind, q.PhysicalCardIds ?? (q.PhysicalCard!.Id > 0 ? [q.PhysicalCard!.Id] : []));
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 50)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 30)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 60)]
    internal bool DistanceVirtualSlash50HasRoundGainedBasicBonus(ProgramSlashDistanceQuery q) => HasRoundGainedBasicBonus(q.Actor, q.Kind, q.PhysicalCardIds ?? []);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 70)]
    internal bool DistanceBorrowedSlash70HasPotentialRoundGainedBasicMaterials(ProgramSlashDistanceQuery q) => q.PhysicalCardIds is null && q.AllowAnyPhysicalSuit && HasPotentialRoundGainedBasicMaterials(q.Actor, q.Kind);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 40)]
    internal bool DistanceBorrowedPhysicalSlash40HasRoundGainedBasicBonus(ProgramSlashDistanceQuery q) => HasRoundGainedBasicBonus(q.Actor, q.Kind, [q.PhysicalCard!.Id]);
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 30)]
    internal bool DistanceBorrowedPaidWeaponSlash30HasRoundGainedBasicBonus(ProgramSlashDistanceQuery q) => HasRoundGainedBasicBonus(q.Actor, CardKind.Slash, [q.PhysicalCard!.Id]);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 20)]
    internal bool RoundGainedSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => HasRoundGainedBasicBonus(query.Actor, query.Kind, query.PhysicalCardIds ?? (query.PhysicalCard is { Id: > 0 } ? [query.PhysicalCard.Id] : []));
}
