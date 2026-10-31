#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYaDanOlRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27260)]
    internal void ExpireQingyaActualTurnAlcohol(IGameEvent payload)
    {
        if (!HasYaDanCapability || payload is not ActualTurnCursorReturnedEvent ended || ended.ActualTurn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber)
            return;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var uses = history.OfType<QingyaNativeIssuedEvent>().Where(e => e.Origin.ActualTurn == ended.ActualTurn && e.Origin.Kind == CardKind.Alcohol).Select(e => e.Origin.UseId).ToHashSet();
        foreach (var latest in history.OfType<AlcoholAppliedEvent>().GroupBy(e => e.SourceSeat).Select(g => g.Last()))
            if (uses.Contains(latest.ResolutionId) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(latest.SourceSeat) && _coreMatchParticipantsPlayersAccess._players[latest.SourceSeat].HasAlcoholEffect)
            {
                _coreMatchParticipantsPlayersAccess._players[latest.SourceSeat].HasAlcoholEffect = false;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlcoholExpiredEvent(latest.SourceSeat));
            }
    }

    internal CardUseFrame? QingyaQualifiedUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || context.OwnerSeat != c.OwnerSeat || context.CardUse is not { } cu || _coreCardUseLifecycleOperations.LifecycleCardUse(cu.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } a } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || a.ActorSeat != c.OwnerSeat || use.SourceSeat != c.OwnerSeat || a.ActionId != cu.CardActionId || use.TargetSeats is not [var target] || target == c.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive)
            return null;
        if (context.ParentFrameId != 0 && (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != context.Window || window.ParentFrameId != use.Id || window.Action.ActionId != a.ActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex]) != c))
            return null;
        return use;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27260)]
    internal bool QualifyQingya(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects is not [{ Op: SkillProgramEffectOp.ResolveQingyaBetweenHandDiscards }] || QingyaQualifiedUse(c, context)is not null;
    internal IReadOnlyList<QingyaArc> QingyaArcs(int owner, int target, IReadOnlyList<int> living)
    {
        // The supplied ring is the original frozen geometry, including its order.
        // Continuations must not substitute the table's later order or living roster.
        var ring = living.ToArray();
        var ownerIndex = Array.IndexOf(ring, owner);
        if (ownerIndex < 0 || !ring.Contains(target))
            throw new InvalidOperationException("Qingya requires both endpoints in its frozen living ring.");
        IReadOnlyList<int> Arc(int step)
        {
            var seats = new List<int>();
            var cursor = ownerIndex;
            for (var i = 0; i < ring.Length; i++)
            {
                cursor = (cursor + step + ring.Length) % ring.Length;
                var seat = ring[cursor];
                if (seat == target)
                    break;
                seats.Add(seat);
            }

            return Array.AsReadOnly(seats.ToArray());
        }

        var left = new QingyaArc(1, Arc(1));
        var right = new QingyaArc(-1, Arc(-1));
        return left.Seats.Count < right.Seats.Count ? [left] : right.Seats.Count < left.Seats.Count ? [right] : left.Seats.SequenceEqual(right.Seats) ? [left] : [left, right];
    }

    internal IReadOnlyList<QingyaHandMaterial> QingyaHandMaterials(int owner, int target)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return[];
        var from = CardLocation.Hand(target);
        return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select((card, slot) => (Card: card, Slot: slot)).Where(x => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, x.Card, from, OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, x.Card, from, OwnedCardMoveIntent.Discard)).Select(x => new QingyaHandMaterial(x.Card.Id, x.Card.Kind, target, x.Slot, _characterLuoXianOlOperations.CurrentHandEntry(target, x.Card.Id) ?? throw new InvalidOperationException("Qingya's original Hand entity lost its residency entry."))).ToArray());
    }

    internal IReadOnlyList<QingyaDiscardEntity> QingyaGrantedEntities(QingyaGrantIssuedEvent grant) => Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<QingyaHandDiscardedEvent>().Where(e => e.FrameId == grant.GrantId).OrderBy(e => e.Cursor).Select(e => new QingyaDiscardEntity(e.Material.CardId, e.Material.Kind, e.Material.HolderSeat, e.Sequence)).ToArray());
    internal bool QingyaEntityStillAvailable(QingyaDiscardEntity entity) => _cardZonesOperations._cardZones.GetLocation(entity.CardId) == CardLocation.DiscardPile && !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == entity.CardId && m.Sequence > entity.Sequence && m.From == CardLocation.DiscardPile);
    internal bool ValidQingyaGrant(QingyaGrantIssuedEvent g) => g.GrantId > 0 && g.Source.SkillId == "ol:qingya" && g.Source.BindingId == "discard-between" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(g.Source.OwnerSeat) && g.ActualTurn > 0 && g.EntityCount > 0 && g.HistoryCutoff >= 0 && _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program?.GameplayHash == g.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<YaDanStartedEvent>().Count(e => e.FrameId == g.GrantId && e.Source == g.Source && e.Op == SkillProgramEffectOp.ResolveQingyaBetweenHandDiscards && e.GameplayHash == g.GameplayHash && e.ActualTurn == g.ActualTurn && e.IssuedPhase == g.IssuedPhase && e.HistoryCutoff == g.HistoryCutoff) == 1 && QingyaGrantedEntities(g).Count == g.EntityCount && QingyaGrantedEntities(g).All(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == CardLocation.Hand(e.HolderSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == QingyaDiscardReason));
    internal bool QingyaGrantDue(QingyaGrantIssuedEvent grant, ActualDiscardRecoveryPhaseKey phase)
    {
        if (!ValidQingyaGrant(grant) || grant.ActualTurn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || phase.TurnNumber != grant.ActualTurn || phase.ActualTurnOwnerSeat != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || phase.Token == grant.IssuedPhase?.Token || _coreProgramEventHistoryOperations.ProgramEventHistory<ActualPhaseEndItemResolvedEvent>().Any(e => e.EntitlementId == grant.GrantId) || _coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == grant.ActualTurn))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var started = history.Skip(grant.HistoryCutoff).OfType<ActualDiscardRecoveryPhaseStartedEvent>().Select(e => e.Phase.Token).ToHashSet();
        if (!started.Contains(phase.Token))
            return false;
        var next = history.Skip(grant.HistoryCutoff).OfType<ActualPhaseEndingEvent>().Select(e => e.Phase).Where(p => started.Contains(p.Token) && p.TurnNumber == grant.ActualTurn && p.ActualTurnOwnerSeat == phase.ActualTurnOwnerSeat && p.Token != grant.IssuedPhase?.Token).Append(phase).First();
        return next == phase;
    }

    internal IReadOnlyList<ActualPhaseEndItem> CollectQingyaPhaseEndCandidates(ActualDiscardRecoveryPhaseKey phase)
    {
        if (!HasYaDanCapability)
            return[];
        return Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<QingyaGrantIssuedEvent>().Where(g => QingyaGrantDue(g, phase)).OrderBy(g => g.GrantId).Select((g, index) =>
        {
            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program!.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.ResolveQingyaNextActualPhaseUse }]);
            return new ActualPhaseEndItem(new(g.Source.OwnerSeat, g.Source.SkillId, trigger.Id, g.Source.SkillInstanceId, g.GameplayHash, trigger.Priority, index), g.GrantId);
        }).ToArray());
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 180)]
    internal bool IsIssuedQingyaCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (_coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is not [{ Op: SkillProgramEffectOp.ResolveQingyaNextActualPhaseUse }] || context.ActualPhaseEnd is not { } due || !_coreActualPhaseEndsOperations.IsExactActualPhaseEndCandidate(c, context) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive)
            return false;
        var grant = _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaGrantIssuedEvent>().SingleOrDefault(g => g.GrantId == due.EntitlementId);
        return grant is not null && grant.Source.OwnerSeat == c.OwnerSeat && grant.Source.SkillId == c.SkillId && grant.Source.SkillInstanceId == c.SkillInstanceId && grant.GameplayHash == c.GameplayHash && QingyaGrantDue(grant, due.Phase);
    }
}
